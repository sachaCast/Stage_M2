// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.transcriptorAA
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using WComp.Beans;
using WComp.Util;

namespace WComp.AADesigner
{
    [Bean(Category = "AADesigner-dev")]
    public class transcriptorAA
    {
        private const string pointCutBean_Type = "WComp.AADesigner.PointCutBean";
        private const string pointCutBean_Method = "SetApplicationAssembly";
        private const string pointCutBean_Event = "ListOfJoinpoints";
        private const string pointCutBean_PropName = "AADefinition";
        private const string adviceBean_Type = "WComp.AADesigner.AdviceBean";
        private const string adviceBean_PropName = "AdviceRules";
        private const string adviceBean_Remove_PropName = "RemoveBeanSignal";
        private const string adviceBean_Remove_PropVal = "<?xml version=\"1.0\" encoding=\"utf-8\"?><boolean>true</boolean>";
        private const string adviceBean_Method = "SetListOfJoinpointCombinations";
        private const string adviceBean_Event = "InstancesOfAdvices";
        private const string adviceBean_Remove_Event = "adviceRemoveEvent";
        private const string filterBean_A_B_Type = "WComp.AADesigner.FilterPoinCut_JoinPoint_Bean";
        private const string filterBean_B_C_Type = "WComp.AADesigner.FilterJoinPoint_Advice_Bean";
        private const string filterAA_PropVal = "<?xml version=\"1.0\" encoding=\"utf-8\"?><string>something</string>";
        private const string cycleManager_Event = "ListOfJoinpoints";
        private const string cycleManagerStartCycle_PropName = "TriggerWeavingCycle";
        private const string jointPointBuilderBean_Event = "JoinpointCombinations";
        private const string jointPointBuilderBean_Method = "SetJoinpointLists";
        private const string superposition_Method = "update_Advice";
        private const string superposition_Remove_Method = "RemoveAA";
        private const string filter_Method = "Filter";
        private const string filter_Event = "FilteredEvent";
        public const string superposition_Name = "superpositionBean";
        public const string cycleManager_Name = "cycleManager";
        private const string adviceToUPnP_Name = "adviceToUPnP1";
        private const string adviceToUPnPEmitAllowed_PropName = "EmitAllowed";
        private string[] joinPointBuilderBean_Type;
        private int type_joinPoint;
        private int x_A;
        private int y_A;
        private int x_B;
        private int y_B;
        private int x_C;
        private int y_C;
        private LinkedList<AASelection> AASelectionQueue;
        private AASelection currentAA;
        private bool currentFinished;

        public transcriptorAA()
        {
            this.x_A = 30;
            this.y_A = 30;
            this.x_B = 30;
            this.y_B = 210;
            this.x_C = 30;
            this.y_C = 390;
            this.joinPointBuilderBean_Type = new string[3]
            {
      "WComp.AADesigner.JoinPointDefaultBean",
      "WComp.AADesigner.JoinPointOrderBean",
      "WComp.AADesigner.JoinPointFullBean"
            };
            this.type_joinPoint = 0;
            this.currentAA.name = (string)null;
            this.currentFinished = true;
            this.GroupAdaptation = true;
            this.AASelectionQueue = new LinkedList<AASelection>();
        }

        [DefaultValue(true)]
        public bool GroupAdaptation { get; set; }

        public int Type_joinPoint
        {
            get => checked(this.type_joinPoint + 1);
            set
            {
                this.type_joinPoint = checked(value - 1);
                if (this.type_joinPoint < 0)
                {
                    this.type_joinPoint = 0;
                }
                else
                {
                    if (this.type_joinPoint <= 2)
                        return;
                    this.type_joinPoint = 2;
                }
            }
        }

