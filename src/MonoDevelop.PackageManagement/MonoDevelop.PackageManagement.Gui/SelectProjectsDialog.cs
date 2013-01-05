using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using ICSharpCode.PackageManagement;

namespace MonoDevelop.PackageManagement
{
	public partial class SelectProjectsDialog : Gtk.Dialog
	{
		SelectProjectsViewModel viewModel;
		ListStore projectsStore;
		const int SelectedCheckBoxColumn = 0;
		const int SelectedProjectNameColumn = 1;
		const int SelectedProjectColumn = 2;
		
		public SelectProjectsDialog (SelectProjectsViewModel viewModel)
		{
			this.Build ();
			this.viewModel = viewModel;
			InitializeTreeView ();
			AddProjectsToTreeView ();
		}
		
		void InitializeTreeView ()
		{
			projectsStore = new ListStore (typeof (bool), typeof (string), typeof (IPackageManagementSelectedProject));
			projectsTreeView.Model = projectsStore;
			projectsTreeView.AppendColumn (CreateTreeViewColumn ());
		}
		
		TreeViewColumn CreateTreeViewColumn ()
		{
			var column = new TreeViewColumn ();
			
			var checkBoxRenderer = new CellRendererToggle ();
			checkBoxRenderer.Toggled += SelectedProjectCheckBoxToggled;
			column.PackStart (checkBoxRenderer, false);
			column.AddAttribute (checkBoxRenderer, "active", SelectedCheckBoxColumn);
			
			var textRenderer = new CellRendererText ();
			column.PackStart (textRenderer, true);
			column.AddAttribute (textRenderer, "markup", SelectedProjectNameColumn);
			
			return column;
		}

		void SelectedProjectCheckBoxToggled(object o, ToggledArgs args)
		{
			TreeIter iter;
			projectsStore.GetIterFromString (out iter, args.Path);
			var project = projectsStore.GetValue (iter, SelectedProjectColumn) as IPackageManagementSelectedProject;
			project.IsSelected = !project.IsSelected;
			projectsStore.SetValue (iter, SelectedCheckBoxColumn, project.IsSelected);
		}
		
		void AddProjectsToTreeView ()
		{
			foreach (IPackageManagementSelectedProject project in GetEnabledProjects ()) {
				AddProjectToTreeView (project);
			}
		}
		
		IEnumerable<IPackageManagementSelectedProject> GetEnabledProjects ()
		{
			return viewModel.Projects.Where(project => project.IsEnabled);
		}
		
		void AddProjectToTreeView (IPackageManagementSelectedProject project)
		{
			projectsStore.AppendValues (project.IsSelected, project.Name, project);
		}
	}
}

