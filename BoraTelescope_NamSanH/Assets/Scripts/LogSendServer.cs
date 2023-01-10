using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;
using System.Net;
using System.Linq;

public class LogData
{
    public string Timestamp;
    public string Type;
    public int LogCode;
    public string LogInformation;
    public string ScriptName;
    public string ContentsVersion;

    public LogData(string timestamp, string type, int codenum, string loginfo, string scriptname, string contentsversion)
    {
        Timestamp = timestamp;
        Type = type;
        LogCode = codenum;
        LogInformation = loginfo;
        ScriptName = scriptname;
        ContentsVersion = contentsversion;
    }
}

public class LogSendServer : ErrorLog
{
    // 일반로그 : Timestamp [Normal] (로그코드) 간단한 설명/스크립트 이름/콘텐츠 버전
    // 에러로그 : Timestamp [Error] (에러코드) 간단한 설명-원인/스크립트 이름/콘텐츠 버전

    public enum NormalLogCode
    {
        StartContents = 1001,
        EndContents = 1002,
        Connect_SystemControl = 1003,
        Connect_Pantilt = 1004,
        Connect_Camera = 1005,
        ChangeMode = 1006,
        Load_ARModeLabelPosition = 1007,
        Load_ResourceFile = 1008,
        Load_PaymentFile = 1009,
        ChangeLanguage = 1010,
        ClickHomeBtn = 1011,

        AR_StartArrow = 2001,
        AR_FinishArrow = 2002,
        AR_SelectLabel = 2003,
        AR_SelectNavi = 2004,
        AR_Detail = 2005,
        AR_DetailMore = 2006,
        AR_DetailSound = 2007,
        AR_DetailClose = 2008,
        AR_Navigation = 2009,
        AR_CategorySelect = 2010,
        AR_Filter = 2011,
        AR_Capture = 2012,
        AR_ImageUpload = 2013,
        AR_ImageListConfirm = 2014,
        AR_QRCode = 2015,
        AR_StartMinimap = 2016,
        AR_FinishMinimap = 2017,
        AR_Joystick = 2018,
        AR_DragStart = 2019,
        AR_DragFinish = 2020,
        AR_Zoom = 2021,
        AR_PinchZoom = 2022,

        NamSanH_StartArrow = 3001,
        NamSanH_FinishArrow = 3002,
        NamSanH_StartDrag = 3003,
        NamSanH_FinishDrag = 3004,
        NamSanH_StartMinimap = 3005,
        NamSanH_FinishMinimap = 3006,
        NamSanH_Zoom = 3007,
        NamSanH_PinchZoom = 3008,
        NamSanH_SelectLabel = 3009,
        NamSanH_SelectNavi = 3010,
        NamSanH_Detail = 3011,
        NamSanH_DetailMore = 3012,
        NamSanH_DetailSound = 3013,
        NamSanH_DetailClose = 3014,
        NamSanH_Navigation = 3015,
        NamSanH_CategorySelect = 3016,
        NamSanH_Filter = 3017,
        NamSanH_Capture = 3018,
        NamSanH_ImageUpload = 3019,
        NamSanH_ImageListConfirm = 3020,
        NamSanH_QRCode = 3021,
        NamSanH_Joystick = 3022,

        Live_StartArrow = 4001,
        LIve_FinishArrow = 4002,
        Live_DragStart = 4003,
        LIve_DragFinish = 4004,

        Etc_SelectLabel = 5001,
        Etc_Detail = 5002,
        Etc_DetailMore = 5003,
        Etc_DetailSound = 5004,
        Etc_DetailClose = 5005,
        Etc_PantiltOrigin = 5006,
        Etc_PantiltStartPosition = 5007,
        Etc_Chromakey = 5009,
        ETC_Movie360 = 5010,

        Record_Start = 6001,
        Record_End = 6002,
        Record_Upload = 6003,
        Record_Save = 6004
    }
    public NormalLogCode lognum;

    public enum ErrorLogCode
    {
        DisConnect_SystemControl = 1001,
        Fail_Connect_Pantilt = 1002,
        Fail_Connect_Camera = 1003,
        UnLoad_ARModeLabelPosition = 1004,
        UnLoad_ResourceFile = 1005,
        Fail_ChangeMode = 1006,
        Fail_ImageUpload = 1007,
        Fail_ImageListConfirm = 1008,
        Fail_EnterMode = 1009,
        Fail_EtcPantilt = 1010,
        Fail_RecordUpload = 1011,
        UnLoad_PaymentFile = 1012,

