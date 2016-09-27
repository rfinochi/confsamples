FROM node:6-slim

# Create app directories
RUN mkdir -p /app
WORKDIR /app

# Set operational variables
ENV NODE_ENV production
EXPOSE 80
ENV PORT 80

# Install app dependencies
COPY package.json ./
RUN npm install

# Bundle app source
COPY systemjs.config.js ./
COPY dist/ dist/

CMD [ "npm", "start" ]
