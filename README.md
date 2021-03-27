# Hammer

Currently, Hammer is a pre-alpha Windows app for getting FCC data about amateur callsigns using the <https://callook.info/> JSON API. Other utilities (TBD) may come later as needed.

The ultimate goal is platform-agnostic amateur radio utility library and UIs that leverage it, either platform-specific or using a technology like .NET MAUI (Multi-platform App User Interface).

## Components

Hammer is separated into components, each a .NET project within the larger .NET solution:

1. `Hammer.Core`: A shared multi-platform .NET library.
2. `Hammer`: A C#/XAML Universal Windows Platform (UWP) app containing the Windows UI and Windows-specific functionality.
3. `Hammer.CLI`: A .NET Console app that allows the library to be leveraged from the command line.
4. `HammerTest`: Unit tests for the Hammer library and applications.

## Development environment

Visual Studio 2019, Visual Studio for Mac, or Visual Studio Code is recommended.

For working with the main body of code, the `Hammer.Core` library, you will need:

* [.NET Core 3.1 or later](https://dotnet.microsoft.com/download)

If you’re working with the `Hammer` Windows/UWP app, you’ll need:

* Windows 10, version 1903 (build 18362) or later (current version recommended)
* [Windows 10 SDK, version 1903 (10.0.18362.1)](https://developer.microsoft.com/en-us/windows/downloads/sdk-archive/)
