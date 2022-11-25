using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;
using System.Net;

public class bora_client_test : MonoBehaviour
{
    public ReceiveServer receiveserver;

    public Text Connect_Text; // 텍스트 클래스를 만들고 GameObject 에서 반드시 추가를 해주어야 한다.
    public Text mode_status_flag_string; // 모드 상태 플래그 값을 Unity에서 사용 가능하도록 변화하여 변수에 저장

    public Text Send_Log_Text;
    public Text Send_Error_Text;

    TcpClient client = null; // 서버 접속을 위한 client 소켓 생성
    StreamReader reader = null;
    StreamWriter writer = null;

    string dataToSend = null;  // 서버에 보낼 데이터를 저장하기 위한 변수 선언
    string dateFromReceive = null; // 서버로 부터 받은 데이터를 저장하기 위한 변수 선언

    public static bool FailControlSystem = false;

    // Start is called before the first frame update
    void Start() // 처음 시작할 때만 실행
    {
        // 모드 상태 플래그 값 가져오기 // 
        //Connect_Button();
    }

    // Update is called once per frame
    void Update() // 무한루프로 계속 실행
    {

    }



    public void Connect_Button()
    {
        // 모드 상태 플래그 값을 받기 위한 서버 만들기 //
        Thread Server_For_Mode_Status_Flag = new Thread(Make_Server_For_Mode_Status_Flag);
        Server_For_Mode_Status_Flag.Start();

        // 서버 접속 하기 // 
        client = new TcpClient();

        try
        {
            client.Connect("localhost", 5003); // 시스템 컨트롤러 로컬 보라 서버 주소

            NetworkStream stream = client.GetStream();  // 서버 접속 시작

            Encoding encode = Encoding.GetEncoding("utf-8");

            reader = new StreamReader(stream, encode); // 데이터 값을 읽기 위해서

            writer = new StreamWriter(stream, encode) // 값을 보내기 위해서 
            { AutoFlush = true }; // 버퍼가 남아있을 시 데이터가 안보내질수 있어서?

            // mode status flag 값 가져오기
            dataToSend = "GIVE_ME_MODE_STATUS_FLAG_AND_ClIENT_VER_ID";

            try
            {
                writer.WriteLine(dataToSend); // 서버로 입력한 데이터 전송
                dateFromReceive = reader.ReadLine(); // 서버에서 받은 데이터를 string 변수에 저장
                var splitMsg = dateFromReceive.Split(',');
                // string client_ver_id = (splitMsg[0]);
                // 모드 상태 플래그 값 저장 //
                //mode_status_flag_string.text = dateFromReceive;
                ContentsInfo.ContentsVersion = splitMsg[0];
                receiveserver.SceneOnOff(splitMsg[1]);
            }
            catch (Exception ex) // 서버로 송신이 안될 때 에러 발생
            {
                Debug.Log(ex.ToString());
                Debug.Log("서버로 전송 실패");
                Debug.Log("서버 연결 불가능");
            }

            Debug.Log("보라 로컬 서버 연결 완료");

        }
        catch (Exception ex) // 서버에 접속이 안될 때 에러 발생
        {
            Debug.Log(ex.ToString());
            Debug.Log("서버 연결 불가능");

        }

    }

    /// <summary>
    ///  Control System 접속 종료
    /// </summary>
    public void Disconnect_Button()
    {
        dataToSend = "접속 종료"; // 접속 끊기

        try
        {
            writer.WriteLine(dataToSend); // 서버로 입력한 데이터 전송

        }
        catch (Exception ex) // 서버로 송신이 안될 때 에러 발생
        {
            Debug.Log(ex.ToString());
            Debug.Log("서버로 전송 실패");
            Debug.Log("서버 연결 불가능");
        }
        finally
        {
            client.Close(); // 보라 클라이언트 종료
            Debug.Log("보라 클라이언트 종료");

            //Connect_Text.text = "접속 종료 완료";
            Debug.Log("접속 종료 완료");
        }

    }


    public void Send_Log_Button(string imp_log)
    {
        //imp_log = "1001"; // 임시 테스트 값
        //dataToSend = "LOG, " + imp_log;

        try
        {
            writer.WriteLine(imp_log); // 서버로 입력한 데이터 전송
            //writer.WriteLine(dataToSend); // 서버로 입력한 데이터 전송

            //Debug.Log(imp_log);
            //Send_Log_Text.text = imp_log;
        }
        catch (Exception ex) // 서버로 송신이 안될 때 에러 발생
        {
            Debug.Log(ex.ToString());
            Debug.Log("서버로 전송 실패");
            Debug.Log("서버 연결 불가능");
        }

    }

    public void Send_Error_Button(string imp_error)
    {
        //imp_error = "2001"; // 임시 테스트 값
        //string imp_ouccr_time = "2022-02-03 16:48:25.224"; // 임시 테스트 값

        //dataToSend = "ERROR, " + imp_error + ", " + imp_ouccr_time; // 

        try
        {
            writer.WriteLine(imp_error); // 서버로 입력한 데이터 전송
            //writer.WriteLine(dataToSend); // 서버로 입력한 데이터 전송

            Debug.Log(imp_error);
            //Send_Error_Text.text = imp_error;

        }
        catch (Exception ex) // 서버로 송신이 안될 때 에러 발생
        {
            Debug.Log(ex.ToString());
            Debug.Log("서버로 전송 실패");
            Debug.Log("서버 연결 불가능");
        }
    }

    public void Make_Server_For_Mode_Status_Flag()
    {
        TcpListener tcpListener = null; // 수신 전용 소켓을 만들기 위해서 선언

        Socket clientsocket = null; // 클라이언트에 대한 전용 소켓을 만들기 위해서 선언

        NetworkStream stream = null; // stream 생성을 위해 선언

        StreamReader reader = null; // 수신한 데이터를 읽기 위해 선언

        StreamWriter writer = null; // 송신할 데이터를 쓰기 위해 선언

        IPAddress ipAd = IPAddress.Parse("127.0.0.1"); // 서버주소 : 로컬호스트 IP주소 : 127.0.0.1

        tcpListener = new TcpListener(ipAd, 5004); // 시스템 컨트롤러가 모드 상태 플래그를 변경이 필요할 때 5004 포트로 접속한다. 

        tcpListener.Start(); // 수신 상태 시작 -> 즉 서버 열림

       

        string strMsg = null; // 수신된 문자를 받기 위한 변수 선언


        while(true)
        {
            clientsocket = tcpListener.AcceptSocket(); // 클라이언트 접속 허락

            Debug.Log("시스템 컨트롤러가 모드 상태 플래그 값 변경으로 인해 접속");

            stream = new NetworkStream(clientsocket); // 입력한 소켓을 통해 stream 생성, stream에 소켓을 통해서 주고 받은 데이터가 저장 되어 있다.

            Encoding encode = Encoding.GetEncoding("utf-8"); // 한글을 인식하기 위한 enode 선언

            reader = new StreamReader(stream, encode); // 수신 받은 데이터 읽기

            writer = new StreamWriter(stream, encode) { AutoFlush = true }; // 송신할 데이터 쓰기


            while (true)
            {
                try
                {
                    strMsg = reader.ReadLine(); // 수신 대기 상태, 받은 데이터 str 변수에 저장

                    // 모드 상태 플래그 값 수신
                    if (strMsg.Contains("MODE_STATUS_FLAG"))  // 명령어 : MODE_STATUS_FLAG,[mode_status_flag]
                    {

                        var splitStr = strMsg.Split(','); // strMSG 내용에서 , 기준으로 값을 배열에 저장

                        // 모드 상태 플래그 값 저장 //
                        //mode_status_flag_string.text = splitStr[1];
                        Debug.Log(splitStr[1]);
                        receiveserver.SceneOnOff(splitStr[1]);
                        writer.WriteLine("모드 상태 플래그 값 수신 완료"); // 시스템 컨트롤러 클라이언트에게 수신 완료 확인 보내기
                        clientsocket.Close(); //클라이언트 접속이 끊키면 소켓을 닫는다.
                        Debug.Log("보라 프로그램 수신 대기 서버 정상 종료");
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    Debug.Log("비정상 종료");
                    clientsocket.Close(); //클라이언트 접속이 끊키면 소켓을 닫는다.
                    break;
                }

            }
        }
    }
    /*
    public string Convert_Mode_Status_Flag_Sentence(string mode_status_flag) // 모드가 추가 될 때 마다 이 함수를 수정해야 한다.
    {
        string result = null;
        //int number_mode = mode_status_flag.Length; // 총 모드 개수

        // 라이브 모드
        if (mode_status_flag.Substring(0, 1) == "1")
        {
            result += "LiveTrue";
        }
        else
        {
            result += "LiveFalse";
        }
        result += "/";

        // AR 모드
        if (mode_status_flag.Substring(1, 1) == "1")
        {
            result += "ARTrue";
        }
        else
        {
            result += "ARFalse";
        }
        result += "/";

        // Clear 모드
        if (mode_status_flag.Substring(2, 1) == "1")
        {
            result += "ClearTrue";
        }
        else
        {
            result += "ClearFalse";
        }
        result += "/";

        // 자동 촬영 모드
        if (mode_status_flag.Substring(3, 1) == "1")
        {
            result += "AutoPhotoTrue";
        }
        else
        {
            result += "AutoPhotoFalse";
        }
        result += "/";

        // 광고 모드
        if (mode_status_flag.Substring(4, 1) == "1")
        {
            result += "AdvTrue";
        }
        else
        {
            result += "AdvFalse";
        }
        result += "/";

        // 가이드 모드
        if (mode_status_flag.Substring(5, 1) == "1")
        {
            result += "GuideTrue";
        }
        else
        {
            result += "GuideFalse";
        }



        return result;

    }

    */

    /*
    public void ClickButton_1() // 적용할려면 유니티에서 해당 버튼 클릭하여 속성창에서 On Click() 속성 부분에 GameObject 추가하고 함수 넣어주면 된다.
    {
        ButObj_1.text = "Click";
        Debug.Log("Click"); // C# 에서 Console.Writeline() 과 똑같다.
    }

    public void ClickButton_2()
    {
        ButObj_2.text = "hohohoho";

    }
    */


}
