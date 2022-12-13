package service_test

import (
	"context"
	"errors"
	"fmt"
	"testing"

	"github.com/google/uuid"
	"github.com/sirupsen/logrus"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/mock"
	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/status"

	"accounts/mocks"
	"accounts/proto"
	"accounts/service"
)

var (
	errFoo      = errors.New("foo")
	errInternal = status.Error(codes.Internal, "internal error")
)

func TestAccountService_CreateWallet(t *testing.T) {
	log := logrus.NewEntry(logrus.New())
	mockPersist := mocks.Persist{}
	srv := service.CreateAccountService(log, &mockPersist)
	id := uuid.NewString()

	tests := []struct {
		name    string
		arg     *proto.CreateWalletRequest
		mock    func()
		want    *proto.Wallet
		wantErr error
	}{
		{
			name: "error on uuid parse",
			arg: &proto.CreateWalletRequest{
				CustomerId: "lorem",
			},
			mock:    func() {},
			wantErr: errInternal,
		},
		{
			name: "a.persist.GetAccount() failed",
			arg: &proto.CreateWalletRequest{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(nil, errFoo)
			},
			wantErr: errInternal,
		},
		{
			name: "account does not exists; error on a.persist.CreateAccount()",
			arg: &proto.CreateWalletRequest{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(nil, nil).
					On("CreateAccount", mock.Anything).Once().Return(errFoo)
			},
			wantErr: errInternal,
		},
		{
			name: "account does not exists; success",
			arg: &proto.CreateWalletRequest{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(nil, nil).
					On("CreateAccount", mock.Anything).Once().Return(nil)
			},
			want: &proto.Wallet{
				CustomerId: id,
			},
		},
		{
			name: "account exists; currencies do not match",
			arg: &proto.CreateWalletRequest{
				CustomerId: id,
				Currency:   proto.Currency_CURRENCY_BTC,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(&service.Account{Wallet: &proto.Wallet{Currency: proto.Currency_CURRENCY_ETH}}, nil)
			},
			wantErr: status.Error(codes.InvalidArgument,
				fmt.Sprintf("the currencies do not match, expected %s", proto.Currency_CURRENCY_ETH.String())),
		},
		{
			name: "account exists; error on a.persist.UpdateAccount()",
			arg: &proto.CreateWalletRequest{
				CustomerId: id,
				Currency:   proto.Currency_CURRENCY_BTC,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(&service.Account{Wallet: &proto.Wallet{Currency: proto.Currency_CURRENCY_BTC}}, nil).
					On("UpdateAccount", mock.Anything).Once().Return(errFoo)
			},
			wantErr: errInternal,
		},
		{
			name: "account exists; success",
			arg: &proto.CreateWalletRequest{
				CustomerId: id,
				Currency:   proto.Currency_CURRENCY_BTC,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(&service.Account{Wallet: &proto.Wallet{CustomerId: id, Currency: proto.Currency_CURRENCY_BTC}}, nil).
					On("UpdateAccount", mock.Anything).Once().Return(nil)
			},
			want: &proto.Wallet{
				CustomerId: id,
			},
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			tt.mock()

			got, gotErr := srv.CreateWallet(context.Background(), tt.arg)
			if tt.wantErr != nil || gotErr != nil {
				if !errors.Is(tt.wantErr, gotErr) {
					t.Errorf("gotErr: %s\nwantErr:%s", gotErr, tt.wantErr)
				}
				return
			}

			assert.Equal(t, tt.want.CustomerId, got.CustomerId)
		})
	}
}

