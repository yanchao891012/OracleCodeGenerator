using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OracleCodeGenerator
{
    public class ConnVo : ObjectNotifyPropertyChanged
    {
        private string connIP;//ip地址
        private string connPort;//端口
        private string connSid;//服务器名称
        private string connUser;//用户名
        private string connPwd;//密码

        public string ConnIP
        {
            get
            {
                return connIP;
            }

            set
            {
                connIP = value;
                RaisePropertyChanged("ConnIP");
            }
        }

        public string ConnPort
        {
            get
            {
                return connPort;
            }

            set
            {
                connPort = value;
                RaisePropertyChanged("ConnPort");
            }
        }

        public string ConnSid
        {
            get
            {
                return connSid;
            }

            set
            {
                connSid = value;
                RaisePropertyChanged("ConnSid");
            }
        }

        public string ConnUser
        {
            get
            {
                return connUser;
            }

            set
            {
                connUser = value;
                RaisePropertyChanged("ConnUser");
            }
        }

        public string ConnPwd
        {
            get
            {
                return connPwd;
            }

            set
            {
                connPwd = value;
                RaisePropertyChanged("ConnPwd");
            }
        }
    }
}
