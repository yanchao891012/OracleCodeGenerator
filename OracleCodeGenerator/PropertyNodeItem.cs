using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleCodeGenerator
{
    public class PropertyNodeItem:ObjectNotifyPropertyChanged
    {
        private string icon;
        private string parentName;
        private string childrenName;
        private bool isExpanded;
        private List<PropertyNodeItem> children;
        
        public string Icon
        {
            get
            {
                return icon;
            }

            set
            {
                icon = value;
                RaisePropertyChanged("Icon");
            }
        }

        public string ParentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
                RaisePropertyChanged("ParentName");
            }
        }

        public string ChildrenName
        {
            get
            {
                return childrenName;
            }

            set
            {
                childrenName = value;
                RaisePropertyChanged("ChildrenName");
            }
        }

        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }

            set
            {
                isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        public List<PropertyNodeItem> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value;
                RaisePropertyChanged("Children");
            }
        }

        public PropertyNodeItem()
        {
            Children = new List<PropertyNodeItem>();
        }
    }
}
