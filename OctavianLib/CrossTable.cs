using System.Collections.Generic;

namespace OctavianLib
{
    public class CrossTable<T, U> : IDictionary<T, U>
    {
        private Dictionary<T, U> _TableAlpha = new Dictionary<T, U>();
        private Dictionary<U, T> _TableBeta = new Dictionary<U, T>();

        public int Count => this._TableAlpha.Count;

        public U FindValue(T t) { return this._TableAlpha[t]; }
        public T FindKey(U u) { return this._TableBeta[u]; }

        public U this[T idx] {
            get => this._TableAlpha[idx];
            set {
                var _val = this._TableAlpha[idx];
                this._TableBeta.Remove(_val);
                this._TableAlpha[idx] = value;
                this._TableBeta.Add(value, idx);
            }
        }

        public T this[U idx] {
            get => this._TableBeta[idx];
            set {
                var _val = this._TableBeta[idx];
                this._TableAlpha.Remove(_val);
                this._TableBeta[idx] = value;
                this._TableAlpha.Add(value, idx);
            }
        }

        public ICollection<T> Keys => this._TableAlpha.Keys;
        public ICollection<U> Values => this._TableAlpha.Values;

        public void Add(T t, U u)
        {
            this._TableAlpha.Add(t, u);
            this._TableBeta.Add(u, t);
        }

        public void Add(KeyValuePair<T, U> _pair)
        {
            this._TableAlpha.Add(_pair.Key, _pair.Value);
            this._TableBeta.Add(_pair.Value, _pair.Key);
        }

        public bool Remove(KeyValuePair<T, U> pair)
        {
            this._TableAlpha.Remove(pair.Key);
            this._TableBeta.Remove(pair.Value);
            return true;
        }

        public void Clear()
        {
            this._TableAlpha.Clear();
            this._TableBeta.Clear();
        }

        public bool ContainsKey(T t)
        {
            return this._TableAlpha.ContainsKey(t);
        }

        public bool Contains(KeyValuePair<T, U> pair)
        {
            return this._TableAlpha.ContainsKey(pair.Key);
        }

        public bool Remove(T t)
        {
            U _val = this._TableAlpha[t];
            this._TableBeta.Remove(_val);
            this._TableAlpha.Remove(t);
            return true;
        }

        public bool TryGetValue(T t, out U u)
        {
            u = this._TableAlpha[t];
            return true;
        }

        public IEnumerator<KeyValuePair<T, U>> GetEnumerator() {
            return this._TableAlpha.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        // TODO IMPLEMENT?

        public void CopyTo(KeyValuePair<T, U>[] array, int i) { }
        public bool IsReadOnly => false;
    }
}