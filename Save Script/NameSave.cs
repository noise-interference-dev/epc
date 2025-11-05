using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameSave : MonoBehaviour
{
    public TMP_Text MyText;
    public TMP_InputField _InputField;
    public string PlayerName;

    public void Start()
    {
        if(PlayerPrefs.HasKey("PlayerName"))
        {
            PlayerName = PlayerPrefs.GetString("PlayerName");
            if(PlayerName != "")
            {
                _InputField.text = $"{PlayerName}";
            }
        }
    }

    private void SaveDataName()
    {
        if(PlayerName != "")
        {
            PlayerPrefs.SetString("PlayerName", PlayerName);
            PlayerPrefs.Save();
        }
    }

    public void SaveName()
    {
        MyText.text = _InputField.text;
        PlayerName = MyText.text.ToString();
        SaveDataName();
    }
}
