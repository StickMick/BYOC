namespace BYOC.Shared;

public class Result
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; }
}

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}

public class SuccessResult : Result
{
    public SuccessResult()
    {
        IsSuccess = true;
    }
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T value)
    {
        IsSuccess = true;
        Value = value;
    }
}

public class ErrorResult : Result
{
    public ErrorResult(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }
}

public class ErrorResult<T> : Result<T>
{
    public ErrorResult(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }
}