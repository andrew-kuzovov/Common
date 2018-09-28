﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
namespace Guards
{
	/// <summary>
	/// Contains a set of guard methods.
	/// </summary>
	[DebuggerStepThrough]
	public static class Guard
	{
		/// <summary>
		/// Ensures that the value is not null,
		/// otherwise it will be treated as contract violation and
		/// <exception cref="ContractViolationException">ContractViolationException</exception> will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="contractViolationMessage"></param>
		/// <returns>Checked value</returns>
		public static T CheckNotNullContract<T>(T value, string contractViolationMessage)
			 where T : class
		{
			return GetNotNull(value, () => new ContractViolationException(contractViolationMessage));
		}

		/// <summary>
		/// Ensures that the value is not null,
		/// otherwise it is treated as contract violation and
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="contractViolationMessage"></param>
		/// <returns>Checked value</returns>
		public static T CheckNotNullContract<T>(T? value, string contractViolationMessage)
			 where T : struct
		{
			return GetNotNull(value, () => new ContractViolationException(contractViolationMessage));
		}

		/// <summary>
		/// Checks that data value is not null
		/// or throws <typeparamref name="TException"/> exception generated by <paramref name="exceptionCreator"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TException"></typeparam>
		/// <param name="data"></param>
		/// <param name="exceptionCreator"></param>
		public static void CheckNotNull<T, TException>(T data, Func<TException> exceptionCreator)
			where TException : Exception
		{
			if (data == null)
			{
				throw CreateUserDefinedException(exceptionCreator);
			}
		}

		/// <summary>
		/// Checks that data value is not null
		/// or throws <typeparamref name="TException"/> exception generated by <paramref name="exceptionCreator"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TException"></typeparam>
		/// <param name="data"></param>
		/// <param name="exceptionCreator"></param>
		public static void CheckNotNull<T, TException>(T? data, Func<TException> exceptionCreator)
			where T : struct
			where TException : Exception
		{
			if (data == null)
			{
				throw CreateUserDefinedException(exceptionCreator);
			}
		}

		/// <summary>
		/// Returns the data parameter, ensuring that it's not null
		/// or throws <typeparamref name="TException"/> exception generated by <paramref name="exceptionCreator"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TException"></typeparam>
		/// <param name="data"></param>
		/// <param name="exceptionCreator">Creates exception, which will be thrown if value is null</param>
		/// <returns></returns>
		public static T GetNotNull<T, TException>(T data, Func<TException> exceptionCreator)
			where TException : Exception
		{
			CheckNotNull(data, exceptionCreator);

			return data;
		}

		/// <summary>
		/// Returns the data parameter, ensuring that it's not null
		/// or throws <typeparamref name="TException"/> exception generated by <paramref name="exceptionCreator"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TException"></typeparam>
		/// <param name="data"></param>
		/// <param name="exception">Exception, which will be thrown if value is null</param>
		/// <returns></returns>
		public static T GetNotNull<T>(T data, Exception exception)
		{
			CheckNotNull(exception, "exception");
			return GetNotNull(data, () => exception);
		}

		/// <summary>
		/// Returns the data parameter, ensuring that it's not null.
		/// Throws <exception cref="ContractViolationException">ContractViolationException</exception>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TException"></typeparam>
		/// <param name="data"></param>
		/// <param name="exceptionCreator">Creates exception, which will be thrown if value is null</param>
		/// <returns></returns>
		public static T GetNotNull<T, TException>(T? data, Func<TException> exceptionCreator)
			where T : struct
			where TException : Exception
		{
			CheckNotNull(data, () => CreateUserDefinedException(exceptionCreator));

			// ReSharper disable PossibleInvalidOperationException
			return data.Value;
			// ReSharper restore PossibleInvalidOperationException
		}

		/// <summary>
		/// Returns the data parameter, ensuring that it's not null.
		/// Should be used to check method parameter value.
		/// Throws <exception cref="ContractViolationException">ContractViolationException</exception>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="argument"></param>
		/// <param name="name">Parameter name</param>
		/// <returns></returns>
		public static T GetNotNull<T>(T argument, string name)
		{
			CheckNotNull(argument, name);

			return argument;
		}

