using System;

namespace SFA.DAS.SharedOuterApi.Exceptions
{
    public class EntityNotFoundException<T> : Exception
    {
        public EntityNotFoundException() 
            : base($"Entity of type [{typeof(T).Name}] cannot be found")
        {
            
        }

        public EntityNotFoundException(string message) 
            : base(message)
        {
            
        }

        public EntityNotFoundException(Exception innerException) 
            : base($"Entity of type [{typeof(T).Name}] cannot be found", innerException)
        {
            
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}