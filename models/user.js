"use strict";

module.exports = function(sequelize, DataTypes) {
  var User = sequelize.define("User", {
    email: DataTypes.STRING,
    first_name: DataTypes.STRING,
    last_name: DataTypes.STRING,
    username: DataTypes.STRING
  }, {
    classMethods: {
      associate: function(models) {
        User.hasMany(models.Review, { foreignKey: 'user_id' });
      }
    },
    instanceMethods: {
      fullName: function() {
        return [this.firstname, this.lastname].join(' ')
      }
    }
  });

  return User;
};
