using System;
using System.Runtime.CompilerServices;
using FMOD;

namespace CFlat.Core.Extensions;

public static class ResultExtensions
{
    public static RESULT OnFailure(this RESULT result, string message, [CallerMemberName] string callerMemberName = "")
    {
        if (result == RESULT.OK)
        {
            return result;
        }

        throw new InvalidOperationException($"\n\n[{callerMemberName}]\n\t{message}\n[FMOD ERROR]\n\t{result}\n");
    }

    public static RESULT OnSuccess(this RESULT result, Action action)
    {
        if (result != RESULT.OK)
        {
            return result;
        }

        action.Invoke();
        return RESULT.OK;
    }

    public static T? GetValueOrDefault<T>(this RESULT result, Func<T> func)
    {
        if (result == RESULT.OK)
        {
            return func.Invoke();
        }

        return default;
    }
}
