(function() {
  var socket = new io.Socket('localhost'),
      $chatMessages = $('#chatMessages'),
      $form = $('form'),
      $messageText = $('#messageText');
  
  var messageFactory = (function() {
    var that = {},
        $chatMessage = $('<p></p>').
          addClass('chat message'),
        $nick = $('<span></span>').
          addClass('nick'),
        $systemMessage = $('<p></p>').
          addClass('system message');
    
    var chat = function(message) {
      var $filledNick = $nick.clone().
            text(message.nick + ':');
      return $chatMessage.clone().
        append($filledNick).
        append(message.text);
    };
    
    var system = function(message) {
      return $systemMessage.clone().text(message.text);
    };
    
    that.chat = chat;
    that.system = system;
    
    return that;
  })();
      
  socket.on('message', function(message) {
    var handler = messageFactory[message.messageType];
    $chatMessages.append(handler(message));
  });
      
  $form.submit(function() {
    var message = $messageText.val(),
        nick;
    $messageText.val('');
    if(message.indexOf('/nick') === 0) {
      nick = message.replace('/nick ', '');
      $chatMessages.append(messageFactory.system({
        text: 'You changed your nick to ' + nick + '.'
      }));
    } else {
      $chatMessages.append(messageFactory.chat({
        nick: 'me',
        text: message
      }));
    }
    socket.send(message);
    return false;
  });
      
  socket.connect();
})();