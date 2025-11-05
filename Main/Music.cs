using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class Music : MonoBehaviour {
    [System.Serializable]
    public struct clip {
        public string name;
        public string author;
        public bool can_play;
        public AudioClip audio;
    }
    [SerializeField] private TMP_Text c_name;
    [SerializeField] private TMP_Text author;
    public AudioSource source;
    [SerializeField] private List<clip> clips = new List<clip>();
    public int clip_cur = 0;

    public void clip_change(int input) {
        clip_cur += input;
        StopAllCoroutines();
        clip_check();
        StartCoroutine(music_change());
        clip_rend();
    }

    // IEnumerator start_coroutine() {
    //     AudioSource audioSource = GetComponent<AudioSource>();
    //     audioSource.resource = m_Clip;
    //     audioSource.Play();
    //     audioSource.resource = m_Resource;
    //     audioSource.Play();
    // }

    private IEnumerator music_change() {
        while (true) {
            source.resource = clips[clip_cur].audio;
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            clip_change(1);
        }
    }

    public void clip_check() {
        if (clip_cur > clips.Count - 1)
            clip_cur = 0;
        if (clip_cur < 0)
            clip_cur = clips.Count - 1;
        if (!clips[clip_cur].can_play)
            clip_change(1);
    }
    public void clip_rend() {
        for (int i = 0; i < clips.Count; i++) {
            if (source.resource == clips[i].audio) {
                c_name.text = clips[i].name;
                author.text = $"- {clips[i].author} -";
            }
        }
    }
}
