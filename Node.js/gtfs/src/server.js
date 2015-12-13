var express = require('express');
var gtfs = require('gtfs');
var app = express();

app.get('/', function(req, res, next) {
	res.send('GTFS Demo Web API');
});

app.get('/agencies/', function(req, res, next) {
	gtfs.agencies(function(e, agencies) {
		res.json(agencies);
	});
});

app.get('/getagenciesbydistance/:lat/:lon/:radius', function(req, res, next) {
	var lat = req.params.lat;
	var lon = req.params.lon;
	var radius = req.params.radius;

	if(isNaN(lat)){
		res.status(400).json({message: 'bad param (lat)'});
		return;
	}
	if(isNaN(lon)){
		res.status(400).json({message: 'bad param (lon)'});
		return;
	}
	if(isNaN(radius)){
		res.status(400).json({message: 'bad param (radius)'});
		return;
	}

	gtfs.getAgenciesByDistance(lat, lon, radius, function(err, agencies) {
		res.json(agencies);
	});
});

app.get('/getagency/:agency_key', function(req, res, next) {
	var agency_key = req.params.agency_key;
	
	gtfs.getAgency(agency_key, function(err, agency) {
		res.json(agency);
	});
});

app.get('/getroutesbyagency/:agency_key', function(req, res, next) {
	var agency_key = req.params.agency_key;
	
	gtfs.getRoutesByAgency(agency_key, function(err, routes) {
		res.json(routes);
	});
});

app.get('/getroutesbyid/:agency_key/:route_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	
	gtfs.getRoutesById(agency_key, route_id, function(err, routes) {
		res.json(routes);
	});
});

app.get('/getroutesbydistance/:lat/:lon/:radius', function(req, res, next) {
	var lat = req.params.lat;
	var lon = req.params.lon;
	var radius = req.params.radius;

	if(isNaN(lat)){
		res.status(400).json({message: 'bad param (lat)'});
		return;
	}
	if(isNaN(lon)){
		res.status(400).json({message: 'bad param (lon)'});
		return;
	}
	if(isNaN(radius)){
		res.status(400).json({message: 'bad param (radius)'});
		return;
	}
	
	gtfs.getRoutesByDistance(lat, lon, radius, function(err, routes) {
		res.json(routes);
	});
});

app.get('/getstops/:agency_key/:stop_ids', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var stop_ids = JSON.parse(req.params.stop_ids);
	
	gtfs.getStops(agency_key, stop_ids, function(err, stops) {
		res.json(stops);
	});
});

app.get('/getstopsbyroute/:agency_key/:route_id/:direction_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	var direction_id = req.params.direction_id;
	
	gtfs.getStopsByRoute(agency_key, route_id, direction_id, function(err, stops) {
		res.json(stops);
	});
});

app.get('/getstopsbydistance/:lat/:lon/:radius', function(req, res, next) {
	var lat = req.params.lat;
	var lon = req.params.lon;
	var radius = req.params.radius;

	if(isNaN(lat)){
		res.status(400).json({message: 'bad param (lat)'});
		return;
	}
	if(isNaN(lon)){
		res.status(400).json({message: 'bad param (lon)'});
		return;
	}
	if(isNaN(radius)){
		res.status(400).json({message: 'bad param (radius)'});
		return;
	}
	
	gtfs.getStopsByDistance(lat, lon, radius, function(err, stops) {
		res.json(stops);
	});
});

app.get('/getstoptimesbytrip/:agency_key/:trip_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var trip_id = req.params.trip_id;
	
	gtfs.getStoptimesByTrip(agency_key, trip_id, function(err, stoptimes) {
		res.json(stoptimes);
	});
});

app.get('/getstoptimesbystop/:agency_key/:route_id/:stop_id/:direction_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	var stop_id = req.params.stop_id;
	var direction_id = req.params.direction_id;
	
	gtfs.getStoptimesByStop(agency_key, route_id, stop_id, direction_id, function(err, stoptimes) {
		res.json(stoptimes);
	});
});

app.get('/gettripsbyrouteanddirection/:agency_key/:route_id/:direction_id/:service_ids', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	var direction_id = req.params.direction_id;
	var service_ids = JSON.parse(req.params.service_ids);
	
	gtfs.getTripsByRouteAndDirection(agency_key, route_id, direction_id, service_ids, function(err, trips) {
		res.json(trips);
	});
});

app.get('/findbothdirectionnames/:agency_key/:route_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	
	gtfs.findBothDirectionNames(agency_key, route_id, function(err, directionNames) {
		res.json(directionNames);
	});
});

app.get('/getshapesbyroute/:agency_key/:route_id/:direction_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	var direction_id = req.params.direction_id;
	
	gtfs.getShapesByRoute(agency_key, route_id, direction_id, function(err, shapes) {
		res.json(shapes);
	});
});

app.get('/getcoordinatesbyroute/:agency_key/:route_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	
	gtfs.getCoordinatesByRoute(agency_key, route_id, function(err, coordinates) {
		res.json(coordinates);
	});
});

app.get('/getcalendars/:agency_key/:route_id/:start_date/:end_date/:monday/:tuesday/:wednesday/:thursday/:friday/:saturday/:sunday', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var start_date = Date.parse(req.params.start_date);
	var end_date = Date.parse(req.params.end_date);
	var monday = req.params.monday;
	var tuesday = req.params.tuesday;
	var wednesday = req.params.wednesday;
	var thursday = req.params.thursday;
	var friday = req.params.friday;
	var saturday = req.params.saturday;
	var sunday = req.params.sunday;
	
	gtfs.getCalendars(agency_key, start_date, end_date, monday, tuesday, wednesday, thursday, friday, saturday, sunday, function(err, calendars) {
		res.json(calendars);
	});
});

app.get('/getcalendarsbyservice/:service_ids', function(req, res, next) {
	var service_ids = JSON.parse(req.params.service_ids);
	
	gtfs.getCalendarsByService(service_ids, function(err, calendars) {
		res.json(calendars);
	});
});

app.get('/getcalendardatesbyservice/:service_ids', function(req, res, next) {
	var service_ids = JSON.parse(req.params.service_ids);
	
	gtfs.getCalendarDatesByService(service_ids, function(err, calendars) {
		res.json(calendars);
	});
});

app.get('/getfeedinfo/:agency_key', function(req, res, next) {
	var agency_key = req.params.agency_key;
	
	gtfs.getFeedInfo(agency_key, function(err, feedinfo) {
		res.json(feedinfo);
	});
});

app.get('/getroutedirection/:agency_key/:route_id/:direction_id', function(req, res, next) {
	var agency_key = req.params.agency_key;
	var route_id = req.params.route_id;
	var direction_id = req.params.direction_id;
	
	gtfs.getRouteDirection(agency_key, route_id, direction_id, function(err, routeDirection) {
		res.json(routeDirection);
	});
});

app.listen(process.env.PORT || 8080);