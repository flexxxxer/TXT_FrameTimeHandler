using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing
{
    public class Maybe<T> where T : class
    {
        private readonly T _value;

        public Maybe() { }

        public Maybe(T someValue) => this._value = someValue;

        public Maybe<TO> Bind<TO>(Func<T, Maybe<TO>> func) where TO : class 
            => this._value != null ? func(this._value) : Maybe<TO>.None();

        public bool HasValue
            => this._value is null;

        public static Maybe<T> None() => new Maybe<T>();
    }
}
