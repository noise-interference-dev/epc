using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace EPC {
    public class MainControl : MonoBehaviour {
        public GameObject panel;
        [SerializeField] private bool panelState;
        public List<GameObject> panels = new List<GameObject>();
        public void rend_time_panel(GameObject panel) {
            StopAllCoroutines();
            StartCoroutine(panel_rend(panel));
        }
        private string pathComb(string path, string name) {
            string pth = Path.Combine(path, name);
            return pth;
        }
        private IEnumerator panel_rend(GameObject panel) {
            panel.SetActive(true);
            yield return new WaitForSeconds(1.25f);
            if (panel.activeSelf) panel.SetActive(false);
        }
        public void rend_panels(int enter) {
            for (int i = 0; i < panels.Count; i++) {
                if (i == enter)
                    panels[i].SetActive(true);
                panels[i].SetActive(false);
            }
        }
        public void scene_load(string name) {
            SceneManager.LoadScene(name);
            Debug.Log($"Запуск {name}");
        }
        public void panel_on_off() {
            if (panel) {
                if (!panelState) {
                    panelState = !panelState;
                    panel.SetActive(true);
                }
                else {
                    panelState = !panelState;
                    panel.SetActive(false);
                }
            }
        }       
        public void game_exit() {
            Application.Quit();
            Debug.Log("Выход");
        }
    }
}