using System;
using System.Collections.Generic;
using UnityEngine;

public class SDManager
{
    private Dictionary<Type, SDContainerBase> sdContainerDict = new Dictionary<Type, SDContainerBase>();

    public bool TryGetSD<TSD>(string findKey, out TSD targetSD) where TSD : SDBase {
        Type type = typeof(TSD);
        if (sdContainerDict.TryGetValue(type, out var containerBase)) {
            var tsd = containerBase as SDContainerBase<TSD>;
            if (tsd.TryGetSD(findKey, out targetSD)) {
                return true;
            }

            Debug.LogError($"<color=red>{findKey}에 해당하는 targetSD가 없음</color>");
        }

        Debug.LogError($"<color=red>{type.GetType()}에 해당하는 SD ContainerBase가 없음</color>");
        targetSD = null;
        return false;
    }

    public bool TryRegisterSD<TSD>(SDContainerBase<TSD> sdContainerBase) where TSD : SDBase {
        if (sdContainerDict.TryAdd(sdContainerBase.GetType(), sdContainerBase)) {
            sdContainerBase.InitSD();
            return true;
        }

        Debug.LogError($"<color=red>SD container base [{sdContainerBase.GetType()}] register failed</color>");
        return false;
    }
    public void ReleaseSDs() {
        foreach (var sdContainerPair in sdContainerDict) {
            sdContainerPair.Value.ReleaseSD();
        }
    }
}
