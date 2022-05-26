using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutomaticGun : MonoBehaviour
{
    [Header("Ammo")]
    public float ammoLoaded;
    public float ammoReserved;
    public float ammoClipSize;
    public float ammoMaxReserved;
    public float ammoOnPickUp;
    bool reloading = false;
    bool hasShot;

    [Header("Other stats")]
    public float reloadTimeBetweenSingleShots;
    public float totalReloadTime;

    [Header("References")]
    public GameObject smallExplosion;
    public GameObject rotator;
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
        hasShot = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !hasShot)
        {
            if (ammoLoaded > 0)
            {
                hasShot = true;
                StartCoroutine(resetLoad());
                ammoLoaded -= 1;
                updateAmmoText();

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (!hit.transform.CompareTag("Player"))
                    {
                        Vector3 targetPostition = new Vector3(hit.point.x,
                        rotator.transform.position.y,
                        hit.point.z);
                        rotator.transform.LookAt(targetPostition);

                        Instantiate(smallExplosion, hit.point, Quaternion.identity);
                        hit.transform.gameObject.SendMessage("gotHit", 2, SendMessageOptions.DontRequireReceiver);
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

    IEnumerator resetLoad()
    {
        yield return new WaitForSeconds(reloadTimeBetweenSingleShots);
        hasShot = false;
    }

    private void OnDisable()
    {
        hasShot = false;
    }

    public void updateAmmoText()
    {
        ammoText.text = ammoLoaded + "/" + ammoReserved;
    }
}
