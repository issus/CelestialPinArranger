namespace InfineonDaveTranslator.Data
{
    internal class Resource
    {
        public string Name { get; set; }
        public string Uri { get; set; }

        public Resource(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public override string ToString()
        {
            return $"{Name}: {Uri}";
        }
    }
}
