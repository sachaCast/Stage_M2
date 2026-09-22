// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.AARepositoryManager
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using WComp.Beans;
using WComp.Util;

namespace WComp.AADesigner
{
    [Bean(Category = "AADesigner-dev")]
    public class AARepositoryManager
    {
        public static readonly string AA_FILE_EXTENSION = "aa";
        public static readonly string AA_SELECT_ALL = "*ALL_AA*";
        private List<AARepositoryManager.AAStruct> AAs;
        private string directory;

        public AARepositoryManager()
        {
            this.AAs = new List<AARepositoryManager.AAStruct>(0);
            this.Clear();
            this.directory = string.Empty;
        }

        [DefaultValue("")]
        public string Directory
        {
            get => this.directory;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                if (System.IO.Directory.Exists(value))
                {
                    this.directory = value;
                    this.ReloadAAsFromDirectory();
                }
                else
                {
                    Logger.Error("AA Directory does not exist");
                    this.FireInformationMessage("Cannot load AA, definition directory not found");
                }
            }
        }

        public string RepositoryStatus => this.generateAAAndStatusList();

        public void GetRepositoryStatus() => this.FireRepositoryChanged();

        public void Clear()
        {
            foreach (AARepositoryManager.AAStruct aa in this.AAs)
            {
                if (aa.selected)
                    this.FireChangeInActivationOfAAEvent(false, aa.name, aa.definition);
            }
            this.AAs.Clear();
            this.FireRepositoryChanged();
        }

        public void ReloadAAsFromDirectory()
        {
            this.FireInformationMessage("Reloading AA repository, updating AA definitions from files.");
            string[] newAAFiles = System.IO.Directory.GetFiles(this.directory, "*." + AARepositoryManager.AA_FILE_EXTENSION);
            int length = newAAFiles.Length;
            List<AARepositoryManager.AAStruct> aaStructList = new List<AARepositoryManager.AAStruct>(length);
            int i = 0;
            while (i < length)
            {
                int index = this.AAs.FindIndex((Predicate<AARepositoryManager.AAStruct>)(aa => aa.filename == newAAFiles[i]));
                AARepositoryManager.AAStruct newAA;
                using (StreamReader streamReader = new StreamReader(newAAFiles[i]))
                {
                    newAA = new AARepositoryManager.AAStruct()
                    {
                        definition = streamReader.ReadToEnd()
                    };
                    newAA.name = this.getAAnameFromDefinition(newAA.definition);
                    if (newAA.name == string.Empty)
                        newAA.name = "Malformed AA";
                    if (aaStructList.Exists((Predicate<AARepositoryManager.AAStruct>)(aa => aa.name == newAA.name)))
                    {
                        this.FireInformationMessage($"You have several AA definitions with the same name ({newAA.name}), please fix this.");
                        goto label_17;
                    }
                    newAA.selected = false;
                    newAA.filename = newAAFiles[i];
                    newAA.saved = true;
                }
                if (index != -1)
                {
                    if (this.AAs[index].name != newAA.name)
                        this.FireInformationMessage($"The name of AA {this.AAs[index].name} has changed to {newAA.name}. Inconsistencies may happen.");
                    newAA.selected = this.AAs[index].selected;
                    if (this.AAs[index].definition != newAA.definition && this.AAs[index].selected)
                    {
                        this.FireInformationMessage($"Updating definition of AA {this.AAs[index].name}.");
                        this.FireChangeAADefinition(this.AAs[index].name, newAA.definition);
                    }
                }
                aaStructList.Add(newAA);
            label_17:
                checked { ++i; }
            }
            this.AAs = aaStructList;
            this.FireRepositoryChanged();
        }

        public void Checkitem(string itemName, bool checkIt)
        {
            Logger.Debug("Checkitem: {0}, {1}", (object)itemName, (object)checkIt);
            AARepositoryManager.AAStruct aaFromName = this.GetAAFromName(itemName);
            if (aaFromName == null)
                return;
            if (aaFromName.selected != checkIt)
            {
                Logger.Debug("Changing back item '{0}' to {1}", (object)itemName, (object)checkIt);
                aaFromName.selected = checkIt;
                this.FireInformationMessage($"Error selecting AA {itemName}. Unselecting it.");
                this.FireRepositoryChanged();
            }
            else if (!aaFromName.saved && checkIt)
            {
                this.SaveCurrentAADefinition(aaFromName);
                this.FireInformationMessage($"AA {itemName} successfully {(checkIt ? (object)"selected" : (object)"unselected")} and saved to file.");
            }
            else
                this.FireInformationMessage($"AA {itemName} successfully {(checkIt ? (object)"selected" : (object)"unselected")}.");
        }

        private void SaveCurrentAADefinition(AARepositoryManager.AAStruct aa)
        {
            aa.saved = true;
            try
            {
                StreamWriter streamWriter;
                using (streamWriter = new StreamWriter(aa.filename))
                    streamWriter.Write(aa.definition);
            }
            catch (Exception ex)
            {
                this.FireInformationMessage($"Could not save AA in file: {ex.Message}.");
                return;
            }
            this.FireInformationMessage($"AA {aa.filename} successfully saved.");
        }

        public void GetAADefinition(string aaName)
        {
            AARepositoryManager.AAStruct aaFromName = this.GetAAFromName(aaName);
            if (aaFromName == null)
                return;
            this.FireAADefinitionEvent(aaFromName.definition);
        }

