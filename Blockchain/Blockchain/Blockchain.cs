using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    public class Blockchain
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();
        public IList<Block> Chain { get; set; }
        public int dif { get; set; } = 1;
        public int Reward { get; set; } = 1;
        public Blockchain()
        {
            InializeChain();
            AddGenesisBlock();
        }
        public void InializeChain()
        {
            Chain = new List<Block>();
        }

        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(dif);
            PendingTransactions = new List<Transaction>();
            return block;
        }
        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        public Block GetLastestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        public void AddBlock(Block block)
        {
            Block lastblock = GetLastestBlock();
            block.Index = lastblock.Index + 1;
            block.PreviousHash = lastblock.Hash;
            block.Hash = block.CalculateHash();
            block.Mine(this.dif);
            Chain.Add(block);
        }
        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string mineAddress)
        {
            CreateTransaction(new Transaction(null, mineAddress, Reward));
            Block block = new Block(DateTime.Now, GetLastestBlock().Hash, PendingTransactions);
            AddBlock(block);
            PendingTransactions = new List<Transaction>();
            
        }
        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];
                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }
                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }

            }
            return true;
        }

        public int GetBalance(string address)
        {
            int balance = 0;
            for (int i = 1; i < Chain.Count; i++)
            {
                for(int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];
                    if (transaction.Gonderen == address)
                    {
                        balance -= transaction.Miktar;
                    }
                    if (transaction.Alici == address)
                    {
                        balance += transaction.Miktar;
                    }
                }
            }
            return balance;
        }

    }
}
