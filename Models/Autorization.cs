namespace pricheson.Models
{
    public class Autorization
    {
        public string checker { get; set; }
        
        public string Checkistrue()
        {
            return checker="1";
        }
        public string Checkisfalse()
        {
            return checker = "0";
        }
        public void cw()
        {
            Console.WriteLine(checker);
        }
    }
}
