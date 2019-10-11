using MonkeyCache;
using MonkeyCache.FileStore;
using System;

namespace SearchPeople.Utils
{
    public class ConfigurationItem<T>
    {
        public T Item { get; set; }
        public bool IsExpired { get; internal set; }
    }
    public class MonkeyManager : IMonkeyManager
    {
        IBarrel file;
        public MonkeyManager()
        {
            Barrel.ApplicationId = Constants.BARREL_ID;
            file = Barrel.Current;
        }

        public void EmptyAllMonkeys()
        {
            file.EmptyAll();
        }
        /// <summary>
        /// Save a json object or item temporarily for a day
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">Object to save</param>
        /// <param name="nameMonkey">Name with which the object will be saved</param>
        public void SaveMokey<T>(T element, string nameMonkey)
        {
            
            var expireIn = TimeSpan.FromDays(1);
            SaveMokey(element, nameMonkey, expireIn);
        }

        /// <summary>
        /// Save a json object or item temporarily for a day
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">Object to save</param>
        /// <param name="expireIn">Time the saved object will last</param>
        /// <param name="nameMonkey">Name with which the object will be saved</param>
        public void SaveMokey<T>(T element, string nameMonkey, TimeSpan expireIn)
        {
            file.Add<T>(nameMonkey, element, expireIn);
        }

        /// <summary>
        /// Find element in file store
        /// </summary>
        /// <typeparam name="T">type element savind in file store</typeparam>
        /// <param name="nameMonkey">name with the save element in file store</param>
        /// <returns></returns>
        public ConfigurationItem<T> GetMonkey<T>(string nameMonkey)
        {
            if (file.Exists(nameMonkey))
            {
                return new ConfigurationItem<T>()
                {
                    IsExpired = file.IsExpired(nameMonkey),
                    Item = file.Get<T>(nameMonkey)
                };
            }

            return null;
        }
    }
}
