using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpoonEditor.GameProject
{
	/// <summary>
	/// Interaction logic for ProjectBrowserDialg.xaml
	/// </summary>
	public partial class ProjectBrowserDialog : Window
	{
		public ProjectBrowserDialog()
		{
			InitializeComponent();

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoaded;
			if(!OpenProject.Projects.Any())
			{
				OpenProjectButton.IsEnabled = false;
				openProjectView.Visibility = Visibility.Hidden;
				OnToggleButtonClick(NewProjectButton, new RoutedEventArgs());
			}
		}

		private void OnToggleButtonClick(object sender, RoutedEventArgs e)
		{
			if (sender == OpenProjectButton)
			{
				if (NewProjectButton.IsChecked == true)
				{
					NewProjectButton.IsChecked = false;
					BrowserContent.Margin = new Thickness(0);
				}
				OpenProjectButton.IsChecked = true;
			}
			else // new project button is clicked
			{
				if (OpenProjectButton.IsChecked == true)
				{
					OpenProjectButton.IsChecked = false;
					BrowserContent.Margin = new Thickness(-800, 0, 0, 0);
				}
				NewProjectButton.IsChecked = true;
			}
		}
	}
}
