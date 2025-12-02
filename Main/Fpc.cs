using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fpc : MonoBehaviour {
    // [Header("Состояние")] 
    // public bool is_UI;
    // public bool can_move;

    // public bool is_ground;
    // public bool is_swim;
    // public bool is_noclip;
    // public bool is_jump;
    // public bool isWantJump;
    // public bool is_croach;
    // public bool is_climb;

    // [Header("Скорость")] 
    // public float speed_max;
    // public float speed_cur;
    // public float speed_grond;
    // public float speed_coef;
    // public float speed_climb;

    // [Header("Гравитация")] 
    // public float g_max;
    // public float g_cur;
    // public float g_coef;
    // public float g_ground;
    // public float pos_down;
    // public float jumpPower;

    // [SerializeField] private int JumpCounter;

    // [Header("Настройки Камеры")] 
    // private int cam_state;
    // public GameObject cam_pos;
    // public Camera cam;
    // public float cam_sens;

    // public bool inPanel;

    // private int right_id;
    // private Vector2 lookInput;
    // private float cam_pitch;
    // public RectTransform cameraControlPanel;

    // [Header("Настройки Игрока")]
    // [SerializeField] private Vector3 move;
    // private Vector3 move_dir;
    // [SerializeField] private int p_id;
    // public string p_rank;

    // public GameObject map;
    // private CharacterController ch_controller;
    // public Joystick joy;

    // [Header("Скрипты")] 
    // public Asembler Asembler;

    // private void Awake() {
    //     Asembler = Transform.FindAnyObjectByType<Asembler>();
    // }

    // private void Start() {
    //     map = this.transform.parent.parent.gameObject;
    //     ch_controller = GetComponent<CharacterController>();

    //     right_id = -1;
        
    //     OnClickRespawn();
    // }

    // private void Update() {
    //     if (!can_move) return;
    //     #if UNITY_MOBILE || UNITY_EDITOR
    //         if (IsFreeCam) HandleKeyboardInput();
    //         HandleMovement();
    //         is_ground = ch_controller.isGrounded;
    //         if (!is_noclip) {
    //             if (is_ground || is_climb || is_swim) {
    //                 g_cur = g_ground;
    //                 is_jump = false;
    //                 move.y = g_cur;
    //                 JumpCounter = 2;
    //             }
    //             else {
    //                 move += Vector3.up * g_max * Time.deltaTime;
    //                 move.y = Mathf.Clamp(move.y, pos_down, -pos_down);
    //             }
    //             if (isWantJump && is_ground) {
    //                 OnClickJump();
    //                 isWantJump = false;
    //             }
    //         }
    //     #endif
    //     if (is_UI) {
    //         GetTouchInput();
    //         HandleMouseMove();
    //         if (right_id != -1)
    //             LookAround();
    //     }
    //     #if UNITY_WIN || UNITY_EDITOR
    //         if (Mouse.current.leftButton.wasPressedThisFrame) {
    //             // Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    //             // if (Physics.Raycast(ray, out _currentHit)) {
    //             //     if (_currentHit.collider.CompareTag("point")) {
    //             //         if (_isDraggingEdge) {
                            
    //             //         }
    //             //         _isDraggingPoint = true;
    //             //         _lastMousePosition = Mouse.current.position.ReadValue();
    //             //     }
    //             //     else _isDraggingCamera = true;
    //             // }
    //             // else _isDraggingCamera = true;
    //         }
            
    //         if (_isDraggingCamera && Mouse.current.leftButton.isPressed) {
    //             HandleMouseMove();
    //         }
    //         if (Mouse.current.leftButton.wasReleasedThisFrame ) {
    //             if (_isDraggingPoint) {
    //                 CheckForPointMerge(_currentHit.transform.gameObject);
    //             }
                
    //             _isDraggingPoint = false;
    //             _isDraggingCamera = false;
    //         }
            
    //         HandleMouseZoom();
    //     #endif
    // }

    // private void GetTouchInput() {
    //     if (Touchscreen.current == null || cameraControlPanel == null) return;
    //     for (int i = 0; i < Touchscreen.current.touches.Count; i++) {
    //         UnityEngine.InputSystem.Controls.TouchControl touch = Touchscreen.current.touches[i];
    //         UnityEngine.InputSystem.TouchPhase phase = touch.phase.ReadValue();
    //         Vector2 position = touch.position.ReadValue();
    //         int touchId = touch.touchId.ReadValue();

    //         inPanel = RectTransformUtility.RectangleContainsScreenPoint(cameraControlPanel, position, cam);

    //         switch (phase) {
    //             case UnityEngine.InputSystem.TouchPhase.Began:
    //                 if (inPanel && right_id == -1) {
    //                     right_id = touchId;
    //                 }
    //                 break;
                    
    //             case UnityEngine.InputSystem.TouchPhase.Canceled:
    //             case UnityEngine.InputSystem.TouchPhase.Ended:
    //                 if (touchId == right_id) {
    //                     right_id = -1;
    //                 }
    //                 break;
                    
    //             case UnityEngine.InputSystem.TouchPhase.Moved:
    //                 if (touchId == right_id) {
    //                     lookInput = touch.delta.ReadValue() * cam_sens * Time.deltaTime;
    //                 }
    //                 break;
                    
    //             case UnityEngine.InputSystem.TouchPhase.Stationary:
    //                 if (touchId == right_id) {
    //                     lookInput = Vector2.zero;
    //                 }
    //                 break;
    //         }
    //     }
    // }

    // private void LookAround() {
    //     cam_pitch = Mathf.Clamp(cam_pitch - lookInput.y, -90f, 90f);
    //     cam_pos.transform.localRotation = Quaternion.Euler(cam_pitch, 0, 0);
    //     transform.Rotate(transform.up, lookInput.x);
    //     lookInput = Vector2.zero;
    //     // cam_pitch = 0;
    // }
    
    // private void HandleMouseMove() {
    //         yRotation += mouseDelta.x * mouseSensitivity * Time.deltaTime;
    //         xRotation -= mouseDelta.y * mouseSensitivity * Time.deltaTime;

    //         MainObj.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        
    //         Cam = new Vector3(-mouseDelta.y, mouseDelta.x, 0) * mouseSensitivity;
    //         transform.eulerAngles += Cam * Time.deltaTime;
    // }

    // private void HandleMovement()
    // {
    //     if (joy == null) return;

    //     move = new Vector3(0, move.y, 0);
    //     move.x = joy.Horizontal * 0.5f;
    //     move.z = joy.Vertical * 0.5f;
    //     if (is_noclip)
    //     {
    //         move_dir = cam_pos.transform.right * move.x + cam_pos.transform.forward * move.z;
    //         transform.position += move_dir * Time.deltaTime * speed_cur;
    //         return;
    //     }
    //     else if (is_climb)
    //         move_dir = transform.up * move.z * speed_climb;
    //     else if (is_swim)
    //         move_dir = (cam_pos.transform.right * move.x + cam_pos.transform.forward * move.z) * speed_cur;
    //     else
    //         move_dir = (transform.right * move.x + transform.forward * move.z) * speed_cur + transform.up * move.y;
    //     ch_controller.Move(move_dir * Time.deltaTime);
    // }

    // public void OnClickJump() {
    //     if (is_climb) 
    //         is_climb = false;
    //     if (JumpCounter > 0) {
    //         g_cur = -g_max;
    //         move.y = Mathf.Sqrt(Mathf.Abs(g_max * jumpPower));
    //         is_jump = true;
    //     }
    //     JumpCounter--;
    //     if (JumpCounter < 0) isWantJump = true;
    // }

    // public void onClickCroach() {
    //     float targetHeight = is_croach ? 2f : 1f;
    //     float targetCenterY = is_croach ? 0f : (is_ground ? -0.5f : 0.5f);
    //     float targetCamPosY = is_croach ? 0.5f : (is_ground ? -0.275f : 0.625f);

    //     ch_controller.height = targetHeight;
    //     ch_controller.center = new Vector3(0, targetCenterY, 0);
    //     cam_pos.transform.localPosition = new Vector3(0, targetCamPosY, 0);

    //     is_croach = !is_croach;
    // }

    // public void OnClickRespawn() {
    //     Asembler.respawn(p_id);
    // }

    // private void OnTriggerEnter(Collider coll) {
    //     if (coll.TryGetComponent<prefab_ladder>(out prefab_ladder prefab_ladder))
    //         is_climb = true;
    // }

    // private void OnTriggerExit(Collider coll) {
    //     if (coll.TryGetComponent<prefab_ladder>(out prefab_ladder prefab_ladder))
    //         is_climb = false;
    // }
}
