using System.Collections;
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
            gm.OnAlienPicked(other.gameObject);
        }

        if (other.CompareTag("BottomBorder"))
        {
            gm.OnBorderHit(this.gameObject);
        }

        if (other.CompareTag("BlackHole"))
        {
            gm.OnBlackHole(this.gameObject);
        }

        if (other.CompareTag("BaseBonusObject"))
        {
            gm.OnBaseBonusPicked(other.gameObject);
        }
        if (other.CompareTag("BlackHolesBonusObject"))
        {
            Debug.Log("BlackHolesBonusObject tag");
            gm.OnBlackHolesBonusPicked(other.gameObject);
        }
    }
}