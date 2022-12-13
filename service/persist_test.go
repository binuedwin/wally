package service_test

import (
	"testing"

	"github.com/google/uuid"
	"github.com/sonyarouje/simdb"
	"github.com/stretchr/testify/assert"

	"accounts/proto"
	"accounts/service"
)

const testDBDir = "../mocks"

var driver, _ = simdb.New(testDBDir)

func TestPersist_CreateAccount(t *testing.T) {
	id := uuid.NewString()
	account := &service.Account{
		CustomerID: id,
		Wallet: &proto.Wallet{
			CustomerId: id,
		},
		Ledger: []*proto.Transaction{
			{
				TransactionId: uuid.NewString(),
			},
		},
	}

	dv, err := service.NewPersist(testDBDir)
	assert.Nil(t, err)

	// create account
	assert.Nil(t, dv.CreateAccount(account))

	// retrieve and compare accounts
	retAccount, err := getAccount(account)
	assert.Nil(t, err)

	// compare transaction strings for now; whole equality not
	// possible unless with timestamp trimming
	assert.Equal(t, account.Ledger[0].TransactionId, retAccount.Ledger[0].TransactionId)
}

func TestPersist_GetAccount(t *testing.T) {
	id := uuid.NewString()
	account := &service.Account{
		CustomerID: id,
		Wallet: &proto.Wallet{
			CustomerId: id,
		},
		Ledger: []*proto.Transaction{
			{
				TransactionId: uuid.NewString(),
			},
		},
	}

	dv, err := service.NewPersist(testDBDir)
	assert.Nil(t, err)

	// create account
	assert.Nil(t, dv.CreateAccount(account))

	// get created account
	retAccount, err := dv.GetAccount(id)
	assert.Nil(t, err)

	// compare transaction strings for now; whole equality not
	// possible unless with timestamp trimming
	assert.Equal(t, account.Ledger[0].TransactionId, retAccount.Ledger[0].TransactionId)

	// check for not found error
	retAccount, err = dv.GetAccount(uuid.NewString())
	assert.Nil(t, err)
	assert.Nil(t, retAccount)
}

func TestPersist_UpdateAccount(t *testing.T) {
	id := uuid.NewString()
	account := &service.Account{
		CustomerID: id,
		Wallet: &proto.Wallet{
			CustomerId: id,
		},
		Ledger: []*proto.Transaction{
			{
				TransactionId: uuid.NewString(),
			},
		},
	}

	dv, err := service.NewPersist(testDBDir)
	assert.Nil(t, err)

	// create account
	assert.Nil(t, dv.CreateAccount(account))

	// update transaction amount
	account.Ledger[0].Amount = 10_000
	assert.Nil(t, dv.UpdateAccount(account))

	retAccount, err := getAccount(account)
	assert.Nil(t, err)

	// make sure transaction amount is 10_000
	assert.True(t, retAccount.Ledger[0].Amount == 10_000)
}

func getAccount(account *service.Account) (*service.Account, error) {
	var fetchedCustomer service.Account
	err := driver.Open(&service.Account{}).Where("id", "=", account.CustomerID).First().AsEntity(&fetchedCustomer)
	return &fetchedCustomer, err
}
