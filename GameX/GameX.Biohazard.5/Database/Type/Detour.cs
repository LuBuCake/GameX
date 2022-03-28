namespace GameX.Database.Type
{
    public class Detour
    {
        private string DetourName { get; set; }
        private int DetourAddress { get; set; }
        private int DetourCallAddress { get; set; }
        private byte[] DetourCallInstruction { get; set; }
        private byte[] DetourContent { get; set; }
        private bool DetourJumpBack { get; set; }
        private int DetourJumpBackAddress { get; set; }

        public Detour(string Name, int Address, int CallAddress, byte[] CallInstruction, byte[] Content, bool JumpBack = false, int JumpBackAddress = 0)
        {
            DetourName = Name;
            DetourAddress = Address;
            DetourCallAddress = CallAddress;
            DetourCallInstruction = CallInstruction;
            DetourContent = Content;
            DetourJumpBack = JumpBack;
            DetourJumpBackAddress = JumpBackAddress;
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

        public int JumpBackAddress()
        {
            return DetourJumpBackAddress;
        }

        public int Size()
        {
            return Content().Length;
        }

        public override string ToString()
        {
            return Name();
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case string s:
                    return Name() == s;
                case int i:
                    return (Address() == i) || (CallAddress() == i);
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}