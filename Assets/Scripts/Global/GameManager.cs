using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager Instance;
    private GameUI gameUI;

    [Header("Menu")]
    public KeyCode pauseKey = KeyCode.Escape;
    public bool isPause;
    private bool isAbleToPause = true;

    [Header("Finish Level UI")]
    public bool isFinish = false;

    [Header("Enemies")]
    public bool isEnemiesWork;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        gameUI = GameUI.Instance;


    }

    public void SetRewindUI(bool isRewind)
    {
        gameUI.isRewind = isRewind;
    }
    public void SetStabializeUI(bool isStabilizating)
    {
        gameUI.isStabilizating = isStabilizating;
    }
    public void SetThrustingUI(bool isThrusting)
    {
        gameUI.isThrusting = isThrusting;
    }
    public void SetLandingUI(bool isLanding)
    {
        gameUI.isLanding = isLanding;
    }
    public void SetCapturingUI(bool isCapturing)
    {
        gameUI.isCapturing = isCapturing;
    }
    public void SetDistanceToCaptureBool(bool isDistanceToCapture)
    {
        gameUI.isDistanceToCapture = isDistanceToCapture;
    }
    public void SetPauseUI(bool isPause)
    {
        gameUI.isPause = isPause;
    }
    public void SetFinishUI(bool isFinish)
    {
        gameUI.isFinish = isFinish;
    }
    public void SetHPUI(bool isAbleToHit)
    {
        gameUI.isAbleToBeHit = isAbleToHit;
    }
    public void SetHP(int hp)
    {
        gameUI.SetHPAmount(hp);
    }
    public void SetHPSlide(float value)
    {
        gameUI.SetHPSlider(value);
    }
    public bool IsRewind()
    {
        return RewindManager.Instance.isRewind;
    }
    public void SetThrustSliderValue(float value)
    {
        gameUI.SetThrustSliderUI(value);
    }
    public void SetAbleToRewind(bool state)
    {
        RewindManager.Instance.isAbleToRewind = state;
    }

    void Update()
    {

        if (isAbleToPause)
        {
            if (Input.GetKeyDown(pauseKey) && isPause)
                isPause = false;
            else if (Input.GetKeyDown(pauseKey))
                isPause = true;

            gameUI.isPause = isPause;

            if (isPause)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (isFinish)
        {
            gameUI.isFinish = isFinish;
            DisableTheGame();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }
    public void DisableTheGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void TurnGameState(bool state)
    {
        isPause = state;
    }
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void SetEnemyFovVisible(bool isVisible)
    {
        var mass = FindObjectsOfType<EnemiesFOV>();
        Debug.Log(mass.Length);
        foreach(EnemiesFOV en in mass)
        {
            en.SetVisionLight(isVisible);
        }
    }
    public void SetMovementState(bool work)
    {
        FindFirstObjectByType<Movement>().isAbleToMove = work;
    }
}
