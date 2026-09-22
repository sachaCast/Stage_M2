// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.ConflictBean
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using WComp.Beans;

namespace WComp.AADesigner
{
    [Bean(Category = "AADesigner-dev")]
    public class ConflictBean
    {
        public object Inp(params object[] o) => this.Out != null ? this.Out(o) : new object();

        public event Generic Out;

        public void setEventWithoutArg() => this.FireConflictEvent();

        public event ConflictBean.EventConflictHandler conflict_event;

        private void FireConflictEvent()
        {
            if (this.conflict_event == null)
                return;
            this.conflict_event();
        }

        public delegate void EventConflictHandler();
    }
}