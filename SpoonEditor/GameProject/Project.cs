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
	class Project : ViewModelBase
	{
		[DataMember]
		public string Name { get; private set; } = Cts.New_Project;

		[DataMember]
		public string Path { get; private set; }
		public string FullPath => $@"{Path}{Name}\{Name}{Cts.ProjectExtension}";

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
		public ICommand UndoCommand { get; private set; }
		public ICommand RedoCommand { get; private set; }


		public ICommand AddSceneCommand { get; private set; }
		public ICommand RemoveSceneCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }

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

			AddSceneCommand = new RelayCommand<object>(x =>
			{
				AddScene($"{Cts.New_Scene} {_scenes.Count}");
				Scene newScene = _scenes.Last();
				int sceneIndex = _scenes.Count - 1;

				UndoRedo.Add(new UndoRedoAction(
					() => RemoveScene(newScene),
					() => _scenes.Insert(sceneIndex, newScene),
					$"Add {newScene.Name}"));
			});

			RemoveSceneCommand = new RelayCommand<Scene>(x =>
			{
				int sceneIndex = _scenes.IndexOf(x);
				RemoveScene(x);

				UndoRedo.Add(new UndoRedoAction(
					() => _scenes.Insert(sceneIndex, x),
					() => RemoveScene(x),
					$"Remove {x.Name}"));
			}, x => !x.IsActive);

			UndoCommand = new RelayCommand<object>(x => UndoRedo.Undo());
			RedoCommand = new RelayCommand<object>(x => UndoRedo.Redo());
			SaveCommand = new RelayCommand<object>(x => Save(this));
		}

		public static Project Load(string file)
		{
			Debug.Assert(File.Exists(file));
			return Serializer.FromFile<Project>(file);
		}

		public static void Save(Project project)
		{
			Serializer.ToFile(project, project.FullPath);
			Logger.Log(MessageType.Info, $"Saved project to {project.FullPath}");
		}

		public void Unload()
		{
			UndoRedo.Reset();
		}

		private void AddScene(string sceneName)
		{
			Debug.Assert(!string.IsNullOrEmpty(sceneName.Trim()));
			_scenes.Add(new Scene(this, sceneName));
		}

		private void RemoveScene(Scene scene)
		{
			Debug.Assert(_scenes.Contains(scene));
			_scenes.Remove(scene);
		}
	}
}
