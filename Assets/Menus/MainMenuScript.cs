using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MainMenuScript : MonoBehaviour
{
    //private string _path = "";
    //private string _persistentPath = "";
    
    [SerializeField] private GameObject noSavedGameDialog = null;
    
    [Header("Audio Settings")]
    [SerializeField] private Slider _masterVolumeSlider = null;
    [SerializeField] private Slider _musicVolumeSlider = null;
    [SerializeField] private Slider _sfxVolumeSlider = null;
    [SerializeField] private TMP_InputField _masterVolumeInputField;
    [SerializeField] private TMP_InputField _sfxVolumeInputField;
    [SerializeField] private TMP_InputField _musicVolumeInputField;
    [SerializeField] private float defaultMasterVolume = 1.0f;
    [SerializeField] private float defaultMusicVolume = 1.0f;
    [SerializeField] private float defaultSfxVolume = 1.0f;

    [Header("Gameplay Settings")]
    /*[SerializeField] private TMP_Text _SensXValue = null;
    [SerializeField] private Slider _SensXSlider = null;
    [SerializeField] private TMP_Text _SensYValue = null;
    [SerializeField] private Slider _SensYSlider = null;
    [SerializeField] private float defaultSensX = 5;
    [SerializeField] private float defaultSensY = 5;*/
    [SerializeField] private Slider _sensSlider = null;
    [SerializeField] private float defaultSens = 6;
    [SerializeField] private TMP_InputField _sensInputField;
    [SerializeField] private Slider _fovSlider = null;
    [SerializeField] private int defaultFov = 60;
    [SerializeField] private TMP_InputField _fovInputField;
    
    [Header("Graphics Settings")]
    [SerializeField] private Slider _brightnessSlider = null;
    [SerializeField] private float defaultBrightness = 1;
    [SerializeField] private TMP_InputField _brightnessInputField;
    [SerializeField] private Slider _fpsLimitSlider;
    [SerializeField] private TMP_InputField _fpsLimitInputField;

    [Space(10)] 
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown displayDropdown;
    [SerializeField] private TMP_Dropdown vSyncDropdown;

    private int _qualityLevel;
    private int _fullScreenMode;
    private float _brightnessLevel;
    private int _vSyncMode;
    private int _fpsLimit;
    private float _currentDisplayRefreshRate;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertXToggle = null;
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Confirmation")]
    public GameObject confirmationPrompt = null;
    
    [Header("Levels To Load")]
    private string _levelToLoad;
    
    [Header("Resolution Dropdown")]
    public TMP_Dropdown _resolutionDropdown;
    private Resolution[] _resolutions;

    // TODO remake the save system for JSON in future
    /*private void SetPaths() {
        _path = Application.dataPath + Path.AltDirectorySeparatorChar + "Settings.json";
        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Settings.json";
    }*/
    
    // in method Start(), set "inMenu" in GameManager to true
    private void Start() 
    {
        GameManager.Instance.IsPaused = false;
        GameManager.Instance.InMenu = true;
        
        //GameObject.DontDestroyOnLoad(this.gameObject);
        
        _currentDisplayRefreshRate = Screen.currentResolution.refreshRate;

        Resolutions();
        
        Settings();
    }

    private void Settings()
    {
        if (!PlayerPrefs.HasKey("hasLaunchedBefore"))
        {
            PlayerPrefs.SetInt("hasLaunchedBefore", 1);
            PlayerPrefs.SetFloat("masterVolume", defaultMasterVolume);
            _masterVolumeInputField.text = defaultMasterVolume.ToString();
            PlayerPrefs.SetFloat("musicVolume", defaultMusicVolume);
            _musicVolumeInputField.text = defaultMusicVolume.ToString();
            PlayerPrefs.SetFloat("sfxVolume", defaultSfxVolume);
            _sfxVolumeInputField.text = defaultSfxVolume.ToString();
            PlayerPrefs.SetFloat("sensitivity", defaultSens);
            _sensInputField.text = defaultSens.ToString();
            PlayerPrefs.SetInt("fov", defaultFov);
            _fovInputField.text = defaultFov.ToString();
            //PlayerPrefs.SetFloat("sensX", defaultSensX);
            //PlayerPrefs.SetFloat("sensY", defaultSensY);
            PlayerPrefs.SetFloat("brightness", defaultBrightness);
            _brightnessInputField.text = defaultBrightness.ToString();
            PlayerPrefs.SetInt("quality", 2);
            QualitySettings.SetQualityLevel(2);
            qualityDropdown.value = 2;
            PlayerPrefs.SetInt("vSyncMode", 0);
            PlayerPrefs.SetInt("invertX", 0);
            PlayerPrefs.SetInt("invertY", 0);
            PlayerPrefs.SetFloat("fpsLimit", _currentDisplayRefreshRate);
            _fpsLimitSlider.value = _currentDisplayRefreshRate;
            _fpsLimitInputField.text = ((int)_currentDisplayRefreshRate).ToString();

            if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
            {
                PlayerPrefs.SetInt("displayMode", 0);
                displayDropdown.value = 0;
            } else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                PlayerPrefs.SetInt("displayMode", 1);
                displayDropdown.value = 1;
            }
            else
            {
                PlayerPrefs.SetInt("displayMode", 2);
                displayDropdown.value = 2;
            }
        }
    }

    private void Resolutions()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    // Dialogs
    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            _levelToLoad = PlayerPrefs.GetString("SavedLevel");
            GameManager.Instance.InMenu = true;
            SceneManager.LoadScene(_levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }
    
    public void TutorialDialogYes()
    {
        GameManager.Instance.InMenu = true;
        SceneManager.LoadScene("Tutorial");
    }
    
    public void TutorialDialogNo()
    {
        GameManager.Instance.InMenu = true;
        SceneManager.LoadScene("Level1");
    }
    
    public void FreeplayDialogYes()
    {
        GameManager.Instance.InMenu = true;
        SceneManager.LoadScene("Freeplay");
    }

    // Volume Settings
    public void SetMasterVolume(float Volume)
    {
        AudioListener.volume = Volume;
        _masterVolumeInputField.text = Volume.ToString("0.0");
    }
    
    public void SetMasterVolumeInput(string volume)
    {
        float localVolume = float.Parse(volume);
        _masterVolumeInputField.text = localVolume.ToString("0.0");
        _masterVolumeSlider.value = localVolume;
    }
    
    public void SetMusicVolume(float Volume)
    {
        //AudioListener.volume = masterVolume;
        _musicVolumeInputField.text = Volume.ToString("0.0");
    }
    
    public void SetMusicVolumeInput(string volume)
    {
        float localVolume = float.Parse(volume);
        _musicVolumeInputField.text = localVolume.ToString("0.0");
        _musicVolumeSlider.value = localVolume;
    }

    public void SetSfxVolume(float Volume)
    {
        //AudioListener.volume = masterVolume;
        _sfxVolumeInputField.text = Volume.ToString("0.0");
    }
    
    public void SetSfxVolumeInput(string volume)
    {
        float localVolume = float.Parse(volume);
        _sfxVolumeInputField.text = localVolume.ToString("0.0");
        _sfxVolumeSlider.value = localVolume;
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        PlayerPrefs.SetFloat("musicVolume", float.Parse(_musicVolumeInputField.text));
        PlayerPrefs.SetFloat("sfxVolume", float.Parse(_sfxVolumeInputField.text));
        
        // Use after testing
        /*PlayerPrefs.SetString("masterVolume", _masterVolumeValue.text);
        PlayerPrefs.SetString("musicVolume", _musicVolumeValue.text);
        PlayerPrefs.SetString("sfxVolume", _sfxVolumeValue.text);*/
        
        StartCoroutine(ConfirmationBox());
    }
    
    // Gameplay Settings
    /*public void SetSensX(float SensX)
    {
        //_SensXValue.text = SensX.ToString("0.0");
    }
    
    public void SetSensY(float SensY)
    {
        _SensYValue.text = SensY.ToString("0.0");
    }*/

    public void SetSens(float Sens)
    {
        _sensInputField.text = Sens.ToString("0.0");
    }
    
    public void SetSensInput(string sens)
    {
        float localSens = float.Parse(sens);
        _sensInputField.text = localSens.ToString("0.0");
        _sensSlider.value = localSens;
    }

    public void SetFov(float Fov) 
    {
        _fovInputField.text = Fov.ToString("0");
    }
    
    public void SetFovInput(string fov)
    { 
        float localFov = float.Parse(fov);
        _fovInputField.text = localFov.ToString("0");
        _fovSlider.value = localFov;
    }
    
    public void GameplayApply()
    {
        if(invertXToggle.isOn)
        {
            // Invert X
            PlayerPrefs.SetInt("invertX", 1);
        }
        else
        {
            // Don't invert X
            PlayerPrefs.SetInt("invertX", 0);
        }
        
        if (invertYToggle.isOn)
        {
            // Invert Y
            PlayerPrefs.SetInt("invertY", 1);
        }
        else
        {
            // Don't invert Y
            PlayerPrefs.SetInt("invertY", 0);
        }
        
        //PlayerPrefs.SetFloat("sensX", _SensXSlider.value);
        //PlayerPrefs.SetFloat("sensY", _SensYSlider.value);
        PlayerPrefs.SetFloat("sensitivity", _sensSlider.value);
        PlayerPrefs.SetInt("fov", (int)_fovSlider.value);
        

        StartCoroutine(ConfirmationBox());
    }

    // Graphics Settings
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        _brightnessInputField.text = brightness.ToString("0.0");
    }
    
    public void SetBrightnessInput(string brightness)
    {
        float localBrightness = float.Parse(brightness);
        _brightnessInputField.text = localBrightness.ToString("0.0");
        _brightnessSlider.value = localBrightness;
    }

    public void SetDisplayMode(int mode)
    {
        if (mode == 0)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen);
            _fullScreenMode = 0;
            print("Fullscreen");
        } else if (mode == 1)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
            _fullScreenMode = 1;
            print("Fullscreen Window");
        } else if (mode == 2)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
            _fullScreenMode = 2;
            print("Windowed");
        } 
    }

    public void SetVsyncMode(int mode)
    {
        if (mode == 0) // off
        {
            QualitySettings.vSyncCount = 0;
            _vSyncMode = 0;
        } else if (mode == 1) // single buffer
        {
            QualitySettings.vSyncCount = 1;
            _vSyncMode = 1;
        } else if (mode == 2) // double buffer
        {
            QualitySettings.vSyncCount = 2;
            _vSyncMode = 2;
        } else if (mode == 3) // triple buffer
        {
            QualitySettings.vSyncCount = 3;
            _vSyncMode = 3;
        }
    }

    public void SetFPSLimitInput(string fps)
    {
        int localFPS = int.Parse(fps);
        _fpsLimitInputField.text = localFPS.ToString("0");
        _fpsLimitSlider.value = localFPS;
        _fpsLimit = int.Parse(fps.ToString());
    }

    public void SetFPSLimit(float fps)
    {
        _fpsLimitInputField.text = fps.ToString("0");
        _fpsLimitSlider.value = fps;
        _fpsLimit = int.Parse(fps.ToString());
    }
    

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
        print("Quality: " + qualityIndex);
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("brightness", _brightnessLevel);
        RenderSettings.ambientLight = new Color(_brightnessLevel, _brightnessLevel, _brightnessLevel);
        
        PlayerPrefs.SetInt("quality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        
        PlayerPrefs.SetInt("displayMode", _fullScreenMode);
        
        PlayerPrefs.SetInt("vSyncMode", _vSyncMode);
        
        PlayerPrefs.SetInt("fpsLimit", _fpsLimit);
        Application.targetFrameRate = _fpsLimit;

        StartCoroutine(ConfirmationBox());
    }
    
    /* Use after implementing audio system
   // set master volume
   public void SetMasterVolume(float volume)
   {
       SetMasterVolume(_masterVolumeSlider.value);
       _masterVolumeValue.text = _masterVolumeSlider.value.ToString("0.00");
   }
   
   // set music volume
   public void SetMusicVolume()
   {
       AudioManager.Instance.SetMusicVolume(_musicVolumeSlider.value);
       _musicVolumeValue.text = _musicVolumeSlider.value.ToString("0.00");
   }
   
   // set sfx volume
   public void SetSfxVolume()
   {
       AudioManager.Instance.SetSfxVolume(_sfxVolumeSlider.value);
       _sfxVolumeValue.text = _sfxVolumeSlider.value.ToString("0.00");
   }*/

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    // General
    
    // Reset specified settings
    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio") 
        {
            _masterVolumeSlider.value = defaultMasterVolume;
            _musicVolumeSlider.value = defaultMusicVolume;
            _sfxVolumeSlider.value = defaultSfxVolume;
            SetMasterVolume(defaultMasterVolume);
            SetMusicVolume(defaultMusicVolume);
            SetSfxVolume(defaultSfxVolume);
            
            VolumeApply();
            //SetMusicVolume(defaultMusicVolume);
            //SetSfxVolume(defaultSfxVolume);
        } else if (MenuType == "Gameplay")
        {
            /*_SensXSlider.value = defaultSensX;
            _SensYSlider.value = defaultSensY;
            SetSensX(defaultSensX);
            SetSensY(defaultSensY);*/
            
            _sensSlider.value = defaultSens;
            SetSens(defaultSens);
            
            invertXToggle.isOn = false;
            invertYToggle.isOn = false;
            
            _fovSlider.value = defaultFov;
            SetFov(defaultFov);
            

            GameplayApply();
        } else if (MenuType == "Graphics")
        {
            _brightnessSlider.value = defaultBrightness;
            _brightnessInputField.text = defaultBrightness.ToString("0.0");

            _qualityLevel = 3; // low quality
            qualityDropdown.value = 3; // low quality
            QualitySettings.SetQualityLevel(3); // low quality

            Screen.fullScreen = false;
            
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            _resolutionDropdown.value = _resolutions.Length;

            _vSyncMode = 0; // vsync off
            vSyncDropdown.value = _vSyncMode; // vsync off
            QualitySettings.vSyncCount = 0; // vsync off
            
            _fpsLimitSlider.value = _currentDisplayRefreshRate;
            _fpsLimitInputField.text = ((int)_currentDisplayRefreshRate).ToString();
            
            GraphicsApply();
        } else if (MenuType == "PlayerPrefs")
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
    }

    // about button
    public void OnLinkClick(String link)
    {
        if (link == "link")
        {
            Application.OpenURL("https://baka.omg.lol");
        } else if (link == "email")
        {
            Application.OpenURL("mailto:contact@baka.omg.lol");
        }
    }

    // exit program
    public void ExitButton()
    {
        Application.Quit();
    }

}
