namespace UnitTest
{
    public class GenericObject
    {
        private string test;

        public GenericObject(string value)
        {
            test = value;
        }

        public override string ToString()
        {
            return test;
        }
    }
}
