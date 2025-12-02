// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using System.Threading.Tasks.Dataflow;
// using System.Numerics;
// using Microsoft.VisualBasic;
// using EPC.player;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int id;
    public control control;
    public string rank;
    public bool isInvincible;
    public float health;
    public float maxHealth = 200f;

    public bool IsAlive => health > 0;
    // public float HealthPercent => health / maxHealth;
}

[System.Serializable]
public class EnemyData
{
    public string name;
    public string id;
    public string isInvicible;
    public float health;
    public float maxHealth = 200f;
}

[System.Serializable]
public class TeleportData
{
    public string name;
    public Transform pointA;
    public Transform pointB;
    // public float cooldown = 3f;
    // [HideInInspector]
    //  public float lastUsedTime;
    // public bool CanTeleport => Time.time >= lastUsedTime + cooldown;
}

public class UnitAsembler : MonoBehaviour
{
    [Header("Основа Сервер")]
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private List<PlayerData> players = new List<PlayerData>();
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<TeleportData> teleports = new List<TeleportData>();
    [SerializeField] private Transform parentPlayers;
    [SerializeField] private Transform parentProps;


    [Header("Основа пропы")]
    [SerializeField] private List<GameObject> spawnedProps = new List<GameObject>();

    [SerializeField] private int maxProps = 100;
    private int _propsCount = 0;
    private int _propsDeletedCount = 0;

    
    [SerializeField] private float respawnHeight = 2.5f;
    [SerializeField] private float spawnRadius = 5f;

    public int PropsCount => _propsCount;
    public int PropsDeletedCount => _propsDeletedCount;
    public int ActivePlayers => players.Count;
    
    // public System.Action<PlayerData> OnPlayerSpawned;
    // public System.Action<PlayerData> OnPlayerRespawned;
    // public System.Action<GameObject> OnPropSpawned;
    // public System.Action<GameObject> OnPropDeleted;

    // public int c_lifts;
    [SerializeField] private SaveLoadManager _saveLoadManager;
    // [SerializeField] private BuildManager _buildManager;
    [SerializeField] private InputManager _inputManager;
    public static UnitAsembler Instance { get; private set; }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init() 
    {
        if (Instance != null) return;

        GameObject go = new GameObject("GameManager");
        Instance = go.AddComponent<UnitAsembler>();
        DontDestroyOnLoad(go);
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        _saveLoadManager = FindAnyObjectByType<SaveLoadManager>();
        // _buildManager = FindAnyObjectByType<BuildManager>();
        _inputManager = FindAnyObjectByType<InputManager>();
    }

