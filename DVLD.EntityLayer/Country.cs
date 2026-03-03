using System;

namespace DVLD.EntityLayer
{
    public class Country
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public Country() { }

        public Country(int id, string name)
        {
            Id = id;
            Name = name?.Trim() ?? string.Empty;
        }
    }
}
