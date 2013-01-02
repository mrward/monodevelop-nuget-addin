using System;
using System.ComponentModel;
using ICSharpCode.PackageManagement;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PackagesWidget : Gtk.Bin
	{
		PackagesViewModel viewModel;
		
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
		
		void PopulatePackageSources ()
		{
			this.packageSourceComboBox.Visible = viewModel.IsSearchable;
			if (viewModel.IsSearchable) {
				
			}
		}

		void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.packagesListTextView.Buffer.Text += "PropertyChanged: " + e.PropertyName + "\r\n";
		}
	}
}

