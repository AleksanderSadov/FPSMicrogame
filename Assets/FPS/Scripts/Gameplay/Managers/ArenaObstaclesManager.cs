using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ArenaObstaclesManager : MonoBehaviour
    {
        [SerializeField] private GameObject floorContainer;

        // Start is called before the first frame update
        void Start()
        {
            EventManager.AddListener<WaveCompleteEvent>(OnWaveComplete);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ReplaceFloorTilesWithLava(List<int> tilesIndexes)
        {
            foreach (int index in tilesIndexes)
            {
                GameObject floorTile = floorContainer.transform.GetChild(index).gameObject;
                GameObject standardFloorMesh = floorTile.transform.GetChild(0).gameObject;
                GameObject lavaMesh = floorTile.transform.GetChild(1).gameObject;

                if (!lavaMesh.activeSelf)
                {
                    lavaMesh.SetActive(true);
                    standardFloorMesh.SetActive(false);
                }
            }
        }

        private List<int> GetRandomFloorTilesIndexes(int minNeighbourRange, int maxNeighbourRange)
        {
            List<int> randomTilesIndexes = new List<int>();

            int floorChildsCount = floorContainer.transform.childCount;
            int randomTileIndex = Random.Range(0, floorChildsCount - 1);
            int randomNeighbourRange = Random.Range(minNeighbourRange, maxNeighbourRange);

            for (int i = -randomNeighbourRange; i <= randomNeighbourRange; i++)
            {
                int neighbourSizeTileIndex = randomTileIndex + i;
                if (neighbourSizeTileIndex > 0 && neighbourSizeTileIndex < floorChildsCount)
                {
                    randomTilesIndexes.Add(neighbourSizeTileIndex);
                }
            }

            return randomTilesIndexes;
        }

        private void OnWaveComplete(WaveCompleteEvent evt)
        {
            ReplaceFloorTilesWithLava(GetRandomFloorTilesIndexes(1, 3));
        }
    }
}
