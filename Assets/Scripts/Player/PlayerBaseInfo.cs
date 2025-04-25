using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerBaseInfo : MonoBehaviour
{
    private GameManager gameManager;

    [Header("HP")]
    [SerializeField] private bool isAbleToBeHit;
    [SerializeField] public float regenTimer;
    [SerializeField] private float regenTime;
    [SerializeField] private float movementSpeed;
    [SerializeField] public Vector3 movementVector;
    private Vector3 prevPos;
    private Vector3 nextPos;
    [SerializeField] private float hitValue;
    [SerializeField] private float timer;
    [SerializeField] private float immunityWindow = 1;
    [SerializeField] public int hpAmount;
    [SerializeField] private int hpMax = 3;

    private void Start()
    {
        nextPos = transform.position;
        gameManager = GameManager.Instance;
        gameManager.SetHPUI(isAbleToBeHit);
        hpAmount = hpMax;
    }

    private void FixedUpdate()
    {   prevPos = nextPos;  
        nextPos = transform.position;
        movementSpeed = Vector3.Distance(prevPos, nextPos) / Time.deltaTime;
        movementVector = nextPos - prevPos;

        if (timer == immunityWindow)
        {
            hpAmount--;
        }
        timer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && timer < 0)
        {
            timer = immunityWindow;
            gameObject.GetComponent<Rigidbody>().AddForce(collision.impulse.normalized * 10, ForceMode.Impulse);
            return;
        }
        var impact = collision.impulse.magnitude;
        impact *= collision.gameObject.GetComponent<EnemiesAI>() ? 5 : 1;
        Debug.Log(impact);
        if (impact > hitValue && timer < 0)
        {
            SoundManager.Instance.Play("strong_hit");
            timer = immunityWindow;
        }else if (impact > 4)
        {
            SoundManager.Instance.Play("normal_hit");
        }
    }

    private void Update()
    {

        if (regenTimer > 0)
            gameManager.SetHPSlide((regenTime - regenTimer) / regenTime);
        else
            gameManager.SetHPSlide(0);

        gameManager.SetHP(hpAmount);

        if (isAbleToBeHit && !gameManager.IsRewind() )
        {
            if (hpAmount <= 0)
                DisablePlayer();

            HPManager();

        }
    }

    private void HPManager()
    {
        if (timer == immunityWindow)
            regenTimer = regenTime;

        if (hpMax > hpAmount && hpAmount > 0)
        {

            if (!gameManager.IsRewind())
                regenTimer -= Time.deltaTime;
            if (regenTimer < 0)
            {
                hpAmount++;
                regenTimer = regenTime;
            }
        }
    }

    public void DisablePlayer()
    {
        GetComponent<Movement>().isAbleToMove = false;
    }

}
