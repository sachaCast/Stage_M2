// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.Document
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WComp.AADesigner
{
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
[DebuggerStepThrough]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.1432")]
[Serializable]
public class Document
{
  private DocumentGraphTransformationSystem[] graphTransformationSystemField;
  private string versionField;

  public Document()
  {
  }

  public Document(string version) => this.versionField = version;

  [XmlElement("GraphTransformationSystem", Form = XmlSchemaForm.Unqualified)]
  public DocumentGraphTransformationSystem[] GraphTransformationSystem
  {
    get => this.graphTransformationSystemField;
    set => this.graphTransformationSystemField = value;
  }

  [XmlAttribute]
  public string version
  {
    get => this.versionField;
    set => this.versionField = value;
  }
}
}