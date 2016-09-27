/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import * as path from 'path';
import {Hal} from '@azure-iot/hal/types';
import {config} from '@azure-iot/configuration';

export class Config {
    constructor(
        /** 
         * {string} connStr Connection string to the IoTHub.
         */
        public IotHubConnectionString: string,
        
        /**
         * {string} Console reporting source ("server", "client", or "both").
         */
        public ConsoleReporting: string,
        
        /**
         * {string} The Log Level. See https://github.com/trentm/node-bunyan#levels 
         * for acceptable values.
         */
        public LogLevel: string,
        
        /**
         * {string} The port to listen on.
         */
        public Port: string,

        /**
         * {boolean} Is Caching Enabled
         */
        public CachingEnabled: boolean,
        
        /**
         * (Optional) Settings for the Authentication module.
         */
        public Auth?: {
            loginUrl: string,
            mongoUri: string,
            sessionSecret: string
        }) {}
    
    private static instance: Config = null;
    
    /**
     * Returns the Config singleton instance. This method should only 
     * be called after Config.initialize() has been run.
     */
    public static get(): Config {
        if (!Config.instance) {
            throw new Error('Config has not yet been initialized');
        }
        
        return Config.instance;
    }
    
    /**
     * Initializes configuration from either the Config Service or user-config.json.
     * If process.env.CONFIG_URL is specified, this function waits till the settings 
     * are available in the Config Service.
     * Otherwise, the configuration is initialized from user-config.json.   
     */
    public static async initialize(): Promise<Config> {
        await config.initialize({
            configFilename: path.join(__dirname, '..', '..', 'user-config.json'),
            requiredKeys: ['IOTHUB_CONNECTION_STRING'],
            defaultValues: {
                CONSOLE_REPORTING: 'both',
                LOG_LEVEL: 'trace',
                PORT: '3003',
                CACHING_ENABLED: true
            }
        });

        const hubConnStr = config.getString('IOTHUB_CONNECTION_STRING');        
        if (!/(^|;)HostName=/i.test(hubConnStr)) {
            throw new Error('IOTHUB_CONNECTION_STRING was not filled out correctly; please fill out the information in configuration');
        }

        // get auth values:
        let auth = {
            loginUrl: config.getString('LOGIN_URL'),
            mongoUri: config.getString('MONGO_URI'),
            sessionSecret: config.getString('SESSION_SECRET')
        };

        if (!(auth.loginUrl && auth.mongoUri && auth.sessionSecret)) {
            auth = null;
        }

        // set the static singleton and return:
        return Config.instance = new Config(
            hubConnStr,
            config.getString('CONSOLE_REPORTING'),
            config.getString('LOG_LEVEL'),
            config.getString('PORT'),
            config.get<boolean>('CACHING_ENABLED'),
            auth);
    }
}
