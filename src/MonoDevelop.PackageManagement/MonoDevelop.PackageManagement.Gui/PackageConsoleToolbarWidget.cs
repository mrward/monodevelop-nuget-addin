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
using Gtk;
using MonoDevelop.Core;

namespace MonoDevelop.PackageManagement
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class PackageConsoleToolbarWidget : Gtk.Bin
	{
		Button clearButton;

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
		
	}
}

