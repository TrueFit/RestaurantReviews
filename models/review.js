"use strict";

module.exports = function(sequelize, DataTypes) {
  var Review = sequelize.define("Review", {
    content: DataTypes.TEXT,
    num_stars: DataTypes.INTEGER
  }, {
    classMethods: {
      associate: function(models) {
        Review.belongsTo(models.User, { foreignKey: 'user_id' } );
        Review.belongsTo(models.Restaurant, { foreignKey: 'restaurant_id' } );
      }
    }
  });

  return Review;
};
