// using System.Diagnostics;
// using System.Numerics;
// using System.Numerics;
// using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAssembler : MonoBehaviour
{
    public enum PlayerStance { Menu, UI, Gravity, Building, Shooting }
    [SerializeField] private PlayerStance _currentStance = PlayerStance.Gravity;

    [Header("Настройки")]
    public float renderDistance = 15f;
    public float gridCurrent = 1.5f;
    private float gridLarge = 1.5f;
    private float gridSmall = 0.1f;

    [Header("Трансформы")]
    public Transform spherePosition; 
    public Transform PropParent;
    
    [Header("Параметры")]
    public Quaternion rot;
    private Vector3 BuildPos;
    [SerializeField] private float throwForce = 20f, lerpSpeed = 10f;
    [SerializeField] private float maxDistance = 30f, distance = 3f, minDistance = 3f;
    
    public GameObject ObjectToMove; // Fantom
    public GameObject Prefab;
    
    private Rigidbody _rigidbody;
    [SerializeField] private MapAsembler mapAsembler;
    [SerializeField] private SaveLoadManager saveLoadManager;

    void Awake()
    {
        mapAsembler = FindAnyObjectByType<MapAsembler>();
    }

    void Update()
    {
        switch (_currentStance)
        {
            case PlayerStance.Gravity: HandleGravity(); break;
            case PlayerStance.Building: HandleBuilding(); break;
        }
    }
    public void AddPropRotation(Vector2 _rot)
    {
        if (ObjectToMove != null) 
        {
            ObjectToMove.transform.Rotate(transform.up, -_rot.x, Space.World);
            ObjectToMove.transform.Rotate(transform.right, _rot.y, Space.World);
        }
    }
    public void AddDistance(float _distance) {
        distance += _distance;
        if (distance > maxDistance) distance = maxDistance; 
        else if (distance < minDistance) distance = minDistance;
        spherePosition.transform.localPosition = new Vector3(0, 0, distance);
    }
    
    #region Gravity
        public void GrabToggle()
        {
            if (_rigidbody != null) { GravityGunReset(); return; }

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, renderDistance))
            {
                if (hit.collider.TryGetComponent<PropInfo>(out PropInfo p) && p.CanGravity)
                {
                    _rigidbody = hit.rigidbody;
                    if (_rigidbody)
                    {
                        _rigidbody.useGravity = false;
                        ObjectToMove = hit.collider.gameObject;
                        rot = ObjectToMove.transform.rotation;
                        distance = hit.distance;
                    }
                }
            }
        }
        public void freezeGrabbedObject()
        {
            if (_rigidbody == null) return;
            _rigidbody.useGravity = false;
            GravityGunReset();
        }
        public void throwGrabbedObject()
        {
            if (_rigidbody == null) return;
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            GravityGunReset();
        }
        public void GravityGunReset()
        {
            if (_rigidbody) _rigidbody.useGravity = true;
            _rigidbody = null;
            ObjectToMove = null;
        }
        public void HandleGravity()
        {
            if (_rigidbody != null)
            {
                _rigidbody.MovePosition(Vector3.Lerp(_rigidbody.position, spherePosition.position, Time.deltaTime * lerpSpeed));
            }
        }
    #endregion

    #region Building
        public void PropSet(GameObject _prefab)
        {
            PropCancel();
            Prefab = _prefab;
            rot = _prefab.transform.rotation;
            ObjectToMove = Instantiate(Prefab, spherePosition.position, rot, PropParent);
            distance = 3f;
            if (ObjectToMove.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
        public void ToggleGridSize() => gridCurrent = (gridCurrent == gridLarge) ? gridSmall : gridLarge;
        public void PropSpawn()
        {
            if (Prefab == null) return;
            mapAsembler.propSpawn(Prefab, BuildPos, rot.eulerAngles);
        }
        public void PropCancel()
        {
            if (ObjectToMove) Destroy(ObjectToMove);
            Prefab = null;
        }

        public void PropDelete()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, renderDistance))
            {
                mapAsembler.propDelete(hit.collider.gameObject);
                Debug.Log("1spawn");
            }
        }
