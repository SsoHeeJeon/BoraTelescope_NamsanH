using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamSanHMode : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject CameraWindow;
    public LabelDetail labeldetail;
    public Docent_Avatar avatar;
    public TimeFlow timeflow;

    public GameObject Docent_avartar;
    public GameObject AllUI;
    public GameObject Docent_AllUI;
    public GameObject Docent_Detail;
    public Text Detail_Name;
    public GameObject Docent_guide;
    public GameObject OnOff_Btn;

    public GameObject SelectLabel;

    public See360 see360;
    public GameObject obj360;
    public Material mat360;
    public Sprite Narr_Off;
    public Sprite Narr_On;
    public Scrollbar DetailImage_Scroll;
    public RectTransform DetailImage_Content;

    public AudioSource Narration;
    public Sprite[] House = new Sprite[10];
    public GameObject[] DetailImage = new GameObject[10];

    public bool PlayNarr = false;

    [SerializeField]
    scroll scroll;
    [SerializeField]
    GameObject Notice360;
    public GameObject Merry;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();
        if (timeflow.TFBackground.activeSelf)
        {
            timeflow.TFBackground.SetActive(false);
        }
        Close360();
    }

    private void Update()
    {
        print(Narration.isPlaying);
    }

    public void NaviLabel(GameObject Label)
    {
        if (gamemanager.Label_Cate_1.Contains(Label.name))      // 시대별변천
        {
            if (obj360.activeSelf)
            {
                Close360();
            }
            scroll.VideoPlay();
            if (timeflow.TFBackground.activeSelf)
            {
                if(SelectLabel == Label)
                {
                    timeflow.CloseTF();
                } else if(SelectLabel != Label)
                {
                    timeflow.ReadytoStart(Label);
                }
            }
            else if (!timeflow.TFBackground.activeSelf)
            {
                timeflow.ReadytoStart(Label);
            }
        } else if (gamemanager.Label_Cate_2.Contains(Label.name))       // 도슨트
        {
            if (timeflow.TFBackground.activeSelf)
            {
                timeflow.CloseTF();
            }

            if (obj360.activeSelf)
            {
                if (SelectLabel == Label)
                {
                    Close360();
                } else if(SelectLabel != Label)
                {
                    ReadyTo360(Label);
                }
            } else if (!obj360.activeSelf)
            {
                ReadyTo360(Label);
            }
        }
    }

    public void ReadyTo360(GameObject type)
    {
        OnOff_Btn.SetActive(false);
        Notice360.SetActive(true);
        scroll.GetComponent<scroll>().ScrollHome();
        SelectLabel = type;
        obj360.gameObject.SetActive(true);
        see360.enabled = true;

        Docent_Start();

        int num = int.Parse(type.name.Substring(5));
        mat360.SetTexture("_MainTex", House[num - 1].texture);

        Docent_AllUI.SetActive(true);

        DetailImage_Scroll.value = 0;
        for(int index = 0; index < DetailImage.Length; index++)
        {
            DetailImage[index].SetActive(false);
        }
        DetailImage[num-1].SetActive(true);

        switch (DetailImage[num - 1].transform.childCount)
        {
            case 1:
                DetailImage_Content.sizeDelta = new Vector2(484, 300);
                break;
            case 2:
                DetailImage_Content.sizeDelta = new Vector2(945, 300);
                break;
            case 3:
                DetailImage_Content.sizeDelta = new Vector2(1410, 300);
                break;
            case 4:
                DetailImage_Content.sizeDelta = new Vector2(1875, 300);
                break;
        }
        Detailname(num - 1);
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_K[num + 4];
            Narration.clip = gamemanager.Narration_Docent_K[num - 1];
            Docent_avartar.transform.GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
            Docent_avartar.transform.GetComponent<DocentAni>().AdClip = Narration.clip;
            //labeldetail.Detail_Background.GetComponent<Image>().sprite = gamemanager.DetailImage_K[num - 1];
        } else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_E[num + 4];
            Narration.clip = gamemanager.Narration_Docent_E[num - 1];
            Docent_avartar.transform.GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
            Docent_avartar.transform.GetComponent<DocentAni>().AdClip = Narration.clip;
            //labeldetail.Detail_Background.GetComponent<Image>().sprite = gamemanager.DetailImage_E[num - 1];
        }
        else if(GameManager.currentLang == GameManager.Language_enum.Chinese)
        {
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_C[num + 4];
            Narration.clip = gamemanager.Narration_Docent_C[num - 1];
            Docent_avartar.transform.GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
            Docent_avartar.transform.GetComponent<DocentAni>().AdClip = Narration.clip;
        }
        else if(GameManager.currentLang == GameManager.Language_enum.Japan)
        {
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_J[num + 4];
            Narration.clip = gamemanager.Narration_Docent_J[num - 1];
            Docent_avartar.transform.GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
            Docent_avartar.transform.GetComponent<DocentAni>().AdClip = Narration.clip;
        }
        Docent_avartar.transform.GetComponent<DocentAni>().Talk();
        Narration.Play();

        if (gamemanager.NaviRect.sizeDelta.x > GameManager.barClose)
        {
            gamemanager.navi_t = 0;
            gamemanager.NaviOn = true;
            gamemanager.moveNavi = true;
        }

        AllUI.SetActive(false);

        labeldetail.ChangeDetailLanguage();
        scroll.DisObject();
        print("Play");
    }

    public void Detailname(int num)
    {
        if(GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            switch (num)
            {
                case 0:
                    Detail_Name.text = "마당";
                    break;
                case 1:
                    Detail_Name.text = "천우각";
                    break;
                case 2:
                    Detail_Name.text = "청학지";
                    break;
                case 3:
                    Detail_Name.text = "서울남산국악당";
                    break;
                case 4:
                    Detail_Name.text = "삼각동 도편수\r\n이승업 가옥";
                    break;
                case 5:
                    Detail_Name.text = "옥인동 윤씨 가옥";
                    break;
                case 6:
                    Detail_Name.text = "삼청동 오위장\r\n김춘영 가옥";
                    break;
                case 7:
                    Detail_Name.text = "관훈동 민씨 가옥";
                    break;
                case 8:
                    Detail_Name.text = "제기동 해풍부원군\r\n윤택영 재실";
                    break;
                case 9:
                    Detail_Name.text = "전망대";
                    break;
            }
        } else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            switch (num)
            {
                case 0:
                    Detail_Name.text = "Madang";
                    break;
                case 1:
                    Detail_Name.text = "Cheonwu Pavilion";
                    break;
                case 2:
                    Detail_Name.text = "Cheonghakji Pond";
                    break;
                case 3:
                    Detail_Name.text = "Seoul Namsan\r\nGukakdang";
                    break;
                case 4:
                    Detail_Name.text = "Carpenter Lee Seung-\r\neop' House";
                    break;
                case 5:
                    Detail_Name.text = "Yun Family's House in\r\nOgin-dong";
                    break;
                case 6:
                    Detail_Name.text = "General Kim Choon-\r\nyeong's House";
                    break;
                case 7:
                    Detail_Name.text = "Min Family's House in\r\nGwanhun-dong";
                    break;
                case 8:
                    Detail_Name.text = "Yun Taek-yeong's\r\nJaesil in Jegi-dong";
                    break;
                case 9:
                    Detail_Name.text = "Observatory";
                    break;
            }
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
        {
            switch (num)
            {
                case 0:
                    Detail_Name.text = "庭院";
                    break;
                case 1:
                    Detail_Name.text = "泉雨阁";
                    break;
                case 2:
                    Detail_Name.text = "青鹤池";
                    break;
                case 3:
                    Detail_Name.text = "国乐堂(户外庭院)";
                    break;
                case 4:
                    Detail_Name.text = "三角洞木匠李程业故居";
                    break;
                case 5:
                    Detail_Name.text = "玉仁洞尹氏故居";
                    break;
                case 6:
                    Detail_Name.text = "三清洞五卫将金春永故居";
                    break;
                case 7:
                    Detail_Name.text = "宽勋洞闵氏故居";
                    break;
                case 8:
                    Detail_Name.text = "祭基洞海丰府院君尹泽荣斋室";
                    break;
                case 9:
                    Detail_Name.text = "展望台";
                    break;
            }
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Japan)
        {
            switch (num)
            {
                case 0:
                    Detail_Name.text = "庭";
                    break;
                case 1:
                    Detail_Name.text = "天羽閣";
                    break;
                case 2:
                    Detail_Name.text = "青学紙";
                    break;
                case 3:
                    Detail_Name.text = "国楽堂(屋外の庭)";
                    break;
                case 4:
                    Detail_Name.text = "三角洞都片手イ·スンオプ家屋";
                    break;
                case 5:
                    Detail_Name.text = "指痛惜光阴虚度，思欲有所作为。";
                    break;
                case 6:
                    Detail_Name.text = "三清洞5位長・金春栄家屋";
                    break;
                case 7:
                    Detail_Name.text = "寛勲洞閔氏の家屋";
                    break;
                case 8:
                    Detail_Name.text = "祭基洞 海豊府院君ユン·テクヨン斎室";
                    break;
                case 9:
                    Detail_Name.text = "展望台";
                    break;
            }
        }
    }

    public void Close360()
    {
        Notice360.SetActive(false);
        SelectLabel = null;
        Narration.clip = null;

        see360.enabled = false;
        obj360.SetActive(false);

        Narration.Stop();
        if(scroll.ad!=null)
        {
            scroll.ad.Stop();
        }
        if (gamemanager.NaviRect.sizeDelta.x < GameManager.barOpen)
        {
            gamemanager.navi_t = 0;
            gamemanager.moveNavi = true;
            gamemanager.NaviOn = false;
        }

        Docent_AllUI.SetActive(false);
        AllUI.SetActive(true);
        scroll.CancelInvoke();
        Docent_Finish();        // 상세설명, 아바타 없어지기
    }

    public void Docent_OnOff()
    {
        if (!Docent_avartar.activeSelf)
        {
            OnOff_Btn.SetActive(false);
            Docent_Start();
        }
        else if (Docent_avartar.activeSelf)
        {
            OnOff_Btn.SetActive(true);
            Docent_Finish();
        }
        if(scroll.ChromaVideo.activeSelf)
        {
            scroll.ChromaVideo.GetComponent<MeshRenderer>().enabled = false;
            scroll.ChromaVideo.gameObject.SetActive(false);
            scroll.ScrollHome();
            Invoke("OnDocent_guideBtn", 3.5f);
        }
    }

    public void Docent_Start()
    {
        Docent_guide.SetActive(true);
        Docent_Detail.SetActive(true);
        labeldetail.enabled = true;

        Docent_avartar.SetActive(true);
        avatar.enabled = true;

        labeldetail.DetailOpen();
    }

    public void Docent_Finish()     // 상세설명 없어지기
    {
        Docent_guide.SetActive(false);
        labeldetail.CloseDetailWindow();

        avatar.enabled = false;
        Docent_avartar.SetActive(false);

        labeldetail.enabled = false;
        Docent_Detail.SetActive(false);

        Narration.Stop();
    }

    // 도슨트에서 선조들의 지혜선택하면 실행될 함수입니다.
    // 도슨트에서 선택한 가옥은 SelectLabel로 불러오시면 됩니다.
    // Close360에서도 선조들의 지혜가 꺼졌는지 확인하고 안꺼졌으면 끌 수 있게 해주세요.
    public void Wisdom()
    {
        // 네비게이션 창 비활성화(역사모드에서는 네비게이션창 사용하지 않음)

        if(!scroll.ChromaVideo.activeSelf)
        {
            if (gamemanager.NaviRect.sizeDelta.x > GameManager.barClose)
            {
                gamemanager.navi_t = 0;
                gamemanager.NaviOn = true;
                gamemanager.moveNavi = true;
            }

            string name = SelectLabel.name;
            name = name.Replace("House", "");
            float num = int.Parse(name);
            switch(num)
            {
                case 1:
                    scroll.ContentName = "메인광장";
                    break;
                case 2:
                    scroll.ContentName = "천우각";
                    break;
                case 3:
                    scroll.ContentName = "청학지";
                    break;
                case 4:
                    scroll.ContentName = "남산국악당";
                    break;
                case 5:
                    scroll.ContentName = "이승업가옥";
                    break;
                case 6:
                    scroll.ContentName = "윤씨가옥";
                    break;
                case 7:
                    scroll.ContentName = "김춘영가옥";
                    break;
                case 8:
                    scroll.ContentName = "민씨가옥";
                    break;
                case 9:
                    scroll.ContentName = "윤택영재실";
                    break;
                case 10:
                    scroll.ContentName = "전망대";
                    break;
            }

            scroll.ChromaVideo.SetActive(true);
            scroll.Intelligence();
            Narration.Stop();
        }
        else
        {
            scroll.Close.gameObject.SetActive(false);
            CloseWisdom();
        }
        Docent_guide.GetComponent<Button>().enabled= false;
    }

    public void CloseWisdom()
    {
        scroll.VideoPlay();
        Invoke("OnDocent_guideBtn", 3.5f);
    }

    void OnDocent_guideBtn()
    {
        Docent_guide.GetComponent<Button>().enabled = true;
    }

    public void NarrStopPlay()
    {
        if(!scroll.ChromaVideo.activeSelf)
        {
            Narration.clip = Docent_avartar.transform.GetComponent<DocentAni>().AdClip;
            if (PlayNarr == true)
            {
                Narration.Stop();
                Docent_avartar.transform.GetComponent<DocentAni>().CancelTalk();
                //Invoke("WaitStopPlay", 0.5f);
            }
            else if (PlayNarr == false)
            {
                Narration.Play();
                Docent_avartar.transform.GetComponent<DocentAni>().Talk();
            }
            Invoke("WaitStopPlay", 0.5f);
        }
    }

    public void WaitStopPlay()
    {
        if (PlayNarr == true)
        {
            //gamemanager.label.Narration.Stop();
            labeldetail.detailsoundbut.GetComponent<Image>().sprite = Narr_On;
            PlayNarr = false;
            gamemanager.WriteLog(LogSendServer.NormalLogCode.NamSanH_DetailSound, "NamSanH_Detail:SoundOff", GetType().ToString());
        }
        else if (PlayNarr == false)
        {
            //gamemanager.label.Narration.Play();
            labeldetail.detailsoundbut.GetComponent<Image>().sprite = Narr_Off;
            PlayNarr = true;
            gamemanager.WriteLog(LogSendServer.NormalLogCode.NamSanH_DetailSound, "NamSanH_Detail:SoundOn", GetType().ToString());
        }
    }

    public void ButtonClick()
    {
        gamemanager.ButtonClickSound();
    }

    public void OnClickMerryBtn()
    {
        gamemanager.OnClickMerry();
    }
}
