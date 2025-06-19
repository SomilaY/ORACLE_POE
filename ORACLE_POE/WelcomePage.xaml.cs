using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ORACLE_POE
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        private static string OracleGreeting = @"assets\ORACLE_GREETING.wav";

        public WelcomePage()
        {
            InitializeComponent();
            PlayWelcomeSound();
        }

        private void PlayWelcomeSound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(OracleGreeting);
                player.Load();
                player.Play();
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error playing greeting: {error.Message}");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPage chatPage = new ChatPage();
            this.NavigationService.Navigate(chatPage);
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            CyberQuiz cyberQuiz = new CyberQuiz();
            this.NavigationService.Navigate(cyberQuiz);
        }
    }
}
