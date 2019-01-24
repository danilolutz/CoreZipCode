workflow "Unit Tests" {
  on = "push"
  resolves = ["docker://microsoft/dotnet"]
}

action "docker://microsoft/dotnet" {
  uses = "docker://microsoft/dotnet"
  runs = "dotnet test"
  args = [
    "CoreZipCode.Tests/CoreZipCode.Tests.csproj",
    "-p:CollectCoverage=true",
    "-p:CoverletOutputFormat=cobertura",
    "-p:CoverletOutput=\".coverage/coverage.cobertura.xml\"",
    "-p:Exclude=\"[CoreZipCode.Tests*]*\"",
    "-p:Exclude=\"[xunit*]*\""
  ]
}
