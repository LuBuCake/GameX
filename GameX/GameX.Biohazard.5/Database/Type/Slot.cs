using System;
using System.Linq;
using GameX.Modules;
using GameX.Enum;
using GameX.Database.ViewBag;
using GameX.Helpers;
using DevExpress.XtraEditors;

namespace GameX.Database.Type
{
    public class Slot
    {
        private SlotType Mode { get; set; }
        private App GUI { get; set; }
        private int PlayerIndex { get; set; }
        private int Index { get; set; }
        public bool BeingUpdatedOnGUI { get; set; }
        public TemporaryItemViewBag FromMemory { get; set; }
        public TemporaryItemViewBag ToMemory { get; set; }
        public Item FromMemoryItem { get; set; }
        public Item ToMemoryItem { get; set; }

        public Slot(SlotType Mode, App GUI, int PlayerIndex, int Index)
        {
            this.Mode = Mode;
            this.GUI = GUI;
            this.PlayerIndex = PlayerIndex;
            this.Index = Index;

            FromMemory = new TemporaryItemViewBag();
            ToMemory = new TemporaryItemViewBag();

            DB db = DBContext.GetDatabase();
            FromMemoryItem = db.Items.Where(x => x.Name == "Nothing").FirstOrDefault();
            ToMemoryItem = db.Items.Where(x => x.Name == "Nothing").FirstOrDefault();
        }

