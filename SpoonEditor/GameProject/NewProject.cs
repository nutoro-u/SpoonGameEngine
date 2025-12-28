using SpoonEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public byte[] Icon { get; set; }
		public byte[] Screenshot { get; set; }
		public string IconFilepath { get; set; }
		public string ScreenshotFilepath { get; set; }
		public string ProjectFilePath { get; set; }
	}

	class NewProject : ViewModelBase
	{
		// TODO get the path from the installation location
		private string _templatesPath = @"..\..\SpoonEditor\ProjectTemplates";

		private string _projectName = "NewProject";
		public string ProjectName
		{
			get => _projectName;
			set
			{
				if (_projectName != value)
				{
					_projectName = value;
					ValidateProjectPath();
					OnPropertyChanged(nameof(ProjectName));
				}
			}
		}

		private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\SpoonProjects\";
		public string ProjectPath
		{
			get => _projectPath;
			set
			{
				if (_projectPath != value)
				{
					_projectPath = value;
					ValidateProjectPath();
					OnPropertyChanged(nameof(ProjectPath));
				}
			}
		}

		private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
		public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
		{ get; }

		private bool _isValid = false;
		public bool IsValid
		{
			get => _isValid;
			set
			{
				if (_isValid != value)
				{
					_isValid = value;
					OnPropertyChanged(nameof(IsValid));
				}
			}
		}

		private string _errorMsg;
		public string ErrorMsg
		{
			get => _errorMsg;
			set
			{
				if (_errorMsg != value)
				{
					_errorMsg = value;
					OnPropertyChanged(nameof(ErrorMsg));
				}
			}
		}

		private bool ValidateProjectPath()
		{
			string path = ProjectPath;
			if (!Path.EndsInDirectorySeparator(path))
				path += @"\";

			path += $@"{ProjectName}\";

			IsValid = false;
			if (string.IsNullOrWhiteSpace(ProjectName.Trim()))
			{
				ErrorMsg = "Type in a project name.";
			}
			else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				ErrorMsg = "Invalid characters in project name.";
			}
			else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
			{
				ErrorMsg = "Type in a project path.";
			}
			else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				ErrorMsg = "Invalid characters in project path.";
			}
			else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
			{
				ErrorMsg = "Selected project folder already exists and not empty";
			}
			else
			{
				ErrorMsg = string.Empty;
				IsValid = true;
			}

			return IsValid;
		}

		public string CreateProject(ProjectTemplate template)
		{
			ValidateProjectPath();
			if(!IsValid)
				return string.Empty;

			if (!Path.EndsInDirectorySeparator(ProjectPath))
				ProjectPath += @"\";

			string path = $@"{ProjectPath}{ProjectName}\";

			try
			{
				if(!Directory.Exists(path))
					Directory.CreateDirectory(path);
				foreach(string folder in template.Folders)
				{
					Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), folder)));
				}
				DirectoryInfo dirIfo = new DirectoryInfo(path + Cts.ProjectHiddenFolderName);
				dirIfo.Attributes |= FileAttributes.Hidden;
				File.Copy(template.IconFilepath, Path.GetFullPath(Path.Combine(dirIfo.FullName, Cts.IconFileName)));
				File.Copy(template.ScreenshotFilepath, Path.GetFullPath(Path.Combine(dirIfo.FullName, Cts.ScreenshotFileName)));

				string projectXml = File.ReadAllText(template.ProjectFilePath);
				projectXml = string.Format(projectXml, ProjectName, ProjectPath);
				string projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Cts.ProjectExtension}"));
				File.WriteAllText(projectPath, projectXml);

				return path;
			}
			catch (Exception ex)
			{
				MyDebug.WriteLine(ex.Message);
				return string.Empty;
			}
		}

		public NewProject()
		{
			ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
			try
			{
				string[] templateFiles = Directory.GetFiles(_templatesPath, "template.xml", SearchOption.AllDirectories);
				Debug.Assert(templateFiles.Any());
				foreach (string file in templateFiles)
				{
					ProjectTemplate template = Serializer.FromFile<ProjectTemplate>(file);
					template.IconFilepath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), Cts.IconFileName));
					template.Icon = File.ReadAllBytes(template.IconFilepath);
					template.ScreenshotFilepath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), Cts.ScreenshotFileName));
					template.Screenshot = File.ReadAllBytes(template.ScreenshotFilepath);

					template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));
					_projectTemplates.Add(template);
				}
				ValidateProjectPath();
			}
			catch (Exception ex)
			{
				MyDebug.WriteLine(ex.Message);
			}
		}
	}
}
