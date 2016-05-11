var http = require('http');
var url = require('url');
var ev3dev = require('./node_modules/ev3dev-lang/bin/index.js');
var device = require('azure-iot-device');
//var transportAmqp = require('azure-iot-device-amqp').Amqp;
//var transportAmqpWs = require('azure-iot-device-amqp-ws').AmqpWs;
//var transportMqtt = require('azure-iot-device-mqtt').Mqtt;
var transportHttp = require('azure-iot-device-http').Http;
var connectionString = '';
var port = process.env.port || 1337
var startAttackCancellationToken = null;

http.createServer(function (req, res) {
    var request = url.parse(req.url, true);
    var action = request.pathname;
    var tablee = [];
    var i = 0;
    
    /* Battery */
    if (action == '/battery') {
        var battery = new ev3dev.PowerSupply();
        var str = '';
        
        if (battery.connected) {
            str += '  Technology: ' + battery.technology + '\n';
            str += '  Type: ' + battery.type + '\n';
            str += '  Current(microamps): ' + battery.measuredCurrent + '\n';
            str += '  Current(amps): ' + battery.currentAmps + '\n';
            str += '  Voltage(microvolts): ' + battery.measuredVoltage + '\n';
            str += '  Voltage(volts): ' + battery.voltageVolts + '\n';
            str += '  Max voltage (microvolts): ' + battery.maxVoltage + '\n';
            str += '  Min voltage (microvolts): ' + battery.minVoltage + '\n';
            tablee[i] = JSON.stringify({
                Sensor: 'PowerSupply', 
                technology: battery.technology,
                type: battery.type,
                measuredCurrent: battery.measuredCurrent,
                currentAmps: battery.currentAmps,
                measuredVoltage: battery.measuredVoltage,
                voltageVolts: battery.voltageVolts,
                maxVoltage: battery.maxVoltage,
                minVoltage: battery.minVoltage
            });
            i++;
        } 
        else {
            str = '  Battery not connected!';
            tablee[i] = JSON.stringify({
                Sensor: 'PowerSupply', 
                error: 'not connected'
            });
            i++
        }
        sendmsg(JSON.stringify( 
            tablee 
        ));
        res.writeHead(200, { 'Content-Type': 'text/plain' });
        res.end(str + JSON.stringify(battery));
    /* Sensors */
    } else if (action == '/sensors') {
        var touchSensor = new ev3dev.TouchSensor();
        var colorSensor = new ev3dev.ColorSensor();
        var ultrasonicSensor = new ev3dev.UltrasonicSensor();
        var infraredSensor = new ev3dev.InfraredSensor();
        var soundSensor = new ev3dev.SoundSensor();

        var str = '';
        
        if (touchSensor.connected) {
            str += 'touch pressed: ' + touchSensor.isPressed + '\n';
            tablee[i] = JSON.stringify({
                Sensor: 'TouchSensor', 
                isPressed: touchSensor.isPressed
            });
            i++;
        }
        if (colorSensor.connected) {
            str += 'color sensor: \n   reflectedLightIntensity: ' + colorSensor.reflectedLightIntensity + '\n';
            str += '   ambientLightIntensity: ' + colorSensor.ambientLightIntensity + '\n';
            str += '   color: ' + colorSensor.color + '\n';
            str += '   red: ' + colorSensor.red + '\n';
            str += '   green: ' + colorSensor.green + '\n';
            str += '   blue: ' + colorSensor.blue + '\n';
            tablee[i] = JSON.stringify({
                Sensor: 'colorSensor', 
                reflectedLightIntensity: colorSensor.reflectedLightIntensity, 
                ambientLightIntensity: colorSensor.ambientLightIntensity, 
                color: colorSensor.color, 
                red: colorSensor.red, 
                green: colorSensor.green, 
                blue: colorSensor.blue
            });
            i++;
        }
        if (ultrasonicSensor.connected) {
            str += 'ultrasonicSensor: \n   distanceCentimeters' + ultrasonicSensor.distanceCentimeters + '\n';
            str += '   otherSensorPresent: ' + ultrasonicSensor.otherSensorPresent + '\n';
            tablee[i] = JSON.stringify({
                Sensor: 'ultrasonicSensor', 
                distanceCentimeters: ultrasonicSensor.reflectedLightIntensity, 
                otherSensorPresent: ultrasonicSensor.ambientLightIntensity
            });
            i++;
        }
        if (infraredSensor.connected) {
            str += 'infraredSensor: \n   proximity: ' + infraredSensor.proximity + '\n';
            tablee[i] = JSON.stringify({
                Sensor: 'infraredSensor', 
                proximity: infraredSensor.proximity
            });
            i++;
        }
        if (soundSensor.connected) {
            str += 'soundSensor: \n   soundPressure: ' + soundSensor.soundPressure + '\n';
            str += '   soundPressureLow: ' + soundSensor.soundPressureLow + '\n';
            tablee[i] = JSON.stringify({
                Sensor: 'soundSensor', 
                soundPressure: soundSensor.soundPressure, 
                soundPressureLow: soundSensor.soundPressureLow
            });
            i++;
        }
        sendmsg(JSON.stringify( 
            tablee 
        ));
        
        res.writeHead(200, { 'Content-Type': 'text/plain' });
        res.end(str);
    /* Watch */
    } else if (action == '/watch') {
        var ultrasonicSensor = new ev3dev.UltrasonicSensor();

		var str = '';
               
        if (ultrasonicSensor.connected) {
			if (ultrasonicSensor.distanceCentimeters < 10) {
				str = '#E70000';
			}
			else
			{
				str = '#5CE700';
			}

            //str = 'Intruder monitoring started';
        }
        else {
            str = 'No valid ultrasonic sensor';
        }
        
        res.writeHead(200, { 'Content-Type': 'text/plain' });
        res.end(str);
    /* Start Atttack */
    } else if (action == '/startAttack') {
        var motorA = new ev3dev.Motor(ev3dev.OUTPUT_A);
        var motorD = new ev3dev.Motor(ev3dev.OUTPUT_D);

		var str = '';
               
        if (motorA.connected && motorD.connected && startAttackCancellationToken == null) {
            motorA.speedRegulationEnabled = 'off';
            motorD.speedRegulationEnabled = 'off';

			startAttackCancellationToken = setInterval(function() {
				motorA.dutyCycleSp = 100;
				motorA.command = 'run-forever';
				
				motorD.dutyCycleSp = 100;
				motorD.command = 'run-forever';
			}, 10);

            str = 'Attack started';
        } else if (startAttackCancellationToken != null) {
            str = 'Attack already started';
		} else {
            str = 'No valid motors on port A or D';
        }
		        
        res.writeHead(200, { 'Content-Type': 'text/plain' });
        res.end(str);
    /* Stop Attack */
    } else if (action == '/stopAttack') {
        var motorA = new ev3dev.Motor(ev3dev.OUTPUT_A);
        var motorD = new ev3dev.Motor(ev3dev.OUTPUT_D);

		var str = '';

		if (motorA.connected && motorD.connected) {
			if (startAttackCancellationToken != null) {
				clearInterval(startAttackCancellationToken);
				startAttackCancellationToken = null;
			}

			motorA.command = 'stop';
			motorD.command = 'stop';

            str = 'Attack stopped';
        }
		else {
            str = 'No valid motors was found in port A or D';
        }
        
        res.writeHead(200, { 'Content-Type': 'text/plain' });
        res.end(str);
    }
	else {
        res.writeHead(200, { 'Content-Type': 'text/plain' });
        res.end('Commands: /battery /sensors /watch /startAttack /stopAttack\n');
    }
}).listen(port);

console.log('Server is listening on port ' + port)

function sendmsg(data) {
    var client = device.Client.fromConnectionString(connectionString, transportHttp);
    var message = new device.Message(data);
    message.properties.add('Ev3Sensors', 'sensorData');
    console.log("Sending message: " + message.getData());
    client.sendEvent(message, printResultFor('send'));
}

function printResultFor(op) {
    return function printResult(err, res) {
        if (err)
            console.log(op + ' error: ' + err.toString());
        else
            console.log(op + ' successful');
    };
}