using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DBFilesClient.NET;

namespace WoWHeadParser.DBFileStorage
{
    public static class DBFileLoader
    {
        private static List<IDBFileLoader> _loaders = new List<IDBFileLoader>(8);

        public static void Initial()
        {
            Type[] types = Assembly.GetCallingAssembly().GetTypes();
            for (int i = 0; i < types.Length; ++i)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Type type = types[i];
                try
                {
                    if (type.GetInterface(typeof(IDBFileLoader).Name) == null)
                        continue;

                    IDBFileLoader loader = Activator.CreateInstance(type) as IDBFileLoader;
                    if (loader == null)
                        throw new InvalidCastException();

                    loader.Load();
                    _loaders.Add(loader);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error while loading {0}! Skip", type.Name);
                    Console.WriteLine(e.Message);
                    continue;
                }
                sw.Stop();
                Console.WriteLine("Loaded {0} db files in {1} ms", _loaders.Count, sw.ElapsedMilliseconds);
            }
        }

        public static T GetLoader<T>() where T : class
        {
            for (int i = 0; i < _loaders.Count; ++i)
            {
                IDBFileLoader loader = _loaders[i];
                if (!(loader is T))
                    continue;

                return (T)loader;
            }
            return null;
        }

        #region Reading dbX files

        public const string DbFilePath = "db2";

        public static DBCStorage Load<T>(bool dbc) where T : struct
        {
            string fileName = string.Format("{0}.{1}", typeof(T).Name.Replace("Entry", ""), dbc ? "dbc" : "db2");
            string path = Path.Combine(DbFilePath, fileName);
            if (!File.Exists(path))
                throw new FileNotFoundException("File " + path + " not found!");

            IStructureFmt fmt = Activator.CreateInstance(typeof(T)) as IStructureFmt;
            if (fmt == null)
                throw new ArgumentNullException(typeof(T).Name + "is not part of IStructureFmt!");

            DBCStorage storage = new DBCStorage(fmt.Fmt);
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                storage.Load(stream, false, true);
            }

            return storage;
        }
        #endregion
    }
}
