using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ArenaObstaclesManager : MonoBehaviour
    {
        [SerializeField] private GameObject floorContainer;
        [SerializeField] private List<GameObject> obstaclesPrefabs = new List<GameObject>();
        [SerializeField] private List<GameObject> activeObstacles = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            ReplaceFloorTileWithLava(SelectRandomFloorTile());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ReplaceFloorTileWithLava(GameObject floorTile)
        {
            GameObject floorMesh = floorTile.transform.GetChild(0).gameObject;
            GameObject lavaMesh = floorTile.transform.GetChild(1).gameObject;

            if (!lavaMesh.activeSelf)
            {
                lavaMesh.SetActive(true);
                floorMesh.SetActive(false);
            }
        }

        private GameObject SelectRandomFloorTile()
        {
            int randomTileIndex = Random.Range(0, floorContainer.transform.childCount - 1);
            return floorContainer.transform.GetChild(randomTileIndex).gameObject;
        }
    }
}
