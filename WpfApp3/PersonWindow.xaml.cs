using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GenealogyTree
{
    public partial class PersonWindow : Window
    {
        private Person _editingPerson;
        private Person _originalPerson;

        public PersonWindow(Person person = null)
        {
            InitializeComponent();
            _originalPerson = person ?? new Person();
            _editingPerson = new Person
            {
                Name = _originalPerson.Name,
                Surname = _originalPerson.Surname,
                BirthDate = _originalPerson.BirthDate,
                Gender = _originalPerson.Gender,
                Parent = _originalPerson.Parent
            };

            DataContext = _editingPerson;

            Loaded += (sender, e) => PopulateParentComboBox();
        }

        private void PopulateParentComboBox()
        {
            if (PersonRepository.People.Any())
            {
                parentComboBox.ItemsSource = PersonRepository.People
                    .Where(p => !IsDescendantOrSelf(_originalPerson, p))
                    .ToList();
            }
        }

        private bool IsDescendantOrSelf(Person ancestor, Person person)
        {
            if (ancestor == null || person == null)
            {
                return false;
            }

            if (ancestor == person)
            {
                return true;
            }

            Person currentPerson = person;
            do
            {
                if (currentPerson == ancestor)
                {
                    return true;
                }

                currentPerson = currentPerson.Parent;
            } while (currentPerson != null);

            return false;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                _originalPerson.Name = _editingPerson.Name;
                _originalPerson.Surname = _editingPerson.Surname;
                _originalPerson.BirthDate = _editingPerson.BirthDate;
                _originalPerson.Gender = _editingPerson.Gender;

                if (_originalPerson.Parent != null && _originalPerson.Parent.Children.Contains(_originalPerson))
                {
                    _originalPerson.Parent.Children.Remove(_originalPerson);
                }

                if (parentComboBox.SelectedItem != null)
                {
                    _originalPerson.Parent = (Person)parentComboBox.SelectedItem;
                    _originalPerson.Parent.Children.Add(_originalPerson);
                }
                else
                {
                    _originalPerson.Parent = null;
                }

                if (PersonRepository.PersonExists(_originalPerson.Id))
                {
                    PersonRepository.EditPerson(_originalPerson, _originalPerson);
                }
                else
                {
                    PersonRepository.AddPerson(_originalPerson);
                }

                Close();
            }
        }
        private bool ValidateInput()
        {
            Regex nameRegex = new Regex("^[a-zA-Zа-яА-ЯґҐєЄіІїЇ'-]+$");
            if (string.IsNullOrWhiteSpace(_editingPerson.Name) || !nameRegex.IsMatch(_editingPerson.Name))
            {
                MessageBox.Show("Please enter a valid name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(_editingPerson.Surname) || !nameRegex.IsMatch(_editingPerson.Surname))
            {
                MessageBox.Show("Please enter a valid surname.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(_editingPerson.BirthDate.ToString("dd.MM.yyyy"), @"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.\d{4}$"))
            {
                MessageBox.Show("Please enter a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (_editingPerson.BirthDate.ToShortDateString() == "01.01.0001")
            {
                MessageBoxResult result = System.Windows.MessageBox.Show($"Maybe the date you entered is incorrect.\nDo you really want to save this proposed date: \"{_editingPerson.BirthDate.ToShortDateString()}\"?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    MessageBox.Show("Please enter date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
                if (string.IsNullOrWhiteSpace(_editingPerson.Gender))
            {
                MessageBox.Show("Please enter a gender.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (PersonRepository.People.Any() && _editingPerson.Parent == null)
            {
                MessageBox.Show("Please enter a child.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (_editingPerson.Parent != null && _editingPerson.Parent.Children.Count == 2 && (!_editingPerson.Parent.Children[0].Equals(_originalPerson) && !_editingPerson.Parent.Children[1].Equals(_originalPerson)))
            {
                MessageBox.Show("Please choose another child.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

    }
}
