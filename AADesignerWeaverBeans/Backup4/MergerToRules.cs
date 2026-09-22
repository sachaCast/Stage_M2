// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.MergerToRules
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class MergerToRules
{
  private const string IF = "IF";
  private const string IF_TYPE = "BasicBeans.IF";
  private const string PAR = "PAR";
  private const string PAR_TYPE = "BasicBeans.PAR";
  private const string SEQ = "SEQ";
  private const string SEQ_TYPE = "BasicBeans.SEQ";
  private const string NOP = "NOP";
  private List<AdviceRule> unhandledRules;
  private List<AdviceRule> componentRules;

  public MergerToRules() => this.unhandledRules = (List<AdviceRule>) null;

  public void SetUnhandledRules(
    List<AdviceRule> unhandledRules,
    List<AdviceRule> componentInfoRules)
  {
    this.unhandledRules = unhandledRules;
    this.componentRules = componentInfoRules;
  }

  public void SetXMLRules(string xmlString)
  {
    Logger.Info("Received XML data from merger");
    this.UpdateRulesFromXML(xmlString);
  }

  public event MergerToRules.EventRulesHandler rulesEvent;

  private void FireRulesEvent(List<AdviceRule> rules)
  {
    if (this.unhandledRules == null)
    {
      Logger.Warn("UNHANDLED RULES NOT SET IN MERGERTORULES");
    }
    else
    {
      Logger.Info("Processed list of rules from XML graph, sending back to weaver ({0} rule + {1} unhandled rules)", (object) rules.Count, (object) this.unhandledRules.Count);
      if (this.rulesEvent == null)
        return;
      rules.AddRange((IEnumerable<AdviceRule>) this.unhandledRules);
      this.rulesEvent(rules);
    }
  }

  private void UpdateRulesFromXML(string xmlString)
  {
    XmlSerializer xmlSerializer;
    StringReader stringReader;
    try
    {
      xmlSerializer = new XmlSerializer(typeof (Document));
      stringReader = new StringReader(xmlString);
    }
    catch (Exception ex)
    {
      Logger.Error("{0}xmlString in not a correct format: {1}{0}" + Environment.NewLine, (object) ex.Message);
      return;
    }
    Document document;
    try
    {
      document = (Document) xmlSerializer.Deserialize((TextReader) stringReader);
    }
    catch (InvalidOperationException ex)
    {
      Logger.Error("Unable to deserialize XML returned by the MERGER: {0}{1}{2}", (object) ex.Message, (object) Environment.NewLine, (object) ex.InnerException.Message);
      return;
    }
    if (document.GraphTransformationSystem[0].Graph.Length == 0)
    {
      this.FireRulesEvent(new List<AdviceRule>(0));
    }
    else
    {
      Graph graph = document.GraphTransformationSystem[0].Graph[0];
      List<AdviceRule> rules = new List<AdviceRule>(100);
      if (graph.Node != null)
      {
        HashSet<string> stringSet = new HashSet<string>();
        foreach (GraphNode graphNode in graph.Node)
        {
          string str = graphNode.Attribute[0].Value[0].@string;
          string instanceName = graphNode.Attribute[1].Value[0].@string;
          string type;
          switch (str)
          {
            case "Port":
              string[] strArray = instanceName.Split('.');
              stringSet.Add(strArray[0]);
              continue;
            case "IF":
              type = "BasicBeans.IF";
              break;
            case "PAR":
              type = "BasicBeans.PAR";
              break;
            case "SEQ":
              type = "BasicBeans.SEQ";
              break;
            default:
              Logger.Error("ERROR: AA, MERGERTORULES: unhandled component creation ({0} : {1})", (object) str, (object) instanceName);
              continue;
          }
          CompCreationRule compCreationRule = new CompCreationRule(new VariableComponent((string) null, instanceName), type);
          rules.Add((AdviceRule) compCreationRule);
        }
        if (stringSet.Count > 0)
        {
          foreach (CompCreationRule componentRule in this.componentRules)
          {
            foreach (string str in stringSet)
            {
              if (str == componentRule.Component.InstanceName)
                rules.Add((AdviceRule) componentRule);
            }
          }
        }
      }
      if (graph.Edge != null)
      {
        foreach (GraphEdge graphEdge in graph.Edge)
        {
          string[] strArray1 = graphEdge.source.Split('.');
          string[] strArray2 = graphEdge.target.Split('.');
          int length;
          LinkCreationRule linkCreationRule;
          if ((length = strArray1[1].IndexOf('[')) != -1)
          {
            string[] strArray3 = strArray1[1].Substring(checked (length + 1), checked (strArray1[1].Length - length - 2)).Split(',');
            LinkedList<string> parameters = new LinkedList<string>();
            foreach (string str in strArray3)
              parameters.AddLast(str);
            linkCreationRule = new LinkCreationRule(new VariableComponent((string) null, strArray1[0]), strArray1[1].Substring(0, length), new VariableComponent((string) null, strArray2[0]), strArray2[1], parameters);
          }
          else
            linkCreationRule = new LinkCreationRule(new VariableComponent((string) null, strArray1[0]), strArray1[1], new VariableComponent((string) null, strArray2[0]), strArray2[1], (LinkedList<string>) null);
          rules.Add((AdviceRule) linkCreationRule);
        }
      }
      this.FireRulesEvent(rules);
    }
  }

  public delegate void EventRulesHandler(List<AdviceRule> rules);
}
