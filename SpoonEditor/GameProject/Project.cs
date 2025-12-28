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

		public Project(string name, string path)
		{
			Name = name;
			Path = path;

			OnDeserialized(new StreamingContext());
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if(_scenes != null)
			{
				Scenes = new ReadOnlyObservableCollection<Scene>(_scenes);
				OnPropertyChanged(nameof(Scenes));
			}
			ActiveScene = Scenes.FirstOrDefault(x => x.IsActive);
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
    }
}
