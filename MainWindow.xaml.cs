using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PlanLekcjiWPF
{
    public partial class MainWindow : Window
    {
        const int Days = 5;
        const int LessonsCount = 11;

        private (string LessonName, string RoomNumber)[,] schedule = new (string, string)[Days, LessonsCount];
        private Button[,] lessonButtons = new Button[Days, LessonsCount];

        private readonly string saveFile = "plan_lekcji.txt";

        public MainWindow()
        {
            InitializeComponent();
            CreateLessonButtons();
            LoadSchedule();
            UpdateAllButtons();
        }

        private void CreateLessonButtons()
        {
            for (int day = 0; day < Days; day++)
            {
                for (int lesson = 0; lesson < LessonsCount; lesson++)
                {
                    var btn = new Button
                    {
                        Tag = (day, lesson),
                        Margin = new Thickness(2),
                        FontSize = 14,
                        Padding = new Thickness(5),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Background = System.Windows.Media.Brushes.Transparent,
                        BorderThickness = new Thickness(0),
                    };
                    btn.Click += LessonButton_Click;

                    Grid.SetRow(btn, lesson + 1);
                    Grid.SetColumn(btn, day + 1);

                    PlanGrid.Children.Add(btn);
                    lessonButtons[day, lesson] = btn;
                }
            }
        }

        private void LessonButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            (int day, int lesson) = ((int, int))btn.Tag;
            var data = schedule[day, lesson];

            var dialog = new LessonDialog(data.LessonName, data.RoomNumber);
            dialog.Owner = this;
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                schedule[day, lesson] = (dialog.LessonName.Trim(), dialog.RoomNumber.Trim());
                UpdateButtonContent(day, lesson);
                SaveSchedule();
            }
            else if (result == false && dialog.IsDeleteRequested)
            {
                schedule[day, lesson] = (null, null);
                UpdateButtonContent(day, lesson);
                SaveSchedule();
            }
        }

        private void UpdateButtonContent(int day, int lesson)
        {
            var btn = lessonButtons[day, lesson];
            var data = schedule[day, lesson];

            if (!string.IsNullOrEmpty(data.LessonName))
            {
                btn.Content = $"{data.LessonName}, {data.RoomNumber}";
                btn.Background = System.Windows.Media.Brushes.LightYellow;
            }
            else
            {
                btn.Content = "";
                btn.Background = System.Windows.Media.Brushes.Transparent;
            }
        }

        private void UpdateAllButtons()
        {
            for (int day = 0; day < Days; day++)
                for (int lesson = 0; lesson < LessonsCount; lesson++)
                    UpdateButtonContent(day, lesson);
        }

        private void SaveSchedule()
        {
            try
            {
                using StreamWriter sw = new(saveFile, false, Encoding.UTF8);
                for (int day = 0; day < Days; day++)
                {
                    for (int lesson = 0; lesson < LessonsCount; lesson++)
                    {
                        var data = schedule[day, lesson];
                        if (!string.IsNullOrEmpty(data.LessonName))
                        {
                            string ln = data.LessonName.Replace("|", " ");
                            string rm = data.RoomNumber?.Replace("|", " ") ?? "";
                            sw.WriteLine($"{day}|{lesson}|{ln}|{rm}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu planu lekcji:\n" + ex.Message);
            }
        }

        private void LoadSchedule()
        {
            if (!File.Exists(saveFile))
                return;

            try
            {
                foreach (var line in File.ReadAllLines(saveFile, Encoding.UTF8))
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 4 &&
                        int.TryParse(parts[0], out int day) &&
                        int.TryParse(parts[1], out int lesson))
                    {
                        string lessonName = parts[2];
                        string room = parts[3];
                        if (day >= 0 && day < Days && lesson >= 0 && lesson < LessonsCount)
                        {
                            schedule[day, lesson] = (lessonName, room);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu planu lekcji:\n" + ex.Message);
            }
        }

        private class LessonDialog : Window
        {
            private TextBox lessonNameTextBox;
            private TextBox roomNumberTextBox;
            private Button saveButton;
            private Button cancelButton;
            private Button deleteButton;

            public string LessonName => lessonNameTextBox.Text;
            public string RoomNumber => roomNumberTextBox.Text;
            public bool IsDeleteRequested { get; private set; } = false;

            public LessonDialog(string lessonName, string roomNumber)
            {
                Title = string.IsNullOrEmpty(lessonName) ? "Dodaj lekcję" : "Edytuj lekcję";
                Width = 350;
                Height = 220;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
                ResizeMode = ResizeMode.NoResize;

                var panel = new StackPanel() { Margin = new Thickness(10) };

                panel.Children.Add(new TextBlock { Text = "Nazwa lekcji:", Margin = new Thickness(0, 0, 0, 5) });
                lessonNameTextBox = new TextBox { Text = lessonName ?? "", Margin = new Thickness(0, 0, 0, 10) };
                panel.Children.Add(lessonNameTextBox);

                panel.Children.Add(new TextBlock { Text = "Numer sali:", Margin = new Thickness(0, 0, 0, 5) });
                roomNumberTextBox = new TextBox { Text = roomNumber ?? "", Margin = new Thickness(0, 0, 0, 15) };
                panel.Children.Add(roomNumberTextBox);

                var buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };

                saveButton = new Button { Content = "Zapisz", Width = 80, Margin = new Thickness(5, 0, 5, 0) };
                saveButton.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(LessonName))
                    {
                        MessageBox.Show("Nazwa lekcji nie może być pusta.");
                        return;
                    }
                    DialogResult = true;
                };

                cancelButton = new Button { Content = "Anuluj", Width = 80, Margin = new Thickness(5, 0, 5, 0) };
                cancelButton.Click += (s, e) => DialogResult = null;

                deleteButton = new Button { Content = "Usuń", Width = 80, Margin = new Thickness(5, 0, 5, 0) };
                deleteButton.Click += (s, e) =>
                {
                    if (MessageBox.Show("Czy na pewno usunąć tę lekcję?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        IsDeleteRequested = true;
                        DialogResult = false;
                    }
                };

                buttonsPanel.Children.Add(saveButton);
                buttonsPanel.Children.Add(cancelButton);
                buttonsPanel.Children.Add(deleteButton);

                panel.Children.Add(buttonsPanel);

                Content = panel;
            }
        }
    }
}
