using System.IO;

namespace Utils.ReadWrite.Reader
{
    public static class FileReader
    {
        /// <summary>
        /// read content of file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// read lines of file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static StringList ReadLines(string path)
        {
            return new StringList(File.ReadAllLines(path));
        }
    }
}
