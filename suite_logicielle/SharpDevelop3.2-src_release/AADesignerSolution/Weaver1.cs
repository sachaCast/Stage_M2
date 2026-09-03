/*
 * Crée par SharpDevelop.
 * Utilisateur: lavirott
 * Date: 26/05/2011
 * Heure: 16:07
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

using WComp.Beans;

namespace DefaultNamespace
{
	/// <summary>
	/// Description of Container1.
	/// </summary>
	public class Weaver1 : System.Windows.Forms.Form
	{
        [BeanDesignLocation(520,368)]
        private WComp.AADesigner.transcriptorAA transcriptorAA1;
        [BeanDesignLocation(720,368)]
        private WComp.UPnPDevice.ControlInterface controlInterface_self;
        [BeanDesignLocation(64,448)]
        private WComp.UPnPProbes.StringMethodProbe AddAA;
        [BeanDesignLocation(176,432)]
        private WComp.UPnPProbes.StringMethodProbe SelectAA;
        [BeanDesignLocation(336,496)]
        private WComp.AADesigner.AARepositoryManager aARepositoryManager1;
        [BeanDesignLocation(632,488)]
        private WComp.UPnPProbes.StringEventProbe AADefinition;
        [BeanDesignLocation(520,528)]
        private WComp.UPnPProbes.StringEventProbe InformationMessage;
        [BeanDesignLocation(632,608)]
        private WComp.UPnPProbes.StringEventProbe RepositoryChanged;
        [BeanDesignLocation(64,552)]
        private WComp.UPnPProbes.StringMethodProbe GetAADefinition;
        [BeanDesignLocation(552,216)]
        private WComp.UPnPDevice.ControlInterface controlInterface_appli;
        [BeanDesignLocation(688,216)]
        private WComp.AADesigner.SingleToDoubleStringSynchronised beansAndLinks;
        [BeanDesignLocation(400,224)]
        private WComp.AADesigner.AdviceToUPnP adviceToUPnP1;
        [BeanDesignLocation(248,232)]
        private WComp.AADesigner.IdentificationConflictManager identificationConflictManager1;
        [BeanDesignLocation(104,232)]
        private WComp.AADesigner.Superposition superpositionBean;
        [BeanDesignLocation(568,40)]
        private WComp.AADesigner.CycleManager cycleManager;
        [BeanDesignLocation(368,56)]
        private WComp.BasicBeans.PrimitiveValueEmitter removalNotifier;
        [BeanDesignLocation(176,568)]
        private WComp.UPnPProbes.VoidMethodProbe GetRepository;
		// Binding information for the AddIn (do not remove or modify)
		// Name: Weaver1 ; Control: 53100 ; Functional: 53101
		public Weaver1()
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
			Application.Run(new Weaver1());
		}
		
		#region WComp.NET designer generated code
		/// <summary>
		/// This method is required for WComp.NET designer support.
		/// Do not change the method contents inside the source code editor.
		/// The WComp.NET designer might not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
            // 
            // Weaver1
            // 
            this.Text = "SharpWComp static application";
        }
		
		/// <summary>
		/// This method is required for WComp.NET designer support.
		/// Do not change the method contents inside the source code editor.
		/// The WComp.NET designer might not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeBeans() {
            this.transcriptorAA1 = new WComp.AADesigner.transcriptorAA();
            this.controlInterface_self = new WComp.UPnPDevice.ControlInterface();
            this.AddAA = new WComp.UPnPProbes.StringMethodProbe();
            this.SelectAA = new WComp.UPnPProbes.StringMethodProbe();
            this.aARepositoryManager1 = new WComp.AADesigner.AARepositoryManager();
            this.AADefinition = new WComp.UPnPProbes.StringEventProbe();
            this.InformationMessage = new WComp.UPnPProbes.StringEventProbe();
            this.RepositoryChanged = new WComp.UPnPProbes.StringEventProbe();
            this.GetAADefinition = new WComp.UPnPProbes.StringMethodProbe();
            this.controlInterface_appli = new WComp.UPnPDevice.ControlInterface();
            this.beansAndLinks = new WComp.AADesigner.SingleToDoubleStringSynchronised();
            this.adviceToUPnP1 = new WComp.AADesigner.AdviceToUPnP();
            this.identificationConflictManager1 = new WComp.AADesigner.IdentificationConflictManager();
            this.superpositionBean = new WComp.AADesigner.Superposition();
            this.cycleManager = new WComp.AADesigner.CycleManager();
            this.removalNotifier = new WComp.BasicBeans.PrimitiveValueEmitter();
            this.GetRepository = new WComp.UPnPProbes.VoidMethodProbe();
            // 
            // transcriptorAA1
            // 
            this.transcriptorAA1.Type_joinPoint = 3;
            // 
            // controlInterface_self
            // 
            this.controlInterface_self.Uri = "http://127.0.0.1:53100/";
            // 
            // controlInterface_appli
            // 
            this.controlInterface_appli.Uri = "http://127.0.0.1:53000/";
            // 
            // adviceToUPnP1
            // 
            this.adviceToUPnP1.CurrentAssembly = null;
            // 
            // identificationConflictManager1
            // 
            this.identificationConflictManager1.InitialAssembly = null;
            // 
            // removalNotifier
            // 
            this.removalNotifier.StringValue = "anything";
            // 
            // Event dispatching
            // 
            this.transcriptorAA1.CreateLink += new WComp.AADesigner.transcriptorAA.CreateLinkDel(this.controlInterface_self.CreateLink);
            this.transcriptorAA1.CreateBean += new WComp.AADesigner.transcriptorAA.CreateBeanAtPosDel(this.controlInterface_self.CreateBeanAtPos);
            this.transcriptorAA1.RemoveBean += new WComp.AADesigner.transcriptorAA.StringDelegate(this.controlInterface_self.RemoveBean);
            this.transcriptorAA1.SetBeanProperty += new WComp.AADesigner.transcriptorAA.SetBeanPropDel(this.controlInterface_self.SetPropertyValue);
            this.transcriptorAA1.GetBeanProperty += new WComp.AADesigner.transcriptorAA.GetBeanPropDel(this.controlInterface_self.GetPropertyValue);
            this.controlInterface_self.GetPropertyValue_Return += new WComp.UPnPDevice._upnporgIdWCompNetAppliService_GetPropertyValue_ReturnHandler(this.transcriptorAA1.ContinueAADeployment);
            this.controlInterface_self.output_Event += new WComp.UPnPDevice._upnporgIdWCompNetAppliService_output_Handler(this.transcriptorAA1.WaitForBeanCreation);
            this.transcriptorAA1.SetAdaptationSchema += new WComp.AADesigner.transcriptorAA.StringDelegate(this.controlInterface_self.SetAdaptationSchema);
            this.aARepositoryManager1.AAChanged += new WComp.AADesigner.AARepositoryManager.AAActivationDel(this.transcriptorAA1.AddAA);
            this.aARepositoryManager1.AAUpdateDefinition += new WComp.AADesigner.AARepositoryManager.AAChangeDefDel(this.transcriptorAA1.UpdateAADefinition);
            this.transcriptorAA1.CheckAAinList += new WComp.AADesigner.transcriptorAA.ItemChangeDel(this.aARepositoryManager1.Checkitem);
            this.SelectAA.Event += new WComp.UPnPProbes.StringMethodProbe.StringEventHandler(this.aARepositoryManager1.SelectAA);
            this.AddAA.Event += new WComp.UPnPProbes.StringMethodProbe.StringEventHandler(this.aARepositoryManager1.AddAA);
            this.aARepositoryManager1.AADefinition += new WComp.AADesigner.AARepositoryManager.StringDelegate(this.AADefinition.Output);
            this.aARepositoryManager1.InformationMessage += new WComp.AADesigner.AARepositoryManager.StringDelegate(this.InformationMessage.Output);
            this.aARepositoryManager1.RepositoryChanged += new WComp.AADesigner.AARepositoryManager.StringDelegate(this.RepositoryChanged.Output);
            this.GetAADefinition.Event += new WComp.UPnPProbes.StringMethodProbe.StringEventHandler(this.aARepositoryManager1.GetAADefinition);
            this.controlInterface_appli.GetBeanNames_Return += new WComp.UPnPDevice._upnporgIdWCompNetAppliService_GetBeanNames_ReturnHandler(this.@__controlInterface_appli_to_beansAndLinks_0);
            this.controlInterface_appli.GetLinks_Return += new WComp.UPnPDevice._upnporgIdWCompNetAppliService_GetLinks_ReturnHandler(this.@__controlInterface_appli_to_beansAndLinks_1);
            this.adviceToUPnP1.CreateLink_event += new WComp.AADesigner.AdviceToUPnP.EventCreateLinkHandler(this.controlInterface_appli.CreateLink);
            this.adviceToUPnP1.CreateComponent_event += new WComp.AADesigner.AdviceToUPnP.EventCreateComponentHandler(this.controlInterface_appli.CreateBean);
            this.adviceToUPnP1.RemoveComponent_event += new WComp.AADesigner.AdviceToUPnP.EventRemoveComponentHandler(this.controlInterface_appli.RemoveBean);
            this.adviceToUPnP1.RemoveLink_event += new WComp.AADesigner.AdviceToUPnP.EventRemoveLinkHandler(this.controlInterface_appli.RemoveLink);
            this.superpositionBean.compositionLAdvicesEvent += new WComp.AADesigner.Superposition.EventCompositionHandler(this.identificationConflictManager1.SetRulesAndFindConflicts);
            this.adviceToUPnP1.SetBeanProperty_event += new WComp.AADesigner.AdviceToUPnP.SetBeanPropHandler(this.controlInterface_appli.SetPropertyValue);
            this.adviceToUPnP1.SetAdaptationSchema_event += new WComp.AADesigner.AdviceToUPnP.EventAdaptationSchemaHandler(this.controlInterface_appli.SetAdaptationSchema);
            this.controlInterface_appli.output_Event += new WComp.UPnPDevice._upnporgIdWCompNetAppliService_output_Handler(this.cycleManager.ContainerEventSink);
            this.beansAndLinks.StringsEvent += new WComp.AADesigner.SingleToDoubleStringSynchronised.StringsEventHandler(this.cycleManager.SetCurrentAssemblyState);
            this.removalNotifier.EmitStringValue += new WComp.BasicBeans.StringValueEventHandler(this.@__removalNotifier_to_cycleManager_2);
            this.superpositionBean.compositionRemoveEvent += new WComp.AADesigner.Superposition.EventRemovedHandler(this.removalNotifier.FireValueEvent);
            this.cycleManager.GetContainerADL += new WComp.AADesigner.CycleManager.VoidDelegate(this.controlInterface_appli.GetBeanNames);
            this.cycleManager.GetContainerADL += new WComp.AADesigner.CycleManager.VoidDelegate(this.controlInterface_appli.GetLinks);
            this.cycleManager.ComputedInitialAssembly += new WComp.AADesigner.CycleManager.ListOfRulesDelegate(this.@__cycleManager_to_identificationConflictManager1_3);
            this.cycleManager.CurrentAssemblyEvent += new WComp.AADesigner.CycleManager.ListOfRulesDelegate(this.@__cycleManager_to_adviceToUPnP1_4);
            this.identificationConflictManager1.rulesEvent += new WComp.AADesigner.IdentificationConflictManager.EventRulesHandler(this.cycleManager.SetAllWeavedRules);
            this.adviceToUPnP1.ActualRules_event += new WComp.AADesigner.AdviceToUPnP.EventActualRulesHandler(this.cycleManager.SetCurrentCycleRules);
            this.identificationConflictManager1.rulesEvent += new WComp.AADesigner.IdentificationConflictManager.EventRulesHandler(this.adviceToUPnP1.SetInputRules);
            this.cycleManager.CancelWeavingCycle += new WComp.AADesigner.CycleManager.BoolDelegate(this.@__cycleManager_to_adviceToUPnP1_5);
            this.GetRepository.Event += new WComp.UPnPProbes.VoidMethodProbe.VoidEventHandler(this.aARepositoryManager1.GetRepositoryStatus);
            this.transcriptorAA1.InformationMessage += new WComp.AADesigner.transcriptorAA.StringDelegate(this.InformationMessage.Output);
        }

		private void @__controlInterface_appli_to_beansAndLinks_0(string _ReturnValue) {
            this.beansAndLinks.Value1 = _ReturnValue;
        }

		private void @__controlInterface_appli_to_beansAndLinks_1(string _ReturnValue) {
            this.beansAndLinks.Value2 = _ReturnValue;
        }

		private void @__removalNotifier_to_cycleManager_2(string val) {
            this.cycleManager.TriggerWeavingCycle = val;
        }

		private void @__cycleManager_to_identificationConflictManager1_3(System.Collections.Generic.List<WComp.AADesigner.AdviceRule> rules) {
            this.identificationConflictManager1.InitialAssembly = rules;
        }

		private void @__cycleManager_to_adviceToUPnP1_4(System.Collections.Generic.List<WComp.AADesigner.AdviceRule> rules) {
            this.adviceToUPnP1.CurrentAssembly = rules;
        }

		private void @__cycleManager_to_adviceToUPnP1_5(bool b) {
            this.adviceToUPnP1.StopCycle = b;
        }
		#endregion
	}
}
