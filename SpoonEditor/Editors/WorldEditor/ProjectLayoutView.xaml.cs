using SpoonEditor.Components;
using SpoonEditor.GameProject;
using System.Windows;
using System.Windows.Controls;

namespace SpoonEditor.Editors
{
	/// <summary>
	/// Interaction logic for ProjectLayoutView.xaml
	/// </summary>
	public partial class ProjectLayoutView : UserControl
	{
		public ProjectLayoutView()
		{
			InitializeComponent();
		}

		private void OnAddEntityButtonClicked(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			Scene scene = button.DataContext as Scene;
			scene.AddGameEntityCommand.Execute(new GameEntity(scene) { Name = "New Game Entity" });
		}
	}
}
