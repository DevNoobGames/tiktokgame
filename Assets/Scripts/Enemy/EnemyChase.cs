using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;
    bool hasHit = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!target)
        {
            target = GameObject.Find("Player");
        }
    }

    void Update()
    {
        if (!hasHit && target)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHit)
        {
            hasHit = true;
            agent.SetDestination(transform.position);
            Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
