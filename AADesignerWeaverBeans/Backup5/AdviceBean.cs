// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.AdviceBean
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System.Collections;
using System.Collections.Generic;
using WComp.Beans;
using WComp.Util;

namespace WComp.AADesigner
{
    [Bean(Category = "AADesigner-dev")]
    public class AdviceBean
    {
        private static int totalInstantiatedAA = 0;
        private static int nbSelectedAA = 0;
        private int idAdvice;
        private bool isInRemoveAction;

        public int Identifier => this.idAdvice;

        public AdviceBean()
        {
            this.idAdvice = AdviceBean.totalInstantiatedAA;
            checked { ++AdviceBean.totalInstantiatedAA; }
            checked { ++AdviceBean.nbSelectedAA; }
            Logger.Info("AdviceBean constructor: instantiating advice id={0}. Static numbers current={1} and max={2}", (object)this.idAdvice, (object)AdviceBean.totalInstantiatedAA, (object)AdviceBean.nbSelectedAA);
            this.isInRemoveAction = false;
        }

        public void SetListOfJoinpointCombinations(AspectOfAssembly aa)
        {
            Logger.Info("--> Advice bean for AA '{0}'", (object)aa.Name);
            if (aa.Advice.Count == 0)
                this.FireAdviceEvent(new List<AspectOfAssembly>());
            else if (aa.JoinpointCombinations.Count == 0)
            {
                this.FireAdviceEvent(new List<AspectOfAssembly>());
            }
            else
            {
                List<AspectOfAssembly> instancesOfAdvices = this.CreateListOfInstancesOfAdvices(aa);
                Logger.Info("<-- Advice bean: sending instances of advices");
                this.FireAdviceEvent(instancesOfAdvices);
            }
        }

        private List<AspectOfAssembly> CreateListOfInstancesOfAdvices(AspectOfAssembly aa)
        {
            List<AspectOfAssembly> instancesOfAdvices = new List<AspectOfAssembly>();
            Hashtable filterComponents = new Hashtable(16 /*0x10*/);
            int num = 0;
            foreach (List<VariableComponent> joinpointCombination in aa.JoinpointCombinations)
            {
                AspectOfAssembly adviceInstance = aa.CreateAdviceInstance(joinpointCombination, filterComponents);
                foreach (VariableComponent localComponent in adviceInstance.LocalComponents)
                {
                    if (localComponent.VariableName != null)
                    {
                        localComponent.InstanceName = $"{localComponent.VariableName}_{aa.Name}{num}";
                        Logger.Debug("set name {0} for component {1}", (object)localComponent.InstanceName, (object)localComponent.VariableName);
                    }
                }
                adviceInstance.SetOriginTags("#" + (object)num, true);
                instancesOfAdvices.Add(adviceInstance);
                checked { ++num; }
            }
            return instancesOfAdvices;
        }

        public bool RemoveBeanSignal
        {
            get => this.isInRemoveAction;
            set
            {
                Logger.Info("AdviceBean: Remove signal");
                checked { --AdviceBean.nbSelectedAA; }
                this.isInRemoveAction = true;
                this.FireRemoveAdviceEvent(this.idAdvice);
            }
        }

        public event AdviceBean.EventAdviceHandler InstancesOfAdvices;

        private void FireAdviceEvent(List<AspectOfAssembly> instancesOfAdvices)
        {
            if (this.InstancesOfAdvices == null || this.isInRemoveAction)
                return;
            int[] IDs = new int[2]
            {
      this.idAdvice,
      AdviceBean.nbSelectedAA
            };
            this.InstancesOfAdvices(instancesOfAdvices, IDs);
        }

        public event AdviceBean.EventRemoveAdviceHandler adviceRemoveEvent;

        private void FireRemoveAdviceEvent(int idAdvice)
        {
            if (this.adviceRemoveEvent == null)
                return;
            this.adviceRemoveEvent(idAdvice);
        }

        public delegate void EventAdviceHandler(List<AspectOfAssembly> instancesOfAdvices, int[] IDs);

        public delegate void EventRemoveAdviceHandler(int idAdvice);
    }
}