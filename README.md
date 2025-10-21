# IoT Sensor Reader App
## Project Overview
This application reads sensor data from a Raspberry Pi via an Azure cloud IoT Hub, and outputs to multiple destinations (console and MongoDB database).

## Overview of Technologies Used
- Raspberry Pi Zero (acts as IoT device)
	- Raspberry Pi OS Lite
	- Python scripts to simulate sensor readings
- Azure IoT Hub
- .NET 9.0 Application
- MongoDB for DB Output


## Application Components Overview 

### Configuration 
**Purpose:** Manages application settings and connection strings

**Components:**
- `AppConfiguration` - Accesses Azure IoT Hub/EventHub connection details
- `DbConfiguration` - Accesses MongoDB connection settings

---

### Messaging 
**Purpose:** Handles all communication with Azure EventHub

**Components:**
- `EventHubConsumer` - Reads messages from ALL EventHub partitions concurrently
- `JsonMessageDeserializer` - Converts raw JSON strings into `SensorReading` objects
- `MessageCoordinator` - delegates and passes of message reading and processing (consumer → deserializer → processor)

---

### Processing
**Purpose:** Routes sensor readings to the appropriate handler

**Component:**
- `SensorMessageProcessor` - Checks which handler can process each reading, then delegates


### Sensors (Handlers)
**Purpose:** Business logic for each sensor type (validation, alerting, calaculations)

**Components:**
- `TemperatureReadingHandler` - Processes temperature readings
- `HumidityReadingHandler` - Processes humidity readings  
- `UnknownSensorHandler` - Fallback for any sensor type we don't recognize

**Note:** Currently these do not do anything but this is where you'd add validation, alerts, or calculations in a real system.

### Models
**Purpose:** Data structures used throughout the application

**Components:**
- `SensorReading` - Represents a single sensor measurement (ID, type, value, timestamp)
- `SensorType` - Enum defining known sensor types (Temperature=1, Humidity=2, Unknown=0)


### Formatting
**Purpose:** Converts sensor readings into deifferent formats

**Components:**
- `TemperatureFormatter` - Formats temperature with °C unit - human readable/console style string
- `HumidityFormatter` - Formats humidity with % symbol - human readable/console style string
- `UnknownSensorFormatter` - Formats unknown types with warning - human readable/console style string
- `JsonSensorFormatter` - Converts any reading to JSON (i.e. used by database)

---

### Output
**Purpose:** Writes processed sensor data to destinations (console, database, etc.)

**Components:**
- `ConsoleOutputService` - Selects appropriate formatter and writes to terminal
- `DbOutputService` - Formats as JSON and inserts into MongoDB
- `CompositeOutputService` - Distributes each reading to ALL output services in parallel



## End-to-End Message Process

Tracing two messages from IoT device (Raspberry Pi) through to Application outputs (console + MongoDB)

### Step 1. IoT Device Sends Message
For this example the IoT Device will send 2 messages - one of Temperature type and one of Humidity Type

```
{
	"SensorId": 123456,
	"Type": 1,
	"Value": 22.5,
	"Timestamp": "2025-10-20T14:30:00.123456"
	"RawMessage": "{...}"
}

{
	"SensorId": 123456,
	"Type": 2,
	"Value": 60.0,
	"Timestamp": "2025-10-20T14:30:00.123456"
	"RawMessage": "{...}"
}

{
	"SensorId": 123456,
	"Type": 5,
	"Value": 1013.25
	"Timestamp": "2025-10-20T14:30:00.123456"
	"RawMessage": "{...}"
}
```
**Component**: Raspberry Pi (data simulated by a python script)
**Action**: Sends JSON message to Azure IoT Hub


### Step 2. Azure IoT Hub Receives and Routes Message

**Component**: Azure IoT Hub (cloud service)

**Action**:
- Authenticates the Raspberry Pi device using its connection string
- Receives the JSON messages 
- Routes messages to the built-in EventHub-compatible endpoint

**Result**: Messages now available in EventHub for the application to consume

### Step 3. EventHub Stores Message

**Component**: Azure IoT Hub Built-in EventHub Endpoint (cloud service)

