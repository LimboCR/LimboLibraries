using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Limbo.Utils.Collections.Generic
{
    /// <summary>
    /// Observable List of Component T
    /// </summary>
    /// <typeparam name="T">Component of the List</typeparam>
    [Serializable]
    public class ObservableList<T> : IList<T>
    {
        private readonly List<T> internalList = new();
        private readonly List<T> garbageCollector = new();

        public event Action<T> OnItemAdded;
        public event Action<T> OnItemRemoved;
        public event Action OnListChanged;

        public bool ActOnChange = true;
        public bool UseGarbageCollecting = false;
        public bool IsEnumerating { get; private set; } = false;

        public T this[int index]
        {
            get => internalList[index];
            set
            {
                internalList[index] = value;
                if (ActOnChange) OnListChanged?.Invoke();
            }
        }

        public int Count => internalList.Count;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            internalList.Add(item);
            if (ActOnChange)
            {
                OnItemAdded?.Invoke(item);
                OnListChanged?.Invoke();
            }
        }

        public bool Remove(T item)
        {
            if (UseGarbageCollecting && IsEnumerating)
            {
                garbageCollector.Add(item);
                return false; // Not removed yet
            }

            bool removed = internalList.Remove(item);
            if (removed && ActOnChange)
            {
                OnItemRemoved?.Invoke(item);
                OnListChanged?.Invoke();
            }
            return removed;
        }

        public void Clear()
        {
            if (ActOnChange)
            {
                foreach (var item in internalList)
                    OnItemRemoved?.Invoke(item);
            }

            internalList.Clear();
            if (ActOnChange)
                OnListChanged?.Invoke();
        }

        public bool Contains(T item) => internalList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => internalList.CopyTo(array, arrayIndex);
        public int IndexOf(T item) => internalList.IndexOf(item);

        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);
            if (ActOnChange)
            {
                OnItemAdded?.Invoke(item);
                OnListChanged?.Invoke();
            }
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < internalList.Count)
            {
                T item = internalList[index];
                internalList.RemoveAt(index);
                if (ActOnChange)
                {
                    OnItemRemoved?.Invoke(item);
                    OnListChanged?.Invoke();
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ObservableEnumerator(this, internalList.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class ObservableEnumerator : IEnumerator<T>
        {
            private readonly ObservableList<T> parent;
            private readonly IEnumerator<T> enumerator;

            public ObservableEnumerator(ObservableList<T> parent, IEnumerator<T> enumerator)
            {
                this.parent = parent;
                this.enumerator = enumerator;
                parent.IsEnumerating = true;
            }

            public T Current => enumerator.Current;
            object IEnumerator.Current => Current;

            public bool MoveNext() => enumerator.MoveNext();

            public void Reset() => enumerator.Reset();

            public void Dispose()
            {
                enumerator.Dispose();
                parent.IsEnumerating = false;

                // Garbage Collection Phase
                if (parent.UseGarbageCollecting && parent.garbageCollector.Count > 0)
                {
                    foreach (var item in parent.garbageCollector)
                    {
                        // Force remove without putting into garbage again
                        bool removed = parent.internalList.Remove(item);
                        if (removed && parent.ActOnChange)
                        {
                            parent.OnItemRemoved?.Invoke(item);
                            parent.OnListChanged?.Invoke();
                        }
                    }
                    parent.garbageCollector.Clear();
                }
            }
        }
    }


    public IEnumerator<T> GetEnumerator()
        {
            return new ObservableEnumerator(this, internalList.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class ObservableEnumerator : IEnumerator<T>
        {
            private readonly ObservableList<T> parent;
            private readonly IEnumerator<T> enumerator;

            public ObservableEnumerator(ObservableList<T> parent, IEnumerator<T> enumerator)
            {
                this.parent = parent;
                this.enumerator = enumerator;
                parent.IsEnumerating = true;
            }

            public T Current => enumerator.Current;
            object IEnumerator.Current => Current;

            public bool MoveNext() => enumerator.MoveNext();

            public void Reset() => enumerator.Reset();

            public void Dispose()
            {
                parent.IsEnumerating = false;
                enumerator.Dispose();
            }
        }
    }

    public static class ArrayUtils
    {
        public static void ListCleaner<T>(List<T> list)
        {
            list.RemoveAll(item => item == null);
        }
    }

    public static class ListExtensions
    {
        public static void Cleaner<T>(this List<T> list)
        {
            list.RemoveAll(item => item == null);
        }

        public static void ComponentCleaner<T>(List<T> list) where T : Component
        {
            list.RemoveAll(item => item == null);
        }

        public static void ReplaceNullsWith<T>(this List<T> list, T fallbackValue)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    list[i] = fallbackValue;
            }
        }
    }
}