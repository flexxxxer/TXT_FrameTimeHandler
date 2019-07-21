﻿using System;
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
        }

        public bool HasValue
            => typeof(T).IsClass ? this._value == null : true;

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

        public static Maybe<T> None { get; } = new Maybe<T>();
    }
}
