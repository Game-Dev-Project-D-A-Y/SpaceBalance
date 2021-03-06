using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
*   Class that manages game events
*/
public class GameManager : MonoBehaviour
{
    [Tooltip("The object of the surface")]
    [SerializeField]
    GameObject baseObject;

    [Tooltip("Alien prefab to be respawn")]
    [SerializeField]
    GameObject alienToSpawn;

    [Tooltip("Black hole prefab to be respawn")]
    [SerializeField]
    GameObject blackHoleToSpawn;

    [SerializeField]
    GameObject baseBonusObjectToSpawn;

    [SerializeField]
    GameObject blackHolesBonusObjectToSpawn;

    [Tooltip("Time text field to be edited")]
    [SerializeField]
    TextMeshPro gameTimeScoreTxt;

    [Tooltip("Alien timer text field to be edited")]
    [SerializeField]
    TextMeshPro alienTimerTxt;

    [Tooltip("Number of aliens collected text field to be edited")]
    [SerializeField]
    TextMeshPro aliensCollectedTxt;

    [Tooltip("Decrease alien timer when achived given score")]
    [SerializeField]
    int scoreToReduceTimeAlien;

    [Tooltip("Maximum time for collecting bottle")]
    [SerializeField]
    float maxTimeToCollectAlien;

    [SerializeField]
    GameObject gameOverPanel;

       [SerializeField]
       int baseBonusChance;

       [SerializeField]
       int blackHolesBonusChance;

    // Variable that holds the current time left to collect alien
    private float timeLeftToCollectAlien;

    // Game timer
    private float timeGained;

    // Indicates whether the game is running
    private bool isActive = true;

    // Number of aliens collected
    private int aliensCollected = 0;

    private Vector3 baseScale;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        baseScale = baseObject.transform.localScale;

