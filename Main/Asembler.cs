using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataA
{
    public string name;
    public int id;
    public control control;
    public string rank;
    public bool isInvincibility;
    public float health;
    public float maxHealth = 200f;
    
    public bool IsAlive => health > 0;
    public float HealthPercent => health / maxHealth;
}

[System.Serializable]
public class EnemyData
{
    public string name;
    public string id;
    public bool isInvincibility;
    public float health;
    public float maxHealth = 100f;
}

[System.Serializable]
public class TeleportData
{
    public string name;
    public Transform pointA;
    public Transform pointB;
    public float cooldown = 3f;
    
    [HideInInspector] public float lastUsedTime;
    
    public bool CanTeleport => Time.time >= lastUsedTime + cooldown;
}

public class Asembler : MonoBehaviour
{
    [Header("Server Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playersParent;
    [SerializeField] private Transform propsParent;
    [SerializeField] private float respawnHeight = 2.5f;
    [SerializeField] private float spawnRadius = 5f;
    
    [Header("Game Data")]
    [SerializeField] private List<PlayerDataA> players = new List<PlayerDataA>();
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<TeleportData> teleports = new List<TeleportData>();
    
    [Header("Props Management")]
    [SerializeField] private int maxProps = 100;
    [SerializeField] private List<GameObject> spawnedProps = new List<GameObject>();
    
    public int PropsCount { get; private set; }
    public int PropsDeletedCount { get; private set; }
    public int ActivePlayers => players.Count;
    
    public System.Action<PlayerDataA> OnPlayerSpawned;
    public System.Action<PlayerDataA> OnPlayerRespawned;
    public System.Action<GameObject> OnPropSpawned;
    public System.Action<GameObject> OnPropDeleted;
    
    private SaveLoadManager saveLoadManager;
    private static Asembler instance;

    public static Asembler Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        
        if (saveLoadManager == null)
            Debug.LogWarning("SaveLoadManager not found!");
    }

    #region Player Management
        
        /// <summary>
        /// Добавляет нового игрока в игру
        /// </summary>
        public PlayerDataA AddPlayer(string playerName)
        {
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned!");
                return null;
            }
            
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject playerObj = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, playersParent);
            
            control control = playerObj.GetComponent<control>();
            if (control == null)
            {
                Debug.LogError("Player prefab doesn't have control component!");
                Destroy(playerObj);
                return null;
            }
            
            PlayerDataA newPlayer = new PlayerDataA
            {
                name = playerName,
                id = players.Count,
                control = control,
                rank = "player",
                isInvincibility = false,
                health = 200f,
                maxHealth = 200f
            };
            
            players.Add(newPlayer);
            OnPlayerSpawned?.Invoke(newPlayer);
            
