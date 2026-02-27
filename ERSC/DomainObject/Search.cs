using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ERSC.DomainObject
{
    // Represents a search team or agent in the environment
    public class Search
    {
        // Enum representing the state of a search team
        public enum SearchStateType
        {
            Ready,
            Busy
        };

        // Constructor initializes a new search agent with default properties
        public Search()
        {
            // Set default image and size for the search agent
            this.Image = Properties.Resources.Search_Blue;
            this.Width = 10;
            this.Height = 10;
        }

        // Converts an integer to a SearchStateType enum value
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

        // Unique identifier for the search agent
        string _ID;
        public string ID
        {
            get { return _ID; }
            private set
            {
                _ID = value;
            }
        }

        // Integer identifier for the search agent
        public int IntID
        {
            get;
            set;
        }

        // Creates and returns a Label representing the search agent for UI display
        public Label GetShape()
        {
            Label lbl = new Label();
            lbl.Name = string.Format("search_{0}", Guid.NewGuid().ToString());
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
            this.Container = Unit.GetUnit(Left, Top);
            return lbl;
        }

        // The unit container type for this search agent
        public UnitType Container
        {
            get;
            set;
        }

        // Background color for the search agent (not always used)
        private Color BackColor
        {
            get;
            set;
        }

        // Image representing the search agent in the UI
        private Image Image
        {
            get;
            set;
        }

        // Width of the search agent's UI representation
        private int Width
        {
            get;
            set;
        }

        // Height of the search agent's UI representation
        private int Height
        {
            get;
            set;
        }

        // Y coordinate of the search agent
        public int Top
        {
            get;
            set;
        }

        // X coordinate of the search agent
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

        // Indicates if the search agent is currently performing a task
        public bool IsDoing
        {
            get;
            set;
        }

        // ID of the point currently assigned to the search agent
        public string PointID
        {
            get;
            set;
        }

        // Current state of the search agent
        public SearchStateType State
        {
            get;
            set;
        }

        // Probabilities for competitor search teams
        public IDictionary<string,double> CompetitorProbabilites
        {
            get;
            set;
        }

        // List of points assigned to this search agent
        public List<DomainObject.Point> ListPoint
        {
            get;
            set;
        }
    }
}
