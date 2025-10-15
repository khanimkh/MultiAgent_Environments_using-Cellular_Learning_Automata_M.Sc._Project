using ERSC.DomainObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using DotSpatial.Symbology;
using DotSpatial.Topology;

namespace ERSC
{
    public partial class Form1 : Form
    {
        //public DotSpatial.Controls.Map geoMap = new DotSpatial.Controls.Map();
        public Form1()
        {
            db = new DataClasses1DataContext();
            points = new List<DomainObject.Point>();
            newPoints = new List<DomainObject.Point>();
            searchs = new List<DomainObject.Search>();
            rescues = new List<Rescue>();
            InitializeComponent();
            AddMap();
        }

        //***********************//
        private DataClasses1DataContext db;
        //private static EventWaitHandle ewh;
        //private ManualResetEvent re;
        //****
        private int requestedCountThreadInNorthWest = 0;
        private int requestedThisSectionCountThreadInNorthWest = 0;
        private int currentCountThreadInNorthWest = 0;
        private int currentCountThreadInNorthWestBeforStart = 0;        
        private int currentCountThreadInNorthWestStart = 0;
        private int currentCountThreadInNorthWestEnd = 0;
        private int currentCountThreadInNorthWestAfterEnd = 0;
        private int currentThisSectionCountThreadInNorthWest = 0;
        //****
        private int requestedCountThreadInNorthEast = 0;
        private int requestedThisSectionCountThreadInNorthEast = 0;
        private int currentCountThreadInNorthEast = 0;
        private int currentCountThreadInNorthEastBeforStart = 0;
        private int currentCountThreadInNorthEastStart = 0;
        private int currentCountThreadInNorthEastEnd = 0;
        private int currentCountThreadInNorthEastAfterEnd = 0;
        private int currentThisSectionCountThreadInNorthEast = 0;
        //****
        private int requestedCountThreadInSouthWest = 0;
        private int requestedThisSectionCountThreadInSouthWest = 0;
        private int currentCountThreadInSouthWest = 0;
        private int currentCountThreadInSouthWestBeforStart = 0;
        private int currentCountThreadInSouthWestStart = 0;
        private int currentCountThreadInSouthWestEnd = 0;
        private int currentCountThreadInSouthWestAfterEnd = 0;
        private int currentThisSectionCountThreadInSouthWest = 0;
        //****
        private int requestedCountThreadInSouthEast = 0;
        private int requestedThisSectionCountThreadInSouthEast = 0;
        private int currentCountThreadInSouthEast = 0;
        private int currentCountThreadInSouthEastBeforStart = 0;
        private int currentCountThreadInSouthEastStart = 0;
        private int currentCountThreadInSouthEastEnd = 0;
        private int currentCountThreadInSouthEastAfterEnd = 0;
        private int currentThisSectionCountThreadInSouthEast = 0;
        //****
        private readonly object syncLockBeforStartInNorthWest = new object();
        private readonly object syncLockStartInNorthWest = new object();
        private readonly object syncLockMiddleInNorthWest = new object();
        private readonly object syncLockEndInNorthWest = new object();
        private readonly object syncLockAfterEndInNorthWest = new object();
        private readonly object syncObjInNorthWest = new object();
        //****
        private readonly object syncLockBeforStartInNorthEast = new object();
        private readonly object syncLockStartInNorthEast = new object();
        private readonly object syncLockMiddleInNorthEast = new object();
        private readonly object syncLockEndInNorthEast = new object();
        private readonly object syncLockAfterEndInNorthEast = new object();
        private readonly object syncObjInNorthEast = new object();
        //****
        private readonly object syncLockBeforStartInSouthWest = new object();
        private readonly object syncLockStartInSouthWest = new object();
        private readonly object syncLockMiddleInSouthWest = new object();
        private readonly object syncLockEndInSouthWest = new object();
        private readonly object syncLockAfterEndInSouthWest = new object();
        private readonly object syncObjInInSouthWest = new object();
        //****
        private readonly object syncLockBeforStartInSouthEast = new object();
        private readonly object syncLockStartInSouthEast = new object();
        private readonly object syncLockMiddleInSouthEast = new object();
        private readonly object syncLockEndInSouthEast = new object();
        private readonly object syncLockAfterEndInSouthEast = new object();
        private readonly object syncObjInSouthEast = new object();
        //****
        public static double totalDistanceSearch = 0;
        public static double totalTimeOfDistanceRescue = 0;
        public static double totalTimeOfDistanceRs = 0;
        public static double totalDistanceRescue = 0;
        //
        public static int busyRescue = 0;
        public static int idelRescue = 0;
        //public static int allIdelRescue = 0;
        //public static int allBusyRescue = 0;
        public static int numIdelRescue = 0;
        public static int numBusyRescue = 0;
        public static int busyNorthWestRs = 0;
        public static int busyNorthEastRs = 0;
        public static int busySouthWestRs = 0;
        public static int busySouthEastRs = 0;
        public static double efficiencyBusyRescue = 0;
        public static List<string> listOfWinRescueIDInNorthWest = new List<string>();
        public static List<string> listOfWinRescueIDInNorthEast = new List<string>();
        public static List<string> listOfWinRescueIDInSouthWest = new List<string>();
        public static List<string> listOfWinRescueIDInSouthEast = new List<string>();
        //
        public static bool isFinish = false;
        public static List<DomainObject.Point> points;
        public static List<DomainObject.Point> newPoints;
        static List<Search> searchs;
        static List<Rescue> rescues;
        List<Thread> AllSearchThreads = new List<Thread>();
        List<Thread> AllRescueThreads = new List<Thread>();
        public static Dictionary<string, bool> dicIsGivedBestSelectInNorthWest = new Dictionary<string, bool>();
        public static Dictionary<string, bool> dicIsGivedBestSelectInNorthEast = new Dictionary<string, bool>();
        public static Dictionary<string, bool> dicIsGivedBestSelectInSouthWest = new Dictionary<string, bool>();
        public static Dictionary<string, bool> dicIsGivedBestSelectInSouthEast = new Dictionary<string, bool>();

        //**********************
        public List<Series> listSeries = new List<Series>();
        public List<Series> listSeries_S1 = new List<Series>();
        public List<Series> listSeries_S2 = new List<Series>();
        public List<Series> listSeries_S3 = new List<Series>();
        //**********************
        public Series seriesTasksOfSearch = new System.Windows.Forms.DataVisualization.Charting.Series
        {
            Name = "seriesTasksOfSearch",
            Color = System.Drawing.Color.DarkBlue,
            IsVisibleInLegend = false,
            IsXValueIndexed = true,
            ChartType = SeriesChartType.Spline,
             BorderWidth=2
        };

        public Series seriesTasksOfRescue = new System.Windows.Forms.DataVisualization.Charting.Series
        {
            Name = "seriesTasksOfRescue",
            Color = System.Drawing.Color.Green,
            IsVisibleInLegend = false,
            IsXValueIndexed = true,
            ChartType = SeriesChartType.Spline,
             BorderWidth=2
        };
        
