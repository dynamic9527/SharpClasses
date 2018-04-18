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
    //    public int type;				//��������	(  0: ����  10:״̬  20:�쳣  )
    //    public char[] szOrderNo;		//�������  
    //    public char[] szData;		//��ϸ���� 
    //}
    public enum TRANSTYPE
    {
        TRANS_NONE = 0,				//��ʼ״̬
        TRANS_REQUEST_ORDER = 1,				//���������󶩵�����					( ROBOT -> RC2 )
        TRANS_ORDER_DATA = 2,				//RC2���������ݴ����������				( RC2 -> ROBOT )
        TRANS_ORDER_DATA_RET = 3,				//�����˷����յ��������ݵ�ȷ����Ϣ		( ROBOT -> RC2 )

        TRANS_GAME_DATA = 10,				//�����˷��ض�������Ϸ����				( ROBOT -> RC2 ) 
        TRANS_GAME_DATA_RET = 11,				//RC2ȷ���յ������˵���Ϸ����			( RC2 -> ROBOT )

        TRANS_GOODS_STAUTS = 12,				//�����˷���ִ�е�����״̬				( ROBOT -> RC2 )
        TRANS_GOODS_STATUS_RET = 13,				//���߻������յ� TRANS_GOODS_STAUTS ��Ϣ( RC2 -> ROBOT )

        TRANS_ORDER_NEW_LOG = 18,				//�����˸���RC2������־ [�˼�¼������վ����ͳ��]
        TRANS_ORDER_NEW_LOG_RET = 19,				//RC2���߻������յ���־ [�˼�¼������վ����ͳ��]

        TRANS_REQ_IDCODE_RESULT = 30,				//����������GTR������֤��               ( ROBOT -> RC2 ) 
        TRANS_RES_IDCODE_RESULT = 31,				//���ʹ��������֤��ĵ������˳���      ( RC2 -> ROBOT ) 
        TRANS_IDCODE_INPUT_RESULT = 32,				//������������֤���Ľ�����͸��ͻ���  ( ROBOT -> RC2 )

        TRANS_AGREE_SENDMAIL = 35,				//���뷢���ʼ�
        TRANS_SEND_MAIL = 36,				//ͬ�ⷢ���ʼ�

        TRANS_INSERT_ORDER = 40,				//������붩��
        TRANS_AGREE_INSERT = 41,				//ͬ����붩��

        TRANS_ORDER_END = 50,				//����������ɣ������ƽ���				( ROBOT -> RC2 )
        TRANS_ORDER_END_RET = 51,				//���߻������յ� TRANS_ORDER_END ��Ϣ	( RC2 -> ROBOT )

        TRANS_ORDER_CANCEL = 52,				//���볷��								( ROBOT -> RC2 )
        TRANS_ORDER_CANCEL_RET = 53,				//���߻������յ� TRANS_ORDER_CANCEL ��Ϣ( RC2 -> ROBOT )

        TRANS_ORDER_ABOLISH = 54,				//ȡ������								( ROBOT -> RC2 )
        TRANS_ORDER_ABOLISH_RET = 55,				//���߻������յ� TRANS_ORDER_ABOLISH ��Ϣ( RC2 -> ROBOT )

        TRANS_ORDER_OP = 71,				//ת�˹�����								( ROBOT -> RC2 )
        TRANS_ORDER_OP_RET = 81,				//���߻������յ� TRANS_ORDER_OP ��Ϣ	( RC2 -> ROBOT )

        TRANS_ORDER_OP_SUCESS = 82,				//ת�˹��������								( ROBOT -> RC2 )
        TRANS_ORDER_OP_SUCESS_RET = 72,				//���߻������յ� TRANS_ORDER_OP_SUCESS ��Ϣ	( RC2 -> ROBOT )

        TRANS_RC2_TO_UDPSVR = 101,				// RC2��UDPServer����QQ��������
        TRANS_UDPSVR_TO_ROBOT = 102,				// UDPServer���ֻ��ű�����������ָ��QQ����������
        TRANS_ROBOT_TO_UDPSVR = 103,				// �ֻ��ű���������UDPServer����ָ��QQ�Ŷ�Ӧ����������
        TRANS_UDPSVR_TO_RC2 = 104,				// UDPServer��RC2����ָ��QQ����������

        TRANS_TALKNUM = 111,                    //�����˺�������
        TRANS_TALKNUM_RET = 112,
        // ��Ӧ�����ݸ�ʽ
        // UDPDATA->type=����
        // UDPDATA->szOrder=������
        // UDPDATA->data="FQQ=100000\r\nFToken=999999\r\n" ���಻�䡣
        // Add By ZLC 20130130 END

    };
    public struct UdpData
    {
        public string strOrderNO;
        public string strData;
        public int udpSignal;
    }

    // ���� UdpState��
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

        //�������ݻ�����
        public byte[] m_recvData = new byte[UDP_DATA_LEN];
        public IPEndPoint RemoteIpEndPoint;//������յ���ip���˿�
        public UdpClient UdpSend;
        public UdpClient UdpRecv;
        //public UdpClient myUdp;
        IAsyncResult iarReceive;
        //int myPort = 6801;//���ն˿�
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
            IPEndPoint localIpep = new IPEndPoint(IPAddress.Any, RecvPort); // ������������IP��ָ���˿ں�
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




        // ���ջص�����
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

                    //        //TRANS_ORDER_DATA ��������
                    //        break;
                    //    case 31:
                    //        //TRANS_RES_IDCODE_RESULT �����
                    //        break;
                    //    case 36:
                    //        //TRANS_SEND_MAIL ͬ���ʼ�
                    //        //GTR_dnf.IsAskMail = true;
                    //        break;
                    //    case 40:
                    //        //TRANS_INSERT_ORDER ���붩��
                    //        //theUDPSend(18, "������붩��:", GTR_dnf.OrdNo);
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
      


        //������Ϣ
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