		/// <summary>
		/// Returns the data parameter, ensuring that it's not null.
		/// Should be used to check method parameter value.
		/// Throws <exception cref="ContractViolationException">ContractViolationException</exception>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="argument"></param>
		/// <param name="name">Parameter name</param>
		/// <returns></returns>
		public static T GetNotNull<T>(T? argument, string name) where T : struct
		{
			CheckNotNull(argument, name);

			// ReSharper disable PossibleInvalidOperationException
			return argument.Value;
			// ReSharper restore PossibleInvalidOperationException
		}

		/// <summary>
		/// Ensures that method parameter is not null
		/// or throws <exception cref="ArgumentNullException">ArgumentNullException</exception> if it is.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="argument"></param>
		/// <param name="name">Method parameter name</param>
		public static void CheckNotNull<T>(T argument, string name)
		{
			if (argument == null)
			{
				throw CreateArgumentNullException(name);
			}
		}

		/// <summary>
		/// Ensures that method parameter is not null
		/// or throws <exception cref="ArgumentNullException">ArgumentNullException</exception> if it is.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="argument"></param>
		/// <param name="name">Method parameter name</param>
		public static void CheckNotNull<T>(T? argument, string name) where T : struct
		{
			if (argument == null)
			{
				throw CreateArgumentNullException(name);
			}
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// </summary>
		public static void CheckPositive(int argumentValue, string argumentName)
		{
			if (argumentValue > 0)
			{
				return;
			}

			throw new ArgumentOutOfRangeException(argumentName, argumentValue, "Argument should contains only positive values.");
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// </summary>
		public static void CheckPositive(long argumentValue, string argumentName)
		{
			if (argumentValue > 0)
			{
				return;
			}

			throw new ArgumentOutOfRangeException(argumentName, argumentValue, "Argument should contains only positive values.");
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws exception generated by <param name="exception"></param> if it is.
		/// </summary>
		public static void CheckPositive(int argumentValue, Func<Exception> exception)
		{
			if (argumentValue > 0)
			{
				return;
			}

			throw exception();
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// </summary>
		public static int GetPositive(int argumentValue, string argumentName)
		{
			CheckPositive(argumentValue, argumentName);
			return argumentValue;
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// </summary>
		public static long GetPositive(long argumentValue, string argumentName)
		{
			CheckPositive(argumentValue, argumentName);
			return argumentValue;
		}

		/// <summary>
		/// Ensures that method parameter is greater or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// </summary>
		public static void CheckNotNegative(decimal value, string name)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException($"'{name}' must be 0 or positive.");
			}
		}

		/// <summary>
		/// Ensures that method parameter is greater or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// </summary>
		public static void CheckNotNegative(long value, string name)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException($"'{name}' must be 0 or positive.");
			}
		}

		/// <summary>
		/// Ensures that method parameter is greater or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is not.
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// </summary>
		public static void CheckNotNegative(int value, string name)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException($"'{name}' must be 0 or positive.");
			}
		}

		/// <summary>
		/// Ensures that method parameter is greater or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// <returns>value</returns>
		/// </summary>
		public static int GetNotNegative(int value, string name)
		{
			CheckNotNegative(value, name);
			return value;
		}

		/// <summary>
		/// Ensures that method parameter is greater or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// <returns>value</returns>
		/// </summary>
		public static long GetNotNegative(long value, string name)
		{
			CheckNotNegative(value, name);
			return value;
		}

