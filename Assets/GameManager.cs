using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    public bool IsPaused { get; set; }
    public bool InMenu { get; set; }
    public Scene CurrentScene { get; set; }
    

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
}
