using System;
using Gtk;
using MonoDevelop.Ide;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class HyperlinkWidget : Gtk.Bin
	{
		LinkButton linkButton;

		public HyperlinkWidget (string uri, string label)
		{
			this.Build ();
			AddLinkButton (uri, label);
		}
		
		public HyperlinkWidget ()
		{
			this.Build ();
			AddLinkButton ();
		}
		
		void AddLinkButton (string uri = "", string label = "")
		{
			linkButton = new LinkButton (uri, label);
			linkButton.Relief = ReliefStyle.None;
			linkButton.CanFocus = false;
			linkButton.SetAlignment (0, 0);
			linkButton.Clicked += LinkButtonClicked;
			this.Add (linkButton);
		}
		
		void LinkButtonClicked (object sender, EventArgs e)
		{
			DesktopService.ShowUrl (linkButton.Uri);
		}
		
		public string Uri {
			get { return linkButton.Uri; }
			set { linkButton.Uri = value; }
		}
		
		public string Label {
			get { return linkButton.Label; }
			set { linkButton.Label = value; }
		}
	}
}

