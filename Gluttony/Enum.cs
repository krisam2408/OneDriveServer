namespace Gluttony
{
    public enum HttpVerb { GET, POST, PUT, DELETE, PATCH }

    public enum ParameterTreatment 
    { 
        String, 
        Number, 
        Date, 
        FilePath,
        ArrayOfStrings, 
        ArrayOfNumbers, 
        ArrayOfDates, 
        ArrayOfFilePaths,
        Object
    }

    public enum ExceptionMessagesOption 
    { 
        ExceptionNameOnly = 1, 
        MessageOnly = 10, 
        StackTraceOnly = 100,
        ExceptionNameAndMessage = 11,
        ExceptionNameAndStackTrace = 101,
        MessageAndStackTrace = 110,
        All = 111
    }

    public enum AuthenticationType
    {
        None,
        Bearer
    }
}
