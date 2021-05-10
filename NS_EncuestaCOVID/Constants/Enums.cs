using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Constants
{
    public class Enums
    {

        public enum SignInStatus
        {
            Success = 0,
            LockedOut = 1,
            RequiresVerification = 2,
            Failure = 3
        }

        public  enum ContextType
        {
            Machine = 0,
            Domain = 1,
            ApplicationDirectory = 2
        }
    }
}