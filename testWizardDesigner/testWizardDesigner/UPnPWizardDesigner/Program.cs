using System;
using System.Windows.Forms;

namespace UPnPWizardDesigner
{
	public class Program
	{
		[STAThread]
		private static void Main()
		{
			Console.WriteLine("UPnP Wizard Designer...");
			Application.Run(new UPnPDevicePanel());
		}
	}
}
