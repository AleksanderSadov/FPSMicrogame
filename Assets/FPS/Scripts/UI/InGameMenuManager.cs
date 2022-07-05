﻿using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        [Tooltip("Root GameObject of the menu used to toggle its activation")]
        public GameObject MenuRoot;

        [Tooltip("Master volume when menu is open")] [Range(0.001f, 1f)]
        public float VolumeWhenMenuOpen = 0.5f;

        [Tooltip("Slider component for look sensitivity")]
        public Slider LookSensitivitySlider;

        [Tooltip("Toggle auto switching to new weapon on pickup")]
        public Toggle weaponAutoSwitchOnPickupToggle;

        [Tooltip("Toggle component for shadows")]
        public Toggle ShadowsToggle;

        [Tooltip("Toggle component for invincibility")]
        public Toggle InvincibilityToggle;

        [Tooltip("Toggle component for framerate display")]
        public Toggle FramerateToggle;

        [Tooltip("GameObject for the controls")]
        public GameObject ControlImage;

        PlayerInputHandler m_PlayerInputsHandler;
        PlayerWeaponsManager m_PlayerWeaponsManager;
        Health m_PlayerHealth;
        FramerateCounter m_FramerateCounter;

        void Start()
        {
            m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler,
                this);

            m_PlayerWeaponsManager = FindObjectOfType<PlayerWeaponsManager>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, InGameMenuManager>(m_PlayerWeaponsManager,
                this);

            m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

            m_FramerateCounter = FindObjectOfType<FramerateCounter>();
            DebugUtility.HandleErrorIfNullFindObject<FramerateCounter, InGameMenuManager>(m_FramerateCounter, this);

            MenuRoot.SetActive(false);

            LookSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
            LookSensitivitySlider.value = DataPersistenceManager.Instance.currentSettings.lookSensitivity;

            weaponAutoSwitchOnPickupToggle.onValueChanged.AddListener(OnWeaponAutoSwitchOnPickupChanged);
            weaponAutoSwitchOnPickupToggle.isOn = DataPersistenceManager.Instance.currentSettings.weaponAutoSwitchOnPickup;

            ShadowsToggle.onValueChanged.AddListener(OnShadowsChanged);
            ShadowsToggle.isOn = DataPersistenceManager.Instance.currentSettings.enableShadows;

            InvincibilityToggle.onValueChanged.AddListener(OnInvincibilityChanged);
            InvincibilityToggle.isOn = DataPersistenceManager.Instance.currentSettings.isInvincible;

            FramerateToggle.onValueChanged.AddListener(OnFramerateCounterChanged);
            FramerateToggle.isOn = DataPersistenceManager.Instance.currentSettings.showFramerate;
        }

        void Update()
        {
            // Lock cursor when clicking outside of menu
            if (!MenuRoot.activeSelf && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
                || (MenuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
            {
                if (ControlImage.activeSelf)
                {
                    ControlImage.SetActive(false);
                    return;
                }

                SetPauseMenuActivation(!MenuRoot.activeSelf);

            }

            if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    LookSensitivitySlider.Select();
                }
            }
        }

        public void ClosePauseMenu()
        {
            SetPauseMenuActivation(false);
        }

        void SetPauseMenuActivation(bool active)
        {
            MenuRoot.SetActive(active);

            if (MenuRoot.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);

                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                AudioUtility.SetMasterVolume(1);

                SaveSettings();
            }

        }

       private void SaveSettings()
        {
            DataPersistenceManager.SettingsSaveData settingsSaveData = DataPersistenceManager.Instance.currentSettings;

            settingsSaveData.lookSensitivity = LookSensitivitySlider.value;
            settingsSaveData.weaponAutoSwitchOnPickup = weaponAutoSwitchOnPickupToggle.isOn;
            settingsSaveData.enableShadows = ShadowsToggle.isOn;
            settingsSaveData.isInvincible = InvincibilityToggle.isOn;
            settingsSaveData.showFramerate = FramerateToggle.isOn;

            DataPersistenceManager.Instance.SaveSettings(settingsSaveData);
        }

        void OnMouseSensitivityChanged(float newValue)
        {
            m_PlayerInputsHandler.LookSensitivity = newValue;
        }

        void OnWeaponAutoSwitchOnPickupChanged(bool newValue)
        {
            m_PlayerWeaponsManager.WeaponAutoSwitchOnPickup = newValue;
        }

        void OnShadowsChanged(bool newValue)
        {
            UniversalRenderPipelineAsset urpAsset = (UniversalRenderPipelineAsset) GraphicsSettings.renderPipelineAsset;
            
            if (newValue)
            {
                urpAsset.shadowDistance = 50;
            }
            else
            {
                urpAsset.shadowDistance = 0;
            }
        }

        void OnInvincibilityChanged(bool newValue)
        {
            m_PlayerHealth.Invincible = newValue;
        }

        void OnFramerateCounterChanged(bool newValue)
        {
            m_FramerateCounter.UIText.gameObject.SetActive(newValue);
        }

        public void OnShowControlButtonClicked(bool show)
        {
            ControlImage.SetActive(show);
        }
    }
}