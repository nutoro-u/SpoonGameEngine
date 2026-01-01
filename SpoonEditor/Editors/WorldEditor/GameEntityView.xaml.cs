using SpoonEditor.Components;
using SpoonEditor.GameProject;
using SpoonEditor.Utils;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpoonEditor.Editors
{
	/// <summary>
	/// Interaction logic for GameEntityView.xaml
	/// </summary>
	public partial class GameEntityView : UserControl
	{
		private string _propertyName;
		private Action _undoAction;

		public static GameEntityView Instance { get; private set; }

		public GameEntityView()
		{
			InitializeComponent();
			DataContext = null;
			Instance = this;

			DataContextChanged += (_, __) =>
			{
				if(DataContext != null)
				{
					(DataContext as MSEntity).PropertyChanged += (s,e) => _propertyName = e.PropertyName;
				}
			};
		}

		private Action GetRenameAction()
		{
			MSEntity msEntity = DataContext as MSEntity;
			var selection = msEntity.SelectedEntities.Select(entity => (entity, entity.Name)).ToList();
			return new Action(() =>
			{
				selection.ForEach(item => item.entity.Name = item.Name);
				(DataContext as MSEntity).Refresh();
			});
		}
		private Action GetEnableAction()
		{
			MSEntity msEntity = DataContext as MSEntity;
			var selection = msEntity.SelectedEntities.Select(entity => (entity, entity.IsEnabled)).ToList();
			return new Action(() =>
			{
				selection.ForEach(item => item.entity.IsEnabled = item.IsEnabled);
				(DataContext as MSEntity).Refresh();
			});
		}

		private void OnNameTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			_undoAction = GetRenameAction();
		}

		private void OnNameTextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if(_propertyName == nameof(MSEntity.Name) && _undoAction != null)
			{
				Action redoAction = GetRenameAction();
				Project.UndoRedo.Add(new UndoRedoAction(_undoAction, redoAction, "Rename game entity"));
				_propertyName = null;
			}
			_undoAction = null;
		}

		private void OnIsEnabledClick(object sender, RoutedEventArgs e)
		{
			var undoAction = GetEnableAction();
			MSEntity msEntity = DataContext as MSEntity;
			msEntity.IsEnabled = (sender as CheckBox).IsChecked == true;

			var redoAction = GetEnableAction();
			Project.UndoRedo.Add(new UndoRedoAction(undoAction, redoAction,
				msEntity.IsEnabled == true ? "Enable game entity" : "Disable game entity"));
		}
	}
}
