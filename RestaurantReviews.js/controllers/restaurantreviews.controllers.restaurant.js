var restaurantController = function(){
	var Restaurant    = require('../models/restaurantreviews.models.restaurant');

	var getByCity = function(req, res){
		Restaurant.find({city: {$regex: new RegExp(req.params.city, 'i')}}, function(err, results){
			if(err){
				res.json(err);
			}
			else{
				res.json(results);
			}
		});
	};

	var addRestaurant = function(req, res){
		//add a new restaurant
		var newRestaurant = new Restaurant(req.body.restaurant);
		newRestaurant.createdDateTime = Date.now();
		newRestaurant.save(function(err, newRestaurant){
			if(err){
				res.json(err);
			}
			else{
				console.log("Added restaurant");
				console.log(newRestaurant);
				res.json(newRestaurant);
			}
		});
	};

	return{
		getByCity		: getByCity,
		addRestaurant	: addRestaurant
	};
};

module.exports = restaurantController;