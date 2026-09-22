// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.DocumentGraphTransformationSystem
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

[XmlType(AnonymousType = true)]
[GeneratedCode("xsd", "2.0.50727.1432")]
[DesignerCategory("code")]
[DebuggerStepThrough]
[Serializable]
public class DocumentGraphTransformationSystem
{
  private WComp.AADesigner.TaggedValue[] taggedValueField;
  private DocumentGraphTransformationSystemTypes[] typesField;
  private WComp.AADesigner.Graph[] graphField;
  private string idField;
  private string nameField;

  public DocumentGraphTransformationSystem()
  {
  }

  public DocumentGraphTransformationSystem(string id) => this.idField = id;

  public DocumentGraphTransformationSystem(string id, string name)
    : this(id)
  {
    this.name = name;
  }

  [XmlElement("TaggedValue")]
  public WComp.AADesigner.TaggedValue[] TaggedValue
  {
    get => this.taggedValueField;
    set => this.taggedValueField = value;
  }

  [XmlElement("Types", Form = XmlSchemaForm.Unqualified)]
  public DocumentGraphTransformationSystemTypes[] Types
  {
    get => this.typesField;
    set => this.typesField = value;
  }

  [XmlElement("Graph")]
  public WComp.AADesigner.Graph[] Graph
  {
    get => this.graphField;
    set => this.graphField = value;
  }

  [XmlAttribute]
  public string ID
  {
    get => this.idField;
    set => this.idField = value;
  }

  [XmlAttribute]
  public string name
  {
    get => this.nameField;
    set => this.nameField = value;
  }
}
