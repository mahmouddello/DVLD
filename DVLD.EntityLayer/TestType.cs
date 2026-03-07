using System;

namespace DVLD.EntityLayer
{
    public enum enTestType
    {
        None = 0,
        VisionTest = 1,
        WrittenTest = 2,
        PracticalTest = 3
    }

    public class TestType
    {
        public enTestType Type { get; set; } = enTestType.None;
        public int Id => (int)Type;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fees { get; set; } = 0;

        public TestType(enTestType type, string title, string description, decimal fees)
        {
            Type = type;
            Title = title;
            Description = description;
            Fees = fees;
        }
    }
}