using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using PanTiltControl_v2;

public class WaitingMode : MonoBehaviour
{
    public GameManager gamemanager;
    public VideoPlayer BackGround_Video;
    private int videonum;
    private bool SeeVideo = false;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.speed_enum = GameManager.Speed_enum.fast;
        PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);
        gamemanager.UISetting();
        gamemanager.alreadywaitingLog = false;
        gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "ChangeMode : Finish(" + GameManager.PrevMode + " - " + "WaitingMode)", GetType().ToString());
        GameManager.PrevMode = "WaitingMode";
        /*
        if(gamemanager.WaitingVideo_path.Length == 0)
        {
            PanTiltControl.Connect("COM14", 38400);

            //bool motorConnect = PanTiltControl.Connect("COM14", 38400);
            if (PanTiltControl.IsConnected == false)
            {
                GameManager.AnyError = true;
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_Connect_Pantilt, "Fail_Connect_Pantilt", GetType().ToString());
                //gamemanager.ErrorMessage.gameObject.SetActive(true);
            }
            else if (PanTiltControl.IsConnected == true)
            {
                gamemanager.WriteLog(LogSendServer.NormalLogCode.Connect_Pantilt, "Connect_Pantilt:On", GetType().ToString());
                if (GameManager.AnyError == false)
                {
                    GameManager.AnyError = false;
                }
            }
            //ConnectCamera();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (BackGround_Video.isPlaying == false && SeeVideo == false)
        {
            LoadViedo();
            SeeVideo = true;
        }

        //Debug.Log(gamemanager.UI_All.transform.GetChild(2).gameObject.transform.position);
    }

    public void AnyErrorTime()
    {
        if (!gamemanager.UI_All.gameObject.activeSelf)
        {
            gamemanager.UISetting();
        }

        if (GameManager.AnyError == true && !gamemanager.ErrorMessage.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gamemanager.MenuBar.gameObject.SetActive(true);
                for(int index = 0; index<gamemanager.MenuBar.transform.childCount; index++)
                {
                    gamemanager.MenuBar.transform.GetChild(index).gameObject.SetActive(false);
                }
                //gamemanager.ErrorMessage.gameObject.SetActive(true);
            }
        }
    }

    public void OutWaitingMode()
    {
        if (gamemanager.MenuBar.gameObject.GetComponent<Image>().enabled == false)
        {
            gamemanager.MenuBar.gameObject.GetComponent<Image>().enabled = true;
            for (int index = 0; index < gamemanager.MenuBar.gameObject.transform.childCount; index++)
            {
                gamemanager.MenuBar.gameObject.transform.GetChild(index).gameObject.SetActive(true);
            }
            gamemanager.MenuBar.gameObject.transform.GetChild(5).gameObject.SetActive(false);
        }

        gamemanager.MenuBar.gameObject.SetActive(false);

        //BackGround_Video.clip = null;
        BackGround_Video.url = "";
        BackGround_Video.Stop();

        if (gamemanager.NaviRect.sizeDelta.x < 472f)
        {
            gamemanager.navi_t = 0;
            gamemanager.moveNavi = true;
            gamemanager.NaviOn = false;
        }
        gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "ChangeMode : Start(" + GameManager.PrevMode + " - " + "XRMode)", GetType().ToString());
        // DB에서 받은 콘텐츠 정보를 이용해서 어디에 설치되어있는지 확인하여
        // 해당 ClearMode에 연결
        if (GameManager.ModeActive[1])
        {
            Loading.nextScene = "XRMode";
            gamemanager.WantNoLabel = false;
        }
        else if (GameManager.ModeActive[0])
        {
            Loading.nextScene = "XRMode";
            gamemanager.WatingtoLive();
        }
        else if (GameManager.ModeActive[2])
        {
            Loading.nextScene = "NamSanHMode";
            gamemanager.WantNoLabel = false;
        }
        SceneManager.LoadScene("Loading");
    }

    public void PlayButtonClick()
    {
        gamemanager.ButtonClickSound();
    }
    public void LoadViedo()
    {
        if (BackGround_Video.isPlaying == false && gamemanager.WaitingVideo_path.Length != 0)
        {
            BackGround_Video.source = VideoSource.Url;
            if (BackGround_Video.url == "")
            {
                videonum = 0;
            }
            else if (BackGround_Video.url != "")
            {
                videonum += 1;
            }
        }
        PlayVideo();
    }

    public void PlayVideo()
    {
        BackGround_Video.Stop();

        if (videonum < gamemanager.WaitingVideo_path.Length)
        {
            BackGround_Video.url = gamemanager.WaitingVideo_path[videonum];
            BackGround_Video.Play();
        }
        //else if (videonum == gamemanager.WaitingVideo.Length)
        else if (videonum == gamemanager.WaitingVideo_path.Length)
        {
            videonum = 0;
            BackGround_Video.url = gamemanager.WaitingVideo_path[videonum];
            BackGround_Video.Play();
        }
        Invoke("waitVideoTime", 2f);
    }

    public void waitVideoTime() {
        SeeVideo = false;
    }

}