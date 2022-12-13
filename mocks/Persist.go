// Code generated by mockery v2.14.0. DO NOT EDIT.

package mocks

import (
	service "accounts/service"

	mock "github.com/stretchr/testify/mock"
)

// Persist is an autogenerated mock type for the Persist type
type Persist struct {
	mock.Mock
}

// CreateAccount provides a mock function with given fields: _a0
func (_m *Persist) CreateAccount(_a0 *service.Account) error {
	ret := _m.Called(_a0)

	var r0 error
	if rf, ok := ret.Get(0).(func(*service.Account) error); ok {
		r0 = rf(_a0)
	} else {
		r0 = ret.Error(0)
	}

	return r0
}

// GetAccount provides a mock function with given fields: _a0
func (_m *Persist) GetAccount(_a0 string) (*service.Account, error) {
	ret := _m.Called(_a0)

	var r0 *service.Account
	if rf, ok := ret.Get(0).(func(string) *service.Account); ok {
		r0 = rf(_a0)
	} else {
		if ret.Get(0) != nil {
			r0 = ret.Get(0).(*service.Account)
		}
	}

	var r1 error
	if rf, ok := ret.Get(1).(func(string) error); ok {
		r1 = rf(_a0)
	} else {
		r1 = ret.Error(1)
	}

	return r0, r1
}

// UpdateAccount provides a mock function with given fields: _a0
func (_m *Persist) UpdateAccount(_a0 *service.Account) error {
	ret := _m.Called(_a0)

	var r0 error
	if rf, ok := ret.Get(0).(func(*service.Account) error); ok {
		r0 = rf(_a0)
	} else {
		r0 = ret.Error(0)
	}

	return r0
}

type mockConstructorTestingTNewPersist interface {
	mock.TestingT
	Cleanup(func())
}

// NewPersist creates a new instance of Persist. It also registers a testing interface on the mock and a cleanup function to assert the mocks expectations.
func NewPersist(t mockConstructorTestingTNewPersist) *Persist {
	mock := &Persist{}
	mock.Mock.Test(t)

	t.Cleanup(func() { mock.AssertExpectations(t) })

	return mock
}