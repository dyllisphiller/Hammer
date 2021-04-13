using System;
using System.Collections.Generic;
using System.Linq;

namespace Hammer.Core.Helpers
{
    public static class ListExtensions
    {
        public static void Enqueue<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
        }

        public static void Dequeue<T>(this List<T> list)
        {
            list.Remove(list.Last());
        }

        public static void Dequeue<T>(this List<T> list, T item)
        {
            list.Remove(item);
        }

        public static void Bump<T>(this List<T> list, T item)
        {
            if (list.Contains(item)) list.Dequeue(item);
            list.Enqueue(item);
        }
    }

    public class MostRecentlyUsedList<T>
    {
        private readonly int _capacity;
        private readonly List<T> _mru;

        public MostRecentlyUsedList(int capacity)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity of MRU list cannot be less than 1!");
            _capacity = capacity;
            _mru = new List<T>();
        }

        public MostRecentlyUsedList(int capacity, IEnumerable<T> initialItems)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity of MRU list cannot be less than 1!");
            _capacity = capacity;
            _mru = new List<T>();

            _mru.AddRange(initialItems);
        }

        public int Count => _mru.Count;

        public void MarkAsRecentlyUsed(T item)
        {
            _mru.Bump(item);

            while (_mru.Count > _capacity)
            {
                _mru.Dequeue();
            }
        }

        public void Clear()
        {
            _mru.Clear();
        }

        public IList<T> GetList() => _mru;
    }
}
