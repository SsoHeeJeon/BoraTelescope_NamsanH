using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class scroll : MonoBehaviour
{
    public enum State
    {
        Kor,
        Eng,
    }
    public State state = 0;

    // Start is called before the first frame update
    [SerializeField]
    VideoClip FINAL;
    [SerializeField]
    VideoClip Fold;
    [SerializeField]
    VideoClip Unfoldimg;
    [SerializeField]
    VideoClip Unfold;


    public GameObject TitleText;
    [SerializeField]
    GameObject Title;
    [SerializeField]
    GameObject SubText;
    [SerializeField]
    GameObject Exit;
    [SerializeField]
    NamSanHMode namsan;

    VideoPlayer Video;
    Intelligentinfo info;
    public AudioSource ad;

    public string ContentName;
    bool DisText;

    void Start()
    {
        ad = GetComponent<AudioSource>();
        info = GetComponent<Intelligentinfo>();
        Video = GetComponent<VideoPlayer>();
        Video.clip= FINAL;
        Video.Pause();
    }

    private void Update()
    {
        if(DisText)
        {
            Title.transform.parent.GetComponent<Image>().fillAmount -= Time.deltaTime * 0.5f;
        }
    }

    public void Intelligence()
    {
        ad.enabled = true;
        namsan.Narration.clip = null;
        Video.clip = FINAL;
        Video.Play();
        Title.transform.parent.GetComponent<Image>().fillAmount = 1;
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
            if(index2 == -1)
            {
                index = index1;
            }
            else
            {
                index = index2;
            }
        }

        if(state ==State.Kor)
        {
            TitleText.GetComponent<TMP_Text>().text = info.intellicontent[index];
            Title.GetComponent<TMP_Text>().text = info.intellititle[index] + "(" + info.ganzi[index]+")";
            SubText.GetComponent<TMP_Text>().text = info.intellitext[index];
            ad.clip = info.Narration[index];
        }
        else if(state==State.Eng)
        {
            TitleText.GetComponent<TMP_Text>().text = info.intellicontent_E[index];
            Title.GetComponent<TMP_Text>().text = info.intellititle_E[index] + "(" + info.ganzi[index] + ")";
            SubText.GetComponent<TMP_Text>().text = info.intellitext_E[index];
            ad.clip = info.Narration_E[index];
        }
    }


    // Update is called once per frame
    void VideoStop()
    {
        print("Cancel");
        Title.SetActive(true);
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
        Invoke("DisObject", 3.5f);
        Invoke("Distrue", 0.6f);
        ad.Stop();
    }

    void Distrue()
    {
        DisText = true;
    }
    
    void DisObject()
    {
        TitleText.GetComponent<TMP_Text>().text = "";
        Title.GetComponent<TMP_Text>().text = "";
        SubText.GetComponent<TMP_Text>().text = "";
        Title.SetActive(false);
        TitleText.SetActive(false);
        SubText.SetActive(false);
        DisText = false;
        TitleText.transform.parent.parent.gameObject.SetActive(false);
    }

    public void ScrollHome()
    {
        CancelInvoke("DisObject");
        Video.clip = null;
        Exit.SetActive(false);
        DisText = true;
        ad.Stop();
        TitleText.GetComponent<TMP_Text>().text = "";
        Title.GetComponent<TMP_Text>().text = "";
        SubText.GetComponent<TMP_Text>().text = "";
        Title.SetActive(false);
        TitleText.SetActive(false);
        SubText.SetActive(false);
        DisText = false;
        TitleText.transform.parent.parent.gameObject.SetActive(false);
    }

    public void InvokeCancel()
    {
        CancelInvoke("VideoStop");
    }

}
