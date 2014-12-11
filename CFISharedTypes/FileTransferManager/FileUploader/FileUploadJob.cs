using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFI
{
    public class FileUploadJob : FileTransferJobBase
    {
        public FileUploadJob(int totalBytes, int chunkSize)
            : this( new byte[ totalBytes ], chunkSize)
        {
        }
        
        public FileUploadJob(byte[] preAllocatedUploadTargetBuffer, int chunkSize)
            : base(preAllocatedUploadTargetBuffer, chunkSize)
        {
        }

        public bool UploadPart(int chunkIndex, byte[] part)
        {
            lock ( this )
            {
                bool validState = ((status == FileTransferStatus.InProgress) || (status == FileTransferStatus.Pending));
                if ( validState == false )
                {
                    throw new InvalidOperationException( string.Format("Cannot upload file part to a job in the {0} state", status.ToString()));
                }
                
                try
                {
                    int destinationStartIndex = chunkSize * chunkIndex;
                    Array.Copy(part, 0, bytes, destinationStartIndex, part.Length);
                    this.status = FileTransferStatus.InProgress;
                    this.lastUpdateTime = DateTime.Now;
                    return true;
                }
                catch
                {
                    this.lastUpdateTime = DateTime.Now;
                    return false;
                }
            }
        }

        public void Cancel()
        {
            lock ( this )
            {
                if ((status == FileTransferStatus.InProgress) || (status == FileTransferStatus.Pending))
                {
                    bytes = null;
                    this.Status = FileTransferStatus.Cancelled;
                    this.lastUpdateTime = DateTime.Now;
                }
            }
        }

        public void EndUpload()
        {
            lock (this)
            {
                bool validState = (status == FileTransferStatus.InProgress);
                if (validState == false)
                {
                    throw new InvalidOperationException(string.Format("Cannot complete upload of file to a job in the {0} state", status.ToString()));
                }

                this.Status = FileTransferStatus.Complete;
                this.lastUpdateTime = DateTime.Now;
            }
        }
    }
}
