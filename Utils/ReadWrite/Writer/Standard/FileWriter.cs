using System.IO;

namespace Utils.ReadWrite.Writer.Standard
{
    internal static class FileWriter
    {

        /// <summary>
        /// Write or replace text parameters in file 
        /// </summary>
        /// <param name="contentToWrite"></param>
        /// <param name="path"></param>
        /// <param name="removeOldText">Optional</param>
        public static void Write(string contentToWrite,string path)
        {
            File.WriteAllText(path, contentToWrite);
        }
        /// <summary>
        /// append text parameters in file 
        /// </summary>
        /// <param name="contentToWrite"></param>
        /// <param name="path"></param>
        public static void Append(string contentToWrite, string path)
        {
            if (File.Exists(path))
            {
                File.AppendAllText(path, contentToWrite);
            }
            else
            {
                throw new FileNotFoundException("File " + path + " not exist");
            }
        }
        

    }
}
