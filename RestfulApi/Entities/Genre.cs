using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "姓名不能大於30個字")]
        public string Name { get; set; }
    }
}
