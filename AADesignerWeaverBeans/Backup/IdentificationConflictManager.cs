// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.IdentificationConflictManager
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections;
using System.Collections.Generic;
using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class IdentificationConflictManager
{
  private List<AdviceRule> outputComponentRules;
  private List<AdviceRule> outputLinkRules;
  private List<AdviceRule> initialAssembly;
  private Hashtable logicProperties;
  private readonly string conflictBeanType = "WComp.AADesigner.ConflictBean";
  private readonly string conflictBeanEvent = "Out";
  private readonly string conflictBeanMethod = "Inp";
  private readonly string orBeanType = "WComp.Logic.OR";
  private readonly string orBeanEvent = "ComposedOR";
  private readonly string andBeanType = "WComp.Logic.AND";
  private readonly string andBeanEvent = "ComposedAND";
  private readonly string logicBeanMethod = "In{0}";
  private readonly string logicBeanProperty = "nb_input";
  private readonly string filterType = "WComp.BasicBeans.Filtre";
  private readonly string filterInput = "Input";
  private readonly string filterOutput = "Output";

  public IdentificationConflictManager()
  {
    this.outputComponentRules = (List<AdviceRule>) null;
    this.outputLinkRules = (List<AdviceRule>) null;
  }

  public List<AdviceRule> InitialAssembly
  {
    set
    {
      this.initialAssembly = value;
      if (this.initialAssembly == null)
        return;
      Logger.Debug("Initial assembly SET in ConflictManager ({0} rules):", (object) this.initialAssembly.Count);
      foreach (AdviceRule adviceRule in this.initialAssembly)
      {
        switch (adviceRule)
        {
          case CompCreationRule _:
            Logger.Debug("  + {0}", (object) (CompCreationRule) adviceRule);
            break;
          case LinkCreationRule _:
            Logger.Debug("  + {0}", (object) (LinkCreationRule) adviceRule);
            break;
          default:
            Logger.Error("UNKNOWN RULE TYPE! (ConflictManager)");
            break;
        }
      }
    }
    get => this.initialAssembly;
  }

  public void SetRulesAndFindConflicts(List<AdviceRule> listOfAdviceRules)
  {
    Logger.Info("--> IdentificationConflictManager {0} rules", (object) listOfAdviceRules.Count);
    if (this.initialAssembly == null)
    {
      Logger.Warn("Initial assembly is NULL, returning.");
    }
    else
    {
      this.identificationConflictFilter(listOfAdviceRules);
      Logger.Info("<-- Sending {0} merged rules", (object) checked (this.outputComponentRules.Count + this.outputLinkRules.Count));
      this.FireRulesEvent();
    }
  }

  private void identificationConflictFilter(List<AdviceRule> listOfRules)
  {
    this.outputComponentRules = new List<AdviceRule>();
    this.outputLinkRules = new List<AdviceRule>();
    this.logicProperties = new Hashtable(8);
    if (listOfRules.Count > 0)
    {
      Logger.Debug("List of rules received in conflict manager:");
      foreach (AdviceRule listOfRule in listOfRules)
      {
        switch (listOfRule)
        {
          case CompCreationRule _:
            Logger.Debug((object) (CompCreationRule) listOfRule);
            break;
          case LinkRule _:
            Logger.Debug((object) (LinkRule) listOfRule);
            break;
          case FilterRule _:
            Logger.Debug((object) (FilterRule) listOfRule);
            break;
          default:
            Logger.Debug((object) (SetPropertyRule) listOfRule);
            break;
        }
      }
    }
    LinkedList<AdviceRule> linkedList = new LinkedList<AdviceRule>();
    foreach (AdviceRule listOfRule1 in listOfRules)
    {
      if (listOfRule1 is FilterRule && !linkedList.Contains(listOfRule1))
      {
        FilterRule filterRule = (FilterRule) listOfRule1;
        VariableComponent component = new VariableComponent((string) null, filterRule.FilterComponent);
        this.outputComponentRules.Add((AdviceRule) new CompCreationRule(component, this.filterType));
        foreach (AdviceRule listOfRule2 in listOfRules)
        {
          if (listOfRule2 is LinkRule)
          {
            LinkRule linkRule = (LinkRule) listOfRule2;
            if (linkRule.DestinationComponent.InstanceName == filterRule.TargetComponent.InstanceName && linkRule.DestinationMethod == filterRule.TargetMethod)
            {
              if (listOfRule2.Type == RuleType.ADDLINK)
              {
                Logger.Info("Found a link to filter: {0}", (object) linkRule.ToString());
                linkRule.DestinationComponent = component;
                linkRule.Type = RuleType.FILTER;
              }
              else
              {
                Logger.Info("Found a link to {0}: {1}", (object) filterRule.FilterComponent, (object) linkRule.ToString());
                linkRule.DestinationComponent = component;
              }
            }
          }
          else if (listOfRule2 is FilterRule && listOfRule2 != listOfRule1 && filterRule.isSameRule(listOfRule2))
          {
            Logger.Debug("Found duplicate filter rule: {0}", (object) filterRule.ToString());
            linkedList.AddLast(listOfRule2);
          }
        }
      }
    }
    foreach (AdviceRule adviceRule in linkedList)
      listOfRules.Remove(adviceRule);
    linkedList.Clear();
    List<AdviceRule> adviceRuleList = listOfRules;
    bool[] flagArray = new bool[this.initialAssembly.Count];
    int index1 = 0;
    while (index1 < adviceRuleList.Count)
    {
      AdviceRule adviceRule1 = adviceRuleList[index1];
      if (adviceRule1 is CompRule || adviceRule1 is SetPropertyRule)
      {
        this.outputComponentRules.Add(adviceRule1);
      }
      else
      {
        switch (adviceRule1)
        {
          case FilterRule _:
            FilterRule fr = (FilterRule) adviceRule1;
            LinkCreationRule r = (LinkCreationRule) this.initialAssembly.Find((Predicate<AdviceRule>) (ar => ar is LinkCreationRule && ((LinkRule) ar).DestinationComponent.InstanceName == fr.TargetComponent.InstanceName && ((LinkRule) ar).DestinationMethod == fr.TargetMethod));
            if (r != null)
            {
              Logger.Info("Rewriting link {0} through filter {1}", (object) r.ToString(), (object) fr.FilterComponent);
              VariableComponent variableComponent = new VariableComponent((string) null, fr.FilterComponent);
              this.outputLinkRules.Add((AdviceRule) new LinkRemovingRule((LinkRule) r));
              this.outputLinkRules.Add((AdviceRule) new LinkCreationRule(variableComponent, this.filterOutput, new VariableComponent((string) null, r.DestinationComponent.InstanceName), r.DestinationMethod, (LinkedList<string>) null));
              this.outputLinkRules.Add((AdviceRule) new LinkCreationRule(new VariableComponent((string) null, r.SourceComponent.InstanceName), r.SourceEvent, variableComponent, this.filterInput, (LinkedList<string>) null));
              break;
            }
            LinkCreationRule linkCreationRule = (LinkCreationRule) listOfRules.Find((Predicate<AdviceRule>) (ar => ar is LinkCreationRule && ((LinkRule) ar).DestinationComponent.InstanceName == fr.FilterComponent && ((LinkRule) ar).DestinationMethod == fr.TargetMethod));
            if (linkCreationRule == null)
            {
              Logger.Debug("Found no link to be rewritten for FILTER on method {0}.{1}", (object) fr.TargetComponent.InstanceName, (object) fr.TargetMethod);
              break;
            }
            VariableComponent origComp = new VariableComponent((string) null, fr.FilterComponent);
            this.outputLinkRules.Add((AdviceRule) new LinkCreationRule(origComp, this.filterOutput, new VariableComponent((string) null, fr.TargetComponent.InstanceName), linkCreationRule.DestinationMethod, (LinkedList<string>) null));
            linkCreationRule.DestinationComponent = origComp;
            linkCreationRule.DestinationMethod = this.filterInput;
            break;
          case LinkCreationRule _:
            LinkCreationRule rule1 = (LinkCreationRule) adviceRule1;
            bool flag1 = rule1.SourceComponent.InstanceName.StartsWith("AA_CONFLICT_OUT");
            bool flag2 = rule1.DestinationComponent.InstanceName.StartsWith("AA_CONFLICT_IN");
            if (flag1 && flag2)
            {
              this.outputLinkRules.Add((AdviceRule) rule1);
              break;
            }
            int index2 = checked (index1 + 1);
            while (index2 < adviceRuleList.Count)
            {
              AdviceRule adviceRule2 = adviceRuleList[index2];
              if (adviceRule2 is LinkCreationRule)
              {
                LinkCreationRule rule2 = (LinkCreationRule) adviceRule2;
                bool toRemove = true;
                this.FindConflictBetween2LinkRules(rule1, rule2, ref toRemove);
              }
              checked { ++index2; }
            }
            int index3 = 0;
            while (index3 < this.initialAssembly.Count)
            {
              AdviceRule adviceRule3 = this.initialAssembly[index3];
              if (adviceRule3 is LinkCreationRule)
              {
                LinkCreationRule rule2 = (LinkCreationRule) adviceRule3;
                this.FindConflictBetween2LinkRules(rule1, rule2, ref flagArray[index3]);
              }
              checked { ++index3; }
            }
            this.outputLinkRules.Add((AdviceRule) rule1);
            break;
          default:
            Logger.Error("ERROR: should not happen, rule is not link creation nor component creation");
            break;
        }
      }
      checked { ++index1; }
    }
    foreach (DictionaryEntry logicProperty in this.logicProperties)
    {
      SetPropertyRule setPropertyRule = new SetPropertyRule(new VariableComponent((string) null, (string) logicProperty.Key), this.logicBeanProperty);
      setPropertyRule.SetIntValue((int) logicProperty.Value);
      Logger.Info("Creating SetPropertyRule for logic greybox component {0}", (object) (string) logicProperty.Key);
      this.outputComponentRules.Add((AdviceRule) setPropertyRule);
    }
    int index4 = 0;
    while (index4 < this.initialAssembly.Count)
    {
      AdviceRule adviceRule = this.initialAssembly[index4];
      if (adviceRule is LinkCreationRule)
      {
        LinkCreationRule linkCreationRule = (LinkCreationRule) adviceRule;
        if (flagArray[index4])
        {
          LinkRemovingRule linkRemovingRule = new LinkRemovingRule(new VariableComponent((string) null, linkCreationRule.SourceComponent.OrigName), linkCreationRule.OrigEvent, new VariableComponent((string) null, linkCreationRule.DestinationComponent.OrigName), linkCreationRule.OrigMethod, linkCreationRule.OrigParameters);
          linkRemovingRule.OriginAA = AdviceRule.INITIALREMOVED_TAG;
          this.outputLinkRules.Add((AdviceRule) linkRemovingRule);
          if (!linkCreationRule.SourceComponent.InstanceName.StartsWith("AA_CONFLICT_OUT") && !linkCreationRule.DestinationComponent.InstanceName.StartsWith("AA_CONFLICT_IN"))
            Logger.Debug("FOUND MODIFIED LINK TO NOT REMOVE (1)!");
        }
        if (linkCreationRule.SourceComponent.InstanceName.StartsWith("AA_CONFLICT_OUT") || linkCreationRule.DestinationComponent.InstanceName.StartsWith("AA_CONFLICT_IN"))
        {
          this.outputLinkRules.Add((AdviceRule) linkCreationRule);
          if (!flagArray[index4])
            Logger.Debug("FOUND MODIFIED LINK TO NOT REMOVE (2)!");
        }
      }
      checked { ++index4; }
    }
  }

  private void FindConflictBetween2LinkRules(
    LinkCreationRule rule1,
    LinkCreationRule rule2,
    ref bool toRemove)
  {
    if (rule1.OriginAA == rule2.OriginAA || rule1.Type == RuleType.FILTER || rule2.Type == RuleType.FILTER)
      return;
    if (rule1.SourceComponent.OrigName == rule2.SourceComponent.OrigName && rule1.OrigEvent == rule2.OrigEvent)
    {
      Logger.Debug("Found unresolved OUT conflict on {0}.^{1}", (object) rule1.SourceComponent.OrigName, (object) rule1.OrigEvent);
      if (!rule1.SourceComponent.InstanceName.StartsWith("AA_CONFLICT_OUT_"))
      {
        string instanceName = $"AA_CONFLICT_OUT_{rule1.SourceComponent.OrigName}_{rule1.OrigEvent}";
        bool flag = false;
        VariableComponent variableComponent = (VariableComponent) null;
        foreach (AdviceRule outputComponentRule in this.outputComponentRules)
        {
          if (outputComponentRule.Type == RuleType.ADDCOMP && ((CompRule) outputComponentRule).Component.InstanceName == instanceName)
          {
            flag = true;
            variableComponent = ((CompRule) outputComponentRule).Component;
            Logger.Info("Bean already created by another rule, not creating again: {0}(leads to other error messages, issue #167)", (object) instanceName);
            break;
          }
        }
        if (!flag)
        {
          variableComponent = new VariableComponent((string) null, instanceName);
          CompRule compRule = (CompRule) new CompCreationRule(variableComponent, this.conflictBeanType);
          compRule.OriginAA = $"{rule1.OriginAA}_{rule2.OriginAA}";
          this.outputComponentRules.Add((AdviceRule) compRule);
        }
        LinkCreationRule linkCreationRule = new LinkCreationRule(rule1.SourceComponent, rule1.SourceEvent, variableComponent, this.conflictBeanMethod, rule1.Parameters);
        linkCreationRule.OriginAA = $"{rule1.OriginAA}_{rule2.OriginAA}";
        this.outputLinkRules.Add((AdviceRule) linkCreationRule);
        rule1.SourceComponent = new VariableComponent(rule1.SourceComponent);
        rule1.SourceComponent.InstanceName = variableComponent.InstanceName;
        rule1.SourceEvent = this.conflictBeanEvent;
        rule1.Parameters = (LinkedList<string>) null;
      }
      if (rule2.OriginAA == AdviceRule.INITIAL_TAG)
        toRemove = true;
      rule2.SourceComponent = rule1.SourceComponent;
      rule2.SourceEvent = this.conflictBeanEvent;
      rule2.Parameters = (LinkedList<string>) null;
    }
    if (!(rule1.DestinationComponent.OrigName == rule2.DestinationComponent.OrigName) || !(rule1.OrigMethod == rule2.OrigMethod) || rule1.GetNbParameters() != rule2.GetNbParameters())
      return;
    bool flag1 = true;
    if (rule1.Parameters != null && rule2.Parameters != null)
    {
      LinkedListNode<string> linkedListNode1 = rule1.Parameters.First;
      for (LinkedListNode<string> linkedListNode2 = rule2.Parameters.First; linkedListNode1 != null && linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
      {
        if (linkedListNode1.Value != linkedListNode2.Value)
        {
          flag1 = false;
          break;
        }
        linkedListNode1 = linkedListNode1.Next;
      }
    }
    if (!flag1)
      return;
    Logger.Debug("Found unresolved IN conflict on {0}.{1}", (object) rule1.DestinationComponent.OrigName, (object) rule1.OrigMethod);
    if (rule1.Oper == Operator.OR)
    {
      Logger.Debug("Conflict is a OR rule.\r\nLink1: {0}\r\nLink2: {1}", (object) rule1.ToString(), (object) rule2.ToString());
      if (!rule1.DestinationComponent.InstanceName.StartsWith("OR_"))
      {
        string str = $"OR_{rule1.DestinationComponent.OrigName}";
        VariableComponent variableComponent = new VariableComponent((string) null, str);
        this.outputComponentRules.Add((AdviceRule) new CompCreationRule(variableComponent, this.orBeanType));
        this.outputLinkRules.Add((AdviceRule) new LinkCreationRule(variableComponent, this.orBeanEvent, new VariableComponent((string) null, rule1.DestinationComponent.InstanceName), rule1.DestinationMethod, (LinkedList<string>) null));
        rule1.DestinationComponent = new VariableComponent(rule1.DestinationComponent);
        rule1.DestinationComponent.InstanceName = variableComponent.InstanceName;
        rule1.DestinationMethod = this.GetMethodNameForNewLink(str);
      }
    }
    else if (rule1.Oper == Operator.AND)
    {
      Logger.Debug("Conflict is a AND rule.\r\nLink1: {0}\r\nLink2: {1}", (object) rule1.ToString(), (object) rule2.ToString());
      if (!rule1.DestinationComponent.InstanceName.StartsWith("AND_"))
      {
        string str = $"AND_{rule1.DestinationComponent.OrigName}";
        VariableComponent variableComponent = new VariableComponent((string) null, str);
        this.outputComponentRules.Add((AdviceRule) new CompCreationRule(variableComponent, this.andBeanType));
        this.outputLinkRules.Add((AdviceRule) new LinkCreationRule(variableComponent, this.andBeanEvent, new VariableComponent((string) null, rule1.DestinationComponent.InstanceName), rule1.DestinationMethod, (LinkedList<string>) null));
        rule1.DestinationComponent = new VariableComponent(rule1.DestinationComponent);
        rule1.DestinationComponent.InstanceName = variableComponent.InstanceName;
        rule1.DestinationMethod = this.GetMethodNameForNewLink(str);
      }
    }
    else if (!rule1.DestinationComponent.InstanceName.StartsWith("AA_CONFLICT_IN_"))
    {
      VariableComponent variableComponent = new VariableComponent((string) null, $"AA_CONFLICT_IN_{rule1.DestinationComponent.OrigName}_{rule1.OrigMethod}");
      CompRule compRule = (CompRule) new CompCreationRule(variableComponent, this.conflictBeanType);
      compRule.OriginAA = $"{rule1.OriginAA}_{rule2.OriginAA}";
      this.outputComponentRules.Add((AdviceRule) compRule);
      LinkCreationRule linkCreationRule1 = new LinkCreationRule(variableComponent, this.conflictBeanEvent, new VariableComponent((string) null, rule1.DestinationComponent.InstanceName), rule1.DestinationMethod, (LinkedList<string>) null);
      LinkCreationRule linkCreationRule2 = linkCreationRule1;
      linkCreationRule2.OriginAA = $"{linkCreationRule2.OriginAA}{rule1.OriginAA}_{rule2.OriginAA}";
      this.outputLinkRules.Add((AdviceRule) linkCreationRule1);
      rule1.DestinationComponent = new VariableComponent(rule1.DestinationComponent);
      rule1.DestinationComponent.InstanceName = variableComponent.InstanceName;
      rule1.DestinationMethod = this.conflictBeanMethod;
    }
    if (rule2.OriginAA == AdviceRule.INITIAL_TAG)
      toRemove = true;
    rule2.DestinationComponent = rule1.DestinationComponent;
    if (rule1.Oper == Operator.AND || rule1.Oper == Operator.OR)
      rule2.DestinationMethod = this.GetMethodNameForNewLink(rule1.DestinationComponent.InstanceName);
    else
      rule2.DestinationMethod = this.conflictBeanMethod;
  }

  private string GetMethodNameForNewLink(string componentName)
  {
    int num;
    if (this.logicProperties.ContainsKey((object) componentName))
    {
      num = checked ((int) this.logicProperties[(object) componentName] + 1);
      this.logicProperties[(object) componentName] = (object) num;
    }
    else
    {
      num = 1;
      this.logicProperties.Add((object) componentName, (object) 1);
    }
    return string.Format(this.logicBeanMethod, (object) num);
  }

  public event IdentificationConflictManager.EventRulesHandler rulesEvent;

  private void FireRulesEvent()
  {
    if (this.rulesEvent == null || this.outputComponentRules == null && this.outputLinkRules == null)
      return;
    if (this.outputComponentRules.Count == 0)
      this.rulesEvent(this.outputLinkRules);
    else if (this.outputLinkRules.Count == 0)
    {
      this.rulesEvent(this.outputComponentRules);
    }
    else
    {
      foreach (AdviceRule outputLinkRule in this.outputLinkRules)
        this.outputComponentRules.Add(outputLinkRule);
      this.rulesEvent(this.outputComponentRules);
    }
  }

  public delegate void EventRulesHandler(List<AdviceRule> rules);
}
