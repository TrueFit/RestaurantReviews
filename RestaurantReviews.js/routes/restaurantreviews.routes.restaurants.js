var controller  = require('../controllers/restaurantreviews.controllers.restaurant')();
var routePrefix = '/restaurants';

module.exports = function(router){
	router.get(routePrefix + '/city/:city', controller.getByCity);
	router.post(routePrefix +'/add', controller.addRestaurant);

	return{
		router: router
	};
};