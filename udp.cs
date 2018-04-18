using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using static System.Console;

namespace UDPTOOL
{
   
    //public struct UDP_DATA
    //{
    //    public int type;				//数据类型	(  0: 数据  10:状态  20:异常  )
    //    public char[] szOrderNo;		//订单编号  
    //    public char[] szData;		//详细数据 
    //}
    public enum TRANSTYPE
    {
        TRANS_NONE = 0,				//初始状态
        TRANS_REQUEST_ORDER = 1,				//机器人请求订单数据					( ROBOT -> RC2 )
        TRANS_ORDER_DATA = 2,				//RC2将订单数据传输给机器人				( RC2 -> ROBOT )
        TRANS_ORDER_DATA_RET = 3,				//机器人返回收到订单数据的确认信息		( ROBOT -> RC2 )

        TRANS_GAME_DATA = 10,				//机器人返回订单的游戏数据				( ROBOT -> RC2 ) 
        TRANS_GAME_DATA_RET = 11,				//RC2确认收到机器人的游戏数据			( RC2 -> ROBOT )

        TRANS_GOODS_STAUTS = 12,				//机器人返回执行的物流状态				( ROBOT -> RC2 )
        TRANS_GOODS_STATUS_RET = 13,				//告诉机器人收到 TRANS_GOODS_STAUTS 消息( RC2 -> ROBOT )

        TRANS_ORDER_NEW_LOG = 18,				//机器人告诉RC2处理日志 [此记录用于主站数据统计]
        TRANS_ORDER_NEW_LOG_RET = 19,				//RC2告诉机器人收到日志 [此记录用于主站数据统计]

        TRANS_REQ_IDCODE_RESULT = 30,				//机器人请求GTR处理验证码               ( ROBOT -> RC2 ) 
        TRANS_RES_IDCODE_RESULT = 31,				//发送处理完的验证码的到机器人程序      ( RC2 -> ROBOT ) 
        TRANS_IDCODE_INPUT_RESULT = 32,				//机器人输入验证码后的结果发送给客户端  ( ROBOT -> RC2 )

        TRANS_AGREE_SENDMAIL = 35,				//申请发送邮件
        TRANS_SEND_MAIL = 36,				//同意发送邮件

        TRANS_INSERT_ORDER = 40,				//申请插入订单
        TRANS_AGREE_INSERT = 41,				//同意插入订单

        TRANS_ORDER_END = 50,				//订单处理完成，正常移交。				( ROBOT -> RC2 )
        TRANS_ORDER_END_RET = 51,				//告诉机器人收到 TRANS_ORDER_END 消息	( RC2 -> ROBOT )

        TRANS_ORDER_CANCEL = 52,				//申请撤单								( ROBOT -> RC2 )
        TRANS_ORDER_CANCEL_RET = 53,				//告诉机器人收到 TRANS_ORDER_CANCEL 消息( RC2 -> ROBOT )

        TRANS_ORDER_ABOLISH = 54,				//取消订单								( ROBOT -> RC2 )
        TRANS_ORDER_ABOLISH_RET = 55,				//告诉机器人收到 TRANS_ORDER_ABOLISH 消息( RC2 -> ROBOT )

        TRANS_ORDER_OP = 71,				//转人工操作								( ROBOT -> RC2 )
        TRANS_ORDER_OP_RET = 81,				//告诉机器人收到 TRANS_ORDER_OP 消息	( RC2 -> ROBOT )

        TRANS_ORDER_OP_SUCESS = 82,				//转人工操作完成								( ROBOT -> RC2 )
        TRANS_ORDER_OP_SUCESS_RET = 72,				//告诉机器人收到 TRANS_ORDER_OP_SUCESS 消息	( RC2 -> ROBOT )

        TRANS_RC2_TO_UDPSVR = 101,				// RC2向UDPServer请求QQ令牌数据
        TRANS_UDPSVR_TO_ROBOT = 102,				// UDPServer向手机脚本机器人请求指定QQ的令牌数据
        TRANS_ROBOT_TO_UDPSVR = 103,				// 手机脚本机器人向UDPServer发送指定QQ号对应的令牌数据
        TRANS_UDPSVR_TO_RC2 = 104,				// UDPServer向RC2发送指定QQ的令牌数据

        TRANS_TALKNUM = 111,                    //机器人喊话次数
        TRANS_TALKNUM_RET = 112,
        // 对应的数据格式
        // UDPDATA->type=信令
        // UDPDATA->szOrder=订单号
        // UDPDATA->data="FQQ=100000\r\nFToken=999999\r\n" 其余不变。
        // Add By ZLC 20130130 END

    };
    public struct UdpData
    {
        public string strOrderNO;
        public string strData;
        public int udpSignal;
    }

    // 定义 UdpState类
    public class UdpState
    {
        public UdpClient udpClient = null;
        public IPEndPoint ipEndPoint = null;
        public const int BufferSize = 8192;
        public byte[] buffer = new byte[BufferSize];
        public int counter = 0;
    }
    public class udpSockets
    {
        public const int UDP_DATA_LEN = 8192;
        public const int UDP_ORDERNO_LEN = 50;
        public const int UDP_INFO_LEN = 8192-50-4;
        public UdpData udpdata;



        UdpState recS;

