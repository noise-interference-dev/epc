// using System;
// using System.Numerics;
// using UnityEngine;

// [System.Serializable]
// public class MovementSettings {
//     public float mouseSens;
//     public float speedRun;
//     public float speedWalk;
//     // public float speedCrouch;
//     // public float speedCrawl;
//     // public float speedSlide;
//     public float jumpPower;
//     public float jumpCountMax;
//     public float jumpCount;
//     public Vector3 gravity;

//     // public float glideFall;
//     // public float speedGlideMove;
//     // public float glideControl;

//     // public float slideDuration;
//     // public float slideCooldown;
//     // public float slideSpeedDecay;
// }
// public class Controller : MonoBehaviour {
//     [Header("References")]
//     [SerializeField] private CharacterController _characterController;
//     // [SerializeField] private CapsuleCollider _coll;
//     [SerializeField] private Transform _cameraTransform;
//     [SerializeField] private MovementSettings movesettings = new MovementSettings {
//         mouseSens = 0.5f,
//         speedWalk = 3f,
//         speedRun = 6f,
//         // speedCrouch = 1.5f,
//         // speedCrawl = 1f,
//         // speedSlide = 8f,
//         jumpPower = 2f,
//         jumpCount = 2f,
//         jumpCountMax = 2f,
//         gravity = new Vector3(0, -9.81f, 0)
//         // glideFall = -2f,
//         // speedGlideMove = 4f,
//         // glideControl = 2f,
//         // slideDuration = 1f,
//         // slideCooldown = 1.5f,
//         // slideSpeedDecay = 0.8f
//     };
//     [SerializeField] private float _currentSpeed;
//     public bool IsFisgunRotActive;
//     private bool _isSliding = false;
//     [SerializeField] private bool _invertY = false, _invertX = false;
    
//     private Vector2 _moveInput;
//     private Vector2 _mouseInput;
//     [SerializeField] private Joystick joy;
//     [SerializeField] private Vector3 _moveDirection;

//     private const float GROUNDED_VELOCITY_Y = -2f;


//     private void HandleMovement(float deltaTime) {
//         #if UNITY_WIN || UNITY_EDITOR
//             // if (_isSliding) {
//             //     _moveDirection = _slideDirection * _currentSlideSpeed;
//             // }
//             // else if (_isGliding) {
//             //     Vector3 glideControl = (transform.right * _moveInput.x + _cameraTransform.forward * _moveInput.y + _cameraTransform.forward * _movementSettings.speedGlideMove) * _movementSettings.glideControl;
//             //     Vector3 currentHorizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
//             //     _moveDirection = Vector3.ClampMagnitude(currentHorizontalVelocity + glideControl, _movementSettings.speedGlideMove);
//             //     _velocity.y = Mathf.MoveTowards(_velocity.y, _movementSettings.glideFall, Mathf.Abs(_movementSettings.gravity) * deltaTime * GRAVITY_MULTIPLIER_GLIDE);
//             // }
//             // else {
//             Vector3 cameraForward = transform.forward;
//             Vector3 cameraRight = _cameraTransform.right;
//             cameraForward.y = 0;
//             cameraRight.y = 0;
//             cameraForward.Normalize();
//             cameraRight.Normalize();
//             _moveDirection = (cameraRight * _moveInput.x + cameraForward * _moveInput.y) * _currentSpeed;
//             // }
            
//             // if (_isJetpackUsed) {
//             //     _velocity.y = Mathf.MoveTowards(_velocity.y, JETPACK_VELOCITY_TARGET, JETPACK_VELOCITY_SPEED * deltaTime * GRAVITY_MULTIPLIER_GLIDE);
//             // }
//             if (_characterController.isGrounded) {
//                 if (_velocity.y < 0) _velocity.y = GROUNDED_VELOCITY_Y;
//             }
//             else if (!_isGliding) _velocity += _movementSettings.gravity * deltaTime;

//             _characterController.Move((_moveDirection + _vectorUp * _velocity.y) * deltaTime);
//         #endif
//         #if UNITY_ANDROID
//             if (joy == null) return;

//             move = new Vector3(0, move.y, 0);
//             move.x = joy.Horizontal * 0.5f;
//             move.z = joy.Vertical * 0.5f;
//             if (is_noclip)
//             {
//                 move_dir = cam_pos.transform.right * move.x + cam_pos.transform.forward * move.z;
//                 transform.position += move_dir * Time.deltaTime * speed_cur;
//                 return;
//             }
//             else if (is_climb)
//                 move_dir = transform.up * move.z * speed_climb;
//             else if (is_swim)
//                 move_dir = (cam_pos.transform.right * move.x + cam_pos.transform.forward * move.z) * speed_cur;
//             else
//                 move_dir = (transform.right * move.x + transform.forward * move.z) * speed_cur + transform.up * move.y;
//             ch_controller.Move(move_dir * Time.deltaTime);
//         #endif
//     }
// }
