// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.PointCutBean
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections.Generic;
using System.Threading;
using WComp.AAGrammar;
using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class PointCutBean
{
  private AspectOfAssembly AA;
  private Mutex parserMutex = new Mutex();
  public static readonly string PARSE_ERROR = "#ERROR#";
  private string error = (string) null;

  public string AADefinition
  {
    get
    {
      if (!this.parserMutex.WaitOne(10000))
        return PointCutBean.PARSE_ERROR;
      if (this.AA != null)
      {
        string aaDefinition = this.AA.ToString();
        this.parserMutex.ReleaseMutex();
        return aaDefinition;
      }
      this.parserMutex.ReleaseMutex();
      return this.error != null ? this.error : PointCutBean.PARSE_ERROR;
    }
    set
    {
      if (!this.parserMutex.WaitOne(1000))
      {
        Logger.Error("TIMEOUT EXCEEDED FOR SET AA DEFINITION - CANNOT GET MUTEX TO WRITE. Aborting.");
      }
      else
      {
        this.AA = this.GetAAFromDefinition(value);
        if (this.AA == null)
          Logger.Warn("set_AADefinition: definition is invalid");
        this.parserMutex.ReleaseMutex();
      }
    }
  }

  private AspectOfAssembly GetAAFromDefinition(string definition)
  {
    AAScanner aaScanner = new AAScanner();
    aaScanner.SetSource(definition, 0);
    AAParser aaParser = new AAParser();
    aaParser.TheScanner = aaScanner;
    Logger.Debug("Parser and Lexer initialized. Starting Parsing");
    if (aaParser.Parse())
      return aaParser.ParsedAA;
    this.error = $"{PointCutBean.PARSE_ERROR}{aaScanner.strError}";
    return (AspectOfAssembly) null;
  }

  public void SetApplicationAssembly(List<VariableComponent> beanList)
  {
    Logger.Debug(string.Empty);
    Logger.Info("--> Poincut bean for AA '{0}' ==============================================", (object) this.AA.Name);
    Logger.Debug(string.Empty);
    if (beanList == null)
      Logger.Error("----- NULL ARGUMENT TO set_ListJoinPoints -----");
    else if (this.AA == null)
    {
      Logger.Warn("Not starting pointcut matching because of the parsing error.");
    }
    else
    {
      Logger.Debug("Joinpoints: ");
      string msg = string.Empty;
      foreach (VariableComponent bean in beanList)
        msg = $"{msg}{bean.InstanceName} ";
      Logger.Debug(msg);
      this.FindAllMatchingJoinpoints(beanList);
      Logger.Info("<-- Poincut bean sending event");
      this.FireLListOfJoinpoints();
      Logger.Debug("+++ Poincut bean '{0}' stack return", (object) this.AA.Name);
    }
  }

  private bool SyntacticPointcutMatching(string beanName, string rule)
  {
    if (rule.StartsWith("*") && rule.EndsWith("*"))
      return beanName.Contains(rule.TrimStart('*').TrimEnd('*'));
    if (rule.EndsWith("*"))
      return beanName.StartsWith(rule.TrimEnd('*'));
    if (!rule.StartsWith("*"))
      return beanName == rule;
    return beanName.EndsWith(rule.TrimStart('*'));
  }

  private bool SemanticPointcutMatching(
    string componentSemantic,
    Dictionary<string, string> semanticRules)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    Logger.Debug("Proceeding to semantic matching...");
    string str1 = componentSemantic;
    char[] chArray = new char[1]{ '&' };
    foreach (string str2 in str1.Split(chArray))
    {
      int length = str2.IndexOf('=');
      int num;
      switch (length)
      {
        case -1:
        case 0:
          num = 0;
          break;
        default:
          num = length < checked (str2.Length - 1) ? 1 : 0;
          break;
      }
      if (num != 0)
        dictionary.Add(str2.Substring(0, length), str2.Substring(checked (length + 1)));
    }
    bool flag = false;
    foreach (KeyValuePair<string, string> semanticRule in semanticRules)
    {
      flag = false;
      if (!dictionary.ContainsKey(semanticRule.Key))
        return false;
      string str3 = dictionary[semanticRule.Key];
      char[] separator = new char[1]{ ',' };
      foreach (string str4 in str3.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        if (semanticRule.Value == str4)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return false;
    }
    return flag;
  }

  private bool FindAllMatchingJoinpoints(List<VariableComponent> lJP)
  {
    bool matchingJoinpoints = true;
    Logger.Debug("Starting pointcut identification.");
    foreach (Pointcut pointcut in this.AA.Pointcuts)
    {
      pointcut.ResetJoinpoints();
      foreach (VariableComponent variableComponent in lJP)
      {
        if (variableComponent.InstanceName == null || variableComponent.InstanceName == "")
          throw new ApplicationException("Joinpoint name should not be void or empty");
        string beanName;
        string metadata;
        if (!this.SplitNameAndMetadata(variableComponent.InstanceName, out beanName, out metadata) ? pointcut.SemanticExpression == null && this.SyntacticPointcutMatching(variableComponent.InstanceName, pointcut.SyntacticExpression) : (pointcut.SemanticExpression != null ? this.SyntacticPointcutMatching(beanName, pointcut.SyntacticExpression) && this.SemanticPointcutMatching(metadata, pointcut.SemanticExpression) : this.SyntacticPointcutMatching(beanName, pointcut.SyntacticExpression)))
        {
          Logger.Debug("Matched component '{0}' for variable '{1}'", (object) variableComponent.InstanceName, (object) pointcut.Variable);
          pointcut.Joinpoints.Add(new VariableComponent(pointcut.Variable, variableComponent.InstanceName));
        }
      }
      if (pointcut.Joinpoints.Count == 0)
      {
        matchingJoinpoints = false;
        Logger.Info("Pointcut '{0}' was not identified, AA '{1}' won't be woven.", (object) pointcut.Variable, (object) this.AA.Name);
        break;
      }
    }
    return matchingJoinpoints;
  }

  private bool SplitNameAndMetadata(string fullName, out string beanName, out string metadata)
  {
    int length = fullName.IndexOf('@');
    int num;
    switch (length)
    {
      case -1:
      case 0:
        num = 0;
        break;
      default:
        num = length < checked (fullName.Length - 4) ? 1 : 0;
        break;
    }
    if (num == 0)
    {
      beanName = fullName;
      metadata = (string) null;
      return false;
    }
    beanName = fullName.Substring(0, length);
    metadata = fullName.Substring(checked (length + 1));
    return true;
  }

  public event PointCutBean.AspectOfAssemblyDel ListOfJoinpoints;

  private void FireLListOfJoinpoints()
  {
    if (this.ListOfJoinpoints == null)
      return;
    this.ListOfJoinpoints(this.AA);
  }

  public delegate void AspectOfAssemblyDel(AspectOfAssembly aa);
}
