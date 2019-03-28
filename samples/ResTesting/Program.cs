using System;
using System.Collections.Generic;
using System.IO;
using TriggersTools.Windows.Resources;

namespace ResTesting {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Hello World!");
			FindCatExecutables(@"C:\Programs\Games\Frontwing\Eden of Grisaia - Copy (2)");
			Console.ReadLine();
			//new ResourceInfo(@"C:\Programs\Games\Frontwing\Eden of Grisaia - Copy (2)\cs2_debug_key.dat", false);
		}
		private const string KeyCodeType = "KEY_CODE";
		private const string KeyCodeName = "KEY";
		private const string VCodeType = "V_CODE";
		private const string VCodeName = "DATA";
		private const string VCode2Type = "V_CODE2";
		private const string VCode2Name = "DATA";
		private const ushort Language = 0x0411;

		static bool IsCatExecutable(string exeFile) {
			if (exeFile == null)
				throw new ArgumentNullException(nameof(exeFile));
			try {
				var resInfo = new ResourceInfo(exeFile, false);
				resInfo.LoadGeneric(KeyCodeType, KeyCodeName, Language);
				resInfo.LoadGeneric(VCodeType, VCodeName, Language);
				resInfo.LoadGeneric(VCode2Type, VCode2Name, Language);
				return true;
			} catch {
				return false;
			}
		}
		/// <summary>
		///  Attempts to locate all executables with V_CODEs in the CatSytem2 game install directory.
		///  Checks every extension except .bak.
		/// </summary>
		/// <param name="installDir">The installation directory to check the files of.</param>
		/// <returns>A collection of executable paths that contain V_CODEs.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="installDir"/> is null.
		/// </exception>
		static void/*IEnumerable<string>*/ FindCatExecutables(string installDir) {
			foreach (string file in Directory.EnumerateFiles(installDir)) {
				string ext = Path.GetExtension(file).ToLower();
				if (ext != ".bak" && IsCatExecutable(file))
					Console.WriteLine(file);
					//yield return file;
			}
		}
	}
}