        public void AddAA(bool adding, string AAName, string AADefinition)
        {
            AASelection aaSelection = new AASelection(Operations.ANY, AAName, AADefinition);
            if (adding)
            {
                aaSelection.operation = Operations.SELECT;
                if (!this.RemoveOppositeOperation(aaSelection))
                {
                    if (this.IsBusyQueue(aaSelection))
                        return;
                    this.StartAASelection(aaSelection);
                }
                else
                    Logger.Info("AA operations discarded ({0})", (object)AAName);
            }
            else
            {
                aaSelection.operation = Operations.DESELECT;
                if (!this.RemoveOppositeOperation(aaSelection))
                {
                    if (!this.IsBusyQueue(aaSelection))
                        this.DeselectAA(aaSelection);
                }
                else
                    Logger.Info("AA operations discarded ({0})", (object)AAName);
            }
            Logger.Info(" ----------------------- definition " + AADefinition + " ----------------------------");

        }

        private void DeselectAA(AASelection AA)
        {
            Logger.Info("Removing AA {0}", (object)AA.name);
            this.currentFinished = false;
            this.currentAA = AA;
            this.FireRemoveBean("A_" + AA.name);
            this.FireSetBeanProp("C_" + AA.name, "RemoveBeanSignal", "<?xml version=\"1.0\" encoding=\"utf-8\"?><boolean>true</boolean>");
            this.FireRemoveBean("B_" + AA.name);
            this.FireRemoveBean("C_" + AA.name);
            checked { this.x_A -= 20; }
            checked { this.y_A -= 20; }
            checked { this.x_B -= 20; }
            checked { this.y_B -= 20; }
            checked { this.x_C -= 20; }
            checked { this.y_C -= 20; }
        }

        private void StartAASelection(AASelection newAA)
        {
            Logger.Info("Adding AA {0}", (object)newAA.name);
            this.currentFinished = false;
            this.currentAA = newAA;
            this.FireCreateBean("WComp.AADesigner.PointCutBean", "A_" + this.currentAA.name, this.x_A, this.y_A);
            checked { this.x_A += 20; }
            checked { this.y_A += 20; }
        }

        public void WaitForBeanCreation(string containerEvent)
        {
            if (this.currentFinished)
                return;
            if (this.currentAA.operation == Operations.SELECT)
            {
                string inst = "A_" + this.currentAA.name;
                if (containerEvent.Contains("|NEW_BEAN|" + inst))
                {
                    this.currentAA.definition = HttpUtility.HtmlEncode(this.currentAA.definition);
                    string pval = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><string>{this.currentAA.definition}</string>";
                    this.FireSetBeanProp(inst, "AADefinition", pval);
                    this.FireGetBeanProp(inst, "AADefinition");
                    Logger.Info("-------------------- envoie definition " + pval + " ----------------------------");

                }
                else if (containerEvent.Contains("|DEL_BEAN|" + inst))
                {
                    this.FireCheckAAinList(this.currentAA.name, false);
                    Logger.Warn("Error deploying AA {0} in the weaver", (object)this.currentAA.name);
                    this.ProcessAASelectionQueue();
                }
                else if (containerEvent.Contains("ERROR") && containerEvent.Contains(inst))
                {
                    if (containerEvent.Contains($"|ERROR|BEAN|bean {inst} already exists in the assembly"))
                    {
                        Logger.Debug("AA {0} was already deployed in the weaver, checking it in the list", (object)this.currentAA.name);
                        this.FireCheckAAinList(this.currentAA.name, true);
                    }
                    else
                    {
                        Logger.Warn("Error deploying AA {0} in the weaver", (object)this.currentAA.name);
                        this.FireCheckAAinList(this.currentAA.name, false);
                    }
                    this.ProcessAASelectionQueue();
                }
                else
                {
                    if (!containerEvent.Contains($"|NEW_LINK|link-C_{this.currentAA.name}-{"superpositionBean"}-{"adviceRemoveEvent"}-{"RemoveAA"}-"))
                        return;
                    Logger.Info("Finished AA selection ({0})", (object)this.currentAA.name);
                    this.FireCheckAAinList(this.currentAA.name, true);
                    this.ProcessAASelectionQueue();
                }
            }
            else
            {
                if (this.currentAA.operation != Operations.DESELECT || !containerEvent.Contains("|DEL_BEAN|C_" + this.currentAA.name) && !containerEvent.Contains("|ERROR|BEAN_REMOVE|bean does not exist: C_" + this.currentAA.name))
                    return;
                Logger.Info("Finished AA deselection ({0})", (object)this.currentAA.name);
                this.FireCheckAAinList(this.currentAA.name, false);
                this.ProcessAASelectionQueue();
            }
        }

