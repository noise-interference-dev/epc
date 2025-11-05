using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class main_settings : MonoBehaviour
{
    [System.Serializable]
    public struct set {
        public string name;
        public bool can_change;
        public bool active;
    }
    public List<set> settings_com = new List<set>();
    
    public void addSetting(string str, string rank) {
        for (int i = 0; i < settings_com.Count; i++) {
            var dash = settings_com[i];
            if (settings_com[i].name == str) {
                if (settings_com[i].can_change) {
                    dash.active = !dash.active;
                    settings_com[i] = dash;
                }
            }
        }
    }
    public void rendSettting(TMP_Text txt, string str) {
        for (int i = 0; i < settings_com.Count; i++) {
            var dash = settings_com[i];
            if (settings_com[i].name == str) {
                if (settings_com[i].can_change) {
                    dash.active = !dash.active;
                    settings_com[i] = dash;
                }
            }
        }
        if (txt.text == "Off") txt.text = "On";
        else if (txt.text == "On") txt.text = "Off";
    }
}

