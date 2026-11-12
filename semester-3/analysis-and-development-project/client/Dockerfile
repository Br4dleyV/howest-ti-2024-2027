# Stage 1: Build Vue App
FROM node:22-alpine AS builder

WORKDIR /app

COPY /package*.json ./
COPY /src ./src
COPY /public ./public
COPY /index.html ./index.html
COPY /vite.config.js ./vite.config.js

RUN npm ci

ARG BUILD_ENV=dev

RUN npm run build:${BUILD_ENV}

# Stage 2: Serve with Nginx
FROM nginx:alpine

COPY --from=builder /app/dist /usr/share/nginx/html
COPY /nginx.conf /etc/nginx/conf.d/default.conf

RUN find /usr/share/nginx/html -type d -exec chmod 755 {} \; \
 && find /usr/share/nginx/html -type f -exec chmod 644 {} \;

RUN apk add --no-cache gettext

EXPOSE 80

ENTRYPOINT ["/bin/sh", "-c", "\
  set -e; \
  echo 'Generating runtime config...'; \
  envsubst < /usr/share/nginx/html/config.template.js > /usr/share/nginx/html/config.js; \
  rm /usr/share/nginx/html/config.template.js; \
  echo 'Starting Nginx...'; \
  exec nginx -g 'daemon off;' \
"]
