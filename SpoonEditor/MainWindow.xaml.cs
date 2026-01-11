using SpoonEditor.Content;
using SpoonEditor.GameProject;
using SpoonEditor.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace SpoonEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static string SpoonPath { get; private set; }

		public MainWindow()
		{
			InitializeComponent();
			Loaded += OnMainWindowLoaded;
			Closing += OnMainWindowClosing;
		}

		private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnMainWindowLoaded;
			GetEnginePath();
			OpenProjectBrowserDialog();
		}

		private void GetEnginePath()
		{
			var spoonPath = Environment.GetEnvironmentVariable("SPOON_ENGINE", EnvironmentVariableTarget.User);
			if (spoonPath == null || !Directory.Exists(Path.Combine(spoonPath, @"Engine\EngineAPI")))
			{
				var dlg = new EnginePathDialog();
				if (dlg.ShowDialog() == true)
				{
					SpoonPath = dlg.SpoonPath;
					Environment.SetEnvironmentVariable("SPOON_ENGINE", SpoonPath.ToUpper(), EnvironmentVariableTarget.User);
				}
				else
				{
					Application.Current.Shutdown();
				}
			}
			else
			{
				SpoonPath = spoonPath;
			}
		}

		private void OnMainWindowClosing(object sender, CancelEventArgs e)
		{
			if (DataContext == null)
			{
				e.Cancel = true;
				Application.Current.MainWindow.Hide();
				OpenProjectBrowserDialog();
				if (DataContext != null)
				{
					Application.Current.MainWindow.Show();
				}
			}
			else
			{
				Closing -= OnMainWindowClosing;
				Project.Current?.Unload();
				DataContext = null;
			}
		}

		private void OpenProjectBrowserDialog()
		{
			var projectBrowser = new ProjectBrowserDialog();
			if (projectBrowser.ShowDialog() == false || projectBrowser.DataContext == null)
			{
				Application.Current.Shutdown();
			}
			else
			{
				Project.Current?.Unload();
				var project = projectBrowser.DataContext as Project;
				Debug.Assert(project != null);
				ContentWatcher.Reset(project.ContentPath, project.Path);
				DataContext = project;
			}
		}
	}
}
