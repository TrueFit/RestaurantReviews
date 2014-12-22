var Models = require('../models');

function createReview(user, restaurant, text, stars) {
  Models.Review.create({ content: text, num_stars: stars }).then(function(review) {
    review.setRestaurant(restaurant);
    review.setUser(user);
  });
}

// Drop all of the tables and recreate them
Models.Review.sync({ force: true })
.then(
  Models.Restaurant.sync({ force: true }).then(function() {
    Models.Restaurant.create({ name: 'Cure', city: 'Pittsburgh', state: 'PA', slug: 'cure' });
    Models.Restaurant.create({ name: 'BRGR', city: 'Cranberry', state: 'PA', slug: 'brgr' });
    Models.Restaurant.create({ name: 'Butcher and the Rye', city: 'Pittsburgh', state: 'PA', slug: 'butcher-and-the-rye' });
    Models.Restaurant.create({ name: 'The French Laundry', city: 'Yountville', state: 'CA', slug: 'the-french-laundry' });
  }
)
.then(
  Models.User.sync({ force: true }).then(function() {
    Models.User.create({ email: 'baileyb@gmail.com', first_name: 'Brian', last_name: 'Bailey', username: 'baileyb' })
    .then(function(user) {
      Models.Restaurant.find({ where: { name: 'Cure' }}).then(function(restaurant) {
        createReview(user, restaurant, "Cure is one of the best restaurants in the city. Highly recommended", 5);
      });
      Models.Restaurant.find({ where: { name: 'Butcher and the Rye' }}).then(function(restaurant) {
        createReview(user, restaurant, "Went there on a Saturday night. The wait was too long and the service was just OK.", 3);
      });
    })
    Models.User.create({ email: 'ntesla@gmail.com', first_name: 'Nicola', last_name: 'Tesla', username: 'ntesla' })
    .then(function(user) {
      Models.Restaurant.find({ where: { name: 'BRGR' }}).then(function(restaurant) {
        createReview(user, restaurant, "It was alright. Will go back, but hopefully the service is better next time.", 3);
      });
      Models.Restaurant.find({ where: { name: 'Butcher and the Rye' }}).then(function(restaurant) {
        createReview(user, restaurant, "Awesome. Every Friday, I'm there.", 5);
      });
    })
    Models.User.create({ email: 'aeinstein@gmail.com', first_name: 'Albert', last_name: 'Einstein', username: 'aeinstein' })
    .then(function(user) {
      Models.Restaurant.find({ where: { name: 'Cure' }}).then(function(restaurant) {
        createReview(user, restaurant, "Cure was terrible.  Never again", 1);
      });
      Models.Restaurant.find({ where: { name: 'BRGR' }}).then(function(restaurant) {
        createReview(user, restaurant, "Mmmm..best burger I've ever had. And the milkshakes are good too.", 4);
      });
      Models.Restaurant.find({ where: { name: 'The French Laundry'}}).then(function(restaurant) {
        createReview(user, restaurant, "The best dining experience in the US. And it's close to beautiful Napa.", 5);
      });
    })
  })
)
);
