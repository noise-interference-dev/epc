using UnityEngine;
using UnityEngine.UI;

public class AsemblyGame : MonoBehaviour
{
    public enum PlayerStance { Menu, UI, Inventory, Gravity, Building, Shooting, Welding }
    private PlayerStance _currentStance = PlayerStance.UI;
    [Header("Настройки")]
    public float renderDistance;
    public float gridCurrent;
    private float gridLarge = 1.5f;
    private float gridSmall = 0.1f;
    public Button yourButton;
    public GameObject use_butt;
    public GameObject lift_pack;

    [SerializeField] private float distance;
    public Vector3 pos;
    public Vector3 rot;
    public Quaternion trot;
    [SerializeField] private Transform spherePosition;

    public Transform ObjectToMove;
    public GameObject Prefab { get; private set; }

    [SerializeField] private float maxGrabDist = 15f;
    [SerializeField] private float throwForce = 20f;

    // [Header("Настройки Строительства")] 
    // [Header("Настройки Гравитации")] 
    [Header("Настройки Стрельбы")] 
    public float Damage;
    public float BulletRange;

    [Header("Скрипты")]
    public UnitAsembler unitAsembler { get; private set; }
    public GravityGun GravityGun { get; private set; }
    Rigidbody _rigidbody;

    public void Awake()
    {
        unitAsembler = FindAnyObjectByType<UnitAsembler>();
    }

