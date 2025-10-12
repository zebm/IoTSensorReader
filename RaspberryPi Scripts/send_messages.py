import os
import json
from datetime import datetime
from azure.iot.device import IoTHubDeviceClient, Message

connection_string = os.getenv("IOTHUB_DEVICE_CONNECTION_STRING")

if not connection_string:
    raise EnvironmentError("IOTHUB_DEVICE_CONNECTION_STRING env variable not found")

client = IoTHubDeviceClient.create_from_connection_string(connection_string)

messages = [
    {"SensorId": 123456, "Type" : 1, "Value": "22.5"},
    {"SensorId": 123456, "Type" : 2, "Value": "30"},
    {"SensorId": 123456, "Type" : 1, "Value": "22.7"},
    {"SensorId": 123456, "Type" : 2, "Value": "29.98"},
    {"SensorId": 123456, "Type" : 1, "Value": "22.6"},
    {"SensorId": 123456, "Type" : 2, "Value": "30.1"},
]

for msg in messages:
    msg["TimeStamp"] = datetime.now().isoformat()
    msg["RawMessage"] = json.dumps(msg)
    message = Message(json.dumps(msg))
    print(f"Sending message: {msg}")
    client.send_message(message)