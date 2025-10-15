using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ERSC.DomainObject
{
    public class Search
    {
        public enum SearchStateType
        {
            Ready,
            Busy
        };

        public Search()
        {
            //this.BackColor = Color.Blue;
            //this.Image = Properties.Resources.PersonSearch;
            this.Image = Properties.Resources.Search_Blue;
            this.Width = 10;
            this.Height = 10;
        }

        public static SearchStateType GetSearchState(int n)
        {
            switch (n)
            {
                case 0:
                    return SearchStateType.Ready;
                case 1:
                    return SearchStateType.Busy;
            }
            return SearchStateType.Ready;
        }

        string _ID;
        public string ID
        {
            get { return _ID; }
            private set
            {
                _ID = value;
            }
        }

        public int IntID
        {
            get;
            set;
        }

        public Label GetShape()
        {
            //
            Label lbl = new Label();
            lbl.Name = string.Format("search_{0}", Guid.NewGuid().ToString());
            this.ID = lbl.Name;
            lbl.Text = "";
            lbl.BackColor = this.BackColor;
            //
            lbl.Visible = true;
            lbl.Show();
            lbl.Size = new Size(this.Width, this.Height);
            lbl.AutoSize = false;
            //
            lbl.Image = this.Image;
            lbl.Width = this.Width;
            lbl.Height = this.Height;
            lbl.Location = new System.Drawing.Point(Left, Top);
            //
            this.Container = Unit.GetUnit(Left, Top);
            return lbl;
        }

        public UnitType Container
        {
            get;
            set;
        }

        private Color BackColor
        {
            get;
            set;
        }

        private Image Image
        {
            get;
            set;
        }

        private int Width
        {
            get;
            set;
        }

        private int Height
        {
            get;
            set;
        }

        public int Top
        {
            get;
            set;
        }

        public int Left
        {
            get;
            set;
        }

        public double TopProjection
        {
            get;
            set;
        }

        public double LeftProjection
        {
            get;
            set;
        }

        public bool IsDoing
        {
            get;
            set;
        }
        
        public string PointID
        {
            get;
            set;
        }

        public SearchStateType State
        {
            get;
            set;
        }

        public IDictionary<string,double> CompetitorProbabilites
        {
            get;
            set;
        }

        public List<DomainObject.Point> ListPoint
        {
            get;
            set;
        }

    }
}
