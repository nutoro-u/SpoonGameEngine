using SpoonEditor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace SpoonEditor.GameProject
{
	[DataContract]
	public class ProjectTemplate
	{
		[DataMember]
		public string ProjectType { get; set; }
		
		[DataMember]
		public string ProjectFile { get; set; }
		
		[DataMember]
		public List<string> Folders { get; set; }
	}

	class NewProject : ViewModelBase
	{
		// TODO get the path from the installation location
		private string _templatesPath = @"..\..\SpoonEditor\ProjectTemplates";

		private string _name = "NewProject";
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

		private string _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\SpoonProjects\";
		public string Path
		{
			get => _path;
			set
			{
				if (_path != value)
				{
					_path = value;
					OnPropertyChanged(nameof(Path));
				}
			}
		}

		public NewProject()
		{
			try
			{
				string[] templateFiles = Directory.GetFiles(_templatesPath, "template.xml", SearchOption.AllDirectories);
				Debug.Assert(templateFiles.Any());
				foreach (string file in templateFiles)
				{
					ProjectTemplate template = new ProjectTemplate()
					{
						ProjectType = "Empty Project",
						ProjectFile = "Project.spoonengine",
						Folders = new List<string> { ".Spoon", "Content", "GameCode"},
					};

					Serializer.ToFile(template, file);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				// TODO log error
			}
		}
	}
}
