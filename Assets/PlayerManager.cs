using System;
using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;

    private SUPERCharacterAIO _charController = null;
    
    private Camera _cam = null;
    
    // Player Stats
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public float Speed { get; set; }
    public float JumpForce { get; set; }

    // Player Variables
    public bool isGrounded { get; set; }
    public bool isJumping { get; set; }
    public bool isFalling { get; set; }
    public bool isDead { get; set; }
    public bool isMoving { get; set; }

    void Awake()
    {
        //singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            // Only one playerManager
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            _charController = GameObject.Find("Player").GetComponent<SUPERCharacterAIO>();
            _cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        }
        
        if (_charController != null)
        {
            // Apply gameplay settings
            _charController.Sensitivity = PlayerPrefs.GetFloat("sensitivity");

            if (PlayerPrefs.GetInt("invertX") == 1)
            {
                _charController.mouseInputInversion = MouseInputInversionModes.Y;
            }

            if (PlayerPrefs.GetInt("invertY") == 1)
            {
                _charController.mouseInputInversion = MouseInputInversionModes.X;
            }
            
            if (PlayerPrefs.GetInt("invertX") == 1 && PlayerPrefs.GetInt("invertY") == 1)
            {
                _charController.mouseInputInversion = MouseInputInversionModes.Both;
            }
            
            if (PlayerPrefs.GetInt("invertX") == 0 && PlayerPrefs.GetInt("invertY") == 0)
            {
                _charController.mouseInputInversion = MouseInputInversionModes.None;
            }
        }
        
        
        if (_cam != null)
        {
            _cam.fieldOfView = PlayerPrefs.GetInt("fov");
        }
    }
}
