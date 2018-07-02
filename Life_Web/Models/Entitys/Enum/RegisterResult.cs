using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Models
{
    public class RegisterResult
    {
        public static RegisterResult Success { get; }
        public bool Succeeded { get;  set; }
        public static RegisterResult Failed { get; }
    }
}
