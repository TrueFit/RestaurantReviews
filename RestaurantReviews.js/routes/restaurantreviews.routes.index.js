var controller = require('../controllers/restaurantreviews.controllers.index')();

module.exports = function(router){
	router.get('/welcome', controller.welcome);	

	return{
		router: router
	};
};