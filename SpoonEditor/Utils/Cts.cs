using System;

namespace SpoonEditor.Utils
{
	public static class Cts
	{
		public const string IconFileName = "Icon.png";
		public const string ScreenshotFileName = "Screenshot.png";

		public const string IconRelativePath = ProjectHiddenFolderName + IconFileName;
		public const string ScreenshotRelativePath = ProjectHiddenFolderName + ScreenshotFileName;

		public const string ProjectExtension = ".spoonengine";
		public const string Game = "Game";

		public const string Scenes = "Scenes";
		public const string Default_Scene = "Default Scene";

		public const string ProjectHiddenFolderName = @".Spoon\";
		public const string SpoonEditor = "SpoonEditor";

		public static readonly string ApplicationDataPath =
			$@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{SpoonEditor}\";

		public const string ProjectData = "ProjectData";
		public const string ProjectDataXml = ProjectData + ".xml";
		public static readonly string ProjectDataPath = $@"{ApplicationDataPath}{ProjectDataXml}";

		public const string New_Project = "New Project";
	}
}
