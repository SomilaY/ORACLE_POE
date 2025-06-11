using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ORACLE_POE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
        }

        private async void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            await ShowRetroLoadingAsync();
            MainFrame.Navigate(new WelcomePage());
        }

        private async Task ShowRetroLoadingAsync()
        {
            // Create loading container
            var loadingGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromRgb(15, 25, 35)), // #0F1923
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // Create loading text
            var loadingText = new TextBlock
            {
                Text = "[LOADING]",
                Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 255)), // #00C8FF
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, -50, 0, 0)
            };

            // Create progress bar container
            var progressContainer = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };

            loadingGrid.Children.Add(loadingText);
            loadingGrid.Children.Add(progressContainer);
            MainFrame.Content = loadingGrid;

            // Create animated blocks
            for (int i = 0; i < 15; i++)
            {
                var block = new Rectangle
                {
                    Width = 20,
                    Height = 10,
                    Fill = new SolidColorBrush(Color.FromRgb(0, 200, 255)), // #00C8FF
                    Margin = new Thickness(2, 0, 2, 0),
                    Opacity = 0
                };

                // Create animation
                var animation = new DoubleAnimation
                {
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(300),
                    BeginTime = TimeSpan.FromMilliseconds(i * 200)
                };

                progressContainer.Children.Add(block);
                block.BeginAnimation(OpacityProperty, animation);
            }

            // Wait for animation to complete
            await Task.Delay(15 * 200 + 500); // 15 blocks * 200ms + buffer
        }
    }
}