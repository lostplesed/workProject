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
            RefreshState();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (RandMusic.S.choose.Contains(m_fileInfo.relativePath))
            {
                RandMusic.S.choose.Remove(m_fileInfo.relativePath);
            }
            else
            {
                RandMusic.S.choose.Add(m_fileInfo.relativePath);
            }
            RefreshState();
        });
    }

    void RefreshState()
    {
        m_textState.text = RandMusic.S.choose.Contains(m_fileInfo.relativePath) ? "O" : "X";
    }
}
