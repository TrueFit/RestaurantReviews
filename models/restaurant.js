"use strict";

module.exports = function(sequelize, DataTypes) {
  var Restaurant = sequelize.define("Restaurant", {
    name: DataTypes.STRING,
    city: DataTypes.STRING,
    state: DataTypes.STRING,
    slug: DataTypes.STRING
  }, {
    classMethods: {
      associate: function(models) {
        Restaurant.hasMany(models.Review, { foreignKey: 'restaurant_id' });
      }
    }
  });

  return Restaurant;
};
