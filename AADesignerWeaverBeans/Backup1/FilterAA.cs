// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.FilterAA
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class FilterAA
{
  private bool propagate = true;
  private bool newCycleRequested = false;
  private bool freezed = false;

  public bool PropagateContainerEvent
  {
    get => this.propagate;
    set
    {
      if (this.freezed)
        return;
      if (!this.propagate && value && this.newCycleRequested)
      {
        this.newCycleRequested = false;
        Logger.Debug("--> FilterAA.set_Propagate({0}), now sends ModificationOnContainer_event", (object) value);
        this.FireModificationOnContainerEvent();
        Logger.Debug("<-- FilterAA.set_Propagate({0}) returning", (object) value);
      }
      this.propagate = value;
      if (this.propagate)
        Logger.Info("=========== RECEIVING EVENTS FROM CONTAINER =============");
      else
        Logger.Info("=========== DROPPING EVENTS FROM CONTAINER =============");
    }
  }

  public bool FreezeWeaver
  {
    set
    {
      this.freezed = value;
      this.PropagateContainerEvent = !this.freezed;
    }
    get => this.freezed;
  }

  public string StartWeavingCycle
  {
    set
    {
      if (string.IsNullOrEmpty(value))
        return;
      string[] strArray = value.Split('|');
      if (strArray.Length < 2)
        return;
      if (strArray[1] == "ERROR")
        this.FireAllowStructuralMod(false);
      else if (!this.propagate)
      {
        if (!(strArray[1] == "DEL_BEAN") || strArray[2].StartsWith("AA_"))
          return;
        this.newCycleRequested = true;
      }
      else if (strArray[1] == "NEW_BEAN" || strArray[1] == "DEL_BEAN" || strArray[1] == "NOTICE" && strArray[2] == "NEWNAME")
        this.FireModificationOnContainerEvent();
    }
    get => string.Empty;
  }

  public void launchInternalUpdate() => this.FireModificationOnContainerEvent();

  public event FilterAA.EventModificationOnContainerHandler ModificationOnContainer_event;

  private void FireModificationOnContainerEvent()
  {
    if (this.ModificationOnContainer_event == null)
      return;
    this.ModificationOnContainer_event();
  }

  public event FilterAA.AllowStructuralModificationsHandler AllowStructuralModifications;

  private void FireAllowStructuralMod(bool allow)
  {
    if (this.AllowStructuralModifications == null)
      return;
    this.AllowStructuralModifications(allow);
  }

  public delegate void EventModificationOnContainerHandler();

  public delegate void AllowStructuralModificationsHandler(bool allow);
}
