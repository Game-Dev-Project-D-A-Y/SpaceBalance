using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
michael
*   Class that manages game events
*/
public class GameManager : MonoBehaviour
{
    [Tooltip("The object of the surface")]
    [SerializeField]
    GameObject baseObject;

    [Tooltip("Bottle prefab to be respawn")]
    [SerializeField]
    GameObject alienToSpawn;

    [Tooltip("Black hole prefab to be respawn")]
    [SerializeField]
    GameObject blackHoleToSpawn;

    [Tooltip("Time text field to be edited")]
    [SerializeField]
    TextMeshPro gameTimeScoreTxt; 

    [Tooltip("Bottle timer text field to be edited")]
    [SerializeField]
    TextMeshPro alienTimerTxt; 

    [Tooltip("Number of bottle collected text field to be edited")]
    [SerializeField]
    TextMeshPro aliensCollectedTxt; 

    [Tooltip("Decrease bottle timer when achived given score")]
    [SerializeField]
    int scoreToReduceTimeAlien; 

    [Tooltip("Maximum time for collecting bottle")]
    [SerializeField]
    float maxTimeToCollectAlien;

    // Variable that holds the current time left to collect bottle
    private float timeLeftToCollectAlien;

    // Game timer
    private float timeGained; 

    // Indicates whether the game is running
    private bool isActive = true;

    // Number of bottles collected
    private int aliensCollected = 0; 

    // Start is called before the first frame update
    void Start()
    {
        timeLeftToCollectAlien = maxTimeToCollectAlien + 1;
        SpawnAlien();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

       // UpdateGameTime();
        AlienTime();
    }

    // PUBLIC METHODS

    // Method thats being called when bottle is picked
    public void OnAlienPicked(GameObject bottle)
    {
        Debug.Log("OnAlienPicked");
        SpawnAlien();
        Destroy (bottle);
        aliensCollected += 1;
        Debug.Log("aliensCollected:" + aliensCollected);
        aliensCollectedTxt.SetText("Score\n{0}", (int)aliensCollected);
        CheckAndUpdateAlienTime();
    }

    // Method thats being called when the ball hits the sea
    public void OnBorderHit(GameObject ball)
    {
        Destroy (ball);
        isActive = false;
        Debug.Log("OnBorderHit");
        LoadCurrnetScene();
    }

    // Method thats being called when the ball step on a black hole
    public void OnBlackHole(GameObject ball)
    {
        Destroy (ball);
        isActive = false;
        Debug.Log("OnBlackHole");
        LoadCurrnetScene();
    }

    // PRIVATE METHODS

    // Method that updates time of game
    private void UpdateGameTime() 
    {
        if (isActive)
        {
            timeGained += Time.deltaTime;
            gameTimeScoreTxt.SetText("Score\n{0}", (int)timeGained);
        }
    }

    // Method that checks and updates time left for collecting bottle
    private void CheckAndUpdateAlienTime()
    {
        if (
            aliensCollected >= scoreToReduceTimeAlien &&
            maxTimeToCollectAlien >= 5
        )
        {
            maxTimeToCollectAlien -= 1;
            scoreToReduceTimeAlien *= 2;
        }
        timeLeftToCollectAlien = maxTimeToCollectAlien;
    }

    // Method that sets time left for collecting bottle
    private void AlienTime()
    {
        timeLeftToCollectAlien -= Time.deltaTime;
        alienTimerTxt.SetText("Bottle Timer\n{0}", (int) timeLeftToCollectAlien);
        if (timeLeftToCollectAlien <= 1)
        {
            timeLeftToCollectAlien = maxTimeToCollectAlien + 1;
            GameObject bottleObject = GameObject.Find("Alien(Clone)");
            Destroy(bottleObject);
            CreateBlackHole(bottleObject);
            SpawnAlien();
        }
    }

    // Method that creates new black hole on the uncollected bottle
    private void CreateBlackHole(GameObject bottleObject)
    {
        GameObject newObject = Instantiate(blackHoleToSpawn.gameObject, bottleObject.transform.position, baseObject.transform.localRotation);
        newObject.transform.parent = baseObject.transform;
        newObject.transform.localPosition = new Vector3(newObject.transform.localPosition.x, 0.6f, newObject.transform.localPosition.z);
    }

    // Method that spawns a new bottle
    private void SpawnAlien()
    {
        Debug.Log("SpawnAlien");

        float scaleX = (baseObject.transform.localScale.x / 2f) - 2;
        float scaleZ = (baseObject.transform.localScale.z / 2f) - 2;
        float randomX = Random.Range(-scaleX, scaleX);
        float randomZ = Random.Range(-scaleZ, scaleZ);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);
        GameObject newObject = Instantiate(alienToSpawn.gameObject, randomPosition, baseObject.transform.localRotation);
        newObject.transform.parent = baseObject.transform;
        newObject.transform.localPosition = new Vector3(newObject.transform.localPosition.x, 0.3f, newObject.transform.localPosition.z);
    }
    private void LoadCurrnetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