        public  System.Diagnostics.Stopwatch ss = new System.Diagnostics.Stopwatch { };
        public  int numPointDoSearch = 0;
        public int numPointDoRescue = 0;
        public  int time = 0;
        //***********************//
        private void AddMap()
        {
            //***** Add ShapeFile
            geoMap.Name = "geomap";
            //geoMap.AddLayer("E:\\Arshad\\TERM_3\\Project\\Start Project\\11-Optimal Simulation of My Project\\Dll GIS && Document\\Map\\New Map2\\Out ArcGis\\PointShap.shx");
            //geoMap.AddLayer("E:\\Arshad\\TERM_3\\Project\\Start Project\\11-Optimal Simulation of My Project\\Dll GIS && Document\\Map\\New Map2\\Out ArcGis\\PolyLineShap.shx");
            //geoMap.AddLayer("E:\\Arshad\\TERM_3\\Project\\Start Project\\11-Optimal Simulation of My Project\\Dll GIS && Document\\Map\\New Map2\\Out ArcGis\\PolyGonShap.shx");
            //string direct = Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\BackgroundCityWithUnite.png";
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PointShap.shx");
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyLineShap.shx");
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyGonShap.shx");
            //
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyLineBlockShap.shx");
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyGonBlockShap.shx");
            //
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyLineParkShap.shx");
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyGonParkShap.shx");
            //
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyLineUnitShap.shx");
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyGonBolvarShap.shx");
            //
            geoMap.AddLayer(Directory.GetParent(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString() + @"\Resources\Map\PolyGonRodShap.shx");
            //
            geoMap.Size = new Size(683, 448);
            ////***Add Color
            //Block1
            DotSpatial.Controls.MapLineLayer lineBlock1 = default(DotSpatial.Controls.MapLineLayer);
            lineBlock1 = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[1];
            LineScheme schemeLineBlock1 = new LineScheme();
            schemeLineBlock1.Categories.Clear();
            LineCategory categoryLine1 = new LineCategory(Color.Gray, Color.Gray, 1, System.Drawing.Drawing2D.DashStyle.Solid, System.Drawing.Drawing2D.LineCap.Flat);
            schemeLineBlock1.Categories.Add(categoryLine1);
            lineBlock1.Symbology = schemeLineBlock1;
            //
            DotSpatial.Controls.MapPolygonLayer polygBlock1 = default(DotSpatial.Controls.MapPolygonLayer);
            polygBlock1 = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[2];
            PolygonScheme schemePolygBlock1 = new PolygonScheme();
            schemePolygBlock1.Categories.Clear();
            PolygonCategory categoryBlock1 = new PolygonCategory(Color.Beige, Color.Gray, 1);//Honeydew//(Color.Tan, Color.SeaShell, 2)//(Color.PeachPuff, Color.Tan, 2)//(Color.LightPink, Color.LightCyan, 2)//(Color.Ivory, Color.Indigo, 2)
            schemePolygBlock1.Categories.Add(categoryBlock1);
            polygBlock1.Symbology = schemePolygBlock1;
            //Block
            DotSpatial.Controls.MapLineLayer lineBlock = default(DotSpatial.Controls.MapLineLayer);
            lineBlock = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[3];
            LineScheme schemeLineBlock = new LineScheme();
            schemeLineBlock.Categories.Clear();
            LineCategory categoryLine = new LineCategory(Color.LightGray, Color.LightGray, 1, System.Drawing.Drawing2D.DashStyle.Solid, System.Drawing.Drawing2D.LineCap.Flat);
            schemeLineBlock.Categories.Add(categoryLine);
            lineBlock.Symbology = schemeLineBlock;
            //
            DotSpatial.Controls.MapPolygonLayer polygBlock = default(DotSpatial.Controls.MapPolygonLayer);
            polygBlock = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[4];
            PolygonScheme schemePolygBlock = new PolygonScheme();
            schemePolygBlock.Categories.Clear();
            PolygonCategory categoryBlock = new PolygonCategory(Color.Beige, Color.Gray, 1);//Honeydew//(Color.Tan, Color.SeaShell, 2)//(Color.PeachPuff, Color.Tan, 2)//(Color.LightPink, Color.LightCyan, 2)//(Color.Ivory, Color.Indigo, 2)
            schemePolygBlock.Categories.Add(categoryBlock);
            polygBlock.Symbology = schemePolygBlock;
            //Park
            DotSpatial.Controls.MapLineLayer linePark = default(DotSpatial.Controls.MapLineLayer);
            linePark = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[5];
            LineScheme schemeLinePark = new LineScheme();
            schemeLineBlock.Categories.Clear();
            LineCategory categoryLinePark = new LineCategory(Color.LightGreen, Color.LightGreen, 2, System.Drawing.Drawing2D.DashStyle.Solid, System.Drawing.Drawing2D.LineCap.Flat);
            schemeLineBlock.Categories.Add(categoryLine);
            schemeLinePark.Categories.Add(categoryLinePark);
            linePark.Symbology = schemeLinePark;
            //
            DotSpatial.Controls.MapPolygonLayer polygPark = default(DotSpatial.Controls.MapPolygonLayer);
            polygPark = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[6];
            PolygonScheme schemePolygPark = new PolygonScheme();
            schemePolygPark.Categories.Clear();
            PolygonCategory categoryPolygPark = new PolygonCategory(Color.PaleGreen, Color.PaleGreen, 2);//(Color.Tan, Color.SeaShell, 2)//(Color.PeachPuff, Color.Tan, 2)//(Color.LightPink, Color.LightCyan, 2)//(Color.Ivory, Color.Indigo, 2)
            schemePolygPark.Categories.Add(categoryPolygPark);
            polygPark.Symbology = schemePolygPark;
            //Unit
            DotSpatial.Controls.MapLineLayer lineUnit = default(DotSpatial.Controls.MapLineLayer);
            lineUnit = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[7];
            LineScheme schemelineUnit = new LineScheme();
            schemeLineBlock.Categories.Clear();
            LineCategory categorylineUnit = new LineCategory(Color.Salmon, 2);
            schemelineUnit.Categories.Add(categorylineUnit);
            lineUnit.Symbology = schemelineUnit;
            //Bolvar
            DotSpatial.Controls.MapPolygonLayer polygBolvar = default(DotSpatial.Controls.MapPolygonLayer);
            polygBolvar = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[8];
            PolygonScheme schemePolygBolvar = new PolygonScheme();
            schemePolygBolvar.Categories.Clear();
            PolygonCategory categoryPolygBolvar = new PolygonCategory(Color.DarkKhaki, Color.Tan, 1);//Wheat//(Color.Tan, Color.SeaShell, 2)//(Color.PeachPuff, Color.Tan, 2)//(Color.LightPink, Color.LightCyan, 2)//(Color.Ivory, Color.Indigo, 2)
            schemePolygBolvar.Categories.Add(categoryPolygBolvar);
            polygBolvar.Symbology = schemePolygBolvar;
            //Rod
            DotSpatial.Controls.MapPolygonLayer polygRod = default(DotSpatial.Controls.MapPolygonLayer);
            polygRod = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[9];
            PolygonScheme schemepolygRod = new PolygonScheme();
            schemepolygRod.Categories.Clear();
            PolygonCategory categorypolygRod = new PolygonCategory(Color.LightBlue, Color.LightBlue, 2);//(Color.Tan, Color.SeaShell, 2)//(Color.PeachPuff, Color.Tan, 2)//(Color.LightPink, Color.LightCyan, 2)//(Color.Ivory, Color.Indigo, 2)
            schemepolygRod.Categories.Add(categorypolygRod);
            polygRod.Symbology = schemepolygRod;
            //
            panel1.Controls.Add(geoMap);
            //*****
        }
        //**********************//
        private void btnSetupPoint_Click(object sender, EventArgs e)
        {
            btnGo.Enabled = false;
            btnStop.Enabled = false;
            btnShowInformation.Enabled = false;
            //
            ss.Reset();
            chartTasksOfSearch.Series.Clear();
            chartTasksOfRescue.Series.Clear();
            seriesTasksOfSearch.Points.Clear();
            seriesTasksOfRescue.Points.Clear();
            //******************//
            chartProbability.Series.Clear();
            chartProbability_S1.Series.Clear();
            chartProbability_S2.Series.Clear();
            chartProbability_S3.Series.Clear();
            //******************//
            //
            DeleteTaskListFromDataBase();
            DeletePoints();
            //
            CreatPoints();
            ShowPoints();
            //
            if (points.Count > 0)
            {
                btnSetupRS.Enabled = true;
                nudSearch.Enabled = true;
                nudRescue.Enabled = true;
            }
            else
            {
                btnSetupRS.Enabled = false;
                nudSearch.Enabled = false;
                nudRescue.Enabled = false;
            }
            //
            totalDistanceSearch = 0;
            totalDistanceRescue = 0;
            totalTimeOfDistanceRescue = 0;
            totalTimeOfDistanceRs = 0;
            txtAverageTimeOfRescue.Text = "";
            txtAvarageBusyResc.Text = "";
            txtIdelRescue.Text = "";
            txtBusyRescue.Text = "";
            efficiencyBusyRescue = 0;
            numBusyRescue = 0;
            //
            AllSearchThreads.Clear();
            AllRescueThreads.Clear();
            //
            dicIsGivedBestSelectInNorthWest.Clear();
            dicIsGivedBestSelectInNorthEast.Clear();
            dicIsGivedBestSelectInSouthWest.Clear();
            dicIsGivedBestSelectInSouthEast.Clear();
            //
            requestedCountThreadInNorthWest = 0;
            requestedThisSectionCountThreadInNorthWest = 0;
            currentCountThreadInNorthWest = 0;
            currentCountThreadInNorthWestBeforStart = 0;
            currentCountThreadInNorthWestStart = 0;
            currentCountThreadInNorthWestEnd = 0;
            currentCountThreadInNorthWestAfterEnd = 0;
            currentThisSectionCountThreadInNorthWest = 0;
            //
            requestedCountThreadInNorthEast = 0;
            requestedThisSectionCountThreadInNorthEast = 0;
            currentCountThreadInNorthEast = 0;
            currentCountThreadInNorthEastBeforStart = 0;
            currentCountThreadInNorthEastStart = 0;
            currentCountThreadInNorthEastEnd = 0;
            currentCountThreadInNorthEastAfterEnd = 0;
            currentThisSectionCountThreadInNorthEast = 0;
            //
            requestedCountThreadInSouthWest = 0;
            requestedThisSectionCountThreadInSouthWest = 0;
            currentCountThreadInSouthWest = 0;
            currentCountThreadInSouthWestBeforStart = 0;
            currentCountThreadInSouthWestStart = 0;
            currentCountThreadInSouthWestEnd = 0;
            currentCountThreadInSouthWestAfterEnd = 0;
            currentThisSectionCountThreadInSouthWest = 0;
            //
            requestedCountThreadInSouthEast = 0;
            requestedThisSectionCountThreadInSouthEast = 0;
            currentCountThreadInSouthEast = 0;
            currentCountThreadInSouthEastBeforStart = 0;
            currentCountThreadInSouthEastStart = 0;
            currentCountThreadInSouthEastEnd = 0;
            currentCountThreadInSouthEastAfterEnd = 0;
            currentThisSectionCountThreadInSouthEast = 0;
            //
            
        }

        private void btnSetupSR_Click(object sender, EventArgs e)
        {
            decimal northWestCriticalPointsCount, southWestCriticalPointsCount, norstEastCriticalPointsCount, southEastCriticalPointsCount;
            northWestCriticalPointsCount = (points.Where(p => p.Container == UnitType.NorthWest).Count() * 100) / points.Count();
            southWestCriticalPointsCount = (points.Where(p => p.Container == UnitType.SouthWest).Count() * 100) / points.Count();
            norstEastCriticalPointsCount = (points.Where(p => p.Container == UnitType.NorthEast).Count() * 100) / points.Count();
            southEastCriticalPointsCount = (points.Where(p => p.Container == UnitType.SouthEast).Count() * 100) / points.Count();

            decimal searchCount = nudSearch.Value;
            Dictionary<UnitType,int> dicUnitSearchCount=new Dictionary<UnitType,int>();

            Unit.NorthWestSearchCount = ((int)Math.Round((searchCount * northWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * northWestCriticalPointsCount) / 100);
            dicUnitSearchCount.Add(UnitType.NorthWest,Unit.NorthWestSearchCount);
            Unit.NorthEastSearchCount = ((int)Math.Round((searchCount * norstEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * norstEastCriticalPointsCount) / 100);
            dicUnitSearchCount.Add(UnitType.NorthEast,Unit.NorthEastSearchCount);
            Unit.SouthEastSearchCount = ((int)Math.Round((searchCount * southEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * southEastCriticalPointsCount) / 100);
            dicUnitSearchCount.Add(UnitType.SouthEast,Unit.SouthEastSearchCount);
            Unit.SouthWestSearchCount = ((int)Math.Round((searchCount * southWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * southWestCriticalPointsCount) / 100);
            dicUnitSearchCount.Add(UnitType.SouthWest,Unit.SouthWestSearchCount);
            
            if (dicUnitSearchCount.Values.Sum() > searchCount)
                switch (dicUnitSearchCount.OrderByDescending(p => p.Value).First().Key)
                {
                    case UnitType.NorthWest:
                        Unit.NorthWestSearchCount--;
                        break;
                    case UnitType.SouthWest:
                        Unit.SouthWestSearchCount--;
                        break;
                    case UnitType.NorthEast:
                        Unit.NorthEastSearchCount--;
                        break;
                    case UnitType.SouthEast:
                        Unit.SouthEastSearchCount--;
                        break;
                }

            if (dicUnitSearchCount.Values.Sum() < searchCount)
                switch (dicUnitSearchCount.OrderByDescending(p => p.Value).First().Key)
                {
                    case UnitType.NorthWest:
                        Unit.NorthWestSearchCount++;
                        break;
                    case UnitType.SouthWest:
                        Unit.SouthWestSearchCount++;
                        break;
                    case UnitType.NorthEast:
                        Unit.NorthEastSearchCount++;
                        break;
                    case UnitType.SouthEast:
                        Unit.SouthEastSearchCount++;
                        break;
                }
            //
            decimal rescueCount = nudRescue.Value;
            Dictionary<UnitType, int> dicUnitRescueCount = new Dictionary<UnitType, int>();

            Unit.NorthWestRescueCount = ((int)Math.Round((rescueCount * northWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * northWestCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.NorthWest, Unit.NorthWestRescueCount);
            Unit.NorthEastRescueCount = ((int)Math.Round((rescueCount * norstEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * norstEastCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.NorthEast, Unit.NorthEastRescueCount);
            Unit.SouthEastRescueCount = ((int)Math.Round((rescueCount * southEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * southEastCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.SouthEast, Unit.SouthEastRescueCount);
            Unit.SouthWestRescueCount = ((int)Math.Round((rescueCount * southWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * southWestCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.SouthWest, Unit.SouthWestRescueCount);

            if (dicUnitRescueCount.Values.Sum() > rescueCount)
            {
                switch (dicUnitRescueCount.OrderByDescending(p => p.Value).First().Key)
                {
                    case UnitType.NorthWest:
                        Unit.NorthWestRescueCount--;
                        break;
                    case UnitType.SouthWest:
                        Unit.SouthWestRescueCount--;
                        break;
                    case UnitType.NorthEast:
                        Unit.NorthEastRescueCount--;
                        break;
                    case UnitType.SouthEast:
                        Unit.SouthEastRescueCount--;
                        break;
                }
            }

            if (dicUnitRescueCount.Values.Sum() < rescueCount)
                switch (dicUnitRescueCount.OrderByDescending(p => p.Value).First().Key)
                {
                    case UnitType.NorthWest:
                        Unit.NorthWestRescueCount++;
                        break;
                    case UnitType.SouthWest:
                        Unit.SouthWestRescueCount++;
                        break;
                    case UnitType.NorthEast:
                        Unit.NorthEastRescueCount++;
                        break;
                    case UnitType.SouthEast:
                        Unit.SouthEastRescueCount++;
                        break;
                }
            //
            DeleteSearchAndRescueTeam();
            //DrawUnit();
            //
            CreateSearchTeam();
            ShowSearchTeam();
            //
            CreateRescueTeam();
            ShowRescueTeam();
            //
            BindDropDownShowInformation();
            //
            busyRescue = 0;
            idelRescue = rescues.Count();
            //
            if (searchs.Count() > 0)
            {
                btnGo.Enabled = true;
                btnStop.Enabled = true;
                btnShowInformation.Enabled = true;
                cboxSearchIntID.Enabled = true;
            }
            else
            {
                btnGo.Enabled = false;
                btnStop.Enabled = false;
                btnShowInformation.Enabled = false;
                cboxSearchIntID.Enabled = false;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            //
            chartTasksOfSearch.Series.Clear();
            chartTasksOfRescue.Series.Clear();
            seriesTasksOfSearch.Points.Clear();
            seriesTasksOfRescue.Points.Clear();
            //
            this.chartTasksOfSearch.Series.Add(seriesTasksOfSearch);
            this.chartTasksOfRescue.Series.Add(seriesTasksOfRescue);

            //*******************************************//
            //*******************************************//
            int searchId = 0;
            chartProbability.ChartAreas[0].AxisX.Interval = 5;
            if (cboxSearchIntID.InvokeRequired)
                cboxSearchIntID.BeginInvoke((MethodInvoker)delegate
                {
                    searchId = (int)cboxSearchIntID.SelectedItem;
                });
            else
            {
                searchId = (int)cboxSearchIntID.SelectedItem;
            }
            Series seriesPropability = new System.Windows.Forms.DataVisualization.Charting.Series { };
            listSeries.Clear();
            System.Drawing.Color color = new System.Drawing.Color();
            List<Rescue> rescuesInUnitOfSelectedSearch = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId)).Container)).ToList();
            for (int j = 0; j < rescuesInUnitOfSelectedSearch.Count(); j++)
            {
                switch (j)
                {
                    case 0: color = System.Drawing.Color.Red;
                        break;
                    case 1: color = System.Drawing.Color.Blue;
                        break;
                    case 2: color = System.Drawing.Color.Green;
                        break;
                    case 3: color = System.Drawing.Color.Yellow;
                        break;
                    case 4: color = System.Drawing.Color.Purple;
                        break;
                    case 5: color = System.Drawing.Color.Brown;
                        break;
                    default: color = System.Drawing.Color.Black;
                        break;
                }
                seriesPropability = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability" + j,
                    Color = color,
                    //Color = System.Drawing.Color.Green,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth=2
                };
                listSeries.Add(seriesPropability);
            }
            if (chartProbability.InvokeRequired)
                chartProbability.BeginInvoke((MethodInvoker)delegate
                {
                    this.chartProbability.Series.Clear();
                    foreach (var item in listSeries)
                    {
                        //item.Points.Clear();
                        this.chartProbability.Series.Add(item);
                    }
                });
            else
            {
                this.chartProbability.Series.Clear();
                foreach (var item in listSeries)
                {
                    //item.Points.Clear();
                    this.chartProbability.Series.Add(item);
                }
            }

            //*************chartProbability1**************//
            int searchId_S1 = 1;
            chartProbability_S1.ChartAreas[0].AxisX.Interval = 5;
            Series seriesPropability_S1 = new System.Windows.Forms.DataVisualization.Charting.Series { };
            listSeries_S1.Clear();
            List<Rescue> rescuesInUnitOfSelectedSearch_S1 = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId_S1)).Container)).ToList();
            for (int j = 0; j < rescuesInUnitOfSelectedSearch_S1.Count(); j++)
            {
                switch (j)
                {
                    case 0: color = System.Drawing.Color.Red;
                        break;
                    case 1: color = System.Drawing.Color.Blue;
                        break;
                    case 2: color = System.Drawing.Color.Green;
                        break;
                    case 3: color = System.Drawing.Color.Yellow;
                        break;
                    case 4: color = System.Drawing.Color.Purple;
                        break;
                    case 5: color = System.Drawing.Color.Brown;
                        break;
                    default: color = System.Drawing.Color.Black;
                        break;
                }
                seriesPropability_S1 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability_S1" + j,
                    Color = color,
                    //Color = System.Drawing.Color.Green,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth = 2
                };
                listSeries_S1.Add(seriesPropability_S1);
            }
            if (chartProbability_S1.InvokeRequired)
                chartProbability_S1.BeginInvoke((MethodInvoker)delegate
                {
                    this.chartProbability_S1.Series.Clear();
                    foreach (var item in listSeries_S1)
                    {
                        //item.Points.Clear();
                        this.chartProbability_S1.Series.Add(item);
                    }
                });
            else
            {
                this.chartProbability_S1.Series.Clear();
                foreach (var item in listSeries_S1)
                {
                    //item.Points.Clear();
                    this.chartProbability_S1.Series.Add(item);
                }
            }

            //*************chartProbability2**************//
            int searchId_S2 = 2;
            chartProbability_S2.ChartAreas[0].AxisX.Interval = 5;
            Series seriesPropability_S2 = new System.Windows.Forms.DataVisualization.Charting.Series { };
            listSeries_S2.Clear();
            List<Rescue> rescuesInUnitOfSelectedSearch_S2 = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId_S2)).Container)).ToList();
            for (int j = 0; j < rescuesInUnitOfSelectedSearch_S2.Count(); j++)
            {
                switch (j)
                {
                    case 0: color = System.Drawing.Color.Red;
                        break;
                    case 1: color = System.Drawing.Color.Blue;
                        break;
                    case 2: color = System.Drawing.Color.Green;
                        break;
                    case 3: color = System.Drawing.Color.Yellow;
                        break;
                    case 4: color = System.Drawing.Color.Purple;
                        break;
                    case 5: color = System.Drawing.Color.Brown;
                        break;
                    default: color = System.Drawing.Color.Black;
                        break;
                }
                seriesPropability_S2 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability_S2" + j,
                    Color = color,
                    //Color = System.Drawing.Color.Green,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth = 2
                };
                listSeries_S2.Add(seriesPropability_S2);
            }
            if (chartProbability_S2.InvokeRequired)
                chartProbability_S2.BeginInvoke((MethodInvoker)delegate
                {
                    this.chartProbability_S2.Series.Clear();
                    foreach (var item in listSeries_S2)
                    {
                        //item.Points.Clear();
                        this.chartProbability_S2.Series.Add(item);
                    }
                });
            else
            {
                this.chartProbability_S2.Series.Clear();
                foreach (var item in listSeries_S2)
                {
                    //item.Points.Clear();
                    this.chartProbability_S2.Series.Add(item);
                }
            }
            //*************chartProbability3**************//
            int searchId_S3 = 3;
            chartProbability_S3.ChartAreas[0].AxisX.Interval =5;
            Series seriesPropability_S3 = new System.Windows.Forms.DataVisualization.Charting.Series { };
            listSeries_S3.Clear();
            List<Rescue> rescuesInUnitOfSelectedSearch_S3 = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId_S3)).Container)).ToList();
            for (int j = 0; j < rescuesInUnitOfSelectedSearch_S3.Count(); j++)
            {
                switch (j)
                {
                    case 0: color = System.Drawing.Color.Red;
                        break;
                    case 1: color = System.Drawing.Color.Blue;
                        break;
                    case 2: color = System.Drawing.Color.Green;
                        break;
                    case 3: color = System.Drawing.Color.Yellow;
                        break;
                    case 4: color = System.Drawing.Color.Purple;
                        break;
                    case 5: color = System.Drawing.Color.Brown;
                        break;
                    default: color = System.Drawing.Color.Black;
                        break;
                }
                seriesPropability_S3 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability_S3" + j,
                    Color = color,
                    //Color = System.Drawing.Color.Green,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth = 2
                };
                listSeries_S3.Add(seriesPropability_S3);
            }
            if (chartProbability_S3.InvokeRequired)
                chartProbability_S3.BeginInvoke((MethodInvoker)delegate
                {
                    this.chartProbability_S3.Series.Clear();
                    foreach (var item in listSeries_S3)
                    {
                        //item.Points.Clear();
                        this.chartProbability_S3.Series.Add(item);
                    }
                });
            else
            {
                this.chartProbability_S3.Series.Clear();
                foreach (var item in listSeries_S3)
                {
                    //item.Points.Clear();
                    this.chartProbability_S3.Series.Add(item);
                }
            }
            //*******************************************//
            //*******************************************//

            numPointDoSearch = points.Count();
            numPointDoRescue = points.Count();
            seriesTasksOfSearch.Points.Add(numPointDoSearch);
            seriesTasksOfRescue.Points.Add(numPointDoRescue);
            ss.Start();
            //
            AllSearchThreads.Clear();
            AllSearchThreads.AddRange(AssignePointToSearchInNorthWest());
            AllSearchThreads.AddRange(AssignePointToSearchInNorthEast());
            AllSearchThreads.AddRange(AssignePointToSearchInSouthWest());
            AllSearchThreads.AddRange(AssignePointToSearchInSouthEast());
            //          
            foreach (var thread in AllSearchThreads)
            {
                thread.Start();
            }
            //
            ShowTime();
        }
        //**********************//
        void DeletePoints()
        {
            //panel1.Controls.Clear();
            for (int index = panel1.Controls.Count - 1; index >= 0; index--)
            {
                if (!panel1.Controls[index].Name.Equals("geomap"))
                {
                    panel1.Controls.RemoveAt(index);
                }
            }

            //Delete From Table
            //var getDataPoints =(from TbPoint in db.TbPoints select TbPoint).ToList();
            //db.TbPoints.DeleteAllOnSubmit(getDataPoints.ToList());
            //db.SubmitChanges();

            using (var dbDelete = new DataClasses1DataContext())
            {
                //Update to sql
                var getDataPoints = (from TbPoint in dbDelete.TbPoints select TbPoint).ToList();
                dbDelete.TbPoints.DeleteAllOnSubmit(getDataPoints.ToList());
                dbDelete.SubmitChanges();
            }

            /////
            using (var dbDelete = new DataClasses1DataContext())
            {
                //Update to sql
                var getDataPoints = (from TbPointFirst in dbDelete.TbPointFirsts select TbPointFirst).ToList();
                dbDelete.TbPointFirsts.DeleteAllOnSubmit(getDataPoints.ToList());
                dbDelete.SubmitChanges();
            }
            /////
        }

        void CreatPoints()
        {
            newPoints.Clear();
            points.Clear();
            Random r = new Random();
            for (int i = 1; i <= nudPoint.Value; i++)
            {
                //DomainObject.Point p = new DomainObject.Point(r.Next(2, 5),false);
                DomainObject.Point p = new DomainObject.Point(2,false);
                p.Left = r.Next(Map.StartX, Map.EndX);
                p.Top = r.Next(Map.StartY, Map.EndY);
                p.Container = Unit.GetUnit(p.Left, p.Top);
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(p.Left,p.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                p.LeftProjection = MyCoordinate.X;
                p.TopProjection = MyCoordinate.Y;
                //
                p.NumVictim= r.Next(DomainObject.Point.minNumVictim, DomainObject.Point.maxNumVictim);
                p.RescueLevel =DomainObject.Point.GetRescueLevel(p.NumVictim);
                p.CreatPoint = r.Next(0, 4);
                p.State = DomainObject.StateType.None;
                //
                //if (points.Where(pt => pt.Top == p.Top && pt.Left == p.Left).Count() > 0)
                //{
                //    throw new Exception("");
                //}
                //
                points.Add(p);
            }
           
        }

        void ShowPoints()
        {
            List<TbPoint> tbPoints = new List<TbPoint>();
            //panel1.Controls.Clear();
            int count = 0;
            foreach (var p in points)
            {
                //DrawPoint(p);
                panel1.Controls.Add(p.GetShape());
                //***Insert in sql
                TbPoint point = new TbPoint();
                //
                point.Point_ID = p.ID;
                point.Point_IntID = count++;
                point.Point_ParentID = "0";
                point.Point_TopCoordinate = p.Top;
                point.Point_LeftCoordinate = p.Left;
                point.Point_TopProjection = p.TopProjection;
                point.Point_LeftProjection = p.LeftProjection;
                point.Poin_Unit = p.Container.ToString();
                point.Point_NumVictim = p.NumVictim;
                point.Point_TimeToBeDone = p.TimeToBeDone;
                point.Point_RescueLevel = (int)p.RescueLevel;
                point.Point_CreatPoint = p.CreatPoint;
                point.Point_State =(int) p.State;
                point.Point_IsAllocatedSTeam = p.IsAllocatedSTeam;
                point.Point_IsAllocatedRTeam = p.IsAllocatedRTeam;
                point.Point_StartSearchDoing = p.StartSearchDoing;
                point.Point_StartRescueDoing = p.StartRescueDoing;
                point.Point_EndSearchDoing = p.EndSearchDoing;
                point.Point_EndRescueDoing = p.EndRescueDoing;
                //
                tbPoints.Add(point);
            }
            db.TbPoints.InsertAllOnSubmit(tbPoints);
            db.SubmitChanges();
            geoMap.SendToBack();
            ////
            List<TbPointFirst> tbPointFirsts = new List<TbPointFirst>();
            foreach (var p in tbPoints)
            {
               
                //***Insert in sql
                TbPointFirst pointFirst = new TbPointFirst();
                //
                pointFirst.Point_ID = p.Point_ID;
                pointFirst.Point_IntID = p.Point_IntID;
                pointFirst.Point_ParentID = p.Point_ParentID;
                pointFirst.Point_TopCoordinate = p.Point_TopCoordinate;
                pointFirst.Point_LeftCoordinate = p.Point_LeftCoordinate;
                pointFirst.Point_TopProjection = p.Point_TopProjection;
                pointFirst.Point_LeftProjection = p.Point_LeftProjection;
                pointFirst.Poin_Unit =p.Poin_Unit;
                pointFirst.Point_NumVictim = p.Point_NumVictim;
                pointFirst.Point_TimeToBeDone = p.Point_TimeToBeDone;
                pointFirst.Point_RescueLevel =p.Point_RescueLevel;
                pointFirst.Point_CreatPoint = p.Point_CreatPoint;
                pointFirst.Point_State = p.Point_State;
                pointFirst.Point_IsAllocatedSTeam = p.Point_IsAllocatedSTeam;
                pointFirst.Point_IsAllocatedRTeam = p.Point_IsAllocatedRTeam;
                pointFirst.Point_StartSearchDoing = p.Point_StartSearchDoing;
                pointFirst.Point_StartRescueDoing = p.Point_StartRescueDoing;
                pointFirst.Point_EndSearchDoing = p.Point_EndSearchDoing;
                pointFirst.Point_EndRescueDoing = p.Point_EndRescueDoing;
                //
                tbPointFirsts.Add(pointFirst);
            }
            db.TbPointFirsts.InsertAllOnSubmit(tbPointFirsts);
            db.SubmitChanges();
            /////
        }   
        //*********************//
        void DeleteSearchAndRescueTeam()
        {
            for (int index = panel1.Controls.Count - 1; index >= 0; index--)
            {
                if (!panel1.Controls[index].Name.StartsWith("geomap")&&!panel1.Controls[index].Name.StartsWith("point"))
                {
                    panel1.Controls.RemoveAt(index);
                }
            }
            geoMap.SendToBack();
            DeleteSearchAndRescueFromDataBase();
        }

        void DeleteSearchAndRescueFromDataBase()
        {
            //Delete From Table
            using (var dbDeleteSearch = new DataClasses1DataContext())
            {
                var getDataSearchs = (from TbSearch in dbDeleteSearch.TbSearches select TbSearch);
                dbDeleteSearch.TbSearches.DeleteAllOnSubmit(getDataSearchs);
                dbDeleteSearch.SubmitChanges();
            }
            //
            using (var dbDeleteRescue = new DataClasses1DataContext())
            {
                var getDataRescues = (from TbRescue in dbDeleteRescue.TbRescues select TbRescue);
                dbDeleteRescue.TbRescues.DeleteAllOnSubmit(getDataRescues);
                dbDeleteRescue.SubmitChanges();
            }

            ////
            //Delete From Table
            using (var dbDeleteSearch = new DataClasses1DataContext())
            {
                var getDataSearchs = (from TbSearchFirst in dbDeleteSearch.TbSearchFirsts select TbSearchFirst);
                dbDeleteSearch.TbSearchFirsts.DeleteAllOnSubmit(getDataSearchs);
                dbDeleteSearch.SubmitChanges();
            }
            //
            using (var dbDeleteRescue = new DataClasses1DataContext())
            {
                var getDataRescues = (from TbRescueFirst in dbDeleteRescue.TbRescueFirsts select TbRescueFirst);
                dbDeleteRescue.TbRescueFirsts.DeleteAllOnSubmit(getDataRescues);
                dbDeleteRescue.SubmitChanges();
            }
            ////

        }

        void DeleteTaskListFromDataBase()
        {
            var getDataTaskList=(from TbTaskList in db.TbTaskLists select TbTaskList);
            db.TbTaskLists.DeleteAllOnSubmit(getDataTaskList.ToList());
            db.SubmitChanges();

            ////
            var getDataTaskListFirst = (from TbTaskListFirst in db.TbTaskListFirsts select TbTaskListFirst);
            db.TbTaskListFirsts.DeleteAllOnSubmit(getDataTaskListFirst.ToList());
            db.SubmitChanges();
        }

        void CreateSearchTeam()
        {
            List<DomainObject.Point> orderdPointInNorthWest = points.Where(p => p.Container == UnitType.NorthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<DomainObject.Point> orderdPointInSouthWest = points.Where(p => p.Container == UnitType.SouthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<DomainObject.Point> orderdPointInNorthEast = points.Where(p => p.Container == UnitType.NorthEast).OrderByDescending(p => p.NumVictim).ToList();
            List<DomainObject.Point> orderdPointInSouthEast = points.Where(p => p.Container == UnitType.SouthEast).OrderByDescending(p => p.NumVictim).ToList();

            int count = 0; 
            searchs.Clear();          
            Random r = new Random();
            for (int i = 1; i <= Unit.NorthWestSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.StartX, (Map.EndX / 2)-15);
                search.Top = r.Next(Map.StartY, (Map.EndY / 2)-20);
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                //
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                //
              
                //
                searchs.Add(search);
            }

            for (int i = 1; i <= Unit.NorthEastSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.EndX / 2, Map.EndX - 15);
                search.Top = r.Next(Map.StartY, (Map.EndY / 2)-20);
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                //
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                
                //
                searchs.Add(search);
            }

            for (int i = 1; i <= Unit.SouthWestSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.StartX, (Map.EndX / 2) - 15);
                search.Top = r.Next(Map.EndY / 2, Map.EndY-20);
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                //
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                //
                
                searchs.Add(search);
            }

            for (int i = 1; i <= Unit.SouthEastSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.EndX / 2, Map.EndX - 15);
                search.Top = r.Next(Map.EndY / 2, Map.EndY-20);
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                //
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                //
                //Search search = new Search();
                //search.Left = orderdPointInSouthEast[i - 1].Left;
                //search.Top = orderdPointInSouthEast[i - 1].Top;
                //search.PointID = orderdPointInSouthEast[i - 1].ID;
                //
                searchs.Add(search);
            }
        }

        void ShowSearchTeam()
        {
            List<TbSearch> tbSearchs = new List<TbSearch>();
            //int count = 0; 
            foreach (var s in searchs)
            {
                //DrawPoint(p);
                panel1.Controls.Add(s.GetShape());
                //***Insert in sql
                TbSearch search = new TbSearch();
                search.Search_ID = s.ID;
                //search.Search_IntID = count++;
                search.Search_IntID = s.IntID;
                search.Search_TopCoordinate = s.Top;
                search.Search_LeftCoordinate = s.Left;
                search.Search_TopProjection = s.TopProjection;
                search.Search_LeftProjection = s.LeftProjection;
                search.Search_Unit = s.Container.ToString();
                search.Search_State = (int) s.State;
                //
                tbSearchs.Add(search);
            }
            db.TbSearches.InsertAllOnSubmit(tbSearchs);
            db.SubmitChanges();
            geoMap.SendToBack();
            ////
            List<TbSearchFirst> tbSearchFirsts = new List<TbSearchFirst>();
            foreach (var s in tbSearchs)
            {
                TbSearchFirst search = new TbSearchFirst();
                search.Search_ID = s.Search_ID;
                search.Search_IntID = s.Search_IntID;
                search.Search_TopCoordinate = s.Search_TopCoordinate;
                search.Search_LeftCoordinate = s.Search_LeftCoordinate;
                search.Search_TopProjection = s.Search_TopProjection;
                search.Search_LeftProjection = s.Search_LeftProjection;
                search.Search_Unit = s.Search_Unit;
                search.Search_State =s.Search_State;
                //
                tbSearchFirsts.Add(search);
            }
            db.TbSearchFirsts.InsertAllOnSubmit(tbSearchFirsts);
            db.SubmitChanges();
            ////
        }

        void CreateRescueTeam()
        {
            rescues.Clear();
            Random r = new Random();
            //
            int leftNorthWest = 0;
            int topNorthWest = 0;
            //
            int leftNorthEast = 0;
            int topNorthEast = 0;
            //
            int leftSouthWest = 0;
            int topSouthWest = 0;
            //
            int leftSouthEast = 0;
            int topSouthEast = 0;
            //
            for (int i = 1; i <= Unit.NorthWestRescueCount; i++)
            {
                Dictionary<int, int> rangeInNorthWest = new Dictionary<int, int>();
                leftNorthWest = r.Next(Map.StartX, (Map.EndX/ 2)-15);
                topNorthWest = r.Next(Map.StartY, (Map.EndY / 2)-20);

                while (searchs.Where(p => p.Container.Equals(UnitType.NorthWest)).Select(q => Enumerable.Range(leftNorthWest-15, 30).Contains(q.Left) && Enumerable.Range(topNorthWest-20, 40).Contains(q.Top)).ToList().Exists(x => x.Equals(true)))
                {
                    leftNorthWest = r.Next(Map.StartX, (Map.EndX / 2)-15);
                    topNorthWest = r.Next(Map.StartY, (Map.EndY / 2)-20);
                }

                Rescue resc = new Rescue();
                resc.Left = leftNorthWest;
                resc.Top = topNorthWest;
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                //
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                //resc.SteamIDs = new StringBuilder("");
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }

            for (int i = 1; i <= Unit.NorthEastRescueCount; i++)
            {
                leftNorthEast = r.Next(Map.EndX / 2, Map.EndX-15);
                topNorthEast = r.Next(Map.StartY, (Map.EndY / 2)-20);

                while (searchs.Where(p => p.Container.Equals(UnitType.NorthEast)).Select(q => Enumerable.Range(leftNorthEast-30, 30).Contains(q.Left) && Enumerable.Range(topNorthEast-40, 40).Contains(q.Top)).ToList().Exists(x => x.Equals(true)))
                {
                    leftNorthEast = r.Next(Map.EndX / 2, Map.EndX-15);
                    topNorthEast = r.Next(Map.StartY, (Map.EndY / 2)-20);
                }

                Rescue resc = new Rescue();
                resc.Left = leftNorthEast;
                resc.Top = topNorthEast;
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                //
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                //resc.SteamIDs = new StringBuilder("");
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }

            for (int i = 1; i <= Unit.SouthWestRescueCount; i++)
            {
                leftSouthWest = r.Next(Map.StartX, (Map.EndX / 2)-15);
                topSouthWest = r.Next(Map.EndY / 2, Map.EndY-20);

                while (searchs.Where(p => p.Container.Equals(UnitType.SouthWest)).Select(q => Enumerable.Range(leftSouthWest-30, 30).Contains(q.Left) && Enumerable.Range(topSouthWest-40, 40).Contains(q.Top)).ToList().Exists(x => x.Equals(true)))
                {
                    leftSouthWest = r.Next(Map.StartX, (Map.EndX / 2)-15);
                    topSouthWest = r.Next(Map.EndY / 2, Map.EndY-20);
                }

                Rescue resc = new Rescue();
                resc.Left = leftSouthWest;
                resc.Top = topSouthWest;
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                //
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                //resc.SteamIDs = new StringBuilder("");
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }

            for (int i = 1; i <= Unit.SouthEastRescueCount; i++)
            {
                leftSouthEast = r.Next(Map.EndX / 2, Map.EndX-15);
                topSouthEast = r.Next(Map.EndY / 2, Map.EndY-20);

                while (searchs.Where(p => p.Container.Equals(UnitType.SouthEast)).Select(q => Enumerable.Range(leftSouthEast-30, 30).Contains(q.Left) && Enumerable.Range(topSouthEast-40, 40).Contains(q.Top)).ToList().Exists(x => x.Equals(true)))
                {
                    leftSouthEast = r.Next(Map.EndX / 2, Map.EndX-15);
                    topSouthEast = r.Next(Map.EndY / 2, Map.EndY-20);
                }

                Rescue resc = new Rescue();
                resc.Left = leftSouthEast;
                resc.Top = topSouthEast;
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                //
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                //resc.SteamIDs = new StringBuilder("");
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }

            //*****Insert in sql
        }

        void ShowRescueTeam()
        {
            List<TbRescue> tbRescues = new List<TbRescue>();
            int count = 0;
            foreach (var r in rescues)
            {
                //DrawPoint(p);
                panel1.Controls.Add(r.GetShape());
                //***Insert in sql
                TbRescue rescue = new TbRescue();
                rescue.Rescue_ID = r.ID;
                rescue.Rescue_IntID = count++;
                rescue.Rescue_TopCoordinate = r.Top;
                rescue.Rescue_LeftCoordinate = r.Left;
                rescue.Rescue_TopProjection = r.TopProjection;
                rescue.Rescue_LeftProjection = r.LeftProjection;
                rescue.Rescue_Unit = r.Container.ToString();
                rescue.Rescue_State = (int)r.State;
                tbRescues.Add(rescue);
            }
            db.TbRescues.InsertAllOnSubmit(tbRescues);
            db.SubmitChanges();
            geoMap.SendToBack();
            /////
            List<TbRescueFirst> tbRescueFirsts = new List<TbRescueFirst>();
            foreach (var r in tbRescues)
            {
                TbRescueFirst rescue = new TbRescueFirst();
                rescue.Rescue_ID = r.Rescue_ID;
                rescue.Rescue_IntID = r.Rescue_IntID;
                rescue.Rescue_TopCoordinate = r.Rescue_TopCoordinate;
                rescue.Rescue_LeftCoordinate = r.Rescue_LeftCoordinate;
                rescue.Rescue_TopProjection = r.Rescue_TopProjection;
                rescue.Rescue_LeftProjection = r.Rescue_LeftProjection;
                rescue.Rescue_Unit = r.Rescue_Unit;
                rescue.Rescue_State = r.Rescue_State;
                tbRescueFirsts.Add(rescue);
            }
            db.TbRescueFirsts.InsertAllOnSubmit(tbRescueFirsts);
            db.SubmitChanges();
        }
        //**********************//
        
        double GetDistance(int x1, int x2, int y1, int y2)
        {
            //int temp = ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
            //return (int)Math.Sqrt(temp);
            System.Drawing.Point pointStart = new System.Drawing.Point(x1, y1);
            System.Drawing.Point pointEnd = new System.Drawing.Point(x2, y2);
            Coordinate MyCoordinateStart = geoMap.PixelToProj(pointStart);
            Coordinate MyCoordinateEnd = geoMap.PixelToProj(pointEnd);
            return MyCoordinateStart.Distance(MyCoordinateEnd);
        }

        void ShowTime()
        {
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //
            if (!numPointDoRescue.Equals(0))
            {
                if (txtbTime.InvokeRequired)
                    txtbTime.BeginInvoke((MethodInvoker)delegate { txtbTime.Text =Math.Floor(ss.Elapsed.TotalSeconds).ToString(); });
                else
                {
                    txtbTime.Text = Math.Floor(ss.Elapsed.TotalSeconds).ToString();
                }
            }
            else
            {
                timer1.Stop();
                //double averageTimeOfRescue = totalTimeOfDistanceRescue / points.Count();
                double averageTimeOfRs = (double) totalTimeOfDistanceRs / points.Count();
                if (txtAverageTimeOfRescue.InvokeRequired)
                    txtAverageTimeOfRescue.BeginInvoke((MethodInvoker)delegate { txtAverageTimeOfRescue.Text = averageTimeOfRs.ToString(); });
                else
                {
                    txtAverageTimeOfRescue.Text = averageTimeOfRs.ToString();
                }
                //
                double averageEfficiencyBusyRescue = (double) efficiencyBusyRescue / numBusyRescue; //Math.Floor(ss.Elapsed.TotalSeconds);
                if (txtAvarageBusyResc.InvokeRequired)
                    txtAvarageBusyResc.BeginInvoke((MethodInvoker)delegate { txtAvarageBusyResc.Text = (averageEfficiencyBusyRescue).ToString(); });//averageBusyRescue.ToString(); });
                else
                {
                    txtAvarageBusyResc.Text = (averageEfficiencyBusyRescue).ToString(); //averageBusyRescue.ToString();
                }
                //
            }
            //
            if (txtTotalDistanceSearch.InvokeRequired)
                txtTotalDistanceSearch.BeginInvoke((MethodInvoker)delegate { txtTotalDistanceSearch.Text = totalDistanceSearch.ToString(); });
            else
            {
                txtTotalDistanceSearch.Text = totalDistanceSearch.ToString();
            }

            if (txtTotalDistanceRescue.InvokeRequired)
                txtTotalDistanceRescue.BeginInvoke((MethodInvoker)delegate { txtTotalDistanceRescue.Text = totalDistanceRescue.ToString(); });
            else
            {
                txtTotalDistanceRescue.Text = totalDistanceRescue.ToString();
            }
            //
            if (txtIdelRescue.InvokeRequired)
                txtIdelRescue.BeginInvoke((MethodInvoker)delegate { txtIdelRescue.Text = idelRescue.ToString(); });
            else
            {
                txtIdelRescue.Text = idelRescue.ToString();
            }

            if (txtBusyRescue.InvokeRequired)
                txtBusyRescue.BeginInvoke((MethodInvoker)delegate { txtBusyRescue.Text = busyRescue.ToString(); });
            else
            {
                txtBusyRescue.Text = busyRescue.ToString();
            }
            
        }

        void BindDropDownShowInformation()
        {
            List<int> listSearchIntIDs = new List<int>();
            using (var dbSelect = new DataClasses1DataContext())
            {
                var intIDs = (from TbSearchs in dbSelect.TbSearches
                            select TbSearchs.Search_IntID).OrderBy(q=>q).ToList();
                listSearchIntIDs = intIDs;
            }
            cboxSearchIntID.DataSource = listSearchIntIDs;
        }

        void BindDropDownSearchState()
        {
            List<Search.SearchStateType> listSearchState = new List<Search.SearchStateType>();
            listSearchState.Add(Search.SearchStateType.Busy);
            listSearchState.Add(Search.SearchStateType.Ready);
            comboxSearchState.DataSource = listSearchState;
        }

        void BindDropDownRescueLevel()
        {
            List<ERSC.DomainObject.RescueLevelType> listRescueLevel = new List<ERSC.DomainObject.RescueLevelType>();
            listRescueLevel.Add(ERSC.DomainObject.RescueLevelType.High);
            listRescueLevel.Add(ERSC.DomainObject.RescueLevelType.Middel);
            listRescueLevel.Add(ERSC.DomainObject.RescueLevelType.Low);
            listRescueLevel.Add(ERSC.DomainObject.RescueLevelType.None);
            comboxRescueLevel.DataSource = listRescueLevel;
        }

        //************************************************//
        public List<Thread> AssignePointToSearchInNorthWest()
        {
            List<DomainObject.Point> orderdPointsInNorthWest = points.Where(p => p.Container == UnitType.NorthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInNorthWest = searchs.Where(p => p.Container == UnitType.NorthWest).OrderBy(p=>p.IntID).ToList();
            List<Thread> Threads = new List<Thread>();
            //
            foreach (var search in searchInNorthWest)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                //
                foreach (var item in orderdPointsInNorthWest)
                {
                    item.Distance = GetDistance(item.Left, search.Left, item.Top, search.Top);
                    //item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                //
                if (orderdPointsInNorthWest.Count() != 0)
                {
                    List<DomainObject.Point> searchOrderdPointsInNorthWest = orderdPointsInNorthWest.OrderByDescending(p => p.NumVictim).OrderBy(p => p.Distance).ToList();
                    //orderdPointsInNorthWest[indexOfSelectedPoint].StartSearchDoing = true;
                    DomainObject.Point Point = searchOrderdPointsInNorthWest.First();
                    Search selectedSearch = search;
                    Thread northWestThread = new Thread(() => DoSearchForPoint(selectedSearch, Point));
                    Threads.Add(northWestThread);
                    requestedCountThreadInNorthWest++;
                    orderdPointsInNorthWest.Remove(Point);
                }
            }
            isFinish = true;
            //
            return Threads;
        }

        public List<Thread> AssignePointToSearchInNorthEast()
        {
            List<DomainObject.Point> orderdPointsInNorthEast = points.Where(p => p.Container == UnitType.NorthEast).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInNorthEast = searchs.Where(p => p.Container == UnitType.NorthEast).OrderBy(p => p.IntID).ToList();
            int indexOfSelectedPoint = 0;

            List<Thread> Threads = new List<Thread>();
            //
            foreach (var search in searchInNorthEast)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                if (indexOfSelectedPoint < orderdPointsInNorthEast.Count())
                {
                    //orderdPointsInNorthWest[indexOfSelectedPoint].StartSearchDoing = true;
                    DomainObject.Point Point = orderdPointsInNorthEast[indexOfSelectedPoint];
                    Search selectedSearch = search;
                    Thread northWestThread = new Thread(() => DoSearchForPoint(selectedSearch, Point));
                    Threads.Add(northWestThread);
                    indexOfSelectedPoint++;
                    requestedCountThreadInNorthEast++;
                }
            }
            isFinish = true;
            return Threads;
        }

        public List<Thread> AssignePointToSearchInSouthWest()
        {
            List<DomainObject.Point> orderdPointsInSouthWest = points.Where(p => p.Container == UnitType.SouthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInSouthWest = searchs.Where(p => p.Container == UnitType.SouthWest).OrderBy(p => p.IntID).ToList();
            int indexOfSelectedPoint = 0;

            List<Thread> Threads = new List<Thread>();
            //
            foreach (var search in searchInSouthWest)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                if (indexOfSelectedPoint < orderdPointsInSouthWest.Count())
                {
                    //orderdPointsInNorthWest[indexOfSelectedPoint].StartSearchDoing = true;
                    DomainObject.Point Point = orderdPointsInSouthWest[indexOfSelectedPoint];
                    Search selectedSearch = search;
                    Thread northWestThread = new Thread(() => DoSearchForPoint(selectedSearch, Point));
                    Threads.Add(northWestThread);
                    indexOfSelectedPoint++;
                    requestedCountThreadInSouthWest++;
                }
            }
            isFinish = true;
            return Threads;
        }

        public List<Thread> AssignePointToSearchInSouthEast()
        {
            List<DomainObject.Point> orderdPointsInSouthEast = points.Where(p => p.Container == UnitType.SouthEast).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInSouthEast = searchs.Where(p => p.Container == UnitType.SouthEast).OrderBy(p => p.IntID).ToList();
            int indexOfSelectedPoint = 0;

            List<Thread> Threads = new List<Thread>();
            //
            foreach (var search in searchInSouthEast)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                if (indexOfSelectedPoint < orderdPointsInSouthEast.Count())
                {
                    //orderdPointsInNorthWest[indexOfSelectedPoint].StartSearchDoing = true;
                    DomainObject.Point Point = orderdPointsInSouthEast[indexOfSelectedPoint];
                    Search selectedSearch = search;
                    Thread northWestThread = new Thread(() => DoSearchForPoint(selectedSearch, Point));
                    Threads.Add(northWestThread);
                    indexOfSelectedPoint++;
                    requestedCountThreadInSouthEast++;
                }
            }
            isFinish = true;
            return Threads;
        }
        //*************************************************//
        public void DoSearchForPoint(Search selectedSearch, DomainObject.Point selectedPoint)
        {
            DomainObject.Point nextPoint = null;
            lock (selectedSearch)
            {
                //re = new ManualResetEvent(true);
                //re.WaitOne();
                selectedSearch.IsDoing = true;
                selectedPoint.IsAllocatedSTeam = true;
                selectedPoint.StartSearchDoing = true;
                selectedPoint.STeamID = selectedSearch.ID;
                selectedSearch.PointID = selectedPoint.ID;
                selectedSearch.State = Search.SearchStateType.Busy;
                //Update And Insert to sql
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    //Update to sql in TbPoint
                    var point = (from TbPoints in dbUpdate.TbPoints
                                 where TbPoints.Point_ID == selectedPoint.ID
                                 select TbPoints).Single();
                    point.Point_IsAllocatedSTeam = selectedPoint.IsAllocatedSTeam;
                    point.Point_StartSearchDoing = selectedPoint.StartSearchDoing;
                    dbUpdate.SubmitChanges();

                    //Update to sql in TbSearch
                    var search = (from TbSearchs in dbUpdate.TbSearches
                                  where TbSearchs.Search_ID == selectedSearch.ID
                                  select TbSearchs).Single();
                    search.Search_State = (int)Search.SearchStateType.Busy;
                    dbUpdate.SubmitChanges();

                    //Insert to sql in TbTaskList
                    TbTaskList task = new TbTaskList();
                    task.ID = Guid.NewGuid().ToString();
                    task.Search_ID = selectedPoint.STeamID;
                    task.Point_ID = selectedPoint.ID;
                    task.Point_ParentID = selectedPoint.ParentID;
                    task.Rescue_ID = null;
                    task.Probability = 0;
                    task.Priority = 0;
                    task.IsAssigned = false;
                    dbUpdate.TbTaskLists.InsertOnSubmit(task);
                    dbUpdate.SubmitChanges();
                }

                int searchTop, searchLeft, pointTop, pointLeft;
                searchTop = selectedSearch.Top;
                searchLeft = selectedSearch.Left;
                pointTop = selectedPoint.Top;
                pointLeft = selectedPoint.Left;

                Label lblSearch = (Label)panel1.Controls.Find(selectedSearch.ID, true)[0];
                Label lblPoint = (Label)panel1.Controls.Find(selectedPoint.ID, true)[0];

                int xDistance = Math.Abs(lblSearch.Location.X - lblPoint.Location.X);
                int yDistance = Math.Abs(lblSearch.Location.Y - lblPoint.Location.Y);

                int total = 10;
                int xpart = xDistance / total;
                int ypart = yDistance / total;
                int totalTimeSleep = 0;
                int newCordinationX = 0;
                int newCordinationY = 0;

                for (int i = 0; i < total; i++)
                {
                    System.Threading.Thread.Sleep(int.Parse(nudSearch.Value.ToString()) * 30);
                    totalTimeSleep += (int.Parse(nudSearch.Value.ToString()) * 30);

                    if (lblSearch.Location.X < lblPoint.Location.X && lblSearch.Location.Y < lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblSearch.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblSearch.Location.Y))
                        {
                            newCordinationX = lblSearch.Location.X + 5;
                            newCordinationY = lblSearch.Location.Y + 5;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X + xpart;
                            newCordinationY = lblSearch.Location.Y + ypart;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    //southwest
                    else if (lblSearch.Location.X < lblPoint.Location.X && lblSearch.Location.Y > lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblSearch.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblSearch.Location.Y))
                        {
                            newCordinationX = lblSearch.Location.X + 5;
                            newCordinationY = lblSearch.Location.Y - 5;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X + xpart;
                            newCordinationY = lblSearch.Location.Y - ypart;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    //northeast
                    else if (lblSearch.Location.X > lblPoint.Location.X && lblSearch.Location.Y > lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblSearch.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblSearch.Location.Y))
                        {
                            newCordinationX = lblSearch.Location.X -5;
                            newCordinationY = lblSearch.Location.Y - 5;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X - xpart;
                            newCordinationY = lblSearch.Location.Y - ypart;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblSearch.Location.X > lblPoint.Location.X && lblSearch.Location.Y < lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblSearch.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblSearch.Location.Y))
                        {
                            newCordinationX = lblSearch.Location.X - 5;
                            newCordinationY = lblSearch.Location.Y + 5;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X - xpart;
                            newCordinationY = lblSearch.Location.Y + ypart;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblSearch.Location.X == lblPoint.Location.X && lblSearch.Location.Y < lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblSearch.Location.Y))
                        {
                            newCordinationX = lblSearch.Location.X;
                            newCordinationY = lblSearch.Location.Y + 5;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X;
                            newCordinationY = lblSearch.Location.Y + ypart;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblSearch.Location.X == lblPoint.Location.X && lblSearch.Location.Y > lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblSearch.Location.Y))
                        {
                            newCordinationX = lblSearch.Location.X;
                            newCordinationY = lblSearch.Location.Y - 5;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X;
                            newCordinationY = lblSearch.Location.Y - ypart;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblSearch.Location.X > lblPoint.Location.X && lblSearch.Location.Y == lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblSearch.Location.X))
                        {
                            newCordinationX = lblSearch.Location.X - 5;
                            newCordinationY = lblSearch.Location.Y;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X - xpart;
                            newCordinationY = lblSearch.Location.Y;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblSearch.Location.X < lblPoint.Location.X && lblSearch.Location.Y == lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblSearch.Location.X))
                        {
                            newCordinationX = lblSearch.Location.X + 5;
                            newCordinationY = lblSearch.Location.Y;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblSearch.Location.X - xpart;
                            newCordinationY = lblSearch.Location.Y;
                            if (lblSearch.InvokeRequired)
                                lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblSearch.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }

                    if (lblSearch.InvokeRequired)
                        lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.BringToFront(); });
                    else
                        lblSearch.BringToFront();

                    //
                    System.Drawing.Point MyPoint = new System.Drawing.Point(newCordinationX, newCordinationY);
                    DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                    //

                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        //Update to sql in TbPoint
                        var search = (from TbSearchs in dbUpdate.TbSearches
                                      where TbSearchs.Search_ID == selectedSearch.ID
                                      select TbSearchs).Single();
                        search.Search_LeftCoordinate = newCordinationX;
                        search.Search_TopCoordinate = newCordinationY;
                        search.Search_LeftProjection = MyCoordinate.X;
                        search.Search_TopProjection = MyCoordinate.Y;
                        dbUpdate.SubmitChanges();
                    }
                }

                totalDistanceSearch += GetDistance(pointTop, searchTop, pointLeft, searchLeft);
              
                //
                selectedSearch.ListPoint.Add(selectedPoint);
                //***** Do Search
                selectedPoint.DoSearch();

                selectedPoint.StartSearchDoing = false;
                selectedPoint.EndSearchDoing = true;
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    //Update to sql in TbPoint
                    var point = (from TbPoints in dbUpdate.TbPoints
                                 where TbPoints.Point_ID == selectedPoint.ID
                                 select TbPoints).Single();
                    point.Point_StartSearchDoing = selectedPoint.StartSearchDoing;
                    point.Point_EndSearchDoing = selectedPoint.EndSearchDoing;
                    dbUpdate.SubmitChanges();
                }
                //***** Show new information && Update sql
                int countNewPoint= ShowNewInformation(selectedPoint);
                //
                lblPoint.Image = Properties.Resources.Point_Blue;
                lock (this)
                {
                    List<string> idOfNewPoints = new List<string>();
                    using (var dbSelect = new DataClasses1DataContext())
                    {
                        idOfNewPoints = (from TbPoints in dbSelect.TbPoints
                                         where TbPoints.Point_ParentID.Equals(selectedPoint.ID)
                                         select TbPoints.Point_ID).ToList();
                    }

                    for (int i = 0; i < idOfNewPoints.Count(); i++)
                    {
                        Label myLabel = (Label)panel1.Controls.Find(idOfNewPoints[i], true)[0];
                        myLabel.Image = Properties.Resources.Point_Blue;
                    }
                }
                //*****
                int time = (totalTimeSleep / 1000) + 2;
                ShowChartTasksOfSearch(time);
                numPointDoRescue = numPointDoRescue + countNewPoint;
                ShowChartTasksOfRescue(time);
                //*****
                int searchIdForChart = 0;
                if (cboxSearchIntID.InvokeRequired)
                    cboxSearchIntID.BeginInvoke((MethodInvoker)delegate
                    {
                        searchIdForChart = (int)cboxSearchIntID.SelectedItem;
                    });
                else
                {
                    searchIdForChart = (int)cboxSearchIntID.SelectedItem;
                }
                //*****
                DecisionAlgorithmForSelectWinRescue(selectedSearch, selectedPoint, countNewPoint, searchIdForChart);
                //
                selectedSearch.State = Search.SearchStateType.Ready;
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    //Update to sql 
                    var search = (from TbSearchs in dbUpdate.TbSearches
                                  where TbSearchs.Search_ID == selectedSearch.ID
                                  select TbSearchs).Single();
                    search.Search_State = (int)Search.SearchStateType.Ready;
                    dbUpdate.SubmitChanges();
                }
                //***** Select Next Point For Search Team
                nextPoint = SelectNextPointForSearch(selectedSearch);
            }

            switch (selectedSearch.Container)
            {
                case DomainObject.UnitType.NorthWest:
                    lock (this)
                    {
                        currentCountThreadInNorthWest++;
                        if (currentCountThreadInNorthWest < requestedCountThreadInNorthWest)
                        {
                            Monitor.Wait(this);
                        }
                        Monitor.PulseAll(this);
                    }
                    currentCountThreadInNorthWest = 0;
                    break;
                case DomainObject.UnitType.NorthEast:
                    lock (this)
                    {
                        currentCountThreadInNorthEast++;
                        if (currentCountThreadInNorthEast < requestedCountThreadInNorthEast)
                        {
                            Monitor.Wait(this);
                        }
                        Monitor.PulseAll(this);
                    }
                    currentCountThreadInNorthEast = 0;
                    break;
                case DomainObject.UnitType.SouthWest:
                    lock (this)
                    {
                        currentCountThreadInSouthWest++;
                        if (currentCountThreadInSouthWest < requestedCountThreadInSouthWest)
                        {
                            Monitor.Wait(this);
                        }
                        Monitor.PulseAll(this);
                    }
                    currentCountThreadInSouthWest = 0;
                    break;
                case DomainObject.UnitType.SouthEast:
                    lock (this)
                    {
                        currentCountThreadInSouthEast++;
                        if (currentCountThreadInSouthEast < requestedCountThreadInSouthEast)
                        {
                            Monitor.Wait(this);
                        }
                        Monitor.PulseAll(this);
                    }
                    currentCountThreadInSouthEast = 0;
                    break;
            }
           
            if (nextPoint != null)
            {
                DoSearchForPoint(selectedSearch, nextPoint);
            }
            else
            {
                switch (selectedSearch.Container)
                {
                    case  DomainObject.UnitType.NorthWest:
                        requestedCountThreadInNorthWest--;
                        break;
                    case DomainObject.UnitType.NorthEast:
                        requestedCountThreadInNorthEast--;
                        break;
                    case DomainObject.UnitType.SouthWest:
                        requestedCountThreadInSouthWest--;
                        break;
                    case DomainObject.UnitType.SouthEast:
                        requestedCountThreadInSouthEast--;
                        break;
                }
            } 

            selectedSearch.IsDoing = false;
        }
        //*************************************************//
        public void UpdateData()
        {
        }
        //***********************************************//
        public void ShowChartTasksOfSearch(int timeSleep)
        {
            lock (this)
            {
                numPointDoSearch--;
                int time=0;
                if (chartTasksOfSearch.InvokeRequired)
                    chartTasksOfSearch.BeginInvoke((MethodInvoker)delegate
                    {
                        //chartTasksOfSearch.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Milliseconds;
                        //chartTasksOfSearch.ChartAreas[0].AxisX.Interval = 5;
                        //chartTasksOfSearch.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                        //time = ss.Elapsed.Seconds - timeSleep;
                        time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                        seriesTasksOfSearch.Points.AddXY(time, numPointDoSearch);
                        //seriesTasksOfSearch.Points.AddXY(ss.Elapsed.Seconds, numPointDoSearch);
                        seriesTasksOfSearch.ChartType = SeriesChartType.Line;
                        //seriesTasksOfSearch.IsVisibleInLegend = true;
                        //seriesTasksOfSearch.IsXValueIndexed = true;
                        chartTasksOfSearch.Invalidate();
                    });
                else
                {
                    //chartTasksOfSearch.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Milliseconds;
                    //chartTasksOfSearch.ChartAreas[0].AxisX.Interval = 5;
                    //time = ss.Elapsed.Seconds - timeSleep;
                    time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                    seriesTasksOfSearch.Points.AddXY(time, numPointDoSearch);
                    //seriesTasksOfSearch.Points.AddXY(ss.Elapsed.Seconds, numPointDoSearch);
                    seriesTasksOfSearch.ChartType = SeriesChartType.Line;
                    //seriesTasksOfSearch.IsVisibleInLegend = true;
                    //seriesTasksOfSearch.IsXValueIndexed = true;
                    chartTasksOfSearch.Invalidate();
                }
            }
        }

        public void ShowChartTasksOfRescue(int timeSleep)
        {
            lock (this)
            {
                int time = 0;
                if (chartTasksOfRescue.InvokeRequired)
                    chartTasksOfRescue.BeginInvoke((MethodInvoker)delegate
                    {
                        //chartTasksOfSearch.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Milliseconds;
                        //chartTasksOfSearch.ChartAreas[0].AxisX.Interval = 5;
                        //chartTasksOfSearch.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                        //time = ss.Elapsed.Seconds - timeSleep;
                        time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                        seriesTasksOfRescue.Points.AddXY(time, numPointDoRescue);
                        //seriesTasksOfSearch.Points.AddXY(ss.Elapsed.Seconds, numPointDoSearch);
                        seriesTasksOfRescue.ChartType = SeriesChartType.Line;
                        //seriesTasksOfSearch.IsVisibleInLegend = true;
                        //seriesTasksOfSearch.IsXValueIndexed = true;
                        chartTasksOfRescue.Invalidate();
                    });
                else
                {
                    //chartTasksOfSearch.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Milliseconds;
                    //chartTasksOfSearch.ChartAreas[0].AxisX.Interval = 5;
                    //time = ss.Elapsed.Seconds - timeSleep;
                    time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                    seriesTasksOfRescue.Points.AddXY(time, numPointDoRescue);
                    //seriesTasksOfSearch.Points.AddXY(ss.Elapsed.Seconds, numPointDoSearch);
                    seriesTasksOfRescue.ChartType = SeriesChartType.Line;
                    //seriesTasksOfSearch.IsVisibleInLegend = true;
                    //seriesTasksOfSearch.IsXValueIndexed = true;
                    chartTasksOfRescue.Invalidate();
                }
            }
        }
        //***********************************************//

        public void ShowChartProbabilityForSelectedSearch(int counter, Dictionary<string, double> dicOfProbabilityRs,int searchIdForChart)
        {
            int k = 0;
            
            k = 0;
            if (chartProbability.InvokeRequired)
                chartProbability.BeginInvoke((MethodInvoker)delegate
                {
                    chartProbability.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p=>p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries[k].Points.AddXY(counter, rescue.Value);
                        listSeries[k].ChartType = SeriesChartType.Spline;
                        //chartProbability.Invalidate();
                        k++;
                    }
                });
            else
            {
            }
        }

        public void ShowChartProbabilityForSelectedSearch_S1(int counter, Dictionary<string, double> dicOfProbabilityRs, int searchIdForChart)
        {
            int k = 0;
            k = 0;
            if (chartProbability_S1.InvokeRequired)
                chartProbability_S1.BeginInvoke((MethodInvoker)delegate
                {
                    chartProbability_S1.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p => p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries_S1[k].Points.AddXY(counter, rescue.Value);
                        listSeries_S1[k].ChartType = SeriesChartType.Spline;
                        //chartProbability.Invalidate();
                        k++;
                    }
                });
            else
            {
            }
        }

        public void ShowChartProbabilityForSelectedSearch_S2(int counter, Dictionary<string, double> dicOfProbabilityRs, int searchIdForChart)
        {
            int k = 0;
            k = 0;
            if (chartProbability_S2.InvokeRequired)
                chartProbability_S2.BeginInvoke((MethodInvoker)delegate
                {
                    chartProbability_S2.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p => p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries_S2[k].Points.AddXY(counter, rescue.Value);
                        listSeries_S2[k].ChartType = SeriesChartType.Spline;
                        //chartProbability.Invalidate();
                        k++;
                    }
                });
            else
            {
            }
        }

        public void ShowChartProbabilityForSelectedSearch_S3(int counter, Dictionary<string, double> dicOfProbabilityRs, int searchIdForChart)
        {
            int k = 0;
            k = 0;
            if (chartProbability_S3.InvokeRequired)
                chartProbability_S3.BeginInvoke((MethodInvoker)delegate
                {
                    chartProbability_S3.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p => p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries_S3[k].Points.AddXY(counter, rescue.Value);
                        listSeries_S3[k].ChartType = SeriesChartType.Spline;
                        //chartProbability.Invalidate();
                        k++;
                    }
                });
            else
            {
            }
        }

        //***********************************************//
        public int ShowNewInformation(DomainObject.Point selectedPoint)
        {
            List<DomainObject.Point> listOfNewPoints = new List<DomainObject.Point>();
            List<DomainObject.Point> listOfNewPointsForInsert = new List<DomainObject.Point>();

            switch (selectedPoint.NumVictim.Equals(0))
            {
                case true:
                    selectedPoint.State = DomainObject.StateType.Finish;
                    selectedPoint.RescueLevel = 0;
                    break;
                case false:
                    selectedPoint.State = DomainObject.StateType.Rescue;
                    switch (selectedPoint.CreatPoint.Equals(0))
                    {
                        case true:
                            break;
                        case false:
                            listOfNewPoints = CreateNewPoints(selectedPoint.CreatPoint, selectedPoint);
                            listOfNewPoints = ShowNewPoints(listOfNewPoints);
                            //
                            lock (this)
                            {
                                foreach (DomainObject.Point newPoint in listOfNewPoints)
                                {
                                    newPoints.Add(newPoint);
                                }
                            }
                            //
                            break;
                    }
                    break;
            }
            using (var dbUpdate = new DataClasses1DataContext())
            {
                //Update to sql
                var point = (from TbPoints in dbUpdate.TbPoints
                             where TbPoints.Point_ID == selectedPoint.ID
                             select TbPoints).Single();
                point.Point_State = (int)selectedPoint.State;
                point.Point_RescueLevel = (int)selectedPoint.RescueLevel;
                //db.SubmitChanges();

                dbUpdate.SubmitChanges();
            }
            //Insert New Point to sql
            listOfNewPointsForInsert = listOfNewPoints;
            if (listOfNewPointsForInsert.Count() > 0)
            {
                List<TbPoint> tbNewPoints = new List<TbPoint>();
                int maxIntID = 0;
                lock (this)
                {
                    using (var dbSelect = new DataClasses1DataContext())
                    {
                        maxIntID = dbSelect.TbPoints.Max(p => p.Point_IntID);
                    }

                    foreach (var p in listOfNewPointsForInsert)
                    {
                        //***Insert in sql
                        TbPoint newP = new TbPoint();
                        //
                        newP.Point_ID = p.ID;
                        newP.Point_IntID = maxIntID++;
                        newP.Point_ParentID = p.ParentID;
                        newP.Point_TopCoordinate = p.Top;
                        newP.Point_LeftCoordinate = p.Left;
                        newP.Point_TopProjection = p.TopProjection;
                        newP.Point_LeftProjection = p.LeftProjection;
                        newP.Poin_Unit = p.Container.ToString();
                        newP.Point_NumVictim = p.NumVictim;
                        newP.Point_TimeToBeDone = p.TimeToBeDone;
                        newP.Point_RescueLevel = (int)p.RescueLevel;
                        newP.Point_CreatPoint = p.CreatPoint;
                        newP.Point_State = (int)p.State;
                        newP.Point_IsAllocatedSTeam = p.IsAllocatedSTeam;
                        newP.Point_IsAllocatedRTeam = p.IsAllocatedRTeam;
                        newP.Point_StartSearchDoing = p.StartSearchDoing;
                        newP.Point_StartRescueDoing = p.StartRescueDoing;
                        newP.Point_EndSearchDoing = p.EndSearchDoing;
                        newP.Point_EndRescueDoing = p.EndRescueDoing;
                        //
                        tbNewPoints.Add(newP);
                    }
                }
                using (var dbInsert = new DataClasses1DataContext())
                {
                    dbInsert.TbPoints.InsertAllOnSubmit(tbNewPoints);
                    dbInsert.SubmitChanges();
                }

            }

            return listOfNewPoints.Count();
        }

        public List<DomainObject.Point> CreateNewPoints(int numPoints,DomainObject.Point parentPoint)
        {
            List<DomainObject.Point> newPoints=new List<DomainObject.Point>();
            Random r = new Random();
            for (int i = 1; i <= numPoints; i++)
            {
                //DomainObject.Point p = new DomainObject.Point(r.Next(1, 4));
                DomainObject.Point p = new DomainObject.Point(2,true);
                switch (i)
                {
                    case 1:
                        p.Left = parentPoint.Left-10;
                        p.Top = parentPoint.Top;
                        break;
                    case 2:
                        p.Left = parentPoint.Left+10;
                        p.Top = parentPoint.Top;
                        break;
                    case 3:
                        p.Left = parentPoint.Left;
                        p.Top = parentPoint.Top-10;
                        break;
                    case 4:
                        p.Left = parentPoint.Left;
                        p.Top = parentPoint.Top + 10;
                        break;
                }
                //convert coordinates to Projection
                System.Drawing.Point MyPoint = new System.Drawing.Point(p.Left, p.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                p.LeftProjection = MyCoordinate.X;
                p.TopProjection = MyCoordinate.Y;
                //
                p.Container = parentPoint.Container;
                //
                p.ParentID = parentPoint.ID;
                p.NumVictim = r.Next(DomainObject.Point.minNumVictim, DomainObject.Point.maxNumVictim);
                p.RescueLevel = DomainObject.Point.GetRescueLevel(p.NumVictim);
                p.CreatPoint = 0;
                p.State = DomainObject.StateType.Search;
                p.ParentID = parentPoint.ID;
                p.IsAllocatedSTeam = true;
                p.StartSearchDoing = false;
                p.EndSearchDoing = true;
                
                //
                if (points.Where(pt => pt.Top == p.Top && pt.Left == p.Left).Count() > 0)
                {
                    throw new Exception("");
                }
                //
                //points.Add(p); ????? // Collection was modified; enumeration operation may not execute
                newPoints.Add(p);
            }
            return newPoints; 
        }

        public List<DomainObject.Point> ShowNewPoints(List<DomainObject.Point> newPoints)
        {
            List<DomainObject.Point> nPoints = new List<DomainObject.Point>();
            lock (newPoints)
            {
                if (panel1.InvokeRequired)
                    panel1.BeginInvoke((MethodInvoker)delegate { foreach (var p in newPoints) { panel1.Controls.Add(p.GetShape()); nPoints.Add(p); } });
                else
                    foreach (var p in newPoints) { panel1.Controls.Add(p.GetShape()); nPoints.Add(p); }
            }

            if (geoMap.InvokeRequired)
                geoMap.BeginInvoke((MethodInvoker)delegate { geoMap.SendToBack(); });
            else
                geoMap.SendToBack();

            return nPoints;
        }
        //***********************************************//
        public void DecisionAlgorithmForSelectWinRescue(Search search, DomainObject.Point point, int countNewPoint, int searchIdForChart)
        {
            Dictionary<string, double> dicOfProbabilityRsInNorthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbabilityRsInNorthEast = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbabilityRsInSouthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbabilityRsInSouthEast = new Dictionary<string, double>();
            //
            Dictionary<string, double> dicOfDistanceRsInNorthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfDistanceRsInNorthEast = new Dictionary<string, double>();
            Dictionary<string, double> dicOfDistanceRsInSouthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfDistanceRsInSouthEast = new Dictionary<string, double>();
            //
            string winRescueID = "";
            double winProbability = 0;
            bool isWin = false;
            //
            switch (point.Container)
            {
                case DomainObject.UnitType.NorthWest:
                    dicOfDistanceRsInNorthWest = ComputeDistanceForRescuesInNorthWest(point);
                    dicOfProbabilityRsInNorthWest = ComputeProbabilityForRescuesInNorthWest(point);
                    winRescueID = SelectWinRescueInNorthWestByAutomataLearning(search, point, dicOfProbabilityRsInNorthWest, dicOfDistanceRsInNorthWest, searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point,countNewPoint,isWin);
                    break;
                case DomainObject.UnitType.NorthEast:
                    dicOfDistanceRsInNorthEast = ComputeDistanceForRescuesInNorthEast(point);
                    dicOfProbabilityRsInNorthEast = ComputeProbabilityForRescuesInNorthEast(point);
                    winRescueID = SelectWinRescueInNorthEastByAutomataLearning(search, point, dicOfProbabilityRsInNorthEast, dicOfDistanceRsInNorthEast,searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point, countNewPoint,isWin);
                    break;
                case DomainObject.UnitType.SouthWest:
                    dicOfDistanceRsInSouthWest = ComputeDistanceForRescuesInSouthWest(point);
                    dicOfProbabilityRsInSouthWest = ComputeProbabilityForRescuesInSouthWest(point);
                    winRescueID = SelectWinRescueInSouthWestByAutomataLearning(search, point, dicOfProbabilityRsInSouthWest, dicOfDistanceRsInSouthWest,searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point, countNewPoint,isWin);
                    break;
                case DomainObject.UnitType.SouthEast:
                    dicOfDistanceRsInSouthEast = ComputeDistanceForRescuesInSouthEast(point);
                    dicOfProbabilityRsInSouthEast = ComputeProbabilityForRescuesInSouthEast(point);
                    winRescueID = SelectWinRescueInSouthEastByAutomataLearning(search, point, dicOfProbabilityRsInSouthEast, dicOfDistanceRsInSouthEast,searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point, countNewPoint,isWin);
                    break;
            }
        }
        //***********************************************//
        public Dictionary<string, double> ComputeProbabilityForRescuesInNorthWest(DomainObject.Point point)
        {
            List<Rescue> rescuesInNorthWest = new List<Rescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            rescuesInNorthWest = rescues.Where(p => p.Container == UnitType.NorthWest).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInNorthWest)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.ID, probability);
                sumAllPropability += probability;
            }
            //
            double sum = 0;
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
                sum += (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeProbabilityForRescuesInNorthWest(TbPoint point)
        {
            List<TbRescue> rescuesInNorthWest = new List<TbRescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            using (var dbSelect = new DataClasses1DataContext())
            {
                rescuesInNorthWest = (from TbRescues in dbSelect.TbRescues
                                where TbRescues.Rescue_Unit.ToLower().Equals(UnitType.NorthWest.ToString().ToLower()) //&& TbRescues.Rescue_State.Equals((int) Rescue.RescueStateType.Ready)
                                select TbRescues).ToList();
            }
            foreach (var rescue in rescuesInNorthWest)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.Rescue_ID, probability);
                sumAllPropability += probability;
            }
            //
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInNorthWest(DomainObject.Point point)
        {
            List<Rescue> rescuesInNorthWest = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInNorthWest = rescues.Where(p => p.Container == UnitType.NorthWest).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInNorthWest)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInNorthWest(TbPoint point)
        {
            List<Rescue> rescuesInNorthWest = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInNorthWest = rescues.Where(p => p.Container == UnitType.NorthWest).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInNorthWest)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }
        //
        public Dictionary<string, double> ComputeProbabilityForRescuesInNorthEast(DomainObject.Point point)
        {
            List<Rescue> rescuesInNorthEast = new List<Rescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            rescuesInNorthEast = rescues.Where(p => p.Container == UnitType.NorthEast).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInNorthEast)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.ID, probability);
                sumAllPropability += probability;
            }
            //
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeProbabilityForRescuesInNorthEast(TbPoint point)
        {
            List<TbRescue> rescuesInNorthEast = new List<TbRescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            using (var dbSelect = new DataClasses1DataContext())
            {
                rescuesInNorthEast = (from TbRescues in dbSelect.TbRescues
                                      where TbRescues.Rescue_Unit.ToLower().Equals(UnitType.NorthEast.ToString().ToLower()) //&& TbRescues.Rescue_State.Equals((int)Rescue.RescueStateType.Ready)
                                      select TbRescues).ToList();
            }
            foreach (var rescue in rescuesInNorthEast)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.Rescue_ID, probability);
                sumAllPropability += probability;
            }
            //
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInNorthEast(DomainObject.Point point)
        {
            List<Rescue> rescuesInNorthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInNorthEast = rescues.Where(p => p.Container == UnitType.NorthEast).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInNorthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInNorthEast(TbPoint point)
        {
            List<Rescue> rescuesInNorthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInNorthEast = rescues.Where(p => p.Container == UnitType.NorthEast).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInNorthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }
        //
        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthWest(DomainObject.Point point)
        {
            List<Rescue> rescuesInSouthWest = new List<Rescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            rescuesInSouthWest = rescues.Where(p => p.Container == UnitType.SouthWest).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInSouthWest)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.ID, probability);
                sumAllPropability += probability;
            }
            //
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthWest(TbPoint point)
        {
            List<TbRescue> rescuesInSouthWest = new List<TbRescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            using (var dbSelect = new DataClasses1DataContext())
            {
                rescuesInSouthWest = (from TbRescues in dbSelect.TbRescues
                                      where TbRescues.Rescue_Unit.ToLower().Equals(UnitType.SouthWest.ToString().ToLower()) //&& TbRescues.Rescue_State.Equals((int)Rescue.RescueStateType.Ready)
                                      select TbRescues).ToList();
            }
            foreach (var rescue in rescuesInSouthWest)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.Rescue_ID, probability);
                sumAllPropability += probability;
            }
            //
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInSouthWest(DomainObject.Point point)
        {
            List<Rescue> rescuesInSouthWest = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInSouthWest = rescues.Where(p => p.Container == UnitType.SouthWest).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInSouthWest)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInSouthWest(TbPoint point)
        {
            List<Rescue> rescuesInSouthWest = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInSouthWest = rescues.Where(p => p.Container == UnitType.SouthWest).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInSouthWest)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }
        //
        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthEast(DomainObject.Point point)
        {
            List<Rescue> rescuesInSouthEast = new List<Rescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            rescuesInSouthEast = rescues.Where(p => p.Container == UnitType.SouthEast).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            //
            foreach (var rescue in rescuesInSouthEast)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.ID, probability);
                sumAllPropability += probability;
            }
            //
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthEast(TbPoint point)
        {
            List<TbRescue> rescuesInSouthEast = new List<TbRescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            //
            using (var dbSelect = new DataClasses1DataContext())
            {
                rescuesInSouthEast = (from TbRescues in dbSelect.TbRescues
                                      where TbRescues.Rescue_Unit.ToLower().Equals(UnitType.SouthEast.ToString().ToLower()) // && TbRescues.Rescue_State.Equals((int) Rescue.RescueStateType.Ready)
                                      select TbRescues).ToList();
            }
            //
            foreach (var rescue in rescuesInSouthEast)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.Rescue_ID, probability);
                sumAllPropability += probability;
            }
            //
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            //
            return dicOfProbability;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInSouthEast(DomainObject.Point point)
        {
            List<Rescue> rescuesInSouthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInSouthEast = rescues.Where(p => p.Container == UnitType.SouthEast).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInSouthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }

        public Dictionary<string, double> ComputeDistanceForRescuesInSouthEast(TbPoint point)
        {
            List<Rescue> rescuesInSouthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            //
            rescuesInSouthEast = rescues.Where(p => p.Container == UnitType.SouthEast).ToList(); //&& p.State.Equals(Rescue.RescueStateType.Ready)
            foreach (var rescue in rescuesInSouthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            //
            return dicOfDistance;
        }

        //****************//
        public double ComputeMaxProbabilityForAnotherRescuesInNorthWest(TbPoint point, string winRescueID)
        {
            List<TbRescue> remainRescue = new List<TbRescue>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            double maxProbability;

            dicOfProbability = ComputeProbabilityForRescuesInNorthWest(point);
            dicOfProbability.Remove(winRescueID);
            maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }

        public double ComputeMaxProbabilityForAnotherRescuesInNorthEast(TbPoint point, string winRescueID)
        {
            List<TbRescue> remainRescue = new List<TbRescue>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            double maxProbability;

            dicOfProbability = ComputeProbabilityForRescuesInNorthEast(point);
            dicOfProbability.Remove(winRescueID);
            maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }

        public double ComputeMaxProbabilityForAnotherRescuesInSouthWest(TbPoint point, string winRescueID)
        {
            List<TbRescue> remainRescue = new List<TbRescue>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            double maxProbability;

            dicOfProbability = ComputeProbabilityForRescuesInSouthWest(point);
            dicOfProbability.Remove(winRescueID);
            maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }

        public double ComputeMaxProbabilityForAnotherRescuesInSouthEast(TbPoint point, string winRescueID)
        {
            List<TbRescue> remainRescue = new List<TbRescue>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            double maxProbability;

            dicOfProbability = ComputeProbabilityForRescuesInSouthEast(point);
            dicOfProbability.Remove(winRescueID);
            maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }
        //***************//
        public double ComputeRewardForPoint(DomainObject.Point point)
        {
            double reward = (int)point.RescueLevel * 10 + point.TimeToBeDone * 70;
            return reward;
        }

        public double ComputeRewardForPoint(TbPoint point)
        {
            double reward = (int)point.Point_RescueLevel * 10 + point.Point_TimeToBeDone * 43;
            return reward;
        }

        public double ComputeCostRescueForPoint(DomainObject.Point point,Rescue rescue)
        {
            
            double cost = GetDistance(point.Left, rescue.Left, point.Top, rescue.Top) / 5; //****** [Time=Distance/Speed 5(m/s)]
            return cost;
            
        }

        public double ComputeCostRescueForPoint(TbPoint point, TbRescue rescue)
        {
            
            double cost = GetDistance(point.Point_LeftCoordinate, rescue.Rescue_LeftCoordinate, point.Point_TopCoordinate, rescue.Rescue_TopCoordinate) /5;
            return cost;
            
        }

        public double ComputeDistanceRescueForPoint(DomainObject.Point point, Rescue rescue)
        {
            double distance = GetDistance(point.Left, rescue.Left, point.Top, rescue.Top);
            return distance;
        }

        public double ComputeDistanceRescueForPoint(TbPoint point, Rescue rescue)
        {
            double distance = GetDistance(point.Point_LeftCoordinate, rescue.Left, point.Point_TopCoordinate, rescue.Top);
            return distance;
        }
        //**********************************************//
        public bool IsBestRescueForAnotherSearchInNorthWest(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = new Dictionary<string, double>();
            dicOfDistanceRs = ComputeDistanceForRescuesInNorthWest(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        public bool IsBestRescueForAnotherSearchInNorthEast(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = new Dictionary<string, double>();
            dicOfDistanceRs = ComputeDistanceForRescuesInNorthEast(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        public bool IsBestRescueForAnotherSearchInSouthWest(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = new Dictionary<string, double>();
            dicOfDistanceRs = ComputeDistanceForRescuesInSouthWest(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        public bool IsBestRescueForAnotherSearchInSouthEast(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = new Dictionary<string, double>();
            dicOfDistanceRs = ComputeDistanceForRescuesInSouthEast(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        //**********************************************// 
        public string SelectWinRescueInNorthWestByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs,int searchIdForChart, out double winProbability, out bool isWin)
        {
            int i = 0;
            string winRescueID = "";
            double winRescueProbability = 0.0;
            isWin = false;
            winProbability = 0;
            int counter = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInNorthWestCopy=0;
            //
            double start = ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInNorthWest = new List<string>();
            busyNorthWestRs = 0;
            //
            while (counter <= 25)
            {
                //**********************************//
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
                //***********//
                if (search.IntID == 1)
                {
                    ShowChartProbabilityForSelectedSearch_S1(counter, dicOfProbabilityRs, 1);
                }
                if (search.IntID == 2)
                {
                    ShowChartProbabilityForSelectedSearch_S2(counter, dicOfProbabilityRs, 2);
                }
                if (search.IntID == 3)
                {
                    ShowChartProbabilityForSelectedSearch_S3(counter, dicOfProbabilityRs, 3);
                }
                //*********************************//
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    //
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInSouthEast.Remove(search.ID);
                            dicIsGivedBestSelectInSouthEast.Add(search.ID, true);
                        }
                    }
                    //
                    ////Update from sql in TbTaskList
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    //
                    lock (syncLockBeforStartInNorthWest)
                    {
                        currentCountThreadInNorthWestBeforStart++;
                        if (currentCountThreadInNorthWestBeforStart < requestedCountThreadInNorthWest)
                        {
                            Monitor.Wait(syncLockBeforStartInNorthWest);
                        }
                        Monitor.PulseAll(syncLockBeforStartInNorthWest);
                    }
                    currentCountThreadInNorthWestBeforStart = 0;
                }
                else
                    dicIsGivedBestSelectInNorthWest.Add(search.ID, false);

                Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
                //
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    //
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    //
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
                        //Update to sql
                        using (var dbUpdate = new DataClasses1DataContext())
                        {
                            var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                        where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                        select TbTaskLists).Single();
                            task.Rescue_ID = winRescueID;
                            task.Probability = winRescueProbability;
                            task.Priority = 0;
                            dbUpdate.SubmitChanges();
                        }
                    }
                }
                //
                lock (syncLockStartInNorthWest)
                {
                    currentCountThreadInNorthWestStart++;
                    if (currentCountThreadInNorthWestStart < requestedCountThreadInNorthWest)
                    {
                        Monitor.Wait(syncLockStartInNorthWest);
                    }
                    Monitor.PulseAll(syncLockStartInNorthWest);
                }
                currentCountThreadInNorthWestStart = 0;
                if (counter == 0)
                {
                    start = ss.Elapsed.TotalSeconds;
                    requestedCountThreadInNorthWestCopy = requestedCountThreadInNorthWest;
                }
                //
                //int b = 0;
                int b = 0;
                double penalty = 0;
                double x;
                List<string> competitorSID = rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs;
                //select sql
                Dictionary<string, double?> dicSID = new Dictionary<string, double?>();
                using (var dbSelect = new DataClasses1DataContext())
                {
                    dicSID = (from TbTaskLists in dbSelect.TbTaskLists
                              where TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                              select TbTaskLists).Select(p => new { p.Search_ID, p.Probability }).ToDictionary(p => p.Search_ID, p => p.Probability);
                }
                //
                //if (competitorSID.Count() <= 1)
                if (dicSID.Count <= 1)
                {
                    
                    if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID)) //.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID))
                    {
                        b = 1;
                        x = 0.4; //x = 0.4;
                        newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        isWin = true;
                    }
                    else
                    {
                        Random r = new Random();
                        double y = r.NextDouble();
                        if (y <= 0.15)//0.2
                        {
                            b = 1;
                            x = 0.4;//x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; //b = 0; // b = -1;
                            x = 0.4;
                            //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                        }
                        //b = -1;
                        //x = 0.4;
                        //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        //isWin = false;

                    }
                }
                else
                {
                    string winSearchID = "";
                    if (dicOfProbabilityRs.Count().Equals(1))
                    {
                        requestedThisSectionCountThreadInNorthWest++;
                        
                        lock (syncLockMiddleInNorthWest)
                        {
                            currentThisSectionCountThreadInNorthWest++;
                            if (currentThisSectionCountThreadInNorthWest < requestedThisSectionCountThreadInNorthWest)
                            {
                                Monitor.Wait(syncLockMiddleInNorthWest);
                            }
                            Monitor.PulseAll(syncLockMiddleInNorthWest);
                        }
                        currentThisSectionCountThreadInNorthWest = 0;
                        requestedThisSectionCountThreadInNorthWest = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderByDescending(p => p.Value).First().Key;
                        winSearchID = dicSID.First(p => p.Value.Equals(dicSID.Max(q => q.Value))).Key;
                        //
                        if (winSearchID.Equals(search.ID))
                        {
                            b = 1;
                            x = 0.4;//x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; //b = 0;// b = -1;
                            x = 0.4;
                            //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                            isGivedBestSelect = true;
                        }
                    }
                    else
                    {
                        //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID) || dicIsGivedBestSelectInNorthWest.First(p => p.Key.Equals(search.ID)).Value)
                        //{
                        bool IsBestRescueForAnotherSearch = false;
                        requestedThisSectionCountThreadInNorthWest++;
                        TbPoint selectedPoint;
                        double minProbability;
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        //string secondWinRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
                        //
                        lock (this)
                        {
                            
                            foreach (var sID in dicSID.Keys)
                            {
                                using (var dbSelect = new DataClasses1DataContext())
                                {
                                    selectedPoint = (from TbPoints in dbSelect.TbPoints
                                                     where TbPoints.Point_ID == ((from TbTaskLists in dbSelect.TbTaskLists
                                                                                  where TbTaskLists.Search_ID == sID && TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                                                                                  select TbTaskLists).Single().Point_ID)
                                                     //select new { leftCoordinationPoint = TbPoints.Point_LeftCoordinate, topCoordinationPoint = TbPoints.Point_TopCoordinate });
                                                     select TbPoints).Single();
                                }

                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInNorthWest(selectedPoint, winRescueID));

                               
                                if (!sID.Equals(search.ID))
                                {
                                    if (IsBestRescueForAnotherSearchInNorthWest(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            //
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;
                        }

                        //
                        lock (syncLockMiddleInNorthWest)
                        {
                            currentThisSectionCountThreadInNorthWest++;
                            if (currentThisSectionCountThreadInNorthWest < requestedThisSectionCountThreadInNorthWest)
                            {
                                Monitor.Wait(syncLockMiddleInNorthWest);
                            }
                            Monitor.PulseAll(syncLockMiddleInNorthWest);
                        }
                        currentThisSectionCountThreadInNorthWest = 0;
                        requestedThisSectionCountThreadInNorthWest = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderBy(p => p.Value).First().Key;
                        //
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID))// || dicIsGivedBestSelectInNorthWest.First(p => p.Key.Equals(search.ID)).Value)
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                //b = 1;
                                //x = 0.4;
                                //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                //isWin = true;
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.5)//0.9//0.5//0.15
                                {
                                    penalty = 0.1; //b = 0; //b = -1;
                                    x = 0.4;
                                    //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                    if (i < dicOfDistanceRs.Count() - 1)
                                        i = i + 1;
                                }
                                else
                                {
                                    b = 1;
                                    x = 0.4;//x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                            }
                            else
                            {
                                penalty = 0.1; //b = 0; //b = -1;
                                x = 0.4;
                                //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false;
                                isGivedBestSelect = true;
                            }
                            
                        }
                        else
                        {
                            
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                b = 1;
                                x = 0.4;//x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                isWin = true;
                            }
                            else
                            {
                                //b = -1;
                                //x = 0.4;
                                //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                //isWin = false;
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.15)//0.2
                                {
                                    b = 1;
                                    x = 0.4;//x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                    isGivedBestSelect = true;
                                }
                                else
                                {
                                    penalty = 0.1; //b = 0; // b = -1;
                                    x = 0.4;
                                    //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                }

                            }
                        }
                       
                    }
                }

                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++;
                //
                lock (syncLockEndInNorthWest)
                {
                    currentCountThreadInNorthWestEnd++;
                    if (currentCountThreadInNorthWestEnd < requestedCountThreadInNorthWest)
                    {
                        Monitor.Wait(syncLockEndInNorthWest);
                    }
                    Monitor.PulseAll(syncLockEndInNorthWest);
                }
                currentCountThreadInNorthWestEnd = 0;
                //
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    requestedCountThreadInNorthWest--;
                    isOutByTime = true;
                    break;
                }
                else
                    isOutByTime = false ;
            }
            //
            lock (this)
            {
                if (isWin == true)
                {
                    if (!listOfWinRescueIDInNorthWest.Any(p => p.Equals(winRescueID)))
                    {
                        listOfWinRescueIDInNorthWest.Add(winRescueID);
                        busyNorthWestRs++;
                    }
                }
            }
            //
            lock (syncLockAfterEndInNorthWest)
            {
                currentCountThreadInNorthWestAfterEnd++;
                if (currentCountThreadInNorthWestAfterEnd < requestedCountThreadInNorthWestCopy)
                {
                    Monitor.Wait(syncLockAfterEndInNorthWest);
                }
                Monitor.PulseAll(syncLockAfterEndInNorthWest);
               
                requestedCountThreadInNorthWest = requestedCountThreadInNorthWestCopy;
            }
            currentCountThreadInNorthWestAfterEnd = 0;
            //
            int countRescuesNorthWest = rescues.Where(p => p.Container.Equals(UnitType.NorthWest)).Count();
            int countSearchsNorthWest = searchs.Where(p => p.Container.Equals(UnitType.NorthWest)).Count();
            int countAllTasksNorthWest = points.Where(p => p.Container.Equals(UnitType.NorthWest) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInNorthWestCopy;

            if (countRescuesNorthWest < countSearchsNorthWest)
            {
                if (countAllTasksNorthWest > countRescuesNorthWest)
                    efficiencyBusyRescue += (double)busyNorthWestRs / countRescuesNorthWest;
                else
                    efficiencyBusyRescue += (double)busyNorthWestRs / countAllTasksNorthWest;
            }
            else
            {
                efficiencyBusyRescue += (double)busyNorthWestRs / countCurrentTask;
            }
            numBusyRescue++;
            //
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInNorthWest.Clear();
            ////delete from sql
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }
            //
            return winRescueID;
        }

        public string SelectWinRescueInNorthEastByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs,int searchIdForChart, out double winProbability, out bool isWin)
        {
            string winRescueID = "";
            double winRescueProbability = 0.0; int i = 0;
            isWin = false;
            winProbability = 0;
            int counter = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInNorthEastCopy=0;
            //
            double start=ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInNorthEast = new List<string>();
            busyNorthEastRs = 0;
            //
            while (counter <= 25)
            {
                //******************//
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
                //*****************//
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    //
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInNorthEast.Remove(search.ID);
                            dicIsGivedBestSelectInNorthEast.Add(search.ID, true);
                        }
                    }
                    //
                    ////Update from sql in TbTaskList
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    //
                    lock (syncLockBeforStartInNorthEast)
                    {
                        currentCountThreadInNorthEastBeforStart++;
                        if (currentCountThreadInNorthEastBeforStart < requestedCountThreadInNorthEast)
                        {
                            Monitor.Wait(syncLockBeforStartInNorthEast);
                        }
                        Monitor.PulseAll(syncLockBeforStartInNorthEast);
                    }
                    currentCountThreadInNorthEastBeforStart = 0;
                }
                else
                    dicIsGivedBestSelectInNorthEast.Add(search.ID, false);

                Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
                //
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    //
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    //
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
                        //Update to sql
                        using (var dbUpdate = new DataClasses1DataContext())
                        {
                            var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                        where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                        select TbTaskLists).Single();
                            task.Rescue_ID = winRescueID;
                            task.Probability = winRescueProbability;
                            task.Priority = 0;
                            dbUpdate.SubmitChanges();
                        }
                    }
                }
                //
                lock (syncLockStartInNorthEast)
                {
                    currentCountThreadInNorthEastStart++;
                    if (currentCountThreadInNorthEastStart < requestedCountThreadInNorthEast)
                    {
                        Monitor.Wait(syncLockStartInNorthEast);
                    }
                    Monitor.PulseAll(syncLockStartInNorthEast);
                }
                currentCountThreadInNorthEastStart = 0;
                if (counter == 0)
                {
                    start = ss.Elapsed.TotalSeconds;
                    requestedCountThreadInNorthEastCopy = requestedCountThreadInNorthEast;
                }
                //
                int b = 0;
                double penalty = 0;
                double x;
                List<string> competitorSID = rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs;
                //select sql
                Dictionary<string, double?> dicSID = new Dictionary<string, double?>();
                using (var dbSelect = new DataClasses1DataContext())
                {
                    dicSID = (from TbTaskLists in dbSelect.TbTaskLists
                              where TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                              select TbTaskLists).Select(p => new { p.Search_ID, p.Probability }).ToDictionary(p => p.Search_ID, p => p.Probability);
                }
                //

                //if (competitorSID.Count() <= 1)
                if (dicSID.Count <= 1)
                {
                    
                    if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                    {
                        b = 1;
                        x = 0.4;
                        newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        isWin = true;
                    }
                    else
                    {
                        Random r = new Random();
                        double y = r.NextDouble();
                        if (y <= 0.15)//0.2
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1;//b = -1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                        }
                        
                    }
                }
                else
                {
                    string winSearchID = "";
                    if (dicOfProbabilityRs.Count().Equals(1))
                    {
                        requestedThisSectionCountThreadInNorthEast++;
                        
                        lock (syncLockMiddleInNorthEast)
                        {
                            currentThisSectionCountThreadInNorthEast++;
                            if (currentThisSectionCountThreadInNorthEast < requestedThisSectionCountThreadInNorthEast)
                            {
                                Monitor.Wait(syncLockMiddleInNorthEast);
                            }
                            Monitor.PulseAll(syncLockMiddleInNorthEast);
                        }
                        currentThisSectionCountThreadInNorthEast = 0;
                        requestedThisSectionCountThreadInNorthEast = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderByDescending(p => p.Value).First().Key;
                        winSearchID = dicSID.First(p => p.Value.Equals(dicSID.Max(q => q.Value))).Key;
                        //
                        if (winSearchID.Equals(search.ID))
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; //b = -1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                            isGivedBestSelect = true;
                        }
                    }
                    else
                    {
                        //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID) || dicIsGivedBestSelectInNorthEast.First(p => p.Key.Equals(search.ID)).Value)
                        //{
                        bool IsBestRescueForAnotherSearch = false;
                        requestedThisSectionCountThreadInNorthEast++;
                        TbPoint selectedPoint;
                        double minProbability;
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        //string secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
                        //
                        lock (this)
                        {
                            //
                            foreach (var sID in dicSID.Keys)
                            {
                                using (var dbSelect = new DataClasses1DataContext())
                                {
                                    selectedPoint = (from TbPoints in dbSelect.TbPoints
                                                     where TbPoints.Point_ID == ((from TbTaskLists in dbSelect.TbTaskLists
                                                                                  where TbTaskLists.Search_ID == sID && TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                                                                                  select TbTaskLists).Single().Point_ID)
                                                     //select new { leftCoordinationPoint = TbPoints.Point_LeftCoordinate, topCoordinationPoint = TbPoints.Point_TopCoordinate });
                                                     select TbPoints).Single();
                                }

                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInNorthEast(selectedPoint, winRescueID));

                               
                                if (!sID.Equals(search.ID))
                                {
                                    if (IsBestRescueForAnotherSearchInNorthEast(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            //
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;
                        }
                        //
                        lock (syncLockMiddleInNorthEast)
                        {
                            currentThisSectionCountThreadInNorthEast++;
                            if (currentThisSectionCountThreadInNorthEast < requestedThisSectionCountThreadInNorthEast)
                            {
                                Monitor.Wait(syncLockMiddleInNorthEast);
                            }
                            Monitor.PulseAll(syncLockMiddleInNorthEast);
                        }
                        currentThisSectionCountThreadInNorthEast = 0;
                        requestedThisSectionCountThreadInNorthEast = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderBy(p => p.Value).First().Key;
                        //
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID))// || dicIsGivedBestSelectInNorthWest.First(p => p.Key.Equals(search.ID)).Value)
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.5)//0.9//0.5//0.15
                                {
                                    penalty = 0.1; //b = -1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                    if (i < dicOfDistanceRs.Count() - 1)
                                        i = i + 1;
                                }
                                else
                                {
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                            }
                            else
                            {
                                penalty = 0.1;//b = -1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false;
                                isGivedBestSelect = true;
                            }
                            
                        }
                        else
                        {
                            
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                b = 1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                isWin = true;
                            }
                            else
                            {
                                //b = -1;
                                //x = 0.4;
                                //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                //isWin = false;
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.15)//0.2
                                {
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                                else
                                {
                                    penalty = 0.1; //b = -1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                }

                            }
                        }
                       
                    }
                }
                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++;
                //
                lock (syncLockEndInNorthEast)
                {
                    currentCountThreadInNorthEastEnd++;
                    if (currentCountThreadInNorthEastEnd < requestedCountThreadInNorthEast)
                    {
                        Monitor.Wait(syncLockEndInNorthEast);
                    }
                    Monitor.PulseAll(syncLockEndInNorthEast);
                }
                currentCountThreadInNorthEastEnd = 0;
                //
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    requestedCountThreadInNorthEast--;
                    isOutByTime = true;
                    break;
                }
                else
                    isOutByTime = false;
                //
            }
            //
            lock (this)
            {
                if (isWin == true)
                {
                    if (!listOfWinRescueIDInNorthEast.Any(p => p.Equals(winRescueID)))
                    {
                        listOfWinRescueIDInNorthEast.Add(winRescueID);
                        busyNorthEastRs++;
                    }
                }
            }
            //
            lock (syncLockAfterEndInNorthEast)
            {
                currentCountThreadInNorthEastAfterEnd++;
                if (currentCountThreadInNorthEastAfterEnd < requestedCountThreadInNorthEastCopy)
                {
                    Monitor.Wait(syncLockAfterEndInNorthEast);
                }
                Monitor.PulseAll(syncLockAfterEndInNorthEast);
                
                requestedCountThreadInNorthEast = requestedCountThreadInNorthEastCopy;
            }
            currentCountThreadInNorthEastAfterEnd = 0;
            //
            int countRescuesNorthEast = rescues.Where(p => p.Container.Equals(UnitType.NorthEast)).Count();
            int countSearchsNorthEast = searchs.Where(p => p.Container.Equals(UnitType.NorthEast)).Count();
            int countAllTasksNorthEast = points.Where(p => p.Container.Equals(UnitType.NorthEast) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInNorthEastCopy;

            if (countRescuesNorthEast < countSearchsNorthEast)
            {
                if (countAllTasksNorthEast > countRescuesNorthEast)
                    efficiencyBusyRescue += (double)busyNorthEastRs / countRescuesNorthEast;
                else
                    efficiencyBusyRescue += (double)busyNorthEastRs / countAllTasksNorthEast;
            }
            else
            {
                efficiencyBusyRescue += (double)busyNorthEastRs / countCurrentTask;
            }
            numBusyRescue++;
            
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInNorthEast.Clear();
            ////delete from sql
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }
            //

            return winRescueID;
        }

        public string SelectWinRescueInSouthWestByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs,int searchIdForChart, out double winProbability, out bool isWin)
        {
            string winRescueID = "";
            double winRescueProbability = 0.0; int i = 0;
            isWin = false;
            winProbability = 0;
            int counter = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInSouthWestCopy = 0;
            //
            double start=ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInSouthWest = new List<string>();
            busySouthWestRs = 0;
            //
            while (counter <= 25)
            {
                //************//
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
                //***********//
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    //
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInNorthEast.Remove(search.ID);
                            dicIsGivedBestSelectInNorthEast.Add(search.ID, true);
                        }
                    }
                    //
                    ////Update from sql in TbTaskList
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    //
                    lock (syncLockBeforStartInSouthWest)
                    {
                        currentCountThreadInSouthWestBeforStart++;
                        if (currentCountThreadInSouthWestBeforStart < requestedCountThreadInSouthWest)
                        {
                            Monitor.Wait(syncLockBeforStartInSouthWest);
                        }
                        Monitor.PulseAll(syncLockBeforStartInSouthWest);
                    }
                    currentCountThreadInSouthWestBeforStart = 0;
                }
                else
                    dicIsGivedBestSelectInSouthWest.Add(search.ID, false);

                Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
                //
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    //
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    //
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
                        //Update to sql
                        using (var dbUpdate = new DataClasses1DataContext())
                        {
                            var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                        where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                        select TbTaskLists).Single();
                            task.Rescue_ID = winRescueID;
                            task.Probability = winRescueProbability;
                            task.Priority = 0;
                            dbUpdate.SubmitChanges();
                        }
                        //
                    }
                }

                //
                lock (syncLockStartInSouthWest)
                {
                    currentCountThreadInSouthWestStart++;
                    if (currentCountThreadInSouthWestStart < requestedCountThreadInSouthWest)
                    {
                        Monitor.Wait(syncLockStartInSouthWest);
                    }
                    Monitor.PulseAll(syncLockStartInSouthWest);
                }
                currentCountThreadInSouthWestStart = 0;
                if (counter == 0)
                {
                    start = ss.Elapsed.TotalSeconds;
                    requestedCountThreadInSouthWestCopy = requestedCountThreadInSouthWest;
                }
                //
                int b = 0;
                double penalty = 0;
                double x;
                List<string> competitorSID = rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs;
                //select sql
                Dictionary<string, double?> dicSID = new Dictionary<string, double?>();
                using (var dbSelect = new DataClasses1DataContext())
                {
                    dicSID = (from TbTaskLists in dbSelect.TbTaskLists
                              where TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                              select TbTaskLists).Select(p => new { p.Search_ID, p.Probability }).ToDictionary(p => p.Search_ID, p => p.Probability);
                }
                //

                //if (competitorSID.Count() <= 1)
                if (dicSID.Count <= 1)
                {
                    
                    if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                    {
                        b = 1;
                        x = 0.4;
                        newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        isWin = true;
                    }
                    else
                    {
                        Random r = new Random();
                        double y = r.NextDouble();
                        if (y <= 0.15) //0.2
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; //b = -1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                        }
                        //b = -1;
                        //x = 0.4;
                        //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        //isWin = false;
                    }
                }
                else
                {
                    string winSearchID = "";
                    if (dicOfProbabilityRs.Count().Equals(1))
                    {
                        requestedThisSectionCountThreadInSouthWest++;

                       
                        lock (syncLockMiddleInSouthWest)
                        {
                            currentThisSectionCountThreadInSouthWest++;
                            if (currentThisSectionCountThreadInSouthWest < requestedThisSectionCountThreadInSouthWest)
                            {
                                Monitor.Wait(syncLockMiddleInSouthWest);
                            }
                            Monitor.PulseAll(syncLockMiddleInSouthWest);
                        }
                        currentThisSectionCountThreadInSouthWest = 0;
                        requestedThisSectionCountThreadInSouthWest = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderByDescending(p => p.Value).First().Key;
                        winSearchID = dicSID.First(p => p.Value.Equals(dicSID.Max(q => q.Value))).Key;
                        //
                        if (winSearchID.Equals(search.ID))
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; //b = -1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                            isGivedBestSelect = true;
                        }
                    }
                    else
                    {
                        //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID) || dicIsGivedBestSelectInSouthWest.First(p => p.Key.Equals(search.ID)).Value)
                        //{
                        bool IsBestRescueForAnotherSearch = false;
                        requestedThisSectionCountThreadInSouthWest++;
                        TbPoint selectedPoint;
                        double minProbability;
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        //string secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
                        //
                        //
                        lock (this)
                        {
                            
                            //
                            foreach (var sID in dicSID.Keys)
                            {
                                using (var dbSelect = new DataClasses1DataContext())
                                {
                                    selectedPoint = (from TbPoints in dbSelect.TbPoints
                                                     where TbPoints.Point_ID == ((from TbTaskLists in dbSelect.TbTaskLists
                                                                                  where TbTaskLists.Search_ID == sID && TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                                                                                  select TbTaskLists).Single().Point_ID)
                                                     //select new { leftCoordinationPoint = TbPoints.Point_LeftCoordinate, topCoordinationPoint = TbPoints.Point_TopCoordinate });
                                                     select TbPoints).Single();
                                }

                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInSouthWest(selectedPoint, winRescueID));

                               
                                if (!sID.Equals(search.ID))
                                {
                                    if (IsBestRescueForAnotherSearchInSouthWest(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            //
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;
                        }
                        //
                        lock (syncLockMiddleInSouthWest)
                        {
                            currentThisSectionCountThreadInSouthWest++;
                            if (currentThisSectionCountThreadInSouthWest < requestedThisSectionCountThreadInSouthWest)
                            {
                                Monitor.Wait(syncLockMiddleInSouthWest);
                            }
                            Monitor.PulseAll(syncLockMiddleInSouthWest);
                        }
                        currentThisSectionCountThreadInSouthWest = 0;
                        requestedThisSectionCountThreadInSouthWest = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderBy(p => p.Value).First().Key;
                        //
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID))// || dicIsGivedBestSelectInNorthWest.First(p => p.Key.Equals(search.ID)).Value)
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.5)//0.9//0.5//0.15
                                {
                                    penalty = 0.1; //b = -1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                    if (i < dicOfDistanceRs.Count() - 1)
                                        i = i + 1;
                                }
                                else
                                {
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                            }
                            else
                            {
                                penalty = 0.1; //b = -1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false;
                                isGivedBestSelect = true;
                            }
                            
                        }
                        else
                        {
                           
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                b = 1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                isWin = true;
                            }
                            else
                            {
                                
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.15)//0.2
                                {
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                                else
                                {
                                    penalty = 0.1; //b = -1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                }

                            }
                        }
                       
                    }
                }
                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++;
                //
                lock (syncLockEndInSouthWest)
                {
                    currentCountThreadInSouthWestEnd++;
                    if (currentCountThreadInSouthWestEnd < requestedCountThreadInSouthWest)
                    {
                        Monitor.Wait(syncLockEndInSouthWest);
                    }
                    Monitor.PulseAll(syncLockEndInSouthWest);
                }
                currentCountThreadInSouthWestEnd = 0;
                //
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    requestedCountThreadInSouthWest--;
                    isOutByTime = true;
                    break;
                }
                else
                    isOutByTime = false;
                //
            }
            //
            lock (this)
            {
                if (isWin == true)
                {
                    if (!listOfWinRescueIDInSouthWest.Any(p => p.Equals(winRescueID)))
                    {
                        listOfWinRescueIDInSouthWest.Add(winRescueID);
                        busySouthWestRs++;
                    }
                }
            }
            //
            lock (syncLockAfterEndInSouthWest)
            {
                currentCountThreadInSouthWestAfterEnd++;
                if (currentCountThreadInSouthWestAfterEnd < requestedCountThreadInSouthWestCopy)
                {
                    Monitor.Wait(syncLockAfterEndInSouthWest);
                }
                Monitor.PulseAll(syncLockAfterEndInSouthWest);
                
                requestedCountThreadInSouthWest = requestedCountThreadInSouthWestCopy;
            }
            currentCountThreadInSouthWestAfterEnd = 0;
            //
            int countRescuesSouthWest = rescues.Where(p => p.Container.Equals(UnitType.SouthWest)).Count();
            int countSearchsSouthWest = searchs.Where(p => p.Container.Equals(UnitType.SouthWest)).Count();
            int countAllTasksSouthWest = points.Where(p => p.Container.Equals(UnitType.SouthWest) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInSouthWestCopy;

            if (countRescuesSouthWest < countSearchsSouthWest)
            {
                if (countAllTasksSouthWest > countRescuesSouthWest)
                    efficiencyBusyRescue += (double)busySouthWestRs / countRescuesSouthWest;
                else
                    efficiencyBusyRescue += (double)busySouthWestRs / countAllTasksSouthWest;
            }
            else
            {
                efficiencyBusyRescue += (double)busySouthWestRs / countCurrentTask;
            }
            numBusyRescue++;
            //
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInSouthWest.Clear();
            ////delete from sql
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }
            //

            return winRescueID;
        }

        public string SelectWinRescueInSouthEastByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs,int searchIdForChart, out double winProbability, out bool isWin)
        {
            string winRescueID = "";
            double winRescueProbability = 0.0; int i = 0;
            isWin = false;
            int counter = 0;
            winProbability = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInSouthEastCopy = 0;
            //
            double start=ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInSouthEast = new List<string>();
            busySouthEastRs = 0;
            //
            while (counter <= 25)
            {
                //************//
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
                //***********//
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    //
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInNorthEast.Remove(search.ID);
                            dicIsGivedBestSelectInNorthEast.Add(search.ID, true);
                        }
                    }
                    //
                    ////Update from sql in TbTaskList
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    //
                    lock (syncLockBeforStartInSouthEast)
                    {
                        currentCountThreadInSouthEastBeforStart++;
                        if (currentCountThreadInSouthEastBeforStart < requestedCountThreadInSouthEast)
                        {
                            Monitor.Wait(syncLockBeforStartInSouthEast);
                        }
                        Monitor.PulseAll(syncLockBeforStartInSouthEast);
                    }
                    currentCountThreadInSouthEastBeforStart = 0;
                }
                else
                    dicIsGivedBestSelectInSouthEast.Add(search.ID, false);
                Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
                //
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    //
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    //
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
                        //Update to sql
                        using (var dbUpdate = new DataClasses1DataContext())
                        {
                            var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                        where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                        select TbTaskLists).Single();
                            task.Rescue_ID = winRescueID;
                            task.Probability = winRescueProbability;
                            task.Priority = 0;
                            dbUpdate.SubmitChanges();
                        }
                        //
                    }
                }
                //
                lock (syncLockStartInSouthEast)
                {
                    currentCountThreadInSouthEastStart++;
                    if (currentCountThreadInSouthEastStart < requestedCountThreadInSouthEast)
                    {
                        Monitor.Wait(syncLockStartInSouthEast);
                    }
                    Monitor.PulseAll(syncLockStartInSouthEast);
                }
                currentCountThreadInSouthEastStart = 0;
                if (counter == 0)
                {
                    start = ss.Elapsed.TotalSeconds;
                    requestedCountThreadInSouthEastCopy = requestedCountThreadInSouthEast;
                }
                //
                int b = 0;
                double penalty = 0;
                double x;
                List<string> competitorSID = rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs;
                //select sql
                Dictionary<string, double?> dicSID = new Dictionary<string, double?>();
                using (var dbSelect = new DataClasses1DataContext())
                {
                    dicSID = (from TbTaskLists in dbSelect.TbTaskLists
                              where TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                              select TbTaskLists).Select(p => new { p.Search_ID, p.Probability }).ToDictionary(p => p.Search_ID, p => p.Probability);
                }
                //

                //if (competitorSID.Count() <= 1)
                if (dicSID.Count <= 1)
                {
                   
                    if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                    {
                        b = 1;
                        x = 0.4;
                        newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        isWin = true;
                    }
                    else
                    {
                        Random r = new Random();
                        double y = r.NextDouble();
                        if (y <= 0.15)//0.2
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; //b = -1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                        }
                       

                    }
                }
                else
                {
                    string winSearchID = "";
                    if (dicOfProbabilityRs.Count().Equals(1))
                    {
                        requestedThisSectionCountThreadInSouthEast++;
                        
                        lock (syncLockMiddleInSouthEast)
                        {
                            currentThisSectionCountThreadInSouthEast++;
                            if (currentThisSectionCountThreadInSouthEast < requestedThisSectionCountThreadInSouthEast)
                            {
                                Monitor.Wait(syncLockMiddleInSouthEast);
                            }
                            Monitor.PulseAll(syncLockMiddleInSouthEast);
                        }
                        currentThisSectionCountThreadInSouthEast = 0;
                        requestedThisSectionCountThreadInSouthEast = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderByDescending(p => p.Value).First().Key;
                        winSearchID = dicSID.First(p => p.Value.Equals(dicSID.Max(q => q.Value))).Key;
                        //
                        if (winSearchID.Equals(search.ID))
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; //b = -1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                            isGivedBestSelect = true;
                        }
                    }
                    else
                    {
                        //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID) || dicIsGivedBestSelectInSouthEast.First(p => p.Key.Equals(search.ID)).Value)
                        //{
                        bool IsBestRescueForAnotherSearch = false;
                        requestedThisSectionCountThreadInSouthEast++;
                        TbPoint selectedPoint;
                        double minProbability;
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        //string secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
                        //
                        lock (this)
                        {


                            //
                            foreach (var sID in dicSID.Keys)
                            {
                                using (var dbSelect = new DataClasses1DataContext())
                                {
                                    selectedPoint = (from TbPoints in dbSelect.TbPoints
                                                     where TbPoints.Point_ID == ((from TbTaskLists in dbSelect.TbTaskLists
                                                                                  where TbTaskLists.Search_ID == sID && TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                                                                                  select TbTaskLists).Single().Point_ID)
                                                     //select new { leftCoordinationPoint = TbPoints.Point_LeftCoordinate, topCoordinationPoint = TbPoints.Point_TopCoordinate });
                                                     select TbPoints).Single();
                                }

                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInSouthEast(selectedPoint, winRescueID));
                               
                                if (!sID.Equals(search.ID))
                                {
                                    if (IsBestRescueForAnotherSearchInSouthEast(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            //
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;

                        }
                        //
                        lock (syncLockMiddleInSouthEast)
                        {
                            currentThisSectionCountThreadInSouthEast++;
                            if (currentThisSectionCountThreadInSouthEast < requestedThisSectionCountThreadInSouthEast)
                            {
                                Monitor.Wait(syncLockMiddleInSouthEast);
                            }
                            Monitor.PulseAll(syncLockMiddleInSouthEast);
                        }
                        currentThisSectionCountThreadInSouthEast = 0;
                        requestedThisSectionCountThreadInSouthEast = 0;
                        //
                        //winSearchID = search.CompetitorProbabilites.OrderBy(p => p.Value).First().Key;
                        //
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            //if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(winRescueID))// || dicIsGivedBestSelectInNorthWest.First(p => p.Key.Equals(search.ID)).Value)
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                //b = 1;
                                //x = 0.4;
                                //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                //isWin = true;
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.5)//0.9//0.5//0.15
                                {
                                    penalty = 0.1;//b = -1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                    if (i < dicOfDistanceRs.Count() - 1)
                                        i = i + 1;
                                }
                                else
                                {
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                            }
                            else
                            {
                                penalty = 0.1;//b = -1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false;
                                isGivedBestSelect = true;
                            }
                            
                        }
                        else
                        {
                            
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                b = 1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                isWin = true;
                            }
                            else
                            {
                                //b = -1;
                                //x = 0.4;
                                //newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                //isWin = false;
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.15)//0.2
                                {
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                    isGivedBestSelect = true;
                                }
                                else
                                {
                                    penalty = 0.1;// = -1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                }

                            }
                        }
                       
                    }
                }
                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++;
                //
                lock (syncLockEndInSouthEast)
                {
                    currentCountThreadInSouthEastEnd++;
                    if (currentCountThreadInSouthEastEnd < requestedCountThreadInSouthEast)
                    {
                        Monitor.Wait(syncLockEndInSouthEast);
                    }
                    Monitor.PulseAll(syncLockEndInSouthEast);
                }
                currentCountThreadInSouthEastEnd = 0;
                //
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    requestedCountThreadInSouthEast--;
                    isOutByTime = true;
                    break;
                }
                else
                    isOutByTime = false;
            }
            ////
            lock (this)
            {
                if (isWin == true)
                {
                    if (!listOfWinRescueIDInSouthEast.Any(p => p.Equals(winRescueID)))
                    {
                        listOfWinRescueIDInSouthEast.Add(winRescueID);
                        busySouthEastRs++;
                    }
                }
            }
            //
            lock (syncLockAfterEndInSouthEast)
            {
                currentCountThreadInSouthEastAfterEnd++;
                if (currentCountThreadInSouthEastAfterEnd < requestedCountThreadInSouthEastCopy)
                {
                    Monitor.Wait(syncLockAfterEndInSouthEast);
                }
                Monitor.PulseAll(syncLockAfterEndInSouthEast);
                //lock (this)
                //{
                //    if (isOutByTime)
                //        requestedCountThreadInSouthEast++;
                //}
                requestedCountThreadInSouthEast = requestedCountThreadInSouthEastCopy;
            }
            currentCountThreadInSouthEastAfterEnd = 0;
            //
            int countRescuesSouthEast = rescues.Where(p => p.Container.Equals(UnitType.SouthEast)).Count();
            int countSearchsSouthEast = searchs.Where(p => p.Container.Equals(UnitType.SouthEast)).Count();
            int countAllTasksSouthEast = points.Where(p => p.Container.Equals(UnitType.SouthEast) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInSouthEastCopy;

            if (countRescuesSouthEast < countSearchsSouthEast)
            {
                if (countAllTasksSouthEast > countRescuesSouthEast)
                    efficiencyBusyRescue += (double)busySouthEastRs / countRescuesSouthEast;
                else
                    efficiencyBusyRescue += (double)busySouthEastRs / countAllTasksSouthEast;
            }
            else
            {
                efficiencyBusyRescue += (double)busySouthEastRs / countCurrentTask;
            }
            numBusyRescue++;
            //
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInSouthEast.Clear();
            ////delete from sql
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }
            //

            return winRescueID;
        }
        //**************//
        public string SelectWinRescueByProbability(Dictionary<string,double> dicOfProbabilityRs)
        {
            string winRescueID="";
            double minValue = dicOfProbabilityRs.OrderBy(p => p.Value).First().Value;
            double maxValue = dicOfProbabilityRs.OrderBy(p => p.Value).Last().Value;
            Random r = new Random();
            double y = r.NextDouble();
            double value = (y * (maxValue - minValue) + minValue);
            double lastProb = 0;
            //
            foreach (var prob in dicOfProbabilityRs.OrderBy(p => p.Value))
            {
                if (value > lastProb && value <= prob.Value)
                {
                    winRescueID = prob.Key;
                }
                lastProb = prob.Value;
            }
            //winRescueID=dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
            return winRescueID ;
        }

        public Dictionary<string,double> ComputeNewProbabilites(string winRescueID,double winRescueProbability,int b,double x, Dictionary<string, double> dicOfProbabilityRs)
        {
            Dictionary<string,double> newDicOfProbabilityRs=new Dictionary<string,double>();
            double probability = 0;
            foreach (var prob in dicOfProbabilityRs)
            {
                
                if (!probability.Equals(0) || !probability.Equals(1))
                {
                    if (prob.Key.Equals(winRescueID))
                        probability = winRescueProbability + x * b * (1 - winRescueProbability);
                    else
                        probability = dicOfProbabilityRs[prob.Key] - x * b * (dicOfProbabilityRs[prob.Key]);
                }

                if (probability < 0)
                    newDicOfProbabilityRs.Add(prob.Key, 0);
                else if (probability > 1)
                    newDicOfProbabilityRs.Add(prob.Key, 1);
                else
                    newDicOfProbabilityRs.Add(prob.Key, probability);
            }
            return newDicOfProbabilityRs;
        }

        public Dictionary<string, double> ComputeNewProbabilitesForPenalty(string winRescueID, double winRescueProbability, double penalty, double x, Dictionary<string, double> dicOfProbabilityRs)
        {
            Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
            double probability = 0;
            int r = dicOfProbabilityRs.Count();
            foreach (var prob in dicOfProbabilityRs)
            {
                
                if (!probability.Equals(0) || !probability.Equals(1))
                {
                    if (prob.Key.Equals(winRescueID))
                        probability = (1 - penalty) * winRescueProbability;
                    else
                        probability = penalty / (r - 1) + (1 - penalty) * dicOfProbabilityRs[prob.Key];
                }

                if (probability < 0)
                    newDicOfProbabilityRs.Add(prob.Key, 0);
                else if (probability > 1)
                    newDicOfProbabilityRs.Add(prob.Key, 1);
                else
                    newDicOfProbabilityRs.Add(prob.Key, probability);
            }
            return newDicOfProbabilityRs;
        }
        //**********************************************//
        public void AssignePointToWinRescue(Search search, string winRescueID,double Probability, DomainObject.Point point,int countNewPoint, bool isWin)
        {
            Dictionary<Search,int> dic=new Dictionary<DomainObject.Search,int>();
            TbTaskList task = new TbTaskList();
            List<TbTaskList> listTasksNewPoints = new List<TbTaskList>();

            lock (this)
            {
                switch (isWin)
                {
                    case true:
                        //Insert task of new point to sql in TbTaskList
                        foreach (var item in newPoints.Where(p => p.ParentID.Equals(point.ID)).ToList())
                        {
                            TbTaskList taskNewPoint = new TbTaskList();
                            taskNewPoint.ID = Guid.NewGuid().ToString();
                            taskNewPoint.Search_ID = search.ID;
                            taskNewPoint.Point_ID = item.ID;
                            taskNewPoint.Point_ParentID = item.ParentID;
                            taskNewPoint.Rescue_ID = winRescueID;
                            taskNewPoint.Probability = Probability;
                            taskNewPoint.Priority = 1;
                            taskNewPoint.IsAssigned = true;
                            listTasksNewPoints.Add(taskNewPoint);
                        }
                        db.TbTaskLists.InsertAllOnSubmit(listTasksNewPoints);
                        db.SubmitChanges();
                        ////
                        //Insert task of point to sql in TbTaskList
                        task.ID = Guid.NewGuid().ToString();
                        task.Search_ID = search.ID;
                        task.Point_ID = point.ID;
                        task.Point_ParentID = point.ParentID;
                        task.Rescue_ID = winRescueID;
                        task.Probability = Probability;
                        task.Priority = 1;
                        task.IsAssigned = true;
                        db.TbTaskLists.InsertOnSubmit(task);
                        db.SubmitChanges();
                        //
                        dic.Add(search, 1);
                        if (rescues.Find(p => p.ID.Equals(winRescueID)).IsAllocated == false)
                        {
                            //lock (this)
                            //{
                            rescues.Find(p => p.ID.Equals(winRescueID)).IsAllocated = true;
                            rescues.Find(p => p.ID.Equals(winRescueID)).ListPoint.Add(point, dic);
                            //}
                        }
                        else
                            //lock (this)
                            //{
                            rescues.Find(p => p.ID.Equals(winRescueID)).ListPoint.Add(point, dic);
                            //}
                        break;
                    case false:
                        //lock (this)
                        //{
                        //Insert task of new point to sql in TbTaskList
                        foreach (var item in newPoints.Where(p => p.ParentID.Equals(point.ID)).ToList())
                        {
                            TbTaskList taskNewPoint = new TbTaskList();
                            taskNewPoint.ID = Guid.NewGuid().ToString();
                            taskNewPoint.Search_ID = search.ID;
                            taskNewPoint.Point_ID = item.ID;
                            taskNewPoint.Point_ParentID = item.ParentID;
                            taskNewPoint.Rescue_ID = winRescueID;
                            taskNewPoint.Probability = Probability;
                            taskNewPoint.Priority = 2;
                            taskNewPoint.IsAssigned = true;
                            listTasksNewPoints.Add(taskNewPoint);
                        }
                        db.TbTaskLists.InsertAllOnSubmit(listTasksNewPoints);
                        db.SubmitChanges();
                        ////
                        //Insert task of point to sql in TbTaskList
                        task.ID = Guid.NewGuid().ToString();
                        task.Search_ID = search.ID;
                        task.Point_ID = point.ID;
                        task.Point_ParentID = point.ParentID;
                        task.Rescue_ID = winRescueID;
                        task.Probability = Probability;
                        task.Priority = 2;
                        task.IsAssigned = true;
                        db.TbTaskLists.InsertOnSubmit(task);
                        db.SubmitChanges();
                        //
                        dic.Add(search, 2);
                        rescues.Find(p => p.ID.Equals(winRescueID)).ListPoint.Add(point, dic);
                        //}
                        break;
                }
            }
            //
            Rescue selectedRescue = rescues.Find(p => p.ID.Equals(winRescueID));
            Thread Thread = new Thread(() => DoRescueForPoint(selectedRescue, point,countNewPoint));
            AllRescueThreads.Add(Thread);
            Thread.Start();
            
        }

        public void DoRescueForPoint(Rescue selectedRescue, DomainObject.Point selectedPoint, int countNewPoint)
        {
            lock (selectedRescue)
            {
               

                selectedRescue.State = Rescue.RescueStateType.Busy;
                selectedRescue.IsDoing = true;
                idelRescue--;
                busyRescue++;

                //allIdelRescue += idelRescue;
                //numIdelRescue++;
                //allBusyRescue += busyRescue;
                //numBusyRescue++;

                selectedPoint.IsAllocatedRTeam = true;
                selectedPoint.StartRescueDoing = true;
                selectedPoint.RTeamID = selectedRescue.ID;

                //***** Update sql
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    var point = (from TbPoints in dbUpdate.TbPoints
                                 where TbPoints.Point_ID == selectedPoint.ID
                                 select TbPoints).Single();
                    point.Point_IsAllocatedRTeam = selectedPoint.IsAllocatedRTeam;
                    point.Point_StartRescueDoing = selectedPoint.StartRescueDoing;
                    dbUpdate.SubmitChanges();

                    var rescue = (from TbRescues in dbUpdate.TbRescues
                                 where TbRescues.Rescue_ID == selectedRescue.ID
                                 select TbRescues).Single();
                    rescue.Rescue_State = (int)selectedRescue.State;
                    dbUpdate.SubmitChanges();
                }

               
                //move
                int rescueTop, rescueLeft, pointTop, pointLeft;
                rescueTop = selectedRescue.Top;
                rescueLeft = selectedRescue.Left;
                pointTop = selectedPoint.Top;
                pointLeft = selectedPoint.Left;

                Label lblRescue = (Label)panel1.Controls.Find(selectedRescue.ID, true)[0];
                Label lblPoint = (Label)panel1.Controls.Find(selectedPoint.ID, true)[0];

                int xDistance = Math.Abs(lblRescue.Location.X - lblPoint.Location.X);
                int yDistance = Math.Abs(lblRescue.Location.Y - lblPoint.Location.Y);

                int total = 10;
                int xpart = xDistance / total;
                int ypart = yDistance / total;
                int totalTimeSleep = 0;
                int newCordinationX = 0;
                int newCordinationY = 0;

                double startTime = Math.Floor(ss.Elapsed.TotalSeconds);
                double endTime=0;

                for (int i = 0; i < total; i++)
                {
                    System.Threading.Thread.Sleep(int.Parse(nudRescue.Value.ToString()) * 30);
                    totalTimeSleep += (int.Parse(nudSearch.Value.ToString()) * 30);

                    if (lblRescue.Location.X < lblPoint.Location.X && lblRescue.Location.Y < lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X + 10;
                            newCordinationY = lblRescue.Location.Y + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X + xpart + 10;
                            newCordinationY = lblRescue.Location.Y + ypart + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X + xpart;
                            newCordinationY = lblRescue.Location.Y + ypart;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    //southwest
                    else if (lblRescue.Location.X < lblPoint.Location.X && lblRescue.Location.Y > lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X + 10;
                            newCordinationY = lblRescue.Location.Y - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X + xpart + 10;
                            newCordinationY = lblRescue.Location.Y - ypart - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X + xpart;
                            newCordinationY = lblRescue.Location.Y - ypart;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    //northeast
                    else if (lblRescue.Location.X > lblPoint.Location.X && lblRescue.Location.Y > lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X - 10;
                            newCordinationY = lblRescue.Location.Y - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X - xpart - 10;
                            newCordinationY = lblRescue.Location.Y - ypart - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X - xpart;
                            newCordinationY = lblRescue.Location.Y - ypart;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblRescue.Location.X > lblPoint.Location.X && lblRescue.Location.Y < lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X - 10;
                            newCordinationY = lblRescue.Location.Y + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X - xpart - 10;
                            newCordinationY = lblRescue.Location.Y + ypart + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X - xpart;
                            newCordinationY = lblRescue.Location.Y + ypart;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblRescue.Location.X == lblPoint.Location.X && lblRescue.Location.Y < lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X ;
                            newCordinationY = lblRescue.Location.Y + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X ;
                            newCordinationY = lblRescue.Location.Y + ypart + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X ;
                            newCordinationY = lblRescue.Location.Y + ypart;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblRescue.Location.X == lblPoint.Location.X && lblRescue.Location.Y > lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X;
                            newCordinationY = lblRescue.Location.Y - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X;
                            newCordinationY = lblRescue.Location.Y - ypart - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X;
                            newCordinationY = lblRescue.Location.Y - ypart;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblRescue.Location.X > lblPoint.Location.X && lblRescue.Location.Y == lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X))
                        {
                            newCordinationX = lblRescue.Location.X - 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X - xpart - 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X - xpart;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    else if (lblRescue.Location.X < lblPoint.Location.X && lblRescue.Location.Y == lblPoint.Location.Y)
                    {
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X))
                        {
                            newCordinationX = lblRescue.Location.X + 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else if(i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X + xpart + 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        else
                        {
                            newCordinationX = lblRescue.Location.X + xpart;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                    }
                    if (lblRescue.InvokeRequired)
                        lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.BringToFront(); });
                    else
                        lblRescue.BringToFront();

                    //
                    System.Drawing.Point MyPoint = new System.Drawing.Point(newCordinationX, newCordinationY);
                    DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                    //

                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        //Update to sql in TbPoint
                        var rescue = (from TbRescues in dbUpdate.TbRescues
                                      where TbRescues.Rescue_ID == selectedRescue.ID
                                      select TbRescues).Single();
                        rescue.Rescue_LeftCoordinate = newCordinationX;
                        rescue.Rescue_TopCoordinate = newCordinationY;
                        rescue.Rescue_LeftProjection = MyCoordinate.X;
                        rescue.Rescue_TopProjection = MyCoordinate.Y;
                        dbUpdate.SubmitChanges();
                    }
                }
                endTime = Math.Floor(ss.Elapsed.TotalSeconds);

                if (lblRescue.InvokeRequired)
                    lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.BringToFront(); });
                else
                    lblRescue.BringToFront();

            

                //***** Do Rescue
                selectedPoint.DoRescue();

                //*****
                selectedPoint.StartRescueDoing = false;
                selectedPoint.EndRescueDoing = true;
                selectedRescue.State = Rescue.RescueStateType.Ready;
                idelRescue++;
                busyRescue--;

                //***** Show new information
                lblPoint.Image = Properties.Resources.Point_Green;
                lock (this)
                {
                    List<string> idOfNewPoints = new List<string>();
                    using (var dbSelect = new DataClasses1DataContext())
                    {
                        idOfNewPoints = (from TbPoints in dbSelect.TbPoints
                                         where TbPoints.Point_ParentID.Equals(selectedPoint.ID)
                                         select TbPoints.Point_ID).ToList();
                    }

                    for (int i = 0; i < idOfNewPoints.Count(); i++)
                    {
                        Label myLabel = (Label)panel1.Controls.Find(idOfNewPoints[i], true)[0];
                        myLabel.Image = Properties.Resources.Point_Green;
                    }
                }
                //
                totalDistanceRescue += GetDistance(pointTop, rescueTop, pointLeft, rescueLeft);
                //totalTimeOfDistanceRescue += (endTime - startTime);
                totalTimeOfDistanceRs += (Math.Round(GetDistance(pointTop, rescueTop, pointLeft, rescueLeft)) / 5);
                //totalTimeOfDistanceRescue +=int.Parse(ComputeCostRescueForPoint(selectedPoint, selectedRescue).ToString());

                //***** Show Chart
                int time = (totalTimeSleep / 1000) + 2;
                numPointDoRescue = numPointDoRescue - countNewPoint;
                numPointDoRescue--;
                ShowChartTasksOfRescue(time);

                //***** Update sql
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    var point = (from TbPoints in dbUpdate.TbPoints
                                 where TbPoints.Point_ID == selectedPoint.ID
                                 select TbPoints).Single();
                    point.Point_StartRescueDoing = selectedPoint.StartRescueDoing;
                    point.Point_EndRescueDoing = selectedPoint.EndRescueDoing;
                    dbUpdate.SubmitChanges();
                    //
                    foreach (var item in newPoints.Where(p => p.ParentID.Equals(selectedPoint.ID)).ToList())
                    {
                        var newPoint = (from TbPoints in dbUpdate.TbPoints
                                     where TbPoints.Point_ID == item.ID
                                     select TbPoints).Single();
                        newPoint.Point_StartRescueDoing = selectedPoint.StartRescueDoing;
                        newPoint.Point_EndRescueDoing = selectedPoint.EndRescueDoing;
                        dbUpdate.SubmitChanges();
                    }
                    //
                    var rescue = (from TbRescues in dbUpdate.TbRescues
                                  where TbRescues.Rescue_ID == selectedRescue.ID
                                  select TbRescues).Single();
                    rescue.Rescue_State = (int)selectedRescue.State;
                    rescue.Rescue_LeftCoordinate = newCordinationX;
                    rescue.Rescue_TopCoordinate = newCordinationY;
                    dbUpdate.SubmitChanges();
                }
             
            }
            selectedRescue.IsDoing = false;
        }
        //**********************************************//
        public DomainObject.Point SelectNextPointForSearch(Search search)
        {
            switch (search.Container)
            {
                case DomainObject.UnitType.NorthWest:
                    return SelectNexPointInNorthWest(search);

                case DomainObject.UnitType.NorthEast:
                    return SelectNexPointInNorthEast(search);

                case DomainObject.UnitType.SouthWest:
                    return SelectNexPointInSouthWest(search);

                case DomainObject.UnitType.SouthEast:
                    return SelectNexPointInSouthEast(search);
            }
            return null;
        }

        public DomainObject.Point SelectNexPointInNorthWest(Search search)
        {
            lock (search)
            {
                List<DomainObject.Point> orderdPointInNorthWest = points.Where(p => p.Container == UnitType.NorthWest && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();
                foreach (var item in orderdPointInNorthWest)
                {
                    //item.Distance = GetDistance(item.Left, search.Left, item.Top, search.Top);
                    item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                orderdPointInNorthWest = orderdPointInNorthWest.OrderBy(p => p.Distance).ToList();
                //
                lock (this)
                {
                    while (orderdPointInNorthWest.Any(p => p.StartSearchDoing.Equals(false)))
                    {
                        //if (orderdPointInNorthWest.Count != 0)
                        //{
                        string nextPointID = orderdPointInNorthWest.First(p => p.StartSearchDoing.Equals(false) && p.EndSearchDoing.Equals(false)).ID;
                        if (points.First(p => p.ID.Equals(nextPointID)).StartSearchDoing.Equals(false) && points.First(p => p.ID.Equals(nextPointID)).EndSearchDoing.Equals(false))
                        {
                            points.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            orderdPointInNorthWest.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            return orderdPointInNorthWest.Find(p => p.ID.Equals(nextPointID));
                        }
                        //}
                    }
                }
            }
            return null;
        }

        public DomainObject.Point SelectNexPointInNorthEast(Search search)
        {
            lock (search)
            {
                List<DomainObject.Point> orderdPointInNorthEast = points.Where(p => p.Container == UnitType.NorthEast && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();
                foreach (var item in orderdPointInNorthEast)
                {
                    //item.Distance = GetDistance(item.Left, search.Left, item.Top, search.Top);
                    item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                orderdPointInNorthEast = orderdPointInNorthEast.OrderBy(p => p.Distance).ToList();
                //
                lock (this)
                {
                    while (orderdPointInNorthEast.Any(p => p.StartSearchDoing.Equals(false)))
                    {
                        //if (orderdPointInNorthWest.Count != 0)
                        //{
                        string nextPointID = orderdPointInNorthEast.First(p => p.StartSearchDoing.Equals(false) && p.EndSearchDoing.Equals(false)).ID;
                        if (points.First(p => p.ID.Equals(nextPointID)).StartSearchDoing.Equals(false) && points.First(p => p.ID.Equals(nextPointID)).EndSearchDoing.Equals(false))
                        {
                            points.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            orderdPointInNorthEast.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            return orderdPointInNorthEast.Find(p => p.ID.Equals(nextPointID));
                        }
                        //}
                    }
                }
            }
            return null;
        }

        public DomainObject.Point SelectNexPointInSouthWest(Search search)
        {
            lock (search)
            {
                List<DomainObject.Point> orderdPointInSouthWest = points.Where(p => p.Container == UnitType.SouthWest && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();
                foreach (var item in orderdPointInSouthWest)
                {
                    //item.Distance = GetDistance(item.Left, search.Left, item.Top, search.Top);
                    item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                orderdPointInSouthWest = orderdPointInSouthWest.OrderBy(p => p.Distance).ToList();
                //
                lock (this)
                {
                    while (orderdPointInSouthWest.Any(p => p.StartSearchDoing.Equals(false)))
                    {
                        //if (orderdPointInNorthWest.Count != 0)
                        //{
                        string nextPointID = orderdPointInSouthWest.First(p => p.StartSearchDoing.Equals(false) && p.EndSearchDoing.Equals(false)).ID;
                        if (points.First(p => p.ID.Equals(nextPointID)).StartSearchDoing.Equals(false) && points.First(p => p.ID.Equals(nextPointID)).EndSearchDoing.Equals(false))
                        {
                            points.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            orderdPointInSouthWest.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            return orderdPointInSouthWest.Find(p => p.ID.Equals(nextPointID));
                        }
                        //}
                    }
                }
            }
            return null;
        }

        public DomainObject.Point SelectNexPointInSouthEast(Search search)
        {
            lock (search)
            {
                List<DomainObject.Point> orderdPointInSouthEast = points.Where(p => p.Container == UnitType.SouthEast && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();
                foreach (var item in orderdPointInSouthEast)
                {
                    //item.Distance = GetDistance(item.Left, search.Left, item.Top, search.Top);
                    item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                orderdPointInSouthEast = orderdPointInSouthEast.OrderBy(p => p.Distance).ToList();
                //
                lock (this)
                {
                    //if (orderdPointInNorthWest.Count != 0 && orderdPointInNorthWest.Any(p => p.StartSearchDoing.Equals(false)))
                    //    return orderdPointInNorthWest.First(p => p.StartSearchDoing.Equals(false));
                    //else
                    //    return null;
                    while (orderdPointInSouthEast.Any(p => p.StartSearchDoing.Equals(false)))
                    {
                        //if (orderdPointInNorthWest.Count != 0)
                        //{
                        string nextPointID = orderdPointInSouthEast.First(p => p.StartSearchDoing.Equals(false) && p.EndSearchDoing.Equals(false)).ID;
                        if (points.First(p => p.ID.Equals(nextPointID)).StartSearchDoing.Equals(false) && points.First(p => p.ID.Equals(nextPointID)).EndSearchDoing.Equals(false))
                        {
                            points.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            orderdPointInSouthEast.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            return orderdPointInSouthEast.Find(p => p.ID.Equals(nextPointID));
                        }
                        //}
                    }
                }
            }
            return null;
        }
        //***********************************************//
        

        private void btnStop_Click(object sender, EventArgs e)
        {
          //ManualResetEvent  ewh2 = new ManualResetEvent(true);
          //re.Reset(); 

            if (btnStop.Tag.Equals("run"))
            {
                btnGo.Enabled = false;
                btnStop.Tag = "stop";
                object b=new object();
                //re.Reset(); ;//.Reset();
                foreach (Thread t in AllSearchThreads)
                {
                    if (t.ThreadState.Equals(System.Threading.ThreadState.Running) || t.ThreadState.Equals(System.Threading.ThreadState.WaitSleepJoin))
                    {
                        t.Suspend();
                    }
                }

                foreach (Thread t in AllRescueThreads)
                {
                    if (t.ThreadState.Equals(System.Threading.ThreadState.Running) || t.ThreadState.Equals(System.Threading.ThreadState.WaitSleepJoin))
                    {
                        t.Suspend();
                    }
                }
                ss.Stop();
            }
            else if (btnStop.Tag.Equals("stop"))
            {
                btnStop.Tag = "run";
                foreach (Thread t in AllSearchThreads)
                {
                    //if (t.ThreadState.Equals(System.Threading.ThreadState.Suspended) || t.ThreadState.Equals(System.Threading.ThreadState.WaitSleepJoin))
                    if (t.ThreadState.Equals(System.Threading.ThreadState.Suspended))
                    {
                        t.Resume();
                    }
                    else if (t.ThreadState.Equals(System.Threading.ThreadState.SuspendRequested | System.Threading.ThreadState.WaitSleepJoin))
                    {
                        t.Resume();
                    }
                }
                foreach (Thread t in AllRescueThreads)
                {
                    if (t.ThreadState.Equals(System.Threading.ThreadState.Suspended) || t.ThreadState.Equals(System.Threading.ThreadState.WaitSleepJoin))
                        if (t.ThreadState.Equals(System.Threading.ThreadState.Suspended))
                        {
                            t.Resume();
                        }
                        else if (t.ThreadState.Equals(System.Threading.ThreadState.SuspendRequested | System.Threading.ThreadState.WaitSleepJoin))
                        {
                            t.Resume();
                        }
                }
                ss.Start();

                for (int index = panel1.Controls.Count - 1; index >= 0; index--)
                {
                    if (panel1.Controls[index].Name.StartsWith("arrow"))
                    {
                        panel1.Controls.RemoveAt(index);
                    }
                }
            }
            
        }

        private void btnShowInformation_Click(object sender, EventArgs e)
        {
            int searchId = (int) cboxSearchIntID.SelectedItem;
            string searchID = "", searchIntID = "", searchTop = "", searchLeft = "", searchX = "", searchY = "",searchState="";
            string rescueID = "", rescueIntID = "", rescueTop = "", rescueLeft = "", rescueX = "", rescueY = "", pointID = "";
            string pointIntID="", pointRescueLevel = "", pointNumVictim = "";
            bool isAccepted = false;

            BindDropDownSearchState();
            BindDropDownRescueLevel();

            for (int index = panel1.Controls.Count - 1; index >= 0; index--)
            {
                if (panel1.Controls[index].Name.StartsWith("arrow"))
                {
                    panel1.Controls.RemoveAt(index);
                }
            }

            using (var dbSelect = new DataClasses1DataContext())
            {
                //
                var search = (from TbSearchs in dbSelect.TbSearches
                              where TbSearchs.Search_IntID.Equals(searchId)
                              select TbSearchs).Single();
                searchID = search.Search_ID.ToString();
                searchIntID = search.Search_IntID.ToString();
                searchLeft = search.Search_LeftCoordinate.ToString();
                searchTop = search.Search_TopCoordinate.ToString();
                searchX = search.Search_LeftProjection.ToString();
                searchY = search.Search_TopProjection.ToString();
                searchState = Search.GetSearchState(search.Search_State).ToString();
                //
                txtSearchID.Text = searchIntID;
                txtSearchX.Text = searchX;
                txtSearchY.Text = searchY;
                //txtTeamState.Text = searchState;
                comboxSearchState.SelectedIndex = comboxSearchState.FindStringExact(searchState);
                //
                if (dbSelect.TbTaskLists.Any(p => p.Search_ID == searchID))
                {
                    var task = (from TbTaskLists in dbSelect.TbTaskLists
                                where TbTaskLists.Search_ID.Equals(searchID) && TbTaskLists.Point_ParentID.Equals(0)
                                select TbTaskLists).ToList().Last();

                    pointID = task.Point_ID;

                    if (task.Rescue_ID != null)
                    {
                        var rescue = (from TbRescues in dbSelect.TbRescues
                                      where TbRescues.Rescue_ID.Equals(task.Rescue_ID)
                                      select TbRescues).Single();

                        rescueID = rescue.Rescue_ID.ToString();
                        rescueIntID = rescue.Rescue_IntID.ToString();
                        rescueLeft = rescue.Rescue_LeftCoordinate.ToString();
                        rescueTop = rescue.Rescue_TopCoordinate.ToString();
                        rescueX = rescue.Rescue_LeftProjection.ToString();
                        rescueY = rescue.Rescue_TopProjection.ToString();
                        isAccepted = true;
                    }

                    var point = (from TbPoints in dbSelect.TbPoints
                                 where TbPoints.Point_ID.Equals(pointID)
                                 select TbPoints).Single();

                    pointIntID = point.Point_IntID.ToString();
                    pointRescueLevel = DomainObject.Point.GetRescueLevel(point.Point_NumVictim).ToString();
                    pointNumVictim = point.Point_NumVictim.ToString();
                }
                //
                txtRescueID.Text = rescueIntID;
                txtRescueX.Text = rescueX;
                txtRescueY.Text = rescueY;
                radioAccept.Checked = isAccepted;
                radioReject.Checked = isAccepted;
                //
                txtPointID.Text = pointIntID;
                //txtRescueLevel.Text = pointRescueLevel;
                comboxRescueLevel.SelectedIndex = comboxRescueLevel.FindStringExact(pointRescueLevel);
                txtNumVictim.Text = pointNumVictim;
                //
                txtIntefraceRescueID.Text = rescueIntID;
                txtInterfaceRescueX.Text = rescueX;
                txtIntefraceRescueY.Text = rescueY;
                //
                if (rescueID != "" || rescueID != null)
                {
                    var rescueTaskList = from p in dbSelect.TbTaskLists
                                         join q in dbSelect.TbPoints on p.Point_ID equals q.Point_ID
                                         where (p.Rescue_ID == rescueID && p.Point_ID == pointID) || (p.Rescue_ID == rescueID && q.Point_ParentID == pointID)
                                         select new
                                         {
                                             q.Point_IntID,
                                             q.Point_LeftProjection,
                                             q.Point_TopProjection,
                                             Point_RescueLevel = DomainObject.Point.GetRescueLevel(q.Point_NumVictim),
                                             q.Point_NumVictim,
                                             p.Priority
                                         };


                    dgvRescueTasks.DataSource = rescueTaskList.ToList();
                }
                //
            }

            for (int index = panel1.Controls.Count - 1; index >= 0; index--)
            {
                if (panel1.Controls[index].Name.ToLower().Equals(searchID))
                {
                    Label lbl = new Label();
                    lbl.Name = "arrow_" + Guid.NewGuid().ToString();
                    lbl.Width = 15;
                    lbl.Height = 15;
                    lbl.Image = Properties.Resources.Arrow3;
                    int left = int.Parse(searchLeft);
                    int top = int.Parse(searchTop)-17;
                    lbl.Location = new System.Drawing.Point(left, top);
                    panel1.Controls.Add(lbl);
                    geoMap.SendToBack();
                }

                if (panel1.Controls[index].Name.ToLower().Equals(rescueID))
                {
                    Label lbl = new Label();
                    lbl.Name = "arrow_" + Guid.NewGuid().ToString();
                    lbl.Width = 15;
                    lbl.Height = 15;
                    lbl.Image = Properties.Resources.Arrow3;
                    int left = int.Parse(rescueLeft);
                    int top = int.Parse(rescueTop) - 17;
                    lbl.Location = new System.Drawing.Point(left, top);
                    panel1.Controls.Add(lbl);
                    geoMap.SendToBack();
                }
            }
        }
        //***********************************************//
    }
}


