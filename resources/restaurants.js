var express = require('express')
  , router = express.Router()
  , Models = require('../models')
  , api = require('./api')
  , helpers = require('./helpers');

// Handles requests to the base route. Checks for query parameters, and if
// there is one matching "city", it filters the results based on city, otherwise
// it returns all restaurants in the database.
router.get('/', function(req, res) {
  if (req.query.city != null) {
    var city = helpers.titleize(req.query.city);
    Models.Restaurant.findAll({ where: { city: city }}).then(function(restaurants) {
      api.sendResultOrError(res, restaurants, 'No restaurants found in ' + city);
    });
  } else {
    Models.Restaurant.findAll().then(function(restaurants) {
      api.sendResultOrError(res, restaurants, 'No restaurants found');
    });
  }
});

// Handles POST requests to the base route, which can be used to create new
// restaurants. See API documentation for correct formatting of the
// request body. The request content-type should be 'appliation/json'
router.post('/', function(req, res) {
  if (req.body != null) {
    Models.Restaurant.create(req.body).then(function(restaurant) {
      api.sendCreated(res, restaurant);
    });
  } else {
    api.sendBadRequest(res);
  }
});

// Handles GET requests for the restaurants matching slug.  For example, the
// restaurant "The French Laundry" can be found via /restaurants/the-french-laundry
router.get('/:slug', function(req, res) {
  Models.Restaurant.find({ where: { slug: req.params.slug }}).then(function(restaurant) {
    if (restaurant != null) {
      api.sendOK(res, restaurant);
    } else {
      api.sendNotFound(res, 'No restaurants found matching that name');
    }
  });
});

// Gets all reviews for a given restaurant, matched by slug
router.get('/:slug/reviews', function(req, res) {
  Models.Restaurant.find({ where: { slug: req.params.slug }, include: [Models.Review] }).then(function(restaurant) {
    if (restaurant != null) {
      api.sendOK(res, restaurant);
    } else {
      api.sendNotFound(res, 'No restaurants found matching that name');
    }
  });
});

// Creates a new review for the restaurant specified by slug. The request body
// should contain the user_id that is creating the review
router.post('/:slug/reviews', function(req, res) {
  if (req.body != null) {
    Models.User.find(req.body.user_id).then(function(user) {
      Models.Restaurant.find({ where: { slug: req.params.slug }}).then(function(restaurant) {
        Models.Review.create({ content: req.body.review.content, stars: req.body.review.num_stars }).then(function(review) {
          review.setRestaurant(restaurant);
          review.setUser(user);
          api.sendCreated(res, review);
        });
      });
    })
  } else {
    api.sendBadRequest(res);
  }
});

module.exports = router;
