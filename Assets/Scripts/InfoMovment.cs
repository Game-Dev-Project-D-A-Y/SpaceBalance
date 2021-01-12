using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMovment : MonoBehaviour
{
    [SerializeField]
    float scrollSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 localVectorUp = transform.TransformDirection(0, 1, 0);

        pos += localVectorUp * scrollSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
