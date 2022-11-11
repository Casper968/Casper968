using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Infrastructure;
public class Result
{
    public bool Success { get; }

    public string? Message { get; }

    public Result(bool success, string? message = null)
    {
        Success = success;
        Message = message;
    }

    public static Result Fail(string error)
    {
        return new Result(false, error);
    }

    public static Result Fail<TValue>(TValue value, string error)
    {
        return new Result<TValue>(value, false, error);
    }

    public static Result Ok()
    {
        return new Result(true);
    }

    public static Result<TValue> Ok<TValue>(TValue value)
    {
        return new Result<TValue>(value, true);
    }

    public static Result<TValue> Ok<TValue>(TValue value, string message)
    {
        return new Result<TValue>(value, true, message);
    }

    public static Result<TValue> Fail<TValue>(string error)
    {
        return new Result<TValue>(default, false, error);
    }
}