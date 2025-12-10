namespace ArtEva.Services.Implementation
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
