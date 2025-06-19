using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ORACLE_POE
{
    public partial class CyberQuiz : Page
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private bool answerSelected = false;

        public CyberQuiz()
        {
            InitializeComponent();
            InitializeQuestions();
            ShowQuestion();
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What is the minimum recommended length for a strong password?",
                    new List<string> { "6 characters", "8 characters", "12 characters", "16 characters" },
                    2,
                    "12 characters is the current minimum recommendation. Longer passwords are harder to crack."),

                new QuizQuestion(
                    "Which of these is a sign of a phishing email?",
                    new List<string> { "Personalized greeting with your name", "Urgent call to action", "Professional logo", "All of the above" },
                    1,
                    "Phishing emails often create a sense of urgency to trick you into acting quickly without thinking."),

                new QuizQuestion(
                    "True or False: You should use the same password for multiple accounts.",
                    new List<string> { "True", "False" },
                    1,
                    "False! If one account is compromised, all accounts with the same password are at risk."),

                // Add 17 more questions following the same pattern
                new QuizQuestion(
                    "What does HTTPS in a website URL indicate?",
                    new List<string> { "The site is popular", "The connection is encrypted", "The site is government-approved", "The site has videos" },
                    1,
                    "HTTPS indicates the connection is encrypted, making it more secure than HTTP."),

                new QuizQuestion(
                    "What should you do if you receive a suspicious link in an email?",
                    new List<string> { "Click it to see where it goes", "Forward it to friends to warn them", "Hover over it to check the URL", "Report it and don't click" },
                    3,
                    "Never click suspicious links. Report them to your IT department or email provider."),

                new QuizQuestion(
                    "True or False: Public Wi-Fi networks are generally safe for online banking.",
                    new List<string> { "True", "False" },
                    1,
                    "False! Public Wi-Fi is often unsecured. Use a VPN or wait until you're on a secure network."),

                new QuizQuestion(
                    "What is two-factor authentication (2FA)?",
                    new List<string> { "Using two passwords", "A second verification step after entering password", "A type of firewall", "A backup email address" },
                    1,
                    "2FA adds an extra security step, like a code sent to your phone, making accounts more secure."),

                new QuizQuestion(
                    "Which of these is NOT a good password practice?",
                    new List<string> { "Using a password manager", "Changing passwords every 90 days", "Writing passwords on sticky notes", "Using passphrases" },
                    2,
                    "Writing passwords on physical notes is risky if someone finds them. Use a password manager instead."),

                new QuizQuestion(
                    "True or False: Software updates often include important security patches.",
                    new List<string> { "True", "False" },
                    0,
                    "True! Keeping software updated is crucial for security as updates often fix vulnerabilities."),

                new QuizQuestion(
                    "What should you do before entering credentials on a website?",
                    new List<string> { "Check for HTTPS", "Verify the URL is correct", "Both of the above", "Neither, just log in quickly" },
                    2,
                    "Always verify both HTTPS and the correct URL to avoid phishing sites."),

                new QuizQuestion(
                    "What is a common tactic used in phishing scams?",
                    new List<string> { "Offering free gifts", "Pretending to be from a trusted company", "Creating a sense of urgency", "All of the above" },
                    3,
                    "Phishing scams often combine these tactics to trick users into revealing information."),

                new QuizQuestion(
                    "True or False: QR codes are always safe to scan.",
                    new List<string> { "True", "False" },
                    1,
                    "False! Malicious QR codes can direct you to phishing sites. Only scan codes from trusted sources."),

                new QuizQuestion(
                    "What is the best way to handle suspicious attachments?",
                    new List<string> { "Open them to check", "Forward them to IT security", "Delete them immediately", "Save them for later" },
                    2,
                    "When in doubt, delete suspicious attachments. They may contain malware."),

                new QuizQuestion(
                    "Which of these makes a password stronger?",
                    new List<string> { "Using common words", "Adding special characters", "Using personal information", "Making it short" },
                    1,
                    "Special characters increase password complexity, making it harder to crack."),

                new QuizQuestion(
                    "True or False: You should share your passwords with family members.",
                    new List<string> { "True", "False" },
                    1,
                    "False! Each person should have their own account whenever possible."),

                new QuizQuestion(
                    "What should you do if you suspect your account was hacked?",
                    new List<string> { "Do nothing", "Change your password immediately", "Wait to see if anything happens", "Tell all your friends" },
                    1,
                    "Immediately change your password and enable 2FA if available. Check for unauthorized activity."),

                new QuizQuestion(
                    "What is a VPN used for?",
                    new List<string> { "Making internet faster", "Securing internet connections", "Blocking all ads", "Increasing storage space" },
                    1,
                    "VPNs encrypt your internet connection, especially important on public Wi-Fi."),

                new QuizQuestion(
                    "True or False: Antivirus software makes you completely safe from all threats.",
                    new List<string> { "True", "False" },
                    1,
                    "False! Antivirus is important but doesn't replace good security practices like strong passwords."),

                new QuizQuestion(
                    "What is 'smishing'?",
                    new List<string> { "A type of fish", "Phishing via SMS/text messages", "A social media trend", "A password manager" },
                    1,
                    "Smishing is phishing attempts sent via text messages, often with malicious links."),

                new QuizQuestion(
                    "When creating security questions, you should:",
                    new List<string> { "Use easily guessable answers", "Use answers that can be found on social media", "Make up answers only you would know", "Use the same answers for all sites" },
                    2,
                    "Security question answers should be treated like passwords - unique and not guessable.")
            };
        }

        private void ShowQuestion()
        {
            if (currentQuestionIndex >= questions.Count)
            {
                ShowResults();
                return;
            }

            var question = questions[currentQuestionIndex];
            QuestionText.Text = question.QuestionText;
            ProgressText.Text = $"Question {currentQuestionIndex + 1} of {questions.Count}";

            OptionsPanel.Children.Clear();
            for (int i = 0; i < question.Options.Count; i++)
            {
                var radioButton = new RadioButton
                {
                    Content = question.Options[i],
                    Foreground = Brushes.White,
                    FontSize = 14,
                    Margin = new Thickness(0, 0, 0, 5),
                    Tag = i
                };
                radioButton.Checked += Option_Checked;
                OptionsPanel.Children.Add(radioButton);
            }

            FeedbackBorder.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;
            answerSelected = false;
        }

        private void Option_Checked(object sender, RoutedEventArgs e)
        {
            if (answerSelected) return;

            var selectedOption = (RadioButton)sender;
            int selectedIndex = (int)selectedOption.Tag;
            var question = questions[currentQuestionIndex];

            FeedbackBorder.Visibility = Visibility.Visible;
            NextButton.IsEnabled = true;

            if (selectedIndex == question.CorrectAnswerIndex)
            {
                score++;
                FeedbackText.Text = "Correct!";
                FeedbackText.Foreground = Brushes.LightGreen;
            }
            else
            {
                FeedbackText.Text = "Incorrect!";
                FeedbackText.Foreground = Brushes.LightPink;
            }

            ExplanationText.Text = question.Explanation;
            answerSelected = true;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Count)
            {
                ShowQuestion();
            }
            else
            {
                ShowResults();
            }
        }

        private void ShowResults()
        {
            OptionsPanel.Children.Clear();
            QuestionText.Text = "Quiz Complete!";
            ProgressText.Text = $"Score: {score}/{questions.Count}";
            NextButton.Content = "Restart Quiz";
            NextButton.Click -= NextButton_Click;
            NextButton.Click += RestartQuiz_Click;

            FeedbackBorder.Visibility = Visibility.Visible;
            FeedbackText.Text = GetResultFeedback(score, questions.Count);
            FeedbackText.Foreground = Brushes.White;
            ExplanationText.Text = "Thank you for testing your cybersecurity knowledge!";
        }

        private string GetResultFeedback(int score, int totalQuestions)
        {
            double percentage = (double)score / totalQuestions * 100;

            if (percentage >= 90) return "Excellent! You're a cybersecurity expert!";
            if (percentage >= 75) return "Great job! You have strong cybersecurity knowledge.";
            if (percentage >= 50) return "Good effort! Consider reviewing cybersecurity basics.";
            return "Keep learning! Cybersecurity is important for everyone.";
        }

        private void RestartQuiz_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex = 0;
            score = 0;
            NextButton.Click -= RestartQuiz_Click;
            NextButton.Click += NextButton_Click;
            NextButton.Content = "Next Question";
            ShowQuestion();
        }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; }
        public List<string> Options { get; }
        public int CorrectAnswerIndex { get; }
        public string Explanation { get; }

        public QuizQuestion(string questionText, List<string> options, int correctAnswerIndex, string explanation)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            Explanation = explanation;
        }
    }
}