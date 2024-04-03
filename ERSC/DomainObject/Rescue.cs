using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERSC.DomainObject
{
    public class Rescue
    {
        public enum RescueStateType
        {
            Ready,
            Busy
        };

        public Rescue()
        {
            //this.BackColor = Color.Green;
            //this.Image = Properties.Resources.PersonRescue;
            this.Image = Properties.Resources.Rescue_Green;
            this.Width = 10;
            this.Height = 10;
        }

        public static RescueStateType GetSearchState(int n)
        {
            switch (n)
            {
                case 0:
                    return RescueStateType.Ready;
                case 1:
                    return RescueStateType.Busy;
            }
            return RescueStateType.Ready;
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

        public Label GetShape()
        {
            //
            Label lbl = new Label();
            lbl.Name = string.Format("resc_{0}", Guid.NewGuid().ToString());
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

        public bool IsAllocated
        {
            get;
            set;
        }

        //public StringBuilder SteamID
        //{
        //    get;
        //    set;
        //}

        public List<string> SteamIDs
        {
            get;
            set;
        }

        public RescueStateType State
        {
            get;
            set;
        }

        public int Tiredness
        {
            get;
            set;
        }

        public Dictionary<Point,Dictionary<Search,int>> ListPoint
        {
            get;
            set;
        }
    }
}
