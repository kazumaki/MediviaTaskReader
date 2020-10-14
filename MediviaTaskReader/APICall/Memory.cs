using System;
using System.Runtime.InteropServices;

namespace MediviaTaskReader.APICall
{
  public static class Memory
  {
    [DllImport("kernel32.dll")]
    static extern bool ReadProcessMemory(IntPtr hProcess,
    UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, uint dwSize, out IntPtr lpNumberOfBytesRead);

    public static byte[] ReadBytes(IntPtr handle, ulong address, uint bytesToRead)
    {
      IntPtr ptrBytesRead;
      byte[] buffer = new byte[bytesToRead];

      ReadProcessMemory(handle, new UIntPtr(address), buffer, bytesToRead, out ptrBytesRead);

      return buffer;
    }
    public static byte ReadByte(IntPtr handle, ulong address)
    {
      return ReadBytes(handle, address, 1)[0];
    }
    public static string ReadString(IntPtr handle, ulong address)
    {
      string s = "";
      byte temp = ReadByte(handle, address++);
      while (temp != 0)
      {
        s += (char)temp;
        temp = ReadByte(handle, address++);
      }
      return s;
    }

    public static Int32 ReadInt32(IntPtr handle, ulong address)
    {
      return BitConverter.ToInt32(ReadBytes(handle, address, 4), 0);
    }

    public static UInt64 ReadUInt64(IntPtr handle, ulong address)
    {
      return BitConverter.ToUInt64(ReadBytes(handle, address, 8), 0);
    }
  }
}
