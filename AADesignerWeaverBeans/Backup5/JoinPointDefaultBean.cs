// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.JoinPointDefaultBean
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System.Collections.Generic;
using WComp.Beans;
using WComp.Util;

namespace WComp.AADesigner
{
    [Bean(Category = "AADesigner-dev")]
    public class JoinPointDefaultBean : JoinPointBuilderBean
    {
        protected override void ComputeCombinations(AspectOfAssembly aa)
        {
            Logger.Debug("ComputeCombinations in JoinPointDefaultBean");

            // LOG ENTREE
            Logger.Debug("=== ENTREE ===");
            Logger.Debug("Nombre de pointcuts : " + aa.Pointcuts.Count);
            foreach (Pointcut pointcut in aa.Pointcuts)
            {
                Logger.Debug("Pointcut variable='" + pointcut.Variable
                    + "' -> " + pointcut.Joinpoints.Count + " joinpoint(s)");
                foreach (VariableComponent jp in pointcut.Joinpoints)
                    Logger.Debug("   joinpoint : " + jp.InstanceName);
            }

            if (aa.Pointcuts.Count == 0)
            {
                aa.JoinpointCombinations.Add(new List<VariableComponent>());
            }
            else
            {
                bool flag = false;
                int index = 0;
                do
                {
                    List<VariableComponent> variableComponentList = new List<VariableComponent>();
                    foreach (Pointcut pointcut in aa.Pointcuts)
                    {
                        if (pointcut.Joinpoints.Count <= index)
                        {
                            flag = true;
                            break;
                        }
                        variableComponentList.Add(pointcut.Joinpoints[index]);
                    }
                    if (!flag)
                        aa.JoinpointCombinations.Add(variableComponentList);
                    checked { ++index; }
                }
                while (!flag);
            }

            // LOG SORTIE
            Logger.Debug("=== SORTIE ===");
            Logger.Debug("Nombre de combinaisons : " + aa.JoinpointCombinations.Count);
            foreach (List<VariableComponent> combo in aa.JoinpointCombinations)
            {
                string line = "  Combo : ";
                foreach (VariableComponent vc in combo)
                    line += "[" + vc.VariableName + "=" + vc.InstanceName + "] ";
                Logger.Debug(line);
            }

        }
    }
}