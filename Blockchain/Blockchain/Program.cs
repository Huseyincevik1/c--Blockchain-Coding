using System;
using Newtonsoft.Json;


namespace Blockchain
{
    class Program
    {
        public static Blockchain blockchain = new Blockchain();
        public static int Port = 0;
        public static Client client = new Client();
        public static Server server = null;
        public static string name = "";
        static void Main(string[] args)
        {
            blockchain.InializeChain();
            if (args.Length >= 1)
                Port = int.Parse(args[0]);
            if (args.Length >= 2)
                name = args[1];
            if (Port > 0)
            {
                server = new Server();
                server.Start();
            }
            if (name != "")
                Console.WriteLine("user:" + name);
            Console.WriteLine("*********************");
            Console.WriteLine("1. Servera Bağlan");
            Console.WriteLine("2. Transaction Ekle");
            Console.WriteLine("3. Blockchain Göster");
            Console.WriteLine("4. Çıkış");
            Console.WriteLine("**********************");

            int choose=0;

            switch (choose)
            {
                case 1:
                    Console.WriteLine("Server URL Girin");
                    string serverURL = Console.ReadLine();
                    client.Connect($"{serverURL}/Blockchain");
                    break;

                case 2:
                    Console.WriteLine("Alıcı Adı Girin");
                    string Rname = Console.ReadLine();
                    Console.WriteLine("Miktar Girin:");
                    string miktar = Console.ReadLine();
                    blockchain.CreateTransaction(new Transaction(name, Rname, int.Parse(miktar)));
                    blockchain.ProcessPendingTransactions(name);
                    client.Broadcast(JsonConvert.SerializeObject(blockchain));
                    break;

                case 3:
                    Console.WriteLine("Blockchain");
                    Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));
                    break;

                case 4:
                    client.close();
                    break;

                default:
                    Console.WriteLine("1 ve 4 Arasında Seçim Yapınız.");
                    break;
            }
            Console.WriteLine("Seçim Yapınız");
            string a = Console.ReadLine();
            choose = int.Parse(a);


           
            Console.ReadKey();

        }
    }
}
