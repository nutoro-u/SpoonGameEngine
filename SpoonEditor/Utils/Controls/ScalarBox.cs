using System.Windows;

namespace SpoonEditor.Utils.Controls
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
