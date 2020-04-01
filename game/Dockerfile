# Build project 
# 
FROM node as build
COPY . /app
WORKDIR /app

RUN npm i
RUN npm run build

# Setup nginx
# 
FROM nginx
EXPOSE 8080
COPY --from=build /app/dist/ /usr/share/nginx/html
COPY --from=build /app/default.conf /etc/nginx/conf.d/default.conf