using System;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PackagesWidget : Gtk.Bin
	{
		public PackagesWidget ()
		{
			this.Build ();
		}
	}
}

