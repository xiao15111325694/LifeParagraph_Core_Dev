using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Models
{
    public class SignInResult
    {
 
        public static SignInResult Success { get; }

        public static SignInResult Failed { get; }

        public static SignInResult LockedOut { get; }

        public static SignInResult NotAllowed { get; }

        public static SignInResult TwoFactorRequired { get; }
    }
}
