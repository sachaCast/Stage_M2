// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.AdviceToUPnP
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

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class AdviceToUPnP
{
  private List<AdviceRule> currentAdvice;
  private List<AdviceRule> previousAdvice;
  private LinkedList<AdviceRule> actualRules;
  private LinkedList<SetPropertyRule> propertyRules;
  private bool grouping;
  private bool stopCycle;
  private List<AdviceRule> currentAssembly;
  private StringBuilder outputAdaptationSchema;

  public AdviceToUPnP()
  {
    this.currentAdvice = new List<AdviceRule>();
    this.actualRules = new LinkedList<AdviceRule>();
    this.propertyRules = new LinkedList<SetPropertyRule>();
    this.previousAdvice = (List<AdviceRule>) null;
    this.currentAssembly = (List<AdviceRule>) null;
    this.stopCycle = false;
    this.grouping = true;
    this.outputAdaptationSchema = new StringBuilder();
    this.outputAdaptationSchema.Append("<adaptation_schema>");
  }

  public List<AdviceRule> CurrentAssembly
  {
    get => this.currentAssembly;
    set
    {
      this.currentAssembly = value;
      if (this.currentAssembly == null)
        return;
      Logger.Debug("Current assembly SET in AdviceToUPnP. {0} rules", (object) this.currentAssembly.Count);
    }
  }

  [DefaultValue(false)]
  public bool StopCycle
  {
    get => this.stopCycle;
    set
    {
      if (value)
        Logger.Debug("{0}STOPPING CYCLE (AdviceToUPnP)", (object) Environment.NewLine);
      else
        Logger.Debug("StopCycle = false");
      this.stopCycle = value;
    }
  }

  [DefaultValue(true)]
  public bool GroupAdaptation
  {
    get => this.grouping;
    set => this.grouping = value;
  }

  public void SetInputRules(List<AdviceRule> rules)
  {
    if (rules == null)
      Logger.Warn("ERROR: NULL RULES in AdviceToUPnP. Doing nothing");
    else if (this.stopCycle)
    {
      Logger.Debug("Blocking cycle in AdviceToUPnP");
    }
    else
    {
      Logger.Debug("--> AdviceToUPnP entry: {0} rules received.", (object) rules.Count);
      if (rules.Count > 0)
      {
        foreach (AdviceRule rule in rules)
        {
          switch (rule)
          {
            case CompCreationRule _:
              Logger.Debug("+ {0}", (object) (CompCreationRule) rule);
              break;
            case LinkCreationRule _:
              Logger.Debug("+ {0}", (object) (LinkCreationRule) rule);
              break;
            case SetPropertyRule _:
              Logger.Debug("  {0}", (object) (SetPropertyRule) rule);
              break;
            case LinkRemovingRule _:
              Logger.Debug("- {0}", (object) (LinkRemovingRule) rule);
              break;
            case CompRemovingRule _:
              Logger.Debug("- {0}", (object) (CompRemovingRule) rule);
              break;
            default:
              Logger.Error("ERROR IN RULE TYPE (AdviceToUPnP input)");
              break;
          }
        }
      }
      this.previousAdvice = this.currentAdvice;
      this.currentAdvice = rules;
      if (rules.Count == 0)
      {
        Logger.Debug("AdviceToUPnP: removing all AA detected.");
        if (this.previousAdvice.Count != 0)
          this.removeAll();
        else
          Logger.Debug("Nothing to remove, previous list of rules is empty.");
        Logger.Info("<-- AdviceToUPnP short exit (removing all from previousAdvice)");
        this.CommitActualRules();
        this.CommitAdaptationSchema();
      }
      else
      {
        bool[] flagArray = (bool[]) null;
        if (this.previousAdvice.Count > 0)
          flagArray = new bool[this.previousAdvice.Count];
        Logger.Info("Applying new (non-existing) rules");
        foreach (AdviceRule rule in rules)
        {
          int index = 0;
          while (index < this.previousAdvice.Count)
          {
            AdviceRule r = this.previousAdvice[index];
            if (!flagArray[index] && rule.Type == r.Type && rule.isSameRule(r))
            {
              Logger.Debug("Detected rule {0} already woven, won't weave it again (type = {1}).", (object) index, (object) rule.Type);
              flagArray[index] = true;
              break;
            }
            checked { ++index; }
          }
          if (rule.Type == RuleType.REMOVELINK && rule.OriginAA == AdviceRule.INITIALREMOVED_TAG)
          {
            LinkRemovingRule lRule = (LinkRemovingRule) rule;
            if (!this.currentAssembly.Exists((Predicate<AdviceRule>) (r =>
            {
              if (r.Type != RuleType.ADDLINK)
                return false;
              LinkRule r1 = (LinkRule) r;
              return r1.SourceComponent.OrigName == lRule.SourceComponent.InstanceName && r1.OrigEvent == lRule.SourceEvent && r1.DestinationComponent.OrigName == lRule.DestinationComponent.InstanceName && r1.OrigMethod == lRule.DestinationMethod && LinkRule.haveOriginallySameParameters(r1, (LinkRule) lRule);
            })))
            {
              Logger.Debug("Link removal {0}.^{1} -> {2}.{3} was already applied.", (object) lRule.SourceComponent.InstanceName, (object) lRule.SourceEvent, (object) lRule.DestinationComponent.InstanceName, (object) lRule.DestinationMethod);
              continue;
            }
          }
          if (index >= this.previousAdvice.Count || !flagArray[index])
          {
            switch (rule)
            {
              case LinkCreationRule _:
                this.CreateLink((LinkRule) rule);
                break;
              case CompCreationRule _:
                this.CreateComponent((CompRule) rule);
                break;
              case LinkRemovingRule _:
                this.RemoveLink((LinkRule) rule);
                break;
              case CompRemovingRule _:
                this.RemoveComponent((CompRule) rule);
                break;
              case SetPropertyRule _:
                this.SetProperty((SetPropertyRule) rule);
                break;
              default:
                throw new ApplicationException("Not Implemented rule type - " + rule.ToString());
            }
          }
        }
        Logger.Info("Removing rules from deselected/deactivated AAs ({0} rules in previous)", (object) this.previousAdvice.Count);
        int index1 = 0;
        while (index1 < this.previousAdvice.Count)
        {
          if (!flagArray[index1])
          {
            AdviceRule adviceRule1 = this.previousAdvice[index1];
            if (adviceRule1.Type == RuleType.ADDLINK)
            {
              LinkCreationRule linkCreationRule = (LinkCreationRule) adviceRule1;
              if (this.LinkExistsInAssembly((LinkRule) linkCreationRule))
              {
                Logger.Debug("Link {0}.^{1} -> {2}.{3} has to be removed from assembly.", (object) linkCreationRule.SourceComponent.InstanceName, (object) linkCreationRule.SourceEvent, (object) linkCreationRule.DestinationComponent.InstanceName, (object) linkCreationRule.DestinationMethod);
                this.RemoveLink((LinkRule) new LinkRemovingRule((LinkRule) linkCreationRule));
              }
              else
                Logger.Debug("Link already removed, don't remove again");
            }
            else if (adviceRule1.Type == RuleType.ADDCOMP)
            {
              if (this.CompExistsInAssembly((CompRule) adviceRule1))
              {
                Logger.Debug("Component {0} has to be removed from assembly.", (object) ((CompRule) adviceRule1).Component.InstanceName);
                this.RemoveComponent((CompRule) new CompRemovingRule((CompRule) adviceRule1));
                string instanceName = ((CompRule) adviceRule1).Component.InstanceName;
                int index2 = checked (this.previousAdvice.Count - 1);
                while (index1 < index2)
                {
                  if (!flagArray[index2])
                  {
                    AdviceRule adviceRule2 = this.previousAdvice[index2];
                    if (adviceRule2 is LinkCreationRule && (((LinkRule) adviceRule2).SourceComponent.InstanceName == instanceName || ((LinkRule) adviceRule2).DestinationComponent.InstanceName == instanceName))
                      flagArray[index2] = true;
                  }
                  checked { --index2; }
                }
              }
              else
                Logger.Debug("Component already removed, don't remove again");
            }
            else if (adviceRule1 is LinkRemovingRule && !this.LinkExistsInAssembly((LinkRule) adviceRule1))
              this.CreateLink((LinkRule) new LinkCreationRule((LinkRule) adviceRule1));
            else if (!(adviceRule1 is SetPropertyRule))
              Logger.Error("ERROR: NOT HANDLED RULE TYPE in AdviceToUPnP.SetInputRules()");
          }
          checked { ++index1; }
        }
        this.CommitActualRules();
        this.CommitAdaptationSchema();
        Logger.Info("<-- AdviceToUPnP exit");
      }
    }
  }

  private void removeAll()
  {
    if (this.stopCycle)
    {
      Logger.Debug("RemoveAll: stopCycle is true, not removing anything.");
    }
    else
    {
      Logger.Debug("Removing {0} rules from previous known rules", (object) this.previousAdvice.Count);
      foreach (AdviceRule adviceRule in this.previousAdvice)
      {
        if (adviceRule is LinkCreationRule && this.LinkExistsInAssembly((LinkRule) adviceRule))
          this.RemoveLink((LinkRule) new LinkRemovingRule((LinkRule) adviceRule));
        else if (adviceRule is LinkRemovingRule && !this.LinkExistsInAssembly((LinkRule) adviceRule))
          this.CreateLink((LinkRule) new LinkCreationRule((LinkRule) adviceRule));
        else if (adviceRule is CompRemovingRule && !this.CompExistsInAssembly((CompRule) adviceRule))
          this.CreateComponent((CompRule) new CompCreationRule((CompRule) adviceRule));
      }
      foreach (AdviceRule adviceRule in this.previousAdvice)
      {
        if (adviceRule is CompCreationRule && this.CompExistsInAssembly((CompRule) adviceRule))
          this.RemoveComponent((CompRule) new CompRemovingRule((CompRule) adviceRule));
      }
    }
  }

  public static bool LinkExistsInRules(List<AdviceRule> rules, LinkRule lRule)
  {
    return rules.Exists((Predicate<AdviceRule>) (r => r.Type == RuleType.ADDLINK && lRule.isSameRule(r)));
  }

  private bool LinkExistsInAssembly(LinkRule lRule)
  {
    return AdviceToUPnP.LinkExistsInRules(this.currentAssembly, lRule);
  }

  public static bool CompExistsInRules(List<AdviceRule> rules, string compName)
  {
    return rules.Exists((Predicate<AdviceRule>) (r => r.Type == RuleType.ADDCOMP && compName == ((CompRule) r).Component.InstanceName));
  }

  public static bool CompExistsInRules(List<AdviceRule> rules, CompRule cRule)
  {
    return AdviceToUPnP.CompExistsInRules(rules, cRule.Component.InstanceName);
  }

  private bool CompExistsInAssembly(string compName)
  {
    return AdviceToUPnP.CompExistsInRules(this.currentAssembly, compName);
  }

  private bool CompExistsInAssembly(CompRule cRule)
  {
    return AdviceToUPnP.CompExistsInRules(this.currentAssembly, cRule);
  }

  private void CreateLink(LinkRule rule)
  {
    string dstParams = (string) null;
    if (rule.GetNbParameters() > 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(rule.Parameters.First.Value);
      for (LinkedListNode<string> next = rule.Parameters.First.Next; next != null; next = next.Next)
        stringBuilder.Append("|").Append(next.Value);
      dstParams = stringBuilder.ToString();
    }
    Logger.Debug($"Creating link {rule.SourceComponent.InstanceName}.^{rule.SourceEvent} {dstParams ?? ""} -> {rule.DestinationComponent.InstanceName}.{rule.DestinationMethod}");
    this.actualRules.AddLast((AdviceRule) rule);
    if (this.grouping)
    {
      this.outputAdaptationSchema.AppendFormat("<link><source>{0}</source><event>{1}</event>", (object) HttpUtility.HtmlEncode(rule.SourceComponent.InstanceName), (object) rule.SourceEvent);
      this.outputAdaptationSchema.AppendFormat("<destination>{0}</destination><method>{1}</method>", (object) HttpUtility.HtmlEncode(rule.DestinationComponent.InstanceName), (object) rule.DestinationMethod);
      this.outputAdaptationSchema.AppendFormat("<callbacks>{0}</callbacks></link>", (object) dstParams);
    }
    else
    {
      if (this.stopCycle)
        return;
      this.FireCreateLinkEvent(HttpUtility.HtmlEncode(rule.SourceComponent.InstanceName), rule.SourceEvent, HttpUtility.HtmlEncode(rule.DestinationComponent.InstanceName), rule.DestinationMethod, dstParams);
    }
  }

  private void RemoveLink(LinkRule rule)
  {
    string s = $"link-{rule.SourceComponent.InstanceName}-{rule.DestinationComponent.InstanceName}-{rule.SourceEvent}-{rule.DestinationMethod}-";
    if (rule.GetNbParameters() > 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(rule.Parameters.First.Value);
      for (LinkedListNode<string> next = rule.Parameters.First.Next; next != null; next = next.Next)
        stringBuilder.Append("|").Append(next.Value);
      s += stringBuilder.ToString();
      Logger.Debug("linkName for removal: {0}", (object) s);
    }
    this.actualRules.AddLast((AdviceRule) rule);
    string linkName = HttpUtility.HtmlEncode(s);
    if (this.grouping)
    {
      this.outputAdaptationSchema.AppendFormat("<unlink name=\"{0}\" />", (object) linkName);
    }
    else
    {
      if (this.stopCycle)
        return;
      this.FireRemoveLinkEvent(linkName);
    }
  }

  private void CreateComponent(CompRule rule)
  {
    this.actualRules.AddLast((AdviceRule) rule);
    if (this.grouping)
    {
      this.outputAdaptationSchema.AppendFormat("<instance type=\"{0}\" name=\"{1}\" />", (object) rule.ComponentType, (object) HttpUtility.HtmlEncode(rule.Component.InstanceName));
    }
    else
    {
      if (this.stopCycle)
        return;
      this.FireCreateComponentEvent(rule.ComponentType, HttpUtility.HtmlEncode(rule.Component.InstanceName));
    }
  }

  private void RemoveComponent(CompRule rule)
  {
    this.actualRules.AddLast((AdviceRule) rule);
    if (this.grouping)
    {
      this.outputAdaptationSchema.AppendFormat("<delete name=\"{0}\" />", (object) HttpUtility.HtmlEncode(rule.Component.InstanceName));
    }
    else
    {
      if (this.stopCycle)
        return;
      this.FireRemoveComponentEvent(rule.Component.InstanceName);
    }
  }

  private void SetProperty(SetPropertyRule rule)
  {
    if (this.grouping)
      this.propertyRules.AddLast(rule);
    else
      this.DoSetProperty(rule);
  }

  private void DoSetProperty(SetPropertyRule rule)
  {
    if (this.stopCycle || rule.Component.InstanceName == null || rule.PropertyValue == null)
      return;
    Logger.Info("Setting property {0} in component {1}", (object) rule.PropertyName, (object) rule.Component.InstanceName);
    this.FireSetBeanProp(HttpUtility.HtmlEncode(rule.Component.InstanceName), rule.PropertyName, rule.PropertyValue);
  }

  private void CommitActualRules()
  {
    if (!this.stopCycle)
    {
      this.FireActualRules();
    }
    else
    {
      Logger.Info("Cycle stopped after rule list creation in AdviceToUPnP. Reinitializing previous rules.");
      this.currentAdvice = this.previousAdvice;
    }
    this.actualRules = new LinkedList<AdviceRule>();
  }

  private void CommitAdaptationSchema()
  {
    if (!this.grouping || this.outputAdaptationSchema.ToString() == "<adaptation_schema>")
      return;
    this.outputAdaptationSchema.Append("</adaptation_schema>");
    if (!this.stopCycle)
    {
      this.FireSetAdaptationSchema(this.outputAdaptationSchema.ToString());
      foreach (SetPropertyRule propertyRule in this.propertyRules)
        this.DoSetProperty(propertyRule);
    }
    this.propertyRules.Clear();
    this.outputAdaptationSchema = new StringBuilder();
    this.outputAdaptationSchema.Append("<adaptation_schema>");
  }

  public event AdviceToUPnP.EventCreateLinkHandler CreateLink_event;

  private void FireCreateLinkEvent(
    string source,
    string srcEvent,
    string destination,
    string dstAction,
    string dstParams)
  {
    if (this.CreateLink_event == null)
      return;
    this.CreateLink_event(source, srcEvent, destination, dstAction, dstParams);
  }

  public event AdviceToUPnP.EventRemoveLinkHandler RemoveLink_event;

  private void FireRemoveLinkEvent(string linkName)
  {
    if (this.RemoveLink_event == null)
      return;
    this.RemoveLink_event(linkName);
  }

  public event AdviceToUPnP.EventCreateComponentHandler CreateComponent_event;

  private void FireCreateComponentEvent(string beanType, string beanName)
  {
    if (this.CreateComponent_event == null)
      return;
    this.CreateComponent_event(beanType, beanName);
  }

  public event AdviceToUPnP.EventRemoveComponentHandler RemoveComponent_event;

  private void FireRemoveComponentEvent(string instName)
  {
    if (this.RemoveComponent_event == null)
      return;
    this.RemoveComponent_event(instName);
  }

  public event AdviceToUPnP.SetBeanPropHandler SetBeanProperty_event;

  private void FireSetBeanProp(string inst, string pname, string pval)
  {
    if (this.SetBeanProperty_event == null)
      return;
    this.SetBeanProperty_event(inst, pname, pval);
  }

  public event AdviceToUPnP.EventAdaptationSchemaHandler SetAdaptationSchema_event;

  private void FireSetAdaptationSchema(string schema)
  {
    if (this.SetAdaptationSchema_event == null)
      return;
    this.SetAdaptationSchema_event(schema);
  }

  public event AdviceToUPnP.EventActualRulesHandler ActualRules_event;

  private void FireActualRules()
  {
    if (this.ActualRules_event == null || this.actualRules.Count <= 0)
      return;
    this.ActualRules_event(this.actualRules);
  }

  public delegate void EventCreateLinkHandler(
    string source,
    string srcEvent,
    string destination,
    string dstAction,
    string dstParams);

  public delegate void EventRemoveLinkHandler(string linkName);

  public delegate void EventCreateComponentHandler(string beanType, string beanName);

  public delegate void EventRemoveComponentHandler(string instName);

  public delegate void SetBeanPropHandler(string instanceName, string propName, string propVal);

  public delegate void EventAdaptationSchemaHandler(string schema);

  public delegate void EventActualRulesHandler(LinkedList<AdviceRule> rules);
}
