using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERSC.DomainObject
{
    // Represents a rescue team or agent in the environment
    public class Rescue
    {
        // Enum representing the state of a rescue team
        public enum RescueStateType
        {
            Ready,
            Busy
        };

        // Constructor initializes a new rescue agent with default properties
        public Rescue()
        {
            // Set default image and size for the rescue agent
            this.Image = Properties.Resources.Rescue_Green;
            this.Width = 10;
            this.Height = 10;
        }

        // Converts an integer to a RescueStateType enum value
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

        // Unique identifier for the rescue agent
        string _ID;
        public string ID
        {
            get { return _ID; }
            private set
            {
                _ID = value;
            }
        }

        // Creates and returns a Label representing the rescue agent for UI display
        public Label GetShape()
        {
            Label lbl = new Label();
            lbl.Name = string.Format("resc_{0}", Guid.NewGuid().ToString());
            this.ID = lbl.Name;
            lbl.Text = "";
            lbl.BackColor = this.BackColor;
            lbl.Visible = true;
            lbl.Show();
            lbl.Size = new Size(this.Width, this.Height);
            lbl.AutoSize = false;
            lbl.Image = this.Image;
            lbl.Width = this.Width;
            lbl.Height = this.Height;
            lbl.Location = new System.Drawing.Point(Left, Top);

            // Assign the unit container based on coordinates
            this.Container = Unit.GetUnit(Left, Top);
            return lbl;
        }

        // The unit container type for this rescue agent
        public UnitType Container
        {
            get;
            set;
        }

        // Background color for the rescue agent (not always used)
        private Color BackColor
        {
            get;
            set;
        }

        // Image representing the rescue agent in the UI
        private Image Image
        {
            get;
            set;
        }

        // Width of the rescue agent's UI representation
        private int Width
        {
            get;
            set;
        }

        // Height of the rescue agent's UI representation
        private int Height
        {
            get;
            set;
        }

        // Y coordinate of the rescue agent
        public int Top
        {
            get;
            set;
        }

        // X coordinate of the rescue agent
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

        // Indicates if the rescue agent is currently performing a task
        public bool IsDoing
        {
            get;
            set;
        }

        // ID of the point currently assigned to the rescue agent
        public string PointID
        {
            get;
            set;
        }

        // Indicates if the rescue agent is allocated to a task
        public bool IsAllocated
        {
            get;
            set;
        }

        // List of search team IDs associated with this rescue agent
        public List<string> SteamIDs
        {
            get;
            set;
        }

        // Current state of the rescue agent
        public RescueStateType State
        {
            get;
            set;
        }

        // Tiredness level of the rescue agent
        public int Tiredness
        {
            get;
            set;
        }

        // List of points and associated search tasks for this rescue agent
        public Dictionary<Point,Dictionary<Search,int>> ListPoint
        {
            get;
            set;
        }
    }
}
