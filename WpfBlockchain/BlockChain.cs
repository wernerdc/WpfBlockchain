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
        public List<Block> Chain { get; set; } = new List<Block>();
        public DateTime Created { get; set; } = DateTime.Now;

        public BlockChain()     // empty Constructor is needed by jsonSerializer
        { 
        }   

        public BlockChain(bool generateFirstBlock = false)
        {
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

        public int ValidateChain()
        {
            for (int i = 0; i < Chain.Count; i++)
            {
                if (!Chain[i].ValidateHash())
                    return i;
                
                if (i > 0 && Chain[i].PreviousHash != Chain[i - 1].Hash)
                    return i;
            }
            return -1;      // -1 => OK, check passed
        }
    }
}
