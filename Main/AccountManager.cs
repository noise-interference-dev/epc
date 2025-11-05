using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPC;

public class AccountManager : MonoBehaviour {
    public string _acc_name;
    public int lvl;
    public int xp;

    public TMP_Text _name_text;
    public TMP_InputField _input_field;

    [SerializeField] private string acc_path;
    [SerializeField] private string acc_name;
    /*[SerializeField] private int acc_level;
    [SerializeField] private int acc_xp;*/

    [SerializeField] private EPC.parser parser;
    [SerializeField] private string c_st;
    [SerializeField] private string end;
    [SerializeField] private string json;

    public bool can_play;

    public void sStart()
    {
        if(PlayerPrefs.HasKey("_acc_name"))
        {
            _acc_name = PlayerPrefs.GetString("_acc_name");
            if(_acc_name != "")
            {
                _input_field.text = $"{_acc_name}";
            }
        }
    }


    public void SaveName() {
        json += JsonUtility.ToJson(_acc_name);
        string cod = parser.encode_text(json, 0);
        File.WriteAllText(acc_path, cod);

        /*_name_text.text = _input_field.text;
        _acc_name = _name_text.text.ToString();
        SaveDataName();*/
    }


    private void Start() {
        parser = GetComponent<parser>();
        acc_path = path_comb("player.json");
        if (File.Exists(acc_path)) {
            // SaveName();
            c_st = File.ReadAllText(acc_path);
            end = parser.decode_text(c_st, 7);
        }
        else {
            File.WriteAllText(acc_path, "end line");
        }
    }

    private string path_comb(string path) {
        string pth = Path.Combine(Application.persistentDataPath, path);
        return pth;
    }
}
