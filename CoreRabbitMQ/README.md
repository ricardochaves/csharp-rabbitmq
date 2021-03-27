# CoreRabbitMq

## Install

`install nuget`

## Publisher for you API

You need inject the configuration:
`services.AddSingleton<IConfiguration>(provider => Configuration);`

In your `appsettings.json` use:

```json
  "CoreRabbitMq":{
    "Hosts":[
      {"Host": "localhost", "Port": 5672},
      {"Host": "localhost", "Port": 5673}
    ],
    "UserName":"guest",
    "Password":"guest",
    "RequestedHeartbeatSeconds": 5,
    "ClientProvidedNamePrefix": "App-the-test",
    "NetworkRecoveryIntervalSeconds": 10,
    "FirstConnectionRetryIntervalMilliSeconds": 5000
  }
```

## Consumers
