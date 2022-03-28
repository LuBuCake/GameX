using System.ComponentModel;

namespace GameX.Enum
{
    public enum MEMORY_PROTECTION
    {
        [Description("Enables execute access to the committed region of pages.")]
        PAGE_EXECUTE = 0x10,

        [Description("Enables execute or read-only access to the committed region of pages.")]
        PAGE_EXECUTE_READ = 0x20,

        [Description("Enables execute, read-only, or read/write access to the committed region of pages.")]
        PAGE_EXECUTE_READWRITE = 0x40,

        [Description("Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object.")]
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

        [Description("Pages in the region will not have their CFG information updated while the protection changes for VirtualProtect.")]
        PAGE_TARGETS_NO_UPDATE = 0x40000000
    }
}
