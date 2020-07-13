using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnlineGallery.BLL.Exceptions
{
    public class IdentityException : InvalidOperationException
    {
        public IdentityException(IEnumerable<IdentityError> errors)
        {
            Errors = errors;
        }

        public IEnumerable<IdentityError> Errors { get; }
    }
}