//         public void BuildGunReset()
//         {
//             Prefab = null;
//             ObjectToMove = null;
//             rot = new Vector3(0,0,0);
//             distance = minDistance;
//         }
//         public void SetRotation(Vector3 rote) => rot += rote;

        private void HandleBuilding()
        {
            if (Prefab == null || ObjectToMove == null) return;

            BuildPos = new Vector3(
                Mathf.Round(spherePosition.position.x / gridCurrent) * gridCurrent,
                Mathf.Round(spherePosition.position.y / gridCurrent) * gridCurrent,
                Mathf.Round(spherePosition.position.z / gridCurrent) * gridCurrent
            );

            ObjectToMove.transform.position = Vector3.Lerp(ObjectToMove.transform.position, BuildPos, Time.deltaTime * 15f);
            // ObjectToMove.transform.rotation = Quaternion.Euler(rot);
        }
        // public void SetDistance(float dist) {
        //     if (renderDistance > maxDistance) renderDistance = maxDistance; 
        //     else if (renderDistance < minDistance) renderDistance = minDistance;
        //     renderDistance += dist;
        //     spherePosition.transform.localPosition = new Vector3(0, 0, renderDistance);
        // }

//         

//         // public void Update() {
//         //     // if (ObjectToMove) ObjectToMove.transform.rotation = Quaternion.Euler(ObjectToMove.GetComponent<BuildPrefab>().prot);
//         //     // trot = Quaternion.Lerp(ObjectToMove.transform.rotation, Quaternion.Euler(rot + ObjectToMove.GetComponent<BuildPrefab>().prot), 0.5f);
//         //     // ObjectToMove.transform.rotation = trot;
//         //     if (Prefab) {
//         //         if (ObjectToMove != null)
//         //         {
//         //             if (ObjectToMove.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
//         //             {
//         //                 // Vector3 pose = bp.ppos;
//         //                 // pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCurrent) * gridCurrent,
//         //                 //                 Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCurrent) * gridCurrent,
//         //                 //                 Mathf.Round((sphere.transform.position.z + pose.z) / gridCurrent) * gridCurrent);
//         //                 // ObjectToMove.transform.position = Vector3.Lerp(ObjectToMove.transform.position, pos, 10f * Time.deltaTime);
//         //             }
//         //             // else
//         //             // {   
//         //             //     ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), PropParent);  
//         //             // }
//         //         }
//         //     }
//         //     /*if(inventory.usedGunAll != 1) {
//         //         if(ObjectToMove.gameObject != null) {
//         //             Destroy(ObjectToMove.gameObject);
//         //         }
//         //         Prefab = null;
//         //         renderDistance = minDistance;
//         //         rot = new Vector3(0f,0f,0f);
//         //     }
//         //     // Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
//         //     // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

            
//         // }
//         // public void OnValidate() {*/
//         // }
    #endregion

}
// using UnityEngine;
// using UnityEngine.UI;

// public class PlayerAsembler : MonoBehaviour
// {
//     public enum PlayerStance { Menu, UI, Inventory, Gravity, Building, Shooting, Welding }
//     private PlayerStance _currentStance = PlayerStance.UI;
//     [Header("Настройки")]
//     public float renderDistance;
//     public float gridCurrent;
//     private float gridLarge = 1.5f;
//     private float gridSmall = 0.1f;
//     public Button yourButton;
//     public GameObject use_butt;
//     public GameObject lift_pack;
//     public Transform PropParent;

//     [SerializeField] private float maxDistance, distance, minDistance;
//     public Vector3 pos;
//     public Vector3 rot;
//     public Quaternion trot;
//     [SerializeField] private Transform spherePosition;
//     [SerializeField] private PlayerController playerController;

//     public GameObject ObjectToMove;
//     public GameObject Prefab { get; private set; }

//     [SerializeField] private float maxGrabDist = 15f, throwForce = 20f, lerpSpeed = 10f, rotationSpeed = 5f;

//     // [Header("Настройки Строительства")] 
//     // [Header("Настройки Гравитации")] 
//     [Header("Настройки Стрельбы")] 
//     public float Damage;
//     public float BulletRange;

//     [Header("Скрипты")]
//     public MapAsembler MapAsembler { get; private set; }
//     // public GravityGun GravityGun { get; private set; }
//     Rigidbody _rigidbody;

//     public void Awake()
//     {
//         // if (inventory == null) inventory = FindAnyObjectByType<InventoryController>();
//         // if (asembler == null) asembler = FindAnyObjectByType<MapAsembler>();
//         // cameraTransform = Camera.main.transform;
//         gridCurrent = gridLarge;
//         MapAsembler = FindAnyObjectByType<MapAsembler>();
//     }

