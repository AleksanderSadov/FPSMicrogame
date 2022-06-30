using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class Lava : MonoBehaviour
    {
        [SerializeField] private float currentIntensity = 1.0f;
        [SerializeField] private float minIntensity = 1.0f;
        [SerializeField] private float maxIntensity = 16.0f;
        [SerializeField] private float heatSpeed = 4;
        [SerializeField] private bool startHeating = false;
        [SerializeField] private bool startCooling = false;
        [SerializeField] private bool isHeated = false;
        [SerializeField] private bool isCooled = false;

        private Material[] emissiveMaterials;
        private Color defaultColor;

        // Start is called before the first frame update
        void Start()
        {
            emissiveMaterials = GetComponent<Renderer>().materials;
            defaultColor = emissiveMaterials[0].GetColor("_EmissionColor");
        }

        // Update is called once per frame
        void Update()
        {
            HandleLavaHeat();
        }

        public void StartHeating()
        {
            startHeating = true;
        }

        public void StartCooling()
        {
            startCooling = true;
        }

        private void HandleLavaHeat()
        {
            if (startCooling && !isCooled)
            {
                isHeated = false;
                startHeating = false;

                currentIntensity -= Time.deltaTime * heatSpeed;

                if (currentIntensity <= minIntensity)
                {
                    currentIntensity = minIntensity;
                    isCooled = true;
                    startCooling = false;
                }

                SetLavaIntensity(currentIntensity);
            }
            else if (startHeating && !isHeated)
            {
                isCooled = false;
                startCooling = false;

                currentIntensity += Time.deltaTime * heatSpeed;

                if (currentIntensity >= maxIntensity)
                {
                    currentIntensity = maxIntensity;
                    isHeated = true;
                    startHeating = false;
                }

                SetLavaIntensity(currentIntensity);
            }
        }

        private void SetLavaIntensity(float intensity)
        {
            foreach (Material mat in emissiveMaterials)
            {
                mat.SetColor("_EmissionColor", defaultColor * intensity);
            }
        }
    }
}
