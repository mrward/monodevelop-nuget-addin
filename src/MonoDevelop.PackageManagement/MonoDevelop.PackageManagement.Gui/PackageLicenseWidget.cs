using System;
using ICSharpCode.PackageManagement;
using NuGet;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PackageLicenseWidget : Gtk.Bin
	{
		public PackageLicenseWidget (PackageLicenseViewModel viewModel)
		{
			this.Build ();
			this.DisplayPackage (viewModel);
		}

		void DisplayPackage (PackageLicenseViewModel viewModel)
		{
			this.packageIdLabel.Markup = GetPackageIdMarkup (viewModel.Id);
			this.packageSummaryTextView.Buffer.Text = viewModel.Summary;
			this.licenseHyperlinkWidget.Uri = viewModel.LicenseUrl.ToString ();
		}
		
		string GetPackageIdMarkup (string id)
		{
			string format = "<span weight='bold'>{0}</span>";
			return MarkupString.Format (format, id);
		}
	}
}

