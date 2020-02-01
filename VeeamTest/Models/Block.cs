namespace VeeamTest.Models
{
    public class Block
    {
        public Block(byte[] data)
        {
            Data = data;
        }
        
        
        public byte[] Data { get; set; }
    }
}