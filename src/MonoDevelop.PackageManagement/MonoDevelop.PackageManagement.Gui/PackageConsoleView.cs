// 
// PackageConsoleView.cs
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
using ICSharpCode.Scripting;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using Pango;

namespace MonoDevelop.PackageManagement
{
	public class PackageConsoleView : ConsoleView, IScriptingConsole
	{
		public PackageConsoleView ()
		{
			// HACK - to allow text to appear before first prompt.
			PromptString = String.Empty;
			Clear ();
			
			FontDescription font = FontDescription.FromString (DesktopService.DefaultMonospaceFont);
			SetFont (font);
		}
		
		void WriteOutputLine (string format, params object[] args)
		{
			WriteOutput (String.Format (format, args) + Environment.NewLine);
		}
		
		public bool ScrollToEndWhenTextWritten { get; set; }
		
		public void SendLine (string line)
		{
		}
		
		public void SendText (string text)
		{
		}
		
		public void WriteLine ()
		{
			DispatchService.GuiSyncDispatch (() => {
				WriteOutput (String.Empty);
			});
		}
		
		public void WriteLine (string text, ScriptingStyle style)
		{
			DispatchService.GuiSyncDispatch (() => {
				if (style == ScriptingStyle.Prompt) {
					WriteOutputLine (text);
					ConfigurePromptString ();
					Prompt (true);
				} else {
					WriteOutputLine (text);
				}
			});
		}
		
		void ConfigurePromptString()
		{
			PromptString = "PM> ";
		}
		
		public void Write (string text, ScriptingStyle style)
		{
			DispatchService.GuiSyncDispatch (() => {
				if (style == ScriptingStyle.Prompt) {
					ConfigurePromptString ();
					Prompt (false);
				} else {
					WriteOutput (text);
				}
			});
		}
		
		public string ReadLine (int autoIndentSize)
		{
			throw new NotImplementedException();
		}
		
		public string ReadFirstUnreadLine ()
		{
			throw new NotImplementedException();
		}
		
		public int GetMaximumVisibleColumns ()
		{
			int maxVisibleColumns = 160;
			
			DispatchService.GuiSyncDispatch (() => {
				int windowWidth = Allocation.Width;
				
				if (windowWidth > 0) {
					maxVisibleColumns = windowWidth / 5;
				}
				
			});
			
			return maxVisibleColumns;
		}
	}
}
