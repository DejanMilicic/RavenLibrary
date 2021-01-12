using System.ComponentModel.DataAnnotations;

namespace CouchLibrary.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
