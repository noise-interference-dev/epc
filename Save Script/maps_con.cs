using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EPC;

public class maps_con : MonoBehaviour {
    [SerializeField] private GameObject map_prefab;
    [SerializeField] private GameObject map_list;
    [SerializeField] private EPC.parser parser;
    private string path;
    public string _map_name;
    public string _map_loc;
    public string map_del_name;
    public GameObject panel_del;

    private void Awake() => DontDestroyOnLoad(gameObject);

    private void Start()
    {
        parser = GameObject.FindAnyObjectByType<parser>();
        path = pathComb(Application.persistentDataPath, "Maps");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        maps_add();
    }

    private void maps_add() {
        unRendMaps();
        var Dirs = Directory.GetFiles(path, "*.mdm");
        if (Dirs.Length != 0)
        {
            foreach (var file in Dirs)
            {
                string content = File.ReadAllText(file);
                string resol = parser.decode_text(content.Substring(0, 14), 6);
                if (resol == "canBeREadEblEn") map_load(Path.GetFileName(file));
            }
        }
    }

    private void map_load(string map_name) {
        var m_obj = Instantiate(map_prefab, map_list.transform);
        var m_com = m_obj.GetComponent<map_com>();
        
        string m_name = Path.GetFileNameWithoutExtension(map_name);
        string cleanName = m_name.Replace("_*", "");
        string prefix = cleanName.Substring(0, 3);
        Debug.Log(prefix);
        
        m_com.map_name.text = cleanName.Substring(4);
        m_com.map_loc.text = prefix == "000" ? "Devs room" : prefix == "001" ? "Ground" : "Unknown";
        m_com.m_loc = $"00{int.Parse(prefix)}";
        
        // _map_name = map_name;
        // _map_loc = m_com.m_loc;
    }
    private void unRendMaps() {
        for (int i = 0; i < map_list.transform.childCount; i++) {
            Destroy(map_list.transform.GetChild(i).gameObject);
        }
    }

    public void map_delete() {
        StartCoroutine(panelOffer());
    }
    IEnumerator panelOffer() {
        yield return StartCoroutine(fileDel());
        maps_add();
        panel_del.SetActive(false);
    }
    IEnumerator fileDel() {
        if (map_del_name != "") File.Delete(pathComb(path, map_del_name));
        yield return null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu" && map_list == null) Destroy(this.gameObject);
        if (scene.name == _map_loc && _map_name != null) {
            var loader = FindFirstObjectByType<SaveLoadManager>();
            loader.load_map(Path.Combine(path, $"{_map_loc}_{_map_name}.mdm"));
        }
    }
    private string pathComb(string path, string name) {
        string pth = Path.Combine(path, name);
        return pth;
    }

    public void map_start(string m_loc, string m_name)
    {
        _map_loc = m_loc;
        _map_name = m_name;
        FindFirstObjectByType<MainControl>().scene_load(m_loc);
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
}