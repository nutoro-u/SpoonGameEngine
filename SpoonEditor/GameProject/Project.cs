using SpoonEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SpoonEditor.GameProject
{
	[DataContract(Name = Cts.Game)]
	public class Project : ViewModelBase
	{
		[DataMember]
		public string Name { get; private set; } = Cts.New_Project;

		[DataMember]
		public string Path { get; private set; }
		public string FullPath => $"{Path}{Name}{Cts.ProjectExtension}";

		[DataMember(Name = Cts.Scenes)]
		private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();
		public ReadOnlyObservableCollection<Scene> Scenes { get; private set; }

		public static Project Current => Application.Current.MainWindow.DataContext as Project;

		private Scene _activeScene;
		public Scene ActiveScene
		{
			get => _activeScene;
			set
			{
				if (_activeScene != value)
				{
					_activeScene = value;
					OnPropertyChanged(nameof(ActiveScene));
				}
			}
		}

		public ICommand AddScene { get; private set; }
		public ICommand RemoveScene { get; private set; }

		public static UndoRedo UndoRedo { get; } = new UndoRedo();

		public Project(string name, string path)
		{
			Name = name;
			Path = path;

			OnDeserialized(new StreamingContext());
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (_scenes != null)
			{
				Scenes = new ReadOnlyObservableCollection<Scene>(_scenes);
				OnPropertyChanged(nameof(Scenes));
			}
			ActiveScene = Scenes.FirstOrDefault(x => x.IsActive);

			AddScene = new RelayCommand<object>(x =>
			{
				AddSceneInternal($"{Cts.New_Scene} {_scenes.Count}");
				Scene newScene = _scenes.Last();
				int sceneIndex = _scenes.Count - 1;

				UndoRedo.Add(new UndoRedoAction(
					() => RemoveSceneInternal(newScene),
					() => _scenes.Insert(sceneIndex, newScene),
					$"Add {newScene.Name}"));
			});

			RemoveScene = new RelayCommand<Scene>(x =>
			{
				int sceneIndex = _scenes.IndexOf(x);
				RemoveSceneInternal(x);

				UndoRedo.Add(new UndoRedoAction(
					() => _scenes.Insert(sceneIndex, x),
					() => RemoveSceneInternal(x),
					$"Remove {x.Name}"));
			}, x => !x.IsActive);
		}

		public static Project Load(string file)
		{
			Debug.Assert(File.Exists(file));
			return Serializer.FromFile<Project>(file);
		}

		public static void Save(Project project)
		{
			Serializer.ToFile(project, project.FullPath);
		}

		public void Unload()
		{
		}

		private void AddSceneInternal(string sceneName)
		{
			Debug.Assert(!string.IsNullOrEmpty(sceneName.Trim()));
			_scenes.Add(new Scene(this, sceneName));
		}

		private void RemoveSceneInternal(Scene scene)
		{
			Debug.Assert(_scenes.Contains(scene));
			_scenes.Remove(scene);
		}
	}
}
