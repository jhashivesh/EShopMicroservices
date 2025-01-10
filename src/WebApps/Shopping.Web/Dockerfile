FROM node:22.12-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app

COPY package*.json ./
RUN npm ci

FROM node:22.12-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .

RUN npm run build --omit=dev

# production image, copy all files and run next
# Define nginx for front-end server

FROM nginx:1.15.8-alpine AS runner
# Copy dist from ng build to nginx html folder
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/shopping.web /usr/share/nginx/html

#Copy default nginx configuration
COPY ./nginx-custom.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

#CMD ["nginx", "-g", "daemon off;"]
# When the container starts, replace the env.js with values from environment variables
CMD ["/bin/sh",  "-c",  "envsubst < /usr/share/nginx/html/assets/env.template.js > /usr/share/nginx/html/assets/env.js && exec nginx -g 'daemon off;'"]

## cd C:\sk\sharedwithvms\repos\EShopMicroservices\src\WebApps\Shopping.Web
## docker build -t shiveshk/webapp -f ./Dockerfile .
## docker run -dp 4200:80 webapp

## docker compose  -f "C:\sk\sharedwithvms\repos\EShopMicroservices\src\docker-compose.yml" -f "C:\sk\sharedwithvms\repos\EShopMicroservices\src\docker-compose.override.yml" up -d
