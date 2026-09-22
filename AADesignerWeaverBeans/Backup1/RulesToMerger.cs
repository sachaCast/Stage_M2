// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.RulesToMerger
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class RulesToMerger
{
  private const string IF = "IF";
  private const string PAR = "PAR";
  private const string SEQ = "SEQ";
  private const string CALL = "CALL";
  private const string DELEGATE = "DEL";
  private const string NOP = "NOP";
  private List<AdviceRule> rules;
  private List<AdviceRule> initialAssembly;
  private List<AdviceRule> unhandledRules;
  private List<AdviceRule> componentRules;
  private bool saveToFile;

  public RulesToMerger()
  {
    this.initialAssembly = (List<AdviceRule>) null;
    this.rules = (List<AdviceRule>) null;
    this.SaveToFile = true;
  }

  public List<AdviceRule> InitialAssembly
  {
    set => this.initialAssembly = value;
    get => this.initialAssembly;
  }

  [DefaultValue(true)]
  public bool SaveToFile
  {
    get => this.saveToFile;
    set => this.saveToFile = value;
  }

  public void SetListOfRules(List<AdviceRule> rules)
  {
    if (this.initialAssembly == null)
    {
      Logger.Warn("RulesToMerger received list of rules before initial assembly");
    }
    else
    {
      this.unhandledRules = new List<AdviceRule>();
      this.componentRules = new List<AdviceRule>();
      this.rules = new List<AdviceRule>(checked (rules.Count + this.initialAssembly.Count));
      this.rules.AddRange((IEnumerable<AdviceRule>) rules);
      this.rules.AddRange((IEnumerable<AdviceRule>) this.initialAssembly);
      this.save_in_XMLFile();
    }
  }

  public event RulesToMerger.DoubleRuleListDelegate UnhandledRules;

  public event RulesToMerger.EventXMLHandler xmlEvent;

  private void FireRulesEvent(string xml)
  {
    Logger.Info("Sending XML data to merger");
    if (this.xmlEvent == null)
      return;
    this.xmlEvent(xml);
  }

  private void FireUnhandledRules(List<AdviceRule> unhandledRules, List<AdviceRule> componentRules)
  {
    if (this.UnhandledRules == null)
      return;
    this.UnhandledRules(unhandledRules, componentRules);
  }

  private void save_in_XMLFile()
  {
    Document o = new Document("1.0");
    DocumentGraphTransformationSystem transformationSystem = new DocumentGraphTransformationSystem("I1", "WComp_MegerTest");
    o.GraphTransformationSystem = new DocumentGraphTransformationSystem[1]
    {
      transformationSystem
    };
    TaggedValue[] taggedValueArray1 = new TaggedValue[9];
    transformationSystem.TaggedValue = taggedValueArray1;
    taggedValueArray1[0] = new TaggedValue("AttrHandler", "Java Expr");
    TaggedValue[] taggedValueArray2 = new TaggedValue[3];
    taggedValueArray1[0].TaggedValue1 = taggedValueArray2;
    taggedValueArray2[0] = new TaggedValue("Package", "java.lang");
    taggedValueArray2[1] = new TaggedValue("Package", "java.util");
    taggedValueArray2[2] = new TaggedValue("Package", "com.objectspace.jgl");
    taggedValueArray1[1] = new TaggedValue("CSP", true.ToString());
    taggedValueArray1[2] = new TaggedValue("injective", true.ToString());
    taggedValueArray1[3] = new TaggedValue("dangling", true.ToString());
    taggedValueArray1[4] = new TaggedValue("NACs", true.ToString());
    taggedValueArray1[5] = new TaggedValue("PACs", true.ToString());
    taggedValueArray1[6] = new TaggedValue("breakAllLayer", true.ToString());
    taggedValueArray1[7] = new TaggedValue("showGraphAfterStep", true.ToString());
    taggedValueArray1[8] = new TaggedValue("TypeGraphLevel", "DISABLED");
    DocumentGraphTransformationSystemTypes[] transformationSystemTypesArray = new DocumentGraphTransformationSystemTypes[1]
    {
      new DocumentGraphTransformationSystemTypes()
    };
    transformationSystem.Types = transformationSystemTypesArray;
    DocumentGraphTransformationSystemTypesNodeType[] systemTypesNodeTypeArray = new DocumentGraphTransformationSystemTypesNodeType[2]
    {
      new DocumentGraphTransformationSystemTypesNodeType("I2", "Comport%:RECT:java.awt.Color[r=0,g=0,b=0]:[NODE]:", false),
      null
    };
    AttrType[] attrTypeArray = new AttrType[2]
    {
      new AttrType("I4", "CTy", "String", true),
      new AttrType("I5", "CN", "String", true)
    };
    systemTypesNodeTypeArray[0].AttrType = attrTypeArray;
    systemTypesNodeTypeArray[1] = new DocumentGraphTransformationSystemTypesNodeType("I6", "Fus%:RECT:java.awt.Color[r=0,g=0,b=0]:[NODE]:", false);
    transformationSystemTypesArray[0].NodeType = systemTypesNodeTypeArray;
    transformationSystemTypesArray[0].EdgeType = new DocumentGraphTransformationSystemTypesEdgeType[1];
    transformationSystemTypesArray[0].EdgeType[0] = new DocumentGraphTransformationSystemTypesEdgeType("I7", "Link%:SOLID_LINE:java.awt.Color[r=0,g=0,b=0]:[EDGE]:", false);
    transformationSystemTypesArray[0].EdgeType[0].AttrType = new AttrType[1]
    {
      new AttrType("I9", "LTy", "String", true)
    };
    transformationSystemTypesArray[0].Graph = new Graph[1];
    transformationSystemTypesArray[0].Graph[0] = new Graph("I10", "TG", "AssemblyTypeGraph");
    transformationSystemTypesArray[0].Graph[0].Node = new GraphNode[2];
    transformationSystemTypesArray[0].Graph[0].Node[0] = new GraphNode("I11", "I2");
    transformationSystemTypesArray[0].Graph[0].Node[0].NodeLayout = new GraphNodeNodeLayout[1]
    {
      new GraphNodeNodeLayout(266, 476)
    };
    transformationSystemTypesArray[0].Graph[0].Node[0].additionalLayout = new additionalLayout[1]
    {
      new additionalLayout("0", "10", false.ToString(), "50")
    };
    transformationSystemTypesArray[0].Graph[0].Node[1] = new GraphNode("I13", "I6");
    transformationSystemTypesArray[0].Graph[0].Node[1].NodeLayout = new GraphNodeNodeLayout[1]
    {
      new GraphNodeNodeLayout(465, 392)
    };
    transformationSystemTypesArray[0].Graph[0].Node[1].additionalLayout = new additionalLayout[1]
    {
      new additionalLayout("0", "10", false.ToString(), "50")
    };
    transformationSystemTypesArray[0].Graph[0].Edge = new GraphEdge[3];
    transformationSystemTypesArray[0].Graph[0].Edge[0] = new GraphEdge("I14", "I11", "I13", "I7");
    transformationSystemTypesArray[0].Graph[0].Edge[0].EdgeLayout = new GraphEdgeEdgeLayout[1]
    {
      new GraphEdgeEdgeLayout("370", "443", "0", "0", "0", "0", "0", "-22")
    };
    transformationSystemTypesArray[0].Graph[0].Edge[0].additionalLayout = new additionalLayout[1]
    {
      new additionalLayout("216", "10", "200")
    };
    transformationSystemTypesArray[0].Graph[0].Edge[1] = new GraphEdge("I16", "I13", "I11", "I7");
    transformationSystemTypesArray[0].Graph[0].Edge[1].EdgeLayout = new GraphEdgeEdgeLayout[1]
    {
      new GraphEdgeEdgeLayout("0", "0", "0", "0", "0", "0", "0", "-22")
    };
    transformationSystemTypesArray[0].Graph[0].Edge[1].additionalLayout = new additionalLayout[1]
    {
      new additionalLayout("216", "10", "200")
    };
    transformationSystemTypesArray[0].Graph[0].Edge[2] = new GraphEdge("I18", "I11", "I11", "I7");
    transformationSystemTypesArray[0].Graph[0].Edge[2].EdgeLayout = new GraphEdgeEdgeLayout[1]
    {
      new GraphEdgeEdgeLayout("0", "0", "0", "0", "0", "0", "0", "-22", "0", "0")
    };
    transformationSystemTypesArray[0].Graph[0].Edge[2].additionalLayout = new additionalLayout[1]
    {
      new additionalLayout("0", "10", "200")
    };
    transformationSystem.Graph = new Graph[1]
    {
      new Graph("I20", "HOST", "Graph_Component_Assembly")
    };
    Graph graph = transformationSystem.Graph[0];
    graph.Node = new GraphNode[200];
    graph.Edge = new GraphEdge[200];
    int index1 = 0;
    int index2 = 0;
    foreach (AdviceRule rule in this.rules)
    {
      if (rule.Type == RuleType.ADDLINK)
      {
        LinkCreationRule linkCreationRule = rule as LinkCreationRule;
        string instanceName1 = linkCreationRule.SourceComponent.InstanceName;
        string instanceName2 = linkCreationRule.DestinationComponent.InstanceName;
        string id1 = $"{instanceName1}.{linkCreationRule.SourceEvent}";
        int index3 = 0;
        while (index3 < index1 && !(graph.Node[index3].ID == id1) && !(graph.Node[index3].ID == instanceName1))
          checked { ++index3; }
        if (index3 == index1)
        {
          graph.Node[index1] = new GraphNode(id1, "I2");
          graph.Node[index1].Attribute = new Attribute[2]
          {
            new Attribute("I4"),
            new Attribute("I5")
          };
          graph.Node[index1].Attribute[0].Value[0].@string = !instanceName1.StartsWith("IF") ? (!instanceName1.StartsWith("SEQ") ? (!instanceName1.StartsWith("PAR") ? (!instanceName1.StartsWith("NOP") ? (!instanceName1.StartsWith("CALL") ? (!instanceName1.StartsWith("DELEGATE") ? "Port" : "DEL") : "CALL") : "NOP") : "PAR") : "SEQ") : "IF";
          if (instanceName1.StartsWith("IF") || instanceName1.StartsWith("SEQ") || instanceName1.StartsWith("PAR") || instanceName1.StartsWith("NOP") || instanceName1.StartsWith("CALL") || instanceName1.StartsWith("DELEGATE"))
          {
            graph.Node[index1].ID = instanceName1;
            graph.Node[index1].Attribute[1].Value[0].@string = instanceName1;
          }
          else
          {
            string str1 = "[";
            bool flag = true;
            if (linkCreationRule.Parameters != null && linkCreationRule.Parameters.Count != 0)
            {
              foreach (string parameter in linkCreationRule.Parameters)
              {
                if (!flag)
                  str1 += ",";
                else
                  flag = false;
                str1 += parameter;
              }
            }
            string str2 = str1 + "]";
            graph.Node[index1].Attribute[1].Value[0].@string = id1;
          }
          checked { ++index1; }
        }
        string id2 = $"{instanceName2}.{linkCreationRule.DestinationMethod}";
        int index4 = 0;
        while (index4 < index1 && !(graph.Node[index4].ID == id2) && !(graph.Node[index4].ID == instanceName2))
          checked { ++index4; }
        if (index4 == index1)
        {
          graph.Node[index1] = new GraphNode(id2, "I2");
          graph.Node[index1].Attribute = new Attribute[2]
          {
            new Attribute("I4"),
            new Attribute("I5")
          };
          graph.Node[index1].Attribute[0].Value[0].@string = !instanceName2.StartsWith("IF") ? (!instanceName2.StartsWith("SEQ") ? (!instanceName2.StartsWith("PAR") ? (!instanceName2.StartsWith("NOP") ? (!instanceName2.StartsWith("CALL") ? (!instanceName2.StartsWith("DELEGATE") ? "Port" : "DEL") : "CALL") : "NOP") : "PAR") : "SEQ") : "IF";
          if (instanceName2.StartsWith("IF") || instanceName2.StartsWith("SEQ") || instanceName2.StartsWith("PAR") || instanceName2.StartsWith("NOP") || instanceName2.StartsWith("CALL") || instanceName2.StartsWith("DELEGATE"))
          {
            graph.Node[index1].ID = instanceName2;
            graph.Node[index1].Attribute[1].Value[0].@string = instanceName2;
          }
          else
            graph.Node[index1].Attribute[1].Value[0].@string = id2;
          checked { ++index1; }
        }
        graph.Edge[index2] = new GraphEdge("Edge" + (object) index2, graph.Node[index3].ID, graph.Node[index4].ID, "I7");
        graph.Edge[index2].Attribute = new Attribute[1]
        {
          new Attribute("I9")
        };
        if (graph.Node[index4].Attribute[0].Value[0].@string == "IF")
          graph.Edge[index2].Attribute[0].Value[0].@string = !(linkCreationRule.DestinationMethod == "Boolean") ? "L" : "C";
        else if (graph.Node[index3].Attribute[0].Value[0].@string == "IF")
        {
          switch (linkCreationRule.SourceEvent)
          {
            case "Cond":
              graph.Edge[index2].Attribute[0].Value[0].@string = "C";
              break;
            case "Then":
              graph.Edge[index2].Attribute[0].Value[0].@string = "T";
              break;
            case "Else":
              graph.Edge[index2].Attribute[0].Value[0].@string = "F";
              break;
            default:
              graph.Edge[index2].Attribute[0].Value[0].@string = "L";
              break;
          }
        }
        else if (graph.Node[index3].Attribute[0].Value[0].@string == "SEQ")
        {
          switch (linkCreationRule.SourceEvent)
          {
            case "Out1":
              graph.Edge[index2].Attribute[0].Value[0].@string = "1";
              break;
            case "Out2":
              graph.Edge[index2].Attribute[0].Value[0].@string = "2";
              break;
            default:
              graph.Edge[index2].Attribute[0].Value[0].@string = "L";
              break;
          }
        }
        else
          graph.Edge[index2].Attribute[0].Value[0].@string = "L";
        checked { ++index2; }
      }
      else if (rule.Type == RuleType.ADDCOMP)
      {
        if (rule.OriginAA != AdviceRule.INITIAL_TAG)
          this.componentRules.Add(rule);
      }
      else if (rule.Type == RuleType.SETPROP)
        this.unhandledRules.Add(rule);
    }
    XmlSerializer xmlSerializer = new XmlSerializer(typeof (Document));
    if (this.saveToFile)
    {
      StreamWriter streamWriter = new StreamWriter("savedRules.ggx");
      xmlSerializer.Serialize((TextWriter) streamWriter, (object) o);
      streamWriter.Close();
    }
    StringWriter stringWriter = new StringWriter();
    xmlSerializer.Serialize((TextWriter) stringWriter, (object) o);
    this.FireUnhandledRules(this.unhandledRules, this.componentRules);
    this.FireRulesEvent(stringWriter.ToString());
  }

  public delegate void DoubleRuleListDelegate(List<AdviceRule> r1, List<AdviceRule> r2);

  public delegate void EventXMLHandler(string xml);
}
