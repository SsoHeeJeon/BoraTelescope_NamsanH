#define PanTilt 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PanTiltControl_v2;
using System.IO;
using System.Net.Sockets;

public class Loading : MonoBehaviour
{
    public GameManager gamemanager;
    public static string nextScene;

    private Slider progressBar;
    float timer_motor;
    //public uint startlabel_x;
    //public uint startlabel_y;

    uint currentPan;
    uint currentTilt;

    bool setMotor = false;
    bool SetmotorFreq = false;
    bool noOrigin = false;

    bool firstAct = false;

    private void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();


        if (GameManager.PrevMode != "WaitingMode")
        {
            GameManager.Readpulse = false;
        }
        if (!firstAct)
        {
            //GameManager.startlabel_x = 0;
            //GameManager.startlabel_y = 0;
            currentPan = 0;
            currentTilt = 0;
            firstAct = true;
        }
        if (gamemanager.changewaiting.MotorOrigin == false)
        {
            PanTiltControl.OriginMidFlag = false;
            PanTiltControl.OriginEndFlag = false;
        }

        GameManager.SettingLabelPosition = false;

#if PanTilt
        PanTiltControl.Connect("COM14", 38400);

        if (PanTiltControl.IsConnected == false)
        {
            GameManager.AnyError = true;
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_Connect_Pantilt, "Fail_Connect_Pantilt", GetType().ToString());
        }
        else if (PanTiltControl.IsConnected == true)
        {
            sendLog_1 = true;
            gamemanager.WriteLog(LogSendServer.NormalLogCode.Connect_Pantilt, "Connect_Pantilt:On", GetType().ToString());
            if (GameManager.AnyError == false)
            {
                GameManager.AnyError = false;
            }
        }
