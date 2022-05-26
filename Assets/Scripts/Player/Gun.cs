using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    [Header("Ammo")]
    public float ammoLoaded;
    public float ammoReserved;
    public float ammoClipSize;
    public float ammoMaxReserved;
    public float ammoOnPickUp;
    bool reloading = false;

    [Header ("References")]
    public GameObject bullet;
    public GameObject rotator;
    public Transform shootPos;
    public TextMeshProUGUI ammoText;

    private void OnEnable()
    {
        if (ammoReserved < ammoMaxReserved)
        {
            ammoReserved += ammoOnPickUp;
        }
        if (ammoReserved > ammoMaxReserved)
        {
            ammoReserved = ammoMaxReserved;
        }

        updateAmmoText();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ammoLoaded > 0)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (!hit.transform.CompareTag("Player"))
                    {
                        ammoLoaded -= 1;
                        updateAmmoText();
                        if (ammoLoaded == 0)
                        {
                            StartCoroutine(reload());
                        }

                        Vector3 targetPostition = new Vector3(hit.point.x,
                        rotator.transform.position.y,
                        hit.point.z);
                        rotator.transform.LookAt(targetPostition);

                        GameObject bulletS = Instantiate(bullet, shootPos.position, Quaternion.identity);
                        bulletS.GetComponent<Bullet>().targetPos = hit.point;

                    }
                }
            }
            else if (ammoLoaded == 0 && ammoReserved > 0 && !reloading)
            {
                reloading = true;
                StartCoroutine(reload());
            }
        }
    }

    public void updateAmmoText()
    {
        ammoText.text = ammoLoaded + "/" + ammoReserved;
    }

    public IEnumerator reload()
    {
        reloading = true;
        ammoText.text = "Reloading";

        yield return new WaitForSeconds(2);

        if (ammoReserved >= ammoClipSize)
        {
            ammoLoaded = ammoClipSize;
            ammoReserved -= ammoClipSize;
        }
        else
        {
            ammoLoaded = ammoReserved;
            ammoReserved = 0;
        }
        reloading = false;
        updateAmmoText();
    }
}

