﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DBFilesClient.NET;
using WoWHeadParser.Properties;

namespace WoWHeadParser.DBFileStorage
{
    public static class DBFileLoader
    {
        private static List<IDBFileLoader> _loaders = new List<IDBFileLoader>(4);

        [AutoInitial(Order = 1)]
        private static void Initial()
        {
            _loaders.Clear();

            Type[] types = Assembly.GetCallingAssembly().GetTypes();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < types.Length; ++i)
            {
                Type type = types[i];
                try
                {
                    if (type.GetInterface(typeof(IDBFileLoader).Name) == null)
                        continue;

                    IDBFileLoader loader = Activator.CreateInstance(type) as IDBFileLoader;
                    if (loader == null)
                        throw new InvalidCastException("IDBFileLoader");

                    loader.Load();
                    _loaders.Add(loader);
                }
                catch(Exception e)
                {
                    Console.WriteLine(Resources.Error_while_loading_db_file, type.Name);
                    Console.WriteLine(e);
                    continue;
                }
            }
            sw.Stop();
            Console.WriteLine(Resources.Loaded_count_db_files, _loaders.Count, sw.ElapsedMilliseconds);
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

            Console.WriteLine(Resources.Error_unsupported_db_file_loader, typeof(T).Name);
            return null;
        }

        #region Reading dbX files

        public const string DbFilePath = "db2";

        public static DBCStorage<T> Load<T>(bool dbc) where T : class, new()
        {
            string fileName = string.Format("{0}.{1}", typeof(T).Name.Replace("Entry", ""), dbc ? "dbc" : "db2");
            string path = Path.Combine(DbFilePath, fileName);
            if (!File.Exists(path))
                throw new FileNotFoundException("File " + path + " not found!");

            DBCStorage<T> storage = new DBCStorage<T>();
            if (!dbc)
                storage = new DB2Storage<T>();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                storage.Load(stream);
            }

            return storage;
        }
        #endregion
    }
}
