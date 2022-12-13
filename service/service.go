package service

import (
	"context"
	"fmt"
	"time"

	"github.com/google/uuid"
	"github.com/sirupsen/logrus"
	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/status"
	"google.golang.org/protobuf/types/known/timestamppb"

	"accounts/proto"
)

var errInternal = status.Error(codes.Internal, "internal error")

type account struct {
	persist Persist
	log     *logrus.Entry

	proto.UnimplementedAccountServiceServer
}

// CreateAccountService creates the account service handler.
func CreateAccountService(log *logrus.Entry, persist Persist) *account {
	return &account{
		persist: persist,
		log:     log,
	}
}

func (a *account) CreateWallet(_ context.Context, req *proto.CreateWalletRequest) (*proto.Wallet, error) {
	// customer_id must be a valid uuid, assumed present in another system
	if _, err := uuid.Parse(req.CustomerId); err != nil {
		a.log.WithError(err).Error("uuid.Parse() failed")
		return nil, errInternal
	}

	// retrieve account from db if there exists one
	account, err := a.persist.GetAccount(req.CustomerId)
	if err != nil {
		a.log.WithError(err).Error("a.persist.GetAccount() failed")
		return nil, errInternal
	}

	switch account == nil {
	case true:
		// account does not exists, create one
		account = &Account{
			CustomerID: req.CustomerId,
			Wallet: &proto.Wallet{
				CustomerId: req.CustomerId,
				Currency:   req.Currency,
			},
		}

		if err = a.persist.CreateAccount(account); err != nil {
			a.log.WithError(err).Error("a.persist.CreateAccount() failed")
			return nil, errInternal
		}

	default:
		// ensure the currencies match
		if account.Wallet.Currency != req.Currency {
			return nil, status.Error(codes.InvalidArgument,
				fmt.Sprintf("the currencies do not match, expected %s", proto.Currency_name[int32(account.Wallet.Currency)]))
		}

		if err = a.persist.UpdateAccount(account); err != nil {
			a.log.WithError(err).Error("a.persist.UpdateAccount() failed")
			return nil, errInternal
		}

	}

	return account.Wallet, nil
}

func (a *account) CreateTransaction(_ context.Context, req *proto.Transaction) (*proto.Transaction, error) {
	// customer_id must be a valid uuid, assumed present in another system
	if _, err := uuid.Parse(req.CustomerId); err != nil {
		a.log.WithError(err).Error("uuid.Parse() failed")
		return nil, errInternal
	}

	// retrive account
	account, err := a.persist.GetAccount(req.CustomerId)
	if err != nil {
		a.log.WithError(err).Error("a.persist.GetAccount() failed")
		return nil, errInternal
	}

	t := timestamppb.New(time.Now())
	req.CreatedAt = t
	req.TransactionId = uuid.NewString()

	switch account == nil {
	case true:
		// start new wallet at 0
		account = &Account{
			Wallet: &proto.Wallet{
				CustomerId: req.CustomerId,
				Currency:   req.Currency,
				CreatedAt:  t,
				UpdatedAt:  t,
				Balance:    req.Amount, // 0 + (signed transaction amt.)
			},
			Ledger: []*proto.Transaction{req},
		}

		if err = a.persist.CreateAccount(account); err != nil {
			a.log.WithError(err).Error("a.persist.CreateAccount() failed")
			return nil, errInternal
		}

	case false:
		// ensure the currencies match
		if account.Wallet.Currency != req.Currency {
			return nil, status.Error(codes.InvalidArgument,
				fmt.Sprintf("the currencies do not match, expected %s", proto.Currency_name[int32(account.Wallet.Currency)]))
		}

		// The amount is +ve on debit and -ve on credit; mutating the wallet balance
		account.Wallet.Balance += req.Amount

		// Transaction is appended to the ledger
		account.Ledger = append(account.Ledger, req)

		if err = a.persist.UpdateAccount(account); err != nil {
			a.log.WithError(err).Error("a.persist.UpdateAccount() failed")
			return nil, errInternal
		}
	}

	return req, nil
}

func (a *account) GetTransactionHistory(_ context.Context, req *proto.GetLedgerRequest) (*proto.GetLedgerResponse, error) {
	// customer_id must be a valid uuid, assumed present in another system
	if _, err := uuid.Parse(req.CustomerId); err != nil {
		a.log.WithError(err).Error("uuid.Parse() failed")
		return nil, errInternal
	}

	// retrive account
	account, err := a.persist.GetAccount(req.CustomerId)
	if err != nil {
		a.log.WithError(err).Error("a.persist.GetAccount() failed")
		return nil, errInternal
	}

	if account == nil {
		return nil, status.Error(codes.NotFound, "the account was not found")
	}

	return &proto.GetLedgerResponse{
		Ledger: paginateList(account.Ledger, int(req.Limit), int(req.Offset)),
	}, nil
}

func paginateList(list []*proto.Transaction, limit, offset int) []*proto.Transaction {
	// default limit is 10
	if limit == 0 {
		limit = 10
	}

	if offset > len(list) {
		offset = len(list)
	}

	end := offset + limit
	if end > len(list) {
		end = len(list)
	}

	return list[offset:end]
}
