using System;

namespace TriggersTools.Windows.Resources {
	/// <summary>
	///  A resource load or save exception.
	/// </summary>
	public class ResourceIOException : Exception {
		/*#region Fields

		private Exception outerException;

		#endregion*/

		/// <summary>
		///  Constructs a new resource IO exception.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="innerException">The inner exception thrown within a single resource.</param>
		public ResourceIOException(string message, Exception innerException) : base(message, innerException) { }
		/*/// <summary>
		///  Constructs a new resource IO exception.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="innerException">The inner exception thrown within a single resource.</param>
		/// <param name="outerException">The outer exception from the Win32 API.</param>
		public ResourceIOException(string message, Exception innerException, Exception outerException)
			: base(message, innerException)
		{
			this.outerException = outerException;
		}*/

		/*/// <summary>
		///  A combined message of the inner and outer exception.
		/// </summary>
		public override string Message {
			get => InnerException != null ? $"{base.Message} {InnerException.Message}" : base.Message;
		}*/
	}
}