FROM node:18-alpine AS build-step

RUN mkdir -p /app

WORKDIR /app

COPY package.json ./app

RUN npm install

COPY .  /app

RUN npm run build --prod

FROM nginx:alpine

COPY --from=build-step /app/dist/app-security /usr/share/nginx/html

EXPOSE 4200

CMD ["nginx", "-g", "daemon off;"]
