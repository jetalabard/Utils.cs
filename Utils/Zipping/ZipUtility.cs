using System.IO.Compression;

namespace Utils.Zip
{
    public static class ZipUtility
    {

        /// <summary>
        /// zip a directory
        /// </summary>
        /// <param name="pathDirectory"></param>
        /// <param name="zipPath"></param>
        public static void ZipDirectory(string pathDirectory, string zipPath)
        {
            ZipFile.CreateFromDirectory(pathDirectory, zipPath);

        }

        /// <summary>
        /// un zip a directory or a file into a directory
        /// </summary>
        /// <param name="pathDirectory"></param>
        /// <param name="zipPath"></param>
        public static void UnZipDirectory(string pathDirectory, string zipPath)
        {
            ZipFile.ExtractToDirectory(zipPath, pathDirectory);
        }
    }

}
