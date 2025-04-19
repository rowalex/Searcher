using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    [HideInInspector]
    public static RewindManager Instance;

    [SerializeField] private KeyCode buttonToRewind;
    public bool isRewind;

    private int time;

    public Action OnRewind;
    public Action OnSaveInfo;
    public Action OnClear;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public void ClearInfo()
    {
        OnClear?.Invoke();
    }

    public void FixedUpdate()
    {
        if (isRewind && time > 0)
        {
            time--;
            OnRewind?.Invoke();
        }
        else
        {
            time++;
            OnSaveInfo?.Invoke();
        }

    }

    public void Update()
    {
        if (Input.GetKey(buttonToRewind))
        {
            isRewind = true;
        }
        else
        {
            isRewind = false;
        }
        GameManager.Instance.SetRewindUI(isRewind);
    }

}
