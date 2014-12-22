var express = require('express')
  , router = express.Router()
  , Models = require('../models')
  , api = require('./api');

// Handles GET requests to the route /reviews/[id], where id is the review ID.
router.get('/:id(\\d+)', function(req, res) {
  Models.Review.find(req.params.id).then(function(review) {
    if (review != null) {
      api.sendOK(res, review);
    } else {
      api.sendNotFound(res);
    }
  });
});

// Delete a review given, the review ID
router.delete('/:id(\\d+)', function(req, res) {
  Models.Review.find(req.params.id).then(function(review) {
    if (review != null) {
      review.destroy();
      api.sendNoContent(res);
    } else {
      api.sendNotFound(res);
    }
  });
});

module.exports = router;
