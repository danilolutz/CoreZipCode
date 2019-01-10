workflow "Unit Tests" {
  on = "push"
  resolves = ["docker://microsoft/dotnet"]
}

action "docker://microsoft/dotnet" {
  uses = "docker://microsoft/dotnet"
  runs = "dotnet build"
  args = "dotnet test"
}
