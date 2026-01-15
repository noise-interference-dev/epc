// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     public float moveSpeed = 15.0f; // Скорость движения персонажа
//     public Joystick joystick;
//     private CharacterController controller;

//     public bool joystickActive = false;

//     private void Start()
//     {
//         controller = GetComponent<CharacterController>();
        
//         //Cursor.lockState = CursorLockMode.Locked;
//         //Cursor.visible = false;
//     }
//     private void Update()
//     {
//         float horizontalInput = 0;
//         float verticalInput = 0;
//         if (joystickActive)
//         {
//             horizontalInput = joystick.Horizontal;
//             verticalInput = joystick.Vertical;
//         }
//         else
//         {
//            horizontalInput = Input.GetAxis("Horizontal"); 
//            verticalInput = Input.GetAxis("Vertical");
//         }
//         // Получаем ввод от игрока
       
        
//         // Вычисляем направление движения
//         Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
//         // Применяем гравитацию
//         moveDirection.y -= 9.81f * Time.deltaTime;
        
//         // Двигаем персонажа
//         controller.Move(moveDirection * moveSpeed * Time.deltaTime);
//     }

//     public void Jump()
//     {
//         transform.position += new Vector3(0, 3, 0);
//     }
// }