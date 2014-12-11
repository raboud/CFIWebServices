using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFI
{
    public class FileTransferManager
    {
        private FileUploader fileUploader = new FileUploader();
        public FileUploader Uploader
        {
            get { return fileUploader; }
        }

        private FileDownloader fileDownloader = new FileDownloader();
        public FileDownloader Downloader
        {
            get { return fileDownloader; }
        }

    }
}
