using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using ICSharpCode.PackageManagement;
using NuGet;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PackagesWidget : Gtk.Bin
	{
		PackagesViewModel viewModel;
		List<PackageSource> packageSources;
		
		public PackagesWidget ()
		{
			this.Build ();
		}
		
		public void LoadViewModel (PackagesViewModel viewModel)
		{
			this.viewModel = viewModel;
			
			this.packageSearchHBox.Visible = viewModel.IsSearchable;
			PopulatePackageSources ();
			viewModel.PropertyChanged += ViewModelPropertyChanged;
		}
		
		List<PackageSource> PackageSources {
			get {
				if (packageSources == null) {
					packageSources = viewModel.PackageSources.ToList ();
				}
				return packageSources;
			}
		}
		
		void PopulatePackageSources ()
		{
			this.packageSourceComboBox.Visible = viewModel.IsSearchable;
			if (viewModel.IsSearchable) {
				for (int index = 0; index < PackageSources.Count; ++index) {
					PackageSource packageSource = PackageSources [index];
					this.packageSourceComboBox.InsertText (index, packageSource.Name);
				}
				
				this.packageSourceComboBox.Active = GetSelectedPackageSourceIndexFromViewModel ();
			}
		}
		
		int GetSelectedPackageSourceIndexFromViewModel ()
		{
			if (viewModel.SelectedPackageSource == null) {
				return -1;
			}
			
			return PackageSources.IndexOf (viewModel.SelectedPackageSource);
		}
		
		void PackageSourceChanged (object sender, EventArgs e)
		{
			viewModel.SelectedPackageSource = GetSelectedPackageSource ();
		}
		
		PackageSource GetSelectedPackageSource ()
		{
			if (this.packageSourceComboBox.Active == -1) {
				return null;
			}
			
			return PackageSources [this.packageSourceComboBox.Active];
		}
		
		void SearchButtonClicked (object sender, EventArgs e)
		{
			Search ();
		}
		
		void Search ()
		{
			viewModel.SearchTerms = this.packageSearchEntry.Text;
			viewModel.SearchCommand.Execute (null);
		}

		void ViewModelPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			this.packagesListTextView.Buffer.Text += "PropertyChanged: " + e.PropertyName + "\r\n";
			
			foreach (PackageViewModel packageViewModel in viewModel.PackageViewModels) {
				this.packagesListTextView.Buffer.Text += packageViewModel.Id + "\r\n";
			}
		}

		void PackageSearchEntryActivated (object sender, EventArgs e)
		{
			Search ();
		}
	}
}

