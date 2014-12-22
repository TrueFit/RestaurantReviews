// Reusable api responses that can be used in all
// the resources
var api = {
  sendError: function(res, error) {
    res.status(404);
    res.json({ error: error });
  },
  sendOK: function(res, result) {
    res.status(200);
    res.json(result);
  },
  sendCreated: function(res, result) {
    res.status(201);
    res.json(result);
  },
  sendNotFound: function(res, error) {
    res.status(404);
    res.json({ error: error });
  },
  sendBadRequest: function(res) {
    res.status(400);
    res.json({ error: 'Invalid parameters' });
  },
  sendNoContent: function(res) {
    res.status(204);
    res.json({ status: 'Content removed' });
  },
  sendResultOrError: function(res, result, error) {
    if (result.length == 0) {
      api.sendError(res, error);
    } else {
      api.sendOK(res, result);
    }
  }
}

module.exports = api;
