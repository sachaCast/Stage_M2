// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.CycleManager
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WComp.Beans;
using WComp.Util;

namespace WComp.AADesigner
{
    [Bean(Category = "AADesigner-dev")]
    public class CycleManager
    {
        private bool cycleRunning;
        private bool newCycleRequested;
        private int modificationsCounter;
        private List<VariableComponent> listJoinPoints;
        private List<AdviceRule> wovenRules;
        private LinkedList<AdviceRule> rulesToAck;
        private List<AdviceRule> currentAssembly;
        private List<AdviceRule> initialAssembly;

        public CycleManager()
        {
            this.cycleRunning = false;
            this.modificationsCounter = -1;
            this.newCycleRequested = false;
            this.listJoinPoints = new List<VariableComponent>();
            this.wovenRules = new List<AdviceRule>();
            this.rulesToAck = new LinkedList<AdviceRule>();
            this.currentAssembly = new List<AdviceRule>();
            this.initialAssembly = new List<AdviceRule>();
        }

        [DefaultValue("Setting this property to something non-void will trigger a new weaving cycle")]
        public string TriggerWeavingCycle
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                this.StartWeavingCycle();
            }
            get => "Setting this property to something non-void will trigger a new weaving cycle";
        }

        public void SetAllWovenRules(List<AdviceRule> rules)
        {
            if (rules == null)
                this.wovenRules.Clear();
            else
                this.wovenRules = rules;
        }

        public void SetCurrentCycleRules(LinkedList<AdviceRule> rules)
        {
            if (this.rulesToAck.Count != 0)
                Logger.Warn("NEW CURRENT RULES RECEIVED WITHOUT NOTIFICATION OF PROPER ADAPTATION");
            this.rulesToAck = rules;
            Logger.Debug("{0} new woven rules properly saved in cycle manager (waiting for resulting events):", (object)rules.Count);
            foreach (AdviceRule rule in rules)
            {
                switch (rule)
                {
                    case CompCreationRule _:
                        Logger.Debug("+ {0}", (object)(CompCreationRule)rule);
                        break;
                    case LinkCreationRule _:
                        Logger.Debug("+ {0}", (object)(LinkCreationRule)rule);
                        break;
                    case SetPropertyRule _:
                        Logger.Debug("  {0}", (object)(SetPropertyRule)rule);
                        break;
                    case LinkRemovingRule _:
                        Logger.Debug("- {0}", (object)(LinkRemovingRule)rule);
                        break;
                    case CompRemovingRule _:
                        Logger.Debug("- {0}", (object)(CompRemovingRule)rule);
                        break;
                    default:
                        Logger.Error("ERROR IN RULE TYPE (CycleManager SetCurrentCycleRules)");
                        break;
                }
            }
        }

        public void SetCurrentAssemblyState(string beanList, string linkList)
        {
            if (beanList == null || linkList == null)
                throw new NullReferenceException("null argument in FormatingEntryBean:SetCurrentAssemblyState");
            this.listJoinPoints.Clear();
            this.currentAssembly.Clear();
            this.rulesToAck.Clear();
            Logger.Info("AADesigner: reinitializing component and link list from container.");
            char[] separator1 = new char[2] { '\r', '\n' };
            string[] strArray1 = !(beanList.Trim() == string.Empty) ? beanList.Split(separator1, StringSplitOptions.RemoveEmptyEntries) : (string[])null;
            string[] strArray2 = !(linkList.Trim() == string.Empty) ? linkList.Split(separator1, StringSplitOptions.RemoveEmptyEntries) : new string[0];
            if (strArray1 == null || strArray1.Length == 0)
            {
                Logger.Info("After resync, the assembly is empty. Reinitializing weaver.");
                this.initialAssembly.Clear();
                this.modificationsCounter = 1;
                this.FireCancelCycle(false);
                this.cycleRunning = false;
                this.StartWeavingCycle();
            }
            else
            {
                foreach (string instanceName in strArray1)
                {
                    VariableComponent component = new VariableComponent((string)null, instanceName);
                    this.listJoinPoints.Add(component);
                    CompCreationRule compCreationRule = new CompCreationRule(component, (string)null);
                    compCreationRule.OriginAA = AdviceRule.INITIAL_TAG;
                    this.currentAssembly.Add((AdviceRule)compCreationRule);
                }
                if (strArray2.Length > 0)
                {
                    foreach (string str1 in strArray2)
                    {
                        char[] separator2 = new char[1] { '-' };
                        string[] strArray3 = str1.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
                        LinkedList<string> parameters = (LinkedList<string>)null;
                        if (strArray3.Length > 5)
                        {
                            string[] separator3 = new string[1]
                            {
              LinkRule.CALLBACK_SEPARATOR
                            };
                            string[] strArray4 = strArray3[5].Split(separator3, StringSplitOptions.None);
                            if (strArray4 != null && strArray4.Length > 0)
                            {
                                parameters = new LinkedList<string>();
                                foreach (string str2 in strArray4)
                                    parameters.AddLast(str2);
                            }
                        }
                        LinkCreationRule linkCreationRule = new LinkCreationRule(new VariableComponent((string)null, strArray3[1]), strArray3[3], new VariableComponent((string)null, strArray3[2]), strArray3[4], parameters);
                        linkCreationRule.OriginAA = AdviceRule.INITIAL_TAG;
                        this.currentAssembly.Add((AdviceRule)linkCreationRule);
                    }
                }
                Logger.Debug("\t\tList of rules in current assembly ({0}):", (object)this.currentAssembly.Count);
                foreach (AdviceRule adviceRule in this.currentAssembly)
                {
                    if (adviceRule is CompCreationRule)
                        Logger.Debug("\t\t" + (object)(CompCreationRule)adviceRule);
                    else
                        Logger.Debug("\t\t" + (object)(LinkCreationRule)adviceRule);
                }
                Logger.Debug("\t\tend");
                this.ExtractInitialAssembly();
                this.modificationsCounter = 1;
                this.FireCancelCycle(false);
                this.cycleRunning = false;
                this.StartWeavingCycle();
            }
        }

        private void ExtractInitialAssembly()
        {
            this.listJoinPoints.Clear();
            this.initialAssembly.Clear();
            if (this.wovenRules == null || this.wovenRules.Count == 0)
            {
                Logger.Info("Initial assembly IS current assembly (no aspects woven).");
                foreach (AdviceRule adviceRule in this.currentAssembly)
                {
                    this.initialAssembly.Add(adviceRule);
                    if (adviceRule.Type == RuleType.ADDCOMP)
                        this.listJoinPoints.Add(((CompRule)adviceRule).Component);
                }
            }
            else
            {
                foreach (AdviceRule lRule in this.currentAssembly)
                {
                    if (lRule.Type == RuleType.ADDCOMP)
                    {
                        if (!AdviceToUPnP.CompExistsInRules(this.wovenRules, ((CompRule)lRule).Component.InstanceName))
                        {
                            this.listJoinPoints.Add(((CompRule)lRule).Component);
                            this.initialAssembly.Add(lRule);
                        }
                    }
                    else if (lRule.Type == RuleType.ADDLINK)
                    {
                        if (!AdviceToUPnP.LinkExistsInRules(this.wovenRules, (LinkRule)lRule))
                            this.initialAssembly.Add(lRule);
                    }
                    else
                        Logger.Error("------- Rule in current assembly of unknown type -------");
                }
                foreach (AdviceRule wovenRule in this.wovenRules)
                {
                    if (wovenRule is LinkRemovingRule && wovenRule.OriginAA == AdviceRule.INITIALREMOVED_TAG)
                    {
                        LinkCreationRule linkCreationRule = new LinkCreationRule((LinkRule)wovenRule);
                        linkCreationRule.OriginAA = AdviceRule.INITIAL_TAG;
                        this.initialAssembly.Add((AdviceRule)linkCreationRule);
                    }
                }
                Logger.Debug("\tList of rules in ExtractedInitialAssembly ({0}):", (object)this.initialAssembly.Count);
                foreach (AdviceRule adviceRule in this.initialAssembly)
                {
                    if (adviceRule is CompCreationRule)
                        Logger.Debug("\t" + (object)(CompCreationRule)adviceRule);
                    else
                        Logger.Debug("\t" + (object)(LinkCreationRule)adviceRule);
                }
                Logger.Debug("\tend");
            }
        }

        private void StartWeavingCycle()
        {
            if (this.cycleRunning)
            {
                this.newCycleRequested = true;
            }
            else
            {
                this.cycleRunning = true;
                try
                {
                    this.FireCurrentAssemblyEvent(this.currentAssembly);
                    this.FireInitialAssembly(this.initialAssembly);
                    this.FireJoinpointsEvent(this.listJoinPoints);
                }
                catch (Exception ex)
                {
                    Logger.Error("=== EXCEPTION CATCHED IN AA ===");
                    Logger.Error(ex.Message);
                    Logger.Debug(ex.StackTrace);
                }
                this.FireCancelCycle(false);
                this.cycleRunning = false;
                if (!this.newCycleRequested)
                    return;
                Logger.Info("Aborted weaving cycle finished, restarting new cycle.");
                this.newCycleRequested = false;
                this.StartWeavingCycle();
            }
        }

        public event CycleManager.BoolDelegate CancelWeavingCycle;

        private void FireCancelCycle(bool allow)
        {
            if (this.CancelWeavingCycle == null)
                return;
            this.CancelWeavingCycle(allow);
        }

        public event CycleManager.ListOfVCDelegate ListOfJoinpoints;

        private void FireJoinpointsEvent(List<VariableComponent> listJP)
        {
            if (this.ListOfJoinpoints == null)
                return;
            this.ListOfJoinpoints(listJP);
        }

        public event CycleManager.ListOfRulesDelegate CurrentAssemblyEvent;

        private void FireCurrentAssemblyEvent(List<AdviceRule> currentAssembly)
        {
            if (this.CurrentAssemblyEvent == null)
                return;
            this.CurrentAssemblyEvent(currentAssembly);
        }

        public event CycleManager.ListOfRulesDelegate ComputedInitialAssembly;

        private void FireInitialAssembly(List<AdviceRule> initialAssembly)
        {
            if (this.ComputedInitialAssembly == null)
                return;
            this.ComputedInitialAssembly(initialAssembly);
        }

        public event CycleManager.VoidDelegate GetContainerADL;

        private void FireGetADL()
        {
            if (this.GetContainerADL == null)
                return;
            this.GetContainerADL();
        }

        public event CycleManager.RuntimeErrorDelegate RuntimeError;

        private void FireRuntimeError(string aa, string msg)
        {
            if (this.RuntimeError == null)
                return;
            this.RuntimeError(aa, msg);
        }

        public void ContainerEventSink(string msg)
        {
            AdviceRule adviceRule1 = (AdviceRule)null;
            if (this.modificationsCounter == -1)
            {
                this.modificationsCounter = 0;
                this.FireGetADL();
            }
            else
            {
                if (this.modificationsCounter < 1)
                    return;
                checked { ++this.modificationsCounter; }
                string[] strArray = msg.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length <= 2)
                    return;
                string instName;
                LinkCreationRule lcr;
                switch (strArray[1])
                {
                    case "NEW_BEAN":
                        instName = strArray[2];
                        string type = strArray[3];
                        if (AdviceToUPnP.CompExistsInRules(this.currentAssembly, instName))
                        {
                            Logger.Debug("New bean already exists in current assembly, dropping event");
                            break;
                        }
                        CompCreationRule compCreationRule1 = new CompCreationRule(new VariableComponent((string)null, instName), type);
                        compCreationRule1.OriginAA = AdviceRule.INITIAL_TAG;
                        Logger.Debug("-AA- adding bean to current assembly: {0}", (object)strArray[2]);
                        this.currentAssembly.Add((AdviceRule)compCreationRule1);
                        foreach (AdviceRule adviceRule2 in this.rulesToAck)
                        {
                            if (adviceRule2.Type == RuleType.ADDCOMP)
                            {
                                CompCreationRule compCreationRule2 = (CompCreationRule)adviceRule2;
                                if (compCreationRule2.ComponentType == type && compCreationRule2.Component.InstanceName == instName)
                                {
                                    adviceRule1 = adviceRule2;
                                    break;
                                }
                            }
                        }
                        if (adviceRule1 != null)
                        {
                            this.rulesToAck.Remove(adviceRule1);
                            Logger.Debug("-AA- acknowledged bean instanciation from container ({0} left)", (object)this.rulesToAck.Count);
                            if (this.rulesToAck.Count == 0)
                            {
                                this.FireCancelCycle(false);
                                Logger.Info("CYCLE ACKNOWLEDGED\r\n\r\n");
                                break;
                            }
                            break;
                        }
                        if (this.cycleRunning)
                            this.FireCancelCycle(true);
                        if (!this.wovenRules.Exists((Predicate<AdviceRule>)(r => r is CompRule && ((CompRule)r).Component.InstanceName == instName)))
                        {
                            if (AdviceToUPnP.CompExistsInRules(this.initialAssembly, instName))
                            {
                                Logger.Debug("BEAN ALREADY EXISTS IN INITIAL ASSEMBLY ({0})", (object)this.initialAssembly.Count);
                                break;
                            }
                            Logger.Info("-AA- Adding new component to initial assembly: {0}", (object)instName);
                            this.listJoinPoints.Add(compCreationRule1.Component);
                            this.initialAssembly.Add((AdviceRule)compCreationRule1);
                            this.StartWeavingCycle();
                        }
                        break;
                    case "DEL_BEAN":
                        instName = strArray[2];
                        int index1 = this.currentAssembly.FindIndex((Predicate<AdviceRule>)(r => r.Type == RuleType.ADDCOMP && ((CompRule)r).Component.InstanceName == instName));
                        if (-1 == index1)
                        {
                            Logger.Debug("\r\n\r\n\tUNKNOWN COMPONENT REMOVE from current: {0}\r\n\r\n", (object)strArray[2]);
                            break;
                        }
                        Logger.Debug("-AA- removing bean from current assembly: {0}", (object)strArray[2]);
                        this.currentAssembly.RemoveAt(index1);
                        if (this.cycleRunning)
                            this.FireCancelCycle(true);
                        foreach (AdviceRule adviceRule3 in this.rulesToAck)
                        {
                            if (adviceRule3.Type == RuleType.REMOVECOMP && ((CompRule)adviceRule3).Component.InstanceName == instName)
                            {
                                adviceRule1 = adviceRule3;
                                break;
                            }
                        }
                        if (adviceRule1 != null)
                        {
                            this.rulesToAck.Remove(adviceRule1);
                            Logger.Debug("-AA- acknowledged bean deletion from container ({0} left)", (object)this.rulesToAck.Count);
                            if (this.rulesToAck.Count == 0)
                            {
                                this.FireCancelCycle(false);
                                Logger.Info("CYCLE ACKNOWLEDGED\r\n\r\n");
                                break;
                            }
                            break;
                        }
                        if (!this.wovenRules.Exists((Predicate<AdviceRule>)(r => r is CompRule && ((CompRule)r).Component.InstanceName == instName)))
                        {
                            Logger.Info("-AA- removing component from initial assembly: {0} ({1} rules)", (object)instName, (object)this.initialAssembly.Count);
                            int index2 = this.initialAssembly.FindIndex((Predicate<AdviceRule>)(r => r.Type == RuleType.ADDCOMP && ((CompRule)r).Component.InstanceName == instName));
                            if (index2 == -1)
                            {
                                Logger.Debug("\r\n\r\n\tUNKNOWN COMPONENT REMOVE from initial: {0} (not in woven rules or rules to ack either)\r\n\r\n", (object)instName);
                            }
                            else
                            {
                                AdviceRule adviceRule4 = this.initialAssembly[index2];
                                if (adviceRule4.OriginAA != AdviceRule.INITIAL_TAG)
                                    Logger.Warn("\r\nINITIAL RULE HAS NO INITIAL TAG! ==========================\r\n");
                                this.listJoinPoints.Remove(((CompRule)adviceRule4).Component);
                                this.initialAssembly.RemoveAt(index2);
                            }
                        }
                        this.StartWeavingCycle();
                        break;
                    case "NEW_LINK":
                        lcr = LinkRule.ParseLink(strArray[2]);
                        if (AdviceToUPnP.LinkExistsInRules(this.currentAssembly, (LinkRule)lcr))
                        {
                            Logger.Debug("New bean already exists in current assembly, dropping event");
                            break;
                        }
                        Logger.Debug("-AA- adding link to current assembly: {0}", (object)strArray[2]);
                        this.currentAssembly.Add((AdviceRule)lcr);
                        foreach (AdviceRule r in this.rulesToAck)
                        {
                            if (r.Type == RuleType.ADDLINK && lcr.isSameRule(r))
                            {
                                adviceRule1 = r;
                                break;
                            }
                        }
                        if (adviceRule1 != null)
                        {
                            this.rulesToAck.Remove(adviceRule1);
                            Logger.Debug("-AA- acknowledged link creation from container ({0} left)", (object)this.rulesToAck.Count);
                            if (this.rulesToAck.Count == 0)
                            {
                                this.FireCancelCycle(false);
                                Logger.Info("CYCLE ACKNOWLEDGED\r\n\r\n");
                                break;
                            }
                            break;
                        }
                        if (this.cycleRunning)
                            this.FireCancelCycle(true);
                        if (!this.wovenRules.Exists((Predicate<AdviceRule>)(rule => (rule.Type == RuleType.ADDLINK || rule.Type == RuleType.REMOVELINK) && lcr.isSameRule(rule))) && !AdviceToUPnP.LinkExistsInRules(this.initialAssembly, (LinkRule)lcr))
                        {
                            Logger.Info("-AA- Adding new link to initial assembly: {0}", (object)strArray[2]);
                            lcr.OriginAA = AdviceRule.INITIAL_TAG;
                            this.initialAssembly.Add((AdviceRule)lcr);
                            this.StartWeavingCycle();
                        }
                        break;
                    case "DEL_LINK":
                        lcr = LinkRule.ParseLink(strArray[2]);
                        if (!AdviceToUPnP.LinkExistsInRules(this.currentAssembly, (LinkRule)lcr))
                        {
                            Logger.Debug("Removed link already not exists in current assembly, dropping event");
                            break;
                        }
                        int index3 = this.currentAssembly.FindIndex((Predicate<AdviceRule>)(r => r.Type == RuleType.ADDLINK && lcr.isSameRule(r)));
                        if (-1 == index3)
                        {
                            Logger.Debug("\r\n\r\n\tUNKNOWN LINK REMOVE from current: {0}\r\n\r\n", (object)strArray[2]);
                        }
                        else
                        {
                            Logger.Debug("-AA- removing link from current assembly: {0}", (object)strArray[2]);
                            this.currentAssembly.RemoveAt(index3);
                            if (this.cycleRunning)
                                this.FireCancelCycle(true);
                        }
                        foreach (AdviceRule r in this.rulesToAck)
                        {
                            if (r.Type == RuleType.REMOVELINK && lcr.isSameRule(r))
                            {
                                adviceRule1 = r;
                                break;
                            }
                        }
                        if (adviceRule1 != null)
                        {
                            this.rulesToAck.Remove(adviceRule1);
                            Logger.Debug("-AA- acknowledged link deletion from container ({0} left)", (object)this.rulesToAck.Count);
                            if (this.rulesToAck.Count == 0)
                            {
                                this.FireCancelCycle(false);
                                Logger.Info("CYCLE ACKNOWLEDGED\r\n\r\n");
                                break;
                            }
                            break;
                        }
                        if (!this.wovenRules.Exists((Predicate<AdviceRule>)(rule => (rule.Type == RuleType.ADDLINK || rule.Type == RuleType.REMOVELINK) && lcr.isSameRule(rule))))
                        {
                            Logger.Info("-AA- removing link from initial assembly: {0} ({1} rules)", (object)strArray[2], (object)this.initialAssembly.Count);
                            int index4 = this.initialAssembly.FindIndex((Predicate<AdviceRule>)(r => r.Type == RuleType.ADDLINK && lcr.isSameRule(r)));
                            if (-1 == index4)
                                Logger.Debug("UNKNOWN LINK REMOVE from initial: {0} (not in woven rules or rules to ack either)", (object)strArray[2]);
                            else
                                this.initialAssembly.RemoveAt(index4);
                        }
                        break;
                    case "ERROR":
                        if (strArray[2] == "NEW_BEAN" && strArray[3].StartsWith("could not instantiate component") && strArray[3].Contains("because its type") && strArray[3].EndsWith("cannot be found."))
                        {
                            int startIndex = checked(strArray[3].IndexOf('(') + 1);
                            int num = strArray[3].IndexOf(')');
                            if (startIndex == -1 || num == -1 || num < startIndex)
                                break;
                            string str = strArray[3].Substring(startIndex, checked(num - startIndex));
                            foreach (AdviceRule wovenRule in this.wovenRules)
                            {
                                if (wovenRule.Type == RuleType.ADDCOMP && ((CompRule)wovenRule).ComponentType == str)
                                {
                                    int length = wovenRule.OriginAA.IndexOf('#');
                                    string aa = length == -1 ? wovenRule.OriginAA : wovenRule.OriginAA.Substring(0, length);
                                    this.FireRuntimeError(aa, $"The component type {str} for local component creation in AA {aa} does not exist.");
                                }
                            }
                        }
                        else if (strArray[2] == "LINK" && (strArray[3].StartsWith("destination method does not exist: ") || strArray[3].StartsWith("destination method does not exist with these parameters: ") || strArray[3].StartsWith("param does not exist: ") || strArray[3].StartsWith("source event does not exist: ")))
                        {
                            int num = strArray[3].IndexOf(':');
                            if (num == -1)
                                break;
                            string str1 = strArray[3].Substring(checked(num + 2));
                            int length1 = str1.LastIndexOf('.');
                            string str2 = str1.Substring(0, length1);
                            if (str1[checked(length1 + 1)] == '^')
                                checked { ++length1; }
                            string str3 = str1.Substring(checked(length1 + 1));
                            LinkCreationRule linkCreationRule1 = (LinkCreationRule)null;
                            foreach (AdviceRule wovenRule in this.wovenRules)
                            {
                                if (wovenRule.Type == RuleType.ADDLINK)
                                {
                                    LinkCreationRule linkCreationRule2 = (LinkCreationRule)wovenRule;
                                    if (strArray[3].StartsWith("destination") && linkCreationRule2.DestinationComponent.InstanceName == str2 && linkCreationRule2.DestinationMethod == str3)
                                    {
                                        linkCreationRule1 = linkCreationRule2;
                                        break;
                                    }
                                    if (strArray[3].StartsWith("source") && linkCreationRule2.SourceComponent.InstanceName == str2 && linkCreationRule2.SourceEvent == str3)
                                    {
                                        linkCreationRule1 = linkCreationRule2;
                                        break;
                                    }
                                    if (strArray[3].StartsWith("param") && linkCreationRule2.SourceComponent.OrigName == str2)
                                    {
                                        foreach (string parameter in linkCreationRule2.Parameters)
                                        {
                                            if (parameter == str3)
                                            {
                                                linkCreationRule1 = linkCreationRule2;
                                                break;
                                            }
                                        }
                                        if (linkCreationRule1 != null)
                                            break;
                                    }
                                }
                            }
                            if (linkCreationRule1 != null)
                            {
                                int length2 = linkCreationRule1.OriginAA.IndexOf('#');
                                string aa = length2 == -1 ? linkCreationRule1.OriginAA : linkCreationRule1.OriginAA.Substring(0, length2);
                                this.FireRuntimeError(aa, $"The port {str1} used in AA {aa} does not exist");
                            }
                        }
                        if (!this.cycleRunning)
                        {
                            this.cycleRunning = true;
                            Logger.Warn("CONTAINER SENT ERROR, RESYNCING WEAVER");
                            this.FireGetADL();
                            break;
                        }
                        break;
                    case "NOTICE":
                        if (strArray[2] == "NEWNAME" && strArray.Length == 5)
                        {
                            if (this.cycleRunning)
                                this.FireCancelCycle(true);
                            foreach (AdviceRule adviceRule5 in this.currentAssembly)
                            {
                                if (adviceRule5.Type == RuleType.ADDCOMP && strArray[3] == ((CompRule)adviceRule5).Component.InstanceName)
                                    ((CompRule)adviceRule5).Component.InstanceName = strArray[4];
                                else if (adviceRule5.Type == RuleType.ADDLINK)
                                {
                                    LinkCreationRule linkCreationRule = (LinkCreationRule)adviceRule5;
                                    if (linkCreationRule.SourceComponent.InstanceName == strArray[3])
                                        linkCreationRule.SourceComponent.InstanceName = strArray[4];
                                    if (linkCreationRule.DestinationComponent.InstanceName == strArray[3])
                                        linkCreationRule.DestinationComponent.InstanceName = strArray[4];
                                }
                            }
                            foreach (AdviceRule adviceRule6 in this.initialAssembly)
                            {
                                if (adviceRule6.Type == RuleType.ADDCOMP && strArray[3] == ((CompRule)adviceRule6).Component.InstanceName)
                                    ((CompRule)adviceRule6).Component.InstanceName = strArray[4];
                                else if (adviceRule6.Type == RuleType.ADDLINK)
                                {
                                    LinkCreationRule linkCreationRule = (LinkCreationRule)adviceRule6;
                                    if (linkCreationRule.SourceComponent.InstanceName == strArray[3])
                                        linkCreationRule.SourceComponent.InstanceName = strArray[4];
                                    if (linkCreationRule.DestinationComponent.InstanceName == strArray[3])
                                        linkCreationRule.DestinationComponent.InstanceName = strArray[4];
                                }
                            }
                            foreach (AdviceRule wovenRule in this.wovenRules)
                            {
                                if (wovenRule is LinkRule)
                                {
                                    LinkRule linkRule = (LinkRule)wovenRule;
                                    if (linkRule.SourceComponent.InstanceName == strArray[3])
                                        linkRule.SourceComponent.InstanceName = strArray[4];
                                    if (linkRule.DestinationComponent.InstanceName == strArray[3])
                                        linkRule.DestinationComponent.InstanceName = strArray[4];
                                }
                            }
                            this.StartWeavingCycle();
                            break;
                        }
                        break;
                    default:
                        Logger.Warn("AA CYCLE MANAGER: NOT HANDLED CONTAINER EVENT: {0}", (object)msg);
                        break;
                }
            }
        }

        public delegate void VoidDelegate();

        public delegate void BoolDelegate(bool b);

        public delegate void ListOfVCDelegate(List<VariableComponent> comps);

        public delegate void ListOfRulesDelegate(List<AdviceRule> rules);

        public delegate void RuntimeErrorDelegate(string aa, string msg);
    }
}