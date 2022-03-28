using System.ComponentModel;

namespace GameX.Enum
{
    public enum MEMORY_ACCESS
    {
        [Description("Grants all other accesses.")]
        PROCESS_ALL_ACCESS = 0x1F0FFF,

        [Description("Required to create a process.")]
        PROCESS_CREATE_PROCESS = 0x0080,

        [Description("Required to create a thread.")]
        PROCESS_CREATE_THREAD = 0x0002,

        [Description("Required to suspend or resume a process.")]
        PROCESS_SUSPEND_RESUME = 0x0800,

        [Description("Required to terminate a process using TerminateProcess.")]
        PROCESS_TERMINATE = 0x0001,

        [Description("Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).")]
        PROCESS_VM_OPERATION = 0x0008,

        [Description("Required to read memory in a process using ReadProcessMemory.")]
        PROCESS_VM_READ = 0x0010,

        [Description("Required to write to memory in a process using WriteProcessMemory.")]
        PROCESS_VM_WRITE = 0x0020,

        [Description("Required to wait for the process to terminate using the wait functions. (LONG)")]
        SYNCHRONIZE = 0x00100000
    }
}
