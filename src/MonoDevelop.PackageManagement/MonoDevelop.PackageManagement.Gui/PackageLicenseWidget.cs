using System;
using ICSharpCode.PackageManagement;
using NuGet;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PackageLicenseWidget : Gtk.Bin
	{
		public PackageLicenseWidget (IPackage package)
		{
			this.Build ();
			this.DisplayPackage (package);
		}

		void DisplayPackage (IPackage package)
		{
			this.packageIdLabel.Markup = GetPackageIdMarkup (package.Id);
			this.packageSummaryTextView.Buffer.Text = GetPackageSummary (package);
			this.licenseHyperlinkWidget.Uri = package.LicenseUrl.ToString ();
		}

		string GetPackageSummary (IPackage package)
		{
			if (!(String.IsNullOrEmpty (package.Summary))) {
				return package.Summary;
			}
			return package.Description;
		}

		string GetPackageIdMarkup (string id)
		{
			string format = "<span weight='bold'>{0}</span>";
			return MarkupString.Format (format, id);
		}
	}
}

