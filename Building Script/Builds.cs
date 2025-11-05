using UnityEngine;

public class Builds : MonoBehaviour {
    [SerializeField] private GameObject obj_selected;
    public void propSet() {
        FindAnyObjectByType<BuildManager>().propSet(obj_selected);
    }
}