        //接收数据缓冲区
        public byte[] m_recvData = new byte[UDP_DATA_LEN];
        public IPEndPoint RemoteIpEndPoint;//保存接收到的ip及端口
        public UdpClient UdpSend;
        public UdpClient UdpRecv;
        //public UdpClient myUdp;
        IAsyncResult iarReceive;
        //int myPort = 6801;//接收端口
        //public int MyPort => myPort;

        //public bool udpInit(int port)
        //{
        //    try
        //    {
        //        myUdp = new UdpClient(port);
        //        //myPort = port;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //    RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //    recS = new UdpState();
        //    recS.udpClient = myUdp;
        //    recS.ipEndPoint = RemoteIpEndPoint;
        //    myUdp.BeginReceive(new AsyncCallback(ReceiveCallback), recS);
        //    return true;
        //}
        public bool udpInit()
        {

            try
            {
                UdpSend = new UdpClient();
            }
            catch (Exception e)
            {
                //myPort++;
            }


            return true;
        }
        public bool udpRecvInit(int RecvPort)
        {
            IPEndPoint localIpep = new IPEndPoint(IPAddress.Any, RecvPort); // 监听本机任意IP的指定端口号
            try
            {
                UdpRecv = new UdpClient(localIpep);
            }
            catch
            {
                return false;
            }

            recS = new UdpState();
            recS.udpClient = UdpRecv;
            iarReceive = UdpRecv.BeginReceive(new AsyncCallback(ReceiveCallback), recS);
            return true;
        }
        public void EndRecv()
        {
            UdpRecv.Close();
        }
        public void EndSend()
        {
            UdpSend.Close();
        }




        // 接收回调函数
        public void ReceiveCallback(IAsyncResult iar)
        {
            
            UdpState udpState = iar.AsyncState as UdpState;
            if (iar.IsCompleted)
            {
                try
                {
                    Array.Clear(m_recvData, 0, m_recvData.Length);
                    m_recvData = udpState.udpClient.EndReceive(iar, ref recS.ipEndPoint);
                    int type = 0;
                    for (int i = 3; i >= 0; i--)
                    {
                        type = type * 256 + Convert.ToInt32(m_recvData[i]);
                    }
                    udpdata.udpSignal = type;
                    udpdata.strOrderNO = TOOL.StringTool.GetCut(Encoding.Default.GetString(m_recvData, 4, UDP_ORDERNO_LEN));

                    int num = 0;
                    for (int i = 54; i < m_recvData.Length; i++)
                    {
                        if (m_recvData[i] == 0)
                        {
                            num=i;
                            break;
                        }     
                    }
                    
                    udpdata.strData = Encoding.Default.GetString(m_recvData, UDP_ORDERNO_LEN + 4, num - 54);
                    HH_RC_Client.Form1.mainForm.HandleUdpData(type, udpdata.strOrderNO, udpdata.strData,recS.ipEndPoint.Address.ToString(),recS.ipEndPoint.Port);
                    //string m_strOrderData = udpdata.strData;
                    
                    //switch (udpdata.udpSignal)
                    //{
                    //    case 2:
                    //        UDPDemo.Form1.mainForm.UpdateTextBox(type, udpdata.strOrderNO,m_strOrderData);

                    //        //TRANS_ORDER_DATA 订单数据
                    //        break;
                    //    case 31:
                    //        //TRANS_RES_IDCODE_RESULT 答题答案
                    //        break;
                    //    case 36:
                    //        //TRANS_SEND_MAIL 同意邮寄
                    //        //GTR_dnf.IsAskMail = true;
                    //        break;
                    //    case 40:
                    //        //TRANS_INSERT_ORDER 插入订单
                    //        //theUDPSend(18, "申请插入订单:", GTR_dnf.OrdNo);
                    //        break;
                    //    default:
                    //        UDPDemo.Form1.mainForm.UpdateTextBox(type, udpdata.strOrderNO, m_strOrderData);
                    //        break;
                    //}

                    UdpRecv.BeginReceive(new AsyncCallback(ReceiveCallback), recS);
                    //myUdp.BeginReceive(new AsyncCallback(ReceiveCallback), recS);
                }
                catch (Exception Err)
                {

                }    
            }
        }
        //
      


        //发送消息
        public void send(int type, string senddata, string orderno, int port, string hostname)
        {
            try
            {
                //if (UdpSend == null)
                //    UdpSend = UdpRecv;
                IPEndPoint SendPoint = new IPEndPoint(IPAddress.Parse(hostname), port);
                //UdpClient UDPSocket = new UdpClient(hostname,port);
                byte[] m_sendData = new byte[UDP_DATA_LEN];
                m_sendData[0] = Convert.ToByte(type);// Convert.ToByte(type.ToString());
                m_sendData[1] = Convert.ToByte(type / 256);
                byte[] ord, sdata;
                ord = Encoding.Default.GetBytes(orderno);
                sdata = Encoding.Default.GetBytes(senddata);
                if (ord.GetLength(0) > 0)
                    Array.Copy(ord, 0, m_sendData, 4, ord.GetLength(0));
                if (sdata.GetLength(0) > 0)
                    Array.Copy(sdata, 0, m_sendData, 54, sdata.GetLength(0));
                UdpSend.Send(m_sendData, UDP_DATA_LEN, SendPoint);
                //myUdp.Send(m_sendData, UDP_DATA_LEN, SendPoint);
            }
            catch(Exception e)
            {
            }
            
        }
    }
}