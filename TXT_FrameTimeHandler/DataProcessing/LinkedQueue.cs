using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing
{
    public class LinkedQueue<TypeValue> : IEnumerable<TypeValue>
    {
        private readonly LinkedList<TypeValue> _items = new LinkedList<TypeValue>();

        public LinkedQueue() { }

        public void Enqueue(TypeValue item)
            => this._items.AddLast(item);

        public Maybe<TypeValue> Dequeue()
        {
            if (this._items.Count == 0)
                return Maybe<TypeValue>.None;

            TypeValue value = this._items.First.Value;

            this._items.RemoveFirst();

            return new Maybe<TypeValue>(value);
        }

        public Maybe<TypeValue> Peek() 
            => this._items.Count == 0 ? Maybe<TypeValue>.None : new Maybe<TypeValue>(this._items.First.Value);

        public int Count => this._items.Count;

        public IEnumerator<TypeValue> GetEnumerator()
            => this._items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._items.GetEnumerator();

    }
}
