# Deployment

## Server op Render

Gebruik de bestaande Dockerfile:

| Instelling | Waarde |
| --- | --- |
| Root Directory | `server` |
| Runtime | `Docker` |
| Dockerfile Path | `config/docker/Dockerfile` |
| Docker Build Context Directory | `.` |
| Health Check Path | `/health` |

Stel deze environment variables in:

```text
ASPNETCORE_ENVIRONMENT=Production
PORT=8000
Persistence__ConnectionString=Server=HOST;Port=3306;Database=skynet;User=USER;Password=PASSWORD;SslMode=Required;
```

Voorzie een bereikbare MySQL 8-database en vul de connection string in met de
gegevens en TLS-instellingen van je databaseprovider. De server luistert al op
`0.0.0.0:8000`. De bestaande CORS-configuratie laat de client toe.

Het bestaande databasegedrag blijft behouden: bij elke serverstart worden de
tabellen opnieuw aangemaakt en de demogegevens opnieuw ingevuld.

## Client op Vercel

| Instelling | Waarde |
| --- | --- |
| Root Directory | `client` |
| Framework Preset | `Vite` |
| Build Command | `npm run build` |
| Output Directory | `dist` |

Stel `VITE_API_URL=https://JOUW-SERVER.onrender.com/api` in als environment
variable, zonder afsluitende `/`, en deploy. Deploy opnieuw als je deze waarde
wijzigt. `vercel.json` zorgt dat rechtstreeks openen en verversen van routes werkt.

Voor pushmeldingen: zet `VITE_VAPID_PUBLIC_KEY` op Vercel en het bijbehorende
`Vapid__PublicKey`, `Vapid__PrivateKey` en `Vapid__Subject` op Render.
