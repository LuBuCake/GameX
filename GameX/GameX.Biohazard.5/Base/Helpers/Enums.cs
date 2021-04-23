using System.ComponentModel;

namespace GameX.Base.Helpers
{
    public static class Enums
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

            [Description(
                "Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).")]
            PROCESS_VM_OPERATION = 0x0008,

            [Description("Required to read memory in a process using ReadProcessMemory.")]
            PROCESS_VM_READ = 0x0010,

            [Description("Required to write to memory in a process using WriteProcessMemory.")]
            PROCESS_VM_WRITE = 0x0020,

            [Description("Required to wait for the process to terminate using the wait functions. (LONG)")]
            SYNCHRONIZE = 0x00100000
        }

        public enum MEMORY_PROTECTION
        {
            [Description("Enables execute access to the committed region of pages.")]
            PAGE_EXECUTE = 0x10,

            [Description("Enables execute or read-only access to the committed region of pages.")]
            PAGE_EXECUTE_READ = 0x20,

            [Description("Enables execute, read-only, or read/write access to the committed region of pages.")]
            PAGE_EXECUTE_READWRITE = 0x40,

            [Description(
                "Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object.")]
            PAGE_EXECUTE_WRITECOPY = 0x80,

            [Description("Disables all access to the committed region of pages.")]
            PAGE_NOACCESS = 0x01,

            [Description("Enables read-only access to the committed region of pages.")]
            PAGE_READONLY = 0x02,

            [Description("Enables read-only or read/write access to the committed region of pages.")]
            PAGE_READWRITE = 0x04,

            [Description("Enables read-only or copy-on-write access to a mapped view of a file mapping object.")]
            PAGE_WRITECOPY = 0x08,

            [Description("Sets all locations in the pages as invalid targets for CFG.")]
            PAGE_TARGETS_INVALID = 0x40000000,

            [Description(
                "Pages in the region will not have their CFG information updated while the protection changes for VirtualProtect.")]
            PAGE_TARGETS_NO_UPDATE = 0x40000000
        }

        public enum MEMORY_INFORMATION
        {
            [Description(
                "Detourates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages.")]
            MEM_COMMIT = 0x00001000,

            [Description("Indicates free pages not accessible to the calling process and available to be Detourated.")]
            MEM_FREE = 0x10000,

            [Description(
                "Reserves a range of the process's virtual address space without Detourating any actual physical storage in memory or in the paging file on disk.")]
            MEM_RESERVE = 0x00002000,

            [Description(
                "Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest.")]
            MEM_RESET = 0x00080000,

            [Description(
                "MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier.")]
            MEM_RESET_UNDO = 0x1000000,

            [Description(
                "Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.")]
            MEM_DECOMMIT = 0x00004000,

            [Description(
                "Releases the specified region of pages, or placeholder (for a placeholder, the address space is released and available for other Detours). After this operation, the pages are in the free state.")]
            MEM_RELEASE = 0x00008000
        }

        public enum ConsoleInterface
        {
            Console = 0,
            Server = 1,
            Client = 2
        }

        public enum MessageBoxType
        {
            None = 0,
            Error = 1,
            Warning = 2,
            Information = 3
        }
    }
}