        timeLeftToCollectAlien = maxTimeToCollectAlien + 1;
        SpawnAlien();
    }

    [Header("For Debugging")]
    [SerializeField] private bool bonus = false;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        BaseSizeBonus(false);
        AlienTime();
    }

    private void RandomBaseBonusObject()
    {
        int rand = Random.Range(0,1000);
        //board size cahnge
        if(rand > baseBonusChance)
        {
            SpawnBaseBonusObject();
        }
        if(rand < blackHolesBonusChance){
            SpawnBlackHolesBonusObject();
        }
    }

    private void BlackHolesBonus(bool isActive){
        
        GameObject [] blackHoles = GameObject.FindGameObjectsWithTag("BlackHole");
        foreach(GameObject blackHole in blackHoles){
            blackHole.SetActive(false);        }

        Debug.Log("blackholes"+ blackHoles.Length);
    }

    private void BaseSizeBonus(bool isActive)
    {
        float newX,newY,newZ;
        Vector3 newScale;
        if (isActive)
        {
            newX = baseObject.transform.localScale.x * 2;
            newY = baseObject.transform.localScale.y;
            newZ = baseObject.transform.localScale.z * 2;
            newScale = new Vector3(newX, newY, newZ);
            SetBaseScale (newScale);
        }

        newX = baseObject.transform.localScale.x - Time.deltaTime;
        newY = baseObject.transform.localScale.y;
        newZ = baseObject.transform.localScale.z - Time.deltaTime;
        newScale = new Vector3(newX, newY, newZ);
        if (newX >= baseScale.x || newZ >= baseScale.z)
        {
            SetBaseScale (newScale);
        }
        else
        {
            SetBaseScale (baseScale);
        }
    }



    private void SetBaseScale(Vector3 newScale)
    {
        Transform[] children = new Transform[baseObject.transform.childCount];
        int i = 0;
        foreach (Transform T in baseObject.transform) children[i++] = T;

        baseObject.transform.DetachChildren();
        baseObject.transform.localScale = newScale;
        foreach (Transform T in children) T.parent = baseObject.transform;
    }

    // PUBLIC METHODS
    // Method thats being called when bottle is picked
    public void OnAlienPicked(GameObject alien)
    {
        RandomBaseBonusObject();
        SpawnAlien();
        Destroy (alien);
        aliensCollected += 1;
        aliensCollectedTxt.SetText("Score\n{0}", (int) aliensCollected);
        CheckAndUpdateAlienTime();
    }

    public void OnBaseBonusPicked(GameObject bonusObj)
    {
        Destroy(bonusObj);
        BaseSizeBonus(true);
    }

    public void OnBlackHolesBonusPicked(GameObject bonusObj)
    {
        Destroy(bonusObj);
        BlackHolesBonus(true);
    }

    // Method thats being called when the ball hits the sea
    public void OnBorderHit(GameObject ball)
    {
        Destroy (ball);
        GameOver();
        //LoadCurrnetScene();
    }

    // Method thats being called when the ball step on a black hole
    public void OnBlackHole(GameObject ball)
    {
        Destroy (ball);
        GameOver();
        //LoadCurrnetScene();
    }

    // PRIVATE METHODS
    // Method that updates time of game
    private void UpdateGameTime()
    {
        if (isActive)
        {
            timeGained += Time.deltaTime;
            gameTimeScoreTxt.SetText("Score: {0}", (int) timeGained);
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
        alienTimerTxt
            .SetText("Alien Vanishes\n in: {0}", (int) timeLeftToCollectAlien);
        if (timeLeftToCollectAlien <= 1)
        {
            timeLeftToCollectAlien = maxTimeToCollectAlien + 1;

            //GameObject alienObject = GameObject.Find("Alien@idle(Clone)");
            GameObject[] alienObjects =
                GameObject.FindGameObjectsWithTag("Alien");
            foreach (GameObject alienObject in alienObjects)
            {
                Transform transform = alienObject.transform;
                CreateBlackHole (transform);
                Destroy (alienObject);
                SpawnAlien();
            }
        }
    }

    // Method that creates new black hole on the uncollected bottle
    private void CreateBlackHole(Transform transform)
    {
        GameObject newObject =
            Instantiate(blackHoleToSpawn.gameObject,
            transform.position,
            baseObject.transform.localRotation);
        newObject.transform.parent = baseObject.transform;
        newObject.transform.localPosition =
            new Vector3(newObject.transform.localPosition.x,
                0.6f,
                newObject.transform.localPosition.z);
    }

    // Method that spawns a new bottle
    private void SpawnBaseBonusObject()
    {
        Debug.Log("SpawnBaseBonusObject");
        float scaleX = (baseScale.x / 2f) - 2;
        float scaleZ = (baseScale.z / 2f) - 2;

        float randomX = Random.Range(-scaleX, scaleX);
        float randomZ = Random.Range(-scaleZ, scaleZ);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);
        GameObject newObject =
            Instantiate(baseBonusObjectToSpawn.gameObject,
            randomPosition,
            baseObject.transform.localRotation);

        newObject.transform.parent = baseObject.transform;
        newObject.transform.localPosition =
            new Vector3(newObject.transform.localPosition.x,
                0.3f,
                newObject.transform.localPosition.z);
    }

    
        private void SpawnBlackHolesBonusObject()
    {
        Debug.Log("SpawnBlackHolesBonusObject");
        float scaleX = (baseScale.x / 2f) - 2;
        float scaleZ = (baseScale.z / 2f) - 2;

        float randomX = Random.Range(-scaleX, scaleX);
        float randomZ = Random.Range(-scaleZ, scaleZ);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);
        GameObject newObject =
            Instantiate(blackHolesBonusObjectToSpawn.gameObject,
            randomPosition,
            baseObject.transform.localRotation);
        Debug.Log("object scale"+newObject.transform.localScale );
        newObject.transform.parent = baseObject.transform;
        newObject.transform.localScale = new Vector3(0.02f,0.07f,0.02f);
        newObject.transform.localPosition =
            new Vector3(newObject.transform.localPosition.x,
                0.8f,
                newObject.transform.localPosition.z);
    }

        private void SpawnAlien()
    {
        float scaleX = (baseScale.x / 2f) - 2;
        float scaleZ = (baseScale.z / 2f) - 2;

        float randomX = Random.Range(-scaleX, scaleX);
        float randomZ = Random.Range(-scaleZ, scaleZ);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);
        GameObject newObject =
            Instantiate(alienToSpawn.gameObject,
            randomPosition,
            baseObject.transform.localRotation);

        newObject.transform.parent = baseObject.transform;
        newObject.transform.localPosition =
            new Vector3(newObject.transform.localPosition.x,
                0.3f,
                newObject.transform.localPosition.z);
        newObject.transform.localRotation =
            Quaternion
                .Euler(new Vector3(newObject.transform.localRotation.x,
                    180,
                    newObject.transform.localRotation.z));
    }

    private void GameOver()
    {
        isActive = false;
        gameOverPanel.SetActive(true);
        GameObject obj = gameOverPanel.transform.GetChild(0).gameObject;
        TextMeshProUGUI yourScoreTxt = obj.GetComponent<TextMeshProUGUI>();
        yourScoreTxt.SetText(aliensCollected + "");

        GameObject objComment = gameOverPanel.transform.GetChild(1).gameObject;
        TextMeshProUGUI commentTxt = objComment.GetComponent<TextMeshProUGUI>();

        if (aliensCollected == 0)
        {
            commentTxt
                .SetText("Zero Points?!\nYou Can Do Better! \n Try Again!!");
        }
        else if (aliensCollected < 10)
        {
            commentTxt.SetText("Try Reach 10 Points \n Try Again!!");
        }
        else if (aliensCollected <= 25)
        {
            commentTxt.SetText("You Are Improving \n Give It Another Try!!");
        }
        else if (aliensCollected <= 35)
        {
            commentTxt.SetText("You Are Becoming A Master \n Let's Go Again!!");
        }
        else
        {
            commentTxt
                .SetText("Wow That Must Be A Record \n Can You Beat Yourself??");
        }
    }

    private void LoadCurrnetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