        public void AddAA(string definition)
        {
            string name = this.getAAnameFromDefinition(definition);
            if (name == string.Empty)
                return;
            int index = this.AAs.FindIndex((Predicate<AARepositoryManager.AAStruct>)(aa => aa.name == name));
            if (index == -1)
            {
                AARepositoryManager.AAStruct aaStruct = new AARepositoryManager.AAStruct();
                aaStruct.name = name;
                if (this.directory.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    aaStruct.filename = $"{this.directory}{aaStruct.name}.{AARepositoryManager.AA_FILE_EXTENSION}";
                else
                    aaStruct.filename = $"{this.directory}{(object)Path.DirectorySeparatorChar}{aaStruct.name}.{AARepositoryManager.AA_FILE_EXTENSION}";
                aaStruct.selected = false;
                aaStruct.definition = definition;
                aaStruct.saved = false;
                this.AAs.Add(aaStruct);
                this.FireRepositoryChanged();
            }
            else
            {
                AARepositoryManager.AAStruct aa = this.AAs[index];
                aa.definition = definition;
                aa.saved = false;
                if (aa.selected)
                    this.FireChangeAADefinition(aa.name, aa.definition);
            }
        }

        public void SelectAA(string AANameAndStatus)
        {
            int length = AANameAndStatus.IndexOf('|');
            if (length < 1 || length == checked(AANameAndStatus.Length - 1))
                return;
            bool add;
            if (AANameAndStatus[checked(length + 1)] == '0')
            {
                add = false;
            }
            else
            {
                if (AANameAndStatus[checked(length + 1)] != '1')
                    return;
                add = true;
            }
            string name = AANameAndStatus.Substring(0, length);
            Logger.Info("AA selection requested: {0} and {1}", (object)name, (object)add.ToString());
            if (name == AARepositoryManager.AA_SELECT_ALL)
            {
                foreach (AARepositoryManager.AAStruct aa in this.AAs)
                {
                    if (add && !aa.selected || !add && aa.selected)
                    {
                        aa.selected = add;
                        this.FireChangeInActivationOfAAEvent(add, aa.name, aa.definition);
                    }
                }
                this.FireRepositoryChanged();
            }
            else
            {
                AARepositoryManager.AAStruct aaFromName = this.GetAAFromName(name);
                if (aaFromName == null || aaFromName.selected == add)
                    return;
                aaFromName.selected = add;
                this.FireRepositoryChanged();
                this.FireChangeInActivationOfAAEvent(add, name, aaFromName.definition);
            }
        }

        public void ReportRuntimeError(string AA, string errorMsg)
        {
            if (!string.IsNullOrEmpty(AA))
            {
                AARepositoryManager.AAStruct aaFromName = this.GetAAFromName(AA);
                if (aaFromName != null)
                {
                    aaFromName.selected = false;
                    this.FireRepositoryChanged();
                    this.FireChangeInActivationOfAAEvent(false, AA, aaFromName.definition);
                }
            }
            this.FireInformationMessage(errorMsg);
        }

        public event AARepositoryManager.StringDelegate RepositoryChanged;

        private void FireRepositoryChanged()
        {
            if (this.RepositoryChanged == null)
                return;
            this.RepositoryChanged(this.generateAAAndStatusList());
        }

        public event AARepositoryManager.AAActivationDel AAChanged;

        private void FireChangeInActivationOfAAEvent(bool add, string name, string def)
        {
            if (this.AAChanged == null)
                return;
            this.AAChanged(add, name, def);
        }

        public event AARepositoryManager.AAChangeDefDel AAUpdateDefinition;

        private void FireChangeAADefinition(string name, string def)
        {
            if (this.AAUpdateDefinition == null)
                return;
            this.AAUpdateDefinition(name, def);
        }

        public event AARepositoryManager.StringDelegate AADefinition;

        private void FireAADefinitionEvent(string val)
        {
            if (this.AADefinition == null)
                return;
            this.AADefinition(val);
        }

        public event AARepositoryManager.StringDelegate InformationMessage;

        private void FireInformationMessage(string msg)
        {
            if (this.InformationMessage == null)
                return;
            this.InformationMessage(msg);
        }

        private string getAAnameFromDefinition(string definition)
        {
            int num1 = definition.IndexOf("advice");
            if (num1 == -1)
                return string.Empty;
            int startIndex = checked(num1 + 7);
            if (startIndex == definition.Length)
                return string.Empty;
            int num2 = definition.IndexOf('(', startIndex);
            if (num2 == -1)
                return string.Empty;
            string str = definition.Substring(startIndex, checked(num2 - startIndex));
            return str.Contains(",") ? string.Empty : str.TrimStart((char[])null).TrimEnd((char[])null);
        }

        private string generateAAAndStatusList()
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = true;
            foreach (AARepositoryManager.AAStruct aa in this.AAs)
            {
                if (!flag)
                    stringBuilder.Append(",");
                else
                    flag = false;
                stringBuilder.AppendFormat("{0},{1}", (object)aa.name, (object)aa.selected.ToString());
            }
            return stringBuilder.ToString();
        }

        private AARepositoryManager.AAStruct GetAAFromName(string name)
        {
            int index = this.AAs.FindIndex((Predicate<AARepositoryManager.AAStruct>)(aa => aa.name == name));
            return index == -1 ? (AARepositoryManager.AAStruct)null : this.AAs[index];
        }

        private class AAStruct
        {
            public string name;
            public string definition;
            public string filename;
            public bool selected;
            public bool saved;
        }

        public delegate void StringDelegate(string val);

        public delegate void AAActivationDel(bool add, string AAName, string AADefinition);

        public delegate void AAChangeDefDel(string AAName, string AADefinition);
    }
}