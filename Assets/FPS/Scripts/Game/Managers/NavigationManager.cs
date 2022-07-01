using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.FPS.Game
{
    public class NavigationManager : MonoBehaviour
    {
        public NavMeshSurface[] surfaces;

        // Start is called before the first frame update
        void Start()
        {
            EventManager.AddListener<ArenaReadyEvent>(OnArenaReady);
        }

        public void BakeNavMeshSurfaces()
        {
            foreach (NavMeshSurface surface in surfaces)
            {
                surface.BuildNavMesh();
            }
        }

        private void OnArenaReady(ArenaReadyEvent evt)
        {
            BakeNavMeshSurfaces();
            NavMeshReadyEvent navMeshReadyEvent = Events.NavMeshReadyEvent;
            EventManager.Broadcast(navMeshReadyEvent);
        }
    }
}
