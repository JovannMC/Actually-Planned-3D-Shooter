using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")] 
    [SerializeField] private bool canUse = false;
    [SerializeField] private MainMenuScript _mainMenuScript;
    
    [Header("Volume Setting")]
    [SerializeField] private TMP_InputField _masterVolumeInputField = null;
    [SerializeField] private TMP_InputField _musicVolumeInputField = null;
    [SerializeField] private TMP_InputField _sfxVolumeInputField = null;
    [SerializeField] private Slider _masterVolumeSlider = null;
    [SerializeField] private Slider _musicVolumeSlider = null;
    [SerializeField] private Slider _sfxVolumeSlider = null;
    
    [Header("Brightness Setting")]
    [SerializeField] private TMP_InputField _brightnessInputField = null;
    [SerializeField] private Slider _brightnessSlider = null;
    
    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown _qualityDropdown = null;
    
    [Header("Display Mode Setting")]
    [SerializeField] private TMP_Dropdown _displayModeDropdown = null;
    
    [Header("Sensitivity Setting")]
    /*[SerializeField] private TMP_Text _sensitivityXValue = null;
    [SerializeField] private Slider _sensitivityXSlider = null;
    [SerializeField] private TMP_Text _sensitivityYValue = null;
    [SerializeField] private Slider _sensitivityYSlider = null; */
    [SerializeField] private TMP_InputField _sensitivityInputField = null;
    [SerializeField] private Slider _sensitivitySlider = null;
    
    [Header("Invert Setting")]
    [SerializeField] private Toggle _invertXToggle = null;
    [SerializeField] private Toggle _invertYToggle = null;
    
    [Header("FOV Setting")]
    [SerializeField] private TMP_InputField _fovInputField = null;
    [SerializeField] private Slider _fovSlider = null;
    
    [Header("VSync Setting")]
    [SerializeField] private TMP_Dropdown _vsyncDropdown = null;
    
    [Header("FPS Setting")]
    [SerializeField] private TMP_InputField _fpsLimitInputField = null;
    [SerializeField] private Slider _fpsLimitSlider = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                _masterVolumeInputField.text = localVolume.ToString("0.0");
                _masterVolumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }

            if (PlayerPrefs.HasKey("musicVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("musicVolume");
                _musicVolumeInputField.text = localVolume.ToString("0.0");
                _musicVolumeSlider.value = localVolume;
                // TODO: Set music volume
            }

            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("sfxVolume");
                _sfxVolumeInputField.text = localVolume.ToString("0.0");
                _sfxVolumeSlider.value = localVolume;
                // TODO: Set sfx volume
            }


            if (PlayerPrefs.HasKey("quality")) 
            {
                int localQuality = PlayerPrefs.GetInt("quality");
                _qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
            
            if (PlayerPrefs.HasKey("displayMode"))
            {
                int localDisplayMode = PlayerPrefs.GetInt("displayMode");
                
                if (localDisplayMode == 0)
                {
                    Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen);
                    _displayModeDropdown.value = 0;
                } else if (localDisplayMode == 1)
                {
                    Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
                    _displayModeDropdown.value = 1;
                } else if (localDisplayMode == 2)
                {
                    Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
                    _displayModeDropdown.value = 2;
                }
            }
            
            if (PlayerPrefs.HasKey("brightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("brightness");
                _brightnessInputField.text = localBrightness.ToString("0.0");
                _brightnessSlider.value = localBrightness;
                RenderSettings.ambientLight = new Color(localBrightness, localBrightness, localBrightness);
            }

            if (PlayerPrefs.HasKey("fov"))
            {   
                float localFov = PlayerPrefs.GetInt("fov");
                _fovInputField.text = localFov.ToString("0");
                _fovSlider.value = localFov;
            }
            
            if (PlayerPrefs.HasKey("vSyncMode"))
            {
                int localVsync = PlayerPrefs.GetInt("vSyncMode");
                _vsyncDropdown.value = localVsync;
                QualitySettings.vSyncCount = localVsync;
            }

            if (PlayerPrefs.HasKey("fpsLimit"))
            {
                int localFpsLimit = PlayerPrefs.GetInt("fpsLimit");
                _fpsLimitInputField.text = localFpsLimit.ToString("0");
                _fpsLimitSlider.value = localFpsLimit;
                Application.targetFrameRate = localFpsLimit;
            }

                /*if (PlayerPrefs.HasKey("sensX"))
                {
                    float localSensX = PlayerPrefs.GetFloat("sensX");
                    _sensitivityXValue.text = localSensX.ToString("0.0");
                    _sensitivityXSlider.value = localSensX;
                }
                
                if (PlayerPrefs.HasKey("sensY"))
                {
                    float localSensY = PlayerPrefs.GetFloat("sensY");
                    _sensitivityYValue.text = localSensY.ToString("0.0");
                    _sensitivityYSlider.value = localSensY;
                }*/
            
            if (PlayerPrefs.HasKey("sensitivity"))
            {
                float localSens = PlayerPrefs.GetFloat("sensitivity");
                _sensitivityInputField.text = localSens.ToString("0.0");
                _sensitivitySlider.value = localSens;
            }
            
            if (PlayerPrefs.HasKey("invertX"))
            {
                if (PlayerPrefs.GetInt("invertX") == 1)
                {
                    _invertXToggle.isOn = true;
                } else
                {
                    _invertXToggle.isOn = false;
                }
            }
            
            if (PlayerPrefs.HasKey("invertY"))
            {
                if (PlayerPrefs.GetInt("invertY") == 1)
                {
                    _invertYToggle.isOn = true;
                } else
                {
                    _invertYToggle.isOn = false;
                }
            }
            
        }
    }
}
