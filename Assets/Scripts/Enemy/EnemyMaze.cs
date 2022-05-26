using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMaze : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;
    public List<GameObject> target1 = new List<GameObject>();
    public List<GameObject> target1Saved = new List<GameObject>();
    public List<GameObject> target2 = new List<GameObject>();
    public List<GameObject> target2Saved = new List<GameObject>();
    public List<GameObject> target3 = new List<GameObject>();
    public List<GameObject> target3Saved = new List<GameObject>();
    public int activeTarget = 1;
    public GameObject endTarget;
    public Transform startPos;
    public Car car;
    public GameObject orewow;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //nextTarget();
    }

    private void OnEnable()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.position = startPos.position;

        target1.Clear();
        foreach (GameObject a in target1Saved)
        {
            target1.Add(a);
        }
        target2.Clear();
        foreach (GameObject b in target2Saved)
        {
            target2.Add(b);
        }
        target3.Clear();
        foreach (GameObject c in target3Saved)
        {
            target3.Add(c);
        }

        activeTarget = 1;
        nextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            agent.SetDestination(target.transform.position);

            if (Vector3.Distance(transform.position, target.transform.position) < 3)
            {
                nextTarget();
            }
        }
    }

    public void nextTarget()
    {
        if (activeTarget == 1)
        {
            int randVal1 = Random.Range(0, target1.Count);
            target = target1[randVal1];
            target1.RemoveAt(randVal1);
            if (target.CompareTag("LastGoal1"))
            {
                activeTarget = 2;
                nextTarget();
            }
        }

        if (activeTarget == 2)
        {
            int randVal2 = Random.Range(0, target2.Count);
            target = target2[randVal2];
            target2.RemoveAt(randVal2);
            if (target.CompareTag("LastGoal2"))
            {
                activeTarget = 3;
                nextTarget();
            }
        }

        if (activeTarget == 3)
        {
            int randVal3 = Random.Range(0, target3.Count);
            target = target3[randVal3];
            target3.RemoveAt(randVal3);
            if (target.CompareTag("LastGoal3"))
            {
                target = endTarget;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MazeEnd"))
        {
            car.StartCoroutine(car.loseMaze());
            orewow.SetActive(true);
        }
    }
}
