using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Reader
{
    public interface IReader
    {
        /// <summary>
        /// allows to read a file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns>list of each object contains in file</returns>
        List<Serializable> read<T>(string fileName) where T: Serializable;

        /// <summary>
        /// allows to read a file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns>list of each line in file with each element in the line</returns>
        List<StringList> read(string fileName);

        /// <summary>
        /// allows to read a file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns>list of each line in file</returns>
        StringList readLine(string fileName);
    }
}
