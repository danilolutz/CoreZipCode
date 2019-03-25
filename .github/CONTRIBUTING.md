# :construction_worker: Contributing to CoreZipCode

Thank you for considering contributing to the CoreZipCode! Welcome to our Contribution Guide.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Reporting an Issue](#reporting-an-issue)
3. [Cloning the repository](#cloning-the-repository)
4. [Testing](#testing)

## Getting Started

If you don't want contribute with code, but want to help us Reporting Issues, just skip to [Reporting an Issue](#reporting-an-issue) section.

However, if you prefer to work with code, simply go to [Cloning the repository](#cloning-the-repository) section and continue to the end of this document.

We follow (or try to) this code architecture principles:

- [OOP](https://en.wikipedia.org/wiki/Object-oriented_programming)
- [SOLID](https://en.wikipedia.org/wiki/SOLID)
- [Design Patterns](https://en.wikipedia.org/wiki/Software_design_pattern)
- [DRY - Don't Repeat Yourself](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself)
- [KISS - Keep It Simple, Stupid](https://en.wikipedia.org/wiki/KISS_principle)
- [YAGNI - You Aren't Gonna Need It](https://en.wikipedia.org/wiki/You_aren%27t_gonna_need_it)

## Reporting an Issue

1. Describe what you expected to happen and what actually happens.

2. If possible, include a minimal but complete example to help us reproduce the issue.

3. We'll try to fix it as soon as possible but be in mind that CoreZipCode is open source and you can probably submit a pull request to fix it even faster.

4. Just [open you issue](https://github.com/danilolutz/CoreZipCode/issues/new).

## Cloning the repository

First of all you'll need some tools like [Microsoft .Net Core](https://dotnet.github.io/) and [Git](https://git-scm.com/) to get your development environment working, as code editor we used [Visual Studio Code](https://code.visualstudio.com/), but it's only a suggestion, use whatever code editor you want.

You can clone by https:

```bash
git clone https://github.com/danilolutz/CoreZipCode.git
```

Or by SSH:

```bash
git clone git@github.com:danilolutz/CoreZipCode.git
```

Whatever you prefer. Clone the repo and start your work following this steps (aka gitflow):

1. :wrench: Create a new branch based on **develop**
2. :white_check_mark: Name you branch how you prefer BUT **develop** or **master**
3. :art: Do the best of your art
4. :pencil: Make commit with a clear message
5. :ok_hand: Open a pull request from your new branch to **develop**

We usually use [TDD](https://pt.wikipedia.org/wiki/Test_Driven_Development) as development workflow, just a suggestion.

Maybe you ask yourself: _How can I contribute?_ or _What i can do?_

We'll help you:

- You can implement you favorite zipcode or postcode service and submit to CoreZipCode to be an out-of-the-box service;
- You can find and fix bugs;
- Implement openned issues;
- Improve the CoreZipCode programming.

**Only thing will be required by us to approve the pull requests are the unit tests (with correct code coverage) for the new implementations**.

## Testing

You already know how it's works, just add you test class in `CoreZipCode.Tests` project, implement it, run the unit tests, be sure everything is okay and submit to repo.

To run the tests go to solution folder and execute:

```bash
dotnet test
```

If you want to see how the code coverage is going, just execute:

```bash
dotnet test CoreZipCode.Tests/CoreZipCode.Tests.csproj -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput=".coverage/coverage.opencover.xml" -p:Exclude="[CoreZipCode.Tests*]*" -p:Exclude="[xunit*]*"

reportgenerator -reports:CoreZipCode.Tests/.coverage/coverage.opencover.xml -targetdir:CoreZipCode.Tests/.coverage
```

And open the file `CoreZipCode.Tests/.coverage/index.htm` to see how the unit tests are affecting the code coverage.

> **NOTE**: You need to have [ReportGenerator](https://www.nuget.org/packages/dotnet-reportgenerator-globaltool) installed. Use the follow command: `dotnet tool install -g dotnet-reportgenerator-globaltool` to install it globally.
