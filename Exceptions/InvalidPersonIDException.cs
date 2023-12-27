namespace Exceptions
{

    // Read the Best Pratices Exception - Read the artical from Microsoft 
    public class InvalidPersonIDException : ArgumentException
    {
        // Empty constructor
        public InvalidPersonIDException() : base()
        {        }

        public InvalidPersonIDException(string? message) : base(message)
        {

        }

        public InvalidPersonIDException(string? message, Exception? innerException) 
        {

        }



    }
}