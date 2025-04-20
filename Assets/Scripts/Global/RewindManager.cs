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
        time = 0;
        OnClear?.Invoke();
    }

    public void FixedUpdate()
    {
        if (time == 0)
        {
            isRewind = false;
        }

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
        if (Input.GetKeyDown(buttonToRewind))
        {
            isRewind = true;
        }
        else if (Input.GetKeyUp(buttonToRewind))
        {
            isRewind = false;
        }
        GameManager.Instance.SetRewindUI(isRewind);
    }

}
