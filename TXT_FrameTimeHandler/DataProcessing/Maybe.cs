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

        private Maybe() { }

        public Maybe(T someValue)
        {
            if (typeof(T).IsClass)
                if (someValue == null)
                    throw new ArgumentException();

            this._value = someValue;
        }

        public bool HasValue 
            => typeof(T).IsClass ? this._value != null : true;

        public T Value
        {
            get
            {
                if (typeof(T).IsClass)
                    if (this._value == null)
                        throw new Exception();

                return this._value;
            }
        }

        public Maybe<U> Bind<U>(Func<T, Maybe<U>> func)
        {
            return this._value != null ? func(this._value) : Maybe<U>.None;
        }

        public static Maybe<T> None { get; } = new Maybe<T>();
    }
}
