using System;
using ICSharpCode.PackageManagement;

namespace MonoDevelop.PackageManagement
{
	public partial class ManagePackagesDialog : Gtk.Dialog
	{
		ManagePackagesViewModel viewModel;
		
		public ManagePackagesDialog (ManagePackagesViewModel viewModel)
		{
			this.Build ();
			
			this.viewModel = viewModel;
			LoadViewModels ();
		}
		
		void LoadViewModels ()
		{
			this.Title = viewModel.Title;
			
			this.availablePackagesWidget.LoadViewModel (viewModel.AvailablePackagesViewModel);
			this.installedPackagesWidget.LoadViewModel (viewModel.InstalledPackagesViewModel);
			this.UpdatedPackagesWidget.LoadViewModel (viewModel.UpdatedPackagesViewModel);
			this.recentPackagesWidget.LoadViewModel (viewModel.RecentPackagesViewModel);
		}
		
		public override void Destroy ()
		{
			viewModel.Dispose ();
			base.Destroy ();
		}
	}
}

