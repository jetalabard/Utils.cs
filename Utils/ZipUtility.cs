using System.IO.Compression;

namespace Utils
{
    public class ZipUtility
    {
        /// <summary>
        /// zip a directory
        /// </summary>
        /// <param name="pathDirectory"></param>
        /// <param name="zipPath"></param>
        public static void ZipDirectory(string pathDirectory, string zipPath)
        {
            try
            {
                ZipFile.CreateFromDirectory(pathDirectory, zipPath);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// un zip a directory or a file into a directory
        /// </summary>
        /// <param name="pathDirectory"></param>
        /// <param name="zipPath"></param>
        public static void UnZipDirectory(string pathDirectory, string zipPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, pathDirectory);
            }
            catch
            {
                throw;
            }

        }
    }

}
