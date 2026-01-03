using SpoonEditor.GameProject;
using SpoonEditor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public static string SpoonPath { get; private set; } = @"E:\GD\DX12\solutions\SpoonGameEngine";

		public MainWindow()
        {
            InitializeComponent();
			Loaded += OnMainWindowLoaded;
			Closing += OnMainWindowClosing;
        }

		private void OnMainWindowClosing(object sender, CancelEventArgs e)
		{
			Closing -= OnMainWindowClosing;
			Project.Current?.Unload();
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

		private void OpenProjectBrowserDialog()
		{
			var projectBrowser = new ProjectBrowserDialog();
			if (projectBrowser.ShowDialog() == false || projectBrowser.DataContext == null)
			{
				Logger.Log(MessageType.Error, $"projectBrowser.DataContext {projectBrowser.DataContext}");
				Application.Current.Shutdown();
			}
			else
			{
				Project.Current?.Unload();
				DataContext = projectBrowser.DataContext;
			}
		}
	}
}
