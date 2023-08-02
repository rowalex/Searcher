using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeBody : MonoBehaviour
{


    [Header("MainReferences")]
    private GameManager gameManager;

    [Header("TimeTravelBase")]
    public bool isAbleToRewind;
    List<BasicPointInTime> pointInTime;
    public bool isRewind;
    private float rewindTimer;
    public float timer;


    public void SetPossibilities()
    {
        isAbleToRewind = gameManager.isAbleToRewind;
        rewindTimer = gameManager.timeForRewind;
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetPossibilities();
        pointInTime = new List<BasicPointInTime>();
    }

    private void Update()
    {
        if (isAbleToRewind)
        {
            if (Input.GetKeyDown(KeyCode.Backspace)) StartRewind();
            if (Input.GetKeyUp(KeyCode.Backspace)) EndRewind();
        }
    }

    void FixedUpdate()
    {
        timer = Time.fixedDeltaTime * pointInTime.Count;
        if (isRewind) Rewind();
        else Record();
    }

    public void ClearHistory()
    {
        pointInTime.Clear();
    }
    public void Record()
    {
        if (pointInTime.Count > (rewindTimer / Time.fixedDeltaTime) - 1) pointInTime.RemoveAt(pointInTime.Count - 1);
        pointInTime.Insert(0, new BasicPointInTime(transform.position, transform.rotation));
    }

    public void Rewind()
    {


        if (pointInTime.Count > 0)
        {
            BasicPointInTime buff = pointInTime[0];
            transform.position = buff.position;
            transform.rotation = buff.rotation;
            pointInTime.RemoveAt(0);
        }else
        {
            EndRewind();
        }
    }

    public void StartRewind()
    {
        if (timer > 0)
        {
            MonoBehaviour[] comps = GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour c in comps)
            {
                c.enabled = false;
            }

            GetComponent<TimeBody>().enabled = true;
            isRewind = true;
            if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    public void EndRewind()
    {
        MonoBehaviour[] comps = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour c in comps)
        {
            c.enabled = true;
        }

        isRewind = false;
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = false;
    }
}
