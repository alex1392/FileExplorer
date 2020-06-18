using NUnit.Framework;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExplorer.Views.Services;

namespace FileExplorer.Models.Tests
{
	[TestFixture()]
	public class FileProviderTests
	{
		private readonly FileProvider fileProvider;

		public FileProviderTests()
		{
			fileProvider = new FileProvider(new DialogService());

		}
		[Test()]
		public void CopyTest()
		{
			fileProvider.Copy(@"C:\Users\alex\Desktop\新增資料夾", @"C:\Users\alex\Desktop\新增資料夾");
		}

		[Test()]
		public void MoveTest()
		{
			fileProvider.Move(@"C:\Users\alex\Desktop\新增資料夾", @"C:\Users\alex\Desktop\新增資料夾 (2)");
		}

		[Test()]
		public void DeleteTest()
		{
			fileProvider.Delete(@"C:\Users\alex\Desktop\新增資料夾");
		}
	}
}