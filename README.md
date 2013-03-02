# NuGet Addin for MonoDevelop and Xamarin Studio

This is a port of the [SharpDevelop NuGet package management addin](http://community.sharpdevelop.net/blogs/mattward/archive/2011/01/23/NuGetSupportInSharpDevelop.aspx) that works with MonoDevelop and Xamarin Studio under Windows, Mac and Linux. It adds a Manage Packages dialog to MonoDevelop and Xamarin Studio where you can install, update or uninstall NuGet packages.

It uses a custom build of [NuGet.Core.dll](https://github.com/mrward/nuget/tree/monodevelop), based on the original NuGet source code taken from the mono-build branch available from [CodePlex](http://nuget.codeplex.com), and also some code from the latest version of the Mono runtime library.

For more information see the [announcement blog post](http://community.sharpdevelop.net/blogs/mattward/archive/2013/01/07/MonoDevelopNuGetAddin.aspx).

# Requirements

 * MonoDevelop 3.0.5 or Xamarin Studio (MonoDevelop 4.0).
 * Mono 2.10.9 or above
 * mono-winfxcore (required by NuGet.Core)
 * mono-wcf (required by NuGet.Core)

# Installation

If you want to build from source then compile using MonoDevelop and then copy the assemblies and .addin file into:

    ~/.local/share/MonoDevelop-3.0/LocalInstall/AddIns/MonoDevelop.PackageManagement

Alternatively the addin is provided as a pre-compiled binary available to download from a custom MonoDevelop repository.

For MonoDevelop 3.0.5:

    http://mrward.github.com/monodevelop-nuget-addin-repository/3.0.5/main.mrep

For Xamarin Studio (MonoDevelop 4.0):

    http://mrward.github.com/monodevelop-nuget-addin-repository/4.0/main.mrep

You can add this repository to MonoDevelop or Xamarin Studio via the [Add-in Manager](http://monodevelop.com/Documentation/Installing_Add-ins).

# Opening the Manage Packages Dialog

To open the **Manage Packages dialog** simply right click your project or your solution and select **Manage NuGet Packages**. You can also open the **Manage Packages dialog** by select **Manage NuGet Packages** from the **Project** menu.

# Configuring NuGet Package Sources

NuGet Package Sources can be configured in MonoDevelop's options.

In Linux open the options dialog by selecting **Preferences** from the **Edit** menu. In Windows select **Options** from the **Tools** menu. In the Options dialog scroll down the categories on the left hand side until you find **NuGet** and then select **Package Sources**. Here you can add and remove NuGet package sources.

#License

[MIT](http://opensource.org/licenses/MIT)

Original SharpDevelop addin code is [LGPL](http://www.gnu.org/licenses/lgpl-2.1.txt).

#Environments Tested

 * OpenSuse 12.2
 * Windows 7
 * Mono 2.10.9

#Issues

## Addin fails to load with Type Load Exception

If the addin fails to load with a "type load exception has occurred" error message then check the required mono libraries are installed.

One way to check what assemblies are missing is to open NuGet.Core.dll in MonoDevelop's Assembly Browser and check the assemblies it references. The Assembly Browser should show the missing assemblies.

NuGet.Core.dll depends on

 1. System.Data.Services.Client v4.0.0.0
 2. WindowsBase v4.0.0.0
  
WindowsBase is in the mono-winfxcore package. Whilst System.Data.Services.Client is in the mono-wcf package.

## Trust failure

If you see a WebException indicating there was an invalid certificate received from the server then you should try running the following:

    mozroots --import --sync

See the original post on [NuGet on Mono](http://monomvc.wordpress.com/2012/03/06/nuget-on-mono/) for more details.

# Todo

  * Share the core parts of the source code between SharpDevelop 5 and MonoDevelop. Main differences are UI and project system, but the majority of the code was used without modification.
  * Seek integration of this plugin into the main MonoDevelop addins repository.
