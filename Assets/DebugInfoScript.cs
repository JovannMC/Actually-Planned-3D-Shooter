using System.Collections;
using System.Collections.Generic;
using SUPERCharacter;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DebugInfoScript : MonoBehaviour
{
    GameObject _player;
    
    // Info vars
    string _currentScene;
    //float health;
    bool _isGrounded;
    bool _isSprinting;
    bool _isSliding;
    string _playerPos;
    float _speed;

    // Debug info text vars
    TextMeshProUGUI _platformText;
    TextMeshProUGUI _currentSceneText;
    //TextMeshProUGUI _healthText;
    TextMeshProUGUI _isGroundedText;
    TextMeshProUGUI _isSprintingText;
    TextMeshProUGUI _isSlidingText;
    TextMeshProUGUI _playerPosText;
    TextMeshProUGUI _speedText;

    void Start()
    {
        _player = GameObject.Find("Player");

        _platformText = GameObject.Find("Platform").GetComponent<TextMeshProUGUI>();
        _currentSceneText = GameObject.Find("CurrentScene").GetComponent<TextMeshProUGUI>();
        //_healthText = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();
        _isGroundedText = GameObject.Find("isGrounded").GetComponent<TextMeshProUGUI>();
        _isSprintingText = GameObject.Find("isSprinting").GetComponent<TextMeshProUGUI>();
        _isSlidingText = GameObject.Find("isSliding").GetComponent<TextMeshProUGUI>();
        _playerPosText = GameObject.Find("PlayerPos").GetComponent<TextMeshProUGUI>();
        _speedText = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();
        _isSprintingText = GameObject.Find("isSprinting").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _currentScene = SceneManager.GetActiveScene().name;
        _isGrounded = _player.GetComponent<SUPERCharacterAIO>().currentGroundInfo.isInContactWithGround;
        _isSprinting = _player.GetComponent<SUPERCharacterAIO>().isSprinting;
        _isSliding = _player.GetComponent<SUPERCharacterAIO>().isSliding;
        _playerPos = _player.transform.position.ToString();
        _speed = _player.GetComponent<Rigidbody>().velocity.magnitude;
        
        _platformText.SetText("Platform: " + Application.platform);
        _currentSceneText.SetText("Current Scene: " + _currentScene);
        //healthText.SetText("Health: " + health.ToString());
        _isGroundedText.SetText("isGrounded: " + _isGrounded.ToString());
        _isSprintingText.SetText("isSprinting: " + _isSprinting.ToString());
        _isSlidingText.SetText("isSliding: " + _isSliding.ToString());
        _playerPosText.SetText("PlayerPos: " + _playerPos.ToString());
        _speedText.SetText("Speed: " + _speed);
    }
}
