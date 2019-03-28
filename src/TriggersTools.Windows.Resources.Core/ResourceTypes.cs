using System.ComponentModel;

namespace TriggersTools.Windows.Resources {
	/// <summary>
	///  The predefined resource types.
	/// </summary>
	public enum ResourceTypes : ushort {
		/// <summary>
		///   For resource types not covered by the standard integer types.
		/// </summary>
		[Description("RT_OTHER")]
		Other = 0,
		/// <summary>
		///  Hardware-dependent cursor resource.
		/// </summary>
		[Description("RT_CURSOR")]
		Cursor = 1,
		/// <summary>
		///  Bitmap resource.
		/// </summary>
		[Description("RT_BITMAP")]
		Bitmap = 2,
		/// <summary>
		///  Hardware-dependent icon resource.
		/// </summary>
		[Description("RT_ICON")]
		Icon = 3,
		/// <summary>
		///  Menu resource.
		/// </summary>
		[Description("RT_MENU")]
		Menu = 4,
		/// <summary>
		///  Dialog box.
		/// </summary>
		[Description("RT_DIALOG")]
		Dialog = 5,
		/// <summary>
		///  String-table entry.
		/// </summary>
		[Description("RT_STRING")]
		String = 6,
		/// <summary>
		///  Font directory resource.
		/// </summary>
		[Description("RT_FONTDIR")]
		FontDir = 7,
		/// <summary>
		///  Font resource.
		/// </summary>
		[Description("RT_FONT")]
		Font = 8,
		/// <summary>
		///  Accelerator table.
		/// </summary>
		[Description("RT_ACCELERATOR")]
		Accelerator = 9,
		/// <summary>
		///  Application-defined resource (raw data).
		/// </summary>
		[Description("RT_RCDATA")]
		RCData = 10,
		/// <summary>
		///  Message-table entry.
		/// </summary>
		[Description("RT_MESSAGETABLE")]
		MessageTable = 11,
		/// <summary>
		///  Hardware-independent cursor resource.
		/// </summary>
		[Description("RT_GROUP_CURSOR")]
		GroupCursor = 12,
		/// <summary>
		///  Hardware-independent icon resource.
		/// </summary>
		[Description("RT_GROUP_ICON")]
		GroupIcon = 14,
		/// <summary>
		///  Version resource.
		/// </summary>
		[Description("RT_VERSION")]
		Version = 16,
		/// <summary>
		///  Allows a resource editing tool to associate a string with an .rc file.
		/// </summary>
		[Description("RT_DLGINCLUDE")]
		DlgInclude = 17,
		/// <summary>
		///  Plug and Play resource.
		/// </summary>
		[Description("RT_PLUGPLAY")]
		PlugPlay = 19,
		/// <summary>
		///  VXD.
		/// </summary>
		[Description("RT_VXD")]
		VXD = 20,
		/// <summary>
		///  Animated cursor.
		/// </summary>
		[Description("RT_ANICURSOR")]
		AniCursor = 21,
		/// <summary>
		///  Animated icon.
		/// </summary>
		[Description("RT_ANIICON")]
		AniIcon = 22,
		/// <summary>
		///  HTML.
		/// </summary>
		[Description("RT_HTML")]
		HTML = 23,
		/// <summary>
		///  Microsoft Windows XP: Side-by-Side Assembly XML Manifest.
		/// </summary>
		[Description("RT_MANIFEST")]
		Manifest = 24,
	}
}