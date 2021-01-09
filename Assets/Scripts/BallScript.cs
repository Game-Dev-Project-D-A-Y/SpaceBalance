﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*   Script that handles ball triggers
*/
public class BallScript : MonoBehaviour
{
    [Tooltip("Reference to the GameManager")]
    [SerializeField] GameManager gm;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Alien")) 
        {
            gm.OnBottlePicked(other.gameObject);
        }

        if (other.CompareTag("BottomBorder"))
        {
            gm.OnBorderHit(this.gameObject);
        }

        if (other.CompareTag("BlackHole"))
        {
            gm.OnBlackHole(this.gameObject);
        }
    }
}