        private ComboBoxEdit ItemCB
        {
            get { return (ComboBoxEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}ItemCB" : $"P{PlayerIndex + 1}SlotKnifeItemCB"); }
        }

        private TextEdit QuantityTE
        {
            get { return (TextEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}QuantityTE" : $"P{PlayerIndex + 1}SlotKnifeQuantityTE"); }
        }

        private TextEdit MaxQuantityTE
        {
            get { return (TextEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}MaxQuantityTE" : $"P{PlayerIndex + 1}SlotKnifeMaxQuantityTE"); }
        }

        private SpinEdit FirepowerSE
        {
            get { return (SpinEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}FirepowerSE" : $"P{PlayerIndex + 1}SlotKnifeFirepowerSE"); }
        }

        private SpinEdit ReloadSpeedSE
        {
            get { return (SpinEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}ReloadSpeedSE" : $"P{PlayerIndex + 1}SlotKnifeReloadSpeedSE"); }
        }

        private SpinEdit CapacitySE
        {
            get { return (SpinEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}CapacitySE" : $"P{PlayerIndex + 1}SlotKnifeCapacitySE"); }
        }

        private SpinEdit CriticalSE
        {
            get { return (SpinEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}CriticalSE" : $"P{PlayerIndex + 1}SlotKnifeCriticalSE"); }
        }

        private SpinEdit PiercingSE
        {
            get { return (SpinEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}PiercingSE" : $"P{PlayerIndex + 1}SlotKnifePiercingSE"); }
        }

        private SpinEdit RangeSE
        {
            get { return (SpinEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}RangeSE" : $"P{PlayerIndex + 1}SlotKnifeRangeSE"); }
        }

        private SpinEdit ScopeSE
        {
            get { return (SpinEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}ScopeSE" : $"P{PlayerIndex + 1}SlotKnifeScopeSE"); }
        }

        private CheckEdit InfiniteAmmoCheckEdit
        {
            get { return (CheckEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}InfiniteAmmoCheckEdit" : $"P{PlayerIndex + 1}SlotKnifeInfiniteAmmoCheckEdit"); }
        }

        private CheckEdit RapidFireCheckEdit
        {
            get { return (CheckEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}RapidFireCheckEdit" : $"P{PlayerIndex + 1}SlotKnifeRapidFireCheckEdit"); }
        }

        private CheckEdit FrozenCheckEdit
        {
            get { return (CheckEdit)GUI.GetInventoryControl(PlayerIndex, Index, Index != 9 ? $"P{PlayerIndex + 1}Slot{Index + 1}FrozenCheckEdit" : $"P{PlayerIndex + 1}SlotKnifeFrozenCheckEdit"); }
        }

        public int Address
        {
            get
            {
                switch (Mode)
                {
                    case SlotType.Loadout:
                        return Memory.ReadPointer("re5dx9.exe", 0x00DA383C, 0x6FF40 + (0x420 * PlayerIndex) + (0x2C * Index));
                    case SlotType.RealTime:
                        return Memory.ReadPointer("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * PlayerIndex), 0x21A8 + (0x30 * Index));
                }

                return 0;
            }
        }

        public int ID
        {
            get { return Memory.Read<int>("", Address); }
            set { Memory.Write(value, "", Address); }
        }
        public int Quantity
        {
            get { return Memory.Read<int>("", Address + 0x04); }
            set { Memory.Write(value, "", Address + 0x04); }
        }
        public int MaxQuantity
        {
            get { return Memory.Read<int>("", Address + 0x08); }
            set { Memory.Write(value, "", Address + 0x08); }
        }
        public byte Firepower
        {
            get { return Memory.Read<byte>("", Address + 0x1C); }
            set { Memory.Write(value, "", Address + 0x1C); }
        }
        public byte ReloadSpeed
        {
            get { return Memory.Read<byte>("", Address + 0x1E); }
            set { Memory.Write(value, "", Address + 0x1E); }
        }
        public byte Capacity
        {
            get { return Memory.Read<byte>("", Address + 0x1F); }
            set { Memory.Write(value, "", Address + 0x1F); }
        }
        public byte Critical
        {
            get { return Memory.Read<byte>("", Address + 0x21); }
            set { Memory.Write(value, "", Address + 0x21); }
        }
        public byte Piercing
        {
            get { return Memory.Read<byte>("", Address + 0x22); }
            set { Memory.Write(value, "", Address + 0x22); }
        }
        public byte Range
        {
            get { return Memory.Read<byte>("", Address + 0x23); }
            set { Memory.Write(value, "", Address + 0x23); }
        }
        public byte Scope
        {
            get { return Memory.Read<byte>("", Address + 0x24); }
            set { Memory.Write(value, "", Address + 0x24); }
        }
        public bool RapidFire
        {
            get { return Memory.Read<bool>("", Address + 0x1D); }
            set { Memory.Write(value, "", Address + 0x1D); }
        }

        public override string ToString()
        {
            return $"Player {PlayerIndex + 1} - Slot {Index}";
        }

        public TemporaryItemViewBag ValidateTemporaryItem(TemporaryItemViewBag item, Item Reference)
        {
            item.Firepower = (byte)Utility.Clamp(item.Firepower, 0, Reference.Firepower.Length - 1);
            item.ReloadSpeed = (byte)Utility.Clamp(item.ReloadSpeed, 0, Reference.ReloadSpeed.Length - 1);
            item.Capacity = (byte)Utility.Clamp(item.Capacity, 0, Reference.Capacity.Length - 1);
            item.Critical = (byte)Utility.Clamp(item.Critical, 0, Reference.Critical.Length - 1);
            item.Piercing = (byte)Utility.Clamp(item.Piercing, 0, Reference.Piercing.Length - 1);
            item.Range = (byte)Utility.Clamp(item.Range, 0, Reference.Range.Length - 1);
            item.Scope = (byte)Utility.Clamp(item.Scope, 0, Reference.Scope.Length - 1);

            if (Reference.Name.Equals("Nothing"))
            {
                item.Quantity = 0;
                item.MaxQuantity = 0;
            }

            return item;
        }

        public void SetFromLoadoutTemporaryItem(TemporaryItemViewBag item, bool WriteOnMemory)
        {
            DB db = DBContext.GetDatabase();

            Item dbItem = (from x in db.Items
                           where x.ID == item.ID
                           select x).FirstOrDefault();

            if (dbItem == null)
                dbItem = db.Items.Where(x => x.Name == "Nothing").FirstOrDefault();

            item = ValidateTemporaryItem(item, dbItem);

            ToMemoryItem = dbItem;

            ToMemory.ID = ToMemoryItem.ID;
            ToMemory.Quantity = item.Quantity;
            ToMemory.MaxQuantity = item.MaxQuantity;
            ToMemory.Firepower = item.Firepower;
            ToMemory.ReloadSpeed = item.ReloadSpeed;
            ToMemory.Capacity = item.Capacity;
            ToMemory.Critical = item.Critical;
            ToMemory.Piercing = item.Piercing;
            ToMemory.Range = item.Range;
            ToMemory.Scope = item.Scope;

            BeingUpdatedOnGUI = true;

            if (!ItemCB.IsPopupOpen)
            {
                foreach (object obj in ItemCB.Properties.Items)
                    if ((obj as Item).ID == ToMemory.ID)
                        ItemCB.SelectedItem = obj;
            }

            if (!QuantityTE.Focused)
                QuantityTE.Text = ToMemory.Quantity.ToString();

            if (!MaxQuantityTE.Focused)
                MaxQuantityTE.Text = ToMemory.MaxQuantity.ToString();

            FirepowerSE.Text = ToMemoryItem.Firepower[ToMemory.Firepower].ToString();
            ReloadSpeedSE.Text = ((decimal)ToMemoryItem.ReloadSpeed[ToMemory.ReloadSpeed]).ToString("F");
            CapacitySE.Text = ToMemoryItem.Capacity[ToMemory.Capacity].ToString();
            CriticalSE.Text = ToMemoryItem.Critical[ToMemory.Critical].ToString();
            PiercingSE.Text = ToMemoryItem.Piercing[ToMemory.Piercing].ToString();
            RangeSE.Text = ToMemoryItem.Range[ToMemory.Range].ToString();
            ScopeSE.Text = ToMemoryItem.Scope[ToMemory.Scope].ToString();

            BeingUpdatedOnGUI = false;

            if (!WriteOnMemory)
                return;

            SetItem(ToMemory);
        }

        public void SetItem(Item item)
        {
            ID = item.ID;
            Quantity = item.Capacity[0];
            MaxQuantity = item.Capacity[0];
            Firepower = 0;
            ReloadSpeed = 0;
            Capacity = 0;
            Critical = 0;
            Piercing = 0;
            Range = 0;
            Scope = 0;
            RapidFire = RapidFireCheckEdit.Checked;

            DB db = DBContext.GetDatabase();

            Item dbItem = (from x in db.Items
                           where x.ID == item.ID
                           select x).FirstOrDefault();

            ToMemoryItem = dbItem;
            FromMemoryItem = dbItem;

            FromMemory.ID = item.ID;
            FromMemory.Quantity = item.Capacity[0];
            FromMemory.MaxQuantity = item.Capacity[0];
            FromMemory.Firepower = 0;
            FromMemory.ReloadSpeed = 0;
            FromMemory.Capacity = 0;
            FromMemory.Critical = 0;
            FromMemory.Piercing = 0;
            FromMemory.Range = 0;
            FromMemory.Scope = 0;
        }

        public void SetItem(TemporaryItemViewBag item)
        {
            ID = item.ID;
            Quantity = item.Quantity;
            MaxQuantity = item.MaxQuantity;
            Firepower = item.Firepower;
            ReloadSpeed = item.ReloadSpeed;
            Capacity = (byte)(item.Capacity + (byte)(InfiniteAmmoCheckEdit.Checked ? 0x80 : 0));
            Critical = item.Critical;
            Piercing = item.Piercing;
            Range = item.Range;
            Scope = item.Scope;
            RapidFire = RapidFireCheckEdit.Checked;

            DB db = DBContext.GetDatabase();

            Item dbItem = (from x in db.Items
                           where x.ID == item.ID
                           select x).FirstOrDefault();

            ToMemoryItem = dbItem;
            FromMemoryItem = dbItem;

            FromMemory.ID = item.ID;
            FromMemory.Quantity = item.Quantity;
            FromMemory.MaxQuantity = item.MaxQuantity;
            FromMemory.Firepower = item.Firepower;
            FromMemory.ReloadSpeed = item.ReloadSpeed;
            FromMemory.Capacity = item.Capacity;
            FromMemory.Critical = item.Critical;
            FromMemory.Piercing = item.Piercing;
            FromMemory.Range = item.Range;
            FromMemory.Scope = item.Scope;
        }

        public void ValidateItemInMemory(Item item)
        {
            Firepower = (byte)Utility.Clamp(Firepower, 0, item.Firepower.Length - 1);
            ReloadSpeed = (byte)Utility.Clamp(ReloadSpeed, 0, item.ReloadSpeed.Length - 1);
            Critical = (byte)Utility.Clamp(Critical, 0, item.Critical.Length - 1);
            Piercing = (byte)Utility.Clamp(Piercing, 0, item.Piercing.Length - 1);
            Range = (byte)Utility.Clamp(Range, 0, item.Range.Length - 1);
            Scope = (byte)Utility.Clamp(Scope, 0, item.Scope.Length - 1);

            if (Capacity > 13)
            {
                int CP = (byte)Utility.Clamp(Capacity > 13 ? (Capacity - 0x80) : Capacity, 0, item.Capacity.Length - 1);
                Capacity = (byte)(CP + 0x80);

                if (!InfiniteAmmoCheckEdit.Checked)
                    InfiniteAmmoCheckEdit.Checked = true;
            }
            else
            {
                if (InfiniteAmmoCheckEdit.Checked)
                    InfiniteAmmoCheckEdit.Checked = false;

                Capacity = (byte)Utility.Clamp(Capacity, 0, item.Capacity.Length - 1);
            }

            if (!RapidFire)
            {
                if (RapidFireCheckEdit.Checked)
                    RapidFireCheckEdit.Checked = false;
            }
            else
            {
                if (!RapidFireCheckEdit.Checked)
                    RapidFireCheckEdit.Checked = true;
            }

            if (item.Name.Equals("Nothing"))
            {
                Quantity = 0;
                MaxQuantity = 0;
            }
        }

        public void UpdateItemFromMemory(bool WriteOnGUI)
        {
            DB db = DBContext.GetDatabase();

            Item item = (from x in db.Items
                         where x.ID == ID
                         select x).FirstOrDefault();

            if (item == null)
            {
                item = db.Items.Where(x => x.Name == "Nothing").FirstOrDefault();
                SetItem(item);
            }

            ValidateItemInMemory(item);

            int CP = Capacity > 13 ? Capacity - 0x80 : Capacity;

            ToMemoryItem = item;
            FromMemoryItem = item;

            FromMemory.ID = ID;
            FromMemory.Quantity = Quantity;
            FromMemory.MaxQuantity = MaxQuantity;
            FromMemory.Firepower = Firepower;
            FromMemory.ReloadSpeed = ReloadSpeed;
            FromMemory.Capacity = (byte)CP;
            FromMemory.Critical = Critical;
            FromMemory.Piercing = Piercing;
            FromMemory.Range = Range;
            FromMemory.Scope = Scope;

            ToMemory.ID = ID;
            ToMemory.Quantity = Quantity;
            ToMemory.MaxQuantity = MaxQuantity;
            ToMemory.Firepower = Firepower;
            ToMemory.ReloadSpeed = ReloadSpeed;
            ToMemory.Capacity = (byte)CP;
            ToMemory.Critical = Critical;
            ToMemory.Piercing = Piercing;
            ToMemory.Range = Range;
            ToMemory.Scope = Scope;

            if (!WriteOnGUI)
                return;

            BeingUpdatedOnGUI = true;

            if (!ItemCB.IsPopupOpen)
            {
                foreach (object obj in ItemCB.Properties.Items)
                    if ((obj as Item).ID == ID)
                        ItemCB.SelectedItem = obj;
            }

            if (!QuantityTE.Focused)
                QuantityTE.Text = Quantity.ToString();

            if (!MaxQuantityTE.Focused)
                MaxQuantityTE.Text = MaxQuantity.ToString();

            FirepowerSE.Text = FromMemoryItem.Firepower[FromMemory.Firepower].ToString();
            ReloadSpeedSE.Text = ((decimal)FromMemoryItem.ReloadSpeed[FromMemory.ReloadSpeed]).ToString("F");
            CapacitySE.Text = FromMemoryItem.Capacity[CP].ToString();
            CriticalSE.Text = FromMemoryItem.Critical[FromMemory.Critical].ToString();
            PiercingSE.Text = FromMemoryItem.Piercing[FromMemory.Piercing].ToString();
            RangeSE.Text = FromMemoryItem.Range[FromMemory.Range].ToString();
            ScopeSE.Text = FromMemoryItem.Scope[FromMemory.Scope].ToString();

            BeingUpdatedOnGUI = false;
        }

        public void UpdateItemToMemory(bool WriteOnMemory)
        {
            ToMemoryItem = ItemCB.SelectedItem as Item;

            ToMemory.ID = ToMemoryItem.ID;
            ToMemory.Quantity = int.Parse(QuantityTE.Text);
            ToMemory.MaxQuantity = int.Parse(MaxQuantityTE.Text);
            ToMemory.Firepower = (byte)Array.IndexOf(ToMemoryItem.Firepower, int.Parse(FirepowerSE.Text));
            ToMemory.ReloadSpeed = (byte)Array.IndexOf(ToMemoryItem.ReloadSpeed, double.Parse(ReloadSpeedSE.Text));
            ToMemory.Capacity = (byte)Array.IndexOf(ToMemoryItem.Capacity, int.Parse(CapacitySE.Text));
            ToMemory.Critical = (byte)Array.IndexOf(ToMemoryItem.Critical, int.Parse(CriticalSE.Text));
            ToMemory.Piercing = (byte)Array.IndexOf(ToMemoryItem.Piercing, int.Parse(PiercingSE.Text));
            ToMemory.Range = (byte)Array.IndexOf(ToMemoryItem.Range, int.Parse(RangeSE.Text));
            ToMemory.Scope = (byte)Array.IndexOf(ToMemoryItem.Scope, int.Parse(ScopeSE.Text));

            if (!WriteOnMemory || FrozenCheckEdit.Checked)
                return;

            SetItem(ToMemory);
        }

        public void Update()
        {
            if (!FrozenCheckEdit.Checked)
            {
                UpdateItemFromMemory(!FrozenCheckEdit.Checked);
                return;
            }

            SetItem(ToMemory);
        }
    }
}