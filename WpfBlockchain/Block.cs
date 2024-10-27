using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfBlockchain
{
    //[Serializable]
    public class Block
    {
        public int Index { get; set; } = 0;
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Data { get; set; } = String.Empty;
        public string Hash { get; set; } = String.Empty;
        public string PreviousHash { get; set; } = String.Empty;

        // DEBUG: empty constructor causes datagrid error that adds an empty row at the end
        //public Block() { }

        public Block(DateTime timeStamp, string data)
        {
            TimeStamp = timeStamp;
            Data = data;
        }

        public void CalculateHash()
        {
            Hash = EncodeHashString();
        }
        
        public bool ValidateHash() 
        { 
            return Hash == EncodeHashString(); 
        } 

        private string EncodeHashString()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash}-{Data}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

    }
}