        public void ContinueAADeployment(string propertyValue)
        {
            if (propertyValue.Contains(PointCutBean.PARSE_ERROR))
            {
                try
                {
                    int startIndex = checked(propertyValue.IndexOf("<string>") + 8);
                    int num = propertyValue.IndexOf("</string>", 30);
                    string str = propertyValue.Substring(startIndex, checked(num - startIndex));
                    this.FireInformationMessage(!(str == PointCutBean.PARSE_ERROR) ? $"AA `{this.currentAA.name}' has some parse errors and can't be selected: {str.Substring(PointCutBean.PARSE_ERROR.Length)}" : $"AA `{this.currentAA.name}' has some unidenfied parse errors and can't be selected");
                }
                catch
                {
                    Logger.Error("TranscriptorAA: ERROR parsing property value");
                    this.FireInformationMessage("TranscriptorAA: ERROR parsing property value");
                }
                if (this.currentAA.operation == Operations.UPDATE)
                {
                    this.currentAA.operation = Operations.DESELECT;
                    this.DeselectAA(this.currentAA);
                }
                else
                {
                    this.FireRemoveBean("A_" + this.currentAA.name);
                    checked { this.x_A -= 20; }
                    checked { this.y_A -= 20; }
                }
            }
            else if (this.currentAA.operation == Operations.UPDATE)
            {
                Logger.Info("Finished AA definition update ({0})", (object)this.currentAA.name);
                this.ProcessAASelectionQueue();
            }
            else
            {
                Logger.Debug("Finishing AA {0} deployment...", (object)this.currentAA.name);
                if (this.GroupAdaptation)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("<adaptation_schema>");
                    stringBuilder.AppendFormat("<instance type=\"{0}\" name=\"{1}\" />", (object)this.joinPointBuilderBean_Type[this.type_joinPoint], (object)("B_" + this.currentAA.name));
                    stringBuilder.AppendFormat("<instance type=\"{0}\" name=\"{1}\" />", (object)"WComp.AADesigner.AdviceBean", (object)("C_" + this.currentAA.name));
                    stringBuilder.AppendFormat("<link><source>{0}</source><event>{1}</event>", (object)"cycleManager", (object)"ListOfJoinpoints");
                    stringBuilder.AppendFormat("<destination>{0}</destination><method>{1}</method></link>", (object)("A_" + this.currentAA.name), (object)"SetApplicationAssembly");
                    stringBuilder.AppendFormat("<link><source>{0}</source><event>{1}</event>", (object)("A_" + this.currentAA.name), (object)"ListOfJoinpoints");
                    stringBuilder.AppendFormat("<destination>{0}</destination><method>{1}</method></link>", (object)("B_" + this.currentAA.name), (object)"SetJoinpointLists");
                    stringBuilder.AppendFormat("<link><source>{0}</source><event>{1}</event>", (object)("B_" + this.currentAA.name), (object)"JoinpointCombinations");
                    stringBuilder.AppendFormat("<destination>{0}</destination><method>{1}</method></link>", (object)("C_" + this.currentAA.name), (object)"SetListOfJoinpointCombinations");
                    stringBuilder.AppendFormat("<link><source>{0}</source><event>{1}</event>", (object)("C_" + this.currentAA.name), (object)"InstancesOfAdvices");
                    stringBuilder.AppendFormat("<destination>{0}</destination><method>{1}</method></link>", (object)"superpositionBean", (object)"update_Advice");
                    stringBuilder.AppendFormat("<link><source>{0}</source><event>{1}</event>", (object)("C_" + this.currentAA.name), (object)"adviceRemoveEvent");
                    stringBuilder.AppendFormat("<destination>{0}</destination><method>{1}</method></link>", (object)"superpositionBean", (object)"RemoveAA");
                    stringBuilder.Append("</adaptation_schema>");
                    this.FireSetAdaptationSchema(stringBuilder.ToString());
                }
                else
                {
                    this.FireCreateBean(this.joinPointBuilderBean_Type[this.type_joinPoint], "B_" + this.currentAA.name, this.x_B, this.y_B);
                    checked { this.x_B += 20; }
                    checked { this.y_B += 20; }
                    this.FireCreateBean("WComp.AADesigner.AdviceBean", "C_" + this.currentAA.name, this.x_C, this.y_C);
                    checked { this.x_C += 20; }
                    checked { this.y_C += 20; }
                    this.FireCreateLink("cycleManager", "ListOfJoinpoints", "A_" + this.currentAA.name, "SetApplicationAssembly");
                    this.FireCreateLink("A_" + this.currentAA.name, "ListOfJoinpoints", "B_" + this.currentAA.name, "SetJoinpointLists");
                    this.FireCreateLink("B_" + this.currentAA.name, "JoinpointCombinations", "C_" + this.currentAA.name, "SetListOfJoinpointCombinations");
                    this.FireCreateLink("C_" + this.currentAA.name, "InstancesOfAdvices", "superpositionBean", "update_Advice");
                    this.FireCreateLink("C_" + this.currentAA.name, "adviceRemoveEvent", "superpositionBean", "RemoveAA");
                }
            }
        }

