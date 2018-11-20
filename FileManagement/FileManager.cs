using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Utils.FileManagement;

namespace Utils.FileManagement
{
    public static class FileManager
    {


        public static bool IsFileLocked(string filename)
        {
            FileStream stream = null;
            try
            {
                stream = new FileInfo(filename).Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// delete file when it exist and not use 
        /// if the delete method doesn't work, we wait a 'delay' time before to re-delete 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="delay"></param>
        public static void Delete(string fileName, int delay = 500)
        {
            if (File.Exists(fileName))
            {
                if (IsFileLocked(fileName))
                {
                    throw new InvalidOperationException("Filename : " + fileName + " is always use");
                }
                File.Delete(fileName);
            }

        }

        public static Process OpenFile(string filename)
        {
            Process process = null;

            if (!File.Exists(filename))
            {
                throw new ArgumentException("Filename : " + filename + " doesn't exist");
            }

            if (!IsFileLocked(filename))
            {
                process = Process.Start(filename);
            }
            return process;

        }


        public static void CloseFile(Process process)
        {
            process.Kill();
        }
    }
}
