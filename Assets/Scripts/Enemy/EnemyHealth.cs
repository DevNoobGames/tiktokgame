using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;

    public void gotHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Instantiate(Resources.Load("Explosion"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            Debug.Log("Hit");
            gotHit(10);
        }
    }
}
