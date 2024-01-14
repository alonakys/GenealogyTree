using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GenealogyTree
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreatePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonWindow personWindow = new PersonWindow();
            personWindow.Owner = this;
            personWindow.ShowDialog();

            // Update tree and fatherComboBox after creating a person
            RefreshTree();
        }

        private void EditPerson_Click(object sender, RoutedEventArgs e)
        {
            if (genealogyTreeView.SelectedItem != null && genealogyTreeView.SelectedItem is TreeViewItem selectedItem)
            {
                Person selectedPerson = (Person)selectedItem.Tag;

                PersonWindow personWindow = new PersonWindow(selectedPerson);
                personWindow.Owner = this;
                personWindow.ShowDialog();

                // Update tree after editing a person
                RefreshTree();
            }
        }

        public void RefreshTree()
        {
            genealogyTreeView.Items.Clear();

            foreach (var person in PersonRepository.People)
            {
                // Check if the person has no parent, meaning it's a root person
                if (person.Parent == null)
                {
                    TreeViewItem personItem = CreateTreeViewItem(person);
                    genealogyTreeView.Items.Add(personItem);
                    personItem.IsExpanded = true;
                }
            }
        }

        private TreeViewItem CreateTreeViewItem(Person person)
        {
            TreeViewItem item = new TreeViewItem { Header = $"{person.Name} {person.Surname}, {person.BirthDate.ToShortDateString()}, {person.Gender}", Tag = person };

            if (person.Children != null && person.Children.Any())
            {
                foreach (var child in person.Children)
                {
                    item.Items.Add(CreateTreeViewItem(child));
                }
            }

            item.IsExpanded = true;

            return item;
        }

        private void DeletePerson_Click(object sender, RoutedEventArgs e)
        {
            if (genealogyTreeView.SelectedItem != null && genealogyTreeView.SelectedItem is TreeViewItem selectedItem)
            {
                Person selectedPerson = (Person)selectedItem.Tag;

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedPerson.Name} {selectedPerson.Surname} and their descendants?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Remove the selected person and their descendants from the repository
                    RemovePersonAndDescendants(selectedPerson);

                    // Update the tree after deletion
                    RefreshTree();
                }
            }
        }
        private void BuildTree_Click(object sender, RoutedEventArgs e)
        {
            // Clear existing visual elements on the canvas
            genealogyCanvas.Children.Clear();

            foreach (var person in PersonRepository.People)
            {
                // Check if the person has no parent, meaning it's a root person
                if (person.Parent == null)
                {
                    DrawTree(person, null, 500, 50, 0); // Adjust the starting position as needed
                }
            }
        }

        private void DrawTree(Person person, Person parent, double x, double y, int generation)
        {
            // Draw the group box for the current person
            var groupBox = new GroupBox
            {
                Header = $"{person.Name} {person.Surname}",
                Width = 150,
                Height = 100,
                Margin = new Thickness(x, y, 0, 0),
            };

            // Draw a line connecting the current person to their parent
            if (parent != null)
            {
                var line = new Line
                {
                    X1 = x + groupBox.Width / 2,
                    Y1 = y,
                    X2 = x + groupBox.Width / 2,
                    Y2 = y - 20,
                    Stroke = Brushes.Black,
                };
                genealogyCanvas.Children.Add(line);
            }

            // Add the group box to the canvas
            genealogyCanvas.Children.Add(groupBox);

            // Draw children recursively
            if (person.Children != null && person.Children.Any())
            {
                double xOffset = 50; 

                foreach (var child in person.Children)
                {
                    if(person.Children!=null && child==person.Children[0])
                    {
                        xOffset *= -1;
                    }
                    DrawTree(child, person, x + xOffset * Math.Pow(-1,person.Children.Count), y + 120, generation + 1);
                    xOffset += groupBox.Width + 50;
                }
            }
        }

        private void RemovePersonAndDescendants(Person person)
        {
            if (person.Parent != null)
            {
                person.Parent.Children.Remove(person);
            }
            // Remove the person
            PersonRepository.People.Remove(person);

            // Remove descendants recursively
            foreach (var child in person.Children.ToList())
            {
                RemovePersonAndDescendants(child);
            }
        }
    }
}
