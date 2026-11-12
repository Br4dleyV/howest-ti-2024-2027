# SkyNet
# Commands to run the application:
```bash
# To run from Howest Docker registry
docker run -d -p 80:80 hcr.technet.howest.be/adria-client-group-04:latest  

# To run from local image
## Build image
docker build --build-arg BUILD_ENV=test -t hcr.technet.howest.be/adria-client-group-04:latest .

## Push to Howest Docker registry
docker push hcr.technet.howest.be/adria-client-group-00:latest

## Run container
docker run -d -p 80:80 hcr.technet.howest.be/adria-client-group-04:latest  
```

# Proof of Concept - Client

## Requirements
- Follow the naming conventions for files, variables, functions, ...
- The application should have least 3 reuasable components.
- Sonar quality gates should be green.

## Prerequisites
- Docker desktop is installed on your machine.
- Testing & quality checks locally

## Production
Docker image gets created and pushed to the Howest Docker Registry. Then you run the command to create the Docker container locally, and go to http://localhost to enter the client.

## Customizable requirements
You need to complete 5 points worth of topics including at least 1 topic with a weight of 2 points.

Any deviations need to be consolidated with the group mentors.
| Title | Description | Points |
| --- | --- | --- |
| ✅~~History API~~ | ~~Support for navigation through the browser history.~~ -> vue-router createWebHistory & router-links | 1 | 
| 🟧Push notifications | Support for notifications sent by the server. | 2 | 
| ✅~~Maps (Leaflet)~~ | ~~Interactive maps via OpenStreetMap which at the very least use the geolocation feature (which for Adria can obviously be spoofed). Add markers, routes, area descriptions—whatever fits your needs.~~ | 2 | 
| 🟧Sensitive information (server) | Implement secure handling of sensitive information, such as API keys, passwords, or personal data. Take a look at dotnet user secrets and how to pass env files to a Docker service. The end goal is to have no sensitive information in the repositories. | 2 | 