//     public void Update()
//     {
//         Debug.DrawRay(transform.position, transform.forward * renderDistance, Color.black);
//         // RaycastHit hit;
//         // if (Physics.Raycast(transform.position, transform.forward, out hit, renderDistance))
//         // {
//         //     GameObject _prop = hit.transform.gameObject;
//         //     if (_prop.TryGetComponent<Prop>(out Prop propComponent))
//         //     {
//         //         if (propComponent.gravity) unitAsembler.propDelete(_prop);
//         //     }
//         // }
//         switch (_currentStance)
//         {
//             case PlayerStance.Gravity:
//                 HandleGravity();
//                 break;
//             case PlayerStance.Building:
//                 break;
//             case PlayerStance.Menu:
//                 break;
//             case PlayerStance.UI:
//                 break;
//             case PlayerStance.Inventory:
//                 break;
//             case PlayerStance.Shooting:
//                 break;
//             case PlayerStance.Welding:
//                 break;
//         }
//         if (ObjectToMove != null)
//             pos = spherePosition.transform.position;
//             trot = Quaternion.Lerp(ObjectToMove.transform.rotation, Quaternion.Euler(rot), 0.6f);
//         // var rot = new Vector3(spherePosition.transform.rotation.x, spherePosition.transform.rotation.y, spherePosition.transform.rotation.z) + new Vector3(mainCamera.rotation.x, mainCamera.rotation.y, mainCamera.rotation.z);
//         // if (_rigidbody)
//         // {
//         //     if (_rigidbody.position != pos) _rigidbody.position = pos;
//         //     if (_rigidbody.rotation != trot) _rigidbody.rotation = trot;
//         // }

//         // if (ObjectToMove) ObjectToMove.transform.rotation = Quaternion.Euler(ObjectToMove.GetComponent<BuildPrefab>().prot);
//         // trot = Quaternion.Lerp(ObjectToMove.transform.rotation, Quaternion.Euler(rot + ObjectToMove.GetComponent<BuildPrefab>().prot), 0.5f);
//         // ObjectToMove.transform.rotation = trot;
//         if (Prefab)
//         {
//             if (ObjectToMove != null)
//             {
//                 if (ObjectToMove.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
//                 {
//                     // Vector3 pose = bp.ppos;
//                     // pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCurrent) * gridCurrent,
//                     //                 Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCurrent) * gridCurrent,
//                     //                 Mathf.Round((spherePosition.transform.position.z + pose.z) / gridCurrent) * gridCurrent);
//                     // ObjectToMove.transform.position = Vector3.Lerp(ObjectToMove.transform.position, pos, 10f * Time.deltaTime);
//                 }
//                 // else
//                 // {   
//                 //     ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), PropParent);  
//                 // }
//             }
//         }
//     }
//     #region Gravity
//         public void SetRotationMobile()
//         {
//             playerController.IsFisgunRotActive = !playerController.IsFisgunRotActive;
//         }
//         public void AddRotation(Vector2 _rot)
//         {
//             if (ObjectToMove != null) 
//             {
//                 // float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
//                 // float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

//                 // Вращаем объект относительно осей камеры, чтобы это выглядело интуитивно
//                 ObjectToMove.transform.Rotate(Camera.main.transform.up, -rot.x, Space.World);
//                 ObjectToMove.transform.Rotate(Camera.main.transform.right, rot.y, Space.World);
//             }
//             // rot += rotate;
//         }
//         public void grabObject() {
//             if (_rigidbody)
//             {
//                 _rigidbody.isKinematic = false;
//                 GravityGunReset();
//             }
//             else
//             {
//                 RaycastHit hit;
//                 // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);
//                 if (Physics.Raycast(transform.position, transform.forward, out hit, maxGrabDist))
//                 {
//                     if (hit.transform.gameObject.GetComponent<Prop>())
//                     {
//                         // Debug.DrawLine(ray.origin, hit.point);
//                         ObjectToMove = hit.collider.gameObject;
//                         if (ObjectToMove.TryGetComponent<Prop>(out Prop _prop))
//                         {
//                             if (_prop.gravity)
//                             {
//                                 _rigidbody = hit.   rigidbody;
//                                 _rigidbody.isKinematic = true;
//                                 spherePosition.localPosition = hit.point;
//                                 // distance = ObjectToMove.transform.localPosition.z;

