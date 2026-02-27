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
    
    // Enum representing the rescue level for a point
    public enum RescueLevelType
    {
        None,
        Low,
        Middel,
        High
    };

    // Enum representing the state of a point
    public enum StateType
    {
        None,
        Search,
        Rescue,
        Finish
    };

    // Represents a location (point) in the map with rescue/search attributes
    public class Point
    {
        // Minimum and maximum number of victims allowed for a point
        public const int minNumVictim = 0, maxNumVictim = 10;

        // Constructor initializes a new point with timing and allocation properties
        public Point(int timeToBeDone,bool newPoint)
        {
            Random r = new Random();
            // Set default size for the point
            this.Width = 10;
            this.Height = 10;
            // Set image depending on whether it's a new point
            if(newPoint)
                this.Image = Properties.Resources.newPoint;
            this.Image = Properties.Resources.Point_Red;
            // Initialize IDs and allocation flags
            this.ParentID = "0";
            this.STeamID = "0";
            this.RTeamID = "0";
            this.StartRescueDoing = false;
            this.EndRescueDoing = false;
            this.IsAllocatedRTeam = false;
            this.IsAllocatedSTeam = false;
            this.TimeToBeDone = timeToBeDone;
        }

        // Determines the rescue level based on the number of victims
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

        // Converts an integer to a StateType enum value
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

        // Unique identifier for the point
        string _ID;
        public string ID
        {
            get { return _ID; }
            private set
            {
                _ID = value;
            }
        }

        // Creates and returns a Label representing the point's shape for UI display
        public Label GetShape()
        {
            Label lbl = new Label();
            lbl.Name = string.Format("point_{0}", Guid.NewGuid().ToString());
            this.ID = lbl.Name;
            lbl.Text = "";
            lbl.Image = this.Image;
            lbl.Width = this.Width;
            lbl.Height = this.Height;
            lbl.Location = new System.Drawing.Point(Left, Top);
            lbl.BorderStyle = BorderStyle.Fixed3D;

            // Assign the unit container based on coordinates
            this.Container = Unit.GetUnit(Left, Top);

            // Set image based on rescue level
            switch (this.RescueLevel)
            {
                case DomainObject.RescueLevelType.None:
                case DomainObject.RescueLevelType.Low:
                    lbl.Image = Properties.Resources.Point_Yellow;
                    break;
                case DomainObject.RescueLevelType.Middel:
                    lbl.Image =Properties.Resources.Point_Red; 
                    break;
                case DomainObject.RescueLevelType.High:
                    lbl.Image =Properties.Resources.Point_Red;
                    break;
            }
            // Display as a circle
            GraphicsPath gp2 = new GraphicsPath();
            gp2.AddEllipse(lbl.DisplayRectangle);
            lbl.Region = new Region(gp2);
            return lbl;
        }

        // The unit container type for this point
        public UnitType Container
        {
            get;
            set;
        }

        // Background color for the point (not always used)
        public Color BackColor
        {
            get;
            set;
        }

        // Image representing the point in the UI
        private Image Image
        {
            get;
            set;
        }

        // Width of the point's UI representation
        private int Width
        {
            get;
            set;
        }

        // Height of the point's UI representation
        private int Height
        {
            get;
            set;
        }

        // Y coordinate of the point
        public int Top
        {
            get;
            set;
        }

        // X coordinate of the point
        public int Left
        {
            get;
            set;
        }

        // Projected Y coordinate (for mapping/projection)
        public double TopProjection
        {
            get;
            set;
        }

        // Projected X coordinate (for mapping/projection)
        public double LeftProjection
        {
            get;
            set;
        }

        // Simulates the search operation for this point
        public void DoSearch()
        {
            int time = this.TimeToBeDone * 1000;
            Thread.Sleep(time);
            EndSearchDoing = true;
            StartSearchDoing = false;
            STeamID = "0";
        }

        // Simulates the rescue operation for this point
        public void DoRescue()
        {
            int time = this.TimeToBeDone * 1000;
            Thread.Sleep(time);
            EndRescueDoing = true;
            StartRescueDoing = false;
            RTeamID = "0";
        }

        // Timer tick event handler (not used)
        void t_Tick(object sender, EventArgs e)
        {
            //EndDoing = true;
            //StartDoing = false;
        }

        // Time required to complete search/rescue operation
        public int TimeToBeDone
        {
            get;
            set;
        }

        // Indicates if search operation has started
        public bool StartSearchDoing
        {
            get;
            set;
        }

        // Indicates if search operation has ended
        public bool EndSearchDoing
        {
            get;
            set;
        }

        // Indicates if rescue operation has started
        public bool StartRescueDoing
        {
            get;
            set;
        }

        // Indicates if rescue operation has ended
        public bool EndRescueDoing
        {
            get;
            set;
        }

        // Distance from reference point (for sorting or assignment)
        public double Distance
        {
            get;
            set;
        }

        // Number of victims at this point
        public int NumVictim
        {
            get;
            set;
        }

        // Rescue level for this point
        public RescueLevelType RescueLevel
        {
            get;
            set;
        }

        // Current state of the point
        public StateType State
        {
            get;
            set;
        }

        // Priority value for assignment
        public int Priority
        {
            get;
            set;
        }

        // Creation index or identifier
        public int CreatPoint
        {
            get;
            set;
        }

        // Parent point ID (for hierarchical relationships)
        public string ParentID
        {
            get;
            set;
        }

        // Search team ID assigned to this point
        public string STeamID
        {
            get;
            set;
        }

        // Rescue team ID assigned to this point
        public string RTeamID
        {
            get;
            set;
        }

        // Indicates if a search team is allocated
        public bool IsAllocatedSTeam
        {
            get;
            set;
        }

        // Indicates if a rescue team is allocated
        public bool IsAllocatedRTeam
        {
            get;
            set;
        }

    }
}
