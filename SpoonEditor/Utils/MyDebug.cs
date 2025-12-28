using System.Diagnostics;

namespace SpoonEditor.Utils
{
	public static class MyDebug
	{
		public static void WriteLine(string? message)
		{
			Debug.WriteLine(message);
			// TODO log error
		}
	}
}
