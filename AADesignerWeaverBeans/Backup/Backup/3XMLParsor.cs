// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.GraphNode
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

[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
[GeneratedCode("xsd", "2.0.50727.1432")]
[Serializable]
public class GraphNode
{
  private GraphNodeNodeLayout[] nodeLayoutField;
  private WComp.AADesigner.additionalLayout[] additionalLayoutField;
  private WComp.AADesigner.Attribute[] attributeField;
  private string idField;
  private string typeField;

  public GraphNode()
  {
  }

  public GraphNode(string id, string type)
  {
    this.idField = id;
    this.typeField = type;
  }

  [XmlElement("NodeLayout", Form = XmlSchemaForm.Unqualified)]
  public GraphNodeNodeLayout[] NodeLayout
  {
    get => this.nodeLayoutField;
    set => this.nodeLayoutField = value;
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
  public string type
  {
    get => this.typeField;
    set => this.typeField = value;
  }
}
