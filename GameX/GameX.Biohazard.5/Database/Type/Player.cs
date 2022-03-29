using System.Linq;
using System.Numerics;
using GameX.Modules;
using GameX.Enum;
using GameX.Helpers;

namespace GameX.Database.Type
{
    public class Player
    {
        private int Index { get; set; }
        public Inventory Inventory { get; set; }

        public Player(int Index, App GUI)
        {
            this.Index = Index;
            Inventory = new Inventory(Index, GUI);
        }

        #region Props

        public int Address
        {
            get { return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index)); }
        }

        public int PartnerAddress
        {
            get { return Memory.Read<short>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2A74); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2A74); }
        }

        public Player Partner
        {
            get { return Biohazard.Players.Where(x => x.Address != 0 && x.Address == PartnerAddress).FirstOrDefault(); }
            set { PartnerAddress = value.Address; }
        }

        public int Character
        {
            get { return Memory.Read<int>("re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * Index)); }
            set { Memory.Write(value, "re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * Index)); }
        }

        public int Costume
        {
            get { return Memory.Read<int>("re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * Index)); }
            set { Memory.Write(value, "re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * Index)); }
        }

        public bool Invulnerable
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C) == 0; }
            set { Memory.Write(!value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C); }
        }

        public bool AI
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C) == 1; }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C); }
        }

        public short Health
        {
            get { return Memory.Read<short>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1364); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1364); }
        }

        public short MaxHealth
        {
            get { return Memory.Read<short>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1366); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1366); }
        }

        public byte Handness
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF9); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF9); }
        }

        public byte WeaponMode
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF8); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF8); }
        }

        public int Melee
        {
            get { return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10E0); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10E0); }
        }

        public int MeleeTarget
        {
            get { return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2DA4); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2DA4); }
        }

        public Vector3 Position
        {
            get
            {
                float X = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD0);
                float Y = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD4);
                float Z = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD8);

                return new Vector3(X, Y, Z);
            }
            set
            {
                Memory.Write(value.X, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD0);
                Memory.Write(value.Y, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD4);
                Memory.Write(value.Z, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD8);
            }
        }

        public Vector3 MeleePosition
        {
            get
            {
                float X = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E10);
                float Y = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E14);
                float Z = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E18);

                return new Vector3(X, Y, Z);
            }
            set
            {
                Memory.Write(value.X, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E10);
                Memory.Write(value.Y, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E14);
                Memory.Write(value.Z, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E18);
            }
        }

        #endregion

        #region Methods

        public bool IsActive() => Address != 0;

        public void Kill() => Health = 0;

        public void Heal(short Value) => Health = (short)Utility.Clamp(Health + Value, 0, MaxHealth);

        public int GetDefaultHandness()
        {
            switch (Character)
            {
                case (int)CharacterEnum.Sheva:
                    return (int)HandnessEnum.Left;
                default:
                    return (int)HandnessEnum.Right;
            }
        }

        public int GetDefaultWeaponMode()
        {
            switch (Character)
            {
                case (int)CharacterEnum.Chris:
                case (int)CharacterEnum.Wesker:
                case (int)CharacterEnum.Josh:
                case (int)CharacterEnum.Barry:
                case (int)CharacterEnum.Irving:
                    return (int)WeaponModeEnum.Male;
                default:
                    return (int)WeaponModeEnum.Female;
            }
        }

        public bool DoingGrabMelee()
        {
            switch (Character)
            {
                case (int)CharacterEnum.Chris when Melee == (int)GrabMoveEnum.LegBack:
                case (int)CharacterEnum.Sheva when Melee == (int)GrabMoveEnum.LegBack || Melee == (int)GrabMoveEnum.FinisherFront:
                case (int)CharacterEnum.Jill when Melee == (int)GrabMoveEnum.LegBack:
                case (int)CharacterEnum.Josh when Melee == (int)GrabMoveEnum.LegBack || Melee == (int)GrabMoveEnum.FinisherFront:
                case (int)CharacterEnum.Excella when Melee == (int)GrabMoveEnum.LegBack || Melee == (int)GrabMoveEnum.ReunionLegFront:
                case (int)CharacterEnum.Barry when Melee == (int)GrabMoveEnum.ReunionHeadFlash:
                case (int)CharacterEnum.Rebecca when Melee == (int)GrabMoveEnum.LegBack:
                case (int)CharacterEnum.Irving when Melee == (int)GrabMoveEnum.LegBack:
                    return true;
                default:
                    return false;
            }
        }

        public bool DoingIdleMove()
        {
            switch (Melee)
            {
                case (int)IdleMoveEnum.StandStill:
                case (int)IdleMoveEnum.MoveFront:
                case (int)IdleMoveEnum.MoveBack:
                case (int)IdleMoveEnum.MoveLeft:
                case (int)IdleMoveEnum.MoveRight:
                case (int)IdleMoveEnum.QuickTurn:
                case (int)IdleMoveEnum.Idle:
                case (int)IdleMoveEnum.Running:
                    return true;
                default:
                    return false;
            }
        }

        public Item GetDefaultKnife()
        {
            DB db = DBContext.GetDatabase();

            Item Default = null;

            switch (Character)
            {
                case (int)CharacterEnum.Chris:
                    Default = db.Items.Where(x => x.Name == "Knife (Chris)").FirstOrDefault();
                    break;
                case (int)CharacterEnum.Sheva:
                    Default = db.Items.Where(x => x.Name == "Knife (Sheva)").FirstOrDefault();
                    break;
                case (int)CharacterEnum.Jill:
                    Default = db.Items.Where(x => x.Name == "Knife (Jill)").FirstOrDefault();
                    break;
                case (int)CharacterEnum.Wesker:
                    Default = db.Items.Where(x => x.Name == "Knife (Wesker)").FirstOrDefault();
                    break;
                case (int)CharacterEnum.Josh:
                case (int)CharacterEnum.Excella:
                case (int)CharacterEnum.Barry:
                case (int)CharacterEnum.Rebecca:
                case (int)CharacterEnum.Irving:
                    Default = db.Items.Where(x => x.Name == "Knife (Jill) (DLC)").FirstOrDefault();
                    break;
                default:
                    Default = db.Items.Where(x => x.Name == "Knife (Chris)").FirstOrDefault();
                    break;
            }

            return Default;
        }

        public void SetRapidFire(bool Enable)
        {
            for (int slot = 0; slot < 9; slot++)
                Inventory.RealTimeSlots[slot].RapidFire = Enable;
        }

        public void SetInfiniteAmmo(bool Enable)
        {
            DB db = DBContext.GetDatabase();

            for (int slot = 0; slot < 9; slot++)
            {
                int CurrentCapacity = Inventory.RealTimeSlots[slot].Capacity;

                if (CurrentCapacity > 13)
                    CurrentCapacity -= 0x80;

                Item dbItem = (from x in db.Items
                               where x.ID == Inventory.RealTimeSlots[slot].ID || (Inventory.RealTimeSlots[slot].ID == 256 && x.ID == 0)
                               select x).FirstOrDefault();

                if (dbItem == null)
                {
                    dbItem = db.Items.Where(x => x.Name == "Nothing").FirstOrDefault();
                    Inventory.RealTimeSlots[slot].SetItem(dbItem);
                }

                CurrentCapacity = (byte)Utility.Clamp(CurrentCapacity, 0, dbItem.Capacity.Length - 1);

                Inventory.RealTimeSlots[slot].MaxQuantity = dbItem.Capacity[CurrentCapacity];
                Inventory.RealTimeSlots[slot].Capacity = (byte)(CurrentCapacity + (Enable ? 0x80 : 0x00));
            }
        }

        public void ResetDash()
        {
            Memory.Write(0, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x210F);
            Memory.Write(1.3f, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10DC, 0x120, 0x1C);
            Memory.Write(0f, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10DC, 0x120, 0x2C);
        }

        #endregion
    }
}