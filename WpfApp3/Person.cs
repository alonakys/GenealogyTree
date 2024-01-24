using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace GenealogyTree
{
    public class Person
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate {get;set;}
        public string Gender { get; set; }
        public Person Parent { get; set; }
        public ObservableCollection<Person> Children { get; set; } = new ObservableCollection<Person>();
        
        public override string ToString()
        {
            return $"{Name} {Surname}, {BirthDate.ToShortDateString()}, {Gender}";
        }
    }
}