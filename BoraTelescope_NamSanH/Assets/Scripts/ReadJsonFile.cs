using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadJsonFile : MonoBehaviour
{
    public GameManager gamemanager;

    string Allstr;
    string PayAllstr;
    public static string RangePT;
    public static string[] allstr_json;

    string Allstr_rec;
    string GetText;
    public string[] GOTEXT_arr;

    public static List<string> DetailText_K = new List<string>();
    public static List<string> DetailText_E = new List<string>();
    //public static List<string> DetailText_C = new List<string>();
    //public static List<string> DetailText_J = new List<string>();

    public void Readfile()
    {
        if (File.Exists(Application.dataPath + ("/DetailText.json")))
        {
            Allstr_rec = File.ReadAllText(Application.dataPath + ("/DetailText.json"));

            if (Allstr_rec.Contains("}"))
            {
                GOTEXT_arr = Allstr_rec.Split('}');

                for (int index = -1; index < GOTEXT_arr.Length - 1; index++)
                {
                    if (index < GOTEXT_arr.Length - 2)
                    {
                        GOTEXT_arr[index + 1] = GOTEXT_arr[index + 1] + "}";
                    }
                    else if (index == GOTEXT_arr.Length - 2)
                    {
                        GetText = GOTEXT_arr[index];
                    }
                }
            }

            for (int index = 0; index < GOTEXT_arr.Length - 1; index++)
            {
                GOTEXT_arr[index] = GOTEXT_arr[index].Replace("\r\n", string.Empty);
                //GOTEXT_arr[index] = GOTEXT_arr[index].Replace(" ","");
                LabelText pantilt = JsonUtility.FromJson<LabelText>(GOTEXT_arr[index]);
                DetailText_K.Add(pantilt.Korean);
                DetailText_E.Add(pantilt.English);
            }
        }

        if (File.Exists(Application.dataPath + ("/XRModeLabelPosition.json")))
        {
            Allstr = File.ReadAllText(Application.dataPath + ("/XRModeLabelPosition.json"));

            gamemanager.WriteLog(LogSendServer.NormalLogCode.Load_ARModeLabelPosition, "Load_XRModeLabelPosition", GetType().ToString());

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
            }

            PanTiltRange pantilt = JsonUtility.FromJson<PanTiltRange>(RangePT);

            XRMode_Manager.MinPan = pantilt.Min_Pan;
            XRMode_Manager.MaxPan = pantilt.Max_Pan;
            XRMode_Manager.MinTilt = pantilt.Min_Tilt;
            XRMode_Manager.MaxTilt = pantilt.Max_Tilt;
            XRMode_Manager.TotalPan = pantilt.ChangeValue_x;
            XRMode_Manager.TotalTilt = pantilt.ChangeValue_y;
            GameManager.waitingTime = pantilt.WaitingTime;

            for (int index = 0; index < allstr_json.Length - 2; index++)
            {
                XRModeLabelPosition labelPosition = JsonUtility.FromJson<XRModeLabelPosition>(allstr_json[index]);

                for (int sindex = 0; sindex < gamemanager.label.Label_Position.Length; sindex++)
                {
                    if (sindex == index)
                    {
                        gamemanager.label.Label_Position[sindex] = new Vector3(labelPosition.Label_X * XRMode_Manager.TotalPan, labelPosition.Label_Y * XRMode_Manager.TotalTilt, 0);
                        gamemanager.label.Label_Scale[sindex] = new Vector3(labelPosition.LabelScale, labelPosition.LabelScale, labelPosition.LabelScale);
                    }
                }

                GameManager.startlabel_x = (uint)pantilt.StartPosition_x;
                GameManager.startlabel_y = (uint)pantilt.StartPosition_y;
            }
        }
        else
        {
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.UnLoad_ARModeLabelPosition, "UnLoad_ARModeLabelPosition", GetType().ToString());
            GameManager.AnyError = true;
            /// 에러로그만 보내고 정상작동할지 아니면 에러로그 보내고 ClearMode로 전환해서 할지
        }
    }
    /*
    public void CustomWaitingTime()
    {
        if (File.Exists(Application.dataPath + ("/ARModeLabelPosition_" + ContentsInfo.ContentsName + ".json")))
        {
            Allstr = File.ReadAllText(Application.dataPath + ("/ARModeLabelPosition_" + ContentsInfo.ContentsName + ".json"));

            gamemanager.WriteLog(LogSendServer.NormalLogCode.Load_ARModeLabelPosition, "Load_ARModeLabelPosition", GetType().ToString());

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
            }

            PanTiltRange pantilt = JsonUtility.FromJson<PanTiltRange>(RangePT);

            string Allpos = "";

            for (int index = 0; index < allstr_json.Length - 2; index++)
            {
                Allpos += allstr_json[index];
            }

            pantilt.WaitingTime = (int)GameManager.waitingTime;

            string newstr = JsonUtility.ToJson(pantilt);

            File.WriteAllText(Application.dataPath + ("/ARModeLabelPosition_" + ContentsInfo.ContentsName + ".json"), Allpos + newstr);
        }
    }*/
}
