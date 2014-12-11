using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace CFI
{
    public class FileDownloader
    {
        private ConcurrentDictionary<string, FileDownloadJob> table = new ConcurrentDictionary<string, FileDownloadJob>(StringComparer.OrdinalIgnoreCase);

        public string QueueFile(byte[] bytes)
        {
            FileDownloadJob job = new FileDownloadJob(bytes, -1);
            table[job.ID] = job;
            sanitizeTable();
            return job.ID;
        }

        public int StartDownload(string transferToken, int chunkSize)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return 0;
                }
                FileDownloadJob job = table[transferToken];
                job.ChunkSize = chunkSize;
                return job.Bytes.Length;
            }
            catch
            {
                return 0;
            }
            finally
            {
                sanitizeTable();
            }
        }

        public byte[] DownloadPart(string transferToken, int chunkIndex)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return null;
                }
                FileDownloadJob job = table[transferToken];
                return job.DownloadPart(chunkIndex);
            }
            catch
            {
                return null;
            }
            finally
            {
                sanitizeTable();
            }
        }

        public bool CancelDownload(string transferToken)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return false;
                }
                FileDownloadJob job = table[transferToken];
                return job.Cancel();
            }
            catch
            {
                return false;
            }
            finally
            {
                sanitizeTable();
            }
        }

        public bool EndDownload(string transferToken)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return false;
                }
                FileDownloadJob job = table[transferToken];
                return job.EndDownload();
            }
            catch
            {
                return false;
            }
            finally
            {
                sanitizeTable();
            }
        }

        private void sanitizeTable()
        {
            try
            {
                // clear all jobs that have been around beyond the maximum time limit or that have failed
                List<string> jobsToRemove = new List<string>();
                foreach (FileDownloadJob job in table.Values)
                {
                    TimeSpan timeSinceLastUpdate = DateTime.Now.Subtract(job.LastUpdateTime);

                    switch (job.Status)
                    {
                        case FileTransferStatus.Pending:
                        case FileTransferStatus.InProgress:
                            if (timeSinceLastUpdate > TimeSpan.FromMinutes(5))
                            {
                                jobsToRemove.Add(job.ID);
                            }
                            break;
                        case FileTransferStatus.Complete:
                            if (timeSinceLastUpdate > TimeSpan.FromMinutes(10))
                            {
                                jobsToRemove.Add(job.ID);
                            }
                            break;
                        case FileTransferStatus.Cancelled:
                        default:
                            // remove immediately
                            jobsToRemove.Add(job.ID);
                            break;
                    }
                }

                foreach (string id in jobsToRemove)
                {
                    try
                    {
                        FileDownloadJob notUsed;
                        table.TryRemove(id, out notUsed);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }


    }
}
