using OpenSource.UPnP;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using WComp.UPnP2Bean;
using WComp.UPnPDesignComponent;

namespace UPnPWizardDesigner
{
	public class UPnPWizard
	{
		public delegate void LogErrorHandler(string message);

		public delegate void LogHandler(string message);

		private string string_0 = "";

		private UPnPWizard.LogHandler logHandler_0;

		private UPnPWizard.LogErrorHandler logErrorHandler_0;

		private Hashtable hashtable_0;

		public string SearchPath
		{
			get
			{
				return this.string_0;
			}
			set
			{
				if (!Directory.Exists(value))
				{
					throw new ApplicationException("Directory does not exists");
				}
				this.string_0 = value;
				StreamWriter streamWriter = File.CreateText("beanpath.cfg");
				streamWriter.WriteLine(value);
				streamWriter.Close();
			}
		}

		public UPnPWizard()
		{
			this.hashtable_0 = new Hashtable();
		}

		public void SetLogger(UPnPWizard.LogHandler logHandler)
		{
			this.logHandler_0 = logHandler;
		}

		public void SetErrorLogger(UPnPWizard.LogErrorHandler logErrorHandler)
		{
			this.logErrorHandler_0 = logErrorHandler;
		}

		public void NewDevice(UPnPSmartControlPoint sender, UPnPDevice device)
		{
			if (this.DeviceIsWCompDevice(sender, device))
			{
				return;
			}
			device.FriendlyName = this.method_3(device.FriendlyName, true);
			if (!this.method_0(device.FriendlyName))
			{
				string text = this.method_1(device);
				if (text != null)
				{
					this.method_8("File generated for UPnP device: " + text);
					if (this.method_4(text))
					{
						return;
					}
					this.method_8("Type loaded in container");
				}
			}
			string text2 = this.method_5(device.FriendlyName);
			if (text2 == null)
			{
				return;
			}
			this.hashtable_0.Add(device.UniqueDeviceName, text2);
			this.method_6(text2, device.LocationURL);
		}

		public void DeviceQuit(UPnPSmartControlPoint sender, UPnPDevice device)
		{
			string text = (string)this.hashtable_0[device.UniqueDeviceName];
			this.method_8(string.Concat(new string[]
			{
				"Device ",
				device.FriendlyName,
				" has exited (",
				text,
				")"
			}));
			if (text != null && text != string.Empty)
			{
				this.method_7(text);
			}
			this.hashtable_0.Remove(device.UniqueDeviceName);
		}

		public bool DeviceIsWCompDevice(UPnPSmartControlPoint sender, UPnPDevice device)
		{
			return device.DeviceURN == "urn:schemas-upnp-org:device:WCompNetProbeDevice:1";
		}

		private bool method_0(string string_1)
		{
			return File.Exists(this.string_0 + "\\" + string_1 + ".UPnPWizardDesigner.dll") || File.Exists(this.string_0 + "\\" + string_1 + ".UPnP2Bean.WComp.NET.Generated.dll");
		}

		private string method_1(UPnPDevice upnPDevice_0)
		{
			DeviceNode deviceNode = new DeviceNode(upnPDevice_0);
			deviceNode.InCodeName = upnPDevice_0.FriendlyName;
			deviceNode.Checked = true;
			foreach (ServiceNode serviceNode in deviceNode.Nodes)
			{
				this.method_8(" with service " + serviceNode.Name);
				serviceNode.Checked = true;
			}
			CodeCompileUnit codeCompileUnit = this.method_2(new TreeView
			{
				Nodes = 
				{
					deviceNode
				}
			});
			if (codeCompileUnit == null)
			{
				this.method_9("Failed to create Compile Unit.");
				return null;
			}
			CompilerParameters compilerParameters = new CompilerParameters(new string[]
			{
				"mscorlib.dll"
			});
			compilerParameters.ReferencedAssemblies.Add("System.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Drawing.dll");
			compilerParameters.ReferencedAssemblies.Add(this.string_0 + "\\UPnP.dll");
			compilerParameters.ReferencedAssemblies.Add(this.string_0 + "\\Beans.dll");
			compilerParameters.GenerateInMemory = false;
			compilerParameters.GenerateExecutable = false;
			compilerParameters.OutputAssembly = this.string_0 + "\\" + upnPDevice_0.FriendlyName + ".UPnPWizardDesigner.dll";
			CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");
			CompilerResults compilerResults = codeDomProvider.CompileAssemblyFromDom(compilerParameters, new CodeCompileUnit[]
			{
				codeCompileUnit
			});
			if (compilerResults != null && compilerResults.Errors.Count <= 0)
			{
				this.method_8("New bean generated for " + upnPDevice_0.FriendlyName);
				return compilerParameters.OutputAssembly;
			}
			this.method_9("Compilation failed: " + compilerResults.Errors.Count + " errors.");
			foreach (CompilerError compilerError in compilerResults.Errors)
			{
				this.method_9(string.Concat(new object[]
				{
					" Line : ",
					compilerError.Line,
					".",
					compilerError.ErrorText
				}));
			}
			return null;
		}