        UnityError = 2001,
        UnityException = 2002
    }
    public ErrorLogCode errornum;

    private string timestamp;
    private string logType;
    private int LogCode;
    public static string ContentsVersion;

    /// <summary>
    /// 로그전송
    /// </summary>
    /// <param name="lognum"></param>
    /// <param name="loginfo"></param>
    /// <param name="scriptname"></param>
    public void WriteLog(NormalLogCode lognum, string loginfo, string scriptname)
    {
        logType = "[NORMAL]";
        //ContentsVersion = Application.version;

        LogCode = (int)lognum;

        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        LogData logdata = new LogData(timestamp, logType, LogCode, loginfo, scriptname, ContentsVersion);
        //string str = JsonUtility.ToJson(logdata);
        //saveLog(str);
        //savestringLog(timestamp + "`^" + logType + "`^" + LogCode + "`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
        Send_Log_Button(timestamp + "`^" + logType + "`^" + LogCode + "`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
    }

    /// <summary>
    /// 오류로그 전송
    /// </summary>
    /// <param name="errornum"></param>
    /// <param name="loginfo"></param>
    /// <param name="scriptname"></param>
    public void WriteErrorLog(ErrorLogCode errornum, string loginfo, string scriptname)
    {
        logType = "[ERROR]";
        //ContentsVersion = Application.version;

        LogCode = (int)errornum;

        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        LogData logdata = new LogData(timestamp, logType, LogCode, loginfo, scriptname, ContentsVersion);
        //string str = JsonUtility.ToJson(logdata);
        //saveLog(str);
        //savestringLog(timestamp + "`^" + logType + "`^" + LogCode +"`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
        Send_Error_Button(timestamp + "`^" + logType + "`^" + LogCode + "`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
    }

    List<string> Log_json = new List<string>();
    List<string> Log_Text = new List<string>();
    string allLog;
    int filenum;

    /// <summary>
    /// 모든로그 로컬 파일에 저장
    /// </summary>
    /// <param name="str"></param>
    public void saveLog(string str)
    {
        allLog = "";
        Log_json.Add(str);

        for(int index =0; index < Log_json.Count; index++)
        {
            allLog += Log_json[index] + System.Environment.NewLine;
        }

        File.WriteAllText(Application.dataPath + ("/LogData_" + ContentsInfo.ContentsName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".json"), allLog);
        //Debug.Log(str);
    }

    /// <summary>
    /// 모든 로그 로컬 파일에 저장(Txt파일)
    /// </summary>
    /// <param name="str"></param>
    public void savestringLog(string str)
    {
        allLog = "";
        Log_Text.Add(str);

        for (int index = 0; index < Log_Text.Count; index++)
        {
            allLog += Log_Text[index] + System.Environment.NewLine;
        }

        File.WriteAllText(Application.dataPath + ("/LogData_" + ContentsInfo.ContentsName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"), allLog);
    }

    public bool IsInternetConnected()
    {
        const string NCSI_TEST_URL = "http://www.msftncsi.com/ncsi.txt";
        const string NCSI_TEST_RESULT = "Microsoft NCSI";
        const string NCSI_DNS = "dns.msftncsi.com";
        const string NCSI_DNS_IP_ADDRESS = "131.107.255.255";

        try
        {
            // Check NCSI test link
            var webClient = new WebClient();
            //string result = webClient.DownloadString(NCSI_TEST_URL);
            string result = new TimedWebClient { Timeout = 500 }.DownloadString(NCSI_TEST_URL);
            if (result != NCSI_TEST_RESULT)
            {
                return false;
            }

            // Check NCSI DNS IP
            var dnsHost = Dns.GetHostEntry(NCSI_DNS);
            if (dnsHost.AddressList.Count() < 0 || dnsHost.AddressList[0].ToString() != NCSI_DNS_IP_ADDRESS)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return false;
        }

        return true;
    }

    public class TimedWebClient : WebClient
    {
        // Timeout in milliseconds, default = 600,000 msec
        public int Timeout { get; set; }

        public TimedWebClient()
        {
            this.Timeout = 100;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest = base.GetWebRequest(address);
            objWebRequest.Timeout = this.Timeout;
            return objWebRequest;
        }
    }
}
