using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

        public class CyberTask
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime? Reminder { get; set; }

            public override string ToString()
            {
                string reminderText = Reminder.HasValue ? $"🔔 Reminder: {Reminder.Value:dd MMM yyyy}" : "No reminder set.";
                return $"📌 {Title}\n{Description}\n{reminderText}";
            }
        }

        private string userName;
        private string currentTopic = "";
        private Random random = new Random();
        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();
        private CyberTask pendingTask = null;
        private ObservableCollection<CyberTask> Tasks { get; } = new ObservableCollection<CyberTask>();
        private List<string> _actionLog = new List<string>();
        private const int MaxLogEntries = 10;

        private Dictionary<string, string> knownTasks = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
        { "set up two-factor authentication", "Enable 2FA on critical accounts like email and banking." },
        { "review account privacy settings", "Review account privacy settings to ensure your data is protected." },
        { "back up important data", "Back up your documents and photos to a secure location." },
        { "turn on spam and phishing filters", "Enable spam filters in your email to block malicious content." },
        { "set up a vpn for safe browsing on public networks", "Install and configure a VPN to encrypt your browsing." },
        { "run a full malware/virus scan", "Use antivirus to scan your device for threats." }
    };

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
                                             "• General cybersecurity awareness\n" +
                                             "• I can help you add cyberelated tasks - just ask 'Add task'\n\n" +
                                             "• Activity log ('show log' to view)\n\n" +
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
            // Check for exit
            if (userInput == "exit" || userInput == "quit" || userInput == "bye" || userInput == "end chat")
            {
                var farewells = new List<string>
            {
            $"Bye Bye {userName}! I'll always be here for you.",
            $"Goodbye {userName}, stay safe online!",
            $"See you next time, {userName}! Keep practicing cybersecurity.",
            $"Farewell, {userName}! I hope you learned something new.",
            $"Until next time, {userName}! Stay cyber-aware."
            };
                AddMessage("Oracle", farewells[random.Next(farewells.Count)], Brushes.Cyan);
                UserInputTextBox.IsEnabled = false;
                return;
            }

            // Greetings
            if (userInput.Contains("how are you") || userInput.Contains("how's it going"))
            {
                var responses = new List<string>
            {
            $"I feel great, {userName}! Thanks for asking.",
            $"I'm running smoothly, {userName}! No cyber threats in sight.",
            $"Cybersecurity keeps me energized, {userName}!",
            $"I'm doing well! Always excited to talk about online safety."
            };

                AddMessage("Oracle", responses[random.Next(responses.Count)], Brushes.Cyan);
                AddMessage("Oracle", "Anything you need help with cybersecurity related?", Brushes.Cyan);
                return;
            }
          
            // Check for quiz command
            if (userInput.Contains("quiz") || userInput.Contains("test") || userInput.Contains("cybersecurity quiz"))
            {
                LogAction("Started cybersecurity quiz");
                AddMessage("Oracle", $"Great idea {userName}! Let's test your cybersecurity knowledge. I'll take you to the quiz now.", Brushes.Cyan);

                // Small delay to let user read the message before navigating
                Task.Delay(1000).ContinueWith(t =>
                {
                    Dispatcher.Invoke(() => NavigationService.Navigate(new CyberQuiz(), this));
                }, TaskScheduler.FromCurrentSynchronizationContext());

                return;
            }

            // Task reminder: "4 days" or "2025-06-20"
            if (pendingTask != null)
            {
                if (userInput.StartsWith("no"))
                {
                    Tasks.Add(pendingTask);
                    AddMessage("Oracle", $"Okay, task added with no reminder.\n \"{pendingTask.Title}\" is now saved.", Brushes.Cyan);
                    pendingTask = null;
                    return;
                }            


                    if (int.TryParse(userInput.Split(' ')[0], out int reminderDays))
                    {
                        pendingTask.Reminder = DateTime.Today.AddDays(reminderDays);
                    LogAction($"Set reminder for task: {pendingTask.Title} ({reminderDays} days)");
                    Tasks.Add(pendingTask);
                        AddMessage("Oracle", $"Okay, I'll remind you in {reminderDays} day{(reminderDays == 1 ? "" : "s")}.", Brushes.Cyan);
                        pendingTask = null;
                        return;
                    }

                    if (DateTime.TryParse(userInput, out DateTime reminderDate))
                    {
                        pendingTask.Reminder = reminderDate;
                        Tasks.Add(pendingTask);
                        AddMessage("Oracle", $"Reminder set for {reminderDate:dd MMM yyyy}. Task saved.", Brushes.Cyan);
                        pendingTask = null;
                        return;
                    }

                    AddMessage("Oracle", "Sorry, I couldn't understand the reminder. Try typing a number of days like '4' or a date like '2025-06-20'.", Brushes.Cyan);
                    return;
                }

                // Choose task by number
                if (currentTopic == "choose-task" && int.TryParse(userInput, out int chosen) && chosen >= 1 && chosen <= knownTasks.Count)
                {
                    var task = knownTasks.ElementAt(chosen - 1);
                    if (Tasks.Any(t => t.Title.Equals(task.Key, StringComparison.OrdinalIgnoreCase)))
                    {
                        AddMessage("Oracle", "You’ve already added that task.", Brushes.Cyan);
                    }
                    else
                    {
                        pendingTask = new CyberTask { Title = task.Key, Description = task.Value };
                    LogAction($"Added task: {task.Key}");
                    AddMessage("Oracle", $"Task added with description \"{task.Value}\"\nWould you like a reminder?", Brushes.Cyan);
                    }
                    currentTopic = "";
                    return;
                }

                // Start guided task selection
                if (userInput.Contains("add task"))
                {
                    int index = 1;
                    StringBuilder builder = new StringBuilder("Here are tasks I can help you add:\n\n");
                    foreach (var task in knownTasks.Keys)
                    {
                        builder.AppendLine($"{index}. {task}");
                        index++;
                    }
                    builder.AppendLine("\nType the task number to add it.");
                    AddMessage("Oracle", builder.ToString(), Brushes.Cyan);
                    currentTopic = "choose-task";
                    return;
                }

                // Show all tasks
                if (userInput.Contains("show task") || userInput.Contains("my task"))
                {
                    if (Tasks.Count == 0)
                    {
                        AddMessage("Oracle", "You haven’t added any tasks yet.", Brushes.Cyan);
                    }
                    else
                    {
                        foreach (var task in Tasks)
                            AddMessage("Oracle", task.ToString(), Brushes.Cyan);
                    }
                    return;
                }

                // Delete or complete tasks
                if (userInput.Contains("delete task") || userInput.Contains("complete task"))
                {
                    if (Tasks.Count == 0)
                    {
                        AddMessage("Oracle", "No tasks to update right now.", Brushes.Cyan);
                        return;
                    }

                    string action = userInput.Contains("delete") ? "delete" : "complete";
                    StringBuilder taskList = new StringBuilder($"Which task would you like to {action}?\n");
                    for (int i = 0; i < Tasks.Count; i++)
                        taskList.AppendLine($"{i + 1}. {Tasks[i].Title}");

                    AddMessage("Oracle", taskList.ToString(), Brushes.Cyan);
                    currentTopic = action;
                    return;
                }

                if ((currentTopic == "delete" || currentTopic == "complete") && int.TryParse(userInput, out int taskIndex))
                {
                    if (taskIndex >= 1 && taskIndex <= Tasks.Count)
                    {
                        var selected = Tasks[taskIndex - 1];
                    LogAction($"{currentTopic}d task: {selected.Title}");
                    Tasks.Remove(selected);
                        AddMessage("Oracle", $"Task \"{selected.Title}\" has been {(currentTopic == "delete" ? "deleted " : "marked as complete ")}.", Brushes.Cyan);
                    }
                    else
                    {
                        AddMessage("Oracle", "That number doesn’t match any task.", Brushes.Cyan);
                    }
                    currentTopic = "";
                    return;
                }

            if (userInput.Contains("show log") || userInput.Contains("activity log"))
            {
                if (_actionLog.Count == 0)
                {
                    AddMessage("Oracle", "No activities logged yet.", Brushes.Cyan);
                }
                else
                {
                    var logMessage = new StringBuilder("Recent activities:\n\n");
                    foreach (var entry in _actionLog)
                    {
                        logMessage.AppendLine($"• {entry}");
                    }
                    AddMessage("Oracle", logMessage.ToString(), Brushes.Cyan);
                }
                return;
            }

            // Fallback chatbot responses
            string fallback = GetChatbotResponse(userInput);
                AddMessage("Oracle", fallback, Brushes.Cyan);
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

        private void LogAction(string action)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            _actionLog.Insert(0, $"[{timestamp}] {action}"); // Add newest entries at the top

            // Keep only the last 10 entries
            if (_actionLog.Count > MaxLogEntries)
            {
                _actionLog.RemoveAt(MaxLogEntries);
            }
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