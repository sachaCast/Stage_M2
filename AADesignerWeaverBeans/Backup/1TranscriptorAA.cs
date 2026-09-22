// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.AASelection
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

#nullable disable
namespace WComp.AADesigner;

internal struct AASelection(Operations op, string n, string d)
{
  public Operations operation = op;
  public string name = n;
  public string definition = d;

  public void SetDefinition(string d) => this.definition = d;
}
