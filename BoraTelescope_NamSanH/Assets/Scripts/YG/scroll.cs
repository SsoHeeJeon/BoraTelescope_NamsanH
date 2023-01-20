using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class scroll : MonoBehaviour
{
    //public State state = 0;

    // Start is called before the first frame update
    [SerializeField]
    DocentAni docentani;

    [SerializeField]
    VideoClip FINAL;

    [SerializeField]
    Button DocentBtn;

    public GameObject TitleText_KE;
    [SerializeField]
    GameObject Title_KE;
    [SerializeField]
    GameObject SubText_KE;

    public GameObject TitleText_C;
    [SerializeField]
    GameObject Title_C;
    [SerializeField]
    GameObject SubText_C;

    public GameObject TitleText_J;
    [SerializeField]
    GameObject Title_J;
    [SerializeField]
    GameObject SubText_J;

    [SerializeField]
    NamSanHMode namsan;
    public Button Close;
    public GameObject ChromaVideo;
    VideoPlayer Video;
    Intelligentinfo info;
    public AudioSource ad;

    public string ContentName;
    bool DisText;
    bool OnText;
    void Start()
    {
        ad = GetComponent<AudioSource>();
        info = GetComponent<Intelligentinfo>();
        Video = ChromaVideo.GetComponent<VideoPlayer>();
        Video.clip= FINAL;
        Video.Pause();
    }

    private void Update()
    {
        if(DisText)
        {
            switch(namsan.gamemanager.curlang)
            {
                case GameManager.Language_enum.Korea:
                    Title_KE.transform.parent.parent.GetComponent<Image>().fillAmount -= Time.deltaTime * 0.3f;
                    break;
                case GameManager.Language_enum.English:
                    Title_KE.transform.parent.parent.GetComponent<Image>().fillAmount -= Time.deltaTime * 0.3f;
                    break;
                case GameManager.Language_enum.Chinese:
                    Title_C.transform.parent.parent.GetComponent<Image>().fillAmount -= Time.deltaTime * 0.3f;
                    break;
                case GameManager.Language_enum.Japan:
                    Title_J.transform.parent.parent.GetComponent<Image>().fillAmount -= Time.deltaTime * 0.3f;
                    break;
            }
        }

        if(OnText)
        {
            switch (namsan.gamemanager.curlang)
            {
                case GameManager.Language_enum.Korea:
                    Title_KE.transform.parent.parent.GetComponent<Image>().fillAmount += Time.deltaTime * 0.4f;
                    break;
                case GameManager.Language_enum.English:
                    Title_KE.transform.parent.parent.GetComponent<Image>().fillAmount += Time.deltaTime * 0.4f;
                    break;
                case GameManager.Language_enum.Chinese:
                    Title_C.transform.parent.parent.GetComponent<Image>().fillAmount += Time.deltaTime * 0.4f;
                    break;
                case GameManager.Language_enum.Japan:
                    Title_J.transform.parent.parent.GetComponent<Image>().fillAmount += Time.deltaTime * 0.4f;
                    break;
            }
        }

    }
    
    void OnRenderer()
    {
        ChromaVideo.GetComponent<MeshRenderer>().enabled = true;
    }


    public void Intelligence()
    {
        Title_KE.transform.parent.parent.gameObject.SetActive(true);
        ad.enabled = true;
        namsan.Narration.clip = null;
        Video.clip = FINAL;
        Video.Play();
        switch (namsan.gamemanager.curlang)
        {
            case GameManager.Language_enum.Korea:
                Title_KE.transform.parent.parent.GetComponent<Image>().fillAmount = 0.3f;
                Title_KE.SetActive(true);
                TitleText_KE.SetActive(true);
                SubText_KE.SetActive(true);
                break;
            case GameManager.Language_enum.English:
                Title_KE.transform.parent.parent.GetComponent<Image>().fillAmount = 0.3f;
                Title_KE.SetActive(true);
                TitleText_KE.SetActive(true);
                SubText_KE.SetActive(true);
                break;
            case GameManager.Language_enum.Chinese:
                Title_C.transform.parent.parent.GetComponent<Image>().fillAmount = 0.3f;
                Title_C.SetActive(true);
                TitleText_C.SetActive(true);
                SubText_C.SetActive(true);
                break;
            case GameManager.Language_enum.Japan:
                Title_J.transform.parent.parent.GetComponent<Image>().fillAmount = 0.3f;
                Title_J.SetActive(true);
                TitleText_J.SetActive(true);
                SubText_J.SetActive(true);
                break;

        }
        OnText = true;
        Invoke("VideoStop", 3f);
        Invoke("OnRenderer", 0.5f);

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
            if (info.intelliname[i] == ContentName&& i != index1)
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

        if(GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            TitleText_KE.GetComponent<TMP_Text>().text = "<" + info.intellicontent[index] + ">";
            if (info.ganzi[index]!="")
            {
                Title_KE.GetComponent<TMP_Text>().text = " " +info.intellititle[index] +"\n"+ "(" + info.ganzi[index]+")";
            }
            else
            {
                Title_KE.GetComponent<TMP_Text>().text = info.intellititle[index];
            }
            SubText_KE.GetComponent<TMP_Text>().text = info.intellitext[index];
            ad.clip = info.Narration[index];
            docentani.NarrtionLen = ad.clip.length;
        }
        else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            TitleText_KE.GetComponent<TMP_Text>().text = "<" + info.intellicontent_E[index] + ">";
            if (info.ganzi[index]!="")
            {
                if(index == 2)
                {
                    Title_KE.GetComponent<TMP_Text>().text = "Seon-Haeng-KiEon(先行其言)\nYi-Hu-Zhong-Ji (而後從之)";
                }
                else if(index ==3)
                {
                    Title_KE.GetComponent<TMP_Text>().text = "Ki-Shin-Bu-Jeong (其身不正)\nSu-Ryeong-BuJong(雖令不從)";
                }
                else if (index == 12)
                {
                    Title_KE.GetComponent<TMP_Text>().text = "Paek-Ron-Bul-Ryeo-Il-Haeng (百論不如一行)";
                }
                else if(index ==13)
                {
                    Title_KE.GetComponent<TMP_Text>().text = "Jeok-Seon-Ji-Ka (積善之家)\nPilYou-Yeo-Kyoung(必有餘慶)";
                }
                else if(index ==14)
                {
                    Title_KE.GetComponent<TMP_Text>().text = "Myeon-Il-Si-Ji-Bun(忍一時之忿)\nMyeonPaek-Il-JiWoo(免百日之憂)";
                    Title_KE.GetComponent<TMP_Text>().fontSize = 32;
                }
                else if(index ==18)
                {
                    Title_KE.GetComponent<TMP_Text>().text = "Ye-Sok-Sang-Kyo (禮俗相交)\nHwan-Nan-Sang-Hyul (患難相恤)";
                }
                else
                {
                    Title_KE.GetComponent<TMP_Text>().text = info.intellititle_E[index] + "\n(" + info.ganzi[index] + ")";
                }
            }
            else
            {
                Title_KE.GetComponent<TMP_Text>().text = info.intellititle_E[index];
            }
            SubText_KE.GetComponent<TMP_Text>().text = info.intellitext_E[index];
            ad.clip = info.Narration_E[index];
            docentani.NarrtionLen = ad.clip.length;
        }
        else if(GameManager.currentLang == GameManager.Language_enum.Chinese)
        {
            if(index==4)
            {
                info.ganzi[index] = "事必归正";
            }
            if(index == 16)
            {
                info.ganzi[index] = "髀肉之嘆";
            }
            TitleText_C.GetComponent<TMP_Text>().text = "<" + info.intellicontent_C[index] + ">";
            if (info.ganzi[index] != "")
            {
                Title_C.GetComponent<TMP_Text>().text = info.ganzi[index];
            }
            else
            {
                Title_C.GetComponent<TMP_Text>().text = info.ganzi[index];
            }
            SubText_C.GetComponent<TMP_Text>().text = info.intellitext_C[index];
            ad.clip = info.Narration_C[index];
            docentani.NarrtionLen = ad.clip.length;
            if (index == 4)
            {
                info.ganzi[index] = "事必歸正";
            }
            if (index == 16)
            {
                info.ganzi[index] = "";
            }
        }
        else if(GameManager.currentLang == GameManager.Language_enum.Japan)
        {
            if (index == 4)
            {
                info.ganzi[index] = "事必帰正";
            }
            if (index == 16)
            {
                info.ganzi[index] = "髀肉之嘆";
            }
            TitleText_J.GetComponent<TMP_Text>().text = "<" + info.intellicontent_J[index] + ">";
            if (info.ganzi[index] != "")
            {
                Title_J.GetComponent<TMP_Text>().text = info.ganzi[index];
            }
            else
            {
                Title_J.GetComponent<TMP_Text>().text = info.ganzi[index];
            }
            SubText_J.GetComponent<TMP_Text>().text = info.intellitext_J[index];
            ad.clip = info.Narration_J[index];
            docentani.NarrtionLen = ad.clip.length;
            if (index == 4)
            {
                info.ganzi[index] = "事必歸正";
            }
            if (index == 16)
            {
                info.ganzi[index] = "";
            }
        }
    }


    // Update is called once per frame
    void VideoStop()
    {
        OnText=false;
        DocentBtn.enabled = true;
        Close.gameObject.SetActive(true);
        switch (namsan.gamemanager.curlang)
        {
            case GameManager.Language_enum.Korea:
                Title_KE.SetActive(true);
                TitleText_KE.SetActive(true);
                SubText_KE.SetActive(true);
                Title_C.SetActive(false);
                TitleText_C.SetActive(false);
                SubText_C.SetActive(false);
                Title_J.SetActive(false);
                TitleText_J.SetActive(false);
                SubText_J.SetActive(false);
                break;
            case GameManager.Language_enum.English:
                Title_KE.SetActive(true);
                TitleText_KE.SetActive(true);
                SubText_KE.SetActive(true);
                Title_C.SetActive(false);
                TitleText_C.SetActive(false);
                SubText_C.SetActive(false);
                Title_J.SetActive(false);
                TitleText_J.SetActive(false);
                SubText_J.SetActive(false);
                break;
            case GameManager.Language_enum.Chinese:
                Title_C.SetActive(true);
                TitleText_C.SetActive(true);
                SubText_C.SetActive(true);
                Title_J.SetActive(false);
                TitleText_J.SetActive(false);
                SubText_J.SetActive(false);
                Title_KE.SetActive(false);
                TitleText_KE.SetActive(false);
                SubText_KE.SetActive(false);
                break;
            case GameManager.Language_enum.Japan:
                Title_J.SetActive(true);
                TitleText_J.SetActive(true);
                SubText_J.SetActive(true);
                Title_KE.SetActive(false);
                TitleText_KE.SetActive(false);
                SubText_KE.SetActive(false);
                Title_C.SetActive(false);
                TitleText_C.SetActive(false);
                SubText_C.SetActive(false);
                break;

        }
        Video.Pause();
        docentani.Talk();
        ad.Play();
    }

    public void VideoPlay()
    {
        Video.Play();
        Invoke("DisObject", 3.5f);
        Invoke("Distrue", 0.6f);
        docentani.CancelTalk();
        ad.Stop();
    }

    void Distrue()
    {
        DisText = true;
    }
    
    public void DisObject()
    {
        Close.gameObject.SetActive(false);
        ChromaVideo.GetComponent<MeshRenderer>().enabled = false;
        switch (namsan.gamemanager.curlang)
        {
            case GameManager.Language_enum.Korea:
                TitleText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.GetComponent<TMP_Text>().text = "";
                SubText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.SetActive(false);
                TitleText_KE.SetActive(false);
                SubText_KE.SetActive(false);
                break;
            case GameManager.Language_enum.English:
                TitleText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.GetComponent<TMP_Text>().text = "";
                SubText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.SetActive(false);
                TitleText_KE.SetActive(false);
                SubText_KE.SetActive(false);
                break;
            case GameManager.Language_enum.Chinese:
                TitleText_C.GetComponent<TMP_Text>().text = "";
                Title_C.GetComponent<TMP_Text>().text = "";
                SubText_C.GetComponent<TMP_Text>().text = "";
                Title_C.SetActive(false);
                TitleText_C.SetActive(false);
                SubText_C.SetActive(false);
                break;
            case GameManager.Language_enum.Japan:
                TitleText_J.GetComponent<TMP_Text>().text = "";
                Title_J.GetComponent<TMP_Text>().text = "";
                SubText_J.GetComponent<TMP_Text>().text = "";
                Title_J.SetActive(false);
                TitleText_J.SetActive(false);
                SubText_J.SetActive(false);
                break;

        }
        DisText = false;
        Title_KE.GetComponent<TMP_Text>().fontSize = 35;
        ChromaVideo.gameObject.SetActive(false);
    }

    public void ScrollHome()
    {
        CancelInvoke("DisObject");
        Video.clip = null;
        DisText = true;
        ad.Stop();
        switch (namsan.gamemanager.curlang)
        {
            case GameManager.Language_enum.Korea:
                TitleText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.GetComponent<TMP_Text>().text = "";
                SubText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.SetActive(false);
                TitleText_KE.SetActive(false);
                SubText_KE.SetActive(false);
                //TitleText_KE.transform.parent.parent.gameObject.SetActive(false);
                break;
            case GameManager.Language_enum.English:
                TitleText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.GetComponent<TMP_Text>().text = "";
                SubText_KE.GetComponent<TMP_Text>().text = "";
                Title_KE.SetActive(false);
                TitleText_KE.SetActive(false);
                SubText_KE.SetActive(false);
                //TitleText_KE.transform.parent.parent.gameObject.SetActive(false);
                break;
            case GameManager.Language_enum.Chinese:
                TitleText_C.GetComponent<TMP_Text>().text = "";
                Title_C.GetComponent<TMP_Text>().text = "";
                SubText_C.GetComponent<TMP_Text>().text = "";
                Title_C.SetActive(false);
                TitleText_C.SetActive(false);
                SubText_C.SetActive(false);
                //TitleText_C.transform.parent.parent.gameObject.SetActive(false);
                break;
            case GameManager.Language_enum.Japan:
                TitleText_J.GetComponent<TMP_Text>().text = "";
                Title_J.GetComponent<TMP_Text>().text = "";
                SubText_J.GetComponent<TMP_Text>().text = "";
                Title_J.SetActive(false);
                TitleText_J.SetActive(false);
                SubText_J.SetActive(false);
                //TitleText_J.transform.parent.parent.gameObject.SetActive(false);
                break;

        }
        DisText = false;
        DocentBtn.enabled = true;
    }

    public void InvokeCancel()
    {
        CancelInvoke("VideoStop");
    }

}
