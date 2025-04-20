using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    private GameManager gameManager;
    private Movement movement;
    [SerializeField] public bool isVisible = true;
    [SerializeField] private Material[] material;
    private float timer;
    [SerializeField] private bool isAbleToInvis;
    [SerializeField] private float timeToInvis;
    public float alpha = 1;

    private void Start()
    {
        gameManager = GameManager.Instance;
        movement = GetComponent<Movement>();
    }
    private void Update()
    {
        bool isLanded = movement.GettingContact() && !movement.isMoving;

        if (isAbleToInvis && !gameManager.IsRewind())
        {

            if (isLanded)
            {
                timer += Time.deltaTime;
                if (timer >= timeToInvis)
                    isVisible = false;
                else
                    isVisible = true;
            }
            else
            {
                timer = 0;
                isVisible = true;
            }

            alpha = (timeToInvis - timer) / timeToInvis < 0 ? 0 : (timeToInvis - timer) / timeToInvis;
        }
        else
        {
            alpha = 1;
            isVisible = true;
        }

        SetInvis(alpha);
    }

    private void SetInvis(float alpha)
    {
        foreach (Material a in material)
        {       
            Color col = a.color;
            col.a = alpha;
            a.color = col;
        }
    }

}
