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

        //public Block() { }

        public Block(DateTime timeStamp, string data)
        {
            TimeStamp = timeStamp;
            Data = data;
        }

        public void CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(
                $"{TimeStamp}-{PreviousHash}-{Data}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);
            Hash = Convert.ToBase64String(outputBytes);

        }
    }
}
