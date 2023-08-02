using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    private List<Sentence> sentences;
    public GameObject dialogWindow;
    public Text dialogSentence;
    public Text dialogName;

    public float dialogTimer = 0;
    public bool isDialog = false;


    void Awake()
    {
        isDialog = false;
        sentences = new List<Sentence>();
        dialogWindow.SetActive(false);
    }


    void Update()
    {
        if (sentences.Count > 0)
            isDialog = true;

        if (isDialog && dialogTimer <= 0)
        {
            OpenWindow();
            SetParams();
        }
        if (isDialog)
            dialogTimer -= Time.deltaTime;

        if (dialogTimer <= 0)
        {
            isDialog = false;
        }

        if (!isDialog)
        {
            CloseWindow();
        }
    }

    public void AddSentence(Sentence newSentence)
    {
        sentences.Add(newSentence);
    }
    public void SetParams()
    {
        dialogSentence.text = sentences[0].sentence;
        dialogName.text = sentences[0].name;
        dialogTimer = sentences[0].time;
        sentences.RemoveAt(0);
    }

    public void OpenWindow()
    {
        dialogWindow.SetActive(true);
    }
    public void CloseWindow()
    {
        dialogWindow.SetActive(false);
    }

}