    public void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * renderDistance, Color.black);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, renderDistance))
        {
            GameObject _prop = hit.transform.gameObject;
            if (_prop.TryGetComponent<Prop>(out Prop propComponent))
            {
                if (propComponent.gravity) unitAsembler.propDelete(_prop);
            }
        }
        switch (_currentStance)
        {
            case PlayerStance.Gravity:
                HandleGravity();
                break;
            case PlayerStance.Building:
                break;
            case PlayerStance.Menu:
                break;
            case PlayerStance.UI:
                break;
            case PlayerStance.Inventory:
                break;
            case PlayerStance.Shooting:
                break;
            case PlayerStance.Welding:
                break;
        }
        if (ObjectToMove != null)
            pos = spherePosition.transform.position;
            trot = Quaternion.Lerp(ObjectToMove.rotation, Quaternion.Euler(rot), 0.6f);
        // var rot = new Vector3(spherePosition.transform.rotation.x, spherePosition.transform.rotation.y, spherePosition.transform.rotation.z) + new Vector3(mainCamera.rotation.x, mainCamera.rotation.y, mainCamera.rotation.z);
        if (_rigidbody)
        {
            if (_rigidbody.position != pos) _rigidbody.position = pos;
            if (_rigidbody.rotation != trot) _rigidbody.rotation = trot;
        }

        // if (ObjectToMove) ObjectToMove.transform.rotation = Quaternion.Euler(ObjectToMove.GetComponent<BuildPrefab>().prot);
        // trot = Quaternion.Lerp(ObjectToMove.transform.rotation, Quaternion.Euler(rot + ObjectToMove.GetComponent<BuildPrefab>().prot), 0.5f);
        // ObjectToMove.transform.rotation = trot;
        if (Prefab)
        {
            if (ObjectToMove != null)
            {
                if (ObjectToMove.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
                {
                    // Vector3 pose = bp.ppos;
                    // pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCurrent) * gridCurrent,
                    //                 Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCurrent) * gridCurrent,
                    //                 Mathf.Round((spherePosition.transform.position.z + pose.z) / gridCurrent) * gridCurrent);
                    // ObjectToMove.transform.position = Vector3.Lerp(ObjectToMove.transform.position, pos, 10f * Time.deltaTime);
                }
                // else
                // {   
                //     ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), parent);  
                // }
            }
        }
    }
    #region Gravity
        public void AddRotation(Vector3 rotate) => rot += rotate;
        public void AddDistance(float _distance) {
            if (distance > 30f) distance = 30f; 
            else if (distance < 3f) distance = 3f;
            distance += _distance;
            spherePosition.transform.localPosition = new Vector3(0, 0, distance);
        }
        public void grabObject() {
            if (_rigidbody)
            {
                _rigidbody.isKinematic = false;
                GravityGunReset();
            }
            else
            {
                RaycastHit hit;
                // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);
                if (Physics.Raycast(transform.position, transform.forward, out hit, maxGrabDist))
                {
                    if (hit.transform.gameObject.GetComponent<Prop>())
                    {
                        // Debug.DrawLine(ray.origin, hit.point);
                        ObjectToMove = hit.transform;
                        if (ObjectToMove.TryGetComponent<Prop>(out Prop _prop))
                        {
                            if (_prop.gravity)
                            {
                                _rigidbody = hit.   rigidbody;
                                _rigidbody.isKinematic = true;
                                spherePosition.localPosition = hit.point;
                                // distance = ObjectToMove.transform.localPosition.z;

                                rot = ObjectToMove.transform.eulerAngles;
                            }
                        }
                    }
                }
            }
        }
        public void freezeGrabbedObject()
        {
            if (_rigidbody == null) return;
            _rigidbody.isKinematic = true;
            GravityGunReset();
        }
        public void throwGrabbedObject()
        {
            if (_rigidbody == null) return;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            GravityGunReset();
        }
        public void GravityGunReset()
        {
            _rigidbody = null;
            ObjectToMove = null;
            rot = new Vector3(0,0,0);
            distance = 3f;
        }
        private void HandleGravity()
        {

        }
    #endregion
    #region Building
        public void BuildGunReset()
        {
            Prefab = null;
            ObjectToMove = null;
            rot = new Vector3(0,0,0);
            distance = 3f;
        }
        public void ToggleGridSize() => gridCurrent = (gridCurrent == gridLarge) ? gridSmall : gridLarge;
        public void propSet(GameObject _prop) {
            Prefab = _prop;
            Destroy(ObjectToMove);
            // ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), parent);  
        }
        public void propSpawn() {   
            if(Prefab != null) {
                unitAsembler.propSpawn(ObjectToMove.gameObject, pos, rot);
                // ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), parent);  
                /*if (asembler.prop_checker()) {
                    if (ObjectToMove.GetComponent<BuildPrefab>().Place(pos, rot)) {
                        ObjectToMove = null;
                        asembler.props += 1;
                        return;
                    }
                    ObjectToMove.transform.rotation = trot;
                    ObjectToMove.transform.position = pos;
                }*/
            }
        }
        public void SetRotation(Vector3 rote) => rot += rote;

        public void SetDistance(float dist) {
            if (renderDistance > 30f) renderDistance = 30f; 
            else if (renderDistance < 3f) renderDistance = 3f;
            renderDistance += dist;
            spherePosition.transform.localPosition = new Vector3(0, 0, renderDistance);
        }

        public void propCancel() {
            if(ObjectToMove != null) Destroy(ObjectToMove);
            Prefab = null;
            renderDistance = 3f;
            rot = new Vector3(0f,0f,0f);
        }

        // public void Update() {
        //     // if (ObjectToMove) ObjectToMove.transform.rotation = Quaternion.Euler(ObjectToMove.GetComponent<BuildPrefab>().prot);
        //     // trot = Quaternion.Lerp(ObjectToMove.transform.rotation, Quaternion.Euler(rot + ObjectToMove.GetComponent<BuildPrefab>().prot), 0.5f);
        //     // ObjectToMove.transform.rotation = trot;
        //     if (Prefab) {
        //         if (ObjectToMove != null)
        //         {
        //             if (ObjectToMove.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
        //             {
        //                 // Vector3 pose = bp.ppos;
        //                 // pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCurrent) * gridCurrent,
        //                 //                 Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCurrent) * gridCurrent,
        //                 //                 Mathf.Round((sphere.transform.position.z + pose.z) / gridCurrent) * gridCurrent);
        //                 // ObjectToMove.transform.position = Vector3.Lerp(ObjectToMove.transform.position, pos, 10f * Time.deltaTime);
        //             }
        //             // else
        //             // {   
        //             //     ObjectToMove = Instantiate(Prefab, pos, Quaternion.Euler(rot + Prefab.GetComponent<BuildPrefab>().prot), parent);  
        //             // }
        //         }
        //     }
        //     /*if(inventory.usedGunAll != 1) {
        //         if(ObjectToMove.gameObject != null) {
        //             Destroy(ObjectToMove.gameObject);
        //         }
        //         Prefab = null;
        //         renderDistance = 3f;
        //         rot = new Vector3(0f,0f,0f);
        //     }
        //     // Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        //     // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

            
        // }
        // public void OnValidate() {*/
        // }
    #endregion
        // Debug.DrawRay(transform.position, transform.forward * renderDistance, Color.black);
    //         GameObject _prop = hit.collider.gameObject;
    //         // Строительство
    //         // Гравитация
    //         // Стрельба
    //         if(_prop.TryGetComponent(out Entity enemy)) {
    //             enemy.Health -= Damage;
    //         }
    //         // Дверь
    //         else if(_prop.GetComponent<prefab_door>()) {
    //             door_obj = p
    //             use_butt.SetActive(true);
    //             Button btn = yourButton.GetComponent<Button>();
    //             btn.onClick.AddListener(door_move);
    //         }
    //         // Окно
    //         else if(_prop.GetComponent<prefab_window>()) {
    //             wind_obj = _prop;
    //             use_butt.SetActive(true);
    //             Button btn = yourButton.GetComponent<Button>();
    //             btn.onClick.AddListener(wind_move);
    //         }
    //         else {
    //             use_butt.SetActive(false);
    //         }
    //         // Лифт
    //         if(_prop.GetComponent<prefab_lift>()) {
    //             lift_obj = hit.collider.gameObject;
    //             lift_pack.SetActive(true);
    //         }
    //         else {
    //             lift_pack.SetActive(false);
    //         }
    //     }
    //     else {
    //         use_butt.SetActive(false);
    //         lift_pack.SetActive(false);
    //     }
    // }
    
    public void prop_delete() 
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, renderDistance)) 
        {
            GameObject _prop = hit.transform.gameObject;
            if(_prop.TryGetComponent<Prop>(out Prop propComponent)) {
                if (propComponent.delete) unitAsembler.propDelete(_prop);
            }
        }
    }
    // public void wind_move() {
    //     if (!wind_obj.GetComponent<prefab_window>().is_active) {
    //         float rote_y = Mathf.Abs(Mathf.Round(transform.parent.localEulerAngles.y - 0.48f));
    //         if (rote_y < 90f || rote_y > 270f) {
    //             wind_obj.GetComponent<prefab_window>().move_z_minus();
    //             wind_obj.GetComponent<prefab_window>().is_active = true;
    //             Debug.Log("sdf");
    //         }
    //         else {
    //             wind_obj.GetComponent<prefab_window>().move_z_plus();
    //             wind_obj.GetComponent<prefab_window>().is_active = true;
    //             Debug.Log("fds");
    //         }
    //     }
    // }
    // public void door_move() {
    //     if (!door_obj.GetComponent<prefab_door>().is_active) {
    //         float rote_y = Mathf.Abs(Mathf.Round(transform.parent.localEulerAngles.y - 0.48f));
    //         if (rote_y < 90f || rote_y > 270f) {
    //             door_obj.GetComponent<prefab_door>().move_z_minus();
    //             door_obj.GetComponent<prefab_door>().is_active = true;
    //             Debug.Log("sdf");
    //         }
    //         else {
    //             door_obj.GetComponent<prefab_door>().move_z_plus();
    //             door_obj.GetComponent<prefab_door>().is_active = true;
    //             Debug.Log("fds");
    //         }
    //     }
    // }

    // public void lift_move_up() {
    //     if (!lift_obj.GetComponent<prefab_lift>().is_active) {
    //         lift_obj.GetComponent<prefab_lift>().move_y_up();
    //         lift_obj.GetComponent<prefab_lift>().is_active = true;
    //     }
    // }

    // public void lift_move_down() {
    //     if (!lift_obj.GetComponent<prefab_lift>().is_active) {
    //         lift_obj.GetComponent<prefab_lift>().move_y_down();
    //         lift_obj.GetComponent<prefab_lift>().is_active = true;
    //     }
    // }
}
