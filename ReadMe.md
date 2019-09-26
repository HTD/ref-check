# ref-check
Solution reference conflict checker

Distributed under [MIT License](https://en.wikipedia.org/wiki/MIT_License).
(c)2019 by CodeDog Ltd. All rights reserved.

## About

> Found conflicts between different versions of the same dependent assembly. Please set the "AutoGenerateBindingRedirects" property to true in the project file. For more information, see http://go.microsoft.com/fwlink/?LinkId=294190.

Looks familiar? Wait.

**Do not** set `AutoGenerateBindingRedirects`! That will terribly break your solution
indroducing insane dependency hell and unexpected `FileNotFoundException`s.

Check which references really conflict with each other with this tool.

Inspired with https://gist.github.com/brianlow/1553265 .

## Prerequisites

- .NET Core 3.0 ([Download SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0))

## Installation

- Publish with `FolderProfile` provided.
- Copy all files from `publish` directory into any directory accessible from `PATH`.

## Usage

```
ref-check [("x64"|"x86"|"all")] [("debug"|"release")] [optional-path]
```

## Disclaimer

Made for .NET Core 3.0 because it's cool. And... Cross-platform ;)
It won't work for any possible solution configuration.
The assembly selection algorithm is braindead simple in this version.
Default names only (this time).

BTW, I wondered how to make an executable tool using .NET Core. That's how.