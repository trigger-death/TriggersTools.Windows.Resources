using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace TriggersTools.SharpUtils.Exceptions {
	/// <summary>
	///  A static class for extensions revolving around exceptions.
	/// </summary>
	internal static class ExceptionExtensions {
		#region Rethrow

		/// <summary>
		///  Rethrows the exception and retains all stack trace information.
		/// </summary>
		/// <param name="ex">The exception to rethrow.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="ex"/> is null.
		/// </exception>
		public static void Rethrow(this Exception ex) {
			if (ex == null)
				throw new ArgumentNullException(nameof(ex));
			ExceptionDispatchInfo.Capture(ex).Throw();
		}

		#endregion
	}
}
