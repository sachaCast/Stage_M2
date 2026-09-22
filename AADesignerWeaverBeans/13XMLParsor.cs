// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.DocumentGraphTransformationSystemTypesNodeType
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
    [GeneratedCode("xsd", "2.0.50727.1432")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [Serializable]
    public class DocumentGraphTransformationSystemTypesNodeType
    {
        private WComp.AADesigner.AttrType[] attrTypeField;
        private string idField;
        private bool abstractField;
        private string nameField;

        public DocumentGraphTransformationSystemTypesNodeType()
        {
        }

        public DocumentGraphTransformationSystemTypesNodeType(string id) => this.ID = id;

        public DocumentGraphTransformationSystemTypesNodeType(string id, string name)
          : this(id)
        {
            this.name = name;
        }

        public DocumentGraphTransformationSystemTypesNodeType(string id, string name, bool abstractValue)
          : this(id, name)
        {
            this.abstractField = abstractValue;
        }

        [XmlElement("AttrType")]
        public WComp.AADesigner.AttrType[] AttrType
        {
            get => this.attrTypeField;
            set => this.attrTypeField = value;
        }

        [XmlAttribute]
        public string ID
        {
            get => this.idField;
            set => this.idField = value;
        }

        [XmlAttribute]
        public bool @abstract
        {
            get => this.abstractField;
            set => this.abstractField = value;
        }

        [XmlAttribute]
        public string name
        {
            get => this.nameField;
            set => this.nameField = value;
        }
    }
}