		private CodeCompileUnit method_2(TreeView treeView_0)
		{
			CodeCompileUnit result;
			try
			{
				UPnPProxyCodeGenerator uPnPProxyCodeGenerator = new UPnPProxyCodeGenerator("WComp.UPnPDevice");
				result = uPnPProxyCodeGenerator.GenerateCode(treeView_0);
			}
			catch (Exception arg)
			{
				this.method_9("Could not create proxy code. Reason: " + arg);
				result = null;
			}
			return result;
		}

		private string method_3(string string_1, bool bool_0)
		{
			if (bool_0)
			{
				if ((string_1[0] >= 'A' && string_1[0] <= 'Z') || (string_1[0] >= 'a' && string_1[0] <= 'z'))
				{
					return string_1[0].ToString() + this.method_3(string_1.Substring(1), false);
				}
				return "Device";
			}
			else
			{
				if (string_1.Length == 0)
				{
					return "";
				}
				if ((string_1[0] >= 'A' && string_1[0] <= 'Z') || (string_1[0] >= 'a' && string_1[0] <= 'z') || (string_1[0] >= '0' && string_1[0] <= '9'))
				{
					return string_1[0].ToString() + this.method_3(string_1.Substring(1), false);
				}
				return "_" + this.method_3(string_1.Substring(1), false);
			}
		}

		private bool method_4(string string_1)
		{
			bool result;
			try
			{
				WCompNetController.getSelectedService().LoadType(string_1);
				result = false;
			}
			catch (UPnPInvokeException ex)
			{
				this.method_9("UPnP exception invoking Loadtype: " + ex.Message + " | " + ex.UPNP.ErrorDescription);
				result = true;
			}
			return result;
		}

		private string method_5(string string_1)
		{
			string result;
			try
			{
				string text = WCompNetController.getSelectedService().CreateBean("WComp.UPnPDevice." + string_1);
				result = text;
			}
			catch (UPnPInvokeException ex)
			{
				this.method_9("UPnP exception invoking CreateBeanAtPos: " + ex.Message + " | " + ex.UPNP.ErrorDescription);
				result = null;
			}
			return result;
		}

		private void method_6(string string_1, string string_2)
		{
			try
			{
				string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?><string>" + string_2 + "</string>";
				WCompNetController.getSelectedService().SetPropertyValue(string_1, "Uri", text);
			}
			catch (UPnPInvokeException ex)
			{
				this.method_9("UPnP exception invoking SetBeanPropertyValue: " + ex.Message + " | " + ex.UPNP.ErrorDescription);
			}
		}

		private bool method_7(string string_1)
		{
			bool result;
			try
			{
				WCompNetController.getSelectedService().RemoveBean(string_1);
				result = false;
			}
			catch (UPnPInvokeException ex)
			{
				this.method_9("UPnP exception invoking RemoveBean: " + ex.Message + " | " + ex.UPNP.ErrorDescription);
				result = true;
			}
			return result;
		}

		private void method_8(string string_1)
		{
			if (this.logHandler_0 != null)
			{
				this.logHandler_0(string_1);
			}
			Console.Out.WriteLine(string_1);
		}

		private void method_9(string string_1)
		{
			if (this.logErrorHandler_0 != null)
			{
				this.logErrorHandler_0(string_1);
			}
			Console.Error.WriteLine(string_1);
			System.Windows.Forms.MessageBox.Show(string_1, "Erreur UPnPWizard", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);		}
	}
}
