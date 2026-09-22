// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.Superposition
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WComp.Beans;
using WComp.Util;

#nullable disable
namespace WComp.AADesigner;

[Bean(Category = "AADesigner-dev")]
public class Superposition
{
  private const int MAX_TABLE_SIZE = 100;
  private List<AspectOfAssembly>[] listOfAdviceInstances;
  private int[] indexTab;
  private int nbReceivedAA;
  private int nbActiveAA;
  private bool stopCycle;

  public Superposition()
  {
    this.listOfAdviceInstances = new List<AspectOfAssembly>[100];
    this.indexTab = new int[100];
    this.nbActiveAA = 0;
    this.nbReceivedAA = 0;
    this.stopCycle = false;
    this.Clean();
  }

  [DefaultValue(false)]
  public bool StopCycle
  {
    get => this.stopCycle;
    set
    {
      if (value)
        Logger.Debug("STOPPING CYCLE (Superposition){0}", (object) Environment.NewLine);
      this.stopCycle = value;
    }
  }

  public void update_Advice(List<AspectOfAssembly> adviceInstances, int[] adviceInfo)
  {
    Logger.Info("--> Entering Superposition");
    if (this.stopCycle)
    {
      Logger.Info("<-- Exiting Superposition (cycle interrupted).");
    }
    else
    {
      if (adviceInstances == null)
        return;
      if (adviceInfo == null || adviceInfo.Length != 2 || adviceInfo[1] == 0)
      {
        Logger.Fatal("ERROR superposition: advice info is not a valid int[2].");
      }
      else
      {
        Logger.Debug("Superposition: received {0} instances for AA number {1} of {2} selected", (object) adviceInstances.Count, (object) adviceInfo[0], (object) adviceInfo[1]);
        this.nbActiveAA = adviceInfo[1];
        checked { ++this.nbReceivedAA; }
        if (this.nbActiveAA > 99)
          throw new Exception("Exceed the maximum number of selected AA supported by the weaver: " + (object) 100);
        int index1 = 0;
        while (index1 < 100)
        {
          if (this.indexTab[index1] == adviceInfo[0])
          {
            this.listOfAdviceInstances[index1] = adviceInstances;
            break;
          }
          checked { ++index1; }
        }
        if (index1 == 100)
        {
          int index2 = 0;
          while (this.indexTab[index2] != -1)
            checked { ++index2; }
          this.listOfAdviceInstances[index2] = adviceInstances;
          this.indexTab[index2] = adviceInfo[0];
        }
        if (this.nbReceivedAA >= this.nbActiveAA && this.nbReceivedAA < checked (2 * this.nbActiveAA))
        {
          int num = 0;
          int index3 = 0;
          while (index3 < 100)
          {
            if (this.indexTab[index3] != -1)
              checked { ++num; }
            checked { ++index3; }
          }
          if (num < this.nbActiveAA)
            Logger.Debug("Waiting for events of other selected AAs to start composition");
          else if (num > this.nbActiveAA)
          {
            Logger.Error("IMPOSSIBLE SYNCHRONIZATION CASE IN SUPERPOSITION - reseting");
            this.Clean();
          }
          else
          {
            this.nbReceivedAA = 0;
            this.FireCompositionEvent(this.get_Superposition());
          }
        }
        else if (this.nbReceivedAA > checked (2 * this.nbActiveAA))
        {
          Logger.Fatal("Error in superposition: received more lists of advice instances than the number of AA ({0}/{1})", (object) this.nbReceivedAA, (object) this.nbActiveAA);
          this.Clean();
          throw new Exception("There was probably a bug in the weaver, synchronization of computing of all selected AAs cannot be done");
        }
      }
    }
  }

  public List<AdviceRule> get_Superposition()
  {
    Logger.Debug("Starting superposition");
    if (this.nbActiveAA == 0)
      Logger.Error("ERROR in Superposition: number of active AA = 0 SHOULD NOT HAPPEN");
    List<AdviceRule> superposition = new List<AdviceRule>();
    int index = 0;
    while (index < 100)
    {
      if (this.indexTab[index] != -1)
      {
        foreach (AspectOfAssembly aspectOfAssembly in this.listOfAdviceInstances[index])
        {
          foreach (AdviceRule adviceRule in aspectOfAssembly.Advice)
            superposition.Add(adviceRule);
        }
      }
      checked { ++index; }
    }
    return superposition;
  }

  public void RemoveAA(int id)
  {
    if (this.nbActiveAA == 0)
      return;
    int index = 0;
    while (index < 100 && this.indexTab[index] != id)
      checked { ++index; }
    if (index >= 100)
    {
      Logger.Warn("Possible error: position of AA not found in index - OK if no cycle performed");
    }
    else
    {
      this.indexTab[index] = -1;
      this.listOfAdviceInstances[index] = (List<AspectOfAssembly>) null;
      checked { --this.nbActiveAA; }
      if (this.nbActiveAA == 0)
      {
        Logger.Info("All AA removed, weaving back initial assembly");
        this.FireCompositionEvent(new List<AdviceRule>());
      }
      else
      {
        Logger.Info("AA {0} was removed, we launch a new weaving cycle", (object) index);
        this.FireRemovedEvent();
      }
      this.nbReceivedAA = 0;
      Logger.Info("<-- EXIT from RemoveAA (stack return)");
    }
  }

  public event Superposition.EventCompositionHandler compositionLAdvicesEvent;

  private void FireCompositionEvent(List<AdviceRule> listAdvices)
  {
    if (this.stopCycle)
    {
      Logger.Info("<-- Exiting Superposition (cycle interrupted).");
    }
    else
    {
      Logger.Info("<-- Sending superposed list of {0} rules", (object) listAdvices.Count);
      if (this.compositionLAdvicesEvent == null)
        return;
      this.compositionLAdvicesEvent(listAdvices);
    }
  }

  public event Superposition.EventRemovedHandler compositionRemoveEvent;

  private void FireRemovedEvent()
  {
    if (this.compositionRemoveEvent == null)
      return;
    this.compositionRemoveEvent();
  }

  public void Clean()
  {
    int index = 0;
    while (index < 100)
    {
      this.indexTab[index] = -1;
      this.listOfAdviceInstances[index] = (List<AspectOfAssembly>) null;
      checked { ++index; }
    }
    this.nbReceivedAA = 0;
    this.nbActiveAA = 0;
  }

  public delegate void EventCompositionHandler(List<AdviceRule> listAdvices);

  public delegate void EventRemovedHandler();
}