#endif
        //Debug.Log("today " + GameManager.AnyError);
        ConnectCamera();
        print(GameManager.currentLang+"111");
    }

    public void waitPantilt()
    {
        LoadingScene();
    }

    private void Update()
    {
        if (GameManager.Readpulse == true)
        {
            currentPan = (uint)PanTiltControl.NowPanPulse;
            currentTilt = (uint)PanTiltControl.NowTiltPulse;
            //Debug.Log("today " + currentPan + " / " + currentTilt);
        }
        print(GameManager.currentLang + "111");
        print(00);
        //Debug.Log(currentPan + " / " + currentPan);
        gamemanager.UISetting();
        print(GameManager.currentLang + "111");
        print(11);
        if (setMotor == true)
        {
            truetrue();
        }
        print(GameManager.currentLang + "111");
        print(22);
        //Debug.Log(GameManager.PrevMode);
        if (GameManager.PrevMode == "WaitingMode")
        {
            if (SetmotorFreq == true)
            {
                SetStartPoint_NoOrigin();
                //SetmotorFreq = false;
            }
        }
        print(GameManager.currentLang + "111");
        print(33);
    }

    float count;
    float counts;
    bool countstart = false;
    bool sendLog_1 = false;
    bool sendLog_2 = false;

    public void truetrue()
    {
        //Debug.Log("today  " + PanTiltControl.OriginMidFlag + " / " + PanTiltControl.OriginEndFlag + (Mathf.Abs(currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(currentTilt - GameManager.startlabel_y) < 1));
        if (PanTiltControl.OriginMidFlag == true)
        {
            if (sendLog_1 == true)
            {
                gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltOrigin, "PantiltOrigin : Sensor", GetType().ToString());
                sendLog_1 = false;
                sendLog_2 = true;
            }
            if (PanTiltControl.OriginEndFlag == false)
            {
                if (countstart == false)
                {
                    count = 0;
                    countstart = true;
                }
                count += Time.deltaTime;
                if ((int)count >= 60)
                {
                    count = 0;
                    countstart = false;
                    GameManager.AnyError = true;
                    gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanEndSensor)", GetType().ToString());
                    MoveNamSanHMode_Change();
                }
            }
            else if (PanTiltControl.OriginEndFlag == true)
            {
                if (sendLog_2 == true)
                {
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltOrigin, "PantiltOrigin : Finish", GetType().ToString());
                    sendLog_2 = false;
                }

                if (Mathf.Abs(currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(currentTilt - GameManager.startlabel_y) < 1)
                {
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltStartPosition, "Finish : (" + currentPan + " / " + currentTilt + ")", GetType().ToString());
                    counts = 0;
                    progressBar.value = 1;
                    setMotor = false;
                    GameManager.Readpulse = false;
                    gamemanager.changewaiting.enabled = true;
                    WaitMotor();
                }
                else
                {
                    counts += Time.deltaTime;
                    if ((int)counts >= 60)
                    {
                        GameManager.AnyError = true;
                        gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanEndSensor)", GetType().ToString());
                        MoveNamSanHMode_Change();
                    }

                    if (nextScene == "XRMode" || nextScene == "LiveMode")
                    {
                        if (SetmotorFreq == true)
                        {
                            Invoke("setpantiltFreq", 0.1f);
                            SetmotorFreq = false;
                        }
                    }
                    else
                    {
                        //gamemanager.telescopeMode();
                    }
                }
            }
        }
        else if (PanTiltControl.OriginMidFlag == false)
        {
            count += Time.deltaTime;
            if ((int)count >= 60)
            {
                GameManager.AnyError = true;
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanEndSensor)", GetType().ToString());
                MoveNamSanHMode_Change();
            }
            countstart = false;
        }
    }

    public void setpantiltFreq()
    {
        print("fast");
        PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);

        Invoke("setpantiltpulse", 0.1f);
    }
    public void setpantiltpulse()
    {
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltStartPosition, "StartPoint", GetType().ToString());

        float tiltYposition = GameManager.startlabel_y;
        PanTiltControl.SetPulse(GameManager.startlabel_x, (uint)tiltYposition);

        if (GameManager.PrevMode == "WaitingMode")
        {
            gamemanager.changewaiting.currentPan = 0;
            gamemanager.changewaiting.currentTilt = 0;
        }
        GameManager.Readpulse = true;
    }

    public void LoadingScene()
    {
        switch (nextScene)
        {
            case "Loading":
                if (GameManager.AnyError == false)
                {
                    if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == false)
                    {
                        gamemanager.ErrorMessage.gameObject.SetActive(true);
                        MoveEtcMode();
                    }
                    else if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == true && GameManager.ModeActive[2] == false)
                    {
                        MoveLoading();
                    }
                    else if (GameManager.ModeActive[0] == true && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == false)
                    {
                        gamemanager.WantNoLabel = true;
                        MoveLoading();
                    }
                    else if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == true)
                    {
                        //남산골로연결
                        MoveNamSanHMode_Change();
                    }
                    else if (GameManager.ModeActive[0] == true && GameManager.ModeActive[1] == true && GameManager.ModeActive[2] == false)
                    {
                        MoveLoading();
                    }
                    else if (GameManager.ModeActive[0] == true && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == true)
                    {
                        gamemanager.WantNoLabel = true;
                        MoveLoading();
                    }
                    else if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == true && GameManager.ModeActive[2] == true)
                    {
                        MoveLoading();
                    }
                    else if (GameManager.ModeActive[0] == true && GameManager.ModeActive[1] == true && GameManager.ModeActive[2] == true)
                    {
                        MoveLoading();
                    }
                }
                else if (GameManager.AnyError == true)
                {
                    if(GameManager.ModeActive[2] == true)
                    {
                        MoveNamSanHMode_Change();
                    } else if(GameManager.ModeActive[2] == false)
                    {
                        gamemanager.ErrorMessage.gameObject.SetActive(true);
                        MoveEtcMode();
                    }
                }
                break;
            case "LiveMode":
                if (GameManager.AnyError == false)
                {
                    if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == false)
                    {
                        gamemanager.ErrorMessage.gameObject.SetActive(true);
                        MoveEtcMode();
                    }
                    else if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == true && GameManager.ModeActive[2] == false)
                    {
                    }
                    else if (GameManager.ModeActive[0] == true && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == false)
                    {
                        gamemanager.WantNoLabel = true;
                        MoveLiveMode();
                    }
                    else if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == true)
                    {
                        //남산골로연결
                    }
                    else if (GameManager.ModeActive[0] == true && GameManager.ModeActive[1] == true && GameManager.ModeActive[2] == false)
                    {
                        gamemanager.WantNoLabel = true;
                        MoveLiveMode();
                    }
                    else if (GameManager.ModeActive[0] == true && GameManager.ModeActive[1] == false && GameManager.ModeActive[2] == true)
                    {
                        gamemanager.WantNoLabel = true;
                        MoveLiveMode();
                    }
                    else if (GameManager.ModeActive[0] == false && GameManager.ModeActive[1] == true && GameManager.ModeActive[2] == true)
                    {
                        MoveLoading();
                    }
                }
                else if (GameManager.AnyError == true)
                {
                    if (GameManager.ModeActive[2] == true)
                    {
                        MoveNamSanHMode_Change();
                    }
                    else if (GameManager.ModeActive[2] == false)
                    {
                        gamemanager.ErrorMessage.gameObject.SetActive(true);
                        MoveEtcMode();
                    }
                }
                gamemanager.WantNoLabel = true;
                nextScene = "XRMode";
                break;
            case "XRMode":
                if (GameManager.ModeActive[1] == true)
                {
                    if (GameManager.AnyError == false)
                    {
                        setMotor = false;
                        SetmotorFreq = true;
                        if (GameManager.PrevMode != "WaitingMode")
                        {
                            StartCoroutine(LoadScene());
                        }
                    }
                    else if (GameManager.AnyError == true)
                    {
                        gamemanager.ErrorMessage.gameObject.SetActive(true);
                        MoveNamSanHMode_Change();
                    }
                    //Invoke("WaitMotor", 10f);
                }
                else if (GameManager.ModeActive[1] == false)
                {
                    gamemanager.ErrorMessage.gameObject.SetActive(true);

                    if (GameManager.PrevMode == "LiveMode" && GameManager.ModeActive[0] == true)
                    {
                        if (GameManager.AnyError == false)
                        {
                            MoveLiveMode();
                        }
                        else if (GameManager.AnyError == true)
                        {
                            if (GameManager.ModeActive[2] == true)
                            {
                                MoveClearMode();
                            }
                            else if (GameManager.ModeActive[2] == false)
                            {
                                MoveEtcMode();
                            }
                        }
                    }
                    else if (GameManager.PrevMode == "LiveMode" && GameManager.ModeActive[0] == false)
                    {
                        if (GameManager.ModeActive[2] == true)
                        {
                            MoveClearMode();
                        }
                        else if (GameManager.ModeActive[2] == false)
                        {
                            MoveEtcMode();
                        }
                    }
                    else if (GameManager.PrevMode == "NamSanHMode" && GameManager.ModeActive[2] == true)
                    {
                        MoveClearMode();
                    }
                    else if (GameManager.PrevMode == "NamSanHMode" && GameManager.ModeActive[2] == false)
                    {
                        if (GameManager.ModeActive[0] == true)
                        {
                            if (GameManager.AnyError == false)
                            {
                                MoveLiveMode();
                            }
                            else if (GameManager.AnyError == true)
                            {
                                MoveEtcMode();
                            }
                        }
                        else if (GameManager.ModeActive[0] == false)
                        {
                            MoveEtcMode();
                        }
                    }
                    else
                    {
                        MoveEtcMode();
                    }
                }
                break;
            case "NamSanHMode":
                if (GameManager.ModeActive[2] == true)
                {
                    setMotor = false;
                    StartCoroutine(LoadScene());
                }
                else if (GameManager.ModeActive[2] == false)
                {
                    gamemanager.ErrorMessage.gameObject.SetActive(true);

                    if (GameManager.PrevMode == "LiveMode" && GameManager.ModeActive[0] == true)
                    {
                        if (GameManager.AnyError == false)
                        {
                            MoveLiveMode();
                        }
                        else if (GameManager.AnyError == true)
                        {
                            MoveEtcMode();
                        }
                    }
                    else if (GameManager.PrevMode == "LiveMode" && GameManager.ModeActive[0] == false)
                    {
                        if (GameManager.ModeActive[1] == true)
                        {
                            if (GameManager.AnyError == false)
                            {
                                MoveARMode();
                            }
                            else if (GameManager.AnyError == true)
                            {
                                MoveEtcMode();
                            }
                        }
                        else if (GameManager.ModeActive[1] == false)
                        {
                            MoveEtcMode();
                        }
                    }
                    else if (GameManager.PrevMode == "XRMode" && GameManager.ModeActive[1] == true)
                    {
                        MoveARMode();
                    }
                    else if (GameManager.PrevMode == "XRMode" && GameManager.ModeActive[1] == false)
                    {
                        if (GameManager.ModeActive[0] == true)
                        {
                            MoveLiveMode();
                        }
                        else if (GameManager.ModeActive[0] == false)
                        {
                            MoveEtcMode();
                        }
                    }
                    else
                    {
                        MoveEtcMode();
                    }
                }
                break;
            case "WaitingMode":
                break;
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log(nextScene);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        if (op != null)
        {
            op.allowSceneActivation = false;

            float timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;

                timer += Time.deltaTime;

                if (op.progress >= 0.9f)
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);

                    if (progressBar.value == 1.0f)
                        op.allowSceneActivation = true;

                }
                else
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                    if (progressBar.value >= op.progress)
                    {
                        timer = 0f;
                    }
                }
            }
        }
        else if (op == null)
        {
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_ChangeMode, "Fail_ChangeMode:" + nextScene, GetType().ToString());
        }
    }

    public void WaitMotor()
    {
        if (setMotor == false)
        {
            timer_motor = 0;
            progressBar.value = 0;
            StartCoroutine(LoadScene());
        }
        else if (setMotor == true)
        {
            //WaitMotor();
        }
    }

    public void SetOriginLabel()
    {
        // gamemanager의 StartLabel을 조회해서
        // 라벨 모터 좌표 json 파일에서 해당 좌표를 조회
        GameManager.startlabel_x = 6000;
        GameManager.startlabel_y = 20;

        PanTiltControl.SetPulse((uint)GameManager.startlabel_x, (uint)GameManager.startlabel_y); // 모터를 해당 좌표로 이동
    }

    public void ReadFile()
    {
        if (File.Exists(Application.dataPath + ("/XRModeLabelPosition.json")))
        {
            //Allstr = File.ReadAllText(Application.dataPath + ("/ARModeLabelPosition_" + ContentsInfo.ContentsName + ".json"));

            gamemanager.WriteLog(LogSendServer.NormalLogCode.Load_ARModeLabelPosition, "Load_ARModeLabelPosition", GetType().ToString());
            /*
            if (Allstr.Contains("}"))
            {
                allstr_json = Allstr.Split('}');

                for (int index = -1; index < allstr_json.Length - 1; index++)
                {
                    if (index < allstr_json.Length - 2)
                    {
                        allstr_json[index + 1] = allstr_json[index + 1] + "}";
                    }
                    else if (index == allstr_json.Length - 2)
                    {
                        RangePT = allstr_json[index];
                    }
                }
            }*/

            ReadJsonFile readjson = GameObject.Find("GameManager").GetComponent<ReadJsonFile>();
            /*
            PanTiltRange pantilt = JsonUtility.FromJson<PanTiltRange>(ReadJsonFile.RangePT);

            for (int index = 0; index < ReadJsonFile.allstr_json.Length - 2; index++)
            {
                ARModeLabelPosition labelPosition = JsonUtility.FromJson<ARModeLabelPosition>(ReadJsonFile.allstr_json[index]);
            }*/
        }
        else
        {
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.UnLoad_ARModeLabelPosition, "UnLoad_ARModeLabelPosition", GetType().ToString());
            GameManager.AnyError = true;
            /// 에러로그만 보내고 정상작동할지 아니면 에러로그 보내고 ClearMode로 전환해서 할지
        }
    }

    NetworkStream stream;

    //tcp
    public const string serverIP = "127.0.0.1";
    public const int port = 8000;
    public TcpClient Client;

    public void ConnectCamera()
    {
        var processList = System.Diagnostics.Process.GetProcessesByName("XRTeleSpinCam");
        if (processList.Length == 0)
        {
            System.Diagnostics.Process.Start(@"C:\XRTelesPinCam\XRTeleSpinCam.exe");
        }

        Invoke("CheckClient", 1f);
        if (Client == null)
        {
            Client = new TcpClient(serverIP, port);
        }

    }

    public void CheckClient()
    {
        if (Client == null)
        {
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_Connect_Camera, "Fail_Connect_Camera", GetType().ToString());
            GameManager.AnyError = true;
            /*
            ReadFile();
            progressBar = GameObject.Find("Loading_UI").transform.GetChild(0).gameObject.GetComponent<Slider>();
            setMotor = false;
            timer_motor = 0;
            progressBar.value = 0;

            if (nextScene == null)
            {
                nextScene = "Loading";
                LoadingScene();
            }
            else if (nextScene != null)
            {
                LoadingScene();
            }*/
        }
        else if (Client != null)
        {
            GameManager.AnyError = false;
            gamemanager.WriteLog(LogSendServer.NormalLogCode.Connect_Camera, "Connect_Camera:On", GetType().ToString());
            Client.Close();
            gamemanager.WriteLog(LogSendServer.NormalLogCode.Connect_Camera, "Connect_Camera:Off", GetType().ToString());
        }
        ReadFile();
        progressBar = GameObject.Find("Loading_UI").transform.GetChild(0).gameObject.GetComponent<Slider>();
        setMotor = false;
        timer_motor = 0;
        progressBar.value = 0;

        if (nextScene == null)
        {
            nextScene = "Loading";
            LoadingScene();
        }
        else if (nextScene != null)
        {
            LoadingScene();
        }
    }

    public void SetStartPoint_Telescope()
    {

    }

    public void SetStartPoint_Origin()
    {
        Debug.Log("today  " + PanTiltControl.OriginMidFlag + " / " + PanTiltControl.OriginEndFlag + (Mathf.Abs(currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(currentTilt - GameManager.startlabel_y) < 1));
        if (PanTiltControl.OriginMidFlag == true)
        {
            if (sendLog_1 == true)
            {
                gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltOrigin, "PantiltOrigin : Sensor", GetType().ToString());
                sendLog_1 = false;
                sendLog_2 = true;
            }
            if (PanTiltControl.OriginEndFlag == false)
            {
                if (countstart == false)
                {
                    count = 0;
                    countstart = true;
                }
                count += Time.deltaTime;
                if ((int)count >= 60)
                {
                    count = 0;
                    countstart = false;
                    GameManager.AnyError = true;
                    gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanEndSensor)", GetType().ToString());
                    MoveNamSanHMode_Change();
                }
            }
            else if (PanTiltControl.OriginEndFlag == true)
            {
                if (sendLog_2 == true)
                {
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltOrigin, "PantiltOrigin : Finish", GetType().ToString());
                    sendLog_2 = false;
                }
                if (Mathf.Abs(currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(currentTilt - GameManager.startlabel_y) < 1)
                {
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltStartPosition, "Finish : (" + currentPan + " / " + currentTilt + ")", GetType().ToString());
                    counts = 0;
                    progressBar.value = 1;
                    setMotor = false;
                    WaitMotor();
                }
                else
                {
                    counts += Time.deltaTime;
                    if ((int)counts >= 60)
                    {
                        GameManager.AnyError = true;
                        gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanEndSensor)", GetType().ToString());
                        MoveNamSanHMode_Change();
                    }

                    if (nextScene == "XRMode")
                    {
                        if (SetmotorFreq == true)
                        {
                            Invoke("setpantiltFreq", 0.1f);
                            SetmotorFreq = false;
                        }
                    }
                }
            }
        }
        else if (PanTiltControl.OriginMidFlag == false)
        {
            count += Time.deltaTime;
            if ((int)count >= 60)
            {
                GameManager.AnyError = true;
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanEndSensor)", GetType().ToString());
                MoveNamSanHMode_Change();
            }
            countstart = false;
        }
    }

    bool SetmotorFreq_1 = false;

    public void SetStartPoint_NoOrigin()
    {
        //Debug.Log("today  " + gamemanager.changewaiting.currentPan + (Mathf.Abs(gamemanager.changewaiting.currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(gamemanager.changewaiting.currentTilt - GameManager.startlabel_y) < 1));
        if (Mathf.Abs(gamemanager.changewaiting.currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(gamemanager.changewaiting.currentTilt - GameManager.startlabel_y) < 1)
        {
            counts = 0;
            progressBar.value = 1;
            setMotor = false;
            SetmotorFreq = false;
            GameManager.Readpulse = false;
            WaitMotor();
        }
        else
        {
            counts += Time.deltaTime;
            if ((int)counts >= 60)
            {
                GameManager.AnyError = true;
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanEndSensor)", GetType().ToString());
                MoveClearMode();
            }
            if (GameManager.Readpulse == false && PanTiltControl.OriginEndFlag == true)
            {
                if (nextScene == "XRMode")
                {
                    if (SetmotorFreq_1 == false)
                    {
                        Invoke("setpantiltFreq", 0.1f);
                        SetmotorFreq_1 = true;
                    }
                }
                else
                {
                    //gamemanager.telescopeMode();
                }
            }
        }
    }

    public void MoveLoading()
    {
        setMotor = true;
        SetmotorFreq = true;
        PanTiltControl.Origin();
        nextScene = "XRMode";
    }
    
    public void MoveLoading_Basic()
    {
        if (GameManager.AnyError == false)
        {
            if (GameManager.ModeActive[0] == true)
            {
                setMotor = true;
                SetmotorFreq = true;
                noOrigin = false;
                PanTiltControl.Origin();
                nextScene = "TelescopeMode";
            }
            else if (GameManager.ModeActive[0] == false)
            {
                gamemanager.ErrorMessage.gameObject.SetActive(true);
                MoveEtcMode();
            }
        }
        else if (GameManager.AnyError == true)
        {
            gamemanager.ErrorMessage.gameObject.SetActive(true);
            MoveEtcMode();
        }
    }
    
    public void MoveLoading_Demo()
    {
        setMotor = false;
        nextScene = "NamSanHMode";
        StartCoroutine(LoadScene());
    }

    public void MoveTelescopeMode()
    {
        setMotor = false;
        SetmotorFreq = true;
        noOrigin = true;
        nextScene = "XRMode";
        StartCoroutine(LoadScene());
    }

    public void MoveLiveMode()
    {
        setMotor = false;
        SetmotorFreq = true;
        gamemanager.WantNoLabel = true;
        StartCoroutine(LoadScene());
    }
    public void MoveARMode()
    {
        gamemanager.WantNoLabel = false;
        setMotor = false;
        SetmotorFreq = true;
        //GameManager.Readpulse = true;
        nextScene = "XRMode";
        if (GameManager.PrevMode != "WaitingMode")
        {
            StartCoroutine(LoadScene());
        }
    }
    public void MoveClearMode()
    {
        setMotor = false;
        StartCoroutine(LoadScene());
    }
    public void MoveCultureMode()
    {
        setMotor = false;
        StartCoroutine(LoadScene());
    }
    public void MoveWaitingMode()
    {
        if (GameManager.AnyError == false)
        {
            if (GameManager.ModeActive[0] == true)
            {
                setMotor = true;
                SetmotorFreq = true;
                noOrigin = false;
                PanTiltControl.Origin();
                nextScene = "WaitingMode";
            }
            else if (GameManager.ModeActive[0] == false)
            {
                gamemanager.ErrorMessage.gameObject.SetActive(true);
                MoveEtcMode();
            }
        }
        else if (GameManager.AnyError == true)
        {
            gamemanager.ErrorMessage.gameObject.SetActive(true);
            MoveEtcMode();
        }
    }
    public void MoveEtcMode()
    {
        setMotor = false;
        nextScene = "WaitingMode";
        gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EnterMode, "ChangeMode : Change(" + GameManager.PrevMode + " - " + "WaitingMode)", GetType().ToString());
        GameManager.PrevMode = "WaitingMode";
        StartCoroutine(LoadScene());
    }

    public void MoveLiveMode_Change()
    {
        setMotor = false;
        SetmotorFreq = true;
        gamemanager.WantNoLabel = true;
        nextScene = "XRMode";
        gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EnterMode, "ChangeMode : Change(" + GameManager.PrevMode + " - " + "LiveMode)", GetType().ToString());
        GameManager.PrevMode = "LiveMode";
        StartCoroutine(LoadScene());
    }
    public void MoveARMode_Change()
    {
        gamemanager.WantNoLabel = false;
        setMotor = false;
        SetmotorFreq = true;
        nextScene = "XRMode";
        gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EnterMode, "ChangeMode : Change(" + GameManager.PrevMode + " - " + "ARMode)", GetType().ToString());
        GameManager.PrevMode = "XRMode";
        StartCoroutine(LoadScene());
    }
    public void MoveNamSanHMode_Change()
    {
        setMotor = false;
        nextScene = "NamSanHMode";
        gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EnterMode, "ChangeMode : Change(" + GameManager.PrevMode + " - " + "ClearMode)", GetType().ToString());
        GameManager.PrevMode = "NamSanHMode";
        StartCoroutine(LoadScene());
    }
}
