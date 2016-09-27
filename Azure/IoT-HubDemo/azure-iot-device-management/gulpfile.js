/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Broken into five types of tasks:
- Install Tasks: Making sure you have a working environment
- Build Tasks: Used to create a working instance
- Development Tasks: Used to work inside your development environment
- Test Tasks: Used to test code

*/
require('./build-scripts/install.js');
require('./build-scripts/build.js');
require('./build-scripts/develop.js');
require('./build-scripts/test.js');
require('./build-scripts/bundle.js');