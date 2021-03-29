using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Types
{
    public class Detour
    {
        private string DetourName { get; set; }
        private int DetourAddress { get; set; }
        private int DetourCallAddress { get; set; }
        private byte[] DetourCallInstruction { get; set; }
        private byte[] DetourContent { get; set; }
        private bool DetourJumpBack { get; set; }

        public Detour(string Name, int Address, int CallAddress, byte[] CallInstruction, byte[] Content, bool JumpBack = false)
        {
            DetourName = Name;
            DetourAddress = Address;
            DetourCallAddress = CallAddress;
            DetourCallInstruction = CallInstruction;
            DetourContent = Content;
            DetourJumpBack = JumpBack;
        }

        public override string ToString()
        {
            return Name();
        }

        public string Name()
        {
            return DetourName;
        }

        public int Address()
        {
            return DetourAddress;
        }

        public int CallAddress()
        {
            return DetourCallAddress;
        }

        public byte[] Content()
        {
            return DetourContent;
        }

        public byte[] CallInstruction()
        {
            return DetourCallInstruction;
        }

        public bool JumpBack()
        {
            return DetourJumpBack;
        }

        public int Size()
        {
            return Content().Length;
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return Name() == (string)obj;
            }
            else if (obj is int)
            {
                return (Address() == (int)obj) || (CallAddress() == (int)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Address();
        }
    }
}
