using OpenSource.UPnP;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using WComp.UPnP2Bean;
using WComp.UPnPDesignComponent;

namespace UPnPWizardDesigner
{
	public class UPnPDevicePanel : Form
	{
		public delegate void DeviceInfoHandler(UPnPSmartControlPoint sender, UPnPDevice device);

		private TreeView devicesTree;

		private MainMenu mainMenu_0;

		private UPnPWizard upnPWizard_0;

		private GroupBox groupBox1;

		private StatusBar statusBar1;

		private StatusBarPanel statustext;

		private StatusBarPanel statuslinked;

		private Button button1;

		private FolderBrowserDialog folderBrowserDialog_0;

		private Label label1;

		private TextBox textBox1;

		private WCompNetController wcompNetController_0;

		private MenuItem menuItem_0;

		private MenuItem menuItem_1;

		private MenuItem menuItem_2;

		private MenuItem menuItem_3;

		private MenuItem menuItem_4;

		private static ArrayList arrayList_0 = new ArrayList();

		private IContainer icontainer_0;

		public UPnPDevicePanel()
		{
			this.InitializeComponent();
			this.method_1();
			this.devicesTree.BeginUpdate();
			this.devicesTree.Nodes.Clear();
			this.devicesTree.EndUpdate();
			this.wcompNetController_0 = new WCompNetController();
			this.wcompNetController_0.SetLogger(new WCompNetController.LogHandler(this.SetStatusMessage));
			this.wcompNetController_0.SetLinkedHandler(new WCompNetController.LinkedHandler(this.SetStatusLink));
			this.wcompNetController_0.SetOnAddedDeviceHandler(new WCompNetController.OnAddedDevice(this.method_2));
			this.wcompNetController_0.SetOnRemovedDeviceHandler(new WCompNetController.OnRemovedDevice(this.method_4));
			this.upnPWizard_0 = new UPnPWizard();
			string text;
			try
			{
				StreamReader streamReader = File.OpenText("beanpath.cfg");
				text = streamReader.ReadLine();
				streamReader.Close();
			}
			catch (Exception)
			{
				text = null;
			}
			if (text == null || text == string.Empty)
			{
				text = "C:\\Program Files\\SharpDevelop\\2.2\\Beans";
			}
			try
			{
				this.upnPWizard_0.SearchPath = text;
			}
			catch (ApplicationException)
			{
				Console.Error.WriteLine("Default beans directory does not exist.");
			}
			this.textBox1.Text = text;
			this.upnPWizard_0.SetLogger(new UPnPWizard.LogHandler(this.SetStatusMessage));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.icontainer_0 != null)
			{
				this.icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.icontainer_0 = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UPnPDevicePanel));
			this.devicesTree = new TreeView();
			this.mainMenu_0 = new MainMenu(this.icontainer_0);
			this.menuItem_0 = new MenuItem();
			this.menuItem_1 = new MenuItem();
			this.menuItem_2 = new MenuItem();
			this.menuItem_3 = new MenuItem();
			this.menuItem_4 = new MenuItem();
			this.groupBox1 = new GroupBox();
			this.statusBar1 = new StatusBar();
			this.statuslinked = new StatusBarPanel();
			this.statustext = new StatusBarPanel();
			this.textBox1 = new TextBox();
			this.label1 = new Label();
			this.folderBrowserDialog_0 = new FolderBrowserDialog();
			this.button1 = new Button();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.statuslinked).BeginInit();
			((ISupportInitialize)this.statustext).BeginInit();
			base.SuspendLayout();
			this.devicesTree.Dock = DockStyle.Fill;
			this.devicesTree.Location = new Point(3, 16);
			this.devicesTree.Name = "devicesTree";
			this.devicesTree.Size = new Size(314, 377);
			this.devicesTree.TabIndex = 0;
			this.mainMenu_0.MenuItems.AddRange(new MenuItem[]
			{
				this.menuItem_0,
				this.menuItem_2,
				this.menuItem_3
			});
			this.menuItem_0.Index = 0;
			this.menuItem_0.MenuItems.AddRange(new MenuItem[]
			{
				this.menuItem_1
			});
			this.menuItem_0.Text = "File";
			this.menuItem_1.Index = 0;
			this.menuItem_1.Text = "Exit";
			this.menuItem_1.Click += new EventHandler(this.menuItem_1_Click);
			this.menuItem_2.Index = 1;
			this.menuItem_2.Text = "Connect";
			this.menuItem_3.Index = 2;
			this.menuItem_3.MenuItems.AddRange(new MenuItem[]
			{
				this.menuItem_4
			});
			this.menuItem_3.Text = "?";
			this.menuItem_4.Index = 0;
			this.menuItem_4.Text = "About";
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.devicesTree);
			this.groupBox1.Location = new Point(0, 29);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(320, 396);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "UPnP Devices";
			this.statusBar1.Location = new Point(0, 401);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new StatusBarPanel[]
			{
				this.statuslinked,
				this.statustext
			});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new Size(320, 24);
			this.statusBar1.TabIndex = 2;
			this.statuslinked.Icon = (Icon)componentResourceManager.GetObject("statuslinked.Icon");
			this.statuslinked.MinWidth = 22;
			this.statuslinked.Name = "statuslinked";
			this.statuslinked.Width = 22;
			this.statustext.Name = "statustext";
			this.statustext.Width = 300;
			this.textBox1.Location = new Point(75, 6);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(214, 20);
			this.textBox1.TabIndex = 3;
			this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
			this.label1.Location = new Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new Size(66, 20);
			this.label1.TabIndex = 4;
			this.label1.Text = "Beans path:";
			this.button1.Location = new Point(292, 5);
			this.button1.Name = "button1";
			this.button1.Size = new Size(25, 20);
			this.button1.TabIndex = 5;
			this.button1.Text = "C";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.AutoScaleBaseSize = new Size(5, 13);
			base.ClientSize = new Size(320, 425);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.statusBar1);
			base.Controls.Add(this.groupBox1);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Menu = this.mainMenu_0;
			base.Name = "UPnPDevicePanel";
			this.Text = "UPnPDevicePanel";
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.statuslinked).EndInit();
			((ISupportInitialize)this.statustext).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void menuItem_1_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void method_0(object sender, EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			foreach (MenuItem menuItem2 in this.menuItem_2.MenuItems)
			{
				menuItem2.Checked = false;
			}
			menuItem.Checked = true;
			WCompNetController.setSelectedService(menuItem.Index);
		}

		private void method_1()
		{
			string[] array = new string[]
			{
				"root",
				"device",
				"service",
				"method",
				"variable",
				"folder_close",
				"folder_open"
			};
			Assembly assembly = base.GetType().Assembly;
			ResourceManager resourceManager = new ResourceManager("UPnPWizardDesigner.pictures", assembly);
			ImageList imageList = new ImageList();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string name = array2[i];
				imageList.Images.Add((Image)resourceManager.GetObject(name));
			}
			this.devicesTree.ImageList = imageList;
		}

		private void method_2(UPnPSmartControlPoint upnPSmartControlPoint_0, UPnPDevice upnPDevice_0)
		{
			if (!this.upnPWizard_0.DeviceIsWCompDevice(upnPSmartControlPoint_0, upnPDevice_0))
			{
				base.Invoke(new UPnPDevicePanel.DeviceInfoHandler(this.method_3), new object[]
				{
					upnPSmartControlPoint_0,
					upnPDevice_0
				});
			}
			this.upnPWizard_0.NewDevice(upnPSmartControlPoint_0, upnPDevice_0);
		}

		private void method_3(UPnPSmartControlPoint upnPSmartControlPoint_0, UPnPDevice upnPDevice_0)
		{
			this.devicesTree.BeginUpdate();
			this.devicesTree.Nodes.Add(new DeviceNode(upnPDevice_0));
			this.devicesTree.EndUpdate();
		}

		private void method_4(UPnPSmartControlPoint upnPSmartControlPoint_0, UPnPDevice upnPDevice_0)
		{
			if (!this.upnPWizard_0.DeviceIsWCompDevice(upnPSmartControlPoint_0, upnPDevice_0))
			{
				base.Invoke(new UPnPDevicePanel.DeviceInfoHandler(this.method_5), new object[]
				{
					upnPSmartControlPoint_0,
					upnPDevice_0
				});
			}
			this.upnPWizard_0.DeviceQuit(upnPSmartControlPoint_0, upnPDevice_0);
		}

		private void method_5(UPnPSmartControlPoint upnPSmartControlPoint_0, UPnPDevice upnPDevice_0)
		{
			this.devicesTree.BeginUpdate();
			foreach (DeviceNode deviceNode in this.devicesTree.Nodes)
			{
				if (deviceNode.GetUPnPObject() == upnPDevice_0)
				{
					this.devicesTree.Nodes.Remove(deviceNode);
				}
			}
			this.devicesTree.EndUpdate();
		}

		public void SetStatusMessage(string str)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new WCompNetController.LogHandler(this.SetStatusMessage), new object[]
				{
					str
				});
				return;
			}
			this.statustext.Text = str;
		}

		public void SetStatusLink(bool linked, int index, string containerName)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new WCompNetController.LinkedHandler(this.SetStatusLink), new object[]
				{
					linked,
					index,
					containerName
				});
				return;
			}
			if (linked)
			{
				this.statuslinked.Icon = UPnPDevicePanel.smethod_0(null, "linked.ico", "UPnPWizardDesigner");
				MenuItem menuItem = new MenuItem();
				menuItem.Text = containerName;
				menuItem.Click += new EventHandler(this.method_0);
				UPnPDevicePanel.arrayList_0.Add(menuItem);
				this.menuItem_2.MenuItems.AddRange(new MenuItem[]
				{
					menuItem
				});
				return;
			}
			if (UPnPDevicePanel.arrayList_0.Count == 1)
			{
				this.statuslinked.Icon = UPnPDevicePanel.smethod_0(null, "unlinked.ico", "UPnPWizardDesigner");
			}
			MenuItem item = (MenuItem)UPnPDevicePanel.arrayList_0[index];
			UPnPDevicePanel.arrayList_0.RemoveAt(index);
			this.menuItem_2.MenuItems.Remove(item);
		}

		private static Icon smethod_0(string string_0, string string_1, string string_2)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string name;
			if (string_0 == null)
			{
				name = string.Format("{0}.{1}", string_2, string_1);
			}
			else
			{
				name = string.Format("{0}.{1}.{2}", string_2, string_0, string_1);
			}
			return new Icon(executingAssembly.GetManifestResourceStream(name));
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			try
			{
				this.upnPWizard_0.SearchPath = this.textBox1.Text;
			}
			catch (ApplicationException)
			{
				MessageBox.Show("The configured path of for compiling proxy beans doesn't exist.\nYou should use the button 'C', (top left) to change it");
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.folderBrowserDialog_0 = new FolderBrowserDialog();
			this.folderBrowserDialog_0.Description = "Select path for compilation of new proxy components";
			this.folderBrowserDialog_0.SelectedPath = this.upnPWizard_0.SearchPath;
			DialogResult dialogResult = this.folderBrowserDialog_0.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				this.textBox1.Text = this.folderBrowserDialog_0.SelectedPath;
			}
		}
	}
}
