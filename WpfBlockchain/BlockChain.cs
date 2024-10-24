using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBlockchain
{
    //[Serializable]
    public class BlockChain
    {
        public List<Block> Chain { get; set; }
        DateTime Created { get; set; } = DateTime.Now;

        public BlockChain() { }

        public BlockChain(bool generateFirstBlock = false)
        {
            Chain = new List<Block>();
            Created = DateTime.Now;
            if (generateFirstBlock)
            {
                Block b = new Block(DateTime.Now, "{}");
                AddBlock(b);
            }
        }

        public void AddBlock(Block block)
        {
            int index = Chain.Count;
            block.Index = index;
            if (index > 0)
            {
                block.PreviousHash = Chain[index - 1].Hash;
            }
            block.CalculateHash();
            Chain.Add(block);
        }
    }
}
