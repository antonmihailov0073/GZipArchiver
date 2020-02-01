namespace VeeamTest.Models
{
    public class ArchiverSettings
    {
        public ArchiverSettings(ArchiverMode mode, string path, string destinationPath, int blockSize, long memoryLimitation)
        {
            Mode = mode;
            Path = path;
            DestinationPath = destinationPath;
            BlockSize = blockSize;
            MemoryLimitation = memoryLimitation;
        }
        
        
        public ArchiverMode Mode { get; }
        
        public string Path { get; }
        
        public string DestinationPath { get;}
        
        public int BlockSize { get; }
        
        public long MemoryLimitation { get; }
    }
}