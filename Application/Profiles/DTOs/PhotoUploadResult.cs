using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Profiles.DTOs
{
    public class PhotoUploadResult
    {
        public required string PublicId { get; set; }

        public required string Url { get; set; }
    }
}
