using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Shapes;

namespace SpoonEditor.Utils
{
    public static class Serializer
    {
		public static void ToFile<T>(T instance, string path)
		{
			try
			{
				using var fs = new FileStream(path, FileMode.Create);
				var serializer = new DataContractSerializer(typeof(T));
				serializer.WriteObject(fs, instance);
			}
			catch(Exception ex)
			{
				MyDebug.WriteLine(ex.Message);
			}
		}

		public static T FromFile<T>(string path)
		{
			try
			{
				using var fs = new FileStream(path, FileMode.Open);
				var serializer = new DataContractSerializer(typeof(T));
				T instance = (T)serializer.ReadObject(fs);
				return instance;
			}
			catch (Exception ex)
			{
				MyDebug.WriteLine(ex.Message);
				return default(T);
			}
		}
	}
}
