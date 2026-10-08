namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var server = new Server();
            await server.StartServer("127.0.0.1", 1234);
        }
    }
}
