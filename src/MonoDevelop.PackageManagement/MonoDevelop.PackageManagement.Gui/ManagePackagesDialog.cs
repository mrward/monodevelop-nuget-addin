using System;
using ICSharpCode.PackageManagement;

namespace MonoDevelop.PackageManagement
{
	public partial class ManagePackagesDialog : Gtk.Dialog
	{
		public ManagePackagesDialog (ManagePackagesViewModel viewModel)
		{
			this.Build ();
			
			LoadViewModels (viewModel);
		}
		
		void LoadViewModels (ManagePackagesViewModel viewModel)
		{
			this.Title = viewModel.Title;
			
			this.availablePackagesWidget.LoadViewModel (viewModel.AvailablePackagesViewModel);
			this.installedPackagesWidget.LoadViewModel (viewModel.InstalledPackagesViewModel);
			this.UpdatedPackagesWidget.LoadViewModel (viewModel.UpdatedPackagesViewModel);
			this.recentPackagesWidget.LoadViewModel (viewModel.RecentPackagesViewModel);
		}
	}
}

