using System.Windows;

namespace SpoonEditor.Utilities.Controls
{
	class ScalarBox : NumberBox
	{
		static ScalarBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ScalarBox),
				new FrameworkPropertyMetadata(typeof(ScalarBox)));
		}
	}
}
