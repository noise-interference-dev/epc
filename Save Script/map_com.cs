using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapComponent : MonoBehaviour {
    public TMP_Text MapName;
    public Image MapIcon;
    public TMP_Text MapLocation;
    public string MapVariation;

    public void MapLoad() {
        string _names = MapName.text;
        var _mapsController = GameObject.FindAnyObjectByType<MapsController>();
        _mapsController.StartMap(MapVariation, _names);
    }
    public void MapDelete() {
        MapsController _mapsController = GameObject.FindAnyObjectByType<MapsController>();
        _mapsController.WantToDeleteMap($"{MapVariation}_{MapName.text}.mdm");
    }
}
