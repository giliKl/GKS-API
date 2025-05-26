using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Service.Shared
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
            : base($"Email '{email}' already exists.") { }
    }

    // Exceptions/UserNotFoundException.cs
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email)
            : base($"User with email '{email}' not found.") { }
    }

    // Exceptions/InvalidPasswordException.cs
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
            : base("The password provided is incorrect.") { }
    }
}
