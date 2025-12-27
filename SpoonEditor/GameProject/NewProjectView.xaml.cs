using System.Windows;
using System.Windows.Controls;

namespace SpoonEditor.GameProject
{
	/// <summary>
	/// Interaction logic for NewProjectView.xaml
	/// </summary>
	public partial class NewProjectView : UserControl
	{
		public NewProjectView()
		{
			InitializeComponent();
		}

		private void OnCreateButtonClick(object sender, RoutedEventArgs e)
		{
			NewProject newProject = DataContext as NewProject;
			string projectPath = newProject.CreateProject(templateListBox.SelectedItem as ProjectTemplate);

			bool dialogResult = false;
			Window window = Window.GetWindow(this);
			if (!string.IsNullOrEmpty(projectPath))
			{
				dialogResult = true;
			}
			window.DialogResult = dialogResult;
			window.Close();
		}
	}
}
