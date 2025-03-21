using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CFlat.Core.Extensions;

/// <summary>A collection of <see langword="static"/> methods to interact with memory.</summary>
public static class MemoryExtensions
{
    /// <summary>Gets a pointer for the <paramref name="obj"/>.</summary>
    /// <param name="obj">The object to get a pointer for.</param>
    /// <returns>A pointer to the <paramref name="obj"/> in unmanaged memory.</returns>
    public static nint GetPointer(object? obj) => GCHandle.ToIntPtr(GCHandle.Alloc(obj));

    /// <summary>Gets the <typeparamref name="T"/> value from the <paramref name="pointer"/>.</summary>
    /// <typeparam name="T">The type of value to return from the <paramref name="pointer"/>.</typeparam>
    /// <param name="pointer">The pointer to unmanaged memory where the <typeparamref name="T"/> lives.</param>
    /// <returns>The <typeparamref name="T"/> value from the <paramref name="pointer"/>, <see langword="default"/> otherwise.</returns>
    public static T? FromPointer<T>(nint pointer) => GCHandle.FromIntPtr(pointer).Target is T obj ? obj : default;
    
    /// <summary>Gets a function pointer for the <paramref name="functionName"/>. Must define <paramref name="type"/> when getting a pointer to a function in another <see cref="Type"/>.</summary>
    /// <param name="functionName">A required name of a function to get a pointer for.</param>
    /// <param name="type">An optional type, useful when the function pointer lives inside of a different <see langword="class"/>.</param>
    /// <returns>A function pointer to the <paramref name="functionName"/>, defaults to <see cref="nint.Zero"/>.</returns>
    [RequiresUnreferencedCode("GetFunctionPointer uses GetMethod() which relies on unreferenced code.")]
    public static nint GetFunctionPointer(string functionName, Type? type = null) => (type == null
        ? new StackFrame(1).GetType().GetMethod(functionName)?.MethodHandle.GetFunctionPointer()
        : type.GetMethod(functionName)?.MethodHandle.GetFunctionPointer()) ?? nint.Zero;

    /// <summary>Frees memory for the value that lives at the <paramref name="pointer"/>.</summary>
    /// <param name="pointer">The pointer where the value lives to be freed.</param>
    [UnmanagedCallersOnly(EntryPoint = "cflat_free")]
    public static void Free(nint pointer) => GCHandle.FromIntPtr(pointer).Free();
}
