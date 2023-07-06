using Microsoft.AspNetCore.Http;
using RestfulApi.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.DTOs
{
    public class MovieCreationDTO : MoviePatchDTO
    {
        [FileSizeValidator(10)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }
    }
}
