using SpoonEditor.Components;
using SpoonEditor.GameProject;
using SpoonEditor.Utils;
using System.Collections.Generic;
using System.Linq;
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

		private void OnGameEntitySelected(object sender, SelectionChangedEventArgs e)
		{
			GameEntityView.Instance.DataContext = null;
			ListBox listBox = sender as ListBox;

			if (e.AddedItems.Count > 0)
			{
				GameEntityView.Instance.DataContext = listBox.SelectedItems[0];
			}

			List<GameEntity> newSelection = listBox.SelectedItems.Cast<GameEntity>().ToList();
			List<GameEntity> LastSelection = newSelection.Except(e.AddedItems.Cast<GameEntity>()).Concat(e.RemovedItems.Cast<GameEntity>()).ToList();
			Project.UndoRedo.Add(new UndoRedoAction(
				() =>
				{
					listBox.UnselectAll();
					LastSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
				},
				() =>
				{
					listBox.UnselectAll();
					newSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
				},
				"Selection Changed"));
		}
	}
}
