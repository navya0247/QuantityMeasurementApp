namespace QuantityMeasurementApp.Exception
{
    public class DatabaseException : System.Exception
    {
        public DatabaseException(string message)
            : base(message) { }

        public DatabaseException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}