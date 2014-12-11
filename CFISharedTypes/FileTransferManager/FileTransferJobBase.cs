using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFI
{
    public abstract class FileTransferJobBase
    {
        protected int numChunks;
        protected int fragmentChunkSize;
        
        protected FileTransferStatus status = FileTransferStatus.Pending;
        public FileTransferStatus Status
        {
            get { return status; }
            set 
            {
                status = value;
            }
        }

        protected int totalBytes;
        public int TotalBytes
        {
            get { return totalBytes; }
        }

        protected int chunkSize;
        public int ChunkSize
        {
            get { return chunkSize; }
            set
            {
                chunkSize = value;
                calculateChunkInfo();
            }
        }

        protected string id;
        public string ID
        {
            get { return id; }
        }

        protected byte[] bytes;
        public byte[] Bytes
        {
            get { return bytes; }
        }

        protected DateTime lastUpdateTime;
        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
        }

        private FileTransferJobBase() { }

        public FileTransferJobBase(byte[] buffer, int chunkSize)
        {
            this.id = Guid.NewGuid().ToString();
            this.totalBytes = buffer.Length;
            this.chunkSize = chunkSize;
            this.lastUpdateTime = DateTime.Now;
            this.bytes = buffer;

            if ( totalBytes < 1 ) 
            {
                throw new ArgumentException("total bytes must be one or more");
            }

            calculateChunkInfo();
        }

        private void calculateChunkInfo()
        {
            // figure out the number of chunks and the size of the last one
            numChunks = totalBytes / chunkSize;
            fragmentChunkSize = totalBytes % chunkSize;
            if (fragmentChunkSize > 0)
            {
                numChunks++;
            }
        }

    }
}
