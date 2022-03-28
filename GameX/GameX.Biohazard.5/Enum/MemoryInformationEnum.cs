using System.ComponentModel;

namespace GameX.Enum
{
    public enum MEMORY_INFORMATION
    {
        [Description("Detourates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages.")]
        MEM_COMMIT = 0x00001000,

        [Description("Indicates free pages not accessible to the calling process and available to be Detourated.")]
        MEM_FREE = 0x10000,

        [Description("Reserves a range of the process's virtual address space without Detourating any actual physical storage in memory or in the paging file on disk.")]
        MEM_RESERVE = 0x00002000,

        [Description("Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest.")]
        MEM_RESET = 0x00080000,

        [Description("MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier.")]
        MEM_RESET_UNDO = 0x1000000,

        [Description("Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.")]
        MEM_DECOMMIT = 0x00004000,

        [Description("Releases the specified region of pages, or placeholder (for a placeholder, the address space is released and available for other Detours). After this operation, the pages are in the free state.")]
        MEM_RELEASE = 0x00008000
    }
}
