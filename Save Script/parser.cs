using UnityEngine;

namespace EPC {
    public class parser : MonoBehaviour {
        [SerializeField] private int key;
        private int code = -1;
        public string encode_text(string txt, int k) {
            if (key != -1) code = key;
            else code = (k == 0 || k == 1) ? 1 : k;
            string res = "";
            for (int i = 0; i < txt.Length; i++) {
                res += (char)(txt[i] ^ code);
            }
            code = -1;
            return res;
        }
        public string decode_text(string txt, int k) {
            if (key != -1) code = key;
            else code = (k == 0 || k == 1) ? 1 : k;
            string res = "";
            for (int i = 0; i < txt.Length; i++) {
                res += (char)(txt[i] ^ code);
            }
            code = -1;
            return res;
        }
    }
}
