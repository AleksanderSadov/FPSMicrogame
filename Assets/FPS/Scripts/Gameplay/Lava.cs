using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class Lava : MonoBehaviour
    {
        public float lavaDamage = 10.0f;
        public float maxLift = 3.0f;
        public float maxIntensity = 16.0f;
        public bool isHeated = false;
        public bool isCooled = false;
        public bool isLifted = false;
        public bool isLowered = false;

        [SerializeField] private float currentIntensity = 1.0f;
        [SerializeField] private float minIntensity = 1.0f;
        [SerializeField] private float heatSpeed = 4;
        [SerializeField] private bool startHeating = false;
        [SerializeField] private bool startCooling = false;
        [SerializeField] private float currentLift = -3.0f;
        [SerializeField] private float minLift = -3.0f;
        [SerializeField] private float liftSpeed = 2;
        [SerializeField] private bool startLifting = false;
        [SerializeField] private bool startLowering = false;

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
            HandleLavaLift();
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

        public void StartLifting(int maxLift)
        {
            startLifting = true;
            this.maxLift = maxLift;
        }

        public void StartLowering()
        {
            startLowering = true;
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

        private void HandleLavaLift()
        {
            if (startLowering && !isLowered)
            {
                isLifted = false;
                startLifting = false;

                currentLift -= Time.deltaTime * liftSpeed;

                if (currentLift <= minLift)
                {
                    currentLift = minLift;
                    isLowered = true;
                    startLowering = false;
                }

                SetLavaLift(currentLift);
            }
            else if (startLifting && !isLifted)
            {
                isLowered = false;
                startLowering = false;

                currentLift += Time.deltaTime * liftSpeed;

                if (currentLift >= maxLift)
                {
                    currentLift = maxLift;
                    isLifted = true;
                    startLifting = false;
                }

                SetLavaLift(currentLift);
            }
        }

        private void SetLavaIntensity(float intensity)
        {
            foreach (Material mat in emissiveMaterials)
            {
                mat.SetColor("_EmissionColor", defaultColor * intensity);
            }
        }

        private void SetLavaLift(float lift)
        {
            transform.position = new Vector3(transform.position.x, lift, transform.position.z);
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
