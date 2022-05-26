using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowlingManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public void addScore()
    {
        score += 1;
        scoreText.text = score.ToString();
    }
}
