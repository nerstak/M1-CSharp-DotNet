using ClientText.view;

namespace ClientText
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 8976);
            client.Start();
        }
    }
}