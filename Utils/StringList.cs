using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utils
{
    public class StringList : List<string>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public StringList()
            : base()
        {

        }

        /// <summary>
        /// initialize StringList from other Ienumerable
        /// </summary>
        /// <param name="collection"></param>
        public StringList(IEnumerable<string> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// check if parameter T has method ToString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        private string CheckIfGenericObjectHasToStringMethod<T>(T item)
        {
            string hasToString = string.Empty;
            try
            {
                MethodInfo m = item.GetType().GetMethod("ToString", Type.EmptyTypes, null);
                if (m == null)
                {
                    throw new ArgumentNullException(string.Format("{0}.ToString method not found", item.GetType().Name));
                }
                else
                {
                    hasToString = item.ToString();
                }

            }
            catch (AmbiguousMatchException)
            {
                throw new AmbiguousMatchException(string.Format("{0}.{1} has multiple public overloads.",
                           item.GetType().Name, "ToString"));
            }
            return hasToString;
        }

        /// <summary>
        /// convert item T to string and get index of this string in stringlist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf<T>(T item)
        {
            return base.IndexOf(CheckIfGenericObjectHasToStringMethod(item));
        }

        /// <summary>
        /// convert item T to string and insert this string in stringlist at position = index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert<T>(int index, T item)
        {
            base.Insert(index, CheckIfGenericObjectHasToStringMethod(item));
        }

        /// <summary>
        /// convert item T to string and add this string in stringlist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void Add<T>(T item)
        {
            base.Add(CheckIfGenericObjectHasToStringMethod(item));
        }
        /// <summary>
        /// convert item T to string and check if stringlist contains this string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains<T>(T item)
        {
            return base.Contains(CheckIfGenericObjectHasToStringMethod(item));

        }
        /// <summary>
        /// convert item T to string and remove this in stringlist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove<T>(T item)
        {
            return base.Remove(CheckIfGenericObjectHasToStringMethod(item));
        }
        /// <summary>
        ///add a collection of generic objets
        ///the generic object will be translate in string object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        public void AddRange<T>(IEnumerable<T> collection)
        {
            InsertRange(Count, collection);
        }
        /// <summary>
        /// add a stringlist at the end of current list
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(StringList collection)
        {
            InsertRange(Count, collection);
        }
        /// <summary>
        /// insert a stringlist at specific position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        public void InsertRange(int index, StringList collection)
        {
            if (collection != null)
            {
                using (IEnumerator<string> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
        }

        /// <summary>
        ///  insert a collection at specific position
        ///  the generic object will be translate in string object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        public void InsertRange<T>(int index, IEnumerable<T> collection)
        {
            if (collection != null)
            {
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
        }

        /// <summary>
        /// translate list to string with each item separate by 'separator' parameter
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="endSeparator">allows to know if there is a separator at the end of string</param>
        /// <returns></returns>
        public string Join(string separator, bool endSeparator = false)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                sb.Append(this[i]);
                if (i < this.Count - 1)
                {
                    sb.Append(separator);
                }
                else if (endSeparator && i == this.Count - 1)
                {
                    sb.Append(separator);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// removes duplicates string
        /// </summary>
        /// <returns>the stringlist with duplicates string remove</returns>
        public StringList RemoveDuplicates() => new StringList(this.Distinct());

        /// <summary>
        /// filter list depends of predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>the stringlist filtered</returns>
        public StringList Filter(Func<string, bool> predicate) => new StringList(this.Where(predicate));

        /// <summary>
        /// update string at index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="update"></param>
        public void Update(int index,string update)
        {
            this[index] = update;
        }
        /// <summary>
        /// add at the start of all string a specific string
        /// </summary>
        /// <param name="startString"></param>
        public void AddStringToStartForAll(string startString)
        {
            for(int i= 0;i < this.Count;i++)
            {
                Update(i, startString + this[i]);
            }
        }
        /// <summary>
        /// add at the end of all string a specific string
        /// </summary>
        /// <param name="endString"></param>
        public void AddStringToEndForAll(string endString)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Update(i,  this[i] + endString);
            }
        }

        /// <summary>
        /// if list contains number, SUM all numbers of list
        /// </summary>
        public int Sum()
        {
            int sum = 0;
            if (IsListOfNumbers())
            {
                this.ForEach(s => sum += s.ToNumber());
            }
            else
            {
                throw new InvalidOperationException("List contains something else that numbers");
            }
            return sum;
        }


        /// <summary>
        /// if list contains number, SUM all numbers of list
        /// </summary>
        public bool IsListOfNumbers()
        {
            bool isNumber = true;
            foreach (string number in this)
            {

                isNumber = number.IsNumber();
                if (!isNumber)
                {
                    break;
                }
            }
            return isNumber;
        }

        /// <summary>
        /// allows encrypt all elements in the list
        /// </summary>
        /// <returns></returns>
        public StringList Encrypt()
        {
            return new StringList(this.Select(s => { s = s.Encrypt(); return s; }).ToList());
        }

        /// <summary>
        /// get all elements which match with the pattern string
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public StringList PatternMatching(string pattern)
        {
            return new StringList(this.Where(s => s.IsMatch(pattern) == true ).ToList());
        }

        /// <summary>
        /// get all elements which match with the pattern regex
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public StringList PatternMatching(Regex reg)
        {
            return new StringList(this.Where(s => s.IsMatchRegex(reg)).ToList());
        }

        /// <summary>
        /// get all elements which match with the pattern regex
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public StringList PatternMatchingRegexString(string regexString)
        {
            return new StringList(this.Where(s => s.IsMatchRegexString(regexString)).ToList());
        }



    }
}
