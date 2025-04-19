using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITimeBody: MonoBehaviour
{

    protected RewindManager rewindManager;

    protected abstract void OnRewind();
    protected abstract void OnSaveInfo();
    protected abstract void OnClear();
}
