using System.Runtime.InteropServices;

namespace TriggersTools.Windows.Resources.Native.Structures {
	/// <summary>
	///  Defines the dimensions and style of a dialog box. This structure, always the first in a standard template for
	///  a dialog box, also specifies the number of controls in the dialog box and therefore specifies the number of
	///  subsequent <see cref="DLGITEMTEMPLATE"/> structures in the template.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct DLGTEMPLATE {
		/// <summary>
		///  The style of the dialog box. This member can be a combination of window style values (such as WS_CAPTION
		///  and WS_SYSMENU) and dialog box style values (such as DS_CENTER).<para/>
		///  
		///  If the style member includes the DS_SETFONT style, the header of the dialog box template contains
		///  additional data specifying the font to use for text in the client area and controls of the dialog box. The
		///  font data begins on the WORD boundary that follows the title array. The font data specifies a 16-bit point
		///  size value and a Unicode font name string. If possible, the system creates a font according to the
		///  specified values. Then the system sends a WM_SETFONT message to the dialog box and to each control to
		///  provide a handle to the font. If DS_SETFONT is not specified, the dialog box template does not include the
		///  font data.<para/>
		///  
		///  The DS_SHELLFONT style is not supported in the <see cref="DLGTEMPLATE"/> header.
		/// </summary>
		public uint style;
		/// <summary>
		///  The extended styles for a window. This member is not used to create dialog boxes, but applications that
		///  use dialog box templates can use it to create other types of windows. For a list of values, see Extended
		///  Window Styles.
		/// </summary>
		public uint dwExtendedStyle;
		/// <summary>
		///  The number of items in the dialog box.
		/// </summary>
		public ushort cdit;
		/// <summary>
		///  The x-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short x;
		/// <summary>
		///  The y-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short y;
		/// <summary>
		///  The width, in dialog box units, of the dialog box.
		/// </summary>
		public short cx;
		/// <summary>
		///  The height, in dialog box units, of the dialog box.
		/// </summary>
		public short cy;
	}
}