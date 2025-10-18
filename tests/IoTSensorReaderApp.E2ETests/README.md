# E2E Test

## Running the Test

```bash
./run-e2e-test.sh
```

## What it Tests

- Sends 6 messages (3 temperature, 3 humidity) to IoT Hub
- Verifies messages are processed through EventHub consumer
- Checks console output formatting
- Checks MongoDB document storage

## Expected Output

When the test passes, you should see:

```
  Passed EndToEndTest_SendTemperatureAndHumidityReadings_ShouldProcessThroughEntireSystem [17 s]
  Standard Output Messages:
 Starting E2E test - Initial MongoDB docu count: 0
 Starting EventHub consumer...
 Waiting 15 seconds for consumer to start listening
 MessageCoordinator listening started
 Consumer should be ready - start sending messages
 Sending Temperature: 22.5
 Temperature message 1 sent
 Sending Humidity: 30
 Humidity message 2 sent
 Sending Temperature: 22.7
 Temperature message 3 sent
 Sending Humidity: 29.98
 Humidity message 4 sent
 Sending Temperature: 22.6
 Temperature message 5 sent
 Sending Humidity: 30.1
 Humidity message 6 sent
 All messages sent - start processing
 Waiting for 6 total documents (current: 0)
 Document wait result: True
 MessageCoordinator error: The operation was canceled.
 MessageCoordinator cancelled
 Checking console output...
 Console output length: 429 characters
 Console output:
   Sensor 123456 | [Temperature] 18/10/2025 11:25:21 PM: 22.5°C]
   Sensor 123456 | [Humidity] 18/10/2025 11:25:22 PM: 30%]
   Sensor 123456 | [Temperature] 18/10/2025 11:25:22 PM: 22.7°C]
   Sensor 123456 | [Humidity] 18/10/2025 11:25:22 PM: 29.98%]
   Sensor 123456 | [Temperature] 18/10/2025 11:25:22 PM: 22.6°C]
   Sensor 123456 | [Humidity] 18/10/2025 11:25:22 PM: 30.1%]
   Partition task has been cancelled.
   Partition task has been cancelled.
 Checking MongoDB for expected documents
 Found 3 temperature documents:
   Temperature: { "_id" : { "$oid" : "68f421e2e64e75f17698acbd" }, "type" : "Temperature", "value" : 22.5, "timestamp" : "2025-10-18T23:25:21.572041", "rawMessage" : "{\"SensorId\":123456,\"Type\":1,\"Value\":22.5,\"TimeStamp\":\"2025-10-18T23:25:21.572041\",\"RawMessage\":\"{\\\"SensorId\\\":123456,\\\"Type\\\":1,\\\"Value\\\":22.5,\\\"TimeStamp\\\":\\\"2025-10-18T23:25:21.572041\\\"}\"}", "insertedAt" : { "$date" : "2025-10-18T23:25:22.177Z" } }
   Temperature: { "_id" : { "$oid" : "68f421e2e64e75f17698acbf" }, "type" : "Temperature", "value" : 22.699999999999999, "timestamp" : "2025-10-18T23:25:22.284743", "rawMessage" : "{\"SensorId\":123456,\"Type\":1,\"Value\":22.7,\"TimeStamp\":\"2025-10-18T23:25:22.284743\",\"RawMessage\":\"{\\\"SensorId\\\":123456,\\\"Type\\\":1,\\\"Value\\\":22.7,\\\"TimeStamp\\\":\\\"2025-10-18T23:25:22.284743\\\"}\"}", "insertedAt" : { "$date" : "2025-10-18T23:25:22.451Z" } }
   Temperature: { "_id" : { "$oid" : "68f421e2e64e75f17698acc1" }, "type" : "Temperature", "value" : 22.600000000000001, "timestamp" : "2025-10-18T23:25:22.574", "rawMessage" : "{\"SensorId\":123456,\"Type\":1,\"Value\":22.6,\"TimeStamp\":\"2025-10-18T23:25:22.574000\",\"RawMessage\":\"{\\\"SensorId\\\":123456,\\\"Type\\\":1,\\\"Value\\\":22.6,\\\"TimeStamp\\\":\\\"2025-10-18T23:25:22.574000\\\"}\"}", "insertedAt" : { "$date" : "2025-10-18T23:25:22.737Z" } }
 Found 3 humidity documents:
   Humidity: { "_id" : { "$oid" : "68f421e2e64e75f17698acbe" }, "type" : "Humidity", "value" : 30, "timestamp" : "2025-10-18T23:25:22.125557", "rawMessage" : "{\"SensorId\":123456,\"Type\":2,\"Value\":30.0,\"TimeStamp\":\"2025-10-18T23:25:22.125557\",\"RawMessage\":\"{\\\"SensorId\\\":123456,\\\"Type\\\":2,\\\"Value\\\":30.0,\\\"TimeStamp\\\":\\\"2025-10-18T23:25:22.125557\\\"}\"}", "insertedAt" : { "$date" : "2025-10-18T23:25:22.306Z" } }
   Humidity: { "_id" : { "$oid" : "68f421e2e64e75f17698acc0" }, "type" : "Humidity", "value" : 29.98, "timestamp" : "2025-10-18T23:25:22.430111", "rawMessage" : "{\"SensorId\":123456,\"Type\":2,\"Value\":29.98,\"TimeStamp\":\"2025-10-18T23:25:22.430111\",\"RawMessage\":\"{\\\"SensorId\\\":123456,\\\"Type\\\":2,\\\"Value\\\":29.98,\\\"TimeStamp\\\":\\\"2025-10-18T23:25:22.430111\\\"}\"}", "insertedAt" : { "$date" : "2025-10-18T23:25:22.594Z" } }
   Humidity: { "_id" : { "$oid" : "68f421e2e64e75f17698acc2" }, "type" : "Humidity", "value" : 30.100000000000001, "timestamp" : "2025-10-18T23:25:22.716815", "rawMessage" : "{\"SensorId\":123456,\"Type\":2,\"Value\":30.1,\"TimeStamp\":\"2025-10-18T23:25:22.716815\",\"RawMessage\":\"{\\\"SensorId\\\":123456,\\\"Type\\\":2,\\\"Value\\\":30.1,\\\"TimeStamp\\\":\\\"2025-10-18T23:25:22.716815\\\"}\"}", "insertedAt" : { "$date" : "2025-10-18T23:25:22.9Z" } }
 E2E Test completed successfully



Test Run Successful.
Total tests: 1
     Passed: 1
 Total time: 17.8584 Seconds
  IoTSensorReaderApp.E2ETests test succeeded (18.0s)

Test summary: total: 1, failed: 0, succeeded: 1, skipped: 0, duration: 18.0s
```
