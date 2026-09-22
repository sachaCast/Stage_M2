// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.GraphEdgeEdgeLayout
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
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[DebuggerStepThrough]
[Serializable]
public class GraphEdgeEdgeLayout
{
  private string bendXField;
  private string bendYField;
  private string sourceMultiplicityOffsetXField;
  private string sourceMultiplicityOffsetYField;
  private string targetMultiplicityOffsetXField;
  private string targetMultiplicityOffsetYField;
  private string textOffsetXField;
  private string textOffsetYField;
  private string loopHField;
  private string loopWField;

  public GraphEdgeEdgeLayout()
  {
  }

  public GraphEdgeEdgeLayout(
    string bendX,
    string bendY,
    string sourceMultiplicityOffsetX,
    string sourceMultiplicityOffsetY,
    string targetMultiplicityOffsetX,
    string targetMultiplicityOffsetY,
    string textOffsetX,
    string textOffsetY)
  {
    this.bendXField = bendX;
    this.bendYField = bendY;
    this.sourceMultiplicityOffsetXField = sourceMultiplicityOffsetX;
    this.sourceMultiplicityOffsetYField = sourceMultiplicityOffsetY;
    this.targetMultiplicityOffsetXField = targetMultiplicityOffsetX;
    this.targetMultiplicityOffsetYField = targetMultiplicityOffsetY;
    this.textOffsetXField = textOffsetX;
    this.textOffsetYField = textOffsetY;
  }

  public GraphEdgeEdgeLayout(
    string bendX,
    string bendY,
    string sourceMultiplicityOffsetX,
    string sourceMultiplicityOffsetY,
    string targetMultiplicityOffsetX,
    string targetMultiplicityOffsetY,
    string textOffsetX,
    string textOffsetY,
    string loopH,
    string loopW)
    : this(bendX, bendY, sourceMultiplicityOffsetX, sourceMultiplicityOffsetY, targetMultiplicityOffsetX, targetMultiplicityOffsetY, textOffsetX, textOffsetY)
  {
    this.loopHField = loopH;
    this.loopWField = loopW;
  }

  [XmlAttribute]
  public string bendX
  {
    get => this.bendXField;
    set => this.bendXField = value;
  }

  [XmlAttribute]
  public string bendY
  {
    get => this.bendYField;
    set => this.bendYField = value;
  }

  [XmlAttribute]
  public string sourceMultiplicityOffsetX
  {
    get => this.sourceMultiplicityOffsetXField;
    set => this.sourceMultiplicityOffsetXField = value;
  }

  [XmlAttribute]
  public string sourceMultiplicityOffsetY
  {
    get => this.sourceMultiplicityOffsetYField;
    set => this.sourceMultiplicityOffsetYField = value;
  }

  [XmlAttribute]
  public string targetMultiplicityOffsetX
  {
    get => this.targetMultiplicityOffsetXField;
    set => this.targetMultiplicityOffsetXField = value;
  }

  [XmlAttribute]
  public string targetMultiplicityOffsetY
  {
    get => this.targetMultiplicityOffsetYField;
    set => this.targetMultiplicityOffsetYField = value;
  }

  [XmlAttribute]
  public string textOffsetX
  {
    get => this.textOffsetXField;
    set => this.textOffsetXField = value;
  }

  [XmlAttribute]
  public string textOffsetY
  {
    get => this.textOffsetYField;
    set => this.textOffsetYField = value;
  }

  [XmlAttribute]
  public string loopH
  {
    get => this.loopHField;
    set => this.loopHField = value;
  }

  [XmlAttribute]
  public string loopW
  {
    get => this.loopWField;
    set => this.loopWField = value;
  }
}
