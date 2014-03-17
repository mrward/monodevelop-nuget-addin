# NuGet Addin for MonoDevelop and Xamarin Studio

This is a port of the [SharpDevelop NuGet package management addin](http://community.sharpdevelop.net/blogs/mattward/archive/2011/01/23/NuGetSupportInSharpDevelop.aspx) that works with MonoDevelop and Xamarin Studio under Windows, Mac and Linux. It adds a Manage Packages dialog to MonoDevelop and Xamarin Studio where you can install, update or uninstall NuGet packages.

It uses a custom build of [NuGet.Core.dll](https://github.com/mrward/nuget/), based on the original NuGet source code taken from [CodePlex](http://nuget.codeplex.com), some code from the latest version of the Mono runtime library and a custom build of [Microsoft's XML Document Transformation (XDT) library](https://github.com/mrward/xdt).

For more information see the [announcement blog post](http://community.sharpdevelop.net/blogs/mattward/archive/2013/01/07/MonoDevelopNuGetAddin.aspx).

[Adam Patridge](http://www.patridgedev.com/) has created a [YouTube video](http://www.youtube.com/watch?v=uHbAgbfgQdU) which covers installation and how to use the addin.

# Requirements

 * MonoDevelop 3.0, 4.0, 4.2 or Xamarin Studio 4.0, 4.2
 * Mono 2.10.9 or later
 * mono-winfxcore (required by NuGet.Core)
 * mono-wcf (required by NuGet.Core)

# Installation

The addin is available from a custom MonoDevelop addin repository. You can add this repository to MonoDevelop or Xamarin Studio via the [Add-in Manager](http://monodevelop.com/Documentation/Installing_Add-ins). To open the Add-in Manager dialog on Windows and on Linux select **Add-in Manager** from the **Tools** menu. On the Mac open the **Xamarin Studio** menu and select **Add-in Manager**. In the Add-in Manager dialog select the **Gallery** tab. Click the down arrow next to the repositories drop down list and select **Manage Repositories** to open the Add-in Repository Management dialog. Click the **Add** button. Copy the MonoDevelop NuGet addin repository url, shown below, that corresponds to your MonoDevelop version, and paste it into the **Url** text box.

For MonoDevelop and Xamarin Studio 4.0 and 4.2:

    mrward.github.com/monodevelop-nuget-addin-repository/4.0/main.mrep

For MonoDevelop 3.0:

    http://mrward.github.com/monodevelop-nuget-addin-repository/3.0.5/main.mrep

Click the **OK** button and then click the **Close** button. Back in the Add-in Manager dialog expand **IDE Extensions** by clicking the arrow next to it. The addin should then be displayed under the IDE Extensions as **NuGet Package Management**. To install the addin select **NuGet Package Management** and then click the **Install** button.

# Opening the Manage Packages Dialog

To open the **Manage Packages dialog** simply right click your project, or your solution or References in the Solutions window  and select **Manage NuGet Packages**. You can also open the **Manage Packages dialog** by selecting **Manage NuGet Packages** from the **Project** menu.

# Configuring NuGet Package Sources

The sources or feeds for NuGet packages can be configured in MonoDevelop's options.

On Linux open the options dialog by selecting **Preferences** from the **Edit** menu. On Windows select **Options** from the **Tools** menu. On a Mac select **Preferences** from the **Xamarin Studio** menu. In the Options dialog scroll down the categories on the left hand side until you find **NuGet** and then select **Package Sources**. Here you can add and remove NuGet package sources.

#License

[MIT](http://opensource.org/licenses/MIT)

Original SharpDevelop addin code is [LGPL](http://www.gnu.org/licenses/lgpl-2.1.txt).

# Environments Tested

 * OpenSuse 12.2 - Mono 2.10.9
 * Windows 7 - Mono 2.10.9
 * OS X 10.8.2 - Mono 2.10.12

# Issues

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
  * Make this addin available in the main MonoDevelop addins repository.
