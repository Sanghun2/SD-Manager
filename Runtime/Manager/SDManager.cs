using System;
using System.Collections.Generic;
using UnityEngine;

namespace BilliotGames
{
    public class SDManager
    {
        private Dictionary<Type, SDContainerBase> sdContainerDict = new Dictionary<Type, SDContainerBase>();

        public int TryGetCount<TSD>() where TSD : SDBase {
            if (TryGetContainer(out SDContainerBase<TSD> container)) {
                return container.Count;
            }

            int errorResult = -1;
            Debug.LogError($"<color=red>{typeof(TSD)}에 해당하는 sd container를 찾을 수 없음</color>");
            return errorResult;
        }
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
            if (sdContainerDict.TryAdd(typeof(TSD), sdContainerBase)) {
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


        private bool TryGetContainer<TSD>(out SDContainerBase<TSD> container) where TSD : SDBase {
            if (sdContainerDict.TryGetValue(typeof(TSD), out var containerBase)) { // 특정 type container찾는 부분 함수로 빼기
                container = containerBase as SDContainerBase<TSD>;
                return true;
            }

            container = null;
            return false;
        }
    }

}
