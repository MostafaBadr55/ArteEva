namespace ArtEva.Services.Implementation
{
    public class NotValidException : Exception
    {
        public NotValidException(string message) : base(message) { }
    }
}
