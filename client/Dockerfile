FROM node:24-alpine AS build
WORKDIR /app

COPY client/package.json client/package-lock.json ./
RUN npm ci

COPY client .
RUN npm run build

FROM nginx:stable-alpine AS runtime
COPY client/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
