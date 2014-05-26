//
// PackageConsoleToolbarWidget.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using System.Collections.Specialized;
using Gtk;
using ICSharpCode.PackageManagement;
using ICSharpCode.PackageManagement.Scripting;
using MonoDevelop.Core;
using MonoDevelop.Projects;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class PackageConsoleToolbarWidget : Gtk.Bin
	{
		Button clearButton;
		PackageManagementConsoleViewModel viewModel;
		bool reloadingPackageSources;
		bool reloadingProjects;
		ListStore projectListStore;

		public PackageConsoleToolbarWidget ()
		{
			this.Build ();

			clearButton = new Button (new Image (Stock.Clear, IconSize.Menu));
			clearButton.TooltipText = GettextCatalog.GetString ("Clear Console");
			clearButton.Clicked += OnClearButtonClicked;
			mainHBox.PackEnd (clearButton);
		}

		void OnClearButtonClicked(object sender, EventArgs e)
		{
			if (ClearButtonClicked != null) {
				ClearButtonClicked (this, e);
			}
		}
		
		public event EventHandler ClearButtonClicked;
		
		public void LoadViewModel (PackageManagementConsoleViewModel viewModel)
		{
			this.viewModel = viewModel;
			
			LoadPackageSources ();
			
			SetUpProjectsComboBox ();
			LoadProjects ();
			
			RegisterEvents();
		}
		
		void RegisterEvents()
		{
			viewModel.PackageSources.CollectionChanged += ViewModelPackageSourcesChanged;
			packageSourcesComboBox.Changed += PackageSourcesComboBoxChanged;
			
			viewModel.Projects.CollectionChanged += ViewModelProjectsChanged;
			projectsComboBox.Changed += ProjectsComboBoxChanged;
		}
		
		void LoadPackageSources ()
		{
			ClearPackageSources ();
			
			for (int index = 0; index < viewModel.PackageSources.Count; ++index) {
				PackageSourceViewModel packageSource = viewModel.PackageSources [index];
				packageSourcesComboBox.InsertText (index, packageSource.Name);
			}
			
			packageSourcesComboBox.Active = GetActivePackageSourceIndexFromViewModel ();
		}
		
		void SetUpProjectsComboBox ()
		{
			projectListStore = new ListStore (typeof (string), typeof(Project));
			projectsComboBox.Model = projectListStore;
		}

		void ClearPackageSources ()
		{
			int count = packageSourcesComboBox.Model.IterNChildren ();
			for (int index = 0; index < count; ++index) {
				packageSourcesComboBox.RemoveText (0);
			}
			
			packageSourcesComboBox.Active = -1;
		}
		
		void ViewModelPackageSourcesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			reloadingPackageSources = true;
			
			LoadPackageSources ();
			
			reloadingPackageSources = false;
		}
		
		int GetActivePackageSourceIndexFromViewModel ()
		{
			if (viewModel.ActivePackageSource == null) {
				if (viewModel.PackageSources.Count > 0) {
					return 0;
				}
				return -1;
			}
			
			int index = viewModel.PackageSources.IndexOf (viewModel.ActivePackageSource);
			if (index >= 0) {
				return index;
			}
			
			return 0;
		}
		
		void PackageSourcesComboBoxChanged (object sender, EventArgs e)
		{
			if (reloadingPackageSources)
				return;
			
			viewModel.ActivePackageSource = GetSelectedPackageSource ();
		}
		
		PackageSourceViewModel GetSelectedPackageSource ()
		{
			if (packageSourcesComboBox.Active == -1) {
				return null;
			}
			
			return viewModel.PackageSources [packageSourcesComboBox.Active];
		}
		
		void LoadProjects ()
		{
			projectListStore.Clear ();
			
			foreach (Project project in viewModel.Projects) {
				projectListStore.AppendValues (project.Name, project);
			}
			
			if (viewModel.Projects.Count > 0) {
				projectsComboBox.Active = 0;
			}
		}
		
		void ViewModelProjectsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			reloadingProjects = true;
			
			LoadProjects ();
			
			reloadingProjects = false;
		}
		
		void ProjectsComboBoxChanged (object sender, EventArgs e)
		{
			if (reloadingProjects)
				return;
			
			UpdateDefaultProject ();
		}
		
		void UpdateDefaultProject ()
		{
			int selectedIndex = projectsComboBox.Active;
			
			if (selectedIndex < 0) {
				viewModel.DefaultProject = null;
				return;
			}
			
			TreeIter iter;
			projectListStore.IterNthChild (out iter, selectedIndex);
			var project = (Project)projectListStore.GetValue (iter, 1);
			viewModel.DefaultProject = project;
		}
	}
}

