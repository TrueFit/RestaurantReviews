var mongoose = require('mongoose');
var db       = mongoose.createConnection('mongodb://truefit:truefit@localhost/restaurantreviews');
var Schema   = mongoose.Schema;

var restaurantModel = new Schema({
	name: String,
	address1: String,
	address2: String,
	city: String,
	state: String,
	zip: String,
	phone: String,
	url: String,
	createdDateTime: Date
});

module.exports = db.model('Restaurant', restaurantModel);