//                                 rot = ObjectToMove.transform.eulerAngles;
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//         public void freezeGrabbedObject()
//         {
//             if (_rigidbody == null) return;
//             _rigidbody.isKinematic = true;
//             GravityGunReset();
//         }
//         public void throwGrabbedObject()
//         {
//             if (_rigidbody == null) return;
//             _rigidbody.isKinematic = false;
//             _rigidbody.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
//             GravityGunReset();
//         }
//         public void GravityGunReset()
//         {
//             _rigidbody = null;
//             ObjectToMove = null;
//             rot = new Vector3(0, 0, 0);
//             distance = minDistance;
//         }
//         private void HandleGravity()
//         {
//             // if (ObjectToMove != null)
//             //     pos = vector3.Lerp(spherePosition.transform.position;
//             //     trot = Quaternion.Lerp(ObjectToMove.rotation, Quaternion.Euler(rot), 0.6f);
//             // var rot = new Vector3(spherePosition.transform.rotation.x, spherePosition.transform.rotation.y, spherePosition.transform.rotation.z) + new Vector3(mainCamera.rotation.x, mainCamera.rotation.y, mainCamera.rotation.z);
//             if (_rigidbody)
//             {
//                 _rigidbody.MovePosition(Vector3.Lerp(_rigidbody.position, spherePosition.position, Time.deltaTime * lerpSpeed));
//                 // if (_rigidbody.position != pos) _rigidbody.position = pos;
//                 // if (_rigidbody.rotation != trot) _rigidbody.rotation = trot;
//             }
//         }
//     #endregion

//     #region Building

//         public void BuildGunReset()
//         {
//             Prefab = null;
//             ObjectToMove = null;
//             rot = new Vector3(0,0,0);
//             distance = minDistance;
//         }
//         public void ToggleGridSize() => gridCurrent = (gridCurrent == gridLarge) ? gridSmall : gridLarge;
//         public void propSet(GameObject _prop) {
//             Prefab = _prop;
//             Destroy(ObjectToMove);
//             ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().AddRot), PropParent);  
//         }
//         public void propSpawn() {   
//             if(Prefab != null) {
//                 MapAsembler.propSpawn(ObjectToMove, pos, rot);
//                 // ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), PropParent);  
//                 /*if (asembler.prop_checker()) {
//                     if (ObjectToMove.GetComponent<BuildPrefab>().Place(pos, rot)) {
//                         ObjectToMove = null;
//                         asembler.props += 1;
//                         return;
//                     }
//                     ObjectToMove.transform.rotation = trot;
//                     ObjectToMove.transform.position = pos;
//                 }*/
//             }
//         }
//         public void SetRotation(Vector3 rote) => rot += rote;

//         public void SetDistance(float dist) {
//             if (renderDistance > maxDistance) renderDistance = maxDistance; 
//             else if (renderDistance < minDistance) renderDistance = minDistance;
//             renderDistance += dist;
//             spherePosition.transform.localPosition = new Vector3(0, 0, renderDistance);
//         }
//         public void AddDistance(float _distance) {
//             if (distance > maxDistance) distance = maxDistance; 
//             else if (distance < minDistance) distance = minDistance;
//             distance += _distance;
//             spherePosition.transform.localPosition = new Vector3(0, 0, distance);
//         }

//         public void propCancel() {
//             if(ObjectToMove != null) Destroy(ObjectToMove);
//             Prefab = null;
//             renderDistance = minDistance;
//             rot = new Vector3(0f,0f,0f);
//         }

//         public void PropDelete() 
//         {
//             RaycastHit hit;
//             if(Physics.Raycast(transform.position, transform.forward, out hit, renderDistance)) 
//             {
//                 GameObject _prop = hit.transform.gameObject;
//                 if(_prop.tag == "Prop") 
//                 {
//                     MapAsembler.propDelete(_prop);
//                 }
//             }
//         }

