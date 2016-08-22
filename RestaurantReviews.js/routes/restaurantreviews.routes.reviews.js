var controller  = require('../controllers/restaurantreviews.controllers.review')();
var routePrefix = '/reviews';

module.exports = function(router){
	router.get(routePrefix + '/user/:userID', controller.getByUser);
	router.get(routePrefix +'/restaurant/:restaurantID', controller.getByRestaurant);
	router.post(routePrefix + '/add', controller.addReview);
	router.delete(routePrefix + '/remove/:ID', controller.deleteReview);

	return{
		router: router
	};
};