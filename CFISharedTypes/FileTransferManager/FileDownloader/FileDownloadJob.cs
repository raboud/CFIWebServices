using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFI
{
    public class FileDownloadJob : FileTransferJobBase
    {
        public FileDownloadJob( byte[] bytesToDownload, int chunkSize )
            : base(bytesToDownload, chunkSize)
        {
        }

        public byte[] DownloadPart(int chunkIndex)
        {
            lock ( this )
            {
                // deliver the bytes from the specified slot
                byte[] part;
                if ((fragmentChunkSize > 0) && (chunkIndex == (numChunks - 1)))
                {
                    // we're on the last chunk and it is not a full-sized one
                    part = new byte[fragmentChunkSize];
                    Array.Copy(bytes, chunkIndex * chunkSize, part, 0, fragmentChunkSize);
                }
                else
                {
                    part = new byte[chunkSize];
                    Array.Copy(bytes, chunkIndex * chunkSize, part, 0, chunkSize);
                }
                this.status = FileTransferStatus.InProgress;
                this.lastUpdateTime = DateTime.Now;
                return part;
            }
        }

        public bool Cancel()
        {
            lock (this)
            {
                if ((status == FileTransferStatus.InProgress) || (status == FileTransferStatus.Pending))
                {
                    bytes = null;
                    this.Status = FileTransferStatus.Cancelled;
                    this.lastUpdateTime = DateTime.Now;
                }
                return true;
            }
        }

        public bool EndDownload()
        {
            lock (this)
            {
                bool validState = (status == FileTransferStatus.InProgress);
                if (validState == false)
                {
                    throw new InvalidOperationException(string.Format("Cannot complete download of file to a job in the {0} state", status.ToString()));
                }

                this.Status = FileTransferStatus.Complete;
                this.lastUpdateTime = DateTime.Now;
                return true;
            }
        }
    }
}
