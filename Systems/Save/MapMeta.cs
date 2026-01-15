using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapMeta : MonoBehaviour {
    [SerializeField] private TMP_Text MapName;
    [SerializeField] private TMP_Text MapLocation;
    [SerializeField] private string MapVariation;
    [SerializeField] private Image MapIcon;

    public void MapLoad() {
        var _mapsController = GameObject.FindAnyObjectByType<MapsController>();
        _mapsController.StartMap(MapLocation.text, MapVariation);
    }
    public void MapDelete() {
        MapsController _mapsController = GameObject.FindAnyObjectByType<MapsController>();
        _mapsController.WantToDeleteMap($"{MapLocation.text}_{MapName.text}.mdm");
    }
    public void MapSet(string _mapName, string _mapLocation, string _mapVariation, Sprite _mapIcon) {
        MapName.text = _mapName;
        MapLocation.text = _mapLocation;
        MapIcon.sprite = _mapIcon;
        MapVariation = _mapVariation;
    }
}