    #region Teleport
        public bool TeleportObject(GameObject objectToTeleport, Transform teleportTransform, string teleportName)
        {
            TeleportData teleport = teleports.Find(t => t.name == teleportName);
            if (teleport == null) return false;
            
            Transform toPoint = teleportTransform == teleport.pointA ? teleport.pointB : teleport.pointA;
            objectToTeleport.transform.position = toPoint.position;
            
            return true;
        }
        public void AddTeleport(TeleportData newTeleport)
        {
            teleports.Add(newTeleport);
        }
    #endregion
    #region Props
        public bool propDelete(GameObject _prop)
        {
            if (_prop == null) return false;
            if (_prop.TryGetComponent<Prop>(out Prop propComponent))
            {
                if (!propComponent.delete) return false;
                if (_saveLoadManager.IsPropContains(_prop))
                {
                    _propsCount--;
                    _propsDeletedCount++;
                    Destroy(_prop);
                    return true;
                }
            }
            return false;
        }
        public bool propSpawn(GameObject propPrefab, Vector3 position, Vector3 rotation)
        {
            if (_propsCount >= maxProps || propPrefab == null) return false;

            GameObject spawnedProp = Instantiate(propPrefab, position, Quaternion.Euler(rotation), parentProps);
            if (spawnedProp.TryGetComponent<BuildPrefab>(out BuildPrefab buildPrefab))
            {
                if (!buildPrefab.Place(position, rotation))
                {
                    Destroy(spawnedProp);
                    return false;
                }
                _saveLoadManager.PropAdd(spawnedProp);
                spawnedProps.Add(spawnedProp);
                _propsCount++;
            }
            return true;
        }
        // public void ClearAllProps()
        // {
        //     foreach (GameObject prop in objes)
        //     {
        //         if (prop != null) Destroy(prop);
        //     }
        //     spawnedProps.Clear();
        //     PropsCount = 0;
        // }
    #endregion
    #region Player
        public PlayerData GetPlayer(int playerId)
        {
            return players.Find(p => p.id == playerId);
        }
        public PlayerData AddPlayer(string playerName)
        {
            if (prefabPlayer == null) return null;

            Vector3 spawnPosition = GetSpawnPositionRandom();
            GameObject playerObj = Instantiate(prefabPlayer, spawnPosition, Quaternion.identity, parentPlayers);

            control control = playerObj.GetComponent<control>();
            if (control == null)
            {
                Destroy(playerObj);
                return null;
            }
            string _rank = "Player";
            if (players.Count == 0) _rank = "Host";
            PlayerData newPlayer = new PlayerData
            {
                name = playerName,
                id = players.Count,
                control = control,
                rank = _rank,
                isInvincible = false,
                health = 200f,
                maxHealth = 200f
            };

            players.Add(newPlayer);
            // OnPlayerSpawned?.Invoke(newPlayer);

            return newPlayer;
        }
        public bool RemovePlayer(int playerId)
        {
            PlayerData player = GetPlayer(playerId);
            if (player == null) return false;

            if (player.control != null && player.control.gameObject != null)
                Destroy(player.control.gameObject);

            players.Remove(player);
            return true;
        }
        public void DamagePlayer(int playerId, float damage)
        {
            PlayerData player = GetPlayer(playerId);
            if (player == null || player.isInvincible || !player.IsAlive) return;
            
            player.health = Mathf.Max(0, player.health - damage);
            
            if (!player.IsAlive)
            {
                StartCoroutine(RespawnDelayed(player.id, 5f));
            }
        }
        public void HealPlayer(int playerId, float heal)
        {
            PlayerData player = GetPlayer(playerId);
            if (player == null || !player.IsAlive) return;
            
            player.health = Mathf.Min(player.maxHealth, player.health + heal);
        }
        
        private System.Collections.IEnumerator RespawnDelayed(int playerId, float delay)
        {
            yield return new WaitForSeconds(delay);
            RespawnPlayer(playerId);
        }
    #endregion
    #region Respawn
        private Vector3 GetSpawnPositionRandom()
        {
            Vector3 newPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), respawnHeight, Random.Range(-spawnRadius, spawnRadius));
            if (spawnPoints.Count == 0) return newPosition;

            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector3 randomOffset = newPosition;

            return randomSpawn.position + randomOffset + Vector3.up * respawnHeight;
        }
        public void RespawnPlayer(int playerId)
        {
            PlayerData player = GetPlayer(playerId);
            if (player == null) return;

            player.health = player.maxHealth;
            player.isInvincible = false;

            Vector3 spawnPosition = GetSpawnPositionRandom();
            player.control.transform.position = spawnPosition;

            // player.control.ResetState();

            // OnPlayerRespawned?.Invoke(player);
        }
        public void SpawnPointAdd(Transform spawnPoint)
        {
            if (!spawnPoints.Contains(spawnPoint)) spawnPoints.Add(spawnPoint);
        }

        public void SpawnPointRemove(Transform spawnPoint)
        {
            spawnPoints.Remove(spawnPoint);
        }
    #endregion
    // public void give_health(GameObject obj, int amount) {
    //     if (obj.TryGetComponent<Fpc>(out Fpc Fpc)) {

    //     }
    //     else if (obj.TryGetComponent<ai_fpc>(out ai_fpc ai_fpc)) {

    //     }
    // }
    // public void take_health(GameObject obj, int amount) {

    // }

    // public void del_player() {
    // // }

    // /*public void ShowMenu() {
    //     // TextYHealthPSP.text = $"HP: {YHealthPSP}";
    //     // TextEHealthPSP.text = $"HP: {EHealthPSP}";
    //     TextAPCounterSP.text = $"Props: {propsCount}";
    //     TextAPDCounterSP.text = $"DProps: {propsDeleteCount}";
    // }*/
}