        private bool IsBusyQueue(AASelection aa)
        {
            bool flag;
            lock (this.AASelectionQueue)
            {
                flag = !this.currentFinished || this.AASelectionQueue.Count > 0;
                if (flag)
                {
                    this.AASelectionQueue.AddLast(aa);
                    Logger.Debug("Selection queued (AA name: {0})", (object)aa.name);
                }
                else
                    this.currentFinished = false;
            }
            return flag;
        }

        private void ProcessAASelectionQueue()
        {
            AASelection aaSelection = new AASelection();
            lock (this.AASelectionQueue)
            {
                this.currentFinished = true;
                this.currentAA = aaSelection;
                if (this.AASelectionQueue.Count != 0)
                {
                    aaSelection = this.AASelectionQueue.First.Value;
                    this.AASelectionQueue.RemoveFirst();
                }
            }
            if (aaSelection.name != null)
            {
                Logger.Debug("AA {0} unqueued", (object)aaSelection.name);
                switch (aaSelection.operation)
                {
                    case Operations.ANY:
                        Logger.Fatal("{0}\t\t\tIMPOSSIBLE CASE HAPPENING{0}", (object)Environment.NewLine);
                        break;
                    case Operations.SELECT:
                        this.StartAASelection(aaSelection);
                        break;
                    case Operations.DESELECT:
                        this.DeselectAA(aaSelection);
                        break;
                    case Operations.UPDATE:
                        this.StartDefinitionUpdate(aaSelection);
                        break;
                }
            }
            else
                this.StartWeavingCycle();
        }

        private bool MoreOperationsToCome()
        {
            bool come;
            lock (this.AASelectionQueue)
                come = this.AASelectionQueue.Count > 0;
            return come;
        }

