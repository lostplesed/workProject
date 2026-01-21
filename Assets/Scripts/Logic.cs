using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private Button m_buttonOne;
    private string oggUrl = "https://inner-cdn.diguogame.com/SoundProjects/Test/main/Sound/Music/BGM_CORE_GAME_BG_LOOP.ogg";

    // Start is called before the first frame update
    void Start()
    {
        oggUrl = "http://192.168.50.152:8888/download?path=Test%2Fmain%2FSound%2FMusic%2FBGM_CORE_GAME_BG_LOOP.ogg";
        m_buttonOne.onClick.AddListener(() =>
        {
            if (m_audioSource.clip != null)
            {
                m_audioSource.clip = null;
            }
            m_audioSource.Stop();
            StartCoroutine(LoadAndPlayOne());
        });
    }

    private IEnumerator LoadAndPlayOne()
    {
        using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(oggUrl, AudioType.OGGVORBIS))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Load audio failed: {req.error}");
                Debug.LogError($"File URL: {oggUrl}");
                Debug.LogError($"Response Code: {req.responseCode}");
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
            m_audioSource.clip = clip;
            m_audioSource.Play();
        }
    }
}