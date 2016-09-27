var http = require('http');

var ev3dev = require('./node_modules/ev3dev-lang/bin/index.js');

var device = require('azure-iot-device');
var transportHttp = require('azure-iot-device-http').Http;
var connectionString = '';

var touchSensor = new ev3dev.TouchSensor();
var colorSensor = new ev3dev.ColorSensor();
var ultrasonicSensor = new ev3dev.UltrasonicSensor();
var soundSensor = new ev3dev.SoundSensor();
var battery = new ev3dev.PowerSupply();

var ret = '';

setInterval(function () {
    ret = {
            batteryMeasuredVoltage: battery.measuredVoltage,
            batteryMaxVoltage: battery.maxVoltage,
            touchSensorIsPressed: touchSensor.isPressed,
            colorSensorReflectedLightIntensity: colorSensor.reflectedLightIntensity, //%
            colorSensorAmbientLightIntensity: colorSensor.ambientLightIntensity,
            colorSensorColor: colorSensor.color, //0: No color, 1: Black, 2: Blue, 3: Green, 4: Yellow, 5: Red , 6: White, 7: Brown
            ultrasonicSensorDistanceCentimeters: ultrasonicSensor.distanceCentimeters,
            soundSensorSoundPressure: 0 //soundSensor.soundPressure //db
        };

    sendmsg(JSON.stringify( 
        ret 
    ));

    function sendmsg(data) {
        var client = device.Client.fromConnectionString(connectionString, transportHttp);
        var message = new device.Message(data);
        message.properties.add('Ev3Sensors', 'sensorData');
        console.log("Sending message: " + message.getData() + '\n\n');
        client.sendEvent(message, printResultFor('send'));
    }
    
    function printResultFor(op) {
        return function printResult(err, res) {
            if (err)
                console.log(op + ' error: ' + err.toString()) + '\n\n';
            else
                console.log(op + ' successful\n\n');
        };
    }
}, 10);