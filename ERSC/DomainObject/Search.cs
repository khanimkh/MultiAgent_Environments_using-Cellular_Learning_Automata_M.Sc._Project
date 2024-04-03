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

        //public void Do(DomainObject.Point selectedPoint)
        //{
        //    this.IsDoing = true;
        //    selectedPoint.StartSearchDoing = true;
        //    selectedPoint.STeamID = this.ID;
        //    //int searchTop, searchLeft, pointTop, pointLeft;
        //    //searchTop = this.Top;
        //    //searchLeft = this.Left;
        //    //pointTop = selectedPoint.Top;
        //    //pointLeft = selectedPoint.Left;

        //    //System.Drawing.Pen myPen;
        //    //myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        //    //System.Drawing.Graphics formGraphics = panel1.CreateGraphics();
        //    //formGraphics.DrawLine(myPen, searchLeft, searchTop, pointLeft, pointTop);

        //    //selectedPoint.Do();

        //    ////Clean
        //    //myPen.Color = Color.Green;
        //    //formGraphics.DrawLine(myPen, searchLeft, searchTop, pointLeft, pointTop);

        //    //formGraphics.Dispose();
        //    //myPen.Dispose();
        //    //
        //    #region Best Performance
        //    //if (panel1.InvokeRequired == false)
        //    //{
        //    //Draw 
        //    //System.Drawing.Pen myPen;
        //    //myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        //    //System.Drawing.Graphics formGraphics = panel1.CreateGraphics();
        //    //formGraphics.DrawLine(myPen, rescLeft, rescTop, pointLeft, pointTop);

        //    //pointInNorthWest[0].Do();

        //    ////Clean
        //    //myPen.Color = Color.White;
        //    //formGraphics.DrawLine(myPen, rescLeft, rescTop, pointLeft, pointTop);

        //    //formGraphics.Dispose();
        //    //myPen.Dispose();
        //    //}
        //    //else
        //    //{
        //    //    DomainObject.Point p = pointInNorthWest[0];
        //    //    DoRescuInvoker invoker = new DoRescuInvoker(DoRescuInvoke);
        //    //    this.Invoke(invoker, new object[] { rescLeft, rescTop, pointLeft, pointTop, p });
        //    //}

        //    //var lbl = panel1.Controls.Find(pointInNorthWest[0].ID, true).ToList().FirstOrDefault();
        //    //if (lbl != null || lbl.Name != "")
        //    //{
        //    //    lbl.BackColor = Color.Navy;
        //    //}
        //    //
        //    #endregion
        //    //
        //    //if (orderdPoints.Remove(selectedPoint))
        //    //{
        //    DomainObject.Point nextPoint = SelectNextPointForSearchInNorthWest(this);
        //    if (nextPoint != null)
        //    {
        //        Do(nextPoint);
        //    }
        //    //}
        //    //else
        //    //{
        //    //}
        //    //
        //    this.IsDoing = false;
        //}

        //public DomainObject.Point SelectNextPointForSearchInNorthWest(Search search)
        //{
        //    List<DomainObject.Point> orderdPointInNorthWest = ERSC.Form1.points.Where(p => p.Container == UnitType.NorthWest && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();

        //    foreach (var item in orderdPointInNorthWest)
        //    {
        //        item.Distance = GetDistance(item.Left, search.Left, item.Top, search.Top);
        //    }
        //    orderdPointInNorthWest = orderdPointInNorthWest.OrderBy(p => p.Distance).ToList();

        //    if (orderdPointInNorthWest.Count != 0)
        //        return orderdPointInNorthWest.First(p => p.StartSearchDoing.Equals(false));
        //    else
        //        return null;
        //}

        //int GetDistance(int x1, int x2, int y1, int y2)
        //{
        //    int temp = ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
        //    return (int)Math.Sqrt(temp);
        //}

    }
}
