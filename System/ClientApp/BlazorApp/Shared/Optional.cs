namespace BlazorApp.Shared;

public class Optional<T>
{
    public bool HasValue { get; }
    public T? Value { get; }

    public bool HasError { get; }
    public string? ErrorMessage { get; }

    public bool IsSuccess => HasValue && !HasError;
    public bool IsFailure => HasError;

    private Optional(bool hasValue = false, bool hasError = false, T? value = default, string? error = null)
    {
        HasValue = hasValue;
        HasError = hasError;
        Value = value;
        ErrorMessage = error;
    }
    private Optional(T value) : this(true, false, value, null) { }
    private Optional() : this(false, false, default, null) { }

    public static Optional<T> Success(T value) => new(value);
    public static Optional<T> Error(string message) => new(false, true, default, message);
    public static Optional<T> Empty() => new();

    public static implicit operator T?(Optional<T> optional) => optional.Value;
}
