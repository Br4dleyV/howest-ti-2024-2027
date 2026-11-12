# Skynet - Test Environment
This repository will contain the code to run the test environment for the Skynet project.

## Goal
This repository is used to run the complete Skynet solution on your local machine independently of any IDE or source code. It's meant to simulate a production environment as closely as possible. It needs to be used to do final testing of the application before final delivery.

```bash
docker compose -f docker-compose.yml up -d
```

Client: http://localhost:80
Server: http://localhost:8000/api

## Requirements
- Create a working docker-compose file that runs the complete Adria solution
    - ✅Server on port 8000
    - ✅Client on port 80
- Must contain at least:
    - ✅A database container (MySQL)
    - ✅A server container
    - ✅A client container
    - ✅A network to connect the containers
    - ✅A volume to persist the database data