            Debug.Log($"Player '{playerName}' added with ID: {newPlayer.id}");
            return newPlayer;
        }
        
        /// <summary>
        /// Удаляет игрока по ID
        /// </summary>
        public bool RemovePlayer(int playerId)
        {
            PlayerDataA player = GetPlayer(playerId);
            if (player == null) return false;
            
            if (player.control != null && player.control.gameObject != null)
                Destroy(player.control.gameObject);
            
            players.Remove(player);
            return true;
        }
        
        /// <summary>
        /// Получает данные игрока по ID
        /// </summary>
        public PlayerDataA GetPlayer(int playerId)
        {
            return players.Find(p => p.id == playerId);
        }
        
        /// <summary>
        /// Возрождает игрока на случайной точке спавна
        /// </summary>
        public void RespawnPlayer(int playerId)
        {
            PlayerDataA player = GetPlayer(playerId);
            if (player == null)
            {
                Debug.LogWarning($"Player with ID {playerId} not found!");
                return;
            }
            
            // Сбрасываем состояние игрока
            player.health = player.maxHealth;
            player.isInvincibility = false;
            
            // Позиционируем на точке спавна
            Vector3 spawnPosition = GetRandomSpawnPosition();
            player.control.transform.position = spawnPosition;
            
            // Сбрасываем управление
            // player.control.ResetState();
            
            OnPlayerRespawned?.Invoke(player);
            Debug.Log($"Player '{player.name}' respawned at position: {spawnPosition}");
        }
        
        /// <summary>
        /// Наносит урон игроку
        /// </summary>
        public void DamagePlayer(int playerId, float damage)
        {
            PlayerDataA player = GetPlayer(playerId);
            if (player == null || player.isInvincibility || !player.IsAlive) return;
            
            player.health = Mathf.Max(0, player.health - damage);
            
            if (!player.IsAlive)
            {
                Debug.Log($"Player '{player.name}' died!");
                // Автоматическое возрождение через 3 секунды
                StartCoroutine(DelayedRespawn(player.id, 3f));
            }
        }
        
        /// <summary>
        /// Лечит игрока
        /// </summary>
        public void HealPlayer(int playerId, float healAmount)
        {
            PlayerDataA player = GetPlayer(playerId);
            if (player == null || !player.IsAlive) return;
            
            player.health = Mathf.Min(player.maxHealth, player.health + healAmount);
        }
        
        private System.Collections.IEnumerator DelayedRespawn(int playerId, float delay)
        {
            yield return new WaitForSeconds(delay);
            RespawnPlayer(playerId);
        }
        
    #endregion

    #region Spawn Point Management
    
        /// <summary>
        /// Добавляет точку спавна
        /// </summary>
        public void AddSpawnPoint(Transform spawnPoint)
        {
            if (!spawnPoints.Contains(spawnPoint))
            {
                spawnPoints.Add(spawnPoint);
                Debug.Log($"Spawn point added: {spawnPoint.name}");
            }
        }
        
        /// <summary>
        /// Удаляет точку спавна
        /// </summary>
        public void RemoveSpawnPoint(Transform spawnPoint)
        {
            spawnPoints.Remove(spawnPoint);
        }
        
        /// <summary>
        /// Получает случайную позицию спавна
        /// </summary>
        private Vector3 GetRandomSpawnPosition()
        {
            if (spawnPoints.Count == 0)
            {
                Debug.LogWarning("No spawn points available! Using default position.");
                return new Vector3(Random.Range(-spawnRadius, spawnRadius), respawnHeight, Random.Range(-spawnRadius, spawnRadius));
            }
            
            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0,
                Random.Range(-spawnRadius, spawnRadius)
            );
            
            return randomSpawn.position + randomOffset + Vector3.up * respawnHeight;
        }
        
    #endregion

    #region Props Management
        
        /// <summary>
        /// Спавнит проп в указанной позиции
        /// </summary>
        public bool propSpawn(GameObject propPrefab, Vector3 position, Vector3 rotation)
        {
            if (PropsCount >= maxProps)
            {
                Debug.LogWarning($"Maximum props limit reached: {maxProps}");
                return false;
            }
            
            if (propPrefab == null)
            {
                Debug.LogError("Prop prefab is null!");
                return false;
            }
            
            GameObject spawnedProp = Instantiate(propPrefab, position, Quaternion.Euler(rotation), propsParent);
            
            BuildPrefab buildPrefab = spawnedProp.GetComponent<BuildPrefab>();
            if (buildPrefab != null)
            {
                if (!buildPrefab.Place(position, rotation))
                {
                    Debug.LogWarning("Prop placement failed!");
                    Destroy(spawnedProp);
                    return false;
                }
            }
            
            spawnedProps.Add(spawnedProp);
            PropsCount++;
            
            // Сохраняем в менеджере сохранений
            saveLoadManager?.PropAdd(spawnedProp);
            
            OnPropSpawned?.Invoke(spawnedProp);
            Debug.Log($"Prop spawned: {spawnedProp.name}");
            
            return true;
        }
        
        /// <summary>
        /// Удаляет проп
        /// </summary>
        public bool propDelete(GameObject prop)
        {
            if (prop == null) return false;
            
            Prop propComponent = prop.GetComponent<Prop>();
            if (propComponent != null && !propComponent.delete) 
            {
                Debug.LogWarning($"Prop {prop.name} cannot be deleted!");
                return false;
            }
            
            if (spawnedProps.Contains(prop))
            {
                spawnedProps.Remove(prop);
                PropsCount--;
                PropsDeletedCount++;
                
                // Удаляем из менеджера сохранений
                saveLoadManager?.PropRemove(prop);
                
                Destroy(prop);
                
                OnPropDeleted?.Invoke(prop);
                Debug.Log($"Prop deleted: {prop.name}");
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Очищает все пропы
        /// </summary>
        public void ClearAllProps()
        {
            foreach (GameObject prop in spawnedProps)
            {
                if (prop != null)
                    Destroy(prop);
            }
            
            spawnedProps.Clear();
            PropsCount = 0;
            Debug.Log("All props cleared");
        }
        
    #endregion

    #region Teleport Management
        
        /// <summary>
        /// Телепортирует объект через указанный телепорт
        /// </summary>
        public bool TeleportObject(GameObject objectToTeleport, string teleportName)
        {
            TeleportData teleport = teleports.Find(t => t.name == teleportName);
            if (teleport == null || !teleport.CanTeleport)
            {
                Debug.LogWarning($"Teleport '{teleportName}' not found or on cooldown!");
                return false;
            }
            
            // Определяем направление телепортации
            Transform fromPoint = teleport.pointA;
            Transform toPoint = teleport.pointB;
            
            // Если объект уже near pointB, телепортируем в обратном направлении
            if (Vector3.Distance(objectToTeleport.transform.position, teleport.pointB.position) < 
                Vector3.Distance(objectToTeleport.transform.position, teleport.pointA.position))
            {
                fromPoint = teleport.pointB;
                toPoint = teleport.pointA;
            }
            
            objectToTeleport.transform.position = toPoint.position;
            teleport.lastUsedTime = Time.time;
            
            Debug.Log($"Object teleported through '{teleportName}'");
            return true;
        }
        
        /// <summary>
        /// Добавляет телепорт
        /// </summary>
        public void AddTeleport(TeleportData newTeleport)
        {
            teleports.Add(newTeleport);
        }
        
    #endregion

    #region Game State Management
        
        /// <summary>
        /// Очищает все данные игры
        /// </summary>
        public void ResetGame()
        {
            // Удаляем всех игроков
            foreach (PlayerDataA player in players.ToArray())
            {
                RemovePlayer(player.id);
            }
            
            // Очищаем пропы
            ClearAllProps();
            
            // Сбрасываем счетчики
            PropsDeletedCount = 0;
            
            Debug.Log("Game reset complete");
        }
        
        /// <summary>
        /// Получает статистику игры
        /// </summary>
        public string GetGameStats()
        {
            int alivePlayers = players.FindAll(p => p.IsAlive).Count;
            
            return $"Players: {alivePlayers}/{ActivePlayers} | " +
                $"Props: {PropsCount} | " +
                $"Deleted Props: {PropsDeletedCount}";
        }
        
    #endregion

    #region Debug Methods
        
        [ContextMenu("Debug - Spawn Test Player")]
        private void DebugSpawnTestPlayer()
        {
            AddPlayer($"TestPlayer_{Random.Range(1000, 9999)}");
        }
        
        [ContextMenu("Debug - Print Game Stats")]
        private void DebugPrintStats()
        {
            Debug.Log(GetGameStats());
        }
        
        [ContextMenu("Debug - Respawn All Players")]
        private void DebugRespawnAll()
        {
            foreach (PlayerDataA player in players)
            {
                RespawnPlayer(player.id);
            }
        }
        
    #endregion
}