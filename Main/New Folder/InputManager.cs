using UnityEngine;

public enum InputType {
    Player,
    Vehicle,
    Game,
    UI
}
public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }
    public InputSystem_Actions actions;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init() {
        if (Instance != null) return;

        GameObject go = new GameObject("InputManager");
        Instance = go.AddComponent<InputManager>();
        DontDestroyOnLoad(go);
    }
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        actions = new InputSystem_Actions();
        actions.Enable();
    }
    private void OnDestroy() {
        if (Instance == this) {
            actions.Disable();
            actions = null;
            Instance = null;
        }
    }

    public void ChangeInputMap(InputType _InputType) {
        if (Instance == _InputType) return;
        switch (_InputType) {
            case InputType.Player:
                actions.Player.Enable();
                actions.Vehicle.Disable();
                actions.Game.Disable();
                actions.UI.Disable();
                break;
            case InputType.Vehicle:
                actions.Player.Disable();
                actions.Vehicle.Enable();
                actions.Game.Disable();
                actions.UI.Disable();
                break;
            case InputType.Game:
                actions.Player.Disable();
                actions.Vehicle.Disable();
                actions.Game.Enable();
                actions.UI.Disable();
                break;
            case InputType.UI:
                actions.Player.Disable();
                actions.Vehicle.Disable();
                actions.Game.Disable();
                actions.UI.Enable();
                break;
        }
    }
}