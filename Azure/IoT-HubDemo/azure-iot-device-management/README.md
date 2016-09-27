# Azure IoT device management Sample UI

This repository contains a UX reference implementation that enables device management capabilities for devices registered with an Azure IoT hub. These capabilities include:

- Querying for devices.
- Configuring a view into the list of devices.
- Basic CRUD operations on single and multiple devices.
- Setting tags, service, and device properties on a device.
- Sending commands to devices that correspond to firmware update, reboot, and factory reset device jobs.
- Querying for a list of jobs submitted to the hub.

For more about device management functionality provided by Azure IoT Hub please see:

- [Azure IoT device management overview](https://azure.microsoft.com/documentation/articles/iot-hub-device-management-overview/)
- [How to create an Azure IoT Hub and provision devices](https://azure.microsoft.com/documentation/articles/iot-hub-device-management-get-started-node/) 
- [Explore Azure IoT Hub device management using the sample UI](https://azure.microsoft.com/documentation/articles/iot-hub-device-management-ui-sample/)

### <a name="knownissues"></a>Known Issues

- UX will start timing out after 60 minutes and server (console window) will report 500. Use below steps to stop and restart the services.
- Server debugging not working
- Browser window does not launch when using develop mode on Mac/OSX

## Prerequisites

### 1. Get an Azure IoT Hub connection string

You will need an active Azure subscription.	If you don't have an account, you can create a free trial account in just a couple of minutes. For details, see [Azure Free Trial](http://azure.microsoft.com/pricing/free-trial/).

You need to create a device management enabled IoT Hub to obtain an IoT Hub connection string. You can create a device management enabled IoT Hub in the Azure [portal](https://portal.azure.com/).

### 2. Install Node.js 6.X.X

You can find Node.js for windows/mac/linux [here](https://nodejs.org).

## Environment preparation

### 1. Setup up your development machine and clone this repository

Windows/OSX/Linux are all supported operating systems. Using a git client clone this repository to a local folder on your machine.

If you are using bash, you can use the following command:

    git clone https://github.com/Azure/azure-iot-device-management.git
    
Execute all the console/command prompt commands below in the root of the cloned folder (for example, /user/me/git/azure-iot-device-management).

### 2. Install libraries and modules 

In a console or node command prompt, run (ignoring any warnings):

    npm install

When the npm install command has completed you are returned to the prompt without any errors. You only need to 
re-run this command after you pull the latest code from the repository.

### 3. Build the sample

In a console or command prompt, run:

    npm run build

Execute this command when you first clone the repository and whenever you update the code by editing it or by pulling changes from the repository.

### 4. Configure IoT Hub

Use a text editor to open the ```user-config.json``` file in root folder of your cloned copy of the repository.

Replace the text &lt;YOUR CONNECTION STRING HERE&gt; with the hub's connection string. You can find the connection string in the Azure [portal](https://portal.azure.com). Be sure to save the file. This is a one-time operation and only needs to be repeated if you purge the local copy of the repository or decide to use a different IoT Hub. 

### 5. Optional user configuration flags

The following flags are in the same ```user-config.json``` file as your hub connection string.

__Console Reporting:__

key: ```"CONSOLE_REPORTING"```

values:

- ```"none"``` - no logging of any kind.
- ```"client"``` - logs all requests for static assets (html/js/css).
- ```"server"``` - logs all API requests going into or out from the server as well as any uncaught exceptions.
- ```"both"```(_default_) - logs all requests coming into and out from the server as well as any uncaught exceptions.
    
__Log Level:__ Setting the log level forces only log records at that level or higher to be logged.

key: ```"LOG_LEVEL"```

values:

- ```"trace"```(_default_)
- ```"debug"```
- ```"info"```  
- ```"warn"```  
- ```"error"```  
- ```"fatal"```  

__Port:__

key: ```"PORT"```

value: string of the port number

__Caching:__

key: ```"CACHING_ENABLED"```

value: boolean; if true, caching is enabled, false, not.
    

## Run the user experience mode

Ensure you have completed the _Environment preparation_ step.

This mode runs the code without the overhead of a developer setup. This is useful when you only need to run the code as an app.

In a console or command prompt, run:

    npm run start

The console window reports:

    Services starting...
    Services started: listening on <PORT>
    
Where `<PORT>` is the configured port that the services are currently listening on (see _Configuration_ below). By default, this is [port 3003](http://127.0.0.1:3003).

Supported browsers are: Edge/IE 11+/Safari/Chrome

## Stop the user experience

In the console window running the services use CTRL+C. You can re-start immediately from this state using the _npm run start_ command.
       
## Run the developer experience mode

The recommended editor is [vscode](https://code.visualstudio.com/)

To start the developer experience, launch a console or command prompt and run:

    npm run develop

This runs a build and launches a browser window to the port configured (see _Configuration_ below). The default is [port 3003](http://127.0.0.1:3003). Whenever you edit and save any files the code automatically recompiles and the browser refreshes.

## Developer tasks

You can find various smaller tasks in the build-scripts\test.js file. More general tasks are listed below.

This command builds the project, putting the output into the dist folder. This is the basis for other tasks:

    npm run build

This runs the both the server and app tests. A browser window launches on success with code coverage reports:

    npm run test

## Debugging

To debug the app, use your browser developer tool. All client files are transpiled javascript, sass/css, or html.

To debug the server

1. Use **vscode** to add breakpoints to javascript transpiled code generated in dist\server folder

2. Run node in debug mode

        node debug dist/server/start.js
    
  Follow the debugging instructions [here](https://nodejs.org/api/debugger.html)

3. Switch to node debugger, select "Attach Server Debug" and press play. You can find the vscode debugger instructions  [here](https://code.visualstudio.com/Docs/editor/debugging)
 
## Deployment guidelines

- Please visit [here](https://azure.microsoft.com/en-us/develop/nodejs/) to learn how run a node.js application on Azure.
- You should run the application using the https protocol if you are deploying outside of a local setup.

### Deploying to Azure Websites with Git

1. Create a website with git credentials, and add it as a remote for your pulled copy. See [Getting Started with NodeJS](https://azure.microsoft.com/en-us/documentation/articles/app-service-web-nodejs-get-started/) for more details.
2. Navigate to the website in the Azure Portal.
3. Add the app setting `IOTHUB_CONNECTION_STRING` with the hub connection string.
4. Push from your local github repository.

#### Notes and common issues

- If you find that the application isn't starting, turn on logging in iisnode.yml and investigate the stderr. This most often occurs because it can't find the connection string. By default stdout and stderr are dropped in the 'logs' folder under the site.
- If you find that you're getting a number of 503s when launching the application, wait a little bit and try refreshing.
- The first deployment sometimes disconnects when doing the install step. If this occurs, pushing again will resolve the issue.

## Configuration

There are three places where you can configure the experience. In order of preference:

1. The user-config.json file. This configuration is suitable for development experience only.
2. Environment variables. This configuration is suitable when deploying.
3. Hard-coded defaults (if they exist).

## Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.