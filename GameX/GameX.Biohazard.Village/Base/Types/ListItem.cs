namespace GameX.Base.Types
{
    public class ListItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public ListItem(string text)
        {
            Text = text;
        }

        public ListItem(string text, int value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}