using UnityEngine;

public class Builds : MonoBehaviour {
    [SerializeField] private GameObject obj_selected;
    public void propSet() {
        FindAnyObjectByType<AsemblyGame>().propSet(obj_selected);
    }
}
