using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ORACLE_POE
{
    public partial class ChatPage : Page
    {
        private string userName;
        private Random rand = new Random();

        public ObservableCollection<ChatMessage> Messages { get; set; } = new ObservableCollection<ChatMessage>();

        public ChatPage()
        {
            InitializeComponent();
            ChatItemsControl.ItemsSource = Messages;
            AskForName();
        }

        private void AskForName()
        {
            AddMessage("ORACLE", "What is your name?", Colors.Cyan);
        }

        private void AddMessage(string sender, string message, Color color)
        {
            // Don't add empty messages
            if (string.IsNullOrWhiteSpace(message)) return;

            Messages.Add(new ChatMessage
            {
                Sender = sender + ":",
                Message = message,
                SenderColor = color
            });
            ChatScrollViewer.ScrollToEnd();
        }

        private async Task SlowType(string sender, string message, Color color)
        {
            // Add the initial message with empty text
            var chatMessage = new ChatMessage
            {
                Sender = sender + ":",
                Message = "",
                SenderColor = color
            };
            Messages.Add(chatMessage);

            // Type out the message character by character
            foreach (char c in message)
            {
                chatMessage.Message += c;
                await Task.Delay(20);
                ChatScrollViewer.ScrollToEnd();
            }
        }

        private void ProcessUserInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput)) return;

            // Add user message to chat
            AddMessage(userName ?? "You", userInput, Colors.White);

            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = userInput.Trim();
                StartChat();
                return;
            }

            // Handle other commands...
            if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                HandleExitCommand();
                return;
            }

            // Add your other chatbot logic here
            // Example:
            AddMessage("ORACLE", "I understand you said: " + userInput, Colors.Cyan);
        }

        private async void StartChat()
        {
            await SlowType("ORACLE", $"Hello {userName}! It's nice to meet you. I'm Oracle.", Colors.Cyan);
            await SlowType("ORACLE", "╔═════════════════════════════════", Colors.Cyan);
            await SlowType("ORACLE", "║          WHAT I COVER!         ║", Colors.Cyan);
            await SlowType("ORACLE", "╚════════════════════════════════╝", Colors.Cyan);
            await SlowType("ORACLE", "✅ Password security", Colors.Cyan);
            await SlowType("ORACLE", "✅ Phishing scams", Colors.Cyan);
            await SlowType("ORACLE", "✅ Safe browsing techniques", Colors.Cyan);
            await SlowType("ORACLE", "✅ Recognizing suspicious links", Colors.Cyan);
            await SlowType("ORACLE", "✅ General cybersecurity awareness", Colors.Cyan);
            await SlowType("ORACLE", "I'm here to assist you with anything cybersecurity related.", Colors.Cyan);
        }

        private void HandleExitCommand()
        {
            var farewellMessages = new List<string>
            {
                $"Bye Bye {userName}! I'll always be here for you.",
                $"Goodbye {userName}, stay safe online!",
                $"See you next time, {userName}! Keep practicing cybersecurity.",
                $"Farewell, {userName}! I hope you learned something new.",
                $"Until next time, {userName}! Stay cyber-aware."
            };

            AddMessage("ORACLE", farewellMessages[rand.Next(farewellMessages.Count)], Colors.Cyan);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput(UserInputTextBox.Text);
            UserInputTextBox.Clear();
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput(UserInputTextBox.Text);
                UserInputTextBox.Clear();
            }
        }
    }

    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public Color SenderColor { get; set; }
    }
}