**Action**:
- Receives messages from IoT Hub's EventHub-compatible endpoint
- Distributes messages across 2 partitions 
- Stores messages temporarily until consumed by the application

**Result**: Messages are partitioned and ready for the .NET application to read


### Step 4. EventHubConsumer Reads Message

**Component**: EventHubConsumer.cs

**Action**:
- Connects to IoT Hub Endpoint using connection string from configuration
- Reads from ALL partitions concurrently (not sequentially)
- Merges messages from both partitions using channels
- Returns raw JSON message strings

**Result**: Stream of raw JSON messages ready for the coordinator to process

**Dependencies:** IAppConfiguration


### Step 5. MessageCoordinator Delegates

**Component**: MessageCoordinator.cs

**Action**:
- Consumes messages from EventHubConsumer
- Delegates conversion to JsonMessageDeserializer 
- Passes deserialized object to MessageProcessor


**Dependencies:** IEventHubConsumer, IMessageDeserializer, IMessageProcessor


### Step 6. JsonMessageDeserializer Converts

**Component**: JsonMessageDeserializer.cs

**Action**:
- Takes raw Json message
- Deserializes Json message and returns a SensorReading Object

### Step. 7 SensorMessageProcessor Selects Strategy for each Message 

**Component**: MessageProcessor.cs 

**Action**:
- Selects appropriate Handler for SensorReading
- Delegates processing of each reading to Handler 
- Passes SensorReading to CompositeOutputService 


**Dependencies:** List\<SensorHandler>, IOutputService 

### Step 8. Handlers Process Readings

**Note:** This is currently "redundant code" but has been retained to demonstrate where the entry point for additional business logic, such as calculations, alerting etc, would be.


### Step 9. CompositeOutputService Distributes

**Component**: CompositeOutputService.cs

**Action:** 
- Receives SensorReading from MessageProcessor which treats it as a single outputservice
- Distributes to all registered output services


**Dependencies:** a Collection of IOutputService

### Step 10a. ConsoleOutputService 

**Component**: CompositeOutputService.cs

**Action:** 
- Recieves SensorReading
- Selects the correct formatter based on ReadingType
- Delegates Formatting to the chosen Formatter which returns human readable string
- Writes the formatted output to Console


**Example Console Outputs:**
```
Sensor 123456 | [Temperature] 20/10/2025 2:30:00 PM: 22.5
Sensor 123456 | [Humidity] 20/10/2025 2:30:00 PM: 60.0
Unknown sensor type 5, setting to Unknown
Sensor 123456 | [Unknown Sensor Type] 20/10/2025 2:30:00 PM: 1013.25 (Raw: {"SensorId": 123456, "Type": 5, "Value": 1013.25, "TimeStamp": "2025-10-20T14:30:00.123456", "RawMessage": "{...}"})
```

**Dependencies:** a Collection of ISensorFormatter

### Step 10b. DbOutputService

**Component**: CompositeOutputService.cs

**Action:** 
- Receives SensorReading
- Delegates Formatting to JsonSensorFormatter
- Creates a MongoDB Document and Inserts it into MongoDB

**Example MongoDB Documents:**
```
{
	"_id": ObjectID("..."),
	"type": "Temperature",
	"value": 22.5,
	"timestamp": "2025-10-20T14:30:00.123Z",
	"rawMessage": "{...}",
	"insertedAt": "2025-10-20T14:30:00.789Z"
}

{
	"_id": ObjectID("..."),
	"type": "Humidity",
	"value": 60.0,
	"timestamp": "2025-10-20T14:30:00.123Z",
	"rawMessage": "{...}",
	"insertedAt": "2025-10-20T14:30:00.789Z"
}

{
	"_id": ObjectID("..."),
	"type": "Humidity",
	"value": 1013.25,
	"timestamp": "2025-10-20T14:30:00.123Z",
	"rawMessage": "{...}",
	"insertedAt": "2025-10-20T14:30:00.789Z"
}
```

**Dependencies:** a Collection of ISensorFormatter


## Example of Application Running

The following is a brief, high level, example of the application running to demonstrate outputs. 
Assuming that the following setup has been completed:

An Azure IoT Hub has been deployed and the raspberry pi device registered. 
With the EventHub compatiable connection string being added to applications enviroment variables:
![picture](readmepictures/event_hub_endpoint_string.png)

