using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.DTOs
{
    public class GenreCreationDTO
    {
        [Required]
        [StringLength(30, ErrorMessage = "姓名不能大於30個字")]
        public string Name { get; set; }
    }
}
