var clients = {},
    userCount = 1;
      
var addUser = function(sessionId) {
  var userName = 'User ' + userCount;
  clients[sessionId] = userName;
  userCount++;
  return {
    messageType: 'system',
    text: userName + ' has connected.'
  };
};
      
var changeNick = function(sessionId, nickMessage) {
  var originalNick = clients[sessionId],
      newNick = nickMessage.replace('/nick ', '');
  clients[sessionId] = newNick;
  return {
    messageType: 'system',
    text: originalNick + ' changed nick to ' + newNick
  };
};
      
var processMessage = function(sessionId, message) {
  if(message.indexOf('/nick') === 0) {
    return changeNick(sessionId, message);
  }
  return {
    messageType: 'chat',
    nick: clients[sessionId],
    text: message
  };
};
      
var removeUser = function(sessionId) {
  var nick = clients[sessionId];
  delete clients[sessionId];
  return {
    messageType: 'system',
    text: nick + ' was disconnected.'
  };
};
      
      
module.exports.addUser = addUser;
module.exports.processMessage = processMessage;
module.exports.removeUser = removeUser;