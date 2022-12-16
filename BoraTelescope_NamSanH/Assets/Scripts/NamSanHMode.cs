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
    
    public void NaviLabel(GameObject Label)
    {
        if (gamemanager.Label_Cate_1.Contains(Label.name))      // �ô뺰��õ
        {
            if (obj360.activeSelf)
            {
                Close360();
            }

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
        } else if (gamemanager.Label_Cate_2.Contains(Label.name))       // ����Ʈ
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
        print(1);
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            print(2);
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_K[num + 4];
            Narration.clip = gamemanager.Narration_Docent_K[num - 1];
            Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
            //labeldetail.Detail_Background.GetComponent<Image>().sprite = gamemanager.DetailImage_K[num - 1];
        } else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_E[num + 4];
            Narration.clip = gamemanager.Narration_Docent_E[num - 1];
            Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
            //labeldetail.Detail_Background.GetComponent<Image>().sprite = gamemanager.DetailImage_E[num - 1];
        }
        //Narration.Play();
        Narration.Pause();

        if (gamemanager.NaviRect.sizeDelta.x > GameManager.barClose)
        {
            gamemanager.navi_t = 0;
            gamemanager.NaviOn = true;
            gamemanager.moveNavi = true;
        }

        AllUI.SetActive(false);

        labeldetail.ChangeDetailLanguage();
        Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().AniHi();
    }

    public void Detailname(int num)
    {
        if(GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            switch (num)
            {
                case 0:
                    Detail_Name.text = "����";
                    break;
                case 1:
                    Detail_Name.text = "õ�찢";
                    break;
                case 2:
                    Detail_Name.text = "û����";
                    break;
                case 3:
                    Detail_Name.text = "���ﳲ�걹�Ǵ�";
                    break;
                case 4:
                    Detail_Name.text = "�ﰢ�� �����\r\n�̽¾� ����";
                    break;
                case 5:
                    Detail_Name.text = "���ε� ���� ����";
                    break;
                case 6:
                    Detail_Name.text = "��û�� ������\r\n���῵ ����";
                    break;
                case 7:
                    Detail_Name.text = "���Ƶ� �ξ� ����";
                    break;
                case 8:
                    Detail_Name.text = "���⵿ ��ǳ�ο���\r\n���ÿ� ���";
                    break;
                case 9:
                    Detail_Name.text = "������";
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
        Docent_Finish();        // �󼼼���, �ƹ�Ÿ ��������
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

    public void Docent_Finish()     // �󼼼��� ��������
    {
        Docent_guide.SetActive(false);
        labeldetail.CloseDetailWindow();

        avatar.enabled = false;
        Docent_avartar.SetActive(false);

        labeldetail.enabled = false;
        Docent_Detail.SetActive(false);

        Narration.Stop();
    }

    // ����Ʈ���� �������� ���������ϸ� ����� �Լ��Դϴ�.
    // ����Ʈ���� ������ ������ SelectLabel�� �ҷ����ø� �˴ϴ�.
    // Close360������ �������� ������ �������� Ȯ���ϰ� �Ȳ������� �� �� �ְ� ���ּ���.
    public void Wisdom()
    {
        // �׺���̼� â ��Ȱ��ȭ(�����忡���� �׺���̼�â ������� ����)
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
                scroll.ContentName = "���α���";
                break;
            case 2:
                scroll.ContentName = "õ�찢";
                break;
            case 3:
                scroll.ContentName = "û����";
                break;
            case 4:
                scroll.ContentName = "���걹�Ǵ�";
                break;
            case 5:
                scroll.ContentName = "�̽¾�����";
                break;
            case 6:
                scroll.ContentName = "��������";
                break;
            case 7:
                scroll.ContentName = "���῵����";
                break;
            case 8:
                scroll.ContentName = "�ξ�����";
                break;
            case 9:
                scroll.ContentName = "���ÿ����";
                break;
            case 10:
                scroll.ContentName = "������";
                break;
        }

        scroll.TitleText.transform.parent.parent.gameObject.SetActive(true);
        scroll.Intelligence();

        Narration.Stop();

    }

    public void NarrStopPlay()
    {
        if (PlayNarr == true)
        {
            Narration.Stop();
            Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().CancelTalk();
            //Invoke("WaitStopPlay", 0.5f);
        }
        else if (PlayNarr == false)
        {
            Narration.Play();
            Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().Talk();
        }
        Invoke("WaitStopPlay", 0.5f);
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
}
