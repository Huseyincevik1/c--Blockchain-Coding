using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Blockchain
{
   public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions { get; set; }
        
        public int Nonce { get; set; }

        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions=transactions;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 SHA256 = SHA256.Create();
            byte[] inbytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
            byte[] outbytes = SHA256.ComputeHash(inbytes);
            return Convert.ToBase64String(outbytes);
        }
        public void Mine(int dif)
        {
            var zeros = new string('0', dif);
            while(this.Hash == null || this.Hash.Substring(0, dif) != zeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }

        }

    }
}
