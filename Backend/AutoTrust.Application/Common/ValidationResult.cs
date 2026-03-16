namespace AutoTrust.Application.Common
{
    public record ValidationResult(bool IsValid, string? ErrorMessage = null);
}
