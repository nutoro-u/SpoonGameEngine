using SpoonEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SpoonEditor.GameProject
{
	[DataContract]
	public class ProjectData
	{
		[DataMember]
		public string ProjectName { get; set; }
		[DataMember]
		public string ProjectPath { get; set; }
		[DataMember]
		public DateTime Date { get; set; }

		public string FullPath => $"{ProjectPath}{ProjectName}{Cts.ProjectExtension}";

		public byte[] Icon { get; set; }
		public byte[] Screenshot { get; set; }
	}

	[DataContract]
	public class ProjectDataList
	{
		[DataMember]
		public List<ProjectData> Projects { get; set; }
	}

	class OpenProject
	{
		public static readonly ObservableCollection<ProjectData> _projects = new ObservableCollection<ProjectData>();
		public static ReadOnlyObservableCollection<ProjectData> Projects { get; }

		static OpenProject()
		{
			try
			{
				if(!Directory.Exists(Cts.ApplicationDataPath))
					Directory.CreateDirectory(Cts.ApplicationDataPath);

				Projects = new ReadOnlyObservableCollection<ProjectData>(_projects);
				ReadProjectData();
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Logger.Log(MessageType.Error, $"Failed to open project | Failed to read project data | {ex.Message}");
				throw;
			}
		}

		public static Project Open(ProjectData data)
		{
			ReadProjectData();
			ProjectData project = _projects.FirstOrDefault(x => x.FullPath == data.FullPath);

			if(project != null)
			{
				project.Date = DateTime.Now;
			}
			else
			{
				project = data;
				project.Date = DateTime.Now;
				_projects.Add(project);
			}
			WriteProjectData();

			return Project.Load(project.FullPath);
		}
		private static void ReadProjectData()
		{
			if(File.Exists(Cts.ProjectDataPath))
			{
				var projects = Serializer.FromFile<ProjectDataList>(Cts.ProjectDataPath).Projects.OrderByDescending(x=>x.Date);
				_projects.Clear();
				foreach(ProjectData project in projects)
				{
					if(File.Exists(project.FullPath))
					{
						project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\{Cts.IconRelativePath}");
						project.Screenshot = File.ReadAllBytes($@"{project.ProjectPath}\{Cts.ScreenshotRelativePath}");
						_projects.Add(project);
					}
				}
			}
		}

		private static void WriteProjectData()
		{
			List<ProjectData> projects = _projects.OrderBy(x => x.Date).ToList();
			Serializer.ToFile(new ProjectDataList() { Projects = projects }, Cts.ProjectDataPath);
		}
	}
}
