using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace NoReinstallHub;

internal static class NativeJunction
{
    // IO_REPARSE_TAG_MOUNT_POINT, not IO_REPARSE_TAG_SYMLINK.
    internal static byte[] BuildBuffer(string source)
    {
        var substitute = Encoding.Unicode.GetBytes(@"\??\" + source);
        var display = Encoding.Unicode.GetBytes(source);
        var size = checked(16 + substitute.Length + 2 + display.Length + 2);
        if (size > 16384)
            throw new JunctionException("PathTooLong");
        using var stream = new MemoryStream(size);
        using var writer = new BinaryWriter(stream, Encoding.Unicode, leaveOpen: true);
        writer.Write(0xA0000003u);
        writer.Write(checked((ushort)(size - 8)));
        writer.Write((ushort)0);
        writer.Write((ushort)0);
        writer.Write(checked((ushort)substitute.Length));
        writer.Write(checked((ushort)(substitute.Length + 2)));
        writer.Write(checked((ushort)display.Length));
        writer.Write(substitute);
        writer.Write((ushort)0);
        writer.Write(display);
        writer.Write((ushort)0);
        return stream.ToArray();
    }

    internal static void Create(string source, string destination)
    {
        var buffer = BuildBuffer(source);
        // Unlike Directory.CreateDirectory, this fails if another process wins the name.
        if (!CreateDirectoryW(destination, IntPtr.Zero))
            ThrowNative(Marshal.GetLastWin32Error());
        // A failed conversion leaves the newly created empty directory. Never delete user data.
        using var handle = CreateFileW(destination, 0x40000000, 0, IntPtr.Zero, 3,
            0x02000000 | 0x00200000, IntPtr.Zero);
        if (handle.IsInvalid)
            ThrowNative(Marshal.GetLastWin32Error());
        if (!DeviceIoControl(handle, 0x000900A4, buffer, buffer.Length, IntPtr.Zero, 0, out _, IntPtr.Zero))
            ThrowNative(Marshal.GetLastWin32Error());
    }

    private static void ThrowNative(int code)
    {
        var key = code switch
        {
            5 or 1314 => "Permission",
            80 or 183 => "DestinationExists",
            206 => "PathTooLong",
            32 or 33 => "InUse",
            _ => "NativeFailure"
        };
        throw new JunctionException(key, new Win32Exception(code));
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateDirectoryW(string path, IntPtr attributes);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFileW(string path, uint access, uint share,
        IntPtr attributes, uint creation, uint flags, IntPtr template);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeviceIoControl(SafeFileHandle handle, uint control,
        byte[] input, int inputSize, IntPtr output, int outputSize, out int returned, IntPtr overlapped);
}
