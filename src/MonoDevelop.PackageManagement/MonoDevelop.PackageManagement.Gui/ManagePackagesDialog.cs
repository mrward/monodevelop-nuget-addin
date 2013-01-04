using System;
using Gtk;
using ICSharpCode.PackageManagement;

namespace MonoDevelop.PackageManagement
{
	public partial class ManagePackagesDialog : Gtk.Dialog
	{
		ManagePackagesViewModel viewModel;
		IPackageManagementEvents packageManagementEvents;
		
		public ManagePackagesDialog (ManagePackagesViewModel viewModel, IPackageManagementEvents packageManagementEvents)
		{
			this.Build ();
			
			this.viewModel = viewModel;
			this.packageManagementEvents = packageManagementEvents;
			AddPackageManagementEventHandlers ();
			LoadViewModels ();
		}
		
		void AddPackageManagementEventHandlers ()
		{
			packageManagementEvents.PackageOperationMessageLogged += PackageOperationMessageLogged;
			packageManagementEvents.PackageOperationError += PackageOperationError;
		}
		
		void RemovePackageManagementEventHandlers ()
		{
			packageManagementEvents.PackageOperationMessageLogged -= PackageOperationMessageLogged;
			packageManagementEvents.PackageOperationError -= PackageOperationError;
		}

		void PackageOperationMessageLogged (object sender, PackageOperationMessageLoggedEventArgs e)
		{
			AppendMessage (e.Message.ToString ());
		}
		
		void PackageOperationError(object sender, PackageOperationExceptionEventArgs e)
		{
			AppendMessage (e.Exception.Message);
		}
		
		void AppendMessage (string message)
		{
			TextIter end = this.messagesTextView.Buffer.EndIter;
			this.messagesTextView.Buffer.Insert (ref end, message + "\n");
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
			RemovePackageManagementEventHandlers ();
			base.Destroy ();
		}
	}
}

