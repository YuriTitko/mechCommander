using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   //VARIABLES
    [SerializeField] private float mouseSensitivity;
   //REFERENCES

   private Transform parent;

   private void Start() 
   {
       parent = transform.parent;
       Cursor.lockState = CursorLockMode.Locked; // locking mouse cursor in the middle of the screen
   }

   private void Update() 
   {
       Rotate();
   }

   private void Rotate()
   {
       float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

       parent.Rotate(Vector3.up, mouseX);
   }
}