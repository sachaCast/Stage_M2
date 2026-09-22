// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.additionalLayout
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

[DebuggerStepThrough]
[XmlRoot(Namespace = "", IsNullable = false)]
[XmlType(AnonymousType = true)]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.1432")]
[Serializable]
public class additionalLayout
{
  private string ageField;
  private string forceField;
  private string frozenField;
  private string zoneField;
  private string aktlengthField;
  private string preflengthField;

  public additionalLayout()
  {
  }

  public additionalLayout(string age, string force, string frozen, string zone)
  {
    this.ageField = age;
    this.forceField = force;
    this.frozenField = frozen;
    this.zoneField = zone;
  }

  public additionalLayout(string aktlength, string force, string preflength)
  {
    this.aktlengthField = aktlength;
    this.forceField = force;
    this.preflengthField = preflength;
  }

  [XmlAttribute]
  public string age
  {
    get => this.ageField;
    set => this.ageField = value;
  }

  [XmlAttribute]
  public string force
  {
    get => this.forceField;
    set => this.forceField = value;
  }

  [XmlAttribute]
  public string frozen
  {
    get => this.frozenField;
    set => this.frozenField = value;
  }

  [XmlAttribute]
  public string zone
  {
    get => this.zoneField;
    set => this.zoneField = value;
  }

  [XmlAttribute]
  public string aktlength
  {
    get => this.aktlengthField;
    set => this.aktlengthField = value;
  }

  [XmlAttribute]
  public string preflength
  {
    get => this.preflengthField;
    set => this.preflengthField = value;
  }
}
