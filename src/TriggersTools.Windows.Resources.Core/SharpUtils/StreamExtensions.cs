using System;
using System.IO;
using System.Net.Sockets;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  Extensions for the <see cref="Stream"/> class.
	/// </summary>
	internal static class StreamExtensions {
		#region IsEndOfStream

		/// <summary>
		///  Determins if the stream pointer is at or passed the length of the stream. If this is a
		///  <see cref="NetworkStream"/>, then <see cref="NetworkStream.DataAvailable"/> will be called.
		/// </summary>
		/// <param name="stream">The stream to check for the end of.</param>
		/// <returns>True if the end of the stream has been reached.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> is null.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support seeking.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred. Or the underlying <see cref="Socket"/> is closed.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="SocketException">
		///  Use the <see cref="SocketException.ErrorCode"/> property to obtain the specific error code, and
		///  description refer to the Windows Sockets version 2 API error code documentation in MSDN for a detailed of
		///  the error.
		/// </exception>
		public static bool IsEndOfStream(this Stream stream) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (stream is NetworkStream netStream)
				return !netStream.DataAvailable;
			return stream.Position >= stream.Length;
		}

		#endregion

		#region ReadToEnd

		/// <summary>
		///  Reads the remaining bytes in the stream and advances the current position.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		/// <returns>A byte array with the remaining bytes.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> is null.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static byte[] ReadToEnd(this Stream stream) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			using (MemoryStream memoryStream = new MemoryStream()) {
				stream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}

		#endregion
	}
}
