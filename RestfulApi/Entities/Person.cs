using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Entities
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Picture { get; set; }
    }
}
