// using UnityEngine;
// using UnityEngine.SceneManager;

// public class MapsController : MonoBehaviour
// {
//     // [SerializeField] private GameObject mapPrefab;
//     // [SerializeField] private RectTransform mapList;
//     // [SerializeField] private TMP_InputField passwordField;
//     [SerializeField] private string mapsPath;
//     // private string currentMapName;
//     // private string currentMapLoc;
//     // private string mapToDelete;
//     // private GameObject deletePanel;
//     public static MapsController Instance { get; private set; }

//     private void Awake() {
//         if (Instance != null && Instance != this) {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;
//         DontDestroyOnLoad(gameObject);
//         mapsPath = Path.Combine(Application.persistentDataPath, "Maps");
//     }
//     private void Start() {
//         if (!Directory.Exists(mapsPath)) Directory.CreateDirectory(mapsPath);
//         RefreshMapsList();
//     }
//     private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
//     private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
//         if (scene.name == currentMapLoc && !string.IsNullOrEmpty(currentMapName)) {
//             var loader = FindAnyObjectByType<SaveLoadManager>();
//             if (loader != null) {
//                 string fullPath = Path.Combine(mapsPath, $"{currentMapLoc}_{currentMapName}.mdm");
//                 // string password = passwordField?.text ?? "";
//                 // loader.MapLoad(fullPath, password);
//             }
//         }
//     }
//     // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//     // public static void Init()  {
//     //     if (Instance != null) return;

//     //     GameObject _go = new GameObject("MapManager");
//     //     Instance = _go.AddComponent<MapsController>();
//     //     DontDestroyOnLoad(_go);
//     // }
    
//     public void RefreshMapsList() {
//         Scene scene = SceneManager.GetActiveScene();
//         if (scene.name != "MainMenu") return;
//         // ClearMapList();
//         string[] mapFiles = Directory.GetFiles(mapsPath, "*.mdm");
//         foreach (string _filePath in mapFiles) {
//             string _fileName = Path.GetFileNameWithoutExtension(_filePath);
//             if (_fileName.Length < 4) continue;
//             string _mapVariation = _fileName.Substring(0, 3);
//             string _mapName = _fileName.Substring(4);

//             int _underscope = _fileName.IndexOf("_");
//             if (_underscope >= 0 && _underscope + 1 < _fileName.Length) {
//                 _mapVariation = _fileName.Substring(0, _underscope);
//                 _mapName = _fileName.Substring(_underscope + 1);
//             }
//             LoadMapEntry(_mapVariation, _mapName);
//         }
//     }
//     private string VariationToLoaction(string _mapVariation) {
//         return _mapVariation switch {
//             "--1" => "Devs room",
//             "001" => "Ground",
//             "002" => "Lake",
//             _ => "Unknown"
//         };
//     }
// }
