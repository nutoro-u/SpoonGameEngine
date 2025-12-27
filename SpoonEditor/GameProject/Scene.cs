using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SpoonEditor.GameProject
{
    public class Scene : ViewModelBase
    {
		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(Name);
				}
			}
		}

		public Project Project { get; private set; }

		public Scene(Project project, string name)
		{
			Debug.Assert(project != null);
			Project = project;
			Name = name;
		}
    }
}
