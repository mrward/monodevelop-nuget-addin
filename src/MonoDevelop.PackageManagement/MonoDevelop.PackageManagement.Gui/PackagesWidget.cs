using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Gdk;
using Gtk;
using ICSharpCode.PackageManagement;
using MonoDevelop.Ide;
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
		const int PackageViewModelColumn = 2;
		
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
			packagesTreeView.Selection.Changed += PackagesTreeViewSelectionChanged;
		}
		
		TreeViewColumn CreateTreeViewColumn ()
		{
			var column = new TreeViewColumn ();
			
			var iconRenderer = new CellRendererPixbuf ();
			column.PackStart (iconRenderer, false);
			column.AddAttribute (iconRenderer, "pixbuf", column: 0);
			
			treeViewColumnTextRenderer = new CellRendererText ();
			treeViewColumnTextRenderer.WrapMode = Pango.WrapMode.Word;
			treeViewColumnTextRenderer.WrapWidth = 250;
			
			column.PackStart (treeViewColumnTextRenderer, true);
			column.AddAttribute (treeViewColumnTextRenderer, "markup", column: 1);
			
			return column;
		}
		
		void PackagesTreeViewSelectionChanged(object sender, EventArgs e)
		{
			ShowSelectedPackage ();
		}
		
		public void LoadViewModel (PackagesViewModel viewModel)
		{
			this.viewModel = viewModel;
			
			this.packageSearchHBox.Visible = viewModel.IsSearchable;
			ClearSelectedPackageInformation ();
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
		
		void ShowSelectedPackage ()
		{
			PackageViewModel packageViewModel = GetSelectedPackageViewModel ();
			if (packageViewModel != null) {
				ShowPackageInformation (packageViewModel);
			} else {
				ClearSelectedPackageInformation ();
			}
		}
		
		PackageViewModel GetSelectedPackageViewModel ()
		{
			TreeIter item;
			if (packagesTreeView.Selection.GetSelected (out item)) {
				return packageStore.GetValue (item, PackageViewModelColumn) as PackageViewModel;
			}
			return null;
		}
		
		void ShowPackageInformation (PackageViewModel packageViewModel)
		{
			this.packageVersionTextBox.Text = packageViewModel.Version.ToString ();
			this.packageCreatedByTextBox.Text = packageViewModel.GetAuthors ();
			this.packageLastUpdatedTextBox.Text = packageViewModel.GetLastPublishedDisplayText ();
			this.packageDownloadsTextBox.Text = packageViewModel.GetDownloadCountDisplayText ();
			this.packageDescriptionTextView.Buffer.Text = packageViewModel.Description;
			
			EnablePackageActionButtons (packageViewModel);
			
			this.packageInfoFrameVBox.Visible = true;
			this.managePackageButtonBox.Visible = true;
		}
		
		void ClearSelectedPackageInformation ()
		{
			this.packageInfoFrameVBox.Visible = false;
			this.managePackageButtonBox.Visible = false;
		}
		
		void EnablePackageActionButtons (PackageViewModel packageViewModel)
		{
			this.addPackageButton.Visible = !packageViewModel.IsManaged;
			this.removePackageButton.Visible = !packageViewModel.IsManaged;
			this.managePackageButton.Visible = packageViewModel.IsManaged;
			
			this.addPackageButton.Sensitive = !packageViewModel.IsAdded;
			this.removePackageButton.Sensitive = packageViewModel.IsAdded;
		}
		
		void ViewModelPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			this.packageStore.Clear ();
			
			foreach (PackageViewModel packageViewModel in viewModel.PackageViewModels) {
				AppendPackageToTreeView (packageViewModel);
			}
		}

		void PackageSearchEntryActivated (object sender, EventArgs e)
		{
			Search ();
		}
		
		void AppendPackageToTreeView (PackageViewModel packageViewModel)
		{
			packageStore.AppendValues (
				ImageService.GetPixbuf ("md-nuget-package", IconSize.Dnd),
				packageViewModel.GetDisplayTextMarkup (),
				packageViewModel);
		}
		
		void OnAddPackageButtonClicked (object sender, EventArgs e)
		{
			PackageViewModel packageViewModel = GetSelectedPackageViewModel ();
			packageViewModel.AddPackage ();
			EnablePackageActionButtons (packageViewModel);
		}
		
		void RemovePackageButtonClicked (object sender, EventArgs e)
		{
			PackageViewModel packageViewModel = GetSelectedPackageViewModel ();
			packageViewModel.RemovePackage ();
			EnablePackageActionButtons (packageViewModel);
		}
		
		void ManagePackagesButtonClicked (object sender, EventArgs e)
		{
			PackageViewModel packageViewModel = GetSelectedPackageViewModel ();
			packageViewModel.ManagePackage ();
		}
	}
}

