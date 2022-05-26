using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    public GameObject carBody;
    public GameObject carEggBody;

    public GameObject[] wheels;
    public Material yellowWheel;

    public DriftController driftCont;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            carBody.SetActive(false);
            carEggBody.SetActive(true);
            foreach (GameObject w in wheels)
            {
                w.GetComponent<MeshRenderer>().material = yellowWheel;
            }
            driftCont.TopSpeed *= 2;

            Destroy(gameObject);
        }
    }
}
