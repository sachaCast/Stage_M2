// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.Graph
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
    [GeneratedCode("xsd", "2.0.50727.1432")]
    [DesignerCategory("code")]
    [XmlRoot(Namespace = "", IsNullable = false)]
    [XmlType(AnonymousType = true)]
    [DebuggerStepThrough]
    [Serializable]
    public class Graph
    {
        private GraphNode[] nodeField;
        private GraphEdge[] edgeField;
        private string idField;
        private string kindField;
        private string nameField;

        private Graph()
        {
        }

        public Graph(string id, string kind, string name)
        {
            this.idField = id;
            this.kindField = kind;
            this.nameField = name;
        }

        [XmlElement("Node", Form = XmlSchemaForm.Unqualified)]
        public GraphNode[] Node
        {
            get => this.nodeField;
            set => this.nodeField = value;
        }

        [XmlElement("Edge", Form = XmlSchemaForm.Unqualified)]
        public GraphEdge[] Edge
        {
            get => this.edgeField;
            set => this.edgeField = value;
        }

        [XmlAttribute]
        public string ID
        {
            get => this.idField;
            set => this.idField = value;
        }

        [XmlAttribute]
        public string kind
        {
            get => this.kindField;
            set => this.kindField = value;
        }

        [XmlAttribute]
        public string name
        {
            get => this.nameField;
            set => this.nameField = value;
        }
    }
}