# Quick Start

You can download the code and build it by yourself or you can install by [Nuget](https://www.nuget.org) package in: [CoreZipCode Package](https://www.nuget.org/packages/CoreZipCode/).

Just in case if you doesn't want leave GitHub at this moment:

```bash
dotnet add package CoreZipCode
```

## Quickest example

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

You can check [Program.cs](https://github.com/danilolutz/CoreZipCode/blob/main/SampleApp/Program.cs) to more robust example.
