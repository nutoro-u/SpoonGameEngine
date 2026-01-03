using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SpoonEditor.GameProject
{
	/// <summary>
	/// Interaction logic for ProjectBrowserDialg.xaml
	/// </summary>
	public partial class ProjectBrowserDialog : Window
	{
		private readonly CubicEase _easing = new CubicEase() { EasingMode = EasingMode.EaseInOut };

		public ProjectBrowserDialog()
		{
			InitializeComponent();

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoaded;
			if (!OpenProject.Projects.Any())
			{
				OpenProjectButton.IsEnabled = false;
				openProjectView.Visibility = Visibility.Hidden;
				OnToggleButtonClick(NewProjectButton, new RoutedEventArgs());
			}
		}

		private void OnToggleButtonClick(object sender, RoutedEventArgs e)
		{
			if (sender == OpenProjectButton)
			{
				if (NewProjectButton.IsChecked == true)
				{
					NewProjectButton.IsChecked = false;

					AnimateToOpenProject();
					openProjectView.IsEnabled = true;
					newProjectView.IsEnabled = false;
				}
				OpenProjectButton.IsChecked = true;
			}
			else // new project button is clicked
			{
				if (OpenProjectButton.IsChecked == true)
				{
					OpenProjectButton.IsChecked = false;

					AnimateToCreateProject();
					openProjectView.IsEnabled = false;
					newProjectView.IsEnabled = true;
				}
				NewProjectButton.IsChecked = true;
			}
		}

		private void AnimateToCreateProject()
		{
			var highlightAnimation = new DoubleAnimation(200, 400, new Duration(TimeSpan.FromSeconds(0.2)));
			highlightAnimation.EasingFunction = _easing;
			highlightAnimation.Completed += (s, e) =>
			{
				var animation = new ThicknessAnimation(new Thickness(0), new Thickness(-1600, 0, 0, 0), new Duration(TimeSpan.FromSeconds(0.5)));
				animation.EasingFunction = _easing;
				BrowserContent.BeginAnimation(MarginProperty, animation);
			};
			highlightRect.BeginAnimation(Canvas.LeftProperty, highlightAnimation);
		}

		private void AnimateToOpenProject()
		{
			var highlightAnimation = new DoubleAnimation(400, 200, new Duration(TimeSpan.FromSeconds(0.2)));
			highlightAnimation.EasingFunction = _easing;
			highlightAnimation.Completed += (s, e) =>
			{
				var animation = new ThicknessAnimation(new Thickness(-1600, 0, 0, 0), new Thickness(0), new Duration(TimeSpan.FromSeconds(0.5)));
				animation.EasingFunction = _easing;
				BrowserContent.BeginAnimation(MarginProperty, animation);
			};
			highlightRect.BeginAnimation(Canvas.LeftProperty, highlightAnimation);
		}
	}
}
