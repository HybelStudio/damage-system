using System;
using System.Collections.Generic;

namespace Hybel.DamageSystem
{
    /// <summary>
    /// An <see cref="Option{T}"/> helps with representing an non-value without creating errors with null values such as the dreaded <see cref="NullReferenceException"/>.
    /// </summary>
    /// <typeparam name="T">The type of value contained in the <see cref="Option{T}"/>.</typeparam>
    internal readonly struct Option<T> : IEquatable<Option<T>>
    {
        /// <summary>
        /// Value which represents nothing. (kinda like null but without all the annoying errors)
        /// </summary>
        public static Option<T> Nothing = new Option<T>();

        private readonly T _value;
        private readonly bool _hasValue;

        /// <summary>
        /// Create a new Option which contains <paramref name="value"/>.
        /// </summary>
        public Option(T value)
        {
            _value = value;
            _hasValue = value != null;
        }

        /// <summary>
        /// The absolute value contained in the <see cref="Option{T}"/>. <b>This can be null!</b>
        /// </summary>
        internal T DangerousValue => _value;

        /// <summary>
        /// Whether or not the <see cref="Option{T}"/> has a value.
        /// </summary>
        public bool HasValue => _hasValue;

        public bool Equals(Option<T> other) => EqualityComparer<T>.Default.Equals(_value, other._value) && _hasValue == other._hasValue;
        public override bool Equals(object obj) => obj is Option<T> option && Equals(option);

        public override int GetHashCode()
        {
            var hashCode = 682175446;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(_value);
            hashCode = hashCode * -1521134295 + _hasValue.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{(this == Nothing ? $"{nameof(Option<T>)}<{typeof(T).Name}>.{nameof(Nothing)}" : _value.ToString())}";

        public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
        public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

        public static implicit operator Option<T>(T value)
        {
            if (value is null)
                return Nothing;

            return value.Some();
        }
    }

    internal static class OptionExtensions
    {
        /// <summary>
        /// Convert from any type to an option of that type.
        /// </summary>
        /// <returns>An <see cref="Option{T}"/> which has a <paramref name="value"/> within it OR if <paramref name="value"/> is null, an <see cref="Option{T}"/> which does not have a <paramref name="value"/> within it.</returns>
        public static Option<T> Some<T>(this T value) => new Option<T>(value);

        /// <summary>
        /// Convert <paramref name="value"/> to an option with that value and <u>safely</u> run any <paramref name="function"/> on it.
        /// </summary>
        /// <typeparam name="T">Type of value being converted to an <see cref="Option{T}"/> and then having a <paramref name="function"/> run on it.</typeparam>
        /// <typeparam name="TReturn">Type of value the run returns wrapped in a new <see cref="Option{T}"/>.</typeparam>
        /// <param name="value">Value being converted to and <paramref name="value"/> and then having a <paramref name="function"/> run on it.</param>
        /// <param name="function">Function to run on the <paramref name="value"/>.</param>
        /// <returns>An <see cref="Option{T}"/> of the <paramref name="function"/>s returned value.</returns>
        public static Option<TReturn> Run<T, TReturn>(this T value, Func<T, Option<TReturn>> function) =>
            value.Some().Run(function);

        /// <summary>
        /// <u>Safely</u> run any <paramref name="function"/> on the value contained within <paramref name="option"/>.
        /// </summary>
        /// <typeparam name="T">Type of value having a <paramref name="function"/> run on it.</typeparam>
        /// <typeparam name="TResult">Type of value the run returns wrapped in a new <see cref="Option{T}"/>.</typeparam>
        /// <param name="option"><see cref="Option{T}"/> which contains a value to run the <paramref name="function"/> on.</param>
        /// <param name="function">Function to run on the value contained on <paramref name="option"/>.</param>
        /// <returns>An <see cref="Option{T}"/> of the <paramref name="function"/>s returned value.</returns>
        public static Option<TResult> Run<T, TResult>(this Option<T> option, Func<T, Option<TResult>> function)
        {
            if (function is null || option == Option<T>.Nothing)
                return Option<TResult>.Nothing;

            return function(option.DangerousValue);
        }

        /// <summary>
        /// Match the <paramref name="option"/> to have a value and retrieve it.
        /// </summary>
        /// <typeparam name="T">Type of the value contained in <paramref name="option"/>.</typeparam>
        /// <param name="option"><see cref="Option{T}"/> which you expect to have a value.</param>
        /// <param name="value">
        /// The retrieved value from <paramref name="option"/>.
        /// <para><b>This can be null!</b> Use the returned bool to branch based on if <paramref name="value"/> is null or not.</para>
        /// </param>
        /// <returns>True if <paramref name="option"/> has a value. False if <paramref name="option"/> does not have a value.</returns>
        public static bool TryUnwrap<T>(this Option<T> option, out T value)
        {
            value = option.DangerousValue;
            return option.HasValue;
        }

        /// <summary>
        /// Expect the <paramref name="option"/> to have a value and retrieve it.
        /// </summary>
        /// <typeparam name="T">Type of the value contained in <paramref name="option"/>.</typeparam>
        /// <param name="option"><see cref="Option{T}"/> which you expect to have a value.</param>
        /// <param name="value">
        /// The retrieved value from <paramref name="option"/>.
        /// <para><b>This can be null!</b> Use the returned bool to branch based on if <paramref name="value"/> is null or not.</para>
        /// </param>
        /// <param name="onError">Action to execute if the <paramref name="option"/> has no value.</param>
        /// <returns>True if <paramref name="option"/> has a value. False if <paramref name="option"/> has no value.</returns>
        public static bool TryUnwrap<T>(this Option<T> option, out T value, Action onError)
        {
            if (!option.HasValue)
                onError?.Invoke();

            value = option.DangerousValue;
            return option.HasValue;
        }

        /// <summary>
        /// Get the value contained in <paramref name="option"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exception is thrown if the <paramref name="option"/> has no value.</exception>
        /// <returns>The value of <paramref name="option"/> or null.</returns>
        public static T Unwrap<T>(this Option<T> option)
        {
            if (!option.HasValue)
                throw new InvalidOperationException($"The Option of type {typeof(T)} did not have a value when trying to access it.");

            return option.DangerousValue;
        }

        /// <summary>
        /// Yields one value if <paramref name="option"/> has value, otherwise none.
        /// </summary>
        public static IEnumerable<T> Iterator<T>(this Option<T> option)
        {
            if (option.TryUnwrap(out var value))
                yield return value;
        }
    }
}
