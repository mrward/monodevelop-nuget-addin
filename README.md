# NuGet Package Management addin for MonoDevelop

This is a port of the [SharpDevelop NuGet package management addin](http://community.sharpdevelop.net/blogs/mattward/archive/2011/01/23/NuGetSupportInSharpDevelop.aspx) that works with MonoDevelop under Windows and Linux.

It uses a custom build of [NuGet.Core.dll](https://github.com/mrward/nuget/tree/monodevelop), based on source code taken from the original mono-build branch available from [CodePlex](http://nuget.codeplex.com), and also some code from the latest version of Mono runtime libraries.

# Requirements

 * MonoDevelop 3.0.5
 * Mono 2.10.9 or above
 * mono-winfxcore (required by NuGet.Core)
 * mono-wcf (required by NuGet.Core)

#License

[MIT](http://opensource.org/licenses/MIT)

Original SharpDevelop addin code is [LGPL](http://www.gnu.org/licenses/lgpl-2.1.txt).

#Environments Tested

 * OpenSuse 12.2
 * Windows 7
 * Mono 2.10.9

#Issues

## Addin fails to load with Type Load Exception

If the addin fails to load with a "type load exception has occurred" error message then check the required mono libraries are installed. Open NuGet.Core.dll into MonoDevelop's Assembly Browser and check the assemblies it references are installed. The Assembly Browser should show the missing assemblies.

NuGet.Core.dll depends on

 1.  System.Data.Services.Client v4.0.0.0
 2. WindowsBase v4.0.0.0
  
WindowsBase is in the mono-winfxcore package for OpenSuse. Whilst System.Data.Services.Client is in the mono-wcf package.

## Trust failure

If you see a WebException indicating there was an invalid certificate received from the server then you should try running the following:

    mozroots --import --sync

See the original post on [NuGet on Mono](http://monomvc.wordpress.com/2012/03/06/nuget-on-mono/) for more details.

# Todo

Look at sharing the core parts of the source code between SharpDevelop 5 and MonoDevelop. Main differences are UI and project system, but a lot of code was used without modification.


