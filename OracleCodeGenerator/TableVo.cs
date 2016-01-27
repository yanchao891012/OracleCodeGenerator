using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleCodeGenerator
{
    public class TableVo : ObjectNotifyPropertyChanged
    {
        private string schema;//图表
        private string name;//名称
        private string comments;//注解
        private string type;//类型
        private string nullable;//是否可空
        private string dataLength;//字段长度

        public string Schema
        {
            get
            {
                return schema;
            }

            set
            {
                schema = value;
                RaisePropertyChanged("Schema");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                RaisePropertyChanged("Comments");
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                RaisePropertyChanged("Type");
            }
        }

        public string Nullable
        {
            get
            {
                return nullable;
            }

            set
            {
                nullable = value;
                RaisePropertyChanged("Nullable");
            }
        }

        public string DataLength
        {
            get
            {
                return dataLength;
            }

            set
            {
                dataLength = value;
                RaisePropertyChanged("DataLength");
            }
        }
    }
}
