var connect = require('connect'),
    io = require('socket.io'),
    room = require('./chatRoom');
      
var server = connect.createServer(
  connect.static(__dirname + '/public')
);
      
var socketServer = io.listen(server);
      
socketServer.on('connection', function(client) {
  var sessionId = client.sessionId,
      addedUserMessage = room.addUser(sessionId);
      
  client.on('message', function(message) {
    var procdMsg = room.processMessage(sessionId, message);
    client.broadcast(procdMsg);
  });
      
  client.on('disconnect', function() {
    var removedUserMessage = room.removeUser(sessionId);
    socketServer.broadcast(removedUserMessage);
  });
      
  client.broadcast(addedUserMessage);
});
      
server.listen(8000);