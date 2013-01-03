using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Gdk;
using Gtk;
using ICSharpCode.PackageManagement;
using NuGet;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PackagesWidget : Gtk.Bin
	{
		PackagesViewModel viewModel;
		List<PackageSource> packageSources;
		ListStore packageStore;
		CellRendererText treeViewColumnTextRenderer;
		
		public PackagesWidget ()
		{
			this.Build ();
			this.InitializeTreeView ();
		}
		
		void InitializeTreeView ()
		{
			packageStore = new ListStore (typeof (Pixbuf), typeof (string), typeof(PackageViewModel));
			packagesTreeView.Model = packageStore;
			packagesTreeView.AppendColumn (CreateTreeViewColumn ());
		}
		
		TreeViewColumn CreateTreeViewColumn ()
		{
			var column = new TreeViewColumn ();
			
			var iconRenderer = new CellRendererPixbuf ();
			column.PackStart (iconRenderer, false);
			column.AddAttribute (iconRenderer, "pixbuf", column: 0);
			
			treeViewColumnTextRenderer = new CellRendererText ();
			treeViewColumnTextRenderer.WrapMode = Pango.WrapMode.Word;
			treeViewColumnTextRenderer.WrapWidth = 300;
			
			column.PackStart (treeViewColumnTextRenderer, true);
			column.AddAttribute (treeViewColumnTextRenderer, "markup", column: 1);
			
			return column;
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
			this.packageStore.Clear ();
			
			foreach (PackageViewModel packageViewModel in viewModel.PackageViewModels) {
				this.packagesListTextView.Buffer.Text += packageViewModel.Id + "\r\n";
				AppendPackageToTreeView (packageViewModel);
			}
			
			if (viewModel.HasError) {
				this.packagesListTextView.Buffer.Text += viewModel.ErrorMessage + "\r\n";
			}
		}

		void PackageSearchEntryActivated (object sender, EventArgs e)
		{
			Search ();
		}
		
		void AppendPackageToTreeView (PackageViewModel packageViewModel)
		{
			packageStore.AppendValues (null, packageViewModel.GetDisplayTextMarkup (), packageViewModel);
		}
	}
}

