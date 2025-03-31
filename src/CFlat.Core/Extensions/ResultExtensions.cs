using System.Runtime.CompilerServices;
using FMOD;

namespace CFlat.Core.Extensions;

/// <summary>A <see langword="static"/> collection of methods to extend functionality of <see cref="RESULT"/>.</summary>
public static class ResultExtensions
{
    /// <summary>Throws an <see cref="InvalidOperationException"/> if <paramref name="result"/> is anything but <see cref="RESULT.OK"/>.</summary>
    /// <param name="result">The result to check for failure.</param>
    /// <param name="message">The message to show in the exception when <paramref name="result"/> is a failure.</param>
    /// <param name="callerMemberName">The name of the method that invoked this method.</param>
    /// <returns>The <paramref name="result"/> if it was successful.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static RESULT OnFailure(this RESULT result, string message, [CallerMemberName] string callerMemberName = "")
    {
        if (result == RESULT.OK)
        {
            return result;
        }

        throw new InvalidOperationException($"\n\n[{callerMemberName}]\n\t{message}\n[FMOD ERROR]\n\t{result}\n");
    }

    /// <summary>Runs the <paramref name="action"/> if <paramref name="result"/> is <see cref="RESULT.OK"/>.</summary>
    /// <param name="result">The result to check for success.</param>
    /// <param name="action">An action to run if the <paramref name="result"/> was successful.</param>
    /// <returns>The <paramref name="result"/> if it failed, <see cref="RESULT.OK"/> otherwise.</returns>
    public static RESULT OnSuccess(this RESULT result, Action action)
    {
        if (result != RESULT.OK)
        {
            return result;
        }

        action.Invoke();
        return RESULT.OK;
    }

    /// <summary>Returns the output of the <paramref name="func"/> if <paramref name="result"/> is <see cref="RESULT.OK"/>.</summary>
    /// <typeparam name="T">The type of object to return from this method.</typeparam>
    /// <param name="result">The result to check for success.</param>
    /// <param name="func">A function to run if <paramref name="result"/> is <see cref="RESULT.OK"/>.</param>
    /// <returns>The <typeparamref name="T"/> output from <paramref name="func"/>, <see langword="default"/> otherwise.</returns>
    public static T? GetValueOrDefault<T>(this RESULT result, Func<T> func)
    {
        if (result == RESULT.OK)
        {
            return func.Invoke();
        }

        return default;
    }
}
