#!/bin/bash

set -e

dotnet nuget push CoreZipCode/bin/Release/CoreZipCode.${TRAVIS_TAG#v}.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json
