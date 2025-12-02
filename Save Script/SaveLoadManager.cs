using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using EPC;

public class SaveLoadManager : MonoBehaviour {
    public string map_name;
    private TMP_InputField inputField;
    public string file_path;
    public props prop_list;
    public Transform parent;
    public string json;
    public Vector3 rote;
    [SerializeField] private List<GameObject> objes = new List<GameObject>();

    [SerializeField] private EPC.parser parser;
    public void Aweke() {
        DontDestroyOnLoad(this.gameObject);
    }
    public void Start() {
        parser = Transform.FindAnyObjectByType<parser>().GetComponent<parser>();
        parent = Transform.FindAnyObjectByType<UnitAsembler>().transform.GetChild(0).transform;// props_parent.transform;
        file_path = pathCombine(Application.persistentDataPath, "Maps");
            // Directory.CreateDirectory(dir_path_maps);
        //load_achieves();
        // save_map("123");
        // load_map("123");
    }

    private string pathCombine(string path, string name) {
        string pth = Path.Combine(path, name);
        return pth;
    }

    public void mapLoad(string map_loc) {
        // if (File.Exists(pathCombine(file_path, map_name))) {
        // var stre = JsonUtility.ToJson(parser.decode_text(json, 0), false);
        string json = File.ReadAllText(map_loc);
        json = json.Remove(0, 14);
        prop_list = JsonUtility.FromJson<props>(json);
        foreach (var prop in prop_list.props_obj) {
            Debug.Log($"{prop.rot} < {Quaternion.Euler(prop.rot)}");
            GameObject gm = Instantiate(Resources.Load<GameObject>($"Props/{prop.id}"), prop.pos, Quaternion.Euler(Vector3.zero), parent);
            objes.Add(gm);
            var prop_com = gm.GetComponent<Prop>();
            prop_com.delete = prop.del;
            prop_com.gravity = prop.grv;
            prop_com.kinematic = prop.kin;

            Destroy(prop_com.gameObject.GetComponent<BuildPrefab>());
            prop_com.transform.position = prop.pos;
            // prop_com.p_set(prop.rot);
            prop_com.transform.eulerAngles = prop.rot;// rotation = Quaternion.Euler(prop.rot); // Rotate(prop.rot, Space.Self);
            prop_com.transform.localScale = prop.scl;
            rote = prop.rot;
        }
    }

    public void mapSave() {
        prop_list = new props();
        foreach (GameObject _prop in objes) {
            var p_l = new prop();
            
            p_l.id = int.Parse(_prop.name.Replace("(Clone)", ""));
            p_l.pos = _prop.transform.position;
            p_l.rot = _prop.transform.eulerAngles;
            p_l.scl = _prop.transform.localScale;
            var p_c = _prop.GetComponent<Prop>();
            p_l.grv = p_c.gravity;
            p_l.del = p_c.delete;
            p_l.kin = p_c.kinematic;
            prop_list.props_obj.Add(p_l);
        }
        json = JsonUtility.ToJson(prop_list, true);
        // string cod = parser.encode_text(json, 0);
        map_name = map_name.Replace(".mdm", "");
        File.WriteAllText(pathCombine(file_path, $"{SceneManager.GetActiveScene().name}_{map_name}.mdm"), "eghDcTCgbCdjCh" + json);
    }
    public void nameEnter() {
        map_name = inputField.text;
    }

    public void PropAdd(GameObject _prop)
    {
        objes.Add(_prop);
        var p_l = new prop();

        // Debug.Log(_prop.name.Replace("(Clone)", ""));
        p_l.id = int.Parse(_prop.name.Replace("(Clone)", ""));
        p_l.pos = _prop.transform.position;
        p_l.rot = _prop.transform.eulerAngles;
        p_l.scl = _prop.transform.localScale;
        var p_c = _prop.GetComponent<Prop>();
        p_l.grv = p_c.gravity;
        p_l.del = p_c.delete;
        p_l.kin = p_c.kinematic;
        prop_list.props_obj.Add(p_l);
    }
    public bool IsPropContains(GameObject _prop)
    {
        PropRemove(_prop);
        return objes.Contains(_prop);
    }
    public void PropRemove(GameObject _prop)
    {
        objes.Remove(_prop);
    }
}

[Serializable]
public class prop {
    public int id;
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scl;
    public bool grv;
    public bool del;
    public bool kin;
}

[Serializable]
public class props {
    public List<prop> props_obj = new List<prop>();
}

