using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ORACLE_POE
{
    public partial class ChatPage : Page
    {
        public class ChatMessage
        {
            public string Sender { get; set; }
            public string Message { get; set; }
            public Brush SenderColor { get; set; }
        }

        private string userName;
        private string currentTopic = "";
        private Random random = new Random();
        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public ChatPage()
        {
            InitializeComponent();
            ChatItemsControl.ItemsSource = Messages;
            StartChat();
        }

        public void StartChat()
        {
            AddMessage("Oracle", "What is your name?", Brushes.Cyan);
        }


        private void AddMessage(string sender, string message, Brush color)
        {
            Messages.Add(new ChatMessage
            {
                Sender = sender,
                Message = message,
                SenderColor = color
            });

            // Scroll to bottom
            ChatScrollViewer.ScrollToEnd();
        }

        private void ProcessUserInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            string userInput = input.ToLower();

            // Add user message to chat - using userName instead of "You"
            AddMessage(userName ?? "You", input, Brushes.White);

            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = input.Trim();
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    string welcomeMessage = $"Hello {userName}! It's nice to meet you. I'm Oracle.\n\n" +
                                             "WHAT I COVER:\n" +
                                             "\n• Password security\n" +
                                             "• Phishing scams\n" +
                                             "• Safe browsing techniques\n" +
                                             "• Recognizing suspicious links\n" +
                                             "• General cybersecurity awareness\n\n" +
                                             "I'm here to assist you with anything cybersecurity related.";

                    AddMessage("Oracle", welcomeMessage, Brushes.Cyan);
                    ChatLoop();
                }
                else
                {
                    AddMessage("Oracle", "I didn't get your name my friend. What is your name?", Brushes.Cyan);
                }
            }
            else
            {
                ProcessChatInput(userInput);
            }
        }

        private void ChatLoop()
        {
            AddMessage("Oracle", "How can I help you with cybersecurity today?", Brushes.Cyan);
        }

        private void ProcessChatInput(string userInput)
        {
            // Check for exit commands
            if (userInput == "exit" ||
                userInput == "quit" ||
                userInput == "goodbye" ||
                userInput == "bye" ||
                userInput == "i'm done" ||
                userInput == "leave" ||
                userInput == "end chat")
            {
                List<string> farewellMessages = new List<string>
                {
                    $"Bye Bye {userName}! I'll always be here for you.",
                    $"Goodbye {userName}, stay safe online!",
                    $"See you next time, {userName}! Keep practicing cybersecurity.",
                    $"Farewell, {userName}! I hope you learned something new.",
                    $"Until next time, {userName}! Stay cyber-aware."
                };

                int index = random.Next(farewellMessages.Count);
                AddMessage("Oracle", farewellMessages[index], Brushes.Cyan);

                // Optionally close the window or disable input
                UserInputTextBox.IsEnabled = false;
                return;
            }
            // Check for greetings/how are you
            else if (userInput.Contains("how are you") ||
                     userInput.Contains("how's it going") ||
                     userInput.Contains("how do you feel") ||
                     userInput.Contains("are you okay") ||
                     userInput.Contains("what's up"))
            {
                List<string> responses = new List<string>
                {
                    $"I feel great, {userName}! Thanks for asking.",
                    $"I'm running smoothly, {userName}! No cyber threats in sight.",
                    $"Cybersecurity keeps me energized, {userName}!",
                    $"I'm doing well! Always excited to talk about online safety."
                };

                int index = random.Next(responses.Count);
                AddMessage("Oracle", responses[index], Brushes.Cyan);
                AddMessage("Oracle", "Anything you need help with cybersecurity related?", Brushes.Cyan);
            }
            else
            {
                // Process other cybersecurity topics
                string response = GetChatbotResponse(userInput);
                AddMessage("Oracle", response, Brushes.Cyan);
            }
        }

        private string GetChatbotResponse(string input)
        {
            if (input.Contains("password"))
            {
                return "Strong passwords should be:\n" +
                       "- At least 12 characters long\n" +
                       "- Include a mix of letters, numbers, and symbols\n" +
                       "- Unique for each account";
            }
            else if (input.Contains("phishing"))
            {
                return "Phishing scams often:\n" +
                       "- Create a sense of urgency\n" +
                       "- Mimic legitimate organizations\n" +
                       "- Contain suspicious links or attachments\n\n" +
                       "Always verify unexpected requests for sensitive information.";
            }
            // Add more response logic as needed

            return "I'm not sure I understand. Could you rephrase your question about cybersecurity?";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInputTextBox.Text;
            UserInputTextBox.Clear();
            ProcessUserInput(input);
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string input = UserInputTextBox.Text;
                UserInputTextBox.Clear();
                ProcessUserInput(input);
            }
        }
    }
}