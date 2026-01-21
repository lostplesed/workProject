using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewCell : MonoBehaviour
{
    [SerializeField] private Text m_textFile;
    [SerializeField] private Text m_textState;

    private OggFileInfo m_fileInfo;
    public OggFileInfo FileInfo
    {
        set
        {
            m_fileInfo = value;

            m_textFile.text = Path.GetFileNameWithoutExtension(m_fileInfo.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log(m_fileInfo.name);
        });
    }
}
