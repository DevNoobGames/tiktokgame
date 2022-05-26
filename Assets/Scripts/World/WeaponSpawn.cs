using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public GameObject[] weapons;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(placeWeapon());
    }

    public IEnumerator placeWeapon()
    {
        int RandomSecs = Random.Range(2, 200);
        yield return new WaitForSeconds(RandomSecs);
        int randomInt = Random.Range(0, weapons.Length);
        GameObject activeweapon = weapons[randomInt];
        GameObject placed = Instantiate(activeweapon, transform.position, Quaternion.identity);
        placed.GetComponent<gunPickUp>().wpnSpawn = this;
    }
}
