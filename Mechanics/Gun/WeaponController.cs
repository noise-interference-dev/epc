using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponController : MonoBehaviour
{
    // public static WeaponController Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private TMP_Text ammoCurrentText;
    [SerializeField] private TMP_Text ammoTypeText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Transform handTransform;
    [SerializeField] public Camera cameraPlayer;

    [Header("Ammo")]
    [SerializeField] private int rifleAmmo;
    [SerializeField] private int smgAmmo;
    [SerializeField] private int pistolAmmo;
    [SerializeField] private int shotgunAmmo;
    [SerializeField] private int rpgAmmo;
    [SerializeField] private int snipeAmmo;

    [Header("Weapon Prefabs")]
    [SerializeField] private List<GameObject> weaponPrefabs; 

    private List<GameObject> spawnedWeapons = new List<GameObject>();
    private List<IWeapon> weaponScripts = new List<IWeapon>();

    private GameObject currentSpawnedWeapon;
    private IWeapon currentWeaponScript;
    private int currentWeaponIndex = 0;

    #region Initialization
        // private void Awake()
        // {
        //     if (Instance == null) { Instance = this; }
        //     else { Destroy(gameObject); }
        // }

        private void Start()
        {
            InitializeWeapons();
            ActivateWeapon(currentWeaponIndex);
        }
        private void InitializeWeapons()
        {
            for (int i = 0; i < weaponPrefabs.Count; i++)
            {
                GameObject weaponInstance = Instantiate(weaponPrefabs[i], handTransform);
                weaponInstance.SetActive(false);
                spawnedWeapons.Add(weaponInstance);

                IWeapon weaponScript = weaponInstance.GetComponent<IWeapon>();
                weaponScripts.Add(weaponScript);
            }
        }

        private void ActivateWeapon(int index)
        {
            if (index < 0 || index >= spawnedWeapons.Count) return;

            foreach (GameObject weapon in spawnedWeapons)
            {
                weapon.SetActive(false);
            }

            currentSpawnedWeapon = spawnedWeapons[index];
            currentWeaponScript = weaponScripts[index];

            currentSpawnedWeapon.SetActive(true);

            if (currentWeaponScript != null)
            {
                weaponImage.sprite = currentWeaponScript.WeaponSprite;
                handTransform.localPosition = currentWeaponScript.DefaultPosition;
                // UpdateAmmoCurrentDisplay();
                // UpdateAmmoTypeDisplay();
            }

            currentWeaponIndex = index;
        }
    #endregion

#if UNITY_STANDALONE_WIN
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon(currentWeaponIndex + 1);
        }
    }
#endif

    #region Actions
        void Shoot() => currentWeaponScript?.Shoot();
        void Reload() => currentWeaponScript?.Reload();
    #endregion

    // #region UI Updates
    //     public void UpdateAmmoCurrentDisplay()
    //     {
    //         if (ammoCurrentText != null && currentWeaponScript != null)
    //         {
    //             if (currentWeaponScript.AmmoType == AmmoType.Melee) ammoCurrentText.gameObject.SetActive(false);
    //             else
    //             {
    //                 ammoCurrentText.gameObject.SetActive(true);
    //                 ammoCurrentText.text = currentWeaponScript.CurrentAmmo.ToString();
    //             }
    //         }
    //     }
    //     public void UpdateAmmoTypeDisplay()
    //     {
    //         if (ammoTypeText != null && currentWeaponScript != null)
    //         {
    //             if (currentWeaponScript.AmmoType == AmmoType.Melee) ammoTypeText.gameObject.SetActive(false);
    //             else
    //             {
    //                 ammoTypeText.gameObject.SetActive(true);
    //                 int ammoCountType = GetAmmoCount(currentWeaponScript.AmmoType);
    //                 ammoTypeText.text = ammoCountType.ToString();
    //             }
    //         }
    //     }
    // #endregion

    #region Ammo Management
        public int GetAmmoCount(AmmoType type)
        {
            return type switch
            {
                AmmoType.Rifle => rifleAmmo,
                AmmoType.SMG => smgAmmo,
                AmmoType.Pistol => pistolAmmo,
                AmmoType.Shotgun => shotgunAmmo,
                AmmoType.RPG => rpgAmmo,
                AmmoType.Snipe => snipeAmmo,
                _ => 0 
            };
        }
        public void DecreaseAmmo(AmmoType type, int amount)
        {
            switch (type)
            {
                case AmmoType.Rifle:
                    rifleAmmo -= amount;
                    break;
                case AmmoType.SMG:
                    smgAmmo -= amount;
                    break;
                case AmmoType.Pistol:
                    pistolAmmo -= amount;
                    break;
                case AmmoType.Shotgun:
                    shotgunAmmo -= amount;
                    break;
                case AmmoType.RPG:
                    rpgAmmo -= amount;
                    break;
                case AmmoType.Snipe:
                    snipeAmmo -= amount;
                    break;
            }

            // UpdateAmmoTypeDisplay();
        }
    #endregion

    #region Weapon Management
        public void SwitchWeapon(int weaponIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Count) weaponIndex = 0;

            ActivateWeapon(weaponIndex);
        }
    #endregion
}