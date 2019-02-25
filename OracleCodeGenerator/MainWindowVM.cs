using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OracleCodeGenerator
{
    public class MainWindowVM : ViewModelBase
    {
        public MainWindowVM()
        {
            //创建文件夹
            CreateVo.CreateDirectory();
            SetConnVo();
        }

        #region 字段
        /// <summary>
        /// 命名空间
        /// </summary>
        private string ns = "";
        public string Ns
        {
            get
            {
                return ns;
            }

            set
            {
                ns = value;
                RaisePropertyChanged("Ns");
            }
        }

        /// <summary>
        /// 连接类
        /// </summary>
        private ConnVo connectionVo = new ConnVo();
        public ConnVo ConnectionVo
        {
            get
            {
                return connectionVo;
            }

            set
            {
                connectionVo = value;
                RaisePropertyChanged("ConnectionVo");
            }
        }

        /// <summary>
        /// 表
        /// </summary>
        private TableVo table;
        public TableVo Table
        {
            get
            {
                return table;
            }

            set
            {
                table = value;
                RaisePropertyChanged("Table");
            }
        }

        /// <summary>
        /// 左侧列表
        /// </summary>
        private ObservableCollection<PropertyNodeItem> treeList = new ObservableCollection<PropertyNodeItem>();
        public ObservableCollection<PropertyNodeItem> TreeList
        {
            get
            {
                return treeList;
            }

            set
            {
                treeList = value;
                RaisePropertyChanged("TreeList");
            }
        }

        /// <summary>
        /// 原始列表
        /// </summary>
        private List<PropertyNodeItem> orgList = new List<PropertyNodeItem>();
        public List<PropertyNodeItem> OrgList
        {
            get
            {
                return orgList;
            }

            set
            {
                orgList = value;
                RaisePropertyChanged("OrgList");
            }
        }

        /// <summary>
        /// 父列表
        /// </summary>
        private List<string> ownerList = new List<string>();
        public List<string> OwnerList
        {
            get
            {
                return ownerList;
            }

            set
            {
                ownerList = value;
                RaisePropertyChanged("OwnerList");
            }
        }

        /// <summary>
        /// 连接成功以后，就隐藏上边的部分
        /// </summary>
        private Visibility isVisibility = Visibility.Visible;
        public Visibility IsVisibility
        {
            get
            {
                return isVisibility;
            }

            set
            {
                isVisibility = value;
                RaisePropertyChanged("IsVisibility");
            }
        }

        /// <summary>
        /// 表名选中项
        /// </summary>
        private TableVo selectTableName = new TableVo();
        public TableVo SelectTableName
        {
            get
            {
                return selectTableName;
            }

            set
            {
                selectTableName = value;
                RaisePropertyChanged("SelectTableName");
            }
        }

        /// <summary>
        /// 表名list
        /// </summary>
        private ObservableCollection<TableVo> tableNameGridList = new ObservableCollection<TableVo>();
        public ObservableCollection<TableVo> TableNameGridList
        {
            get
            {
                return tableNameGridList;
            }

            set
            {
                tableNameGridList = value;
                RaisePropertyChanged("TableNameGridList");
            }
        }
        /// <summary>
        /// 表内容
        /// </summary>
        private ObservableCollection<TableVo> tableContentGridList = new ObservableCollection<TableVo>();
        public ObservableCollection<TableVo> TableContentGridList
        {
            get
            {
                return tableContentGridList;
            }

            set
            {
                tableContentGridList = value;
                RaisePropertyChanged("TableContentGridList");
            }
        }

        /// <summary>
        /// 表名是否显示
        /// </summary>
        private Visibility visibilityName = Visibility.Collapsed;
        public Visibility VisibilityName
        {
            get
            {
                return visibilityName;
            }

            set
            {
                visibilityName = value;
                RaisePropertyChanged("VisibilityName");
            }
        }

        /// <summary>
        /// 表内容是否显示
        /// </summary>
        private Visibility visibilityContent = Visibility.Collapsed;
        public Visibility VisibilityContent
        {
            get
            {
                return visibilityContent;
            }

            set
            {
                visibilityContent = value;
                RaisePropertyChanged("VisibilityContent");
            }
        }
        #endregion

        #region 函数
        /// <summary>
        /// 给连接的东西赋值
        /// </summary>
        private void SetConnVo()
        {
            ConnectionVo = XMLHelper.ReadXML();
        }
        /// <summary>
        /// DataTable转换到List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private void DataTableToList(DataTable dt)
        {
            //获取最原始的List
            var orgList = (from dataTable in dt.AsEnumerable()
                           select new PropertyNodeItem()
                           {
                               ParentName = dataTable["OWNER"].ToString(),
                               ChildrenName = dataTable["TABLE_NAME"].ToString()
                           }).ToList();
            OrgList.AddRange(orgList);

            DataTable newdt = new DataView(dt.Columns["OWNER"].Table).ToTable(true, "OWNER");//去除重复，只留下OWNER
            //只获得Owner列表
            var newList = (from dataTable in newdt.AsEnumerable()
                           select dataTable["OWNER"].ToString());
            OwnerList.AddRange(newList);
        }
        /// <summary>
        /// 给Treeview赋值
        /// </summary>
        /// <param name="listOrg">原始列表</param>
        /// <param name="listNew">Own列表</param>
        private void SetTree(List<PropertyNodeItem> listOrg, List<string> listNew)
        {
            TreeList.Clear();
            for (int i = 0; i < listNew.Count; i++)
            {
                PropertyNodeItem node = new PropertyNodeItem()
                {
                    ParentName = listNew[i].ToString(),
                    ChildrenName = listNew[i].ToString(),
                    Icon = @"../Icon/father.ico",
                    IsExpanded = false
                };
                ForeachPropertyNode(node, listOrg, listNew[i].ToString());
                TreeList.Add(node);
            }
        }
        /// <summary>
        /// 向父节点中添加子节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="listOrg">原始的列表</param>
        /// <param name="parentName">父节点名称</param>
        private void ForeachPropertyNode(PropertyNodeItem node, List<PropertyNodeItem> listOrg, string parentName)
        {
            var listChildren = (from dataTable in listOrg.AsEnumerable()
                                where dataTable.ParentName == parentName
                                select new PropertyNodeItem()
                                {
                                    ParentName = dataTable.ParentName.ToString(),
                                    ChildrenName = dataTable.ChildrenName.ToString(),
                                    Icon = @"../Icon/children.ico",
                                    IsExpanded = false
                                });
            node.Children.AddRange(listChildren);
        }

        /// <summary>
        /// 获取表名列
        /// </summary>
        /// <param name="owner"></param>
        private void GetTableNameList(string owner)
        {
            TableNameGridList.Clear();
            DBHelper.GetTableName(owner);
            var list = (from dt in DBHelper.ReturnTableName.AsEnumerable()
                        select new TableVo()
                        {
                            Schema = dt["OWNER"].ToString(),
                            Name = dt["TABLE_NAME"].ToString(),
                            Type = dt["TABLE_TYPE"].ToString(),
                            Comments = dt["COMMENTS"].ToString()
                        }).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                TableVo tablevo = new TableVo()
                {
                    Schema = list[i].Schema,
                    Comments = list[i].Comments,
                    Name = list[i].Name,
                    Type = list[i].Type
                };
                TableNameGridList.Add(tablevo);
            }
        }
        /// <summary>
        /// 获取表内容
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name"></param>
        private void GetTableContentList(string owner, string name)
        {
            TableContentGridList.Clear();
            DBHelper.GetTableContent(owner, name);
            var list = (from dt in DBHelper.ReturnTableContent.AsEnumerable()
                        select new TableVo()
                        {
                            Name = dt["COLUMN_NAME"].ToString(),
                            Type = dt["DATA_TYPE"].ToString(),
                            Comments = dt["COMMENTS"].ToString(),
                            Nullable = dt["NULLABLE"].ToString(),
                            DataLength = dt["DATA_LENGTH"].ToString()
                        }).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                TableVo tablevo = new TableVo()
                {
                    Name = list[i].Name,
                    Type = list[i].Type,
                    Comments = list[i].Comments,
                    Nullable = list[i].Nullable,
                    DataLength = list[i].DataLength
                };
                TableContentGridList.Add(tablevo);
            }
        }

        /// <summary>
        /// 获得数据库里的字段名
        /// </summary>
        /// <returns></returns>
        private List<string> GetListName()
        {
            List<string> list = new List<string>();
            if (TableContentGridList.Count > 0)
            {
                for (int i = 0; i < TableContentGridList.Count; i++)
                {
                    list.Add(TableContentGridList[i].Name.ToLower());
                }
                return list;
            }
            return null;
        }
        #endregion

        #region 按钮
        /// <summary>
        /// 连接按钮
        /// </summary>
        private RelayCommand linkCommand;
        public RelayCommand LinkCommand
        {
            get
            {
                return linkCommand = new RelayCommand(() =>
                  {
                      if (DBHelper.OpenConnection(ConnectionVo.ConnIP, ConnectionVo.ConnPort, ConnectionVo.ConnSid, ConnectionVo.ConnUser, ConnectionVo.ConnPwd))
                      {
                          DataTableToList(DBHelper.ReturnOwner);
                          SetTree(OrgList, OwnerList);
                          IsVisibility = Visibility.Collapsed;
                          XMLHelper.CreateXML(ConnectionVo.ConnIP, ConnectionVo.ConnPort, ConnectionVo.ConnSid, ConnectionVo.ConnUser, ConnectionVo.ConnPwd);
                      }
                      else
                      {
                          MessageBox.Show("连接失败！");
                      }
                  });
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private RelayCommand closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand = new RelayCommand(() =>
                  {
                      DBHelper.CloseConnection();
                      Messenger.Default.Send<string>(null, "WinClose");
                  });
            }
        }

        /// <summary>
        /// 创建普通VO按钮
        /// </summary>
        private RelayCommand<PropertyNodeItem> createVoNoINotifyPropertyChangedCommand;
        public RelayCommand<PropertyNodeItem> CreateVoNoINotifyPropertyChangedCommand
        {
            get
            {
                return createVoNoINotifyPropertyChangedCommand = new RelayCommand<PropertyNodeItem>(p =>
                  {
                      if (p == null || (p.ParentName.Equals(p.ChildrenName) && string.IsNullOrEmpty(SelectTableName.Name)))
                          return;
                      if (p.ParentName.Equals(p.ChildrenName) && !string.IsNullOrEmpty(SelectTableName.Name))
                      {
                          GetTableContentList(p.ParentName, SelectTableName.Name);
                          CreateVo.CreateVoNoINotifyPropertyChanged(Ns != "" ? Ns : "test", SelectTableName.Name, TableContentGridList.ToList());
                      }
                      else
                      {                          
                          CreateVo.CreateVoNoINotifyPropertyChanged(Ns != "" ? Ns : "test", p.ChildrenName, TableContentGridList.ToList());
                      }
                  });
            }
        }

        /// <summary>
        /// 创建通知VO按钮
        /// </summary>
        private RelayCommand<PropertyNodeItem> createVoWithINotifyPropertyChangedCommand;
        public RelayCommand<PropertyNodeItem> CreateVoWithINotifyPropertyChangedCommand
        {
            get
            {
                return createVoWithINotifyPropertyChangedCommand = new RelayCommand<PropertyNodeItem>(p =>
                {
                    if (p == null || (p.ParentName.Equals(p.ChildrenName) && string.IsNullOrEmpty(SelectTableName.Name)))
                        return;
                    if (p.ParentName.Equals(p.ChildrenName) && !string.IsNullOrEmpty(SelectTableName.Name))
                    {
                        GetTableContentList(p.ParentName, SelectTableName.Name);
                        CreateVo.CreateVoWithINotifyPropertyChanged(Ns != "" ? Ns : "test", SelectTableName.Name, TableContentGridList.ToList());
                    }
                    else
                    {
                        CreateVo.CreateVoWithINotifyPropertyChanged(Ns != "" ? Ns : "test", p.ChildrenName, TableContentGridList.ToList());
                    }

                });
            }
        }
        /// <summary>
        /// 创建通知类按钮
        /// </summary>
        private RelayCommand createINotifyPropertyChangedCommand;
        public RelayCommand CreateINotifyPropertyChangedCommand
        {
            get
            {
                return createINotifyPropertyChangedCommand = new RelayCommand(() =>
                  {
                      CreateVo.CreateRaisePropertyChangedCS(Ns != "" ? Ns : "test");
                  });
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// Treeview切换事件
        /// </summary>
        private RelayCommand<PropertyNodeItem> selectChangedCommand;
        public RelayCommand<PropertyNodeItem> SelectChangedCommand
        {
            get
            {
                return selectChangedCommand = new RelayCommand<PropertyNodeItem>(p =>
                {
                    if (p.ParentName.Equals(p.ChildrenName))
                    {
                        GetTableNameList(p.ParentName);
                        VisibilityName = Visibility.Visible;
                        VisibilityContent = Visibility.Collapsed;
                    }
                    if (!p.ParentName.Equals(p.ChildrenName))
                    {
                        GetTableContentList(p.ParentName, p.ChildrenName);
                        VisibilityName = Visibility.Collapsed;
                        VisibilityContent = Visibility.Visible;
                    }
                });
            }
        }
        #endregion
    }
}
