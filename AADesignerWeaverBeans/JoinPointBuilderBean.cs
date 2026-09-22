// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.JoinPointBuilderBean
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using WComp.Util;

namespace WComp.AADesigner
{
    public abstract class JoinPointBuilderBean
    {
        public void SetJoinpointLists(AspectOfAssembly aa)
        {
            Logger.Debug("--> Joinpoint builder: Starting joinpoint combination computing.");
            aa.ResetJoinpointCombinations();
            this.ComputeCombinations(aa);
            Logger.Debug("<-- Joinpoint builder: sending {0} combinations.", (object)aa.JoinpointCombinations.Count);
            this.FireJointPointsCombinaisonEvent(aa);
        }

        protected abstract void ComputeCombinations(AspectOfAssembly aa);

        public event JoinPointBuilderBean.JoinpointsCombinationHandler JoinpointCombinations;

        private void FireJointPointsCombinaisonEvent(AspectOfAssembly aa)
        {
            if (this.JoinpointCombinations == null)
                return;
            this.JoinpointCombinations(aa);
        }

        public delegate void JoinpointsCombinationHandler(AspectOfAssembly aa);
    }
}