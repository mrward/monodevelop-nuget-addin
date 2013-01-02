// 
// ManagePackagesUserPrompts.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013 Matthew Ward
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

namespace ICSharpCode.PackageManagement
{
	public class ManagePackagesUserPrompts
	{
		ILicenseAcceptanceService licenseAcceptanceService;
		ISelectProjectsService selectProjectsService;
		IPackageManagementEvents packageManagementEvents;
		
		public ManagePackagesUserPrompts(IPackageManagementEvents packageManagementEvents)
			: this(
				packageManagementEvents,
				new LicenseAcceptanceService(),
				new SelectProjectsService())
		{
		}
		
		public ManagePackagesUserPrompts(
			IPackageManagementEvents packageManagementEvents,
			ILicenseAcceptanceService licenseAcceptanceService,
			ISelectProjectsService selectProjectsService)
		{
			this.packageManagementEvents = packageManagementEvents;
			this.licenseAcceptanceService = licenseAcceptanceService;
			this.selectProjectsService = selectProjectsService;
			
			SubscribeToEvents();
		}
		
		void SubscribeToEvents()
		{
			packageManagementEvents.AcceptLicenses += AcceptLicenses;			
			packageManagementEvents.SelectProjects += SelectProjects;
		}
		
		void AcceptLicenses(object sender, AcceptLicensesEventArgs e)
		{
			e.IsAccepted = licenseAcceptanceService.AcceptLicenses(e.Packages);
		}
		
		void SelectProjects(object sender, SelectProjectsEventArgs e)
		{
			e.IsAccepted = selectProjectsService.SelectProjects(e.SelectedProjects);
		}
		
		public void Dispose()
		{
			UnsubscribeFromEvents();
		}
		
		public void UnsubscribeFromEvents()
		{
			packageManagementEvents.SelectProjects -= SelectProjects;
			packageManagementEvents.AcceptLicenses -= AcceptLicenses;
		}
	}
}
