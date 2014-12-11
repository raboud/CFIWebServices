using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace CFI
{
    public class FileUploader
    {
        private ConcurrentDictionary<string, FileUploadJob> table = new ConcurrentDictionary<string, FileUploadJob>(StringComparer.OrdinalIgnoreCase);

        public string StartUpload(int totalBytes, int chunkSize)
        {
            FileUploadJob job = new FileUploadJob( totalBytes, chunkSize );
            table[job.ID] = job;
            return job.ID;
        }

        public bool UploadPart(string transferToken, int chunkIndex, byte[] part)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return false;
                }
                FileUploadJob job = table[transferToken];
                if (job.UploadPart(chunkIndex, part) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        public bool EndUpload(string transferToken)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return false;
                }
                FileUploadJob job = table[transferToken];
                job.EndUpload();
                return true;
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


        public bool CancelUpload(string transferToken)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return false;
                }
                FileUploadJob job = table[transferToken];
                job.Cancel();
                return true;
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
                foreach (FileUploadJob job in table.Values)
                {
                    TimeSpan timeSinceLastUpdate = DateTime.Now.Subtract(job.LastUpdateTime);

                    switch (job.Status)
                    {
                        case FileTransferStatus.Pending:
                        case FileTransferStatus.InProgress:
                            if (timeSinceLastUpdate > TimeSpan.FromMinutes(1))
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
                        FileUploadJob notUsed;
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

        public byte[] ClaimFile(string transferToken)
        {
            try
            {
                if (table.ContainsKey(transferToken) == false)
                {
                    return null;
                }

                FileUploadJob job;
                if (table.TryRemove(transferToken, out job) == false)
                {
                    return null;
                }
                return job.Bytes;
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
    }
}
