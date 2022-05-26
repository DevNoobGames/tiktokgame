using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin : MonoBehaviour
{
    public bool scored;
    public BowlingManager bowlingMan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lane") && !scored && bowlingMan)
        {
            scored = true;
            bowlingMan.addScore();
        }
    }
}
