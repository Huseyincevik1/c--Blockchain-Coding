using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace Blockchain
{
    public class Client
    {
        IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();
        public void Connect(string url)
        {
            if (!wsDict.ContainsKey(url))
            {
                WebSocket websocket = new WebSocket(url);

                websocket.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Merhaba Client")
                    {
                        Console.WriteLine(e.Data);
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
                };
                websocket.Connect();
                websocket.Send("Merhaba Client");
                websocket.Send(JsonConvert.SerializeObject(Program.blockchain));
                wsDict.Add(url, websocket);
            }
        }
        public void Send(string url, string data)
        {
            foreach (var i in wsDict)
            {
                if (i.Key == url)
                {
                    i.Value.Send(data);
                }
            }
        }
        public void Broadcast(string data)
        {

            foreach (var i in wsDict)
            {
                i.Value.Send(data);
            }
        }
        public IList<string> GetServer()
        {
            IList<string> servers = new List<string>();
            foreach (var i in wsDict)
            {
                servers.Add(i.Key);
            }
            return servers;
        }
        public void close()
        {
            foreach (var i in wsDict)
            {
                i.Value.Close();
            }
        }
    }
}
