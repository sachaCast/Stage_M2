// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.AttrType
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
    [XmlRoot(Namespace = "", IsNullable = false)]
    [XmlType(AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("xsd", "2.0.50727.1432")]
    [Serializable]
    public class AttrType
    {
        private string idField;
        private string attrnameField;
        private string typenameField;
        private bool visibleField;

        public AttrType()
        {
        }

        public AttrType(string id) => this.ID = id;

        public AttrType(string id, string attrname)
          : this(id)
        {
            this.attrnameField = attrname;
        }

        public AttrType(string id, string attrname, string typename)
          : this(id, attrname)
        {
            this.typenameField = typename;
        }

        public AttrType(string id, string attrname, string typename, bool visible)
          : this(id, attrname, typename)
        {
            this.visibleField = visible;
        }

        [XmlAttribute]
        public string ID
        {
            get => this.idField;
            set => this.idField = value;
        }

        [XmlAttribute]
        public string attrname
        {
            get => this.attrnameField;
            set => this.attrnameField = value;
        }

        [XmlAttribute]
        public string typename
        {
            get => this.typenameField;
            set => this.typenameField = value;
        }

        [XmlAttribute]
        public bool visible
        {
            get => this.visibleField;
            set => this.visibleField = value;
        }
    }
}