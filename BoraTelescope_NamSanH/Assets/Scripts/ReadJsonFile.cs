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
    public static List<string> DetailText_C = new List<string>();
    public static List<string> DetailText_J = new List<string>();

    public void Readfile()
    {
        if (ContentsInfo.ContentsName == "Normal" || ContentsInfo.ContentsName == "OceanCafe")
        {
            if (File.Exists(Application.dataPath + ("/BoraTelescope_" + ContentsInfo.ContentsName + ".json")))
            {
                Allstr = File.ReadAllText(Application.dataPath + ("/BoraTelescope_" + ContentsInfo.ContentsName + ".json"));

                gamemanager.WriteLog(LogSendServer.NormalLogCode.Load_ARModeLabelPosition, "Load_ARModeLabelPosition", GetType().ToString());
                /*
                GameManager.startlabel_x = uint.Parse(Allstr.Substring(Allstr.IndexOf('(') + 1, Allstr.IndexOf(',') - 2));
                GameManager.startlabel_y = uint.Parse(Allstr.Substring(Allstr.IndexOf(',') + 1, (Allstr.IndexOf('/') - 1) - Allstr.IndexOf(',')));
                TelescopeMode.Minpan = float.Parse(Allstr.Substring(Allstr.IndexOf('/') + 1, (Allstr.IndexOf(';') - 1) - Allstr.IndexOf('/')));
                TelescopeMode.Maxpan = float.Parse(Allstr.Substring(Allstr.IndexOf(';') + 1, (Allstr.IndexOf(':') - 1) - Allstr.IndexOf(';')));
                TelescopeMode.Mintilt = float.Parse(Allstr.Substring(Allstr.IndexOf(':') + 1, (Allstr.IndexOf('*') - 1) - Allstr.IndexOf(':')));
                TelescopeMode.Maxtilt = float.Parse(Allstr.Substring(Allstr.IndexOf('*') + 1, (Allstr.IndexOf(')') - 1) - Allstr.IndexOf('*')));
                Debug.Log(GameManager.startlabel_x);
                Debug.Log(GameManager.startlabel_y);
                Debug.Log(TelescopeMode.Minpan);
                Debug.Log(TelescopeMode.Maxpan);
                Debug.Log(TelescopeMode.Mintilt);
                Debug.Log(TelescopeMode.Maxtilt);*/
            }
            else
            {
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.UnLoad_ARModeLabelPosition, "UnLoad_ARModeLabelPosition", GetType().ToString());
                GameManager.AnyError = true;
                /// 에러로그만 보내고 정상작동할지 아니면 에러로그 보내고 ClearMode로 전환해서 할지
            }
        }
        else
        {
            if (!(ContentsInfo.ContentsName == "Demo" || ContentsInfo.ContentsName == "Dora"))
            //if (!(ContentsInfo.ContentsName == "Dora"))
            {
                Allstr_rec = File.ReadAllText(Application.dataPath + ("/DetailText_" + ContentsInfo.ContentsName + ".json"));

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
            /*
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

                ARMode_Manager.MinPan = pantilt.Min_Pan;
                ARMode_Manager.MaxPan = pantilt.Max_Pan;
                ARMode_Manager.MinTilt = pantilt.Min_Tilt;
                ARMode_Manager.MaxTilt = pantilt.Max_Tilt;
                ARMode_Manager.TotalPan = pantilt.ChangeValue_x;
                ARMode_Manager.TotalTilt = pantilt.ChangeValue_y;
                GameManager.waitingTime = pantilt.WaitingTime;

                for (int index = 0; index < allstr_json.Length - 2; index++)
                {
                    ARModeLabelPosition labelPosition = JsonUtility.FromJson<ARModeLabelPosition>(allstr_json[index]);

                    for (int sindex = 0; sindex < gamemanager.label.Label_Position.Length; sindex++)
                    {
                        if (sindex == index)
                        {
                            gamemanager.label.Label_Position[sindex] = new Vector3(labelPosition.Label_X * ARMode_Manager.TotalPan, labelPosition.Label_Y * ARMode_Manager.TotalTilt, 0);
                        }
                    }

                    if (pantilt.StartPosition != "")
                    {
                        if (pantilt.StartPosition == labelPosition.LabelName)
                        {
                            float startYposition = labelPosition.Label_Y;

                            GameManager.startlabel_x = (uint)labelPosition.Label_X;
                            GameManager.startlabel_y = (uint)startYposition;
                        }
                    }
                    else if (pantilt.StartPosition == "")
                    {
                        switch (ContentsInfo.ContentsName)
                        {
                            case "Demo":
                                if (labelPosition.LabelName == "Park")
                                {
                                    float startYposition = labelPosition.Label_Y;

                                    GameManager.startlabel_x = (uint)labelPosition.Label_X;
                                    GameManager.startlabel_y = (uint)startYposition;
                                }
                                break;
                            case "Dora":
                                if (labelPosition.LabelName == "Factory")
                                {
                                    float startYposition = labelPosition.Label_Y;

                                    GameManager.startlabel_x = (uint)labelPosition.Label_X;
                                    GameManager.startlabel_y = (uint)startYposition;
                                }
                                break;
                            case "Odu":
                                if (labelPosition.LabelName == "School")
                                {
                                    float startYposition = labelPosition.Label_Y;

                                    GameManager.startlabel_x = (uint)labelPosition.Label_X;
                                    GameManager.startlabel_y = (uint)startYposition;
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.UnLoad_ARModeLabelPosition, "UnLoad_ARModeLabelPosition", GetType().ToString());
                GameManager.AnyError = true;
                /// 에러로그만 보내고 정상작동할지 아니면 에러로그 보내고 ClearMode로 전환해서 할지
            }*/
        }
    }

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
    }
}
