using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ObjectiveKillWave : Objective
    {
        [Tooltip("Wave count")]
        public int WaveCount = 1;

        [Tooltip("Number of enemies in wave")]
        public int EnemiesInWave = 1;

        [Tooltip("Start sending notification about remaining enemies when this amount of enemies is left")]
        public int NotificationEnemiesRemainingThreshold = 3;

        int m_KillTotal;

        protected override void Start()
        {
            if (string.IsNullOrEmpty(Title))
                Title = "Wave " + WaveCount;
            if (string.IsNullOrEmpty(Description))
                Description = "Remaining enemies in wave: " + GetUpdatedCounterAmount();
            base.Start();

            EventManager.AddListener<EnemyKillEvent>(OnEnemyKilled);
        }

        void OnEnemyKilled(EnemyKillEvent evt)
        {
            if (IsCompleted)
                return;

            m_KillTotal++;

            int targetRemaining = evt.RemainingEnemyCount;

            // update the objective text according to how many enemies remain to kill
            if (targetRemaining == 0)
            {
                CompleteObjective(string.Empty, GetUpdatedCounterAmount(), "Objective complete : " + Title);

                WaveCompleteEvent waveCompleteEvent = Events.WaveCompleteEvent;
                EventManager.Broadcast(waveCompleteEvent);
            }
            else if (targetRemaining == 1)
            {
                string notificationText = NotificationEnemiesRemainingThreshold >= targetRemaining
                    ? "One enemy left"
                    : string.Empty;
                UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
            else
            {
                // create a notification text if needed, if it stays empty, the notification will not be created
                string notificationText = NotificationEnemiesRemainingThreshold >= targetRemaining
                    ? targetRemaining + " enemies to kill left"
                    : string.Empty;

                UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
        }

        string GetUpdatedCounterAmount()
        {
            return m_KillTotal + " / " + EnemiesInWave;
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<EnemyKillEvent>(OnEnemyKilled);
        }
    }
}