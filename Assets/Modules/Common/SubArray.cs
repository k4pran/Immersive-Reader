using System;
using System.Collections.Generic;

namespace Modules.Common {

    public class SubArray<T> {

        private readonly ArraySegment<T> segment;

        public SubArray(T[] array, int offset, int count) {
            segment = new ArraySegment<T>(array, offset, count);
        }

        public int Count => segment.Count;

        public T this[int index] => segment.Array[segment.Offset + index];

        public T[] ToArray() {
            var temp = new T[segment.Count];
            Array.Copy(segment.Array, segment.Offset, temp, 0, segment.Count);
            return temp;
        }

        public IEnumerator<T> GetEnumerator() {
            for (var i = segment.Offset; i < segment.Offset + segment.Count; i++) yield return segment.Array[i];
        }
    }
}