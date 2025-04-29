using System;
using System.Collections.Generic;
using UnityEngine;

namespace Limbo.DynamicStorages
{
    [Serializable]
    public class DynamicStorage<T> where T : StorageItemBase
    {
        #region Variables
        [SerializeField] private List<T> itemsList = new();
        private Dictionary<string, T> keyItems = new();
        private Dictionary<int, T> idItems = new();
        #endregion

        #region Variables Properties
        public List<T> ItemsList => itemsList;
        public Dictionary<string, T> KeyItems => keyItems;
        public Dictionary<int, T> IdItems => idItems;
        #endregion

        #region Dictionary Logics
        public void PopulateDictionaries()
        {
            keyItems.Clear();
            idItems.Clear();

            foreach (var item in itemsList)
            {
                if (!string.IsNullOrEmpty(item.Key))
                    keyItems[item.Key] = item;

                idItems[item.ID] = item;
            }
        }

        public void ClearDictionary(EStorageType type)
        {
            switch (type)
            {
                case EStorageType.All:
                    keyItems.Clear();
                    idItems.Clear();
                    break;
                case EStorageType.KeyBased:
                    keyItems.Clear();
                    break;
                case EStorageType.IdBased:
                    idItems.Clear();
                    break;
            }
        }

        public void AddItem(EAddMode addMode, EStorageType type, T item)
        {
            switch (type)
            {
                case EStorageType.All:
                    TryAddKeyItem(addMode, item);
                    TryAddIdItem(addMode, item);
                    break;
                case EStorageType.KeyBased:
                    TryAddKeyItem(addMode, item);
                    break;
                case EStorageType.IdBased:
                    TryAddIdItem(addMode, item);
                    break;
            }
        }

        private void TryAddKeyItem(EAddMode addMode, T item)
        {
            if (string.IsNullOrEmpty(item.Key)) return;

            if (addMode == EAddMode.Replace || !keyItems.ContainsKey(item.Key))
                keyItems[item.Key] = item;
        }

        private void TryAddIdItem(EAddMode addMode, T item)
        {
            if (addMode == EAddMode.Replace || !idItems.ContainsKey(item.ID))
                idItems[item.ID] = item;
        }

        #endregion

        #region Properties
        public bool KeyExists(string key) => keyItems.ContainsKey(key);
        public bool IdExists(int id) => idItems.ContainsKey(id);
        public T GetByKey(string key) => keyItems.TryGetValue(key, out var item) ? item : null;
        public T GetByID(int id) => idItems.TryGetValue(id, out var item) ? item : null;

        #endregion
    }
}