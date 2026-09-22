// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.SingleToDoubleStringSynchronised
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System.ComponentModel;
using WComp.Beans;

namespace WComp.AADesigner
{
    [Bean(Category = "AADesigner-dev")]
    public class SingleToDoubleStringSynchronised
    {
        private string value1 = "";
        private string value2 = "";
        private bool isValue1Updated = false;
        private bool isValue2Updated = false;

        [DefaultValue("")]
        public string Value1
        {
            get => this.value1;
            set
            {
                this.value1 = value;
                this.isValue1Updated = true;
                if (!this.isValue2Updated)
                    return;
                this.FireLightEvent(this.value1, this.value2);
            }
        }

        [DefaultValue("")]
        public string Value2
        {
            get => this.value2;
            set
            {
                this.value2 = value;
                this.isValue2Updated = true;
                if (!this.isValue1Updated)
                    return;
                this.FireLightEvent(this.value1, this.value2);
            }
        }

        public void Clean()
        {
            this.value1 = "";
            this.value2 = "";
            this.isValue1Updated = false;
            this.isValue2Updated = false;
        }

        public event SingleToDoubleStringSynchronised.StringsEventHandler StringsEvent;

        private void FireLightEvent(string s1, string s2)
        {
            if (!this.isValue1Updated || !this.isValue2Updated || this.StringsEvent == null)
                return;
            this.isValue1Updated = false;
            this.isValue2Updated = false;
            this.StringsEvent(s1, s2);
        }

        public delegate void StringsEventHandler(string sel1, string sel2);
    }
}