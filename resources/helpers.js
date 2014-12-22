// Various helper methods
var helpers = {
  titleize: function(str) {
    if (str == null) return '';
    str  = String(str).toLowerCase();
    return str.replace(/(?:^|\s|-)\S/g, function(c){ return c.toUpperCase(); });
  }
};

module.exports = helpers;
