namespace ElderlyCareSupport.Admin.Contracts.Response;

public class Error
{
    public Error(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
    public string ErrorMessage { get; init; } 
}