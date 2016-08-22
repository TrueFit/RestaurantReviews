//This will import all the routes from the /routes folder. This way we don't have to mess with app
//logic to add a new route. Just add the file to the folder and it's all good to go (on app reload)

var fs             = require('fs');
var normalizedPath = require('path').join(__dirname, 'routes');

module.exports = function(router){
	fs.readdirSync(normalizedPath).forEach(function(file){
		require('./routes/' + file)(router);
	});
};
