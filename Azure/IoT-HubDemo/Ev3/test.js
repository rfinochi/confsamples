var ev3dev = require('./node_modules/ev3dev-lang/bin/index.js');

var ultrasonicSensor = new ev3dev.UltrasonicSensor();
if(!ultrasonicSensor.connected) {
    console.error("No ultrasonic sensor could be found");
    process.exit(1);
}

var motorA = new ev3dev.Motor(ev3dev.OUTPUT_A);
if(!motorA.connected) {
    console.error("No valid motor was found in port A");
    process.exit(1);
}

var motorB = new ev3dev.Motor(ev3dev.OUTPUT_D);
if(!motorB.connected) {
    console.error("No valid motor was found in port B");
    process.exit(1);
}

setInterval(function() {
	console.log("Distance " + ultrasonicSensor.distanceCentimeters);

	if (ultrasonicSensor.distanceCentimeters < 10) {
		motorA.dutyCycleSp = 100;
		motorA.command = 'run-direct';
		
		motorB.dutyCycleSp = 100;
		motorB.command = 'run-direct';
	}
	else
	{
		motorA.command = 'stop';
		motorB.command = 'stop';
	}
}, 10);