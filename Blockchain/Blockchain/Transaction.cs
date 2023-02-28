using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
   public  class Transaction
    {
        public string Gonderen { get; set; }
        public string Alici { get; set; }
        public int Miktar { get; set; }
        public Transaction(string gonderen, string alici, int miktar)
        {
            Gonderen = gonderen;
            Alici = alici;
            Miktar = miktar;
        }

    }
}
