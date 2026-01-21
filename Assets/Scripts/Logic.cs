using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private Button m_buttonOne;
    [SerializeField] private Button m_buttonRand;
    [SerializeField] private Button m_buttonChoose;
    [SerializeField] private ScrollRect m_scrollRect;
    [SerializeField] private GameObject m_scrollContent;
    [SerializeField] private GameObject m_scrollCell;


    private string oggUrl = "https://inner-cdn.diguogame.com/SoundProjects/Test/main/Sound/Music/BGM_CORE_GAME_BG_LOOP.ogg";
    private string dirJsonUrl = "http://192.168.50.152:8888/download?path=.files.json";

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
        m_buttonChoose.onClick.AddListener(() =>
        {
            m_scrollRect.gameObject.SetActive(!m_scrollRect.gameObject.activeSelf);
            if (!m_scrollRect.gameObject.activeSelf)
            {
                foreach (Transform child in m_scrollContent.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            else
            {
                StartCoroutine(ChooseFromRemoteDir());
            }
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

    private IEnumerator ChooseFromRemoteDir()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(dirJsonUrl))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Download JSON failed: {req.error}");
                Debug.LogError($"File URL: {dirJsonUrl}");
                Debug.LogError($"Response Code: {req.responseCode}");
                yield break;
            }

            string jsonContent = req.downloadHandler.text;
            OggFileList list = SoundDirFileDeSerializer.Deserialize(jsonContent);
            Debug.Log($"list count:{list.files.Count}");
            for (int i = 0; i < list.files.Count; i++)
            {
                GameObject cell =  Instantiate(m_scrollCell, m_scrollContent.transform);
                ScrollViewCell scrollViewCell = cell.GetComponent<ScrollViewCell>();
                scrollViewCell.FileInfo = list.files[i];
            }
        }
    }
}
