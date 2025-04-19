using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public static GameUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject rewindUI;
    [SerializeField] private GameObject thrusterUI;
    [SerializeField] private Slider trustSlider;
    [SerializeField] private GameObject capturingUI;
    [SerializeField] private GameObject capturingDistanceUI;
    [SerializeField] private GameObject hpUI;
    [SerializeField] private Text hpText;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject finishUI;
    

    [Header("triggers")]
    public bool isRewind;
    public bool isCapturing;
    public bool isStabilizating;
    public bool isThrusting;
    public bool isLanding;
    public bool isDistanceToCapture;
    public bool isPause;
    public bool isFinish;
    public bool isAbleToBeHit;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void UIChecker()
    {
        if (isRewind) rewindUI.SetActive(true); else rewindUI.SetActive(false);
        if (isThrusting) thrusterUI.SetActive(true); else thrusterUI.SetActive(false);
        if (isCapturing) capturingUI.SetActive(true); else capturingUI.SetActive(false);
        if (isDistanceToCapture && !isCapturing) capturingDistanceUI.SetActive(true); else capturingDistanceUI.SetActive(false);
        if (isPause) pauseMenu.SetActive(true); else pauseMenu.SetActive(false);
        if (isFinish) finishUI.SetActive(true); else finishUI.SetActive(false);
        if (isAbleToBeHit) hpUI.SetActive(true); else hpUI.SetActive(false);
    }

    private void Update()
    {
        UIChecker();
    }

    public void SetHPAmount(int hp)
    {
        if (hp > 0)
        hpText.text = hp + " HP";
        else
            hpText.text = "---___---";
    }
    public void SetHPSlider(float value)
    {
        hpSlider.value = value;
    }
    public void SetThrustSliderUI(float value)
    {
        trustSlider.value = value;
    }
}
