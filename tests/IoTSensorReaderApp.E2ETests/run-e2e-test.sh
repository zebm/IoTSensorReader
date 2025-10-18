#!/bin/bash
cd "$(dirname "$0")/../.."
export $(grep -v '^#' .env | xargs)
dotnet test tests/IoTSensorReaderApp.E2ETests/IoTSensorReaderApp.E2ETests.csproj --filter "EndToEndTest_SendTemperatureAndHumidityReadings_ShouldProcessThroughEntireSystem" --logger "console;verbosity=detailed"