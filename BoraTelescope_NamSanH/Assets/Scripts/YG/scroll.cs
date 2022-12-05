using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class scroll : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    VideoClip FINAL;
    [SerializeField]
    VideoClip Fold;
    [SerializeField]
    VideoClip Unfoldimg;
    [SerializeField]
    VideoClip Unfold;

    [SerializeField]
    GameObject TitleText;
    [SerializeField]
    GameObject SubText;
    [SerializeField]
    GameObject Exit;


    VideoPlayer Video;
    Intelligentinfo info;
    AudioSource ad;

    string ContentName;


    void Start()
    {
        ad = GetComponent<AudioSource>();
        info = GetComponent<Intelligentinfo>();
        Video = GetComponent<VideoPlayer>();
        Video.clip= FINAL;
        ContentName = "ÀÌ½Â¾÷°¡¿Á";
        Video.Pause();
        Invoke("Intelligence", 2f);

    }

    public void Intelligence()
    {
        Video.Play();
        Invoke("VideoStop", 3f);

        int index = -1;
        int index1 = -1;
        int index2 = -1;
        for (int i = 0; i < info.intelliname.Count; i++)
        {
            if (info.intelliname[i] == ContentName)
            {
                index1 = i;
                break;
            }
        }

        for (int i = 0; i < info.intelliname.Count; i++)
        {
            if (info.intelliname[i] == ContentName && i != index1)
            {
                index2 = i;
                break;
            }
        }

        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            index = index1;
        }
        else if (rand == 1)
        {
            index = index2;
        }

        TitleText.GetComponent<TMP_Text>().text = info.intellititle[index] + "(" + info.intellicontent[index] + ")";
        SubText.GetComponent<TMP_Text>().text = info.intellitext[index];
        ad.clip = info.Narration[index];
    }


    // Update is called once per frame
    void VideoStop()
    {
        TitleText.SetActive(true);
        SubText.SetActive(true);
        Exit.SetActive(true);
        Video.Pause();
        ad.Play();
    }

    public void VideoPlay()
    {
        Video.Play();
        Exit.SetActive(false);
        TitleText.GetComponent<TMP_Text>().text = "<fade>" + TitleText.GetComponent<TMP_Text>().text + "</fade>";
        SubText.GetComponent<TMP_Text>().text = "<fade>" + SubText.GetComponent<TMP_Text>().text + "</fade>";
        Invoke("DisObject", 3.5f);
        ad.Stop();
    }
    
    void DisObject()
    {
        TitleText.transform.parent.gameObject.SetActive(false);
    }


}
