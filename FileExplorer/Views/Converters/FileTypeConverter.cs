using FileExplorer.Models;

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Data;
using System.Windows.Markup;

namespace FileExplorer.Converters {

	internal class FileTypeConverter : MarkupExtension, IValueConverter {

		#region Private Structs

		[StructLayout(LayoutKind.Sequential)]
		private struct SHFILEINFO {
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}

		#endregion Private Structs

		#region Private Fields

		private const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
		private const uint FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
		private const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;
		private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		private const uint FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
		private const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
		private const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
		private const uint FILE_ATTRIBUTE_OFFLINE = 0x00001000;
		private const uint FILE_ATTRIBUTE_READONLY = 0x00000001;
		private const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
		private const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
		private const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
		private const uint FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
		private const uint FILE_ATTRIBUTE_VIRTUAL = 0x00010000;
		private const uint SHGFI_ATTR_SPECIFIED = 0x000020000;
		private const uint SHGFI_ATTRIBUTES = 0x000000800;
		private const uint SHGFI_DISPLAYNAME = 0x000000200;
		private const uint SHGFI_EXETYPE = 0x000002000;
		private const uint SHGFI_ICON = 0x000000100;

		// get attributes
		private const uint SHGFI_ICONLOCATION = 0x000001000;

		// get only specified attributes
		private const uint SHGFI_LARGEICON = 0x000000000;

		private const uint SHGFI_LINKOVERLAY = 0x000008000;
		private const uint SHGFI_OPENICON = 0x000000002;
		private const uint SHGFI_PIDL = 0x000000008;

		// put a link overlay on icon
		private const uint SHGFI_SELECTED = 0x000010000;

		// get open icon
		private const uint SHGFI_SHELLICONSIZE = 0x000000004;

		// show icon in selected state
		// get large icon
		private const uint SHGFI_SMALLICON = 0x000000001;

		// get icon location
		// return exe type
		private const uint SHGFI_SYSICONINDEX = 0x000004000;

		// get icon
		// get display name
		private const uint SHGFI_TYPENAME = 0x000000400;

		// get type name
		// get system icon index
		// get small icon
		// get shell size icon
		// pszPath is a pidl
		private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

		private static readonly FileTypeConverter instance = new FileTypeConverter();

		#endregion Private Fields

		#region Public Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ListFileItem fileItem) {
				return GetFileTypeDescription(fileItem.Name);
			} else if (value is TreeFolderItem folderItem) {
				return GetFolderTypeDescription(folderItem.Path);
			} else {
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return instance;
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Originally retrieved from <seealso cref="https://stackoverflow.com/questions/3780028/how-can-i-get-the-description-of-a-file-extension-in-net"/>
		/// </summary>
		private static string GetFileTypeDescription(string fileNameOrExtension)
		{
			SHFILEINFO shfi;
			if (IntPtr.Zero != SHGetFileInfo(
								fileNameOrExtension,
								FILE_ATTRIBUTE_NORMAL,
								out shfi,
								(uint)Marshal.SizeOf(typeof(SHFILEINFO)),
								SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME)) {
				return shfi.szTypeName;
			}
			return null;
		}

		private static string GetFolderTypeDescription(string folderPath)
		{
			SHFILEINFO shfi;
			if (IntPtr.Zero != SHGetFileInfo(
								folderPath,
								FILE_ATTRIBUTE_NORMAL | FILE_ATTRIBUTE_DIRECTORY,
								out shfi,
								(uint)Marshal.SizeOf(typeof(SHFILEINFO)),
								SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME)) {
				return shfi.szTypeName;
			}
			return null;
		}

		[DllImport("shell32")]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);

		#endregion Private Methods

		// use passed dwFileAttribute
	}
}