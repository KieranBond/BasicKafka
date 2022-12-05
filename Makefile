SHELL := /bin/bash

.SILENT:

.PHONY: kafka

kafka: 
	@docker-compose up