# Requirements
1. Design a data structure for storing customer ledgers, balance details etc.
2. By making use of the created data structure, create a gRPC service to support below features
    - Create user wallet – assuming user profile is created already in other systems and a unique user id (uuid) is created
    - Record keeping for user's transaction (credit to or debit from user's wallet) in ledgers
    - Retrieve user wallet summary
    - Get user transaction history (pagination required)
3. The service should also support
    - Multiple currency (e.g ETC, BTC, USD etc.)
    - Multiple users
4. The service should have proper unit testing coverage
5. Proper logging in the service
6. Documentation
7. Anything you think it can make the service better

* Please try not to over complicate the task.
* You can use any third-party framework or libraries

# Building protogen
```bash
 protoc -I=./proto --go_out=. ./proto/contract.proto --go-grpc_out=.
```
# Generating Persist mock data

Using mockery to generate mock data, how to install [here](https://github.com/vektra/mockery#go-install)
```bash
mockery --name Persist --dir "./service" 
```

# Running tests with coverage
```bash
$ go test ./... --coverprofile=coverage.out
$ go tool cover -html=coverage.out
```

# Storage
The storage used is a JSON file based system. Library used is [here](https://pkg.go.dev/github.com/sonyarouje/simdb@v0.1.0).