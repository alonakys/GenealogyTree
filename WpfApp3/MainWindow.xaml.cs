using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace GenealogyTree
{
    public partial class MainWindow : Window
    {
        public List<SampleDataModel> sampleTree;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreatePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonWindow personWindow = new PersonWindow();
            personWindow.Owner = this;
            personWindow.ShowDialog();

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

                RefreshTree();
            }
        }

        public void RefreshTree()
        {
            genealogyTreeView.Items.Clear();
            sampleTree = new List<SampleDataModel>();
            if (PersonRepository.People.Any())
            {
                var person = PersonRepository.People[0];
                if (person.Parent == null)
                {
                    //створюємо перший node
                    sampleTree.Add(new SampleDataModel { Id = $"{person.Id}", ParentId = string.Empty, Name = $"{person.Name}", 
                        Surname = $"{person.Surname}", BirthDate = $"{person.BirthDate.ToShortDateString()}", Gender = $"{person.Gender}" });
                    TreeViewItem personItem = CreateTreeViewItem(person, ref sampleTree);
                    genealogyTreeView.Items.Add(personItem);
                    personItem.IsExpanded = true;
                }
            }
        }

        private TreeViewItem CreateTreeViewItem(Person person, ref List<SampleDataModel> sampleTree)
        {
            TreeViewItem item = new TreeViewItem { Header = $"{person.Name} {person.Surname}, {person.BirthDate.ToShortDateString()}, {person.Gender}", Tag = person };

            if (person.Children != null && person.Children.Any())
            {
                foreach (var child in person.Children)
                {
                    //створюємо node з даними людини
                    sampleTree.Add(new SampleDataModel { Id = $"{child.Id}", ParentId = $"{person.Id}", Name = $"{child.Name}", 
                        Surname = $"{child.Surname}", BirthDate = $"{child.BirthDate.ToShortDateString()}", Gender = $"{child.Gender}" });

                    item.Items.Add(CreateTreeViewItem(child, ref sampleTree));
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
                MessageBoxResult result = System.Windows.MessageBox.Show($"Are you sure you want to delete {selectedPerson.Name} {selectedPerson.Surname} and their descendants?", 
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    RemovePersonAndDescendants(selectedPerson);
                    RefreshTree();
                }
            }
        }
        private void RemovePersonAndDescendants(Person person)
        {
            if (person.Parent != null)
            {
                person.Parent.Children.Remove(person);
            }
            PersonRepository.People.Remove(person);
            foreach (var child in person.Children.ToList())
            {
                RemovePersonAndDescendants(child);
            }
        }
        private void BuildTree_Click(object sender, RoutedEventArgs e)
        {
            Form1 winFormsWindow = new Form1(sampleTree);
            winFormsWindow.Show();
        }
    }
}
