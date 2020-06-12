using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExplorer.Views {
	class DialogService : IDialogService {
		public void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}
	}
}
