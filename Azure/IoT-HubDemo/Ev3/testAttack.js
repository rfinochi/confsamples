var ev3dev = require('./node_modules/ev3dev-lang/bin/index.js');

var touchSensor = new ev3dev.TouchSensor();
if(!touchSensor.connected) {
    console.error("No ultrasonic sensor could be found");
    process.exit(1);
}

var motorA = new ev3dev.Motor(ev3dev.OUTPUT_A);
if(!motorA.connected) {
    console.error("No valid motor was found in port A");
    process.exit(1);
}

var motorB = new ev3dev.Motor(ev3dev.OUTPUT_B);
if(!motorB.connected) {
    console.error("No valid motor was found in port B");
    process.exit(1);
}

var motorC = new ev3dev.Motor(ev3dev.OUTPUT_C);
if(!motorC.connected) {
    console.error("No valid motor was found in port C");
    process.exit(1);
}

var motorD = new ev3dev.Motor(ev3dev.OUTPUT_D);
if(!motorD.connected) {
    console.error("No valid motor was found in port D");
    process.exit(1);
}

var step1 = false;
var step2 = false;

motorA.dutyCycleSp = 100;
motorD.dutyCycleSp = 100;
motorB.speedSp = 600;
motorC.speedSp = 600;

setInterval(function() {
	console.log("Touch Sensor Is Pressed: " + touchSensor.isPressed);
	console.log("Motor B Running State: " + motorB.state.indexOf("running"));
	console.log("Motor C Running State: " + motorC.state.indexOf("running"));
	console.log("Motor A Running State: " + motorA.state.indexOf("running"));
	console.log("Motor D Running State: " + motorD.state.indexOf("running"));
	console.log("Step 1 Flag: " + step1);
	console.log("Step 2 Flag: " + step2);
	console.log("");

	if (touchSensor.isPressed) {
		motorB.command = 'stop';
		motorC.command = 'stop';

		motorA.command = 'run-direct';
		motorD.command = 'run-direct';
	
		step1 = false;
		step2 = false;
	}
	else if (motorC.state.indexOf("running") == -1 && step1 && !step2) {
		motorA.command = 'stop';
		motorD.command = 'stop';

		motorB.command = 'stop';
		motorC.timeSp = 1000;
		motorC.command = "run-timed";

		step2 = true;
	}
	else if (motorB.state.indexOf("running") == -1 &&
		     motorC.state.indexOf("running") == -1 && !step2) {
		motorA.command = 'stop';
		motorD.command = 'stop';
		
		motorB.timeSp = 3000;
		motorB.command = "run-timed";
		motorC.timeSp = 3000;
		motorC.command = "run-timed";

		step1 = true;
	}
	else if (motorB.state.indexOf("running") == -1 &&
		     motorC.state.indexOf("running") == -1 && step1 && step2) {
		step1 = false;
		step2 = false;
	}
}, 10);

function sleep(milliseconds) {
  var start = new Date().getTime();
  for (var i = 0; i < 1e7; i++) {
    if ((new Date().getTime() - start) > milliseconds){
      break;
    }
  }
}