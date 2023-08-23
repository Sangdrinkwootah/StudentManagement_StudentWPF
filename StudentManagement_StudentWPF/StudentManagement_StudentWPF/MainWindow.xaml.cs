using StudentManagement_StudentWPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement_StudentWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StudentDatabase database = new StudentDatabase();
        private List<Student> students = new List<Student>();
        private bool isClassViewMode = true;

        public MainWindow()
        {
            InitializeComponent();
            // Initialize student data (replace this with your data source)
            students = database.GetAllStudents();
            ClassView.ItemsSource = students;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ClassView_Click(object sender, RoutedEventArgs e)
        {
            isClassViewMode = true;
            ClassView.Visibility = Visibility.Visible;
            studentDetailText.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Visible;
            studentDetailsPanel.Visibility = Visibility.Collapsed;
            ClearButton.Visibility = Visibility.Visible;
            searchTextBox.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void StudentView_Click(object sender, RoutedEventArgs e)
        {
            isClassViewMode = false;
            ClassView.Visibility = Visibility.Collapsed;
            studentDetailText.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;
            ClearButton.Visibility = Visibility.Collapsed;
            searchTextBox.Visibility = Visibility.Collapsed;
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var studentsToDelete = students.Where(s => s.IsSelected).ToList();
            database.RemoveStudents(studentsToDelete);

            students = database.GetAllStudents();

            // Update UI
            ClassView.ItemsSource = null;
            ClassView.ItemsSource = students;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Tag is Student student)
            {
                student.IsSelected = checkBox.IsChecked == true;
                deleteButton.Visibility = students.Any(s => s.IsSelected) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is Student selectedStudent)
            {
                studentDetailText.Text = $"Student Details - {selectedStudent.Name}";
                nameTextBox.Text = selectedStudent.Name;
                addressTextBox.Text = selectedStudent.Address;
                phonenumberTextBox.Text = selectedStudent.PhoneNumber;
                countryTextBox.Text = selectedStudent.Country;

                studentDetailsPanel.Visibility = Visibility.Visible;
                ClassView.Visibility = Visibility.Collapsed;
                deleteButton.Visibility = Visibility.Collapsed;
                ClearButton.Visibility = Visibility.Collapsed;
                searchTextBox.Visibility= Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClassView.SelectedItem is Student selectedStudent)
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(addressTextBox.Text) ||
                    string.IsNullOrWhiteSpace(phonenumberTextBox.Text) ||
                    string.IsNullOrWhiteSpace(countryTextBox.Text))
                {
                    MessageBox.Show("Please enter valid student information.");
                    return;
                }

                selectedStudent.Name = nameTextBox.Text;
                selectedStudent.Address = addressTextBox.Text;
                selectedStudent.PhoneNumber = phonenumberTextBox.Text;
                selectedStudent.Country = countryTextBox.Text;

                //database.UpdateStudent(selectedStudent);
                studentDetailsPanel.Visibility = Visibility.Collapsed;

                LoadStudents(); // Refresh the list
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            studentDetailsPanel.Visibility= Visibility.Collapsed;
            ClassView.Visibility = Visibility.Visible;
        }

        private void LoadStudents()
        {
            ClassView.ItemsSource = null;
            students = database.GetAllStudents();
            ClassView.ItemsSource = students;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.ToLower();
            var filteredStudents = students.Where(student => student.Name.ToLower().Contains(searchTerm)).ToList();
            ClassView.ItemsSource = filteredStudents;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = ""; // Clear the search text
            LoadStudents(); // Reset to show all students
        }
    }
}

