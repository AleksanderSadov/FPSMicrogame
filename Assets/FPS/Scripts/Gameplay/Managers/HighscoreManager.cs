using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField] int currentScore = 0;

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
