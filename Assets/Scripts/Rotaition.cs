using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotaition : MonoBehaviour
{
    [SerializeField]
    float rotate = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Rotate(new Vector3(0, rotate,0 ));

       
        
    }
}
