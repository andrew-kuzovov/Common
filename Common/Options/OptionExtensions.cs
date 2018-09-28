using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guards;

namespace Options
{
    public static class OptionExtensions
    {
        public static TResult Match<T, TResult>(this Option<T> source, Func<T, TResult> map, TResult missingValue)
        {
            Guard.CheckNotNull(map, nameof(map));

            return source.Match(map, () => missingValue);
        }

        public static Option<TOut> Bind<TIn, TOut>(this Option<TIn> source, Func<TIn, Option<TOut>> function)
        {
            Guard.CheckNotNull(function, nameof(function));

            return source.Match(function, () => Option.None());
        }

        public static Option<TOut> Select<TIn, TOut>(this Option<TIn> source, Func<TIn, TOut> function)
        {
            Guard.CheckNotNull(function, nameof(function));

            return source.Bind(value => function(value).MayBe());
        }

        public static Option<TOut> SelectMany<TIn, TIntermediate, TOut>(
            this Option<TIn> source,
            Func<TIn, Option<TIntermediate>> select,
            Func<TIn, TIntermediate, TOut> selectOut)
        {
            Guard.CheckNotNull(select, nameof(select));
            Guard.CheckNotNull(selectOut, nameof(selectOut));

            return source.Bind(s => select(s).Select(m => selectOut(s, m)));
        }

        public static Option<T> Where<T>(this Option<T> value, Func<T, bool> filter)
        {
            Guard.CheckNotNull(filter, nameof(filter));

            return value.Bind(e => filter(e) ? e.MayBe() : Option.None());
        }

        public static Option<TTo> Cast<TFrom, TTo>(this Option<TFrom> value)
        {
            return value.Bind(e => ((TTo) Convert.ChangeType(e, typeof (TTo))).MayBe());
        }

        public static Option<T> Unwrap<T>(this Option<Option<T>> value)
        {
            return value.Bind(e => e);
        }

        public static T GetOrDefault<T>(this Option<T> source, Func<T> defaultValue)
        {
            Guard.CheckNotNull(defaultValue, nameof(defaultValue));

            return source.Match(value => value, defaultValue);
        }

        public static T GetOrDefault<T>(this Option<T> source, T defaultValue)
        {
            return source.GetOrDefault(() => defaultValue);
        }

        public static T GetOrThrow<T>(this Option<T> source, Func<Exception> exception)
        {
            Guard.CheckNotNull(exception, nameof(exception));

            return source.Match(
                value => value,
                () => throw exception());
        }

        public static T GetOrThrow<T>(this Option<T> source, Exception exception)
        {
            Guard.CheckNotNull(exception, nameof(exception));

            return source.GetOrThrow(() => exception);
        }

        public static void Do<T>(this Option<T> source, Action<T> action)
        {
            Guard.CheckNotNull(source, nameof(source));

            source.Match(
                value =>
                {
                    action(value);
                    return VoidValue;
                },
                VoidValue);
        }

        public static void DoOrDefault<T>(this Option<T> source, Action<T> doIfSome, Action doIfNone)
        {
            Guard.CheckNotNull(doIfSome, nameof(doIfSome));
            Guard.CheckNotNull(doIfNone, nameof(doIfNone));

            source.Match(
                value =>
                {
                    doIfSome(value);
                    return VoidValue;
                },
                () =>
                {
                    doIfNone();
                    return VoidValue;
                });
        }

        public static Task DoOrDefaultAsync<T>(this Option<T> source, Func<T, Task> doIfSome, Func<Task> doIfNone)
        {
            Guard.CheckNotNull(doIfSome, nameof(doIfSome));
            Guard.CheckNotNull(doIfNone, nameof(doIfNone));

            return source.Match(doIfSome, doIfNone);
        }

        public static IEnumerable<T> TakeValues<T>(this IEnumerable<Option<T>> source)
        {
            Guard.CheckNotNull(source, nameof(source));

            return source
                .Where(item => item.IsSome)
                .Select(item => item.GetOrThrow(() => new InvalidOperationException("Item is none.")));
        }

        public static bool IsTrue<T>(this Option<T> source, Func<T, bool> filter)
        {
            Guard.CheckNotNull(filter, nameof(filter));

            return source.Select(filter).GetOrDefault(false);
        }

        public static T? ToNullable<T>(this Option<T> value) where T : struct
		{
			return value.Match(v => v, default(T?));
		}

		public static TOut? ToNullable<TIn, TOut>(this Option<TIn> value, Func<TIn, TOut> map) where TOut : struct
		{
			Guard.CheckNotNull(map, nameof(map));

			return value.Match(v => map(v), default(TOut?));
		}

		/// <summary>
		/// Gets the value or throws an <see cref="InvalidOperationException"/>.
		/// </summary>
		public static T Some<T>(this Option<T> option)
		{
			return option.GetOrThrow(() => new InvalidOperationException("Some() may not be called for Option.None()"));
		}

		/// <summary>
		/// Gets the value if there is some; returns <typeparamref name="T"/> CLR default value otherwise — a simple special case for 
		/// </summary>
		public static T GetOrDefault<T>(this Option<T> option)
		{
			return option.GetOrDefault(default(T));
		}

		public static T GetOrThrowError<T, TError>(this Option<T> option, Func<TError> errorFactory) where TError : Exception
		{
			Guard.CheckNotNull(errorFactory, nameof(errorFactory));

			return option.Match(value => value, () => throw errorFactory());
		}

		public static T GetOrThrowError<T, TError>(this Option<T> option, TError error) where TError : Exception
		{
			Guard.CheckNotNull(error, nameof(error));

			return option.Match(value => value, () => throw error);
		}

