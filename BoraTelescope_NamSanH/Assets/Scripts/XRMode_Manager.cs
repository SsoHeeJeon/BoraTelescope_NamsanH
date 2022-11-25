using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using PanTiltControl_v2;
using System;

public class XRMode_Manager : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject ManagerMode;
    public GameObject CameraWindow;

    public GameObject AllLabel;

    string LabelName;
    float Label_x;
    float Label_y;

    string Allstr;
    string RangePT;
    string[] allstr_json;

    public List<string> AllLabelPosition;
    public string[] AllLabelPosition_arr;


    public GameObject CurrentlabelPosition;
    public Slider ChangeValuePos;
    public GameObject ChangeValuePos_t;
    public GameObject SelectLabel;
    public GameObject SelectLabel_btn;

    float Current_x;
    float Current_y;

    public static float MinPan;
    public static float MaxPan;
    public static float MinTilt;
    public static float MaxTilt;
    public static string StartPosition;
    public static float TotalPan;
    public static float TotalTilt;

    public GameObject PTRange;

    public bool AllchangeLabel = false;
    public bool setlabelcamera = false;

    public InputField SetValueX;
    public InputField SetValueY;
    public GameObject Totalpan_Text;
    public GameObject Totaltilt_Text;

    float SetValueX_min;
    float SetValueX_max;
    float SetValueY_min;
    float SetValueY_max;

    public Slider PanFreq_value;
    public Text PanFreq_text;
    bool setPanFreq = false;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //ManagerMode = GameObject.Find("ManagerMode");
        gamemanager.UISetting();

        AllchangeLabel = false;
        ReadFile();
        PTRange.GetComponent<Text>().text = "(" + MinPan + "~" + MaxPan + ", " + MinTilt + "~" + MaxTilt + ")";
        XRMode.ValueX = TotalPan;
        XRMode.ValueY = TotalTilt;
        
        if (GameManager.SettingLabelPosition == true)
        {
            gamemanager.xrMode.enabled = false;
            gamemanager.xrMode_manager.enabled = true;
            ManagerMode.gameObject.SetActive(true);
            for (int index = 0; index < AllLabel.transform.childCount; index++)
            {
                AllLabel.transform.GetChild(index).gameObject.GetComponent<Image>().fillAmount = 1;
            }
        }
        else if (GameManager.SettingLabelPosition == false)
        {
            gamemanager.xrMode.enabled = true;
            ManagerMode.gameObject.SetActive(false);
            gamemanager.xrMode_manager.enabled = false;
        }
        //SettingLabel();
    }

    // Update is called once per frame
    void Update()
    {
        Current_x = PanTiltControl.NowPanPulse;
        Current_y = PanTiltControl.NowTiltPulse;
        CameraWindow.transform.localPosition = new Vector3(Current_x * TotalPan, Current_y * TotalTilt, CameraWindow.transform.localPosition.z);

        ChangeValuePos_t.GetComponent<Text>().text = ((int)(ChangeValuePos.value * 200)).ToString();
        PanFreq_text.text = ((int)(PanFreq_value.value * 20)).ToString();

        if (SelectLabel == null)
        {
            CurrentlabelPosition.GetComponent<Text>().text = "(" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")";
        } else if(SelectLabel != null)
        {
            if (setlabelcamera == true)
            {
                CurrentlabelPosition.GetComponent<Text>().text = "(" + SelectLabel.transform.localPosition.x/TotalPan + ", " + SelectLabel.transform.localPosition.y/TotalTilt + ")";
            }else if(setlabelcamera == false)
            {
                CurrentlabelPosition.GetComponent<Text>().text = "(" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")";
                if(Mathf.Abs(SelectLabel.transform.localPosition.x/TotalPan - Current_x)<1 && Mathf.Abs(SelectLabel.transform.localPosition.y / TotalTilt - Current_y)<1)
                {
                    setlabelcamera = true;
                }
            }
        }

        if (GameManager.MoveCamera == true)
        {
            MoveCamera_Arrow();
            if (setlabelcamera == true && SelectLabel != null && setPanFreq == false)
            {
                SelectLabel.transform.localPosition = new Vector3(CameraWindow.transform.localPosition.x, CameraWindow.transform.localPosition.y, 0);
            }
        }
    }

    public void SettingLabel()
    {
        for(int index = 0; index < AllLabel.transform.childCount; index++)
        {
            LabelName = AllLabel.transform.GetChild(index).transform.gameObject.name;
            Label_x = AllLabel.transform.GetChild(index).transform.localPosition.x / TotalPan;
            Label_y = AllLabel.transform.GetChild(index).transform.localPosition.y / TotalTilt;
            
            SaveLabelPosition(LabelName, Label_x, Label_y);
        }
    }

    public void SaveLabelPosition(string labelname, float label_x, float label_y)
    {
        XRModeLabelPosition labelposition = new XRModeLabelPosition(labelname, label_x, label_y);
        //StartPosition = gamemanager.StartLabel;

        PanTiltRange pantilt = new PanTiltRange(MinPan, MaxPan, MinTilt, MaxTilt, StartPosition,TotalPan, TotalTilt, (int)GameManager.waitingTime);

        string str = JsonUtility.ToJson(labelposition);
        string str_1 = JsonUtility.ToJson(pantilt);

        AllLabelPosition_arr = new string[AllLabel.transform.childCount];

        AllLabelPosition.Add(str);
        AllLabelPosition_arr = AllLabelPosition.ToArray();

        Allstr = "";

        if(AllLabelPosition.Count == AllLabel.transform.childCount)
        {
            for(int index = 0; index<AllLabel.transform.childCount; index++)
            {
                Allstr += AllLabelPosition_arr[index].ToString();
            }

            File.WriteAllText(Application.dataPath + ("/XRModeLabelPosition_" + ContentsInfo.ContentsName + ".json"), Allstr + str_1);
        }
        GameObject.Find("GameManager").GetComponent<ReadJsonFile>().Readfile();
    }

    public void ReadFile()
    {
        /*
        Allstr = File.ReadAllText(Application.dataPath + ("/XRModeLabelPosition_"+ContentsInfo.ContentsName+".json"));

        if (Allstr.Contains("}"))
        {
            allstr_json = Allstr.Split('}');

            for(int index = -1; index<allstr_json.Length-1; index++)
            {
                if (index < allstr_json.Length - 2)
                {
                    allstr_json[index + 1] = allstr_json[index + 1] + "}";
                } else if(index == allstr_json.Length - 2)
                {
                    RangePT = allstr_json[index];
                }
            }
        }
        */
        PanTiltRange pantilt = JsonUtility.FromJson<PanTiltRange>(ReadJsonFile.RangePT);
        MinPan = pantilt.Min_Pan;
        MaxPan = pantilt.Max_Pan;
        MinTilt = pantilt.Min_Tilt;
        MaxTilt = pantilt.Max_Tilt;
        TotalPan = pantilt.ChangeValue_x;
        TotalTilt = pantilt.ChangeValue_y;
        for (int index = 0; index< ReadJsonFile.allstr_json.Length - 2; index++)
        {
            XRModeLabelPosition labelPosition = JsonUtility.FromJson<XRModeLabelPosition>(ReadJsonFile.allstr_json[index]);

            if(AllLabel.transform.GetChild(index).gameObject.name == labelPosition.LabelName)
            {
                AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(labelPosition.Label_X * TotalPan, labelPosition.Label_Y * TotalTilt, 0);
            }
        }
    }

    // 라벨 버튼이나 네비게이션의 라벨을 눌렀을때 라벨의 위치로 팬틸트이동
    public void ChangePositionLabel(GameObject label)
    {
        if (setPanFreq == false)
        {
            SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = label.name;

            SelectLabel_btn.GetComponent<Image>().color = new Color(0.8632076f, 0.9884475f, 1f);
            SelectLabel_btn.transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);

            GameObject navilabel = gamemanager.NavigationBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            for (int index = 0; index < navilabel.transform.childCount; index++)
            {
                navilabel.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }

            for (int index = 0; index < ReadJsonFile.allstr_json.Length - 2; index++)
            {
                if (AllLabel.transform.GetChild(index).gameObject.name == label.name)
                {
                    SelectLabel = AllLabel.transform.GetChild(index).gameObject;
                    SelectLabel.transform.localPosition = AllLabel.transform.GetChild(index).gameObject.transform.localPosition;
                }
            }

            if (SelectLabel.transform.localPosition.x != PanTiltControl.NowPanPulse / TotalPan || SelectLabel.transform.localPosition.y != PanTiltControl.NowTiltPulse / TotalTilt)
            {
                float xpulse = SelectLabel.transform.localPosition.x / TotalPan;
                float ypulse = SelectLabel.transform.localPosition.y / TotalTilt;

                PanTiltControl.SetPulse((uint)xpulse, (uint)ypulse);
            }
        } else if(setPanFreq == true)
        {
            GameObject navilabel = gamemanager.NavigationBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            for (int index = 0; index < navilabel.transform.childCount; index++)
            {
                navilabel.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }

            for (int index = 0; index < ReadJsonFile.allstr_json.Length - 2; index++)
            {
                if (AllLabel.transform.GetChild(index).gameObject.name == label.name)
                {
                    SelectLabel = AllLabel.transform.GetChild(index).gameObject;
                    SelectLabel.transform.localPosition = AllLabel.transform.GetChild(index).gameObject.transform.localPosition;

                }
            }


            if (Mathf.Abs(Current_x - (SelectLabel.transform.localPosition.x / TotalPan)) <= 2000 && XRMode.PanFreq != XRMode.panFreq_Near)
            {
                XRMode.PanFreq = XRMode.panFreq_Near;
                PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Slow);
                gamemanager.speed_enum = GameManager.Speed_enum.slow;
                //PanTiltControl.SetFreq(PanTiltControl.Motor.Tilt, 12);
                Debug.Log(XRMode.PanFreq);
            }
            else if (Mathf.Abs(Current_x - (SelectLabel.transform.localPosition.x / TotalPan)) > 2000 && XRMode.PanFreq != XRMode.panFreq_Far)
            {
                XRMode.PanFreq = XRMode.panFreq_Far;
                PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);
                gamemanager.speed_enum = GameManager.Speed_enum.fast;
                //PanTiltControl.SetFreq(PanTiltControl.Motor.Tilt, 6);
                Debug.Log(XRMode.PanFreq);
            }
            Invoke("waitPanMove", 0.1f);
        }
    }

    public void waitPanMove()
    {
        if (SelectLabel.transform.localPosition.x != PanTiltControl.NowPanPulse / TotalPan || SelectLabel.transform.localPosition.y != PanTiltControl.NowTiltPulse / TotalTilt)
        {
            float xpulse = SelectLabel.transform.localPosition.x / TotalPan;
            float ypulse = SelectLabel.transform.localPosition.y / TotalTilt;

            PanTiltControl.SetPulse((uint)xpulse, (uint)ypulse);
        }
    }

    // 화살표 선택하면 해당 방향으로 팬틸트와 카메라 움직이기
    // 카메라 중앙에 라벨이 있다면 라벨도 동시에 움직이기
    public void MoveCamera_Arrow()
    {
        switch (gamemanager.MoveDir)
        {
            case "Left":
                if (Current_x >= MinPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.LEFT);
                }
                break;
            case "Right":
                if (Current_x <= MaxPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.RIGHT);
                }
                break;
            case "Up":
                //PanTiltControl.SetFreq(PanTiltControl.Motor.Tilt, 10);
                if (Current_y < MaxTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.UP);
                }
                else
                {
                    PanTiltControl.Stop();
                }
                break;
            case "Down":
                if (Current_y > MinTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.DOWN);
                } else
                {
                    PanTiltControl.Stop();
                }
                break;
        }
    }

    public void AllLabelChange(GameObject btn)
    {
        if (btn.name == "All")
        {
            btn.GetComponent<Image>().color = new Color(0.8632076f, 0.9884475f,1f);
            btn.transform.parent.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            SelectLabel = null;
            AllchangeLabel = true;
        } else if(btn.name == "One")
        {
            btn.GetComponent<Image>().color = new Color(0.8632076f, 0.9884475f, 1f);
            btn.transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            AllchangeLabel = false;
            if(SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text != "LabelName")
            {
                for(int index = 0; index<AllLabel.transform.childCount; index++)
                {
                    if(AllLabel.transform.GetChild(index).gameObject.name == SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text)
                    {
                        SelectLabel = AllLabel.transform.GetChild(index).gameObject;
                    }
                }
            } else if(SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text == "LabelName")
            {
                SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "LabelName";
                SelectLabel = null;
            }
        }
    }

    public void ChangePositionPulse(GameObject btn)
    {
        float changepos =  float.Parse(ChangeValuePos_t.GetComponent<Text>().text);
        switch (btn.name)
        {
            case "XPlus":
                if(AllchangeLabel == true)
                {
                    for(int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x + changepos;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                } else if(AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x + changepos;
                        float lab_y = SelectLabel.transform.localPosition.y;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
            case "XMinus":
                if (AllchangeLabel == true)
                {
                    for (int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x - changepos;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                else if (AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x - changepos;
                        float lab_y = SelectLabel.transform.localPosition.y;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
            case "YPlus":
                if (AllchangeLabel == true)
                {
                    for (int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y + changepos;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                else if (AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x;
                        float lab_y = SelectLabel.transform.localPosition.y + changepos;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
            case "YMinus":
                if (AllchangeLabel == true)
                {
                    for (int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y - changepos;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                else if (AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x;
                        float lab_y = SelectLabel.transform.localPosition.y - changepos;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
        }
    }

    public void ConfirmPantiltRange(GameObject btn)
    {
        switch (btn.name)
        {
            case "PantiltSetting":
                MinPan = 0;
                MaxPan = 23600;
                MinTilt = 0;
                MaxTilt = 1500;
                break;
            case "MinX":
                MinPan = Current_x;
                Debug.Log("MinPan" + MinPan);
                break;
            case "MaxX":
                MaxPan = Current_x;
                Debug.Log("MaxPan" + MaxPan);
                break;
            case "MinY":
                MinTilt = Current_y;
                Debug.Log("MinTilt" + MinTilt);
                break;
            case "MaxY":
                MaxTilt = Current_y;
                Debug.Log("MaxTilt" + MaxTilt);
                break;
        }
        PTRange.GetComponent<Text>().text = "(" + MinPan + "~" + MaxPan + ", " + MinTilt + "~" + MaxTilt + ")";
    }

    public void SetFinish()
    {
        if (AllchangeLabel == false)
        {
            if (SelectLabel != null)
            {
                for(int index = 0; index<AllLabel.transform.childCount; index++)
                {
                    if(AllLabel.transform.GetChild(index).gameObject.name == SelectLabel.name)
                    {
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = SelectLabel.transform.localPosition;
                    }
                }
                SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "LabelName";
                SelectLabel = null;
            }
        } else if(AllchangeLabel == true)
        {
            SelectLabel = null;
            SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "LabelName";
            AllchangeLabel = false;
        }
    }

    public void SetValue(GameObject btn)
    {
        switch (btn.name)
        {
            case "ValueSetting":
                TotalPan = 1;
                TotalTilt = 1;
                System.Diagnostics.Process ps = new System.Diagnostics.Process();
                ps.StartInfo.FileName = "osk.exe";
                ps.Start();
                break;
            case "Move":
                float pulseX = int.Parse(SetValueX.text);
                float pulseY = int.Parse(SetValueY.text);

                PanTiltControl.SetPulse((uint)pulseX, (uint)pulseY);
                break;
            case "MinPan":
                SetValueX_min = float.Parse(PanTiltControl.NowPanPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowPanPulse.ToString();

                if(btn.transform.parent.gameObject.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MaxPan")
                {
                    Totalpan_Text.GetComponent<Text>().text = Math.Round((1920 / Mathf.Abs(SetValueX_max - SetValueX_min)),3).ToString();
                }
                break;
            case "MaxPan":
                SetValueX_max = float.Parse(PanTiltControl.NowPanPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowPanPulse.ToString();

                if (btn.transform.parent.gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MinPan")
                {
                    Totalpan_Text.GetComponent<Text>().text = Math.Round((1920 / Mathf.Abs(SetValueX_max - SetValueX_min)),3).ToString();
                }
                break;
            case "MinTilt":
                SetValueY_min = float.Parse(PanTiltControl.NowTiltPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowTiltPulse.ToString();

                if (btn.transform.parent.gameObject.transform.GetChild(7).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MaxTilt")
                {
                    Totaltilt_Text.GetComponent<Text>().text = Math.Round((1080 / Mathf.Abs(SetValueY_max - SetValueY_min)), 3).ToString();
                }
                break;
            case "MaxTilt":
                SetValueY_max = float.Parse(PanTiltControl.NowTiltPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowTiltPulse.ToString();

                if (btn.transform.parent.gameObject.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MinTilt")
                {
                    Totaltilt_Text.GetComponent<Text>().text = Math.Round((1080 / Mathf.Abs(SetValueY_max - SetValueY_min)),3).ToString();
                }
                break;
            case "Finish":
                TotalPan = (float)Math.Round((1920 / Mathf.Abs(SetValueX_max - SetValueX_min)), 3);
                TotalTilt = (float)Math.Round((1080 / Mathf.Abs(SetValueY_max - SetValueY_min)), 3);

                btn.transform.parent.gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MinPan";
                btn.transform.parent.gameObject.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MaxPan";
                btn.transform.parent.gameObject.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MinTilt";
                btn.transform.parent.gameObject.transform.GetChild(7).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MaxTilt";
                break;
        }
    }

    public void SetPanFreq(GameObject btn)
    {
        setPanFreq = true;
        switch (btn.name)
        {
            case "PanFreq":
                setPanFreq = true;
                break;
            case "Arrow":
                PanFreq_value.value = (float)XRMode.panFreq_ARR/20;
                PanFreq_text.text = (XRMode.panFreq_ARR).ToString();
                Debug.Log((XRMode.panFreq_ARR).ToString());
                break;
            case "Near":
                PanFreq_value.value = (float)XRMode.panFreq_Near/20;
                PanFreq_text.text = (XRMode.panFreq_Near).ToString();
                Debug.Log((XRMode.panFreq_ARR).ToString());
                break;
            case "Far":
                PanFreq_value.value = (float)XRMode.panFreq_Far/20;
                PanFreq_text.text = (XRMode.panFreq_Far).ToString();
                Debug.Log((XRMode.panFreq_ARR).ToString());
                break;
            case "Arrow_set":
                XRMode.panFreq_ARR = int.Parse(PanFreq_text.text);
                break;
            case "Near_set":
                XRMode.panFreq_Near = int.Parse(PanFreq_text.text);
                break;
            case "Far_set":
                XRMode.panFreq_Far = int.Parse(PanFreq_text.text);
                break;
            case "Finish":
                break;
        }
    }
}
