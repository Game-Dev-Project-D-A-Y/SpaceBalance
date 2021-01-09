using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpaceShip : MonoBehaviour
{
   private Animator anim;

    void Start()
    {
        anim=GetComponent<Animator>();
        anim.SetBool("drive",true);
    }

    void Update()
    {
        
    }
}
