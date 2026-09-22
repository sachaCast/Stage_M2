// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.TaggedValue
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
    [DesignerCategory("code")]
    [DebuggerStepThrough]
    [GeneratedCode("xsd", "2.0.50727.1432")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    [Serializable]
    public class TaggedValue
    {
        private TaggedValue[] taggedValue1Field;
        private string tagField;
        private string tagValueField;

        public TaggedValue()
        {
        }

        public TaggedValue(string tagField) => this.tagField = tagField;

        public TaggedValue(string tagField, string tagValueField)
          : this(tagField)
        {
            this.tagValueField = tagValueField;
        }

        [XmlElement("TaggedValue")]
        public TaggedValue[] TaggedValue1
        {
            get => this.taggedValue1Field;
            set => this.taggedValue1Field = value;
        }

        [XmlAttribute]
        public string Tag
        {
            get => this.tagField;
            set => this.tagField = value;
        }

        [XmlAttribute]
        public string TagValue
        {
            get => this.tagValueField;
            set => this.tagValueField = value;
        }
    }
}