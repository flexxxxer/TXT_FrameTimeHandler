using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing
{
    public class Maybe<T>
    {
        private readonly T _value;

        private Maybe() => this.HasValue = false;

        public Maybe(T someValue)
        {
            if (typeof(T).IsClass)
                if (someValue == null)
                    throw new ArgumentException();

            this.HasValue = true;
            this._value = someValue;
        }

        public bool HasValue { get; }

        public T Value
            => this.HasValue ? this._value : throw new InvalidOperationException();

        public static Maybe<T> None => new Maybe<T>();
    }
}
