namespace ElderlyCareSupport.Admin.Contracts.Response;

public record Error
{
    public Error(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
    public string ErrorMessage { get; init; } 
}