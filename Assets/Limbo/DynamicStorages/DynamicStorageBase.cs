using System.Collections.Generic;
using UnityEngine;

namespace Limbo.DynamicStorages
{
    public abstract class DynamicStorageBase<T> : MonoBehaviour where T : StorageItemBase
    {
        #region Variables
        [SerializeField] protected List<T> ItemsList = new();
        protected Dictionary<string, T> KeyItems = new();
        protected Dictionary<int, T> IdItems = new();

        #endregion

        #region Dictionary Logics
        public virtual void PopulateKeyDictionary()
        {
            KeyItems.Clear();

            foreach (var item in ItemsList)
            {
                if(item.Key != null && item.Key != "")
                    KeyItems[item.Key] = item;
            }
        }

        public virtual void PopulateIdDictionary()
        {
            IdItems.Clear();

            foreach (var item in ItemsList)
                IdItems[item.ID] = item;
        }

        public void PopulateDictionaries()
        {
            KeyItems.Clear();
            IdItems.Clear();

            foreach (var item in ItemsList)
            {
                if (!string.IsNullOrEmpty(item.Key))
                    KeyItems[item.Key] = item;

                IdItems[item.ID] = item;
            }
        }

        public virtual void ClearDictionary(EStorageType type)
        {
            switch (type)
            {
                case EStorageType.None:
                    break;
                case EStorageType.All:
                    KeyItems.Clear();
                    IdItems.Clear();
                    break;
                case EStorageType.KeyBased:
                    KeyItems.Clear();
                    break;
                case EStorageType.IdBased:
                    IdItems.Clear();
                    break;
                default: break;
            }
        }

        /// <summary>
        /// <u><b>[Safe]</b></u> Adds item to dictionary or all dictionaries with instructions towards what to do if key already exists
        /// </summary>
        /// <param name="addMode">If key exists Replace/Skip</param>
        /// <param name="type">Storage type</param>
        /// <param name="item">Item to add</param>
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

            if (addMode == EAddMode.Replace || !KeyItems.ContainsKey(item.Key))
                KeyItems[item.Key] = item;
        }

        private void TryAddIdItem(EAddMode addMode, T item)
        {
            if (addMode == EAddMode.Replace || !IdItems.ContainsKey(item.ID))
                IdItems[item.ID] = item;
        }

        #endregion

        #region Properties
        public bool KeyExists(string key) => KeyItems.ContainsKey(key);
        public bool IdExists(int id) => IdItems.ContainsKey(id);
        public T GetByKey(string key) => KeyItems.TryGetValue(key, out var item) ? item : null;
        public T GetByID(int id) => IdItems.TryGetValue(id, out var item) ? item : null;

        #endregion
    }

    #region Enums
    public enum EStorageType
    {
        None,
        All,
        KeyBased,
        IdBased
    }

    public enum EAddMode
    {
        Replace,
        Skip
    }

    #endregion
}