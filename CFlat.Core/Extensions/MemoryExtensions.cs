using System.Runtime.InteropServices;

namespace CFlat.Core.Extensions;

/// <summary>A collection of <see langword="static"/> methods to interact with memory.</summary>
public static class MemoryExtensions
{
    /// <summary>Gets a pointer for the <paramref name="obj"/>.</summary>
    /// <param name="obj">The object to get a pointer for.</param>
    /// <returns>A pointer to the <paramref name="obj"/> in unmanaged memory.</returns>
    public static IntPtr GetPointer(object? obj) => GCHandle.ToIntPtr(GCHandle.Alloc(obj));

    /// <summary>Gets the <typeparamref name="T"/> value from the <paramref name="pointer"/>.</summary>
    /// <typeparam name="T">The type of value to return from the <paramref name="pointer"/>.</typeparam>
    /// <param name="pointer">The pointer to unmanaged memory where the <typeparamref name="T"/> lives.</param>
    /// <returns>The <typeparamref name="T"/> value from the <paramref name="pointer"/>, <see langword="default"/> otherwise.</returns>
    public static T? FromPointer<T>(IntPtr pointer) => GCHandle.FromIntPtr(pointer).Target is T obj ? obj : default;

    /// <summary>Frees memory for the value that lives at the <paramref name="pointer"/>.</summary>
    /// <param name="pointer">The pointer where the value lives to be freed.</param>
    [UnmanagedCallersOnly(EntryPoint = "cflat_free")]
    public static void Free(IntPtr pointer) => GCHandle.FromIntPtr(pointer).Free();
}
