var indexController = function(){
	var welcome = function(req, res){
		//say hello and send folks to the API project... 
		res.json({
			message: "Welcome to the Restaurant Reviews API... Check out http://github.com/jobo5432/RestaurantReviews for more info..."
		});
	};

	return{
		welcome: welcome
	};
};

module.exports = indexController;