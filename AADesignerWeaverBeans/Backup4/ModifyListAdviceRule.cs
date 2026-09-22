// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.ModifyListAdviceRule
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections.Generic;
using System.Text;
using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class ModifyListAdviceRule
{
  private List<AdviceRule> received;
  private List<AdviceRule> added;
  private List<AdviceRule> savedLinks = new List<AdviceRule>();

  public List<AdviceRule> CurrentAssembly
  {
    get => this.received;
    set
    {
      this.received = value;
      if (this.received == null)
        return;
      this.FireStringReceivedAdviceRules(this.ToString(this.received));
      this.SaveLinks(this.received);
      this.AutoAdaptation(this.received);
    }
  }

  private void SaveLinks(List<AdviceRule> receivedAssembly)
  {
    foreach (AdviceRule adviceRule in receivedAssembly)
    {
      if (adviceRule is LinkCreationRule & !this.savedLinks.Contains(adviceRule))
        this.savedLinks.Add(adviceRule);
    }
    if (this.savedLinks == null)
      return;
    this.FireStringSavedLinks(this.ToString(this.savedLinks));
  }

  private void AutoAdaptation(List<AdviceRule> receivedAssembly)
  {
    this.added = new List<AdviceRule>();
    foreach (AdviceRule savedLink in this.savedLinks)
    {
      if (savedLink is LinkCreationRule)
      {
        if (this.BeansExistsInAssembly(((LinkRule) savedLink).SourceBean) && this.BeansExistsInAssembly(((LinkRule) savedLink).DestBean) && !receivedAssembly.Contains(savedLink))
          this.added.Add(savedLink);
      }
      else
        Logger.Error("Impossible to have something else than a link in savedLinks variable");
    }
    if (this.added != null)
      this.FireStringAddedAdviceRules(this.ToString(this.added));
    this.FireAddedRules(this.added);
  }

  private bool BeansExistsInAssembly(VariableComponent bean)
  {
    foreach (AdviceRule adviceRule in this.received)
    {
      if (adviceRule is CompCreationRule && ((CompRule) adviceRule).Component.InstanceName == bean.InstanceName)
        return true;
    }
    return false;
  }

  private string ToString(List<AdviceRule> lar)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (AdviceRule adviceRule in lar)
      stringBuilder.Append(adviceRule.ToHumanString()).Append(Environment.NewLine);
    return stringBuilder.ToString();
  }

  public event ModifyListAdviceRule.ListAdviceRuleEventHandler AddedRules;

  private void FireAddedRules(List<AdviceRule> lar)
  {
    if (this.AddedRules == null)
      return;
    this.AddedRules(lar);
  }

  public event StringEventHandler StringReceivedAdviceRules;

  private void FireStringReceivedAdviceRules(string str)
  {
    if (this.StringReceivedAdviceRules == null)
      return;
    this.StringReceivedAdviceRules(str);
  }

  public event StringEventHandler StringAddedAdviceRules;

  private void FireStringAddedAdviceRules(string str)
  {
    if (this.StringAddedAdviceRules == null)
      return;
    this.StringAddedAdviceRules(str);
  }

  public event StringEventHandler StringSavedLinks;

  private void FireStringSavedLinks(string str)
  {
    if (this.StringSavedLinks == null)
      return;
    this.StringSavedLinks(str);
  }

  public delegate void ListAdviceRuleEventHandler(List<AdviceRule> lar);
}
