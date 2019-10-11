using System;

namespace SearchPeople.Utils
{
    public interface IMonkeyManager
    {
        /// <summary>
        /// Clean all keys in monkey
        /// </summary>
        void EmptyAllMonkeys();
        /// <summary>
        /// Save a json object or item temporarily for a day
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">Object to save</param>
        /// <param name="nameMonkey">Name with which the object will be saved</param>
        void SaveMokey<T>(T element, string nameMonkey);

        /// <summary>
        /// Save a json object or item temporarily for a day
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">Object to save</param>
        /// <param name="expireIn">Time the saved object will last</param>
        /// <param name="nameMonkey">Name with which the object will be saved</param>
        void SaveMokey<T>(T element, string nameMonkey, TimeSpan expireIn);

        /// <summary>
        /// Find element in file store
        /// </summary>
        /// <typeparam name="T">type element savind in file store</typeparam>
        /// <param name="nameMonkey">name with the save element in file store</param>
        /// <returns></returns>
        ConfigurationItem<T> GetMonkey<T>(string nameMonkey);
    }
}