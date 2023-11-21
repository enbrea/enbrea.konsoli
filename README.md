[![NuGet Gallery](https://img.shields.io/badge/NuGet%20Gallery-enbrea.konsoli-blue.svg)](https://www.nuget.org/packages/Enbrea.Konsoli/)
![GitHub](https://img.shields.io/github/license/enbrea/enbrea.konsoli)

# ENBREA Konsoli

A small .NET library for displaying nice looking messages and visualising progress in console applications:

+ Supports .NET 6, .NET 7 and .NET 8
+ Supports callouts (information, success, warning, error)
+ Supports progress visualisation with different progress units:
  + Percent
  + Count
  + FileSize
  + and additionally custom formatted values
+ Supports [output redirection](https://learn.microsoft.com/en-us/dotnet/api/Console.isoutputredirected).
+ Supports logging via [ILogger abstraction](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger).

It looks like this:

![GIF Animation: ENBREA Konsoli in action](img/screenshots/readme-animation.gif)

## Installation

```
dotnet add package Enbrea.Konsoli
```

## Getting started

Documentation is available in the [GitHub wiki](https://github.com/enbrea/enbrea.konsoli/wiki).

## Can I help?

Yes, that would be much appreciated. The best way to help is to post a response via the Issue Tracker and/or submit a Pull Request.
