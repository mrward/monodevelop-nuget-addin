// 
// PackageConsolePad.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2014 Matthew Ward
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using ICSharpCode.PackageManagement;
using ICSharpCode.PackageManagement.Scripting;
using MonoDevelop.Components;
using MonoDevelop.Components.Docking;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.PackageManagement
{
	public class PackageConsolePad : IPadContent
	{
		PackageConsoleView view;
		PackageManagementConsoleViewModel viewModel;
		PackageConsoleToolbarWidget toolbarWidget;
		
		public PackageConsolePad ()
		{
		}
		
		public Gtk.Widget Control {
			get { return view; }
		}
		
		public void Initialize (IPadWindow window)
		{
			CreateToolbar (window);
			CreatePackageConsoleView ();
			CreatePackageConsoleViewModel ();
			BindingViewModelToView ();
		}
		
		void CreatePackageConsoleViewModel()
		{
			var viewModels = new PackageManagementViewModels ();
			viewModel = viewModels.PackageManagementConsoleViewModel;
			viewModel.RegisterConsole (view);
		}

		void CreateToolbar (IPadWindow window)
		{
			toolbarWidget = new PackageConsoleToolbarWidget ();
			toolbarWidget.ClearButtonClicked += ClearButtonClicked;
			DockItemToolbar toolbar = window.GetToolbar (Gtk.PositionType.Top);
			toolbar.Add (toolbarWidget, false);
			toolbar.ShowAll ();
		}

		void ClearButtonClicked(object sender, EventArgs e)
		{
			viewModel.ClearConsole ();
		}

		void CreatePackageConsoleView ()
		{
			view = new PackageConsoleView ();
			view.ConsoleInput += OnConsoleInput;
			view.ShadowType = Gtk.ShadowType.None;
			view.ShowAll ();
		}

		void OnConsoleInput (object sender, ConsoleInputEventArgs e)
		{
			//view.WriteOutput (e.Text);
			//view.Prompt (true);
			viewModel.ProcessUserInput (e.Text);
		}
		
		public void RedrawContent ()
		{
		}
		
		public void Dispose ()
		{
		}
		
		void BindingViewModelToView ()
		{
			toolbarWidget.LoadViewModel (viewModel);
		}
	}
}