        private bool RemoveOppositeOperation(AASelection theNewAA)
        {
            bool flag = false;
            lock (this.AASelectionQueue)
            {
                LinkedListNode<AASelection> node = (LinkedListNode<AASelection>)null;
                for (LinkedListNode<AASelection> linkedListNode = this.AASelectionQueue.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
                {
                    if (linkedListNode.Value.name == theNewAA.name)
                    {
                        if (linkedListNode.Value.operation == theNewAA.operation)
                            linkedListNode.Value.SetDefinition(theNewAA.definition);
                        else
                            node = linkedListNode;
                        flag = true;
                        break;
                    }
                }
                if (node != null)
                    this.AASelectionQueue.Remove(node);
            }
            return flag;
        }

        public void UpdateAADefinition(string name, string definition)
        {
            AASelection aa = new AASelection(Operations.UPDATE, name, definition);
            if (this.IsBusyQueue(aa))
                return;
            this.StartDefinitionUpdate(aa);
        }

        private void StartDefinitionUpdate(AASelection aa)
        {
            this.currentFinished = false;
            this.currentAA = aa;
            aa.definition = HttpUtility.HtmlEncode(aa.definition);
            string pval = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><string>{aa.definition}</string>";
            string inst = "A_" + aa.name;
            this.FireSetBeanProp(inst, "AADefinition", pval);
            this.FireGetBeanProp(inst, "AADefinition");
        }

        private void StartWeavingCycle()
        {
            this.FireSetBeanProp("cycleManager", "TriggerWeavingCycle", "<?xml version=\"1.0\" encoding=\"utf-8\"?><string>something</string>");
        }

        public event transcriptorAA.CreateBeanAtPosDel CreateBean;

        private void FireCreateBean(string t, string i, int x, int y)
        {
            if (this.CreateBean == null)
                return;
            this.CreateBean(t, i, x, y);
        }

        public event transcriptorAA.CreateLinkDel CreateLink;

        private void FireCreateLink(string s, string e, string d, string m, string a)
        {
            if (this.CreateLink == null)
                return;
            this.CreateLink(s, e, d, m, a);
        }

        private void FireCreateLink(string s, string e, string d, string m)
        {
            this.FireCreateLink(s, e, d, m, (string)null);
        }

        public event transcriptorAA.SetBeanPropDel SetBeanProperty;

        private void FireSetBeanProp(string inst, string pname, string pval)
        {
            if (this.SetBeanProperty == null)
                return;
            this.SetBeanProperty(inst, pname, pval);
        }

        public event transcriptorAA.GetBeanPropDel GetBeanProperty;

        private void FireGetBeanProp(string inst, string pname)
        {
            if (this.GetBeanProperty == null)
                return;
            this.GetBeanProperty(inst, pname);
        }

        public event transcriptorAA.StringDelegate RemoveBean;

        private void FireRemoveBean(string inst)
        {
            if (this.RemoveBean == null)
                return;
            this.RemoveBean(inst);
        }

        public event transcriptorAA.ItemChangeDel CheckAAinList;

        private void FireCheckAAinList(string itemName, bool state)
        {
            if (this.CheckAAinList == null)
                return;
            try
            {
                this.CheckAAinList(itemName, state);
            }
            catch (Exception ex)
            {
                Logger.Warn("Error changing checked state of AA {0}: {1}{2}{3}", (object)itemName, (object)ex.Message, (object)Environment.NewLine, (object)ex.StackTrace);
            }
        }

        public event transcriptorAA.StringDelegate SetAdaptationSchema;

        private void FireSetAdaptationSchema(string schema)
        {
            if (this.SetAdaptationSchema == null)
                return;
            this.SetAdaptationSchema(schema);
        }

        public event transcriptorAA.StringDelegate InformationMessage;

        private void FireInformationMessage(string msg)
        {
            if (this.InformationMessage == null)
                return;
            this.InformationMessage(msg);
        }

        public delegate void CreateBeanAtPosDel(string type, string instName, int x, int y);

        public delegate void CreateLinkDel(
          string source,
          string ev,
          string dest,
          string meth,
          string args);

        public delegate void SetBeanPropDel(string instanceName, string propName, string propVal);

        public delegate void GetBeanPropDel(string instanceName, string propName);

        public delegate void StringDelegate(string s);

        public delegate void ItemChangeDel(string name, bool checkState);
    }
}