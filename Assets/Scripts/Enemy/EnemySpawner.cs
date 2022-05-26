using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyCar;
    public int maxTimer = 190;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(spawnCounter());
    }

    public IEnumerator spawnCounter()
    {
        int RandomSecs = Random.Range(2, maxTimer);
        yield return new WaitForSeconds(RandomSecs);
        Instantiate(enemyCar, transform.position, Quaternion.identity);
        if (maxTimer > 20)
        {
            maxTimer -= 10;
        }
        StartCoroutine(spawnCounter());

    }
}