func TestAccountService_CreateTransaction(t *testing.T) {
	log := logrus.NewEntry(logrus.New())
	mockPersist := mocks.Persist{}
	srv := service.CreateAccountService(log, &mockPersist)
	id := uuid.NewString()

	tests := []struct {
		name    string
		arg     *proto.Transaction
		mock    func()
		want    *proto.Transaction
		wantErr error
	}{
		{
			name: "error on uuid parse",
			arg: &proto.Transaction{
				CustomerId: "lorem",
			},
			mock:    func() {},
			wantErr: errInternal,
		},
		{
			name: "error on a.persist.GetAccount()",
			arg: &proto.Transaction{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything, mock.Anything).Once().Return(nil, errFoo)
			},
			wantErr: errInternal,
		},
		{
			name: "account does not exists; error on a.persist.CreateAccount",
			arg: &proto.Transaction{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything, mock.Anything).Once().Return(nil, nil).
					On("CreateAccount", mock.Anything).Once().Return(errFoo)
			},
			wantErr: errInternal,
		},
		{
			name: "account does not exists; success",
			arg: &proto.Transaction{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything, mock.Anything).Once().Return(nil, nil).
					On("CreateAccount", mock.Anything).Once().Return(nil)
			},
			want: &proto.Transaction{CustomerId: id},
		},
		{
			name: "account exists; currencies do not match",
			arg: &proto.Transaction{
				CustomerId: id,
				Currency:   proto.Currency_CURRENCY_BTC,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything, mock.Anything).Once().Return(&service.Account{
					Wallet: &proto.Wallet{Currency: proto.Currency_CURRENCY_USD},
					Ledger: make([]*proto.Transaction, 0),
				}, nil)
			},
			wantErr: status.Error(codes.InvalidArgument,
				fmt.Sprintf("the currencies do not match, expected %s", proto.Currency_CURRENCY_USD.String())),
		},
		{
			name: "account exists; error on a.persist.UpdateAccount()",
			arg: &proto.Transaction{
				CustomerId: id,
				Currency:   proto.Currency_CURRENCY_USD,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything, mock.Anything).Once().Return(&service.Account{
					Wallet: &proto.Wallet{Currency: proto.Currency_CURRENCY_USD},
					Ledger: make([]*proto.Transaction, 0),
				}, nil).
					On("UpdateAccount", mock.Anything).Once().Return(errFoo)
			},
			wantErr: errInternal,
		},
		{
			name: "account exists; success",
			arg: &proto.Transaction{
				CustomerId: id,
				Currency:   proto.Currency_CURRENCY_USD,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything, mock.Anything).Once().Return(&service.Account{
					Wallet: &proto.Wallet{Currency: proto.Currency_CURRENCY_USD},
					Ledger: make([]*proto.Transaction, 0),
				}, nil).
					On("UpdateAccount", mock.Anything).Once().Return(nil)
			},
			want: &proto.Transaction{CustomerId: id},
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			tt.mock()

			got, gotErr := srv.CreateTransaction(context.Background(), tt.arg)
			if tt.wantErr != nil || gotErr != nil {
				if !errors.Is(tt.wantErr, gotErr) {
					t.Errorf("gotErr: %s\nwantErr:%s", gotErr, tt.wantErr)
				}
				return
			}

			assert.Equal(t, tt.want.CustomerId, got.CustomerId)
		})
	}
}

func TestAccountService_GetTransactionHistory(t *testing.T) {
	log := logrus.NewEntry(logrus.New())
	mockPersist := mocks.Persist{}
	srv := service.CreateAccountService(log, &mockPersist)
	id := uuid.NewString()

	res := []*proto.Transaction{
		{
			TransactionId: uuid.NewString(),
		},
	}

	tests := []struct {
		name    string
		arg     *proto.GetLedgerRequest
		mock    func()
		want    *proto.GetLedgerResponse
		wantErr error
	}{
		{
			name: "error on uuid parse",
			arg: &proto.GetLedgerRequest{
				CustomerId: "lorem",
			},
			mock:    func() {},
			wantErr: errInternal,
		},
		{
			name: "error on a.persist.GetAccount",
			arg: &proto.GetLedgerRequest{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(nil, errFoo)
			},
			wantErr: errInternal,
		},
		{
			name: "account not found error",
			arg: &proto.GetLedgerRequest{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(nil, nil)
			},
			wantErr: status.Error(codes.NotFound, "the account was not found"),
		},
		{
			name: "account not found error",
			arg: &proto.GetLedgerRequest{
				CustomerId: id,
			},
			mock: func() {
				mockPersist.On("GetAccount", mock.Anything).Once().Return(&service.Account{Ledger: res}, nil)
			},
			want: &proto.GetLedgerResponse{
				Ledger: res,
			},
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			tt.mock()

			got, gotErr := srv.GetTransactionHistory(context.Background(), tt.arg)
			if tt.wantErr != nil || gotErr != nil {
				if !errors.Is(tt.wantErr, gotErr) {
					t.Errorf("gotErr: %s\nwantErr:%s", gotErr, tt.wantErr)
				}
				return
			}

			assert.Equal(t, tt.want, got)
		})
	}
}
