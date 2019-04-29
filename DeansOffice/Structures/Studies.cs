namespace DeansOffice.Structures
{
    public class Studies
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            var toCheck = obj as Studies;
            return Name == toCheck.Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}