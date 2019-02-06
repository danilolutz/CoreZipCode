ApiKey=$1
PackageVersion=$2

dotnet pack CoreZipCode.sln -v detailed -p:PackageVersion=$PackageVersion

dotnet nuget push CoreZipCode/bin/Debug/CoreZipCode.$PackageVersion.nupkg --api-key $ApiKey --source https://api.nuget.org/v3/index.json
