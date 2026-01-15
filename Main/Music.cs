// using System;
// using System.IO;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Audio;
// using TMPro;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EPC {
    [Serializable]
    public class ClipData {
        public string name;
        public string author;
        public bool can_play = true;
        public AudioClip audio;
    }

    public class Music : MonoBehaviour {
        [SerializeField] private TMP_Text clipNameTxt, clipAuthorTxt;
        [SerializeField] private List<ClipData> clips = new List<ClipData>();
        [SerializeField] private int clipCur = 0;
        [SerializeField] private AudioSource source;
        private Coroutine musicCoroutine;

        private void Start() {
            if (clips.Count > 0) PlayTrack(clipCur);
        }

        public void ClipChange(int input) {
            clipCur += input;
            ValidateIndex();

            int _i = 0;
            while (!clips[clipCur].can_play && _i < clips.Count) {
                clipCur = (clipCur + 1) % clips.Count;
                _i++;
            }

            if (clips[clipCur].can_play) PlayTrack(clipCur);
            else Debug.LogWarning("No Tracks!");
        }

        private void PlayTrack(int index) {
            if (musicCoroutine != null) StopCoroutine(musicCoroutine);
            
            ClipData track = clips[index];
            source.clip = track.audio;
            source.Play();

            if (clipNameTxt is not null) clipNameTxt.text = track.name;
            if (clipAuthorTxt is not null) clipAuthorTxt.text = $"- {track.author} -";

            musicCoroutine = StartCoroutine(WaitAndNext());
        }

        private IEnumerator WaitAndNext() {
            while (source.clip != null && source.time < source.clip.length - 0.1f) {
                yield return new WaitForSeconds(1f); 
            }
            yield return new WaitForSeconds(1f);
            ClipChange(1);
        }

        private void ValidateIndex() {
            if (clipCur >= clips.Count) clipCur = 0;
            if (clipCur < 0) clipCur = clips.Count - 1;
        }
    }
}
