// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.GraphEdge
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace WComp.AADesigner;

[GeneratedCode("xsd", "2.0.50727.1432")]
[XmlType(AnonymousType = true)]
[DebuggerStepThrough]
[DesignerCategory("code")]
[Serializable]
public class GraphEdge
{
  private GraphEdgeEdgeLayout[] edgeLayoutField;
  private WComp.AADesigner.additionalLayout[] additionalLayoutField;
  private WComp.AADesigner.Attribute[] attributeField;
  private string idField;
  private string sourceField;
  private string targetField;
  private string typeField;

  private GraphEdge()
  {
  }

  public GraphEdge(string id, string source, string target, string type)
  {
    this.idField = id;
    this.source = source;
    this.target = target;
    this.typeField = type;
  }

  [XmlElement("EdgeLayout", Form = XmlSchemaForm.Unqualified)]
  public GraphEdgeEdgeLayout[] EdgeLayout
  {
    get => this.edgeLayoutField;
    set => this.edgeLayoutField = value;
  }

  [XmlElement("additionalLayout")]
  public WComp.AADesigner.additionalLayout[] additionalLayout
  {
    get => this.additionalLayoutField;
    set => this.additionalLayoutField = value;
  }

  [XmlElement("Attribute")]
  public WComp.AADesigner.Attribute[] Attribute
  {
    get => this.attributeField;
    set => this.attributeField = value;
  }

  [XmlAttribute]
  public string ID
  {
    get => this.idField;
    set => this.idField = value;
  }

  [XmlAttribute]
  public string source
  {
    get => this.sourceField;
    set => this.sourceField = value;
  }

  [XmlAttribute]
  public string target
  {
    get => this.targetField;
    set => this.targetField = value;
  }

  [XmlAttribute]
  public string type
  {
    get => this.typeField;
    set => this.typeField = value;
  }
}
