using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace GenealogyTree
{
    public static class PersonRepository
    {
        public static ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

        public static void AddPerson(Person person)
        {
            People.Add(person);
            Debug.WriteLine($"Person added: {person}");
        }

        public static void EditPerson(Person oldPerson, Person newPerson)
        {
            Debug.WriteLine($"Person edited: {newPerson.Parent}");
            Debug.WriteLine($"oldPerson: {oldPerson.Parent}");
            if (oldPerson.Parent != newPerson.Parent)
            {
                if (oldPerson.Parent != null && oldPerson.Parent.Children.Contains(oldPerson))
                {
                    oldPerson.Parent.Children.Remove(oldPerson);
                }
            }
            int index = People.IndexOf(oldPerson);
            People[index] = newPerson;
        }

        public static bool PersonExists(Guid personId)
        {
            return People.Any(p => p.Id == personId);
        }
    }
}
