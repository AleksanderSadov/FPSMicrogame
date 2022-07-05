using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class HighscoreManager : MonoBehaviour
    {
        [Tooltip("Decrease player score by received damage multiplied with this multiplier")]
        public float playerDamageScoreMultiplier = 10;

        [SerializeField] private float currentScore = 0;

        private void Start()
        {
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        public int GetCurrentScore()
        {
            return (int) Math.Round(currentScore);
        }

        public void IncreaseCurrentScore(float increase)
        {
            currentScore += increase;
        }

        public void DecreaseCurrentScore(float decrease, bool useDamageScoreMultiplier)
        {
            if (useDamageScoreMultiplier)
            {
                currentScore -= decrease * playerDamageScoreMultiplier;
            }
            else
            {
                currentScore -= decrease;
            }
        }

        private void OnPlayerDeath(PlayerDeathEvent evt)
        {
            DataPersistenceManager.Instance.playerScore = GetCurrentScore();
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }
    }
}

