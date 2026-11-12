# Skynet
Deployment naar Vercel (client) en Render (server): zie [DEPLOYMENT.md](DEPLOYMENT.md).

Here's how to run the Skynet solution on your local machine.

```bash
cd test-environment
docker compose -f docker-compose.yml up -d
```

Client: http://localhost:80
Server: http://localhost:8000/api