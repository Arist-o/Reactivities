using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Application.Activities.DTOs
{
    public class RpcRequestDto
    {
        public required string Method { get; set; }

        public required JsonElement Params{ get; set; }
    }
}
