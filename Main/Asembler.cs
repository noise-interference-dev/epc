using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using EPC.player;

[System.Serializable]
public struct player
{
    public string name;
    public int id;
    public control control;
    public string rank;
    public bool can_move;
    public bool is_invicibilty;
    public float health;
}

[System.Serializable]
public struct enemy
{
    public string name;
    public string id;
    public string is_invicibilty;
    public float health;
}

[System.Serializable]
public struct teleport
{
    public string name;
    public GameObject t_one;
    public GameObject t_two;
}

public class Asembler : MonoBehaviour
{
    [Header("Основа Сервер")]
    [SerializeField] private GameObject prefab_player;
    [SerializeField] private List<player> players = new List<player>();
    [SerializeField] private List<GameObject> spawns = new List<GameObject>();
    [SerializeField] private teleport[] teleports;
    [SerializeField] private GameObject players_parent;
    [SerializeField] private GameObject props_par;


    [Header("Основа пропы")]
    public int props = 1;
    public int d_props;

    public int c_lifts;
    [SerializeField] private SaveLoadManager s_l_man;
    private void Start()
    {
        s_l_man = GameObject.FindAnyObjectByType<SaveLoadManager>();
    }

    
    public void prop_delete(GameObject prop)
    {
        if (prop.TryGetComponent<prefab_prop>(out prefab_prop p_prop))
        {
            if (p_prop.p_del)
            {
                props -= 1;
                d_props += 1;
                Destroy(prop);
            }
        }
    }

    public void prop_spawn(GameObject spawned, Vector3 pos, Vector3 rot) {
        // if (spawned.GetComponent<prefab_lift>()) c_lifts += 1;
        // if (spawned.GetComponent<prefab_spawn>()) spawns.Add(spawned);
        if (spawned.GetComponent<BuildPrefab>().Place(pos, rot)) {
            props += 1;
            s_l_man.prop_add(spawned);
            return;
        }
        // spawned.transform.rotation = trot;
        spawned.transform.position = pos;
    }

    public void respawn(int id) {
        int rand = Random.Range(0, spawns.Count);
        var plays = players[id];
        plays.health = 200f;
        plays.can_move = true;
        plays.Fpc.can_move = plays.can_move;
        players[id] = plays;
        for (int i = 0; i < spawns.Count; i++) {
            if (i == rand) {
                players[id].Fpc.gameObject.transform.position = spawns[i].transform.position + new Vector3(Random.Range(-5f, 5f), 2.5f, Random.Range(-5f, 5f));
                return;
            }
        }
        players[id].Fpc.gameObject.transform.position = new Vector3(Random.Range(-5f, 5f), 2.5f, Random.Range(-5f, 5f));
    }

    public void give_health(GameObject obj, int amount) {
        if (obj.TryGetComponent<Fpc>(out Fpc Fpc)) {

        }
        else if (obj.TryGetComponent<ai_fpc>(out ai_fpc ai_fpc)) {

        }
    }
    public void take_health(GameObject obj, int amount) {

    }

    public void add_player(string p_name, Fpc Fpc) {
        var player = Instantiate(prefab_player, new Vector3(0, 2.5f, 0), Quaternion.Euler(Vector3.zero), players_parent.transform);
        var plays = new player();
        plays.name = p_name;
        plays.id = players.Count;
        plays.Fpc = player.GetComponent<Fpc>();
        plays.rank = "player";
        plays.can_move = true;
        plays.is_invicibilty = false;
        plays.health = 200f;
        players.Add(plays);
        // players.Add(plays);
    }
    // public void del_player() {
    // }

    /*public void ShowMenu() {
        // TextYHealthPSP.text = $"HP: {YHealthPSP}";
        // TextEHealthPSP.text = $"HP: {EHealthPSP}";
        TextAPCounterSP.text = $"Props: {props}";
        TextAPDCounterSP.text = $"DProps: {d_props}";
    }*/
}
