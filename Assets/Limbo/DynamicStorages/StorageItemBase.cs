/*
 * Base script for the items of DynamicStorage.
 * Just inherit it, and add whatever is required.
 */

namespace Limbo.DynamicStorages
{
    [System.Serializable]
    public abstract class StorageItemBase
    {
        public string Key;
        public int ID;
    }
}