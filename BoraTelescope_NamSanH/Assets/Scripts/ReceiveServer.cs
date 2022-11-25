using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveServer : MonoBehaviour
{
    string[] modeonoffinfo;
    List<string> ModeOnOff = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //SceneOnOff("LiveFalse/ARFalse/ClearTrue/HeritageTrue");
    }

    public void SceneOnOff(string onoff)
    {
        ModeOnOff.Clear();
        modeonoffinfo = new string[onoff.Split('/').Length];
        Debug.Log(onoff);

        modeonoffinfo = onoff.Split('/');
        
        for (int index = 0; index < modeonoffinfo.Length; index++)
        {
            if (ContentsInfo.ContentsName == "Dora")
            {
                if (modeonoffinfo[index].Contains("Live") || modeonoffinfo[index].Contains("AR") || modeonoffinfo[index].Contains("Clear") || modeonoffinfo[index].Contains("Heritage"))
                {
                    ModeOnOff.Add(modeonoffinfo[index]);
                }else
                {

                }
            } else
            {
                if (modeonoffinfo[index].Contains("Live") || modeonoffinfo[index].Contains("AR") || modeonoffinfo[index].Contains("Clear"))
                {
                    ModeOnOff.Add(modeonoffinfo[index]);
                }
                else
                {

                }
            }
        }

        modeonoffinfo = ModeOnOff.ToArray();

        for (int index = 0; index < modeonoffinfo.Length; index++)
        {
            switch (ContentsInfo.ContentsName)
            {
                case "Demo":

                    break;
                case "Dora":
                    switch (modeonoffinfo[index])
                    {
                        case "LiveTrue":
                            GameManager.ModeActive[0] = true;
                            break;
                        case "LiveFalse":
                            GameManager.ModeActive[0] = false;
                            break;
                        case "ARTrue":
                            GameManager.ModeActive[1] = true;
                            break;
                        case "ARFalse":
                            GameManager.ModeActive[1] = false;
                            break;
                        case "ClearTrue":
                            GameManager.ModeActive[2] = true;
                            break;
                        case "ClearFalse":
                            GameManager.ModeActive[2] = false;
                            break;
                        case "HeritageTrue":
                            GameManager.ModeActive[3] = true;
                            break;
                        case "HeritageFalse":
                            GameManager.ModeActive[3] = false;
                            break;
                    }
                    break;
                case "Odu":
                    switch (modeonoffinfo[index])
                    {
                        case "LiveTrue":
                            GameManager.ModeActive[0] = true;
                            break;
                        case "LiveFalse":
                            GameManager.ModeActive[0] = false;
                            break;
                        case "ARTrue":
                            GameManager.ModeActive[1] = true;
                            break;
                        case "ARFalse":
                            GameManager.ModeActive[1] = false;
                            break;
                        case "ClearTrue":
                            GameManager.ModeActive[2] = true;
                            break;
                        case "ClearFalse":
                            GameManager.ModeActive[2] = false;
                            break;
                    }
                    break;
                case "BEXCO":
                    switch (modeonoffinfo[index])
                    {
                        case "LiveTrue":
                            GameManager.ModeActive[0] = true;
                            break;
                        case "LiveFalse":
                            GameManager.ModeActive[0] = false;
                            break;
                        case "ARTrue":
                            GameManager.ModeActive[1] = true;
                            break;
                        case "ARFalse":
                            GameManager.ModeActive[1] = false;
                            break;
                        case "ClearTrue":
                            GameManager.ModeActive[2] = true;
                            break;
                        case "ClearFalse":
                            GameManager.ModeActive[2] = false;
                            break;
                        case "HeritageTrue":
                            GameManager.ModeActive[3] = true;
                            break;
                        case "HeritageFalse":
                            GameManager.ModeActive[3] = false;
                            break;
                    }
                    break;
            }
        }
    }
}
