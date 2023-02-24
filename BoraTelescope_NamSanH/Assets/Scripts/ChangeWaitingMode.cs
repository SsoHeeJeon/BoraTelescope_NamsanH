using PanTiltControl_v2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWaitingMode : MonoBehaviour
{
    public GameManager gamemanager;

    public uint currentPan;
    public uint currentTilt;

    public bool MotorOrigin = false;
    bool SetmotorFreq = false;
    float count;
    float counts;
    bool countstart = false;
    bool sendLog_1 = false;
    bool sendLog_2 = false;

    // Start is called before the first frame update
    void Start()
    {
        currentPan = 0;
        currentTilt = 0;
        if (PanTiltControl.OriginEndFlag == false)
        {
            GameManager.Readpulse = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("today " + GameManager.AnyError + " / " + GameManager.Readpulse);
        if (GameManager.AnyError == false)
        {
            if (GameManager.Readpulse == true)
            {
                currentPan = (uint)PanTiltControl.NowPanPulse;
                currentTilt = (uint)PanTiltControl.NowTiltPulse;
                //Debug.Log("today " + currentPan + " / " + currentTilt);
            }
            if (MotorOrigin == true)
            {
                //Debug.Log("today MotorOrigin " + MotorOrigin);
                CheckOriginState();
            }
        }
    }

    /// <summary>
    /// 확장형에서 팬틸트 초기화
    /// </summary>
    public void SetPantiltOrigin()
    {
        //Debug.Log("today SetPantiltOrigin start ");
        PanTiltControl.OriginMidFlag = false;
        PanTiltControl.OriginEndFlag = false;

        MotorOrigin = true;
        SetmotorFreq = true;
        PanTiltControl.Origin();

        sendLog_1 = true;
        //sendLog_2 = true;
        currentPan = 0;
        currentTilt = 0;
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltOrigin, "PantiltOrigin : Start", GetType().ToString());
        //Debug.Log("today SetPantiltOrigin finish");
    }

    /// <summary>
    /// 초기지점으로 이동
    /// </summary>
    public void CheckOriginState()
    {
        //Debug.Log("today " + PanTiltControl.OriginMidFlag + " / " + PanTiltControl.OriginEndFlag);
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
                }
            }
            else if (PanTiltControl.OriginEndFlag == true)
            {
                if (sendLog_2 == true)
                {
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltOrigin, "PantiltOrigin : Finish", GetType().ToString());
                    sendLog_2 = false;
                }

                //Debug.Log("today " + currentPan + " / " + GameManager.startlabel_x + (Mathf.Abs(currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(currentTilt - GameManager.startlabel_y) < 1));
                if (Mathf.Abs(currentPan - GameManager.startlabel_x) < 1 && Mathf.Abs(currentTilt - GameManager.startlabel_y) < 1)
                {
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltStartPosition, "Finish : (" + currentPan + " / " + currentTilt + ")", GetType().ToString());
                    counts = 0;
                    MotorOrigin = false;
                    GameManager.Readpulse = false;
                }
                else
                {
                    counts += Time.deltaTime;
                    if ((int)counts >= 60)
                    {
                        counts = 0;
                        GameManager.AnyError = true;
                        SetmotorFreq = true;
                        //팬틸트 모터가 계속 동작중 중단시킬 함수 필요
                        gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(StartPosition)", GetType().ToString());
                    }
                    //Debug.Log("today today");
                    if (SetmotorFreq == true)
                    {
                        Debug.Log("today today");
                        Invoke("setpantiltFreq", 0.5f);
                        SetmotorFreq = false;
                    }
                }
            }
        }
        else if (PanTiltControl.OriginMidFlag == false)
        {
            count += Time.deltaTime;
            if ((int)count >= 60)
            {
                counts = 0;
                GameManager.AnyError = true;
                //팬틸트 모터가 계속 동작중 중단시킬 함수 필요
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_EtcPantilt, "PantiltOrigin : Error(PanMinSensor)", GetType().ToString());
            }
            countstart = false;
        }
    }

    public void setpantiltFreq()
    {
        PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);
        gamemanager.speed_enum = GameManager.Speed_enum.fast;
        //Debug.Log("today today");
        Invoke("setpantiltpulse", 0.5f);
    }
    public void setpantiltpulse()
    {
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Etc_PantiltStartPosition, "Start", GetType().ToString());

        float tiltYposition = GameManager.startlabel_y;
        PanTiltControl.SetPulse(GameManager.startlabel_x, (uint)tiltYposition);

        GameManager.Readpulse = true;
    }
}