		public static void DoOrThrowError<T, TError>(this Option<T> option, Action<T> existingValueAction, Func<TError> errorFactory)
			where TError : Exception
		{
			Guard.CheckNotNull(existingValueAction, nameof(existingValueAction));
			Guard.CheckNotNull(errorFactory, nameof(errorFactory));

			existingValueAction(option.GetOrThrowError(errorFactory()));
		}

		public static void DoOrThrowError<T, TError>(this Option<T> option, Action<T> existingValueAction, TError error) where TError : Exception
		{
		    Guard.CheckNotNull(existingValueAction, nameof(existingValueAction));

			existingValueAction(option.GetOrThrowError(error));
		}

		/// <summary>
		/// Runs the <see cref="existingValueAction"/> if there is some value.
		/// </summary>
		public static void DoForExistingValue<T>(this Option<T> option, Action<T> existingValueAction)
		{
		    Guard.CheckNotNull(existingValueAction, nameof(existingValueAction));

			if (option.IsSome)
			{
				existingValueAction(option.GetOrDefault());
			}
		}

		/// <summary>
		/// Returns the <see cref="existingValueTask"/> if there is some value; otherwise, returns a completed task.
		/// </summary>
		public static Task DoForExistingValueAsync<T>(this Option<T> option, Func<T, Task> existingValueTask)
		{
			Guard.CheckNotNull(existingValueTask, nameof(existingValueTask));

			return option.IsSome ? existingValueTask(option.GetOrDefault()) : Task.FromResult(0);
		}

		/// <summary>
		/// Runs the <see cref="existingValueAction"/> if these is some value; otherwise, runs the <see cref="missingValueAction"/> if it is specified.
		/// </summary>
		/// <remarks>
		/// Optional missing value action is not supported because of conflicting overloads.
		/// </remarks>
		public static void Match<T>(this Option<T> option, Action<T> existingValueAction, Action missingValueAction)
		{
		    Guard.CheckNotNull(existingValueAction, nameof(existingValueAction));

			if (option.IsSome)
			{
				existingValueAction(option.GetOrDefault());
			}
			else
            {
                missingValueAction?.Invoke();
            }
        }

		public static Option<T> OrTry<T>(this Option<T> source, Func<Option<T>> orTryAction)
		{
		    return source.IsSome ? source : orTryAction();
		}

		public static Option<T> OrTry<T>(this Option<T> source, Option<T> orTryOption)
		{
		    return source.IsSome ? source : orTryOption;
		}

        public static async Task DoOrThrowErrorAsync<T, TError>(this Option<T> source, Func<T, Task> action, Func<TError> error)
			where TError : Exception
		{
		    Guard.CheckNotNull(action, nameof(action));
		    Guard.CheckNotNull(error, nameof(error));

			await source.Match(
				action,
				() => throw error());
		}

		public static Task DoOrThrowErrorAsync<T, TError>(this Option<T> source, Func<T, Task> action, TError error)
			where TError : Exception
		{
		    Guard.CheckNotNull(action, nameof(action));
		    Guard.CheckNotNull(error, nameof(error));

			return source.DoOrThrowErrorAsync(action, () => error);
		}

        public static Task<T> GetOrDefaultAsync<T>(this Option<T> source, Func<Task<T>> defaultValue)
        {
            Guard.CheckNotNull(defaultValue, nameof(defaultValue));

            return source.Match(Task.FromResult, defaultValue);
        }

        public static Task<T> GetOrDefaultAsync<T>(this Option<T> source, T defaultValue)
        {
            return source.Match(Task.FromResult, () => Task.FromResult(defaultValue));
        }

        public static Task DoAsync<T>(this Option<T> source, Func<T, Task> action)
        {
            return source.Match(action, () => Task.CompletedTask);
        }

        public static Task<Option<TOut>> BindAsync<TIn, TOut>(this Option<TIn> source, Func<TIn, Task<Option<TOut>>> function)
        {
            Guard.CheckNotNull(function, nameof(function));

            return source.Match(function, () => Task.FromResult<Option<TOut>>(Option.None()));
        }

        public static Task<Option<TOut>> SelectAsync<TIn, TOut>(this Option<TIn> source, Func<TIn, Task<TOut>> function)
        {
            Guard.CheckNotNull(function, nameof(function));

            return BindAsync(
                source,
                async value =>
                {
                    var result = await function(value);
                    return result.MayBe();
                });
        }

        public static async Task<Option<T>> WhereAsync<T>(this Option<T> source, Func<T, Task<bool>> predicate)
        {
            Guard.CheckNotNull(predicate, nameof(predicate));

            return await source.BindAsync(async e => await predicate(e) ? e.MayBe() : Option.None());
        }

        public static Task<Option<T>> OrTryAsync<T>(this Option<T> source, Func<Task<Option<T>>> alternative)
        {
            Guard.CheckNotNull(alternative, nameof(alternative));

            return source.IsSome ? Task.FromResult(source) : alternative();
        }

        public static Option<TOut> Combine<TIn1, TIn2, TOut>(
            this Option<TIn1> source,
            Option<TIn2> option,
            Func<TIn1, TIn2, TOut> select)
        {
            Guard.CheckNotNull(select, nameof(select));

            return source.SelectMany(s => option, select);
        }

        public static void DoIfNone<T>(this Option<T> source, Action doIfNone)
        {
            Guard.CheckNotNull(doIfNone, nameof(doIfNone));

            source.Match(
                _ => VoidValue,
                () =>
                {
                    doIfNone();
                    return VoidValue;
                });
        }

        private static readonly object VoidValue = new object();
    }
}
