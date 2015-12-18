var fs = require('fs');
    
var logFileContents = function(fileName) {
  fs.readFile(fileName, function(err, file) {
    if(err) {
      console.log('There was an error.');
    } else {
      console.log(file.toString());
    }
  });
};
// 'Look! Flying monkeys!'
logFileContents(__dirname + '/flyingMonkeys.txt');
// 'There was an error.'
logFileContents('does not exist');