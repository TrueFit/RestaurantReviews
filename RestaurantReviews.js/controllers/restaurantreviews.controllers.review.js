var reviewController = function(){
	var ObjectId = require('mongodb').ObjectID;
	var Review   = require('../models/restaurantreviews.models.review');

	var getByUser = function(req, res){
		//get reviews by user
		var userId = req.params.userID;
		Review.find({userId: ObjectId(userId)}, function(err, results){
			if(err){
				res.json(err);
			}
			else{
				res.json(results);
			}
		});
	};

	var getByRestaurant = function(req, res){
		//delete a review
		var restaurantId = req.params.restaurantID;
		Review.find({restaurantId: ObjectId(restaurantId)}, function(err, results){
			if(err){
				res.json(err);
			}
			else{
				res.json(results);
			}
		});
	};

	var addReview = function(req, res){
		//add the review
		var newReview = new Review(req.body.review);
		
		newReview.userId = ObjectId(newReview.userId);
		newReview.restaurantId = ObjectId(newReview.restaurantId);
		newReview.createdDateTime = Date.now();

		newReview.save(function(err, newReview){
			if(err){
				res.json(err);
			}
			else{
				res.json(newReview);
			}
		});
	};

	var deleteReview = function(req, res){
		//delete a review
		var oldReviewId = req.params.ID;
		console.log(oldReviewId);
		Review.findOne({_id: ObjectId(oldReviewId)}, function(err, review){
			console.log(review);
			if(err){
				res.json(err);
			}
			else{
				review.remove(function(e, r){
					if(e){
						res.json(e);
					}
					else{
						res.json(r);
					}
				});
			}
		});
	};

	return{
		addReview: addReview,
		deleteReview: deleteReview,
		getByUser: getByUser,
		getByRestaurant: getByRestaurant
	};
};

module.exports = reviewController;