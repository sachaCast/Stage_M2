/*
 * Crée par SharpDevelop.
 * Utilisateur: lavirott
 * Date: 26/05/2011
 * Heure: 16:38
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

using WComp.Beans;

namespace Test
{
	/// <summary>
	/// Description of Container1.
	/// </summary>
	public class GUI1 : System.Windows.Forms.Form
	{
        [BeanDesignLocation(16,816)]
        private System.Windows.Forms.TextBox RepositoryPath;
        [BeanDesignLocation(32,104)]
        private System.Windows.Forms.Button Clear;
        [BeanDesignLocation(472,16)]
        private System.Windows.Forms.RichTextBox selectedAA;
        [BeanDesignLocation(648,680)]
        private WComp.UPnPDevice.ControlInterface controlInterface1;
        [BeanDesignLocation(288,16)]
        private WComp.AADesignerUI.SelectorAA AAlist;
        [BeanDesignLocation(16,656)]
        private System.Windows.Forms.TextBox WeaverControlURL;
        [BeanDesignLocation(16,752)]
        private System.Windows.Forms.TextBox ApplicationControlURL;
        [BeanDesignLocation(504,736)]
        private WComp.InterfaceTranslator.StringToBeanProperty stringToBeanProperty1;
        [BeanDesignLocation(184,536)]
        private System.Windows.Forms.Label weaverOutput;
        [BeanDesignLocation(32,432)]
        private System.Windows.Forms.Button Reload;
        [BeanDesignLocation(32,368)]
        private System.Windows.Forms.Button Save;
        [BeanDesignLocation(232,416)]
        private WComp.UPnPDevice.WeaverFunctionalInterface weaverFunctionalInterface1;
        [BeanDesignLocation(504,816)]
        private WComp.InterfaceTranslator.StringToBeanProperty stringToBeanProperty2;
        [BeanDesignLocation(16,592)]
        private System.Windows.Forms.TextBox WeaverFunctionalInterface;
        [BeanDesignLocation(368,688)]
        private WComp.InterfaceTranslator.StringToBeanProperty stringToBeanProperty3;
        [BeanDesignLocation(176,104)]
        private WComp.BasicBeans.PrimitiveValueEmitter clear_PVE;
        [BeanDesignLocation(368,360)]
        private WComp.BasicBeans.PrimitiveValueEmitter definition_PVE;
        [BeanDesignLocation(240,696)]
        private System.Windows.Forms.Button initURL;
		// Binding information for the AddIn (do not remove or modify)
		// Name: GUI1 ; Control: 53200 ; Functional: 53201
		public GUI1()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			//
			// The InitializeBeans() call is required for WComp.NET designer support.
			//
			InitializeBeans();
			
			//
			// TODO: Add constructor code after the InitializeBeans() call.
			//
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new GUI1());
		}
		
		#region WComp.NET designer generated code
		/// <summary>
		/// This method is required for WComp.NET designer support.
		/// Do not change the method contents inside the source code editor.
		/// The WComp.NET designer might not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
            this.RepositoryPath = new System.Windows.Forms.TextBox();
            this.Clear = new System.Windows.Forms.Button();
            this.selectedAA = new System.Windows.Forms.RichTextBox();
            this.AAlist = new WComp.AADesignerUI.SelectorAA();
            this.WeaverControlURL = new System.Windows.Forms.TextBox();
            this.ApplicationControlURL = new System.Windows.Forms.TextBox();
            this.weaverOutput = new System.Windows.Forms.Label();
            this.Reload = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.WeaverFunctionalInterface = new System.Windows.Forms.TextBox();
            this.initURL = new System.Windows.Forms.Button();
            // 
            // RepositoryPath
            // 
            this.RepositoryPath.AutoCompleteCustomSource = null;
            this.RepositoryPath.Text = "D:\\Dev\\src\\WComp\\SharpDevelop3.2-src_release\\exampleAA\\";
            this.RepositoryPath.Controls = null;
            this.RepositoryPath.DataBindings = null;
            this.RepositoryPath.Location = new System.Drawing.Point(16, 816);
            this.RepositoryPath.Size = new System.Drawing.Size(432, 20);
            // 
            // Clear
            // 
            this.Clear.Text = "Clear AA list";
            this.Clear.Controls = null;
            this.Clear.DataBindings = null;
            this.Clear.Location = new System.Drawing.Point(32, 104);
            this.Clear.Size = new System.Drawing.Size(104, 23);
            // 
            // selectedAA
            // 
            this.selectedAA.DetectUrls = false;
            this.selectedAA.Controls = null;
            this.selectedAA.DataBindings = null;
            this.selectedAA.Location = new System.Drawing.Point(472, 16);
            this.selectedAA.Size = new System.Drawing.Size(384, 480);
            // 
            // AAlist
            // 
            this.AAlist.List = "callback,False,checkbox,False,gps_semantic,False,labtb,False,switch_and_light,Fal" +
                "se,switchsocket,False,";
            this.AAlist.Items = null;
            this.AAlist.CustomTabOffsets = null;
            this.AAlist.Controls = null;
            this.AAlist.DataBindings = null;
            this.AAlist.Location = new System.Drawing.Point(288, 16);
            this.AAlist.Size = new System.Drawing.Size(143, 304);
            // 
            // WeaverControlURL
            // 
            this.WeaverControlURL.AutoCompleteCustomSource = null;
            this.WeaverControlURL.Text = "http://127.0.0.1:53100/";
            this.WeaverControlURL.Controls = null;
            this.WeaverControlURL.DataBindings = null;
            this.WeaverControlURL.Location = new System.Drawing.Point(16, 656);
            this.WeaverControlURL.Size = new System.Drawing.Size(268, 20);
            // 
            // ApplicationControlURL
            // 
            this.ApplicationControlURL.AutoCompleteCustomSource = null;
            this.ApplicationControlURL.Text = "http://127.0.0.1:53000/";
            this.ApplicationControlURL.Controls = null;
            this.ApplicationControlURL.DataBindings = null;
            this.ApplicationControlURL.Location = new System.Drawing.Point(16, 752);
            this.ApplicationControlURL.Size = new System.Drawing.Size(268, 20);
            // 
            // weaverOutput
            // 
            this.weaverOutput.Text = "Cannot load AA, definition directory not found";
            this.weaverOutput.Controls = null;
            this.weaverOutput.DataBindings = null;
            this.weaverOutput.Location = new System.Drawing.Point(184, 536);
            this.weaverOutput.Size = new System.Drawing.Size(672, 33);
            // 
            // Reload
            // 
            this.Reload.Text = "Reload AA list";
            this.Reload.Controls = null;
            this.Reload.DataBindings = null;
            this.Reload.Location = new System.Drawing.Point(32, 432);
            this.Reload.Size = new System.Drawing.Size(104, 23);
            // 
            // Save
            // 
            this.Save.Text = "Save current AA";
            this.Save.Controls = null;
            this.Save.DataBindings = null;
            this.Save.Location = new System.Drawing.Point(32, 368);
            this.Save.Size = new System.Drawing.Size(104, 23);
            // 
            // WeaverFunctionalInterface
            // 
            this.WeaverFunctionalInterface.AutoCompleteCustomSource = null;
            this.WeaverFunctionalInterface.Text = "http://127.0.0.1:53101/";
            this.WeaverFunctionalInterface.Controls = null;
            this.WeaverFunctionalInterface.DataBindings = null;
            this.WeaverFunctionalInterface.Location = new System.Drawing.Point(16, 592);
            this.WeaverFunctionalInterface.Size = new System.Drawing.Size(272, 20);
            // 
            // initURL
            // 
            this.initURL.Text = "Init";
            this.initURL.Controls = null;
            this.initURL.DataBindings = null;
            this.initURL.Location = new System.Drawing.Point(240, 696);
            this.initURL.Size = new System.Drawing.Size(56, 23);
            // 
            // GUI1
            // 
            this.Text = "SharpWComp static application";
            this.Controls.Add(this.RepositoryPath);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.selectedAA);
            this.Controls.Add(this.AAlist);
            this.Controls.Add(this.WeaverControlURL);
            this.Controls.Add(this.ApplicationControlURL);
            this.Controls.Add(this.weaverOutput);
            this.Controls.Add(this.Reload);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.WeaverFunctionalInterface);
            this.Controls.Add(this.initURL);
        }
		
		/// <summary>
		/// This method is required for WComp.NET designer support.
		/// Do not change the method contents inside the source code editor.
		/// The WComp.NET designer might not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeBeans() {
            this.controlInterface1 = new WComp.UPnPDevice.ControlInterface();
            this.stringToBeanProperty1 = new WComp.InterfaceTranslator.StringToBeanProperty();
            this.weaverFunctionalInterface1 = new WComp.UPnPDevice.WeaverFunctionalInterface();
            this.stringToBeanProperty2 = new WComp.InterfaceTranslator.StringToBeanProperty();
            this.stringToBeanProperty3 = new WComp.InterfaceTranslator.StringToBeanProperty();
            this.clear_PVE = new WComp.BasicBeans.PrimitiveValueEmitter();
            this.definition_PVE = new WComp.BasicBeans.PrimitiveValueEmitter();
            // 
            // controlInterface1
            // 
            this.controlInterface1.Uri = "http://127.0.0.1:53100/";
            // 
            // stringToBeanProperty1
            // 
            this.stringToBeanProperty1.PropertyName = "Uri";
            this.stringToBeanProperty1.PropertyValue = "http://127.0.0.1:53000/";
            this.stringToBeanProperty1.BeanName = "controlInterface_appli";
            // 
            // weaverFunctionalInterface1
            // 
            this.weaverFunctionalInterface1.Uri = "http://127.0.0.1:53101/";
            // 
            // stringToBeanProperty2
            // 
            this.stringToBeanProperty2.PropertyName = "Directory";
            this.stringToBeanProperty2.PropertyValue = "D:\\Dev\\src\\WComp\\SharpDevelop3.2-src_release\\exampleAA\\";
            this.stringToBeanProperty2.BeanName = "aARepositoryManager1";
            // 
            // stringToBeanProperty3
            // 
            this.stringToBeanProperty3.PropertyName = "Uri";
            this.stringToBeanProperty3.PropertyValue = "http://127.0.0.1:53100/";
            this.stringToBeanProperty3.BeanName = "controlInterface_self";
            // 
            // clear_PVE
            // 
            this.clear_PVE.StringValue = "";
            // 
            // definition_PVE
            // 
            this.definition_PVE.StringValue = "";
            // 
            if ((this.Control != null)) {
                this.Control.CheckForIllegalCrossThreadCalls = false;
            }
            // 
            // Event dispatching
            // 
            this.ApplicationControlURL.Leave += new System.EventHandler(this.@__ApplicationControlURL_to_stringToBeanProperty1_0);
            this.WeaverControlURL.Leave += new System.EventHandler(this.@__WeaverControlURL_to_controlInterface1_1);
            this.stringToBeanProperty1.StringsEvent += new WComp.InterfaceTranslator.StringToBeanProperty.StringsEventHandler(this.controlInterface1.SetPropertyValue);
            this.weaverFunctionalInterface1.InformationMessage_Event += new WComp.UPnPDevice._upnporgIdInformationMessage_InformationMessage_Handler(this.@__weaverFunctionalInterface1_to_weaverOutput_2);
            this.stringToBeanProperty2.StringsEvent += new WComp.InterfaceTranslator.StringToBeanProperty.StringsEventHandler(this.controlInterface1.SetPropertyValue);
            this.RepositoryPath.Leave += new System.EventHandler(this.@__RepositoryPath_to_stringToBeanProperty2_3);
            this.weaverFunctionalInterface1.AADefinition_Event += new WComp.UPnPDevice._upnporgIdAADefinition_AADefinition_Handler(this.@__weaverFunctionalInterface1_to_selectedAA_4);
            this.weaverFunctionalInterface1.RepositoryChanged_Event += new WComp.UPnPDevice._upnporgIdRepositoryChanged_RepositoryChanged_Handler(this.@__weaverFunctionalInterface1_to_AAlist_5);
            this.AAlist.SelectedIndexChanged += new System.EventHandler(this.@__AAlist_to_weaverFunctionalInterface1_6);
            this.AAlist.AASelection += new WComp.AADesignerUI.SelectorAA.AASelectionDeleg(this.weaverFunctionalInterface1.SelectAA);
            this.Reload.Click += new System.EventHandler(this.@__Reload_to_weaverFunctionalInterface1_7);
            this.WeaverFunctionalInterface.Leave += new System.EventHandler(this.@__WeaverFunctionalInterface_to_weaverFunctionalInterface1_8);
            this.WeaverControlURL.Leave += new System.EventHandler(this.@__WeaverControlURL_to_stringToBeanProperty3_9);
            this.stringToBeanProperty3.StringsEvent += new WComp.InterfaceTranslator.StringToBeanProperty.StringsEventHandler(this.controlInterface1.SetPropertyValue);
            this.Clear.Click += new System.EventHandler(this.@__Clear_to_clear_PVE_10);
            this.clear_PVE.EmitStringValue += new WComp.BasicBeans.StringValueEventHandler(this.@__clear_PVE_to_AAlist_11);
            this.clear_PVE.EmitStringValue += new WComp.BasicBeans.StringValueEventHandler(this.@__clear_PVE_to_selectedAA_12);
            this.definition_PVE.EmitStringValue += new WComp.BasicBeans.StringValueEventHandler(this.weaverFunctionalInterface1.AddAA);
            this.selectedAA.TextChanged += new System.EventHandler(this.@__selectedAA_to_definition_PVE_13);
            this.Save.Click += new System.EventHandler(this.@__Save_to_definition_PVE_14);
            this.initURL.Click += new System.EventHandler(this.@__initURL_to_stringToBeanProperty3_15);
            this.controlInterface1._DeviceOK += new WComp.UPnPDevice.VoidDelegateHandler(this.stringToBeanProperty3.ResendEvent);
        }

		private void @__ApplicationControlURL_to_stringToBeanProperty1_0(object sender, System.EventArgs e) {
            this.stringToBeanProperty1.PropertyValue = this.ApplicationControlURL.Text;
        }

		private void @__WeaverControlURL_to_controlInterface1_1(object sender, System.EventArgs e) {
            this.controlInterface1.Uri = this.WeaverControlURL.Text;
        }

		private void @__weaverFunctionalInterface1_to_weaverOutput_2(string NewValue) {
            this.weaverOutput.Text = NewValue;
        }

		private void @__RepositoryPath_to_stringToBeanProperty2_3(object sender, System.EventArgs e) {
            this.stringToBeanProperty2.PropertyValue = this.RepositoryPath.Text;
        }

		private void @__weaverFunctionalInterface1_to_selectedAA_4(string NewValue) {
            this.selectedAA.Text = NewValue;
        }

		private void @__weaverFunctionalInterface1_to_AAlist_5(string NewValue) {
            this.AAlist.List = NewValue;
        }

		private void @__AAlist_to_weaverFunctionalInterface1_6(object sender, System.EventArgs e) {
            this.weaverFunctionalInterface1.GetAADefinition(this.AAlist.Text);
        }

		private void @__Reload_to_weaverFunctionalInterface1_7(object sender, System.EventArgs e) {
            this.weaverFunctionalInterface1.GetRepository();
        }

		private void @__WeaverFunctionalInterface_to_weaverFunctionalInterface1_8(object sender, System.EventArgs e) {
            this.weaverFunctionalInterface1.Uri = this.WeaverFunctionalInterface.Text;
        }

		private void @__WeaverControlURL_to_stringToBeanProperty3_9(object sender, System.EventArgs e) {
            this.stringToBeanProperty3.PropertyValue = this.WeaverControlURL.Text;
        }

		private void @__Clear_to_clear_PVE_10(object sender, System.EventArgs e) {
            this.clear_PVE.FireValueEvent();
        }

		private void @__clear_PVE_to_AAlist_11(string val) {
            this.AAlist.List = val;
        }

		private void @__clear_PVE_to_selectedAA_12(string val) {
            this.selectedAA.Text = val;
        }

		private void @__selectedAA_to_definition_PVE_13(object sender, System.EventArgs e) {
            this.definition_PVE.StringValue = this.selectedAA.Text;
        }

		private void @__Save_to_definition_PVE_14(object sender, System.EventArgs e) {
            this.definition_PVE.FireValueEvent();
        }

		private void @__initURL_to_stringToBeanProperty3_15(object sender, System.EventArgs e) {
            this.stringToBeanProperty3.ResendEvent();
        }
		#endregion
	}
}
