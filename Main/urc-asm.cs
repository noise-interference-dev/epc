using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using Microsoft.VisualBasic;
// using EPC.player;

[System.Serializable]
public class PlayerData
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
public class enemy
{
    public string name;
    public string id;
    public string is_invicibilty;
    public float health;
}

[System.Serializable]
public class Teleport
{
    public string name;
    public GameObject one;
    public GameObject two;
}

public class Asemblero : MonoBehaviour
{
    [Header("Основа Сервер")]
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private List<PlayerData> players = new List<PlayerData>();
    [SerializeField] private List<GameObject> spawns = new List<GameObject>();
    [SerializeField] private List<Teleport> teleports = new List<Teleport>();
    [SerializeField] private GameObject parentPlayers;
    [SerializeField] private GameObject parentProps;


    [Header("Основа пропы")]
    [SerializeField] private List<GameObject> spawnedProps = new List<GameObject>();

    [SerializeField] private int maxProps = 100;
    private int _propsCount = 0;
    private int _propsDeleteCount = 0;

    
    [SerializeField] private float respawnHeight = 2.5f;
    [SerializeField] private float spawnRadius = 5f;

    public int PropsCount => _propsCount;
    public int PropsDeleteCount => _propsDeleteCount;

    // public int c_lifts;
    [SerializeField] private SaveLoadManager _saveLoadManager;

    private void Awake()
    {
        _saveLoadManager = GameObject.FindAnyObjectByType<SaveLoadManager>();
    }
    #region Props
        public void propDelete(GameObject _prop)
        {
            if (_prop.TryGetComponent<Prop>(out Prop propComponent))
            {
                if (propComponent.delete)
                {
                    propsCount -= 1;
                    propsDeleteCount += 1;
                    Destroy(_prop);
                }
            }
        }
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
        public bool propSpawn(GameObject propPrefab, Vector3 position, Vector3 rotation)
        {
            if (PropsCount >= maxProps || propPrefab == null) return false;

            GameObject spawnedProp = Instantiate(propPrefab, position, Quaternion.Euler(rotation), propsParent);

            if (spawnedProp.TryGetComponent<BuildPrefab>(out buildPrefab buildPrefab))
            {
                if (!buildPrefab.Place(position, rotation))
                {
                    Destroy(spawnedProp);
                    return false;
                }
                _saveLoadManager.PropAdd(spawnedProp);
                spawnedProps.Add(spawnedProp);
                PropsCount++;
            }
            // OnPropSpawned?.Invoke(spawnedProp);
            return true;
        }
    #endregion
    #region Player
        public PlayerData AddPlayer(string playerName)
        {
            if (playerPrefab == null) return null;

            Vector3 spawnPosition = GetSpawnPositionRandom();
            GameObject playerObj = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, playersParent);

            control control = playerObj.GetComponent<control>();
            if (control == null)
            {
                Destroy(playerObj);
                return null;
            }
            string _rank = "Player";
            if (playres.Count == 0) _rank = "Host";
            PlayerData newPlayer = new PlayerData
            {
                name = playerName,
                id = players.Count,
                control = control,
                rank = _rank,
                isInvincibility = false,
                health = 200f,
                maxHealth = 200f
            };

            players.Add(newPlayer);
            // OnPlayerSpawned?.Invoke(newPlayer);

            return newPlayer;
        }
        public void add_player(string p_name, control _control)
        {
            var player = Instantiate(prefabPlayer, new Vector3(0, 2.5f, 0), Quaternion.Euler(Vector3.zero), players_parent.transform);
            var plays = new player();
            plays.name = p_name;
            plays.id = players.Count;
            plays.control = player.GetComponent<control>();
            plays.rank = "player";
            // plays.can_move = true;
            plays.is_invicibilty = false;
            plays.health = 200f;
            players.Add(plays);
            // players.Add(plays);
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
    #endregion
    #region Respawn
    public PlayerData GetPlayer(int playerId)
    {
        return players.Find(p => p.id == playerId);
    }
    private Vector3 GetSpawnPositionRandom()
    {
        if (spawnPoints.Count == 0) return new Vector3(Random.Range(-spawnRadius, spawnRadius), respawnHeight, Random.Range(-spawnRadius, spawnRadius));

        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));

        return randomSpawn.position + randomOffset + Vector3.up * respawnHeight;
    }
    public void RespawnPlayer(int playerId)
    {
        PlayerData player = GetPlayer(playerId);
        if (player == null) return;

        player.health = player.maxHealth;
        player.isInvincibility = false;

        Vector3 spawnPosition = GetSpawnPositionRandom();
        player.control.transform.position = spawnPosition;

        // player.control.ResetState();

        OnPlayerRespawned?.Invoke(player);
    }
    public void respawn(int id)
    {
        int rand = Random.Range(0, spawns.Count);
        var plays = players[id];
        plays.health = 200f;
        // plays.can_move = true;
        // plays.control.can_move = plays.can_move;
        players[id] = plays;
        for (int i = 0; i < spawns.Count; i++)
        {
            if (i == rand)
            {
                players[id].control.gameObject.transform.position = spawns[i].transform.position + new Vector3(Random.Range(-5f, 5f), 2.5f, Random.Range(-5f, 5f));
                return;
            }
        }
        players[id].control.gameObject.transform.position = new Vector3(Random.Range(-5f, 5f), 2.5f, Random.Range(-5f, 5f));
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