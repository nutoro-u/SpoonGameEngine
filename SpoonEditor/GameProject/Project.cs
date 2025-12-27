using SpoonEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;

namespace SpoonEditor.GameProject
{
	[DataContract(Name = Cts.Game)]
    public class Project : ViewModelBase
	{
		[DataMember]
		public string Name { get; private set; }
		
		[DataMember]
		public string Path { get; private set; }
		public string FullPath => $"{Path}{Name}{Cts.ProjectExtension}";

		[DataMember(Name = Cts.Scenes)]
		private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();
		public ReadOnlyObservableCollection<Scene> Scenes { get; }

		public Project(string name, string path)
		{
			Name = name;
			Path = path;

			_scenes.Add(new Scene(this, Cts.Default_Scene));
		}
    }
}
