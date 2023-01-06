SHELL := /bin/bash

.SILENT:

.PHONY: kafka

kafka: 
	@docker-compose up
	
kafka-build: 
	@docker-compose up --force-recreate