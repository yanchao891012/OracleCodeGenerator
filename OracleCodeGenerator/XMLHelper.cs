using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace OracleCodeGenerator
{
    /// <summary>
    /// 功能描述    ：XMLHelper  
    /// 创 建 者    ：yanc
    /// 创建日期    ：2019/2/25 10:30:16 
    /// 最后修改者  ：
    /// 最后修改日期：
    /// </summary>
    public class XMLHelper
    {
        /// <summary>
        /// 数据连接路径
        /// </summary>
        static string ConnPath = AppDomain.CurrentDomain.BaseDirectory + "ConnDb.xml";
        static string TypePath = AppDomain.CurrentDomain.BaseDirectory + "TypeXML.xml";
        /// <summary>
        /// 创建XML节点
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="sid"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        public static void CreateXML(string ip, string port, string sid, string user, string pwd)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement xe = xmlDoc.CreateElement("Conn");//创建一个Conn节点

            XmlElement xc1 = xmlDoc.CreateElement("ConnIP");//IP
            xc1.InnerText = ip;
            xe.AppendChild(xc1);

            XmlElement xc2 = xmlDoc.CreateElement("ConnPort");//端口
            xc2.InnerText = port;
            xe.AppendChild(xc2);

            XmlElement xc3 = xmlDoc.CreateElement("ConnSid");//服务器名称
            xc3.InnerText = sid;
            xe.AppendChild(xc3);

            XmlElement xc4 = xmlDoc.CreateElement("ConnUser");//用户名
            xc4.InnerText = user;
            xe.AppendChild(xc4);

            XmlElement xc5 = xmlDoc.CreateElement("ConnPwd");//密码
            xc5.InnerText = pwd;
            xe.AppendChild(xc5);

            xmlDoc.AppendChild(xe);
            xmlDoc.Save(ConnPath);
        }
        /// <summary>
        /// 读取XML内容赋值给连接
        /// </summary>
        /// <returns></returns>
        public static ConnVo ReadXML()
        {
            ConnVo vo = new ConnVo();
            if (File.Exists(ConnPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConnPath);
                XmlNodeList list = xmlDoc.SelectSingleNode("Conn").ChildNodes;
                vo.ConnIP = list[0].InnerText;
                vo.ConnPort = list[1].InnerText;
                vo.ConnSid = list[2].InnerText;
                vo.ConnUser = list[3].InnerText;
                vo.ConnPwd = list[4].InnerText;
            }
            return vo;
        }
        /// <summary>
        /// 获取转换类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetTypes()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (File.Exists(TypePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(TypePath);
                XmlNodeList list = xmlDoc.SelectNodes("//Type");
                foreach (XmlElement item in list)
                {
                    dic.Add(item.Attributes["key"].Value.ToString(), item.InnerText);
                }
            }
            return dic;
        }
    }
}
