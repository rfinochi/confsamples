/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Docker Tasks: Used to work with Docker

*/

'use strict'

const proc = require('child_process');
const program = require('commander');
const path = require('path');
const npmPackage = require('../package.json');

/**
 * Run a command in docker and return the result, with optional console logging
 * @param {Array} cmd List of string arguments to pass to 'docker'
 * @param {string} log Optional. If specified, the output of the command is written
 * directly to the current console. Otherwise, the command's stdout is buffered
 * and returned as a string.
 * @return {string} The command's stdout as a string, if 'log' isn't specified. 
 * Otherwise, an empty string is returned.
 */
function docker(cmd, log) {
    if (log) {
        process.stdout.write(log + '...\n');
    }
    
    let result = proc.spawnSync('docker', cmd, {stdio: (log ? 'inherit' : 'pipe')});
    if (result.status) {
        if (result.stderr) {
            process.stderr.write(result.stderr.toString());
        }
        
        throw new Error(`${cmd} returned error code ${result.status}`);
    }
        
    return result.stdout ? result.stdout.toString() : '';
}

// calculate the image name to use
function image_name() {
    return program.user + '/' + program.image;
}

// obtain a list of containers for the image
function docker_containers() {
    return docker(['ps', '-a', '-q', '-f', `ancestor=${image_name()}`]).split('\n').filter(id => id.length > 0);
}

program
    .option('-i, --image [name]', 'image name', 'device-management')
    .option('-u, --user [name]', `user name [azureiot]`, 'azureiot');

program
    .command('build')
    .description('build the image')
    .option('-c, --clean', 'clean the image before building')
    .option('-f, --force', 'ensure the image is not running')
    .option('-s, --save [file]', 'save the image to a file')
    .action(options => {
        if (options.force) {
            program.emit('kill');
        } else if (docker_containers().length) {
            // prevent dangling images
            throw new Error(`Error: There are containers created from ${image_name()} on this system. Please remove them before rebuilding or run this command with the "--force" option.`);
        }
        if (options.clean) {
            program.emit('clean');
        }
        
        docker(['build', '-t', `${image_name()}:latest`, '-t', `${image_name()}:${npmPackage.version}`, '.'], `Building image ${image_name()}:${npmPackage.version}`);
        if (options.save) {
            let filename = typeof options.save === 'string' ? options.save : program.image + '.tar';
            docker(['save', '-o', filename, image_name()], `Saving image to ${filename}`);
        }
    });

program
    .command('push')
    .arguments('[tag]')
    .description(`push image to DockerHub with specified tag (defaults to npm version)`)
    .action((tag, options) => {
        const image = `${image_name()}:${tag || npmPackage.version}`;
        docker(['push', image], `Pushing ${image}`);
    });
    
program
    .command('clean')
    .description('remove the image from the local images')
    .option('-f, --force', 'ensure the image is not running')
    .action(options => {
        if (options.force) {
            program.emit('kill');
        }
        
        // Docker will fail if we try to remove a nonexistant image; make sure it exists
        if (docker(['images', '-q', image_name()])) {
            docker(['rmi', image_name()], `Cleaning image ${image_name()}`);
        }
    });

program
    .command('start')
    .description('run the image')
    .option('-p, --ports <file>', 'specify the JSON file which lists port mappings [ports.json]', 'ports.json')
    .action(options => {
        let ids = docker_containers();
        if (ids.length) {
            // if there are any existing containers for this image, start them
            docker(['start'].concat(ids), `Starting containers for image ${image_name()}`);
        } else {
            // parse port mappings
            let ports = require(path.resolve(options.ports));
            let portArgs = Object.keys(ports).map(port => ['-p', port + ':' + ports[port]]).reduce((prev, curr) => prev.concat(curr));

            // start a new instance of the image
            docker(['run', '-d'].concat(portArgs).concat(image_name()),
                `Starting image ${image_name()} on the following ports: ${Object.keys(ports).join(' ')}`);
        }
    });

program
    .command('stop')
    .description('stop all running instances of the image')
    .action(() => {
        let ids = docker_containers();
        if (ids.length) {
            docker(['stop'].concat(ids), `Stopping containers for image ${image_name()}`);
        }
    });

program
    .command('kill')
    .description('stop and remove all instances of the image')
    .action(() => {
        let ids = docker_containers();
        if (ids.length) {
            docker(['stop'].concat(ids), `Stopping containers for image ${image_name()}`);
            docker(['rm'].concat(ids), `Removing containers for image ${image_name()}`);
        }
    });

program
    .command('logs')
    .description('print out logs from all running instances of the image')
    .action(() => {
        for (let id of docker_containers()) {
            console.log(docker(['logs', id]));
        }
    });

program
    .command('ip')
    .description('print out the IP address of the Docker host')
    .action(() => {
        console.log(proc.execSync('docker-machine ip default').toString());
    });

// main
(() => {
    // set docker environment variables
    for (let line of proc.execSync('docker-machine env --shell cmd default').toString().split('\n')) {
        let cmd = line.split(/[ =]/);
        if (cmd[0] === 'SET') {
            process.env[cmd[1]] = cmd[2];
        }
    }

    try {
        program.parse(process.argv);
    } catch (e) {
        console.error(e.message);
    }
})();