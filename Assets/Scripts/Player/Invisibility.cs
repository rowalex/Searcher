using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    public GameManager gameManager;

    public bool isVisible = true;

    public Material[] material;

    private float inVisTimer;

    private float timer;

    private void Start()
    {
        inVisTimer = gameManager.timeToInvis;
    }
    private void Update()
    {
        if (gameManager.isAbleToInvis)
        {


            if (gameManager.isLanded)
            {
                timer += Time.deltaTime;
                if (timer >= inVisTimer)
                    isVisible = false;
                else
                    isVisible = true;
            }
            else
            {
                timer = 0;
                isVisible = true;
            }
        }
        foreach (Material a in material)
        {       
            Color col = a.color;
            col.a = (inVisTimer - timer) / inVisTimer < 0 ? 0 : (inVisTimer - timer) / inVisTimer;
            a.color = col;
        }
        
    }



}
