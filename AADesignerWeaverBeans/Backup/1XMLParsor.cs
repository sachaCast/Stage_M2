// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.AttributeValueJava
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
[DebuggerStepThrough]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.1432")]
[Serializable]
public class AttributeValueJava
{
  private string stringField;

  [XmlElement(Form = XmlSchemaForm.Unqualified)]
  public string @string
  {
    get => this.stringField;
    set => this.stringField = value;
  }
}
