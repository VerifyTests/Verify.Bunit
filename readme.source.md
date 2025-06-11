# <img src="/src/icon.png" height="30px"> Verify.Bunit

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/spyere4ubpl1tca8?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Bunit)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Bunit.svg?label=Verify.Bunit)](https://www.nuget.org/packages/Verify.Bunit/)

Support for rendering a [Blazor Component](https://docs.microsoft.com/en-us/aspnet/core/blazor/#components) to a verified file via [bunit](https://bunit.egilhansen.com). Verify.Bunit uses the bUnit APIs to take a snapshot (metadata and html) of the current state of a Blazor component. Since it leverages the bUnit API, snapshots can be on a component that has been manipulated using the full bUnit feature set, for example [trigger event handlers](https://bunit.egilhansen.com/docs/interaction/trigger-event-handlers.html).

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors

include: zzz


## Component

The below samples use the following Component:

snippet: BlazorApp/TestComponent.razor


## NuGet

* https://nuget.org/packages/Verify.Bunit


## Usage

Enable at startup:

snippet: BunitEnable

This test:

snippet: BunitComponentTest

Will produce:

The component rendered as html `...Component.verified.html`:

snippet: Tests/Samples.Component.verified.html

And the current model rendered as txt `...Component.verified.txt`:

snippet: Tests/Samples.Component.verified.txt


## Exclude Component

Rendering of the Component state (Samples.Component.verified.txt from above) can be excluded by
using `excludeComponent`.

snippet: BunitEnableExcludeComponent


## Scrubbing


## Integrity check

In Blazor an integrity check is applied to the `dotnet.*.js` file.

```
<script src="_framework/dotnet.5.0.2.js" defer="" integrity="sha256-AQfZ6sKmq4EzOxN3pymKJ1nlGQaneN66/2mcbArnIJ8=" crossorigin="anonymous"></script>
```

This line will change when the dotnet SDK is updated.


## Noise in rendered template

Blazor uses `<!--!-->` to delineate components in the resulting html. Some empty lines can be rendered when components are stitched together.


## Resulting scrubbing

snippet: scrubbers


## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed
by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).