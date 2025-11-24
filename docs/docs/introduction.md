# Introduction

Normaly we must implement ZipCode or Postcode services every time in each new software we create. Well this package is for keep you [DRY](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself) and eliminate the necessity to implement ZipCode or Postcode services over and over.

Also this package could be used for easily implements address services to yours [Microsoft .NET](https://dotnet.github.io/) based software.

And the **CoreZipCode** was designed to be easily extensible, and if you want, implement your own address services, you only must override the API calls methods.

## Concept and flow

- `*BaseService` are classes with reusable predefined methods.
- `ApiHandler` is a `HttpClient` adapter witch is consumed by `*BaseService` classes.

A step-by-step guide:

1. `CustomZipCodeService` extends `ZipCodeBaseService` and define how API URLs must be treated via abstract methods.
2. `ZipCodeBaseService` builds the URL and execute some validations (or not, it's up to you).
3. `ZipCodeBaseService` calls `ApiHandler`, witch do a request to external desired API and do some response validations.

Nothing like a diagram to simplify the explanation:

![CoreZipCode flow](../images/flow.svg)

