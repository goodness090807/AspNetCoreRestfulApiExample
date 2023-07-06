using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.DTOs
{
    public class MoviePatchDTO
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public string Summary { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
