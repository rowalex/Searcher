using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManagerFields
{
    public bool isAbleToMove;
    public bool isMouseWork;
    public bool isAbleToThrust;
    public bool isAbleToStabizate;
    public bool isAbleToLand;
    public bool isAbleToRewind;
    public bool isAbleToCapture;
    public int currHP;
    public float regenTimer;
    public float currFuel;
    public GameManagerFields(GameManager obj)
    {
        isAbleToMove = obj.isAbleToMove;
        isMouseWork = obj.isMouseWork;
        isAbleToThrust = obj.isAbleToThrust;
        isAbleToStabizate = obj.isAbleToStabizate;
        isAbleToLand = obj.isAbleToLand;
        isAbleToRewind = obj.isAbleToRewind;
        isAbleToCapture = obj.isAbleToCapture;
        currHP = obj.playerInf.hpAmount;
        regenTimer = obj.regenTimer;
        currFuel = obj.currFuel;

    }

    public void GetField(GameManager obj)
    {
        obj.isAbleToMove = isAbleToMove;
        obj.isMouseWork = isMouseWork;
        obj.isAbleToThrust = isAbleToThrust;
        obj.isAbleToStabizate = isAbleToStabizate;
        obj.isAbleToLand = isAbleToLand;
        obj.isAbleToRewind = isAbleToRewind;
        obj.isAbleToCapture = isAbleToCapture;
        obj.playerInf.hpAmount = currHP;
        obj.regenTimer = regenTimer;
        obj.currFuel = currFuel;
    }

    public void GetPossibilities(GameManager obj)
    {
        obj.isAbleToMove = isAbleToMove;
        obj.isMouseWork = isMouseWork;
        obj.isAbleToThrust = isAbleToThrust;
        obj.isAbleToStabizate = isAbleToStabizate;
        obj.isAbleToLand = isAbleToLand;
        obj.isAbleToRewind = isAbleToRewind;
        obj.isAbleToCapture = isAbleToCapture;
    }


}

public class GameManager : MonoBehaviour
{
    public bool isTimeGoes = true;
    public Movement movementScript;
    [Header("Move")]
    public bool isAbleToMove;
    public bool isMoving;
    [Header("Mouse")]
    public bool isMouseWork;
    public bool isMouseMoving;
    [Header("Thrust")]
    public bool isAbleToThrust;
    private bool isThurustUI;
    public bool isThrusting;
    public float currFuel;
    public float maxFuel;
    public float thrustCost;
    public float refuelSpeed;
    [Header("Stabialize")]
    public bool isAbleToStabizate;
    public bool isStabilizating;
    [Header("Landing")]
    public bool isAbleToLand;
    public bool isLanding;
    public bool isLanded;
    [Header("Rewind")]
    public bool isAbleToRewind;
    public bool isRewind;
    public float timeForRewind;
    private List<GameManagerFields> fields;
    [Header("Capture")]
    public Capturing capturingScript;
    public bool isAbleToCapture;   
    public bool isCapturing;
    public bool isDistanceToCapture;
    public float timeToCapture;
    public float captureDistance;
    public LayerMask captureMask;
    public KeyCode captureKey = KeyCode.F;
    [Header("Invisibility")]
    public bool isAbleToInvis;
    public bool isInvis;
    public float timeToInvis;
    public Invisibility invisScript;

    [Header("UI")]
    public GameObject rewindUI;
    public GameObject stabilizerUI;
    public GameObject thrusterUI;
    public Slider trustSlider;
    public GameObject landingUI;
    public GameObject capturingUI;
    public GameObject capturingDistanceUI;

    [Header("Menu")]
    public KeyCode pauseKey = KeyCode.Escape;
    public GameObject pauseMenu;
    public bool isPause;
    private bool isAbleToPause = true;

    [Header("HP")]
    public bool isAbleToBeHit;
    public GameObject hpUI;
    public int maxHP;
    public Slider slider;
    public PlayerBaseInfo playerInf;
    public Text hpText;
    public float regenTimer; 
    public float regenTime;

    [Header("Finish Level UI")]
    public GameObject finishUI;
    public bool isFinish = false;

    [Header("Enemies")]
    public bool isEnemiesWork;



    private GameManagerFields buff;

