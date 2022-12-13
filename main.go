package main

import (
	"net"

	"github.com/sirupsen/logrus"
	"google.golang.org/grpc"

	"accounts/proto"
	"accounts/service"
)

const JSONStore = "accounts_db"

func main() {
	log := logrus.NewEntry(logrus.New()) // logs to stderr

	listener, err := net.Listen("tcp", ":50001")
	if err != nil {
		log.WithError(err).Error("net.Listen() failed")
		return
	}

	// connect to db
	p, err := service.NewPersist(JSONStore)
	if err != nil {
		log.WithError(err).Error("service.NewPersist() failed")
		return
	}

	accSrv := service.CreateAccountService(log, p)
	srv := grpc.NewServer()
	proto.RegisterAccountServiceServer(srv, accSrv)

	if err = srv.Serve(listener); err != nil {
		log.WithError(err).Error("srv.Serve() failed")
		return
	}
}
