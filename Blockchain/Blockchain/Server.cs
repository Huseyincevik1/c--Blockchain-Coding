using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Blockchain
{
    public class Server : WebSocketBehavior
    {
        bool synched = false;
        WebSocketServer webss = null;
        public void Start()
        {
            webss = new WebSocketServer($"ws//127.0.0.1:{Program.Port}");
            webss.AddWebSocketService<Server>("/Blockchain");
            webss.Start();
            Console.WriteLine($"Server Başlatıldı ws://127.0.0.1:{Program.Port}");
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Merhaba Server")
            {
                Console.WriteLine(e.Data);
                Send("Merhaba Client");
            }
            else
            {
                Blockchain chain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                if (chain.IsValid() && chain.Chain.Count > Program.blockchain.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(chain.PendingTransactions);
                    newTransactions.AddRange(Program.blockchain.PendingTransactions);
                    chain.PendingTransactions = newTransactions;
                    Program.blockchain = chain;
                }

            }
            if (!synched)
            {
                Send(JsonConvert.SerializeObject(Program.blockchain));
                synched = true;
            }
        }

    }
}
