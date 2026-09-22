// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.JoinPointFullBean
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System.Collections.Generic;
using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class JoinPointFullBean : JoinPointBuilderBean
{
  protected override void ComputeCombinations(AspectOfAssembly aa)
  {
    Logger.Debug("ComputeCombinations in JoinPointFullBean");
    if (aa.Pointcuts.Count == 0)
    {
      aa.JoinpointCombinations.Add(new List<VariableComponent>());
    }
    else
    {
      int[] numArray = new int[aa.Pointcuts.Count];
      bool flag1 = false;
      bool flag2 = false;
      do
      {
        List<VariableComponent> variableComponentList = new List<VariableComponent>();
        int index = -1;
        foreach (Pointcut pointcut in aa.Pointcuts)
        {
          checked { ++index; }
          if (numArray[index] == pointcut.Joinpoints.Count)
          {
            if (index == 0)
            {
              flag1 = true;
              break;
            }
            numArray[index] = 0;
            checked { ++numArray[index - 1]; }
            flag2 = true;
            break;
          }
          variableComponentList.Add(pointcut.Joinpoints[numArray[index]]);
        }
        if (!flag1)
        {
          if (!flag2)
          {
            checked { ++numArray[index]; }
            aa.JoinpointCombinations.Add(variableComponentList);
          }
          else
            flag2 = false;
        }
        else
          goto label_18;
      }
      while (!flag1);
      goto label_11;
label_18:
      return;
label_11:;
    }
  }
}
