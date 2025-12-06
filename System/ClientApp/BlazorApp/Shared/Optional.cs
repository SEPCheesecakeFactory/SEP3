namespace BlazorApp.Shared;

public class Optional<T>
{
    public bool HasValue { get; }
    public T? Value { get; }

    public bool HasError { get; }
    public string? ErrorMessage { get; }

    public bool IsSuccess => HasValue && !HasError;
    public bool IsFailure => HasError;

    private Optional(T value)
    {
        Value = value;
        HasValue = true;
        HasError = false;
    }

    private Optional(string error)
    {
        ErrorMessage = error;
        HasValue = false;
        HasError = true;
    }

    private Optional()
    {
        HasValue = false;
        HasError = false;
    }

    public static Optional<T> Success(T value) => new Optional<T>(value);

    public static Optional<T> Error(string message) => new Optional<T>(message);

    public static Optional<T> Empty() => new Optional<T>();
}
