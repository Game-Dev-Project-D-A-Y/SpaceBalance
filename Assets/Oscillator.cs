using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField]
    float timeCounter = 0;

    [SerializeField]
    float speed;

    [SerializeField]
    float width;

    [SerializeField]
    float heigt;

    float x;
    float y;

    [SerializeField]
    float heightOfPlanet = 5;

    [SerializeField]
    float z = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * speed;

        x = Mathf.Cos(timeCounter) * width;
        y = heightOfPlanet + Mathf.Sin(timeCounter) * heigt;
        transform.position = new Vector3(x, y, z);
    }
}
