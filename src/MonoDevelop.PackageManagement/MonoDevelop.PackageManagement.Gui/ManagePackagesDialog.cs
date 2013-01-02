using System;
using ICSharpCode.PackageManagement;

namespace MonoDevelop.PackageManagement
{
	public partial class ManagePackagesDialog : Gtk.Dialog
	{
		public ManagePackagesDialog (PackageManagementViewModels viewModels)
		{
			this.Build ();
			
			LoadViewModels (viewModels);
		}
		
		void LoadViewModels (PackageManagementViewModels viewModels)
		{
			this.Title = viewModels.ManagePackagesViewModel.Title;
		}
	}
}