//         // public void Update() {
//         //     // if (ObjectToMove) ObjectToMove.transform.rotation = Quaternion.Euler(ObjectToMove.GetComponent<BuildPrefab>().prot);
//         //     // trot = Quaternion.Lerp(ObjectToMove.transform.rotation, Quaternion.Euler(rot + ObjectToMove.GetComponent<BuildPrefab>().prot), 0.5f);
//         //     // ObjectToMove.transform.rotation = trot;
//         //     if (Prefab) {
//         //         if (ObjectToMove != null)
//         //         {
//         //             if (ObjectToMove.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
//         //             {
//         //                 // Vector3 pose = bp.ppos;
//         //                 // pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCurrent) * gridCurrent,
//         //                 //                 Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCurrent) * gridCurrent,
//         //                 //                 Mathf.Round((sphere.transform.position.z + pose.z) / gridCurrent) * gridCurrent);
//         //                 // ObjectToMove.transform.position = Vector3.Lerp(ObjectToMove.transform.position, pos, 10f * Time.deltaTime);
//         //             }
//         //             // else
//         //             // {   
//         //             //     ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), PropParent);  
//         //             // }
//         //         }
//         //     }
//         //     /*if(inventory.usedGunAll != 1) {
//         //         if(ObjectToMove.gameObject != null) {
//         //             Destroy(ObjectToMove.gameObject);
//         //         }
//         //         Prefab = null;
//         //         renderDistance = minDistance;
//         //         rot = new Vector3(0f,0f,0f);
//         //     }
//         //     // Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
//         //     // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

            
//         // }
//         // public void OnValidate() {*/
//         // }
//     #endregion
//         // Debug.DrawRay(transform.position, transform.forward * renderDistance, Color.black);
//     //         GameObject _prop = hit.collider.gameObject;
//     //         // Строительство
//     //         // Гравитация
//     //         // Стрельба
//     //         if(_prop.TryGetComponent(out Entity enemy)) {
//     //             enemy.Health -= Damage;
//     //         }
//     //         // Дверь
//     //         else if(_prop.GetComponent<prefab_door>()) {
//     //             door_obj = p
//     //             use_butt.SetActive(true);
//     //             Button btn = yourButton.GetComponent<Button>();
//     //             btn.onClick.AddListener(door_move);
//     //         }
//     //         // Окно
//     //         else if(_prop.GetComponent<prefab_window>()) {
//     //             wind_obj = _prop;
//     //             use_butt.SetActive(true);
//     //             Button btn = yourButton.GetComponent<Button>();
//     //             btn.onClick.AddListener(wind_move);
//     //         }
//     //         else {
//     //             use_butt.SetActive(false);
//     //         }
//     //         // Лифт
//     //         if(_prop.GetComponent<prefab_lift>()) {
//     //             lift_obj = hit.collider.gameObject;
//     //             lift_pack.SetActive(true);
//     //         }
//     //         else {
//     //             lift_pack.SetActive(false);
//     //         }
//     //     }
//     //     else {
//     //         use_butt.SetActive(false);
//     //         lift_pack.SetActive(false);
//     //     }
//     // }
    

//     // public void wind_move() {
//     //     if (!wind_obj.GetComponent<prefab_window>().is_active) {
//     //         float rote_y = Mathf.Abs(Mathf.Round(transform.PropParent.localEulerAngles.y - 0.48f));
//     //         if (rote_y < 90f || rote_y > 270f) {
//     //             wind_obj.GetComponent<prefab_window>().move_z_minus();
//     //             wind_obj.GetComponent<prefab_window>().is_active = true;
//     //             Debug.Log("sdf");
//     //         }
//     //         else {
//     //             wind_obj.GetComponent<prefab_window>().move_z_plus();
//     //             wind_obj.GetComponent<prefab_window>().is_active = true;
//     //             Debug.Log("fds");
//     //         }
//     //     }
//     // }
//     // public void door_move() {
//     //     if (!door_obj.GetComponent<prefab_door>().is_active) {
//     //         float rote_y = Mathf.Abs(Mathf.Round(transform.PropParent.localEulerAngles.y - 0.48f));
//     //         if (rote_y < 90f || rote_y > 270f) {
//     //             door_obj.GetComponent<prefab_door>().move_z_minus();
//     //             door_obj.GetComponent<prefab_door>().is_active = true;
//     //             Debug.Log("sdf");
//     //         }
//     //         else {
//     //             door_obj.GetComponent<prefab_door>().move_z_plus();
//     //             door_obj.GetComponent<prefab_door>().is_active = true;
//     //             Debug.Log("fds");
//     //         }
//     //     }
//     // }

//     // public void lift_move_up() {
//     //     if (!lift_obj.GetComponent<prefab_lift>().is_active) {
//     //         lift_obj.GetComponent<prefab_lift>().move_y_up();
//     //         lift_obj.GetComponent<prefab_lift>().is_active = true;
//     //     }
//     // }

//     // public void lift_move_down() {
//     //     if (!lift_obj.GetComponent<prefab_lift>().is_active) {
//     //         lift_obj.GetComponent<prefab_lift>().move_y_down();
//     //         lift_obj.GetComponent<prefab_lift>().is_active = true;
//     //     }
//     // }
// }
