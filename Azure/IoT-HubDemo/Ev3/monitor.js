var http = require('http');

var ev3dev = require('./node_modules/ev3dev-lang/bin/index.js');

var device = require('azure-iot-device');
var transportHttp = require('azure-iot-device-http').Http;
var connectionString = 'HostName=Ev3.azure-devices.net;DeviceId=Ev3;SharedAccessKey=0Pjh/FCH+dZi/PGw9lmV01ed9NiCb34ooruFE3hDTYw=';

var touchSensor = new ev3dev.TouchSensor();
var colorSensor = new ev3dev.ColorSensor();
var ultrasonicSensor = new ev3dev.UltrasonicSensor();
var infraredSensor = new ev3dev.InfraredSensor();
var soundSensor = new ev3dev.SoundSensor();
var battery = new ev3dev.PowerSupply();

var tablee = [];
var str = '';
var i = 0;
setInterval(function () {
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
        str += 'ultrasonicSensor: \n   distanceCentimeters: ' + ultrasonicSensor.distanceCentimeters + '\n';
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
}, 10);