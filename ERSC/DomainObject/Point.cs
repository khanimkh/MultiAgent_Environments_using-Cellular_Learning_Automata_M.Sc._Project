using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Drawing.Drawing2D;

namespace ERSC.DomainObject
{
    
    public enum RescueLevelType
    {
        None,
        Low,
        Middel,
        High
    };

    public enum StateType
    {
        None,
        Search,
        Rescue,
        Finish
    };

    public class Point
    {
        public const int minNumVictim = 0, maxNumVictim = 10;

        public Point(int timeToBeDone,bool newPoint)
        {
            Random r = new Random();
            //this.BackColor = Color.Red;
            this.Width = 10;
            this.Height = 10;
            if(newPoint)
                this.Image = Properties.Resources.newPoint;
            this.Image = Properties.Resources.Point_Red;
            this.ParentID = "0";
            this.STeamID = "0";
            this.RTeamID = "0";
            //this.StartSearchDoing = false;
            //this.EndSearchDoing = false;
            this.StartRescueDoing = false;
            this.EndRescueDoing = false;
            this.IsAllocatedRTeam = false;
            this.IsAllocatedSTeam = false;
            this.TimeToBeDone = timeToBeDone;
        }

        public static RescueLevelType GetRescueLevel(int numVictim)
        {
            if (numVictim.Equals(0))
                return RescueLevelType.None;
            else if (numVictim < maxNumVictim / 2)
                return RescueLevelType.Low;
            else if (numVictim >= maxNumVictim / 2)
                return RescueLevelType.Middel;
            else if (numVictim.Equals(maxNumVictim))
                return RescueLevelType.High;
            else return RescueLevelType.None;
        }

        public static StateType GetState(int n)
        {
            switch (n)
            {
                case 0:
                    return StateType.None;
                case 1:
                    return StateType.Search;
                case 2:
                    return StateType.Rescue;
                case 3:
                    return StateType.Finish;
            }
            return StateType.None;
        }
        //Timer t = new Timer();

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
            lbl.Name = string.Format("point_{0}", Guid.NewGuid().ToString());
            this.ID = lbl.Name;
            lbl.Text = "";
            //lbl.BackColor = this.BackColor;.
            lbl.Image = this.Image;
            lbl.Width = this.Width;
            lbl.Height = this.Height;
            lbl.Location = new System.Drawing.Point(Left, Top);
            lbl.BorderStyle = BorderStyle.Fixed3D;

            //
            this.Container = Unit.GetUnit(Left, Top);

            //Determin if they unitize correctly 
            //if (Container == UnitType.NorstEast)
            //    lbl.BackColor = Color.Red;
            //else if (Container == UnitType.SouthEast)
            //    lbl.BackColor = Color.Blue;
            //else if (Container == UnitType.NorthWest)
            //    lbl.BackColor = Color.Green;
            //else if (Container == UnitType.SouthWest)
            //    lbl.BackColor = Color.Brown;
            //else
            //    lbl.BackColor = Color.Yellow;

            switch (this.RescueLevel)
            {
                case DomainObject.RescueLevelType.None:
                case DomainObject.RescueLevelType.Low:
                    lbl.Image = Properties.Resources.Point_Yellow;
                    //lbl.BackColor = Color.Yellow;
                    break;
                case DomainObject.RescueLevelType.Middel:
                    lbl.Image =Properties.Resources.Point_Red; 
                    //lbl.BackColor = Color.Red;
                    break;
                case DomainObject.RescueLevelType.High:
                    lbl.Image =Properties.Resources.Point_Red;
                    //lbl.BackColor = Color.DarkRed;
                    break;
            }
            //Show Circle
            GraphicsPath gp2 = new GraphicsPath();
            gp2.AddEllipse(lbl.DisplayRectangle);
            lbl.Region = new Region(gp2);
            //
            return lbl;
        }

        public UnitType Container
        {
            get;
            set;
        }

        public Color BackColor
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

        public void DoSearch()
        {
            int time = this.TimeToBeDone * 1000;
            Thread.Sleep(time);
            EndSearchDoing = true;
            StartSearchDoing = false;
            STeamID = "0";
        }

        public void DoRescue()
        {
            int time = this.TimeToBeDone * 1000;
            Thread.Sleep(time);
            EndRescueDoing = true;
            StartRescueDoing = false;
            RTeamID = "0";
        }

        void t_Tick(object sender, EventArgs e)
        {
            //EndDoing = true;
            //StartDoing = false;
        }

        public int TimeToBeDone
        {
            get;
            set;
        }

        public bool StartSearchDoing
        {
            get;
            set;
        }

        public bool EndSearchDoing
        {
            get;
            set;
        }

        public bool StartRescueDoing
        {
            get;
            set;
        }

        public bool EndRescueDoing
        {
            get;
            set;
        }

        public double Distance
        {
            get;
            set;
        }

        public int NumVictim
        {
            get;
            set;
        }
       
        public RescueLevelType RescueLevel
        {
            get;
            set;
        }

        public StateType State
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public int CreatPoint
        {
            get;
            set;
        }
        
        public string ParentID
        {
            get;
            set;
        }

        public string STeamID
        {
            get;
            set;
        }

        public string RTeamID
        {
            get;
            set;
        }

        public bool IsAllocatedSTeam
        {
            get;
            set;
        }

        public bool IsAllocatedRTeam
        {
            get;
            set;
        }

    }
}
