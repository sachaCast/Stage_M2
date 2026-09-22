// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.Attribute
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WComp.AADesigner
{
    [XmlRoot(Namespace = "", IsNullable = false)]
    [XmlType(AnonymousType = true)]
    [GeneratedCode("xsd", "2.0.50727.1432")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [Serializable]
    public class Attribute
    {
        private AttributeValueJava[] valueField;
        private bool constantField;
        private string typeField;

        public Attribute()
        {
            this.constantField = true;
            this.valueField = new AttributeValueJava[1];
            this.valueField[0] = new AttributeValueJava();
        }

        public Attribute(string type)
          : this()
        {
            this.typeField = type;
        }

        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("java", typeof(AttributeValueJava), Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public AttributeValueJava[] Value
        {
            get => this.valueField;
            set => this.valueField = value;
        }

        [XmlAttribute]
        public bool constant
        {
            get => this.constantField;
            set => this.constantField = value;
        }

        [XmlAttribute]
        public string type
        {
            get => this.typeField;
            set => this.typeField = value;
        }
    }
}