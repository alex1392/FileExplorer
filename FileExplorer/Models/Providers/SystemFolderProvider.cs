using System;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace FileExplorer.Models
{
	internal class SystemFolderProvider : ISystemFolderProvider
	{
		#region Public Methods

		public string[] GetLogicalDrives()
		{
			return Directory.GetLogicalDrives();
		}

		public string GetRecent()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Recent);
		}

		public string GetDesktop()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}

		public Uri GetHome()
		{
			return new Uri("/Views/HomePage.xaml", UriKind.Relative);
		}

		public string GetDownloads()
		{
			return GetDownLoadPath();
		}

		public string GetDocuments()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		public string GetPictures()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
		}

		public string GetMusic()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
		}

		public string GetVideos()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
		}

		public string GetComputer()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
		}

		#endregion Public Methods

		#region SHGetKnownFolderPath

		/// <summary>
		/// Ref: https://stackoverflow.com/questions/10667012/getting-downloads-folder-in-c
		/// </summary>
		private static string GetDownLoadPath()
		{
			return GetDownLoadPath(KnownFolderFlags.DontVerify, false);

			static string GetDownLoadPath(KnownFolderFlags flags,
			   bool defaultUser)
			{
				int result = SHGetKnownFolderPath(new Guid("{374DE290-123F-4565-9164-39C4925E467B}"),
					(uint)flags, new IntPtr(defaultUser ? -1 : 0), out IntPtr outPath);
				if (result >= 0)
				{
					string path = Marshal.PtrToStringUni(outPath);
					Marshal.FreeCoTaskMem(outPath);
					return path;
				}
				else
				{
					throw new ExternalException("Unable to retrieve the known folder path. It may not "
						+ "be available on this system.", result);
				}
			}
		}

		[DllImport("Shell32.dll")]
		private static extern int SHGetKnownFolderPath(
			[MarshalAs(UnmanagedType.LPStruct)]Guid rfid, uint dwFlags, IntPtr hToken,
			out IntPtr ppszPath);

		[Flags]
		private enum KnownFolderFlags : uint
		{
			SimpleIDList = 0x00000100,
			NotParentRelative = 0x00000200,
			DefaultPath = 0x00000400,
			Init = 0x00000800,
			NoAlias = 0x00001000,
			DontUnexpand = 0x00002000,
			DontVerify = 0x00004000,
			Create = 0x00008000,
			NoAppcontainerRedirection = 0x00010000,
			AliasOnly = 0x80000000
		}
		
		#endregion
	}


}