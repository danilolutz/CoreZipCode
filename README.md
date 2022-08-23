# CoreZipCode

[![Build Status](https://travis-ci.com/danilolutz/CoreZipCode.svg?branch=master)](https://travis-ci.com/danilolutz/CoreZipCode)
[![Coverage Status](https://coveralls.io/repos/github/danilolutz/CoreZipCode/badge.svg?branch=master)](https://coveralls.io/github/danilolutz/CoreZipCode?branch=master)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/e80743d2e8d8415dbc03cb459a3e8639)](https://www.codacy.com/app/danilolutz/CoreZipCode?utm_source=github.com&utm_medium=referral&utm_content=danilolutz/CoreZipCode&utm_campaign=Badge_Grade)
[![Known Vulnerabilities](https://snyk.io/test/github/danilolutz/CoreZipCode/badge.svg?targetFile=CoreZipCode%2FCoreZipCode.csproj)](https://snyk.io/test/github/danilolutz/CoreZipCode?targetFile=CoreZipCode%2FCoreZipCode.csproj)
[![CoreZipCode Nuget Package](https://img.shields.io/nuget/v/CoreZipCode.svg)](https://www.nuget.org/packages/CoreZipCode/) [![License: MIT](https://img.shields.io/badge/License-MIT-428f7e.svg)](https://opensource.org/licenses/MIT)

## Overview

Normaly we must implement ZipCode or Postcode services every time in each new software we create. Well this package is for keep you [DRY](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself) and eliminate the necessity to implement ZipCode or Postcode services over and over.

Also this package could be used for easily implements address services to yours [Microsoft .Net Core](https://dotnet.github.io/) based software.

And the **CoreZipCode** was designed to be easily extensible, and if you want, implement your own address services, you only must override the API calls methods.

We follow the [Semantic Versioning](https://semver.org), so check the package compatibility before use it.

## :sunglasses: Get Started

Well, you can download the code and build it by yourself or you can install by [Nuget](https://www.nuget.org) package in: [CoreZipCode Package](https://www.nuget.org/packages/CoreZipCode/).

Just in case if you doesn't want leave GitHub at this moment:

```bash
dotnet add package CoreZipCode
```

After you decide how you'll get the CoreZipCode, just inject (e.g.: [Simple Injector](https://simpleinjector.org/)) and use it.

```CSharp
using CoreZipCode.Interfaces;

namespace YourProject
{
    public YourClass
    {
        private readonly ZipCodeBaseService _coreZipCode;

        public YourClass(ZipCodeBaseService coreZipCode)
        {
            _coreZipCode = coreZipCode;
        }

        public void YourMethod()
        {
            var addressByZipCode = _coreZipCode.Execute("14810100");
            var zipCodeByAddress = _coreZipCode.Execute("sp", "araraquara", "barão do rio");

            // Generic type return was added in version 1.1.0
            var addressByZipCodeObject = _coreZipCode.GetAddress<ViaCepAddress>("14810100");
            var zipCodeByAddressObjectList = _coreZipCode.ListAddresses<ViaCepAddress>("sp", "araraquara", "barão do rio");
        }

        // Async methods introduced in 1.1.0
        public async void YourMethodAsync()
        {
            var addressByZipCode = await _coreZipCode.ExecuteAsync("14810100");
            var zipCodeByAddress = await _coreZipCode.ExecuteAsync("sp", "araraquara", "barão do rio");

            // Generic type return.
            var addressByZipCodeObject = await _coreZipCode.GetAddressAsync<ViaCepAddress>("14810100");
            var zipCodeByAddressObjectList = await _coreZipCode.ListAddressesAsync<ViaCepAddress>("sp", "araraquara", "barão do rio");
        }
    }
}
```

The `ViaCepAddress` POCO class is the type for returned JSON from [ViaCep](https://viacep.com.br) brazilian service. So you will must to implement the POCO class for your new service.

### Extending CoreZipCode

Also you can extends `ZipCodeBaseService` abstract class and create your own implementation of your prefered address service. Like below:

```CSharp
using CoreZipCode.Interfaces;

namespace CoreZipCode.Services.YourService
{
    public class YourService : ZipCodeBaseService
    {
        public override string SetZipCodeUrl(string zipcode)
        {
            // You can implement some validation method here.
            return $"https://yourservice.com/{zipcode}/json/";
        }

        public override string SetZipCodeUrlBy(string state, string city, string street)
        {
            // You can implement some validation method here.
            return $"https://yourservice.com/{state}/{city}/{street}/json/";
        }
    }
}
```

> **NOTE**: Same principles are used to extends postcode lookups (`PostCodeBaseService`).

## :heavy_check_mark: Available Services

Below a list of available services out-of-the-box **address by zipcode** lookup services.

| Service                                     | Country | Queries Limit     |
| :------------------------------------------ | :------ | :---------------- |
| [ViaCep](https://viacep.com.br)             | Brazil  | 300 by 15 minutes |
| [Smarty](https://www.smarty.com/)           | USA     | 250 by month      |

Below a list of available services out-of-the-box **postcodes** lookup services.

| Service                                    | Country        | Queries Limit |
| :----------------------------------------- | :------------- | :------------ |
| [Postcodes](https://postcodes.io)          | United Kingdom | Unknown       |
| [Postal Pin Code](http://postalpincode.in) | India          | Unknown       |

## :construction_worker: Contributing

Thank you for considering contributing to the CodeZipCore! Just see our [Contributing Guide](.github/CONTRIBUTING.md).

### :innocent: Code of Conduct

In order to ensure that the CodeZipCore community is welcoming to all, please review and abide by the [Code of Conduct](.github/CODE_OF_CONDUCT.md).

## :rotating_light: Security Vulnerabilities

If you discover any security vulnerability within CoreZipCode, please [create a vulnerability issue](https://github.com/danilolutz/CoreZipCode/issues/new?labels=security%20vulnerabilities). All security vulnerabilities will be promptly addressed.

## :scroll: License

The CoreZipCode is open-sourced software licensed under the [MIT license](https://opensource.org/licenses/MIT).
