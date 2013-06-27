using System;

namespace SeekYouRS.Utilities
{
    public delegate bool TryFunc<in tIn, tOut>(tIn input, out tOut output);

    public static class Option
    {
        public static Option<tType> None<tType>()
        {
            return Option<tType>.None;
        }

        public static Option<tType> Some<tType>(this tType value)
        {
            return Option<tType>.Some(value);
        }

        public static tRes Maybe<tType, tRes>(this Option<tType> option, tRes result, Func<tType, tRes> map)
        {
            if (option.IsNone) return result;
            return map(option.Value);
        }

        public static tValue DefaultsTo<tValue>(this Option<tValue> option, tValue defValue = default(tValue))
        {
            return option.Maybe(defValue, x => x);
        }

        public static Option<tResult> Map<tInput, tResult>(this Option<tInput> option,
                                                   Func<tInput, tResult> map)
        {
            return option.Maybe(Option<tResult>.None, x => Some(map(x)));
        }

        public static Option<tResult> Bind<tInput, tResult>(this Option<tInput> option,
                                                   Func<tInput, Option<tResult>> map)
        {
            return option.Maybe(Option.None<tResult>(), map);
        }

        public static Option<tResult> BindNullable<tInput, tResult>(this Option<tInput> option, Func<tInput, tResult?> map)
            where tResult : struct 
        {
            var res = option.IsSome ? map(option.Value) : null;
            return res.HasValue ? Option.Some(res.Value) : Option.None<tResult>();
        }

        public static void Do<tInput>(this Option<tInput> option, Action<tInput> action)
        {
            if (option.IsSome)
                action(option.Value);
        }

        public static Option<tOutput> Try<tInput, tOutput>(this TryFunc<tInput, tOutput> f, tInput input)
        {
            tOutput value;
            return !f(input, out value) ? Option<tOutput>.None : Option.Some(value);
        }

        public static Option<tValue> ToOption<tValue>(this tValue value)
        {
            return value.Some();
        }

        public static Option<tValue> ToOption<tValue>(this tValue? value)
            where tValue : struct
        {
            return value == null ? Option<tValue>.None : value.Value.Some();
        }

        public static Option<tB> SelectMany<tA, tB>(this Option<tA> m, Func<tA, Option<tB>> f)
        {
            return Bind(m, f);
        }

        public static Option<tC> SelectMany<tA, tB, tC>(this Option<tA> m, Func<tA, Option<tB>> f, Func<tA, tB, tC> select)
        {
            return m.Bind(x => f(x).Bind(y => select(x, y).ToOption()));
        }

        public static Option<tV> OrElse<tV>(this Option<tV> o, Option<tV> alt)
        {
            if (o.IsSome) return o;
            return alt;
        }

        public static Option<tV> OrElse<tV>(this Option<tV> o, Func<Option<tV>> altGen)
        {
            if (o.IsSome) return o;
            return altGen();
        }
    }

    public class Option<tType>
    {
        private readonly tType _value;
        private readonly bool _isSome;

        private Option(tType value)
        {
            _value = value;
            _isSome = true;
        }

        private Option()
        {
            _value = default(tType);
            _isSome = false;
        }

        public bool IsSome
        {
            get { return _isSome; }
        }

        public bool IsNone
        {
            get { return !IsSome; }
        }

        public tType Value
        {
            get
            {
                if (IsNone) throw new NotSupportedException("kein Wert angegeben");
                return _value;
            }
        }

        public static Option<tType> Some(tType value)
        {
            return new Option<tType>(value);
        }

        public static Option<tType> None
        {
            get { return new Option<tType>(); }
        }

        public override string  ToString()
        {
            return IsSome ? string.Format("Some {0}", _value) : "None";
        }

        public override int  GetHashCode()
        {
            return IsSome ? _value.GetHashCode() : 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Option<tType>)
            {
                var vgl = (Option<tType>) obj;
                return (_isSome == false && vgl.IsSome == false) || (_isSome && vgl.IsSome && Equals(Value, vgl.Value));
            }
            if (obj is tType)
            {
                var vgl = (tType) obj;
                return _isSome && Equals(_value, vgl);
            }
            return false;
        }

        public static bool operator ==(Option<tType> o1, Option<tType> o2  )
        {
            if (o1 == null) return o2 == null;
            return o1.Equals(o2);
        }

        public static bool operator ==(Option<tType> o1, tType o2  )
        {
            if (Equals(o1, null)) return Equals(o2, null);
            return o1.Equals(o2);
        }

        public static bool operator !=(Option<tType> o1, tType o2)
        {
            return !(o1 == o2);
        }

        public static bool operator !=(Option<tType> o1, Option<tType> o2)
        {
            return !(o1 == o2);
        }

        public static implicit operator Option<tType> (tType value)
        {
            return Some(value);
        }

    }
}