using Microsoft.AspNetCore.Http;
using RestfulApi.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.DTOs
{
    public class PersonCreationDTO : PersonPatchDTO
    {
        [FileSizeValidator(10)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Picture { get; set; }
    }
}
