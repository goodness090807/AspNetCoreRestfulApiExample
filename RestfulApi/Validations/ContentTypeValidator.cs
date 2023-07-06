using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Validations
{
    public class ContentTypeValidator : ValidationAttribute
    {
        private readonly string[] _validContentTypes;

        private readonly string[] imageContentTypes = new string[] { "image/jpeg", "image/png", "image/gif" };

        public ContentTypeValidator(string[] ValidContentTypes)
        {
            _validContentTypes = ValidContentTypes;
        }

        public ContentTypeValidator(ContentTypeGroup contentTypeGroup)
        {
            switch(contentTypeGroup)
            {
                case ContentTypeGroup.Image:
                    _validContentTypes = imageContentTypes;
                    break;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return ValidationResult.Success;

            if(!_validContentTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"上傳檔案只能使用{string.Join(", ", _validContentTypes)}的格式");
            }

            return ValidationResult.Success;
        }

    }

    public enum ContentTypeGroup
    {
        Image
    }
}
