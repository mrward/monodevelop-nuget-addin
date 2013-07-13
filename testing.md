# Testing Procedure

Tests to be run on Windows, Linux and Mac before publishing addin.

## Assembly reference relative paths for project and solution in same folder

1. Install NUnit package into a project with solution in same folder
 
    MySolution.sln
    MyProject.csproj

2. Check relative path to reference added in project is correct.
3. Should be "packages\NUnit...."

## Assembly reference relative paths for project and solution in different folders

1. Install NUnit package into a project with a solution is in a folder below project

    MySolution.sln 
    MyProject/MyProject.csproj

2. Check relative path to reference added in project is correct.
3. Should be "..\packages\NUnit...."

## Package containing subfolder with files has files added to project 

1. Install jQuery into empty project
2. Check that the Scripts folder is created in the project and it contains jquery files.

## Changing NuGet options are saved

1. Open the NuGet General Options.
2. Change the Enable package restore settings.
3. Click OK
4. No error should be reported.
5. Check the NuGet.config is updated (~/.config/NuGet/NuGet.config)

## View and install packages from a NuGet feeds requiring basic and Windows authentication

1. Open Manage Packages dialog and try to view packages from a feed requiring authentication.
2. Check that an error is shown on Linux and Mac.
3. Use custom NuGet.exe build to add credentials for authenticated feed.

      mono --runtime=v4.0.30319 NuGet.exe sources update  -name FeedName -username user -password pass

4. Open Manage Packages dialog and view packages from authenticated feed.
5. Packages can be viewed and installed from authenticated feed.

## app.config and web.config Transforms

1. Install a package with an app.config.transform into a console project.
2. Check the app.config is updated with the transform.
3. Install a package (e.g. Nancy.Hosting.Aspnet) with an web.config.transform into a web project.
4. Check the web.config is updated with the transform.

Known issue with case sensitivity of .config filenames.

## XML Document Transforms (XDTs)

1. Install a package with an app.config.install.xdt into a web project.
3. Check the web.config is updated with the transform.