		/// <summary>
		/// Ensures that method parameter is greater or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// <returns>value</returns>
		/// </summary>
		public static decimal GetNotNegative(decimal value, string name)
		{
			CheckNotNegative(value, name);
			return value;
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// </summary>
		public static byte GetPositive(byte argumentValue, string argumentName)
		{
			CheckPositive(argumentValue, argumentName);
			return argumentValue;
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// </summary>
		public static short GetPositive(short argumentValue, string argumentName)
		{
			CheckPositive(argumentValue, argumentName);
			return argumentValue;
		}

		/// <summary>
		/// Ensures that method parameter is not less or equals to zero.
		/// Throws <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception> if it is.
		/// </summary>
		public static decimal GetPositive(decimal argumentValue, string argumentName)
		{
			if (argumentValue > 0)
			{
				return argumentValue;
			}

			throw new ArgumentOutOfRangeException(argumentName, argumentValue, "Argument should contains only positive values.");
		}

		/// <summary>
		/// Ensures that all elements of the collection are not null
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if they are.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="message"></param>
		public static void CheckElementsNotNull<T>(IEnumerable<T> collection, string message) where T : class
		{
			CheckNotNull(collection, "collection");
			CheckContainsText(message, "message");

			CheckTrue(collection.All(element => element != null), message);
		}

		/// <summary>
		/// Checks if object is not disposed
		/// or throws <exception cref="ObjectDisposedException">ObjectDisposedException</exception> if it is.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="isDisposed"></param>
		/// <param name="obj"></param>
		public static void CheckNotDisposed<T>(bool isDisposed, T obj) where T : IDisposable
		{
			if (isDisposed)
			{
				throw new ObjectDisposedException(obj.GetType().FullName);
			}
		}

		/// <summary>
		/// Checks invariant is true
		/// or throws <exception cref="InvariantViolationException">InvariantViolationException</exception> if it's false.
		/// </summary>
		/// <param name="invariant"></param>
		/// <param name="message"></param>
		public static void CheckInvariant(bool invariant, string message)
		{
			if (!invariant)
			{
				throw new InvariantViolationException(message);
			}
		}

		/// <summary>
		/// Checks value is not null
		/// or throws <exception cref="InvariantViolationException">InvariantViolationException</exception> if it is.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mustNotBeNull"></param>
		/// <param name="message"></param>
		public static void CheckNotNullInvariant<T>(T mustNotBeNull, string message) where T : class
		{
			if (mustNotBeNull == null)
			{
				throw new InvariantViolationException(message);
			}
		}

		/// <summary>
		/// Checks string contains text
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="name"></param>
		public static void CheckContainsText(string argument, string name)
		{
			if (argument == null)
			{
				throw CreateArgumentNullException(name);
			}

			if (string.IsNullOrWhiteSpace(argument))
			{
				throw new ContractViolationException(
					$"Argument '{name}' cannot be empty or contain whitespaces only : '{argument}'.");
			}
		}

		/// <summary>
		/// Checks string contains text or throws user defined exception generated by <paramref name="exceptionCreator"/>.
		/// </summary>
		/// <param name="argument"></param>
		/// /// <param name="exceptionCreator"></param>
		public static void CheckContainsText<TException>(string argument, Func<TException> exceptionCreator)
			where TException : Exception
		{
			if (string.IsNullOrEmpty(argument) || string.IsNullOrWhiteSpace(argument))
			{
				throw CreateUserDefinedException(exceptionCreator);
			}
		}

		/// <summary>
		/// Checks string is not empty
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="name"></param>
		public static void CheckNotEmpty(string argument, string name)
		{
			if (argument == null)
			{
				throw CreateArgumentNullException(name);
			}

			if (argument == string.Empty)
			{
				throw new ContractViolationException($"Argument '{name}' cannot be empty string.");
			}
		}

		/// <summary>
		/// Checks Guid is not empty
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="name"></param>
		public static void CheckNotEmpty(Guid argument, string name)
		{
			if (argument == Guid.Empty)
			{
				throw new ContractViolationException($"Argument '{name}' cannot be empty Guid.");
			}
		}

		/// <summary>
		/// Checks Guid is not empty
		/// or throws exception generated from <param name="exception"/> if it is.
		/// </summary>
		public static void CheckNotEmpty(Guid argument, Func<Exception> exception)
		{
			if (argument == Guid.Empty)
			{
				throw exception();
			}
		}

		/// <summary>
		/// Ensures that argument value is not empty Guid.
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		public static Guid GetNotEmpty(Guid argument, string name)
		{
			CheckNotEmpty(argument, name);
			return argument;
		}

		/// <summary>
		/// Checks DateTime is not default value
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="name"></param>
		public static void CheckNotEmpty(DateTime argument, string name)
		{
			if (argument == default(DateTime))
			{
				throw new ContractViolationException($"Argument '{name}' cannot be default value.");
			}
		}

		/// <summary>
		/// Checks DateTime is not default value
		/// or throws exception generated from <param name="exception"/> if it is.
		/// </summary>
		public static void CheckNotEmpty(DateTime argument, Func<Exception> exception)
		{
			if (argument == default(DateTime))
			{
				throw exception();
			}
		}

		/// <summary>
		/// Ensures that argument value is not default value
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		public static DateTime GetNotEmpty(DateTime argument, string name)
		{
			CheckNotEmpty(argument, name);
			return argument;
		}

		/// <summary>
		/// Ensures that array argument is not empty.
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		public static T[] GetNotEmpty<T>(T[] argument, string name)
		{
			CheckNotEmpty(argument, name);
			return argument;
		}

		/// <summary>
		/// Ensures that IEnumerable argument is not empty.
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		public static IEnumerable<T> GetNotEmpty<T>(IEnumerable<T> argument, string name)
		{
			CheckNotEmpty(argument, $"Argument '{name}' should not be empty.");
			return argument;
		}

		/// <summary>
		/// Checks collection contains elements
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if does not.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="message"></param>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
		public static void CheckNotEmpty<T>(IEnumerable<T> collection, string message)
		{
			CheckNotNull(collection, "collection");
			CheckContainsText(message, "message");

			CheckTrue(collection.Any(), message);
		}

		/// <summary>
		/// Returns argument value, checking that its value is not empty
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is.
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetNotEmpty(string argument, string name)
		{
			return
				GetNotEmpty(
					argument,
					() =>
						new ContractViolationException(
							$"Argument '{name}' cannot be null or resolve to an empty string : '{argument}'."));
		}

		/// <summary>
		/// Returns <see cref="Guid"/> argument value, checking that its value is not empty
		/// or throws exception created with <paramref name="exception"/> factory method.
		/// </summary>
		public static Guid GetNotEmpty(Guid argument, Func<Guid, Exception> exception)
		{
			if (argument == Guid.Empty)
			{
				throw exception(argument);
			}

			return argument;
		}

		/// <summary>
		/// Returns argument value, checking that its value is not empty
		/// or throws exception of <typeparamref name="TException"/>.
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="argument"></param>
		/// <param name="exceptionCreator"></param>
		/// <returns></returns>
		public static string GetNotEmpty<TException>(string argument, Func<TException> exceptionCreator)
			where TException : Exception
		{
			if (!string.IsNullOrEmpty(argument))
			{
				return argument;
			}

			throw CreateUserDefinedException(exceptionCreator);
		}

		/// <summary>
		/// Checks <paramref name="check"/> is true
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is not.
		/// </summary>
		/// <param name="check"></param>
		/// <param name="errorMessage"></param>
		public static void CheckTrue(bool check, string errorMessage)
		{
			if (!check)
			{
				throw new ContractViolationException(errorMessage);
			}
		}

		/// <summary>
		/// Checks <paramref name="check"/> is false
		/// or throws <exception cref="ContractViolationException">ContractViolationException</exception> if it is not.
		/// </summary>
		/// <param name="check"></param>
		/// <param name="condition"></param>
		public static void CheckFalse(bool check, string condition)
		{
			CheckTrue(!check, condition);
		}

		/// <summary>
		/// Checks <paramref name="check"/> is true
		/// or throws of <typeparamref name="TException"/> if it is not.
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="check"></param>
		/// <param name="exceptionCreator"></param>
		public static void CheckTrue<TException>(bool check, Func<TException> exceptionCreator)
			where TException : Exception
		{
			if (!check)
			{
				throw CreateUserDefinedException(exceptionCreator);
			}
		}

		/// <summary>
		/// Checks <paramref name="condition"/> is true
		/// or throws of <paramref name="exception"/> if it is not.
		/// </summary>
		public static void CheckTrue(bool condition, Exception exception)
		{
			if (condition)
			{
				return;
			}

			throw GetNotNull(exception, "exception");
		}

		/// <summary>
		/// Checks <paramref name="condition"/> is false
		/// or throws of <paramref name="exception"/> if it is not.
		/// </summary>
		public static void CheckFalse(bool condition, Exception exception)
		{
			if (!condition)
			{
				return;
			}

			throw GetNotNull(exception, "exception");
		}

		/// <summary>
		/// Checks <paramref name="check"/> is false
		/// or throws of <typeparamref name="TException"/> if it is not.
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="check"></param>
		/// <param name="exceptionCreator"></param>
		public static void CheckFalse<TException>(bool check, Func<TException> exceptionCreator)
			where TException : Exception
		{
			CheckTrue(!check, exceptionCreator);
		}

		public static T GetTrue<T>(T argumentValue, string condition, Predicate<T> predicate)
		{
			CheckTrue(predicate(argumentValue), condition);

			return argumentValue;
		}

		public static T GetTrue<T>(T argumentValue, Func<T, string> errorMessage, Predicate<T> predicate)
		{
			CheckNotNull(errorMessage, "errorMessage");
			CheckNotNull(predicate, "predicate");

			CheckTrue(predicate(argumentValue), errorMessage(argumentValue));

			return argumentValue;
		}

		public static void CheckGuidNotEmpty(Guid check, string name)
		{
			if (check != Guid.Empty)
			{
				return;
			}

			throw new ArgumentException($"{name} can`t be empty.");
		}

		public static void CheckNullableGuidNotEmpty(Guid? check, string name)
		{
			if (!check.HasValue)
			{
				return;
			}

			CheckGuidNotEmpty(check.Value, name);
		}

		public static void CheckTaskStarted(Task task)
		{
			CheckNotNull(task, "task");

			CheckFalse(
				task.Status == TaskStatus.Created,
				() => new InvalidOperationException("Asynchronous task not started."));
		}

		public static void CheckStringContains(string argument, string required, string name)
		{
			CheckContainsText(argument, name);
			CheckTrue(argument.Contains(required), name);
		}

		public static void CheckUtc(DateTime? argument, string name)
		{
			if (argument.HasValue)
			{
				CheckTrue(argument.Value.Kind == DateTimeKind.Utc, $"DateTime.Kind for parameter '{name}' has to equal DateTimeKind.Utc");
			}
		}

		#region Guard function results are not null

		/// <summary>
		/// Wraps function with a proxy, ensuring that target <paramref name="function"/> won't return null value.
		/// Throws <exception cref="ContractViolationException">ContractViolationException</exception>
		/// if target function returns null.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="function"></param>
		/// <returns></returns>
		public static Func<TResult> ToNotNullResultGuarded<TResult>(this Func<TResult> function) where TResult : class
		{
			CheckNotNull(function, "function");

			return
				() =>
				GetNotNull(
					function(),
					() => new ContractViolationException($"Function {function} returned null."));
		}

		/// <summary>
		/// Wraps function with a proxy, ensuring that target <paramref name="function"/> won't return null value.
		/// Throws <exception cref="ContractViolationException">ContractViolationException</exception>
		/// if target function returns null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="function"></param>
		/// <returns></returns>
		public static Func<T, TResult> ToNotNullResultGuarded<T, TResult>(this Func<T, TResult> function)
			where T : class
			where TResult : class
		{
			CheckNotNull(function, "function");

			return
				inputParameter =>
				GetNotNull(
					function(inputParameter),
					() => new ContractViolationException($"Function {function}({inputParameter}) returned null."));
		}

		/// <summary>
		/// Wraps function with a proxy, ensuring that target <paramref name="function"/> won't return null value.
		/// Throws <exception cref="ContractViolationException">ContractViolationException</exception>
		/// if target function returns null.
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="function"></param>
		/// <returns></returns>
		public static Func<T1, T2, TResult> ToNotNullResultGuarded<T1, T2, TResult>(this Func<T1, T2, TResult> function)
			where T1 : class
			where T2 : class
			where TResult : class
		{
			CheckNotNull(function, "function");

			return
				(param1, param2) =>
				GetNotNull(
					function(param1, param2),
					() => new ContractViolationException($"Function {function}({param1},{param2}) returned null."));
		}

		#endregion

		private static TException CreateUserDefinedException<TException>(Func<TException> exceptionCreator)
			where TException : Exception
		{
			if (exceptionCreator == null)
			{
				throw CreateExceptionForNullReference("exceptionCreator");
			}

			var exception = exceptionCreator();
			if (exception == null)
			{
				throw new ContractViolationException("Cannot throw an exception: exceptionCreator returned null.");
			}

			return exception;
		}

		private static ContractViolationException CreateExceptionForNullReference(string name)
		{
			return new ContractViolationException("Object reference '" + name + "' is null.");
		}

		private static ArgumentNullException CreateArgumentNullException(string name)
		{
			return new ArgumentNullException(name, $"Argument '{name}' cannot be null.");
		}
	}
}
