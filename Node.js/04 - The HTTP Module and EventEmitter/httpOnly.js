var http = require('http'),
    server = http.createServer();
    
server.on('request', function(req, res) {
  // Handler functions receive http.ServerRequest
  // and http.ServerResponse objects as arguments
  res.writeHead(200, {
    'Content-Type': 'text/plain'
  });
  res.write('Look! Flying monkeys!');
  res.end();
});
    
// Start the http server on port 8000
server.listen(8000);