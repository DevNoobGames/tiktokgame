using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Car : MonoBehaviour
{
    [Header("Settings")]
    public float health = 100;

    [Header ("Other")]
    public DriftController driftCont;
    public GameObject carCam;
    public Slider healthSlider;
    public GameObject gameOverScreen;
    public TextMeshProUGUI randomCenterText;
    public GameObject[] guns; //has to be gameobj due different gun scripts. Can change but cant be arsed
    public GameObject[] enemySpawnpoints;

    [Header ("Bowling")]
    public GameObject resultCam;
    public int plays = 0;
    public bool wallHit;
    public BowlingManager bowlingMan;

    [Header("Maze")]
    public Transform startPoint;
    public GameObject endingCollider;
    public GameObject startingCube;
    public GameObject[] enemyMazeCars;
    bool inMaze = false;
    public Transform endPos;
    public GameObject orewow;

    private void Start()
    {
        driftCont = GetComponent<DriftController>();
        healthSlider.maxValue = health;
        updateHealth(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float y = transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, y, 0);
            GetComponent<Rigidbody>().velocity = new Vector3(0, -1f, 0);
            transform.position += Vector3.up * 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && !wallHit) //check bool because it repeatitly hits wall
        {
            wallHit = true;
            Debug.Log("Hit wall");
            driftCont.canSteer = false;
            carCam.SetActive(false);
            resultCam.SetActive(true);
            if (plays == 0 && bowlingMan.score < 10) 
            {
                StartCoroutine(resetCar());
                plays += 1;
            }
            else
            {
                StartCoroutine(resetGame());
            }
        }

        if (other.CompareTag("GunPickup"))
        {
            activateGun(other.GetComponent<gunPickUp>().gunInt);
            if (other.GetComponent<gunPickUp>().wpnSpawn)
            {
                other.GetComponent<gunPickUp>().wpnSpawn.StartCoroutine(other.GetComponent<gunPickUp>().wpnSpawn.placeWeapon());
            }
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Explosion"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            updateHealth(10);
        }

        if (other.CompareTag("MazeStarter"))
        {
            StartCoroutine(mazeStart());
        }

        if (other.CompareTag("MazeEnd") && inMaze)
        {
            StartCoroutine(winMaze());
        }
    }

    public void updateHealth(float damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator resetCar()
    {
        yield return new WaitForSeconds(5);
        driftCont.canSteer = true;
        resultCam.SetActive(false);
        carCam.SetActive(true);
        wallHit = false;
        transform.position = new Vector3(0, 0.2f, -18f);
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    IEnumerator resetGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("SampleScene");
        bowlingMan.score = 0;
    }

    IEnumerator mazeStart()
    {
        //set pos
        transform.position = startPoint.position;
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().velocity = new Vector3(0, -1f, 0);
        transform.position += Vector3.up * 2;

        //place enemies
        foreach (GameObject c in enemyMazeCars)
        {
            c.SetActive(true);
            c.GetComponent<EnemyMaze>().enabled = false;
            c.transform.localEulerAngles = new Vector3(0, 0, 0);
            c.transform.position = c.GetComponent<EnemyMaze>().startPos.position;
            //c.GetComponent<EnemyMaze>().activeTarget = 1;
        }

        //set start and end border
        startingCube.GetComponent<MeshRenderer>().enabled = true;
        startingCube.GetComponent<BoxCollider>().isTrigger = false;
        endingCollider.GetComponent<MeshRenderer>().enabled = false;
        endingCollider.GetComponent<BoxCollider>().isTrigger = true;

        //disable drive & start maze bool
        driftCont.enabled = false;
        inMaze = true;

        //disable enemy spawn points and destroy enemies
        foreach (GameObject a in enemySpawnpoints)
        {
            a.GetComponent<EnemySpawner>().StopAllCoroutines();
            a.SetActive(false);
        }
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("EnemyCar");
        foreach (GameObject b in currentEnemies)
        {
            Destroy(b);
        }

        //countdown
        randomCenterText.text = "3";
        yield return new WaitForSeconds(1);
        randomCenterText.text = "2";
        yield return new WaitForSeconds(1);
        randomCenterText.text = "1";
        yield return new WaitForSeconds(1);
        randomCenterText.text = "GO!";

        //start the drive
        driftCont.enabled = true;

        //start enemy drive
        foreach (GameObject c in enemyMazeCars)
        {
            c.GetComponent<EnemyMaze>().enabled = true;
        }

        //disable text
        yield return new WaitForSeconds(1);
        randomCenterText.text = "";
    }

    IEnumerator winMaze()
    {
        //remove enemies
        foreach (GameObject c in enemyMazeCars)
        {
            c.transform.localEulerAngles = new Vector3(0, 0, 0);
            c.transform.position = c.GetComponent<EnemyMaze>().startPos.position;

            c.SetActive(false);
            c.GetComponent<EnemyMaze>().enabled = false;
            c.GetComponent<EnemyMaze>().target = null;
            c.GetComponent<EnemyMaze>().activeTarget = 1;

        }

        randomCenterText.text = "YOU WIN!";
        orewow.SetActive(true);
        yield return new WaitForSeconds(1);
        randomCenterText.text = "";

        inMaze = false;

        //set pos 
        transform.position = endPos.position;
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().velocity = new Vector3(0, -1f, 0);
        transform.position += Vector3.up * 2;

        //set start and end colliders
        startingCube.GetComponent<MeshRenderer>().enabled = false;
        startingCube.GetComponent<BoxCollider>().isTrigger = true;
        endingCollider.GetComponent<MeshRenderer>().enabled = true;
        endingCollider.GetComponent<BoxCollider>().isTrigger = false;

        //set enemy spawn points
        foreach (GameObject a in enemySpawnpoints)
        {
            a.SetActive(true);
            //a.GetComponent<EnemySpawner>().StartCoroutine(a.GetComponent<EnemySpawner>().spawnCounter());
        }
    }

    public IEnumerator loseMaze()
    {
        inMaze = false;
        driftCont.enabled = false;

        //set pos 
        transform.position = endPos.position;
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().velocity = new Vector3(0, -1f, 0);
        transform.position += Vector3.up * 2;

        //remove everyone
        foreach (GameObject c in enemyMazeCars)
        {
            c.transform.localEulerAngles = new Vector3(0, 0, 0);
            c.transform.position = c.GetComponent<EnemyMaze>().startPos.position;

            c.SetActive(false);
            c.GetComponent<EnemyMaze>().enabled = false;
            c.GetComponent<EnemyMaze>().target = null;
            c.GetComponent<EnemyMaze>().activeTarget = 1;
        }

        randomCenterText.text = "YOU LOSE!";
        yield return new WaitForSeconds(1);
        randomCenterText.text = "";

        //drive again
        driftCont.enabled = true;

        //set start and end colliders
        startingCube.GetComponent<MeshRenderer>().enabled = false;
        startingCube.GetComponent<BoxCollider>().isTrigger = true;
        endingCollider.GetComponent<MeshRenderer>().enabled = true;
        endingCollider.GetComponent<BoxCollider>().isTrigger = false;

        //set enemy spawn points
        foreach (GameObject a in enemySpawnpoints)
        {
            a.SetActive(true);
            //a.GetComponent<EnemySpawner>().StartCoroutine(a.GetComponent<EnemySpawner>().spawnCounter());
        }

    }

    public void activateGun(int gunToActivate)
    {
        foreach (GameObject g in guns)
        {
            g.SetActive(false);
        }
        guns[gunToActivate].SetActive(true);
    }
}
