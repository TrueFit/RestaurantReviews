var express = require('express')
  , bodyParser = require('body-parser')
  , methodOverride = require('method-override')
  , Sequelize = require('sequelize');

// Create the express app
var app = new express();
app.use(bodyParser.json()); // Parse the request body - we need this make JSON payloads work correctly
app.use(methodOverride());  // Enable DELETE requests

// Simple middleware to log incoming request URLs to the console
app.use(function(req, res, next) {
  console.log('Request ' + req.originalUrl);
  next();
});

// Load all of our main resources
app.use('/users', require('./resources/users'));
app.use('/restaurants', require('./resources/restaurants'));
app.use('/reviews', require('./resources/reviews'));

// Start listening on port 3000
console.log('API server ready on port 3000...');
app.listen(3000);
