using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFI
{
    public class FileTransferJobStatusChangeEventArgs : EventArgs
    {
        private FileTransferStatus oldStatus;
        public FileTransferStatus OldStatus
        {
            get { return oldStatus; }
        }

        private FileTransferStatus newStatus;
        public FileTransferStatus NewStatus
        {
            get { return newStatus; }
        }

        private FileTransferJobStatusChangeEventArgs()
        {
        }

        public FileTransferJobStatusChangeEventArgs( FileTransferStatus oldStatus, FileTransferStatus newStatus)
        {
            this.oldStatus = oldStatus;
            this.newStatus = newStatus;
        }
    }

}
