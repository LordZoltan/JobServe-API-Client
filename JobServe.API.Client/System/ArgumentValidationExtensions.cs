/// JobServe Jobs API Client - for the Jobs API at http://services.jobserve.com
/// 
/// Copyright Andras Zoltan (https://github.com/LordZoltan) 2013 onwards
/// 
/// You are free to use code, clone it, alter it at will.  But please give me credit if you are incorporating
/// this code into a public repo.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class ArgumentValidationExtensions
	{
		/// <summary>
		/// Throws an ArgumentNullException if the argument value is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		/// <param name="paramName"></param>
		public static void ThrowIfNull<T>(this T arg, string paramName = null)
		{
			if (arg == null)
				throw new ArgumentNullException(paramName);
		}

		/// <summary>
		/// Throws an ArgumentException if the given condition returns true
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		/// <param name="condition"></param>
		/// <param name="message"></param>
		/// <param name="paramName"></param>
		public static void ThrowIf<T>(this T arg, Func<T, bool> condition, string message = null, string paramName = null)
		{
			condition.ThrowIfNull("condition");
			if (condition(arg))
				throw new ArgumentException(message, paramName);
		}

		public static void ThrowIfIsNullOrWhiteSpace(this string arg, string paramName = null)
		{
			if (string.IsNullOrWhiteSpace(arg))
				throw new ArgumentException("The string cannot be null, empty or all-whitespace", paramName);
		}

		public static void ThrowIfIsNullOrEmptyOrAny<T>(this IEnumerable<T> enumerable, Func<T, bool> itemCondition, string message = null, string paramName = null)
		{
			enumerable.ThrowIf(e => e == null || !e.Any() || e.Any(itemCondition), message, paramName);
		}

	}
}
