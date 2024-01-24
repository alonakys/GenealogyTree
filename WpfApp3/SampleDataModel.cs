using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenealogyTree
{
    public class SampleDataModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }

        // just for testing
        public override string ToString()
        {
            return $"{Name}\n{Surname}\n{BirthDate}\n{Gender}";
        }
    }
}
