using System;
using System.Runtime.InteropServices;

namespace FileExplorer.Models
{
	internal class TypeDescriptionProvider : ITypeDescriptionProvider
	{
		#region Private Structs

		[StructLayout(LayoutKind.Sequential)]
		private struct SHFILEINFO
		{
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

		private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
		private const uint SHGFI_TYPENAME = 0x000000400;
		private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

		#endregion Private Fields

		#region Public Methods

		/// <summary>
		/// Originally retrieved from <seealso cref="https://stackoverflow.com/questions/3780028/how-can-i-get-the-description-of-a-file-extension-in-net"/>
		/// </summary>
		public string GetFileTypeDescription(string filePath)
		{
			if (IntPtr.Zero != SHGetFileInfo(
								filePath,
								FILE_ATTRIBUTE_NORMAL,
								out var shfi,
								(uint)Marshal.SizeOf(typeof(SHFILEINFO)),
								SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME))
			{
				return shfi.szTypeName;
			}
			return null;
		}

		public string GetFolderTypeDescription(string folderPath)
		{
			if (IntPtr.Zero != SHGetFileInfo(
								folderPath,
								FILE_ATTRIBUTE_NORMAL | FILE_ATTRIBUTE_DIRECTORY,
								out var shfi,
								(uint)Marshal.SizeOf(typeof(SHFILEINFO)),
								SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME))
			{
				return shfi.szTypeName;
			}
			return null;
		}

		#endregion Public Methods

		#region Private Methods

		[DllImport("shell32")]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);

		#endregion Private Methods
	}
}