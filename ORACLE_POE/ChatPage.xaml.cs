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
        public class ChatMessage //this method controls how every chat bubble looks so it doesnt have to be set again every time
        {
            public string Sender { get; set; } 
            public string Message { get; set; }
            public Brush SenderColor { get; set; }
        }

        public class CyberTask //Keeps all task info together and shows them in a readable format
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime? Reminder { get; set; }

            public override string ToString()
            {
                string reminderText = Reminder.HasValue ? $"Reminder: {Reminder.Value:dd MMM yyyy}" : "No reminder set.";
                return $"{Title}\n{Description}\n{reminderText}";
            }
        }

        private string userName;
        private string currentTopic = "";
        private Random random = new Random();
        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>(); //a dynamic list to store messages and keep them updated at all times
        private CyberTask pendingTask = null;
        private ObservableCollection<CyberTask> Tasks { get; } = new ObservableCollection<CyberTask>(); //a dynamic list that keeps track of the tasks
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
        }; //list of known cybersecurity tasks and their descriptions

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


        private void AddMessage(string sender, string message, Brush color) //adds a new message whether from the user or Oracle and what colour it should be
        {
            Messages.Add(new ChatMessage
            {
                Sender = sender,
                Message = message,
                SenderColor = color
            });

            // Scroll to bottom automatically
            ChatScrollViewer.ScrollToEnd();
        }

        private void ProcessUserInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) //checks if the user types nothing
                return;

            string userInput = input.ToLower();
            AddMessage(userName ?? "You", input, Brushes.White); //if the chatbot doesnt know the users name it uses "You"

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
                                             "• I can help you add cyberelated tasks - just ask 'Add task'\n" +
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

            //purpose
            else if (userInput.Contains("purpose") || userInput.Contains("what do you do") || userInput.Contains("why are you here") ||
                     userInput.Contains("your goal") ||userInput.Contains("what can i ask you") || userInput.Contains("topics") ||
                     userInput.Contains("what do you know"))
            {
                List<string> purposeResponses = new List<string>
            {
                $"I love this question, {userName}!\nI'm here to make sure your digital life stays protected.",
                $"Great question, {userName}! My mission is to help you navigate the world of cybersecurity safely.",
                $"Cybersecurity is my specialty, {userName}! I'm here to help you avoid scams and stay secure online.",
                $"I exist to make sure you never fall victim to online threats, {userName}! Let's keep your data safe.",
                $"My goal? To arm you with knowledge about cybersecurity so you're always one step ahead of scammers, {userName}."
            };

                int index = random.Next(purposeResponses.Count);
                string response = purposeResponses[index] + "\n\n" +
                                  "WHAT I COVER:\n\n" +
                                  "• Password security\n" +
                                  "• Phishing scams\n" +
                                  "• Safe browsing techniques\n" +
                                  "• Recognizing suspicious links\n" +
                                  "• General cybersecurity awareness\n\n" +
                                  "Is there anything specific you'd like help with?";

                AddMessage("Oracle", response, Brushes.Cyan);
                return;
            }

            else if (userInput.Contains("password") || userInput.Contains("security") || userInput.Contains("protect") || userInput.Contains("secure"))
            {
                currentTopic = "password";
                StringBuilder passwordResponse = new StringBuilder();

                // Detect sentiment
                if (userInput.Contains("worried") || userInput.Contains("frustrated") || userInput.Contains("confused") ||
                    userInput.Contains("stressed") || userInput.Contains("overwhelmed") || userInput.Contains("anxious") ||
                    userInput.Contains("nervous") || userInput.Contains("upset") || userInput.Contains("uncertain") ||
                    userInput.Contains("lost"))
                {
                    passwordResponse.AppendLine("I understand that cybersecurity can feel overwhelming, but you're not alone. I'll guide you through it step by step. You've got this!\n");
                }
                else if (userInput.Contains("excited") || userInput.Contains("happy") || userInput.Contains("love") ||
                         userInput.Contains("awesome") || userInput.Contains("cool") || userInput.Contains("great") ||
                         userInput.Contains("fantastic") || userInput.Contains("amazing") || userInput.Contains("enthusiastic"))
                {
                    passwordResponse.AppendLine("I love your enthusiasm! Cybersecurity knowledge is empowering—let's explore something fun today!\n");
                }

                // Add random password introduction
                List<string> passwordResponses = new List<string>
                {
                 $"I'm so excited to teach you about passwords, {userName}!",
                 $"Passwords are your first line of defense, {userName}! Let's make them stronger.",
                 $"A strong password keeps your data safe from cyber threats. Here's how to create one!",
                 $"Protecting your information starts with a strong password. Let's go over the basics!"
                };
                passwordResponse.AppendLine(passwordResponses[random.Next(passwordResponses.Count)]);

                // Add password essentials
                passwordResponse.AppendLine("\nThe importance of a password is to protect your personal information in the digital world.");
                passwordResponse.AppendLine($"You need a strong password, {userName}, because it helps protect you from scammers and hackers.\n");

                passwordResponse.AppendLine("PASSWORD SECURITY ESSENTIALS:\n");

                // Add random password tips
                List<string> passwordTips = new List<string>
                {
                 "• Make your password at least 12-16 characters long for stronger security.",
                 "• Use a passphrase instead of a random password (e.g., 'ChickenOvaB33f!').",
                 "• Include a mix of uppercase & lowercase letters, numbers, and special characters.",
                 "• Never reuse passwords across different accounts.",
                 "• Consider using a password manager to securely store your passwords.",
                 "• Enable multi-factor authentication (MFA) for extra security.",
                 "• Beware of password-sharing scams. Never share your credentials!"
                };

                // Select 3 random tips
                var selectedTips = passwordTips.OrderBy(x => random.Next()).Take(3).ToList();
                foreach (var tip in selectedTips)
                {
                    passwordResponse.AppendLine(tip);
                }

                passwordResponse.AppendLine($"\nExample: '1Luv{userName}!###'\n");
                passwordResponse.AppendLine("Is there anything else you'd like to explore?");

                AddMessage("Oracle", passwordResponse.ToString(), Brushes.Cyan);
                return;
            }

            // Add phishing/scam handler
            else if (userInput.Contains("phishing") || userInput.Contains("scam") || userInput.Contains("fraud"))
            {
                currentTopic = "phishing";
                StringBuilder phishingResponse = new StringBuilder();

                // Detect sentiment
                if (userInput.Contains("worried") || userInput.Contains("frustrated") || userInput.Contains("confused") ||
                    userInput.Contains("stressed") || userInput.Contains("overwhelmed") || userInput.Contains("anxious") ||
                    userInput.Contains("nervous") || userInput.Contains("upset") || userInput.Contains("uncertain") ||
                    userInput.Contains("lost"))
                {
                    phishingResponse.AppendLine("Phishing scams can be stressful, but don't worry! I'll guide you through it so you can stay safe and secure. You've got this!\n");
                }

                // Add random phishing introduction
                List<string> phishingResponses = new List<string>
                {
                 $"I can't stand phishing scams, {userName}! They're like digital pickpockets, sneaking into your inbox.",
                 $"Phishing scams are everywhere, {userName}! They disguise themselves as legitimate messages, but they're out to steal your info.",
                 $"Scammers are sneaky, {userName}! They create fake emails and links to trick people into handing over their personal details.",
                 $"Online fraud is a serious threat, {userName}. Let's go over how to spot and avoid phishing scams!"
                };

                phishingResponse.AppendLine(phishingResponses[random.Next(phishingResponses.Count)]);
                phishingResponse.AppendLine("\nScammers impersonate trusted organizations, hoping you'll fall for their tricks.");
                phishingResponse.AppendLine($"But don't worry, {userName}, I'll help you spot the red flags so they don't get you!\n");
                phishingResponse.AppendLine("SPOTTING A PHISHING SCAM:\n");

                // Add random phishing red flags
                List<string> phishingRedFlags = new List<string>
                {
                 $"• Emails starting with \"Dear Customer\" instead of \"Dear {userName}\"",
                  "• Unknown attachments you weren't expecting",
                  "• Misspelled domain names that look similar to real ones",
                  "• Urgent language like \"Claim your prize immediately!\", \"If you don't update your account, it will be deleted\"",
                  "• Links that look trustworthy but redirect to fraudulent sites"
                };

                var selectedRedFlags = phishingRedFlags.OrderBy(x => random.Next()).Take(3).ToList();
                foreach (var redFlag in selectedRedFlags)
                {
                    phishingResponse.AppendLine(redFlag);
                }

                phishingResponse.AppendLine("\nHOW TO STAY PROTECTED:\n");

                // Add random prevention tips
                List<string> phishingPreventionTips = new List<string>
                {
                 "• Add an extra layer of security like Two-Factor Authentication (2FA)",
                 "• Never open strange attachments or links from unknown senders",
                 "• Double-check email sender addresses—scammers use subtle variations",
                 "• Hover over links before clicking to verify their real destination",
                 "• Never share your password, no matter who asks for it",
                 "• Use a password manager to prevent phishing attempts",
                 "• Be wary of QR codes—fraudsters embed malicious links inside them"
                };

                var selectedPreventionTips = phishingPreventionTips.OrderBy(x => random.Next()).Take(3).ToList();
                foreach (var tip in selectedPreventionTips)
                {
                    phishingResponse.AppendLine(tip);
                }

                phishingResponse.AppendLine($"\nAnything else you need, {userName}?");
                AddMessage("Oracle", phishingResponse.ToString(), Brushes.Cyan);
                return;
            }

            else if (userInput.Contains("safe browsing") || userInput.Contains("browsing safely") || userInput.Contains("safe"))
            {
                currentTopic = "safe browsing";
                StringBuilder safeBrowsingResponse = new StringBuilder();

                // Detect sentiment
                if (userInput.Contains("worried") || userInput.Contains("frustrated") || userInput.Contains("confused") ||
                    userInput.Contains("stressed") || userInput.Contains("overwhelmed") || userInput.Contains("anxious") ||
                    userInput.Contains("nervous") || userInput.Contains("upset") || userInput.Contains("uncertain") ||
                    userInput.Contains("lost"))
                {
                    safeBrowsingResponse.AppendLine("I totally understand how scary this all can be, but don't worry—I'm here to help.\n");
                }

                // Add random safe browsing introduction
                List<string> safeBrowsingResponses = new List<string>
                {
                 $"Safe browsing is all about protecting yourself while exploring the vast web, {userName}.",
                 $"Navigating the internet safely is key, {userName}. Let's make sure you stay secure online.",
                 $"Scammers and hackers are lurking everywhere, {userName}. Let me show you how to browse safely.",
                 $"Cyber threats are out there, {userName}, but don't worry—I've got your back.",
                 $"The internet is a great place, {userName}, but it can also be risky. Here's how to stay safe."
                };

                safeBrowsingResponse.AppendLine(safeBrowsingResponses[random.Next(safeBrowsingResponses.Count)]);
                safeBrowsingResponse.AppendLine("\nHOW TO BROWSE SAFELY:\n");

                // Add random safe browsing tips
                List<string> safeBrowsingTips = new List<string>
                {
                 "• Always check for HTTPS in the address bar—this means the site is secure.",
                 "• Double-check URLs before entering personal info—scammers create fake sites that look identical to real ones.",
                 "• Avoid public Wi-Fi for sensitive tasks—hackers can get your data on unsecured networks.",
                 "• Never download files from unknown sources—they could contain viruses or malware.",
                 "• Keep your browser updated regularly—updates address security vulnerabilities.",
                 "• Don't click on suspicious pop-ups or ads—these often lead to scams or malicious sites.",
                 "• Use a secure search engine that prioritizes privacy.",
                 "• Enable safe browsing settings in your browser for added protection.",
                 "• Always verify the legitimacy of links before clicking—hover over them to check the true destination."
                };

                var selectedTips = safeBrowsingTips.OrderBy(x => random.Next()).Take(3).ToList();
                foreach (var tip in selectedTips)
                {
                    safeBrowsingResponse.AppendLine(tip);
                }

                safeBrowsingResponse.AppendLine($"\nAnything else you need, {userName}?");
                AddMessage("Oracle", safeBrowsingResponse.ToString(), Brushes.Cyan);
                return;
            }

            // Add suspicious links handler
            else if (userInput.Contains("suspicious links") || userInput.Contains("unsafe links") || userInput.Contains("links") || userInput.Contains("weird links"))
            {
                currentTopic = "suspicious links";
                StringBuilder suspiciousLinksResponse = new StringBuilder();

                // Detect sentiment
                if (userInput.Contains("worried") || userInput.Contains("frustrated") || userInput.Contains("confused") ||
                    userInput.Contains("stressed") || userInput.Contains("overwhelmed") || userInput.Contains("anxious") ||
                    userInput.Contains("nervous") || userInput.Contains("upset") || userInput.Contains("uncertain") ||
                    userInput.Contains("lost"))
                {
                    suspiciousLinksResponse.AppendLine($"Criminals are a scary threat on the internet, but don't worry {userName}, I've got you.\n");
                }

                // Add random suspicious links introduction
                List<string> suspiciousLinkResponses = new List<string>
                {
                 $"Great! I love this topic, {userName}. Recognizing shady links is key to avoiding scams online.",
                 $"Hackers love using fake links, {userName}. Let's break down how to identify them.",
                 $"Scammers are always finding new ways to disguise dangerous links, {userName}. Here's how to stay safe.",
                 $"Suspicious links can lead to phishing attacks, {userName}. Let's go over how to recognize them."
                };

                suspiciousLinksResponse.AppendLine(suspiciousLinkResponses[random.Next(suspiciousLinkResponses.Count)]);
                suspiciousLinksResponse.AppendLine("\nHOW TO SPOT SUSPICIOUS LINKS:\n");

                // Add random suspicious link red flags
                List<string> suspiciousLinkRedFlags = new List<string>
                {
                 "• Check for misspellings – Scammers use fake domains like \"Go0gle.com\" instead of \"Google.com\".",
                 "• Watch out for shortened URLs – Hackers hide dangerous links behind URL shorteners like bit.ly or tinyurl.",
                 "• Look for HTTPS encryption – A secure website starts with \"https://\"—avoid sites that only have \"http://\".",
                 "• Hover over links before clicking – On computers, hovering over a link shows the real destination.",
                 "• Be cautious with unexpected emails or messages – If someone sends you a link out of nowhere, think twice before clicking.",
                 "• Always check the sender's email – Scammers use fake sender addresses that appear legitimate.",
                 "• Avoid clicking on links in urgent messages – If an email says \"Your account will be deactivated unless you click here!\", it's likely a scam."
                };

                var selectedRedFlags = suspiciousLinkRedFlags.OrderBy(x => random.Next()).Take(3).ToList();
                foreach (var redFlag in selectedRedFlags)
                {
                    suspiciousLinksResponse.AppendLine(redFlag);
                }

                suspiciousLinksResponse.AppendLine($"\nHere's how to protect yourself, {userName}:\n");

                // Add random prevention tips
                List<string> suspiciousLinkPreventionTips = new List<string>
                {
                 "• Enable Two-Factor Authentication (2FA) for extra protection.",
                 "• Never open links from unknown sources—especially in unsolicited messages.",
                 "• Use a link checker tool to verify URLs before clicking.",
                 "• If in doubt, go directly to the official website instead of clicking an email link.",
                 "• Enable safe browsing settings in your browser to detect malicious sites.",
                 "• Be cautious with QR codes—scammers embed phishing links inside them."
                };

                var selectedPreventionTips = suspiciousLinkPreventionTips.OrderBy(x => random.Next()).Take(3).ToList();
                foreach (var tip in selectedPreventionTips)
                {
                    suspiciousLinksResponse.AppendLine(tip);
                }

                suspiciousLinksResponse.AppendLine($"\nAnything else you need help with, {userName}?");
                AddMessage("Oracle", suspiciousLinksResponse.ToString(), Brushes.Cyan);
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
                    Dispatcher.Invoke(() => NavigationService.Navigate(new CyberQuiz(), this)); //navigate to cyberquiz page
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

            // 1. First handle general follow-up patterns (these should come FIRST)
            else if (userInput.Contains("example") || userInput.Contains("show me"))
            {
                if (currentTopic == "phishing")
                {
                    AddMessage("Oracle", "Here's an example: A phishing email might ask you to click a link to 'update your account'. Always be cautious before clicking!", Brushes.Cyan);
                }
                else if (currentTopic == "password")
                {
                    AddMessage("Oracle", "Here's an example of a strong password: 'BlueSky$123!' It includes uppercase, lowercase, numbers, and special characters!", Brushes.Cyan);
                }
            }
            // 2. Context-aware explanations
            else if (userInput.Contains("what is") || userInput.Contains("explain"))
            {
                if (currentTopic == "password")
                {
                    AddMessage("Oracle", "A password is a secret code that protects your personal information. It's the first line of defense against unauthorized access to your accounts.", Brushes.Cyan);
                }
                else if (currentTopic == "phishing")
                {
                    AddMessage("Oracle", "Phishing is a scam where attackers try to trick you into giving them your personal details, often via fake emails or websites.", Brushes.Cyan);
                }
                else
                {
                    AddMessage("Oracle", "Let me know what you'd like me to explain, and I'll help!", Brushes.Cyan);
                }
            }
            // 3. Specific topic tip requests
            else if (userInput == "password tip")
            {
                List<string> passwordTips = new List<string>
    {
        "✅ Use a **passphrase** instead of a short password—it's easier to remember and harder to crack.",
        "✅ **Never reuse passwords** across different accounts.",
        "✅ **Enable multi-factor authentication (MFA)** whenever possible."
    };
                AddMessage("Oracle", passwordTips[random.Next(passwordTips.Count)], Brushes.Cyan);
            }
            else if (userInput.Contains("phishing tip"))
            {
                List<string> phishingTips = new List<string>
    {
        "🔎 Always verify sender email addresses—scammers often use subtle misspellings.",
        "🚫 Never click links in unsolicited emails—instead, go directly to the official website.",
        "⚠️ Beware of urgent messages—scammers create fake emergencies to trick you into reacting."
    };
                AddMessage("Oracle", phishingTips[random.Next(phishingTips.Count)], Brushes.Cyan);
            }
            else if (userInput.Contains("safe browsing tip"))
            {
                List<string> safeBrowsingTips = new List<string>
    {
        "🔒 Always check for HTTPS and the lock icon before entering sensitive data.",
        "🚫 Never enter credentials on sites from unknown emails—they could be phishing scams.",
        "👀 Hover over links before clicking to ensure they lead to legitimate destinations."
    };
                AddMessage("Oracle", safeBrowsingTips[random.Next(safeBrowsingTips.Count)], Brushes.Cyan);
            }
            // 4. Specific topic follow-up questions
            else if (userInput.Contains("trustworthy") && userInput.Contains("website"))
            {
                AddMessage("Oracle", "Great question! Look for HTTPS in the address bar, check for spelling errors in the URL, and avoid sites that ask for too much personal info. Do you want tips on checking for fake sites?", Brushes.Cyan);
            }
            else if (userInput.Contains("report") && userInput.Contains("phishing"))
            {
                AddMessage("Oracle", "You can report phishing emails to your email provider, your IT department (if at school or work), or directly at reportphishing@apwg.org. Would you like help drafting a report?", Brushes.Cyan);
            }
            else if (userInput.Contains("trick") && userInput.Contains("link"))
            {
                AddMessage("Oracle", "Cybercriminals often use urgent language like 'your account will be locked!' or fake rewards to get clicks. Want to see common phrases they use?", Brushes.Cyan);
            }
            // 5. Topic initiation and specific handlers (these come LAST)
            else if (userInput.Contains("password") && currentTopic != "password")
            {
                currentTopic = "password";
                AddMessage("Oracle", "Strong passwords are essential, but there's more to it! Do you want to learn about password managers?", Brushes.Cyan);
            }
            else if (currentTopic == "password")
            {
                if (userInput.Contains("manager") || userInput.Contains("store pass"))
                {
                    AddMessage("Oracle", "A password manager helps you securely store all your passwords in one place. Would you like to know how to choose the best one?", Brushes.Cyan);
                }
                else if (userInput.Contains("mfa") || userInput.Contains("2fa"))
                {
                    AddMessage("Oracle", "Multi-factor authentication (MFA) adds an extra layer of protection. It typically involves something you know (like a password) and something you have (like your phone). Would you like a step-by-step guide to set it up?", Brushes.Cyan);
                }
                else if (userInput.Contains("how often change"))
                {
                    AddMessage("Oracle", "It's recommended to change your passwords every 3 to 6 months, or sooner if you suspect a breach. Do you want to know how to make password updates easy?", Brushes.Cyan);
                }
                else
                {
                    AddMessage("Oracle", "Is there a specific aspect of password security you'd like to explore further?", Brushes.Cyan);
                }
            }
            // Default response for unrecognized input
            else
            {
                List<string> unknownResponses = new List<string>
        {
            $"Hmm, {userName}, I'm not sure I understand. Could you rephrase?",
            $"Umm, I'm a bit lost here, {userName}. Mind explaining in a different way?",
            $"I don't quite get what you mean, {userName}. Try asking another way!",
            $"Oops! That one puzzled me, {userName}. Could you clarify?",
            $"I'm not sure what you meant by '{userInput}', {userName}. Can you reword it?"
        };
                AddMessage("Oracle", unknownResponses[random.Next(unknownResponses.Count)], Brushes.Cyan);
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