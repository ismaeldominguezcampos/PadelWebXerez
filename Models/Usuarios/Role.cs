using System;
using Microsoft.AspNetCore.Identity;

namespace PadelWebXerez
{
    public class Role : IdentityRole<string>
    {
        public string Descripcion { get; set; }
    }
}
