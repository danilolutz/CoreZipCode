# Extending CoreZipCode

You can extends `ZipCodeBaseService` abstract class and create your own implementation of your prefered address service. Like below:

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