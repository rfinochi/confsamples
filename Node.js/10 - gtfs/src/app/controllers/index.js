'use strict';

var app = require('angular').module('app');

app.controller('About', require('./about'));
app.controller('AboutResult', require('./aboutresult'));
app.controller('CoordinatesByRoute', require('./coordinatesbyroute'));
app.controller('Home', require('./home'));
app.controller('Route', require('./route'));
app.controller('Routes', require('./routes'));
app.controller('RoutesByDistance', require('./routesbydistance'));
app.controller('Stop', require('./stop'));
app.controller('StopsByDistance', require('./stopsbydistance'));