The Raspberry Pi has been set up with an OS and provided the python sensor data script (send_messages.py): 
![picture](readmepictures/pi.png)

And the device connection string added to the raspberry pi env variables for the script to connect:
![picture](readmepictures/iothub_device_string.png)

**Step 1. Build the .Net Application** 
![picture](readmepictures/buildapp.png)

**Step 2. Run the .Net Application** 
![picture](readmepictures/runapp.png)

**Step 3. Send Sensor data from Raspberry Pi device** 
![picture](readmepictures/send_messages_from_pi.png)

**Results** 
**Console Output:** 
![picture](readmepictures/app_console_output.png)
**MongoDB documents:** 
![picture](readmepictures/mongodb_documents.png)


## Designed to Support Extension 
This application was designed to support easy extension, without code changes - for example:

### Adding new sensor types
To add a new sensor type - just one enum needs to be added and two implementations, a Handler and a Formatter:


```csharp
// First add a new enum Models/SensorType.cs
public enum SensorType
{
    Unknown = 0,
    Temperature = 1,
    Humidity = 2,
    Pressure = 3        // New type
}

// Then create a new Formatter Formatting/PressureFormatter.cs
public class PressureFormatter : ISensorFormatter
{
    public bool CanFormat(SensorReading reading)
    {
        return reading.Type == SensorType.Pressure;
    }

    public string Format(SensorReading reading)
    {
        return $"Sensor {reading.SensorId} | [Pressure] {reading.TimeStamp}: {reading.Value} hPa";
    }
}

// And finally add a Handler Sensors/PressureReadingHandler.cs
public class PressureReadingHandler : ISensorReadingHandler
{
    public bool CanHandle(SensorReading reading)
    {
        return reading.Type == SensorType.Pressure;
    }

    public string Handle(SensorReading reading)
    {
        // Add any pressure-specific business logic here
    }
}
```


### Adding a new output type (i.e. SQL. DB)
A new output type would just require an additional implementation of both IOutputService and IDbConfiguration

```csharp
// First create the new SQL Output Service Output/SqlOutputService.cs
public class SqlOutputService : IOutputService
{
    private readonly string _connectionString;
    private readonly ISensorFormatter _formatter;

    public SqlOutputService(string connectionString, ISensorFormatter formatter)
    {
        _connectionString = connectionString;
        _formatter = formatter;
    }

    public async Task WriteAsync(SensorReading reading)
    {
        var json = _formatter.Format(reading);
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = new SqlCommand(
            "INSERT INTO SensorReadings (Type, Value, Timestamp, RawMessage) VALUES (@Type, @Value, @Timestamp, @RawMessage)",
            connection);
        
        command.Parameters.AddWithValue("@Type", reading.Type.ToString());
        command.Parameters.AddWithValue("@Value", reading.Value);
        command.Parameters.AddWithValue("@Timestamp", reading.TimeStamp);
        command.Parameters.AddWithValue("@RawMessage", reading.RawMessage ?? "");
        
        await command.ExecuteNonQueryAsync();
    }
}

// and create an Interface and Implementation to provide the required config values for SQL
// Configuration/ISqlConfiguration.cs
public interface ISqlConfiguration
{
    string ConnectionString { get; }
    string TableName { get; }
}

// Configuration/SqlConfiguration.cs
public class SqlConfiguration : ISqlConfiguration
{
    public string ConnectionString { get; }
    public string TableName { get; }

    public SqlConfiguration()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
            ?? throw new InvalidOperationException("SQL_CONNECTION_STRING environment variable not set");
        
        TableName = Environment.GetEnvironmentVariable("SQL_TABLE_NAME")
            ?? "SensorReadings";  // Default table name
    }
}

// Finally this would just need to be added to Program.cs and injected into CompositeOutputService
var sqlConfig = new SqlConfiguration();
var sqlOutputService = new SqlOutputService(sqlConfig, jsonFormatter);

var compositeOutputService = new CompositeOutputService(new List<IOutputService>
{
    consoleOutputService,
    dbOutputService,
    sqlOutputService        
});

```



# Testing 

This project utilized the following testing types
- Unit testing
- Integration testing
- End-to-end testing