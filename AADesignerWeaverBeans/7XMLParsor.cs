// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.GraphNodeNodeLayout
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace WComp.AADesigner
{
    [XmlType(AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("xsd", "2.0.50727.1432")]
    [Serializable]
    public class GraphNodeNodeLayout
    {
        private int xField;
        private int yField;

        public GraphNodeNodeLayout()
        {
        }

        public GraphNodeNodeLayout(int x, int y)
        {
            this.xField = x;
            this.yField = y;
        }

        [XmlAttribute]
        public int X
        {
            get => this.xField;
            set => this.xField = value;
        }

        [XmlAttribute]
        public int Y
        {
            get => this.yField;
            set => this.yField = value;
        }
    }
}