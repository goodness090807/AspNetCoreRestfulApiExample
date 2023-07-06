﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Validations
{
    public class FileSizeValidator : ValidationAttribute
    {
        private readonly int _maxFileSizeInMbs;

        public FileSizeValidator(int MaxFileSizeInMbs)
        {
            _maxFileSizeInMbs = MaxFileSizeInMbs;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return ValidationResult.Success;

            if(formFile.Length > _maxFileSizeInMbs * 1024 * 1024)
            {
                return new ValidationResult($"檔案大小不能超過{_maxFileSizeInMbs}MB!!!");
            }

            return ValidationResult.Success;
        }
    }
}
