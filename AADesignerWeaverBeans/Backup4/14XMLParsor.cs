// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.DocumentGraphTransformationSystemTypesEdgeType
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace WComp.AADesigner;

[GeneratedCode("xsd", "2.0.50727.1432")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
[DesignerCategory("code")]
[Serializable]
public class DocumentGraphTransformationSystemTypesEdgeType
{
  private WComp.AADesigner.AttrType[] attrTypeField;
  private string idField;
  private bool abstractField;
  private string nameField;

  public DocumentGraphTransformationSystemTypesEdgeType()
  {
  }

  public DocumentGraphTransformationSystemTypesEdgeType(string id) => this.idField = id;

  public DocumentGraphTransformationSystemTypesEdgeType(string id, string name)
    : this(id)
  {
    this.nameField = name;
  }

  public DocumentGraphTransformationSystemTypesEdgeType(string id, string name, bool abstractValue)
    : this(id, name)
  {
    this.abstractField = abstractValue;
  }

  [XmlElement("AttrType")]
  public WComp.AADesigner.AttrType[] AttrType
  {
    get => this.attrTypeField;
    set => this.attrTypeField = value;
  }

  [XmlAttribute]
  public string ID
  {
    get => this.idField;
    set => this.idField = value;
  }

  [XmlAttribute]
  public bool @abstract
  {
    get => this.abstractField;
    set => this.abstractField = value;
  }

  [XmlAttribute]
  public string name
  {
    get => this.nameField;
    set => this.nameField = value;
  }
}
