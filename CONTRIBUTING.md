# :construction_worker: Contributing to CoreZipCode

Thank you for considering contributing to the CoreZipCode! Welcome to our Contribution Guide.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Cloning the repository](#cloning-the-repository)
3. [Testing](#testing)
4. [Reporting an Issue](#reporting-an-issue)

## Getting Started

First of all you need [Microsoft .Net Core](https://dotnet.github.io/) and [Git](https://git-scm.com/), as editor we use [Visual Studio Code](https://code.visualstudio.com/), but it's only a suggestion.

## Cloning the repository

You can clone by https:

```bash
git clone https://github.com/danilolutz/CoreZipCode.git
```

Or by SSH:

```bash
git clone git@github.com:danilolutz/CoreZipCode.git
```

What you prefer. Clone the repo and start your work following this steps: 

1. :wrench: Create a new branch based on master 
2. :white_check_mark: Name you branch how you prefer BUT **develop** or **master**
3. :art: Do the best of your art
4. :pencil: Make commit with a clear message
5. :ok_hand: Open a pull request from you new branch to master

We usually use [TDD](https://pt.wikipedia.org/wiki/Test_Driven_Development) as development process, just a suggestion. 

Maybe you ask yourself: _How can I contribute?_ or _What i can do?_ 

We'll help you:

* You can implement you favorite address service and submit to CoreZipCode to be an out-of-the-box service;
* You can find and fix bugs;
* Implement openned issues;
* Improve the CoreZipCode programming;.

**Only thing will be required by us to approve the pull requests are the unit tests for the new implementations**.

## Testing

You already know how it's works, just add you test class in ```CoreZipCode.Tests``` project implement it, run your tests be sure everything is okay and submit to repo.

## Reporting an Issue

1. Describe what you expected to happen and what actually happens.

2. If possible, include a minimal but complete example to help us reproduce the issue.

3. We'll try to fix it as soon as possible but be in mind that CoreZipCode is open source and you can probably submit a pull request to fix it even faster.

4. Just [open you issue](https://github.com/danilolutz/CoreZipCode/issues/new).