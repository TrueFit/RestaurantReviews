var fs             = require('fs');
var normalizedPath = require('path').join(__dirname, 'models');
var mongoose       = require('mongoose');

module.exports = function(db){
	fs.readdirSync(normalizedPath).forEach(function(file){
		require('./routes/' + file)(db);
	});
};