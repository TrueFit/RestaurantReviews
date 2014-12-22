var express = require('express')
  , router = express.Router()
  , Models = require('../models')
  , api = require('./api');

// Handle requests for all of the users
router.get('', function(req, res) {
  Models.User.findAll().then(function(users) {
    api.sendResultOrError(res, users, 'No users found');
  });
})

// Handle GET requests asking for one users details
router.get('/:id(\\d+)', function(req, res) {
  Models.User.find(req.params.id).then(function(user) {
    if (user != null) {
      api.sendOK(res, user);
    } else {
      api.sendNotFound(res, 'No user found with id ' + req.params.id);
    }
  });
});

// Handle GET requests asking for all the reviews for a given user
router.get('/:id(\\d+)/reviews', function(req, res) {
  Models.User.find({ include: [ Models.Review ] }, req.params.id).then(function(user) {
    if (user != null) {
      api.sendOK(res, user);
    } else {
      api.sendNotFound(res, 'No user found with id ' + req.params.id);
    }
  });
});

// Handle GET requests for a user, looking up by username instead of id
router.get('/:name', function(req, res) {
  Models.User.find({ where: { username: req.params.name }}).then(function(user) {
    if (user != null) {
      api.sendOK(res, user);
    } else {
      api.sendNotFound(res, 'No user found with name ' + req.params.id);
    }
  });
});

// Handle GET requests for a users' reviews, looking up by username instead of id
router.get('/:name/reviews', function(req, res) {
  Models.User.find({ where: { username: req.params.name }, include: [ Models.Review ]}).then(function(user) {
    if (user != null) {
      api.sendOK(res, user);
    } else {
      api.sendNotFound(res, 'No user found with name ' + req.params.id);
    }
  });
});

module.exports = router;
