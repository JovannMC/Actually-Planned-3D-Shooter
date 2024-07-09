using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    private float sensX;
    private float sensY;
    
    public Transform orientation;

    private float _rotationX;
    private float _rotationY;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Set sensitivity variables
        sensX = PlayerPrefs.GetFloat("sensX", sensX);
        sensY = PlayerPrefs.GetFloat("sensY", sensY);
    }

    void Update()
    {

        if (GameManager.Instance.IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * (sensX * 40);
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * (sensY * 40);
            
            _rotationY += mouseX;
            _rotationX -= mouseY;
            
            print("clamping camera rotation");
            _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
            /*if (PlayerPrefs.GetInt("invertX") == 1)
            {
                transform.rotation = Quaternion.Euler(-_rotationX, _rotationY, 0);
                orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
            } else */if (PlayerPrefs.GetInt("invertY") == 1)
            {
                transform.rotation = Quaternion.Euler(-_rotationX, _rotationY, 0);
                orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
            }/* else if (PlayerPrefs.GetInt("invertY") == 1 && PlayerPrefs.GetInt("invertX") == 1)
            {
                transform.rotation = Quaternion.Euler(-_rotationX, -_rotationY * -1, 0);
                orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
            }*/ else
            {
                transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
                orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
            }

        }
    }
}
