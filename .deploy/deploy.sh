ApiKey=$1

dotnet pack CoreZipCode.sln -v detailed

dotnet nuget push CoreZipCode/bin/Debug/CoreZipCode.*.nupkg --api-key $ApiKey --source https://api.nuget.org/v3/index.json
