// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.DocumentGraphTransformationSystemTypes
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
    [DebuggerStepThrough]
    [GeneratedCode("xsd", "2.0.50727.1432")]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [Serializable]
    public class DocumentGraphTransformationSystemTypes
    {
        private DocumentGraphTransformationSystemTypesNodeType[] nodeTypeField;
        private DocumentGraphTransformationSystemTypesEdgeType[] edgeTypeField;
        private WComp.AADesigner.Graph[] graphField;

        [XmlElement("NodeType", Form = XmlSchemaForm.Unqualified)]
        public DocumentGraphTransformationSystemTypesNodeType[] NodeType
        {
            get => this.nodeTypeField;
            set => this.nodeTypeField = value;
        }

        [XmlElement("EdgeType", Form = XmlSchemaForm.Unqualified)]
        public DocumentGraphTransformationSystemTypesEdgeType[] EdgeType
        {
            get => this.edgeTypeField;
            set => this.edgeTypeField = value;
        }

        [XmlElement("Graph")]
        public WComp.AADesigner.Graph[] Graph
        {
            get => this.graphField;
            set => this.graphField = value;
        }
    }
}