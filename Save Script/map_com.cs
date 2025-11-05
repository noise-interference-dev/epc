using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class map_com : MonoBehaviour {
    public TMP_Text map_name;
    public Image map_icon;
    public TMP_Text map_loc; 
    public string m_loc;

    public void map_load() {
        string names = map_name.text;
        GameObject.FindAnyObjectByType<maps_con>().map_start(m_loc, names);
    }
    public void map_del() {
        maps_con gmo = GameObject.FindAnyObjectByType<maps_con>();
        gmo.map_del_name = $"{m_loc}_{map_name.text}.mdm";
        gmo.panel_del.SetActive(true);
    }
}
