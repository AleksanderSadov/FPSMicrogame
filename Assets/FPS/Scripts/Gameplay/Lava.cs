using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class Lava : MonoBehaviour
    {
        public float lavaDamage = 10.0f;
        public bool isHeated = false;

        [SerializeField] private float currentIntensity = 1.0f;
        [SerializeField] private float minIntensity = 1.0f;
        [SerializeField] private float maxIntensity = 16.0f;
        [SerializeField] private float heatSpeed = 4;
        [SerializeField] private bool startHeating = false;
        [SerializeField] private bool startCooling = false;
        [SerializeField] private bool isCooled = false;

        private Material[] emissiveMaterials;
        private Color defaultColor;
        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            emissiveMaterials = GetComponent<Renderer>().materials;
            defaultColor = emissiveMaterials[0].GetColor("_EmissionColor");
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleLavaHeat();
            HandleLavaSound();
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

        private void HandleLavaSound()
        {
            if (startHeating || isHeated)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
        }
    }
}
