package service

import (
	"time"

	"github.com/sonyarouje/simdb"
	"google.golang.org/protobuf/types/known/timestamppb"

	"accounts/proto"
)

// Account holds all account information for each customer.
type Account struct {
	CustomerID string `json:"id"`
	Wallet     *proto.Wallet
	Ledger     []*proto.Transaction
}

// Account must implement the ID hook to allow to identify the uinique field
func (a *Account) ID() (string, interface{}) {
	return "id", a.CustomerID
}

// Persist provides all the methods
type Persist interface {
	CreateAccount(*Account) error
	// GetAccount, if account was not found both return types are nil otherwise error is returned to caller
	// for logging.
	GetAccount(string) (*Account, error)
	// UpdateAccount requires that the CustomerID be set and fields to update are mutated after fetch from GetAccount.
	UpdateAccount(*Account) error
}

type persist struct {
	driver *simdb.Driver
}

// NewPersist creates the persistance layer of the service.
// Persistance if json based and lives only for the duration of an instance of the program
// when main() returns the data is cleared.
func NewPersist(dir string) (Persist, error) {
	// initialize storage
	driver, err := simdb.New(dir)
	if err != nil {
		return nil, err
	}

	return &persist{
		driver: driver,
	}, nil
}

func (p *persist) CreateAccount(account *Account) error {
	now := timestamppb.New(time.Now())
	account.Wallet.CreatedAt = now
	account.Wallet.UpdatedAt = now
	return p.driver.Open(&Account{}).Insert(account)
}

func (p *persist) GetAccount(id string) (*Account, error) {
	var acc Account
	if err := p.driver.Open(&Account{}).Where("id", "=", id).First().AsEntity(&acc); err != nil {
		if err == simdb.ErrRecordNotFound {
			return nil, nil
		}
		return nil, err
	}

	return &acc, nil
}

func (p *persist) UpdateAccount(account *Account) error {
	account.Wallet.UpdatedAt = timestamppb.New(time.Now())
	if err := p.driver.Where("id", "=", account.CustomerID).First().Update(account); err != nil {
		return err
	}

	return nil
}
