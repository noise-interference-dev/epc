// using System.Diagnostics;
using UnityEngine;

public class Builds : MonoBehaviour {
    [SerializeField] private GameObject obj_selected;
    public void propSet() {
        FindAnyObjectByType<PlayerAssembler>().PropSet(obj_selected);
        Debug.Log("0spawn");
    }
}