    void UIChecker()
    {
        if (isRewind) rewindUI.SetActive(true); else rewindUI.SetActive(false);
        if (isStabilizating) stabilizerUI.SetActive(true); else stabilizerUI.SetActive(false);
        if (isThurustUI) thrusterUI.SetActive(true); else thrusterUI.SetActive(false);
        if (isLanding) landingUI.SetActive(true); else landingUI.SetActive(false);
        if (isCapturing) capturingUI.SetActive(true); else capturingUI.SetActive(false);
        if (isDistanceToCapture && !isCapturing) capturingDistanceUI.SetActive(true); else capturingDistanceUI.SetActive(false);
        if (isPause) pauseMenu.SetActive(true); else pauseMenu.SetActive(false);
        if (isFinish) finishUI.SetActive(true); else finishUI.SetActive(false);
        if (isAbleToBeHit) hpUI.SetActive(true); else hpUI.SetActive(false);
    }


    void Start()
    {
        fields = new List<GameManagerFields>();
        buff = new GameManagerFields(this);
        currFuel = maxFuel;
        isThurustUI = isAbleToThrust;

        rewindUI.SetActive(false);
        stabilizerUI.SetActive(false);
        thrusterUI.SetActive(false);
        landingUI.SetActive(false);
        capturingUI.SetActive(false);
        capturingDistanceUI.SetActive(false);
        isPause = false;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        finishUI.SetActive(false);
        
    }
    void Update()
    {
        isInvis = invisScript.isVisible;
        isMoving = movementScript.isMoving;
        isMouseMoving = movementScript.isMouseMoving;
        isStabilizating = movementScript.isStabializing;
        isThrusting = movementScript.isThrusting;
        isLanding = movementScript.isLanding;
        isRewind = FindAnyObjectByType<TimeBody>().isRewind;
        isCapturing = capturingScript.isCapturing;
        isDistanceToCapture = capturingScript.isDistanceToCapture;

        if (isAbleToPause) 
        {
            if (Input.GetKeyDown(pauseKey) && isPause)
                isPause = false;
            else if (Input.GetKeyDown(pauseKey))
                isPause = true;

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

        if (isAbleToBeHit)
        {
            if (playerInf.hpAmount <= 0)
                DisableThePlayer();

            HPManager();
        }
        else
            playerInf.hpAmount = maxHP;

        if (isAbleToLand)
            PlayerInvisManager();

        ThrustManager();


        if (isFinish)
        {
            DisableTheGame();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }



        UIChecker();
    }

    private void FixedUpdate()
    {
        RewindGameManager();
    }

    private void RewindGameManager()
    {
        if (isRewind)
        {
            isTimeGoes = false;
            if (fields.Count > 0)
            {
                fields[0].GetField(this);
                fields.RemoveAt(0);
            }
        }
        else
        {
           isTimeGoes = true;
           if (fields.Count > (timeForRewind / Time.fixedDeltaTime) - 1) fields.RemoveAt(fields.Count - 1);
           fields.Insert(0, new GameManagerFields(this));
        }
    }

    private void HPManager()
    {
        if (playerInf.hpAmount > maxHP) playerInf.hpAmount = maxHP;

        if (playerInf.hpAmount > 0)
            hpText.text = playerInf.hpAmount + " HP";
        else
            hpText.text = "---___---";

        if (playerInf.timer == playerInf.immunityWindow)
            regenTimer = regenTime;

        if (maxHP > playerInf.hpAmount && playerInf.hpAmount > 0)
        {

            if (isTimeGoes)
                regenTimer -= Time.deltaTime;
            if (regenTimer < 0)
            {
                playerInf.hpAmount++;
                regenTimer = regenTime;
            }

            if (regenTimer > 0)
                slider.value = (regenTime - regenTimer) / regenTime;
            else
                slider.value = 0;
        }

    }

    private void PlayerInvisManager()
    {


        if(isLanding)
        {
            isAbleToMove = false;
            isAbleToThrust = false;
            isAbleToStabizate = false;
            isLanded = false;
            isMouseWork = false;
        }
        else if (!isLanding && movementScript.GettingContact() && !isMoving)
        {
            isLanded = true;
        }
        else
        {
            isLanded = false;
        }
        if (!isLanding)
        {
            buff.GetPossibilities(this);
        }
    }

    private void ThrustManager()
    {
        if (currFuel < maxFuel)
            currFuel += refuelSpeed * Time.deltaTime;
        else
            currFuel = maxFuel;

        trustSlider.value = currFuel / maxFuel;
    }

    public bool FuelReduce()
    {
        if (currFuel >= thrustCost)
        {
            currFuel -= thrustCost;
            return true;
        }else return false;
    }

    private void DisableThePlayer()
    {
        isAbleToCapture = false;
        isAbleToLand = false;
        isAbleToMove = false;
        isAbleToStabizate = false;
        isAbleToThrust = false;
        isMouseWork = false;
    }

    public void TurnGameOn()
    {
        isPause = false;
    }
    public void DisableTheGame()
    {
        DisableThePlayer();
        isEnemiesWork = false;
        isAbleToPause = false;
    }
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
