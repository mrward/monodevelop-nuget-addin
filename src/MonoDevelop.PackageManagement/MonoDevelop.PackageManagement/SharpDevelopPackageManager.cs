// 
// SharpDevelopPackageManager.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2012 Matthew Ward
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace ICSharpCode.PackageManagement
{
	public class SharpDevelopPackageManager : PackageManager, ISharpDevelopPackageManager
	{
		IProjectSystem projectSystem;
		IPackageOperationResolverFactory packageOperationResolverFactory;
		
		public SharpDevelopPackageManager(
			IPackageRepository sourceRepository,
			IProjectSystem projectSystem,
			ISolutionPackageRepository solutionPackageRepository,
			IPackageOperationResolverFactory packageOperationResolverFactory)
			: base(
				sourceRepository,
				solutionPackageRepository.PackagePathResolver,
				solutionPackageRepository.FileSystem,
				solutionPackageRepository.Repository)
		{
			this.projectSystem = projectSystem;
			this.packageOperationResolverFactory = packageOperationResolverFactory;
			CreateProjectManager();
		}
		
		// <summary>
		/// project manager should be created with:
		/// 	local repo = PackageReferenceRepository(projectSystem, sharedRepo)
		///     packageRefRepo should have its RegisterIfNecessary() method called before creating the project manager.
		/// 	source repo = sharedRepository
		/// </summary>
		void CreateProjectManager()
		{
			var packageRefRepository = CreatePackageReferenceRepository();
			ProjectManager = CreateProjectManager(packageRefRepository);
		}
		
		PackageReferenceRepository CreatePackageReferenceRepository()
		{
			var sharedRepository = LocalRepository as ISharedPackageRepository;
			var packageRefRepository = new PackageReferenceRepository(projectSystem, sharedRepository);
			packageRefRepository.RegisterIfNecessary();
			return packageRefRepository;
		}
		
		public ISharpDevelopProjectManager ProjectManager { get; set; }
		
		SharpDevelopProjectManager CreateProjectManager(PackageReferenceRepository packageRefRepository)
		{
			return new SharpDevelopProjectManager(LocalRepository, PathResolver, projectSystem, packageRefRepository);
		}
		
		public void InstallPackage(IPackage package)
		{
			bool ignoreDependencies = false;
			bool allowPreleaseVersions = false;
			InstallPackage(package, ignoreDependencies, allowPreleaseVersions);
		}
		
		public void InstallPackage(IPackage package, InstallPackageAction installAction)
		{
			TempLoggingService.LogInfo("InstallPackage operations.Count: " + installAction.Operations.Count());
			foreach (PackageOperation operation in installAction.Operations) {
				Execute(operation);
			}
			AddPackageReference(package, installAction.IgnoreDependencies, installAction.AllowPrereleaseVersions);
		}
		
		void AddPackageReference(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions)
		{
			ProjectManager.AddPackageReference(package.Id, package.Version, ignoreDependencies, allowPrereleaseVersions);			
		}
		
		public override void InstallPackage(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions)
		{
			base.InstallPackage(package, ignoreDependencies, allowPrereleaseVersions);
			AddPackageReference(package, ignoreDependencies, allowPrereleaseVersions);
		}
		
//		public void UninstallPackage(IPackage package, UninstallPackageAction uninstallAction)
//		{
//			UninstallPackage(package, uninstallAction.ForceRemove, uninstallAction.RemoveDependencies);
//		}
//		
//		public override void UninstallPackage(IPackage package, bool forceRemove, bool removeDependencies)
//		{
//			ProjectManager.RemovePackageReference(package.Id, forceRemove, removeDependencies);
//			if (!IsPackageReferencedByOtherProjects(package)) {
//				base.UninstallPackage(package, forceRemove, removeDependencies);
//			}
//		}
		
		bool IsPackageReferencedByOtherProjects(IPackage package)
		{
			var sharedRepository = LocalRepository as ISharedPackageRepository;
			return sharedRepository.IsReferenced(package.Id, package.Version);
		}
		
		public IEnumerable<PackageOperation> GetInstallPackageOperations(IPackage package, InstallPackageAction installAction)
		{
			IPackageOperationResolver resolver = CreateInstallPackageOperationResolver(installAction);
			return resolver.ResolveOperations(package);
		}
		
		IPackageOperationResolver CreateInstallPackageOperationResolver(InstallPackageAction installAction)
		{
			return packageOperationResolverFactory.CreateInstallPackageOperationResolver(
				LocalRepository,
				SourceRepository,
				Logger,
				installAction);
		}
		
//		public void UpdatePackage(IPackage package, UpdatePackageAction updateAction)
//		{
//			foreach (PackageOperation operation in updateAction.Operations) {
//				Execute(operation);
//			}
//			UpdatePackageReference(package, updateAction);
//		}
//		
//		void UpdatePackageReference(IPackage package, UpdatePackageAction updateAction)
//		{
//			ProjectManager.UpdatePackageReference(package.Id, package.Version, updateAction.UpdateDependencies, updateAction.AllowPrereleaseVersions);		
//		}
		
		protected override void OnInstalling(PackageOperationEventArgs e)
		{
			TempLoggingService.LogInfo("OnInstalling: " + e.Package.Id);
			base.OnInstalling(e);
		}
		
		protected override void OnInstalled(PackageOperationEventArgs e)
		{
			TempLoggingService.LogInfo("OnInstalled: " + e.Package.Id);
			base.OnInstalled(e);
		}
		
		protected override void OnExpandFiles(PackageOperationEventArgs e)
		{
			TempLoggingService.LogInfo("OnExpandFiles: " + e.Package.Id);
			base.OnExpandFiles(e);
		}
	}
}
