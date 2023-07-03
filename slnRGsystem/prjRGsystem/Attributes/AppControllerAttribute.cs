namespace prjRGsystem.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppControllerAttribute : System.Attribute
    {
        private string Name;

        public AppControllerAttribute(string _name)
        {
            this.Name = _name;
        }

        public string GetName()
        {
            return this.Name;
        }
    }
}
