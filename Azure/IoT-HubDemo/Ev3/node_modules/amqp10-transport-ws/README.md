node-amqp10-transport-ws
========================

[![Build Status](https://secure.travis-ci.org/noodlefrenzy/node-amqp10-transport-ws.svg?branch=master)](https://travis-ci.org/noodlefrenzy/node-amqp10-transport-ws)
[![Dependency Status](https://david-dm.org/noodlefrenzy/node-amqp10-transport-ws.svg)](https://david-dm.org/noodlefrenzy/node-amqp10-transport-ws)
[![Code Climate](https://codeclimate.com/github/noodlefrenzy/node-amqp10-transport-ws/badges/gpa.svg)](https://codeclimate.com/github/noodlefrenzy/node-amqp10-transport-ws)
[![Test Coverage](https://codeclimate.com/github/noodlefrenzy/node-amqp10-transport-ws/badges/coverage.svg)](https://codeclimate.com/github/noodlefrenzy/node-amqp10-transport-ws)
[![npm version](https://badge.fury.io/js/amqp10-transport-ws.svg)](http://badge.fury.io/js/amqp10-transport-ws)
[![Join the main amqp10 chat at https://gitter.im/noodlefrenzy/node-amqp10](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/noodlefrenzy/node-amqp10?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Provides a Websocket implementation for the transport layer for node-amqp10.

The [amqp10](http://github.com/noodlefrenzy/node-amqp10/) library exports a `TransportProvider` class that is used to manage and inject new transports for the library to use.
Each transport should expose a `register` method that takes the `TransportProvider` as an argument, allowing any transport to register a new protocol to be used.

In the present case the protocol registered is `wss` (webocket transport using the [node-websocket](https://github.com/sitegui/nodejs-websocket) library).

# Usage

```js
var amqp10 = require('amqp10');
var wsTransport = require('amqp10-transport-ws');

wsTransport.register(amqp10.TransportProvider);
```

Once registered, any URI starting with `wss://` given to the `connect` method of `amqp10.Client` will be handled by the websocket transport.
