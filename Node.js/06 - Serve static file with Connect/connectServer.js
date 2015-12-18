var connect = require('connect');
    
// Creating a connect server with
// two middleware modules. The logger
// module will always be called first.
var server = connect.createServer(
  connect.logger(),
  connect.static(__dirname)
);
    
server.listen(8000);