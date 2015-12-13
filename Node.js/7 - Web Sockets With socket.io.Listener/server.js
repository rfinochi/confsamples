var connect = require('connect'),
    io = require('socket.io');
      
var server = connect.createServer(
  connect.static(__dirname + '/public')
);
      
var socketListener = io.listen(server);
      
socketListener.on('connection', function(client) {
  client.on('message', function(message) {
    client.broadcast({
      messageType: 'chat',
      nick: client.sessionId,
      text: message
    });
  });
      
  client.on('disconnect', function() {
    socketListener.broadcast({
      messageType: 'system',
      text: client.sessionId + ' was disconnected.'
    });
  });
});
      
server.listen(8000);