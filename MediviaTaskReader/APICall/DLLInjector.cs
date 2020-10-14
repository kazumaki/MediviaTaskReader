using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MediviaTaskReader.APICall
{
  public static class DLLInjector
  {
    [Flags]
    public enum AllocationType
    {
      Commit = 0x1000,
      Reserve = 0x2000,
      Decommit = 0x4000,
      Release = 0x8000,
      Reset = 0x80000,
      Physical = 0x400000,
      TopDown = 0x100000,
      WriteWatch = 0x200000,
      LargePages = 0x20000000
    }

    [Flags]
    public enum MemoryProtection
    {
      Execute = 0x10,
      ExecuteRead = 0x20,
      ExecuteReadWrite = 0x40,
      ExecuteWriteCopy = 0x80,
      NoAccess = 0x01,
      ReadOnly = 0x02,
      ReadWrite = 0x04,
      WriteCopy = 0x08,
      GuardModifierflag = 0x100,
      NoCacheModifierflag = 0x200,
      WriteCombineModifierflag = 0x400
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
   uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll")]
    public static extern Int64 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
    [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess,
   IntPtr lpThreadAttributes, uint dwStackSize, IntPtr
   lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    public static bool Inject(IntPtr handle, string dllPath)
    {
      if (handle == IntPtr.Zero)
      {
        return false;
      }

      IntPtr loadLibAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

      if (loadLibAddress == IntPtr.Zero)
      {
        return false;
      }

      IntPtr memoryAllocationAddress = VirtualAllocEx(handle, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ReadWrite);

      if (memoryAllocationAddress == IntPtr.Zero)
      {
        return false;
      }

      IntPtr bytesWritten;

      if ((WriteProcessMemory(handle, memoryAllocationAddress, Encoding.Default.GetBytes(dllPath), (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten)) == 0)
      {
        return false;
      }

      IntPtr result = CreateRemoteThread(handle, IntPtr.Zero, 0, loadLibAddress, memoryAllocationAddress, 0, IntPtr.Zero);

      if (result != IntPtr.Zero)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
