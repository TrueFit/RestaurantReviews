var mongoose = require('mongoose');
var db       = mongoose.createConnection('mongodb://truefit:truefit@localhost/restaurantreviews');
var Schema   = mongoose.Schema;

var reviewModel = new Schema({
	userId: Schema.Types.ObjectId,
	restaurantId: Schema.Types.ObjectId,
	isPositive: Boolean,
	comment: String,
	createdDateTime: Date
});

module.exports = db.model('Review', reviewModel);