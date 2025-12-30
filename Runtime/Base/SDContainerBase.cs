using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class SDContainerBase
{
    public abstract void InitSD();
    public abstract void ReleaseSD();
}

public abstract class SDContainerBase<TSD> : SDContainerBase where TSD : SDBase
{
    public enum Process {
        Available,
        InProress,
        Done,
    }

    public string SDResourcePath => _sdResourcePath;
    public bool IsInit => _isInit;

    private Dictionary<string, TSD> sdDict = new Dictionary<string, TSD>();
    private Process currentProcess;

    private bool _isInit;
    private string _sdResourcePath;
    private Func<string[], string> keyCreateAction = null;

    private StringBuilder keyBuilder;

    public SDContainerBase(string sdResourcePath) {
        if (string.IsNullOrEmpty(_sdResourcePath) && string.IsNullOrEmpty(sdResourcePath)) { Debug.LogWarning($"<color=orange>sd resource path는 empty인 경우 문제가 될 수 있습니다.</color>"); }
        this._sdResourcePath = sdResourcePath;
    }

    public override void InitSD() {

        if (currentProcess != Process.Available) {
            return;
        }

        currentProcess = Process.InProress;
        sdDict.Clear();

        TSD[] loadedSDs = Resources.LoadAll<TSD>(_sdResourcePath);
        for (int i = 0; i < loadedSDs.Length; i++) {
            var targetSD = loadedSDs[i];
            string key = GenerateFindKey(targetSD.ID); // 이 부분 따로 커스텀 할 수 있게 만들어야 함
            if (!sdDict.TryAdd(key, targetSD)) {
                Debug.LogError($"<color=red>{key} add dictionary failed</color>");
            }
        }

        currentProcess = Process.Done;
        _isInit = true;
    }
    public override void ReleaseSD() {
        sdDict.Clear(); 
    }

    public void SetFindKeyAction(Func<string[], string> keyCrateAction) {
        this.keyCreateAction = keyCrateAction;
    }

    public bool TryGetSD(string key, out TSD targetSD) {
        if (!IsInit) InitSD();
        if (sdDict.TryGetValue(key, out targetSD)) {
            return true;
        }

        Debug.LogError($"<color=red>{key}에 해당하는 data null</color>");
        targetSD = null;    
        return false;
    }


    private string GenerateFindKey(params string[] keys) {
        return keyCreateAction == null ? DefaultFindKey(keys) : keyCreateAction(keys);
    }
    private string DefaultFindKey(params string[] keys) {
        if (keyBuilder == null) {
            keyBuilder = new StringBuilder();
        }

        keyBuilder.Clear();
        return keyBuilder.AppendJoin('_', keys).ToString();
    }
}
