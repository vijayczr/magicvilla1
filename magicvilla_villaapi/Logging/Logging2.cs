namespace magicvilla_villaapi.Logging
{
    public class Logging2 : ILogging
    {
        public void Log(string message ,string type)
        {
            if(type=="error")
            {
                Console.BackgroundColor= ConsoleColor.Red;
                Console.WriteLine("ERROR --" + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                if(type=="warning")
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ERROR  --" + message);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}
