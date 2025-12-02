// using System;
// using System.IO;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using EPC;

// public class maps_con : MonoBehaviour {
//     [SerializeField] private GameObject map_prefab;
//     [SerializeField] private GameObject map_list;
//     [SerializeField] private EPC.parser parser;
//     private string path;
//     public string _map_name;
//     public string _map_loc;
//     public string map_del_name;
//     public GameObject panel_del;

//     private void Awake() => DontDestroyOnLoad(gameObject);

//     private void Start()
//     {
//         parser = GameObject.FindAnyObjectByType<parser>();
//         path = pathComb(Application.persistentDataPath, "Maps");
//         if (!Directory.Exists(path))
//             Directory.CreateDirectory(path);
//         maps_add();
//     }

//     private void maps_add() {
//         unRendMaps();
//         var Dirs = Directory.GetFiles(path, "*.mdm");
//         if (Dirs.Length != 0)
//         {
//             foreach (var file in Dirs)
//             {
//                 string content = File.ReadAllText(file);
//                 string resol = parser.decode_text(content.Substring(0, 14), 6);
//                 if (resol == "canBeREadEblEn") map_load(Path.GetFileName(file));
//             }
//         }
//     }

//     private void map_load(string map_name) {
//         var m_obj = Instantiate(map_prefab, map_list);
//         var m_com = m_obj.GetComponent<map_com>();
        
//         string m_name = Path.GetFileNameWithoutExtension(map_name);
//         string cleanName = m_name.Replace("_*", "");
//         string prefix = cleanName.Substring(0, 3);
//         Debug.Log(prefix);
        
//         m_com.map_name.text = cleanName.Substring(4);
//         m_com.map_loc.text = prefix == "000" ? "Devs room" : prefix == "001" ? "Ground" : "Unknown";
//         m_com.m_loc = $"00{int.Parse(prefix)}";
        
//         // _map_name = map_name;
//         // _map_loc = m_com.m_loc;
//     }
//     private void unRendMaps() {
//         for (int i = 0; i < map_list.childCount; i++) {
//             Destroy(map_list.GetChild(i).gameObject);
//         }
//     }

//     public void map_delete() {
//         StartCoroutine(panelOffer());
//     }
//     IEnumerator panelOffer() {
//         yield return StartCoroutine(fileDel());
//         maps_add();
//         panel_del.SetActive(false);
//     }
//     IEnumerator fileDel() {
//         if (map_del_name != "") File.Delete(pathComb(path, map_del_name));
//         yield return null;
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
//         if (scene.name == "MainMenu" && map_list == null) Destroy(this.gameObject);
//         if (scene.name == _map_loc && _map_name != null) {
//             var loader = FindFirstObjectByType<SaveLoadManager>();
//             // loader.load_map(Path.Combine(path, $"{_map_loc}_{_map_name}.mdm"));
//         }
//     }
//     private string pathComb(string path, string name) {
//         string pth = Path.Combine(path, name);
//         return pth;
//     }

//     public void map_start(string m_loc, string m_name)
//     {
//         _map_loc = m_loc;
//         _map_name = m_name;
//         FindFirstObjectByType<MainControl>().scene_load(m_loc);
//     }

//     void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
//     void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
// }
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Для TMP_InputField, если используешь password
using TMPro; // Если используешь TMP

public class MapsController : MonoBehaviour {
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private Transform mapList;
    // [SerializeField] private TMP_InputField passwordField; // Добавь UI поле для пароля (save/load)
    private string mapsPath;
    private string currentMapName;
    private string currentMapLoc;
    private string mapToDelete;
    private GameObject deletePanel;
    public static MapsController Instance { get; private set; }
    #region Initialization
        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name == currentMapLoc && !string.IsNullOrEmpty(currentMapName)) {
                var loader = FindAnyObjectByType<SaveLoadManager>();
                if (loader != null) {
                    string fullPath = Path.Combine(mapsPath, $"{currentMapLoc}_{currentMapName}.mdm");
                    // string password = passwordField?.text ?? ""; // Берем пароль из UI, если есть
                    // loader.MapLoad(fullPath, password);
                }
            }
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init() 
        {
            if (Instance != null) return;

            GameObject _go = new GameObject("MapManager");
            Instance = _go.AddComponent<MapsController>();
            DontDestroyOnLoad(_go);
        }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            mapsPath = Path.Combine(Application.persistentDataPath, "Maps");
        }


        private void Start() {
            if (!Directory.Exists(mapsPath)) {
                Directory.CreateDirectory(mapsPath);
            }
            RefreshMapsList();
        }
    #endregion
    public void RefreshMapsList() {
        ClearMapList();
        string[] mapFiles = Directory.GetFiles(mapsPath, "*.mdm");
        foreach (string _filePath in mapFiles) {
            string _fileName = Path.GetFileNameWithoutExtension(_filePath);
            if (_fileName.Length < 4) continue;
            // Проверка на читаемость (теперь с шифрованием, убрал старый parser)
            // if (IsValidMapFile(_filePath, "")) { // Пробуем с empty password сначала
            string _mapVariation = _fileName.Substring(0, 3);
            string _mapName = _fileName.Substring(4);

            int _underscope = _fileName.IndexOf("_");
            if (_underscope >= 0 && _underscope + 1 < _fileName.Length)
            {
                _mapVariation = _fileName.Substring(0, _underscope);
                _mapName = _fileName.Substring(_underscope + 1);
            }
            LoadMapEntry(_mapVariation, _mapName);
            // }
        }
    }
    private void LoadMapEntry(string _mapVariation, string _mapName) {
        GameObject mapObject = Instantiate(mapPrefab, mapList);
        var _mapComponent = mapObject.GetComponent<MapComponent>();

        _mapComponent.MapName.text = _mapName;
        _mapComponent.MapLocation.text = VariationToLoaction(_mapVariation);
        _mapComponent.MapVariation = _mapVariation;
    }
    private string VariationToLoaction(string _mapVariation)
    {
        return _mapVariation switch {
            "000" => "Devs room",
            "001" => "Ground",
            "002" => "Lake",
            _ => "Unknown"
        };
    }
    #region Delete
        private void ClearMapList() {
            for (int i = mapList.childCount - 1; i >= 0; i--) {
                Destroy(mapList.GetChild(i).gameObject);
            }
        }
        public void WantToDeleteMap(string _mapToDelete)
        {
            mapToDelete = _mapToDelete;
            deletePanel.SetActive(true);
        }
        public void DeleteMap() {
            StartCoroutine(DeleteMapCoroutine());
        }
        private IEnumerator DeleteMapCoroutine() {
            yield return StartCoroutine(DeleteFile());
            RefreshMapsList();
            deletePanel.SetActive(false);
        }
        private IEnumerator DeleteFile() {
            if (!string.IsNullOrEmpty(mapToDelete)) {
                string _mapFullPath = Path.Combine(mapsPath, mapToDelete);
                if (File.Exists(_mapFullPath)) File.Delete(_mapFullPath);
            }
            yield return null;
        }

    #endregion
    public void StartMap(string _mapLocation, string _mapName) {
        currentMapLoc = _mapLocation;
        currentMapName = _mapName;
        SceneManager.LoadScene(_mapLocation);
    }

    
}