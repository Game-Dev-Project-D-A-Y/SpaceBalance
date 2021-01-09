using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMover : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        anim.SetBool("drive",true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
