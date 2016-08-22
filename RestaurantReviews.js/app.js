var express    = require('express');
var bodyParser = require('body-parser');
var app        = express();
var router     = express.Router();

var port       = process.env.PORT || 8085;

//configure all our routes
var routes     = require('./routes')(router);

app.use(bodyParser.urlencoded({extended: true}));
app.use(bodyParser.json());

app.use('/api', router);
app.listen(port);

console.log('RestaurantReviews API now listening on port ' + port);