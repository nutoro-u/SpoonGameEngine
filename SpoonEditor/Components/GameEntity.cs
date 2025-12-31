using SpoonEditor.GameProject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace SpoonEditor.Components
{
	[DataContract]
	[KnownType(typeof(Transform))]
    public class GameEntity : ViewModelBase
	{
		private string _name;

		[DataMember]
		public string Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(nameof(Name));
				}
			}
		}
	
		[DataMember]
		public Scene ParentScene { get; private set; }

		[DataMember(Name = nameof(Components))]
		private readonly ObservableCollection<Component> _components = new ObservableCollection<Component>();
		public ReadOnlyObservableCollection<Component> Components { get; private set; }

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if(Components != null)
			{
				Components = new ReadOnlyObservableCollection<Component>(_components);
				OnPropertyChanged(nameof(Components));
			}
		}

		public GameEntity(Scene parentScene)
		{
			Debug.Assert(parentScene != null);
			ParentScene = parentScene;
			_components.Add(new Transform(this));
		}
	}
}
