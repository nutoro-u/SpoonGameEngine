using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpoonEditor.GameProject
{
	/// <summary>
	/// Interaction logic for OpenProjectView.xaml
	/// </summary>
	public partial class OpenProjectView : UserControl
	{
		public OpenProjectView()
		{
			InitializeComponent();
		}

		private void OpenButtonClicked(object sender, RoutedEventArgs e)
		{
			OpenSelectedProject();
		}

		private void ListBoxItemMouseDoubleClicked(object sender, RoutedEventArgs e)
		{
			OpenSelectedProject();
		}

		private void OpenSelectedProject()
		{
			Project project = OpenProject.Open(projectsListBox.SelectedItem as ProjectData);

			bool dialogResult = false;
			Window window = Window.GetWindow(this);
			if (project != null)
			{
				dialogResult = true;
			}
			window.DialogResult = dialogResult;
			window.Close();
		}
	}
}
