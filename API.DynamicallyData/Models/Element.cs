namespace API.DynamicallyData.Models
{
    public struct Element
    {
        public Element(string key, string value)
        {
            this.key = key;
            this.Value = value;
        }

        public string key { get; }
        public string Value { get; }
    }
}
