using System;

namespace Obsequy.Utility
{
    public class ValidationException : Exception
    {
        public string Errors { get; set; }

        public ValidationException()
            : base()
        {
        }
    }
}
