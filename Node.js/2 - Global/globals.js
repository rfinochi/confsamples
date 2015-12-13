require('./external');
    
var inThisFile = 'this file';
    
// 'without var'
console.log(global.externalWithoutVar);
    
// undefined
console.log(global.externalWithVar);
    
// undefined
console.log(global.inThisFile);
    
// 'this file'
console.log(inThisFile);