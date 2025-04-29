using Limbo.DynamicStorages;
using UnityEngine;

public class TestItemsStorage : DynamicStorageBase<TestItem>
{
    private void Awake()
    {
        PopulateDictionaries();
    }
}