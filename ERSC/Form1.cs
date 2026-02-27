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
    // Main form class for the ERSC application
    public partial class Form1 : Form
    {
        //public DotSpatial.Controls.Map geoMap = new DotSpatial.Controls.Map();
        public Form1()
        {
            // Initialize database context and main data lists
            db = new DataClasses1DataContext();
            points = new List<DomainObject.Point>();
            newPoints = new List<DomainObject.Point>();
            searchs = new List<DomainObject.Search>();
            rescues = new List<Rescue>();
            InitializeComponent();
            AddMap(); // Add and configure the map control
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
            // Add and configure map layers for different city features
            geoMap.Name = "geomap";
            // Add shapefile layers for points, lines, polygons, blocks, parks, units, boulevards, and roads
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
            // Set symbology (color, style) for each map layer
            // Block1 lines
            DotSpatial.Controls.MapLineLayer lineBlock1 = default(DotSpatial.Controls.MapLineLayer);
            lineBlock1 = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[1];
            LineScheme schemeLineBlock1 = new LineScheme();
            schemeLineBlock1.Categories.Clear();
            LineCategory categoryLine1 = new LineCategory(Color.Gray, Color.Gray, 1, System.Drawing.Drawing2D.DashStyle.Solid, System.Drawing.Drawing2D.LineCap.Flat);
            schemeLineBlock1.Categories.Add(categoryLine1);
            lineBlock1.Symbology = schemeLineBlock1;
            // Block1 polygons
            DotSpatial.Controls.MapPolygonLayer polygBlock1 = default(DotSpatial.Controls.MapPolygonLayer);
            polygBlock1 = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[2];
            PolygonScheme schemePolygBlock1 = new PolygonScheme();
            schemePolygBlock1.Categories.Clear();
            PolygonCategory categoryBlock1 = new PolygonCategory(Color.Beige, Color.Gray, 1);
            schemePolygBlock1.Categories.Add(categoryBlock1);
            polygBlock1.Symbology = schemePolygBlock1;
            // Block lines
            DotSpatial.Controls.MapLineLayer lineBlock = default(DotSpatial.Controls.MapLineLayer);
            lineBlock = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[3];
            LineScheme schemeLineBlock = new LineScheme();
            schemeLineBlock.Categories.Clear();
            LineCategory categoryLine = new LineCategory(Color.LightGray, Color.LightGray, 1, System.Drawing.Drawing2D.DashStyle.Solid, System.Drawing.Drawing2D.LineCap.Flat);
            schemeLineBlock.Categories.Add(categoryLine);
            lineBlock.Symbology = schemeLineBlock;
            // Block polygons
            DotSpatial.Controls.MapPolygonLayer polygBlock = default(DotSpatial.Controls.MapPolygonLayer);
            polygBlock = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[4];
            PolygonScheme schemePolygBlock = new PolygonScheme();
            schemePolygBlock.Categories.Clear();
            PolygonCategory categoryBlock = new PolygonCategory(Color.Beige, Color.Gray, 1);
            schemePolygBlock.Categories.Add(categoryBlock);
            polygBlock.Symbology = schemePolygBlock;
            // Park lines
            DotSpatial.Controls.MapLineLayer linePark = default(DotSpatial.Controls.MapLineLayer);
            linePark = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[5];
            LineScheme schemeLinePark = new LineScheme();
            schemeLineBlock.Categories.Clear();
            LineCategory categoryLinePark = new LineCategory(Color.LightGreen, Color.LightGreen, 2, System.Drawing.Drawing2D.DashStyle.Solid, System.Drawing.Drawing2D.LineCap.Flat);
            schemeLineBlock.Categories.Add(categoryLine);
            schemeLinePark.Categories.Add(categoryLinePark);
            linePark.Symbology = schemeLinePark;
            // Park polygons
            DotSpatial.Controls.MapPolygonLayer polygPark = default(DotSpatial.Controls.MapPolygonLayer);
            polygPark = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[6];
            PolygonScheme schemePolygPark = new PolygonScheme();
            schemePolygPark.Categories.Clear();
            PolygonCategory categoryPolygPark = new PolygonCategory(Color.PaleGreen, Color.PaleGreen, 2);
            schemePolygPark.Categories.Add(categoryPolygPark);
            polygPark.Symbology = schemePolygPark;
            // Unit lines
            DotSpatial.Controls.MapLineLayer lineUnit = default(DotSpatial.Controls.MapLineLayer);
            lineUnit = (DotSpatial.Controls.MapLineLayer)geoMap.Layers[7];
            LineScheme schemelineUnit = new LineScheme();
            schemeLineBlock.Categories.Clear();
            LineCategory categorylineUnit = new LineCategory(Color.Salmon, 2);
            schemelineUnit.Categories.Add(categorylineUnit);
            lineUnit.Symbology = schemelineUnit;
            // Boulevard polygons
            DotSpatial.Controls.MapPolygonLayer polygBolvar = default(DotSpatial.Controls.MapPolygonLayer);
            polygBolvar = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[8];
            PolygonScheme schemePolygBolvar = new PolygonScheme();
            schemePolygBolvar.Categories.Clear();
            PolygonCategory categoryPolygBolvar = new PolygonCategory(Color.DarkKhaki, Color.Tan, 1);
            schemePolygBolvar.Categories.Add(categoryPolygBolvar);
            polygBolvar.Symbology = schemePolygBolvar;
            // Road polygons
            DotSpatial.Controls.MapPolygonLayer polygRod = default(DotSpatial.Controls.MapPolygonLayer);
                            // --- Begin: Automata Learning and Rescue Assignment Logic for SouthEast ---
                            // Penalty and probability variables initialization
            polygRod = (DotSpatial.Controls.MapPolygonLayer)geoMap.Layers[9];
            PolygonScheme schemepolygRod = new PolygonScheme();
                            // Retrieve competitor SteamIDs for the winning rescue
            schemepolygRod.Categories.Clear();
            PolygonCategory categorypolygRod = new PolygonCategory(Color.LightBlue, Color.LightBlue, 2);
                            // Dictionary to store Search_ID and Probability for unassigned tasks for the winning rescue
            schemepolygRod.Categories.Add(categorypolygRod);
            polygRod.Symbology = schemepolygRod;
            // Add map to panel
                                // Query task list for the winning rescue and build dictionary
            panel1.Controls.Add(geoMap);
        }
        //**********************//
        // Handles the setup of points on the map and resets all related UI and data
        private void btnSetupPoint_Click(object sender, EventArgs e)
        {
            // Disable main action buttons during setup
                            // If only one competitor or one probability, handle win/loss and update probabilities
            btnGo.Enabled = false;
            btnStop.Enabled = false;
            btnShowInformation.Enabled = false;
            // Reset stopwatch and clear charts
            ss.Reset();
            chartTasksOfSearch.Series.Clear();
            chartTasksOfRescue.Series.Clear();
            seriesTasksOfSearch.Points.Clear();
                                // Check if the current rescue is the closest
            seriesTasksOfRescue.Points.Clear();
            // Clear probability charts
                                    // Win case: update probabilities positively
            chartProbability.Series.Clear();
            chartProbability_S1.Series.Clear();
            chartProbability_S2.Series.Clear();
            chartProbability_S3.Series.Clear();
            // Remove all previous tasks and points from database and UI
            DeleteTaskListFromDataBase();
            DeletePoints();
                                    // Random chance for win/loss, update probabilities accordingly
            // Create new random points and display them
            CreatPoints();
            ShowPoints();
            // Enable or disable controls based on whether points exist
            if (points.Count > 0)
            {
                btnSetupRS.Enabled = true;
                nudSearch.Enabled = true;
                nudRescue.Enabled = true;
            }
            else
                                        // Penalty case: update probabilities with penalty
            {
                btnSetupRS.Enabled = false;
                nudSearch.Enabled = false;
                nudRescue.Enabled = false;
            }
            // Reset statistics and UI fields
            totalDistanceSearch = 0;
            totalDistanceRescue = 0;
            totalTimeOfDistanceRescue = 0;
            totalTimeOfDistanceRs = 0;
            txtAverageTimeOfRescue.Text = "";
            txtAvarageBusyResc.Text = "";
            txtIdelRescue.Text = "";
            txtBusyRescue.Text = "";
                                // Multiple competitors: synchronize threads and determine winner
            efficiencyBusyRescue = 0;
            numBusyRescue = 0;
            // Clear all thread and selection tracking lists
                                    // Thread synchronization for single probability case
            AllSearchThreads.Clear();
            AllRescueThreads.Clear();
            dicIsGivedBestSelectInNorthWest.Clear();
            dicIsGivedBestSelectInNorthEast.Clear();
            dicIsGivedBestSelectInSouthWest.Clear();
            dicIsGivedBestSelectInSouthEast.Clear();
            // Reset thread counters for all regions
            requestedCountThreadInNorthWest = 0;
            requestedThisSectionCountThreadInNorthWest = 0;
            currentCountThreadInNorthWest = 0;
            currentCountThreadInNorthWestBeforStart = 0;
            currentCountThreadInNorthWestStart = 0;
            currentCountThreadInNorthWestEnd = 0;
            currentCountThreadInNorthWestAfterEnd = 0;
            currentThisSectionCountThreadInNorthWest = 0;
            requestedCountThreadInNorthEast = 0;
            requestedThisSectionCountThreadInNorthEast = 0;
            currentCountThreadInNorthEast = 0;
            currentCountThreadInNorthEastBeforStart = 0;
            currentCountThreadInNorthEastStart = 0;
            currentCountThreadInNorthEastEnd = 0;
            currentCountThreadInNorthEastAfterEnd = 0;
            currentThisSectionCountThreadInNorthEast = 0;
            requestedCountThreadInSouthWest = 0;
                                    // Find winner by max probability
            requestedThisSectionCountThreadInSouthWest = 0;
            currentCountThreadInSouthWest = 0;
            currentCountThreadInSouthWestBeforStart = 0;
            currentCountThreadInSouthWestStart = 0;
            currentCountThreadInSouthWestEnd = 0;
                                        // Win case: update probabilities
            currentCountThreadInSouthWestAfterEnd = 0;
            currentThisSectionCountThreadInSouthWest = 0;
            //
            requestedCountThreadInSouthEast = 0;
            requestedThisSectionCountThreadInSouthEast = 0;
            currentCountThreadInSouthEast = 0;
            currentCountThreadInSouthEastBeforStart = 0;
                                        // Penalty case: update probabilities
            currentCountThreadInSouthEastStart = 0;
            currentCountThreadInSouthEastEnd = 0;
            currentCountThreadInSouthEastAfterEnd = 0;
            currentThisSectionCountThreadInSouthEast = 0;
        }

        // Handles the setup of search and rescue teams based on the distribution of points
        private void btnSetupSR_Click(object sender, EventArgs e)
        {
                                    // Complex case: evaluate best rescue for another search
            // Calculate the percentage of points in each region
            decimal northWestCriticalPointsCount, southWestCriticalPointsCount, norstEastCriticalPointsCount, southEastCriticalPointsCount;
            northWestCriticalPointsCount = (points.Where(p => p.Container == UnitType.NorthWest).Count() * 100) / points.Count();
            southWestCriticalPointsCount = (points.Where(p => p.Container == UnitType.SouthWest).Count() * 100) / points.Count();
            norstEastCriticalPointsCount = (points.Where(p => p.Container == UnitType.NorthEast).Count() * 100) / points.Count();
            southEastCriticalPointsCount = (points.Where(p => p.Container == UnitType.SouthEast).Count() * 100) / points.Count();

            // Assign search team counts to each region proportionally
            decimal searchCount = nudSearch.Value;
                                    // Determine second best rescue
            Dictionary<UnitType,int> dicUnitSearchCount=new Dictionary<UnitType,int>();

            Unit.NorthWestSearchCount = ((int)Math.Round((searchCount * northWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * northWestCriticalPointsCount) / 100);
            dicUnitSearchCount.Add(UnitType.NorthWest,Unit.NorthWestSearchCount);
            Unit.NorthEastSearchCount = ((int)Math.Round((searchCount * norstEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * norstEastCriticalPointsCount) / 100);
            dicUnitSearchCount.Add(UnitType.NorthEast,Unit.NorthEastSearchCount);
            Unit.SouthEastSearchCount = ((int)Math.Round((searchCount * southEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * southEastCriticalPointsCount) / 100);
            dicUnitSearchCount.Add(UnitType.SouthEast,Unit.SouthEastSearchCount);
            Unit.SouthWestSearchCount = ((int)Math.Round((searchCount * southWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((searchCount * southWestCriticalPointsCount) / 100);
                                        // For each competitor, compute max probability for other rescues
            dicUnitSearchCount.Add(UnitType.SouthWest,Unit.SouthWestSearchCount);
            // Adjust search team counts if rounding caused mismatch
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
                                                // Select point for each competitor and compute probability
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
                                                // Check if this rescue is best for another search
                    case UnitType.SouthEast:
                        Unit.SouthEastSearchCount++;
                        break;
                }
            // Assign rescue team counts to each region proportionally
                                        // Find minimum probability and winner
            decimal rescueCount = nudRescue.Value;
            Dictionary<UnitType, int> dicUnitRescueCount = new Dictionary<UnitType, int>();

            Unit.NorthWestRescueCount = ((int)Math.Round((rescueCount * northWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * northWestCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.NorthWest, Unit.NorthWestRescueCount);
                                    // Thread synchronization for end of section
            Unit.NorthEastRescueCount = ((int)Math.Round((rescueCount * norstEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * norstEastCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.NorthEast, Unit.NorthEastRescueCount);
            Unit.SouthEastRescueCount = ((int)Math.Round((rescueCount * southEastCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * southEastCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.SouthEast, Unit.SouthEastRescueCount);
            Unit.SouthWestRescueCount = ((int)Math.Round((rescueCount * southWestCriticalPointsCount) / 100)).Equals(0) ? 1 : (int)Math.Round((rescueCount * southWestCriticalPointsCount) / 100);
            dicUnitRescueCount.Add(UnitType.SouthWest, Unit.SouthWestRescueCount);
            // Adjust rescue team counts if rounding caused mismatch
            if (dicUnitRescueCount.Values.Sum() > rescueCount)
            {
                switch (dicUnitRescueCount.OrderByDescending(p => p.Value).First().Key)
                {
                    case UnitType.NorthWest:
                        Unit.NorthWestRescueCount--;
                        break;
                                    // Handle win/loss based on best rescue for another search
                    case UnitType.SouthWest:
                        Unit.SouthWestRescueCount--;
                        break;
                    case UnitType.NorthEast:
                        Unit.NorthEastRescueCount--;
                        break;
                    case UnitType.SouthEast:
                        Unit.SouthEastRescueCount--;
                        break;
                                            // Random chance for penalty or win
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
            // Remove previous search and rescue teams from UI and database
            DeleteSearchAndRescueTeam();
            // Create and display new search and rescue teams
                                            // Penalty case
            CreateSearchTeam();
            ShowSearchTeam();
            CreateRescueTeam();
            ShowRescueTeam();
            // Bind information to dropdowns and update rescue statistics
            BindDropDownShowInformation();
            busyRescue = 0;
            idelRescue = rescues.Count();
            // Enable or disable controls based on whether search teams exist
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
                                            // Win case
        }

        // Handles the start of the simulation: initializes charts, sets up probability series, and starts search threads
        private void btnGo_Click(object sender, EventArgs e)
        {
            // Clear previous chart data
            chartTasksOfSearch.Series.Clear();
                                            // Random chance for win or penalty
            chartTasksOfRescue.Series.Clear();
            seriesTasksOfSearch.Points.Clear();
            seriesTasksOfRescue.Points.Clear();
            // Add main series to charts
            this.chartTasksOfSearch.Series.Add(seriesTasksOfSearch);
            this.chartTasksOfRescue.Series.Add(seriesTasksOfRescue);

            // --- Setup probability chart for selected search ---
            int searchId = 0;
            chartProbability.ChartAreas[0].AxisX.Interval = 5;
            // Get selected search ID (thread-safe for UI)
            if (cboxSearchIntID.InvokeRequired)
                cboxSearchIntID.BeginInvoke((MethodInvoker)delegate
                {
                    searchId = (int)cboxSearchIntID.SelectedItem;
                });
            else
            {
                searchId = (int)cboxSearchIntID.SelectedItem;
            }
            // Create probability series for each rescue in the selected unit
            Series seriesPropability = new System.Windows.Forms.DataVisualization.Charting.Series { };
            listSeries.Clear();
            System.Drawing.Color color = new System.Drawing.Color();
            List<Rescue> rescuesInUnitOfSelectedSearch = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId)).Container)).ToList();
            for (int j = 0; j < rescuesInUnitOfSelectedSearch.Count(); j++)
            {
                // Assign a unique color to each rescue
                switch (j)
                {
                    case 0: color = System.Drawing.Color.Red; break;
                    case 1: color = System.Drawing.Color.Blue; break;
                    case 2: color = System.Drawing.Color.Green; break;
                    case 3: color = System.Drawing.Color.Yellow; break;
                            // Update probabilities and thread counters
                    case 4: color = System.Drawing.Color.Purple; break;
                    case 5: color = System.Drawing.Color.Brown; break;
                    default: color = System.Drawing.Color.Black; break;
                            // Thread synchronization for end of automata learning
                }
                seriesPropability = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability" + j,
                    Color = color,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth=2
                };
                listSeries.Add(seriesPropability);
                            // Time check for automata learning loop
            }
            // Add probability series to chart (thread-safe)
            if (chartProbability.InvokeRequired)
                chartProbability.BeginInvoke((MethodInvoker)delegate
                {
                    this.chartProbability.Series.Clear();
                    foreach (var item in listSeries)
                    {
                            // --- End: Automata Learning and Rescue Assignment Logic for SouthEast ---
                        this.chartProbability.Series.Add(item);
                    }
                        // --- Begin: Rescue Assignment and Efficiency Calculation ---
                });
            else
            {
                this.chartProbability.Series.Clear();
                                // Add winning rescue to list and increment busy counter
                foreach (var item in listSeries)
                {
                    this.chartProbability.Series.Add(item);
                }
            }

            // --- Setup probability charts for search IDs 1, 2, 3 (for comparison/visualization) ---
            // Chart 1
                        // Thread synchronization after rescue assignment
            int searchId_S1 = 1;
            chartProbability_S1.ChartAreas[0].AxisX.Interval = 5;
            Series seriesPropability_S1 = new System.Windows.Forms.DataVisualization.Charting.Series { };
            listSeries_S1.Clear();
            List<Rescue> rescuesInUnitOfSelectedSearch_S1 = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId_S1)).Container)).ToList();
            for (int j = 0; j < rescuesInUnitOfSelectedSearch_S1.Count(); j++)
            {
                switch (j)
                {
                    case 0: color = System.Drawing.Color.Red; break;
                    case 1: color = System.Drawing.Color.Blue; break;
                    case 2: color = System.Drawing.Color.Green; break;
                    case 3: color = System.Drawing.Color.Yellow; break;
                    case 4: color = System.Drawing.Color.Purple; break;
                    case 5: color = System.Drawing.Color.Brown; break;
                    default: color = System.Drawing.Color.Black; break;
                }
                        // Calculate efficiency metrics for rescue assignment
                seriesPropability_S1 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability_S1" + j,
                    Color = color,
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
                        this.chartProbability_S1.Series.Add(item);
                        // Clear SteamIDs and best select dictionary for next round
                    }
                });
            else
                        // Delete task list entries for the current point from database
            {
                this.chartProbability_S1.Series.Clear();
                foreach (var item in listSeries_S1)
                {
                    this.chartProbability_S1.Series.Add(item);
                }
            }
            // Chart 2
                        // Return the winning rescue ID
            int searchId_S2 = 2;
                        // --- End: Rescue Assignment and Efficiency Calculation ---
            chartProbability_S2.ChartAreas[0].AxisX.Interval = 5;
            Series seriesPropability_S2 = new System.Windows.Forms.DataVisualization.Charting.Series { };
                    // --- Begin: Utility Methods for Probability and Rescue Assignment ---
            listSeries_S2.Clear();
            List<Rescue> rescuesInUnitOfSelectedSearch_S2 = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId_S2)).Container)).ToList();
                        // Select a rescue based on probability distribution
            for (int j = 0; j < rescuesInUnitOfSelectedSearch_S2.Count(); j++)
            {
                switch (j)
                {
                    case 0: color = System.Drawing.Color.Red; break;
                    case 1: color = System.Drawing.Color.Blue; break;
                    case 2: color = System.Drawing.Color.Green; break;
                    case 3: color = System.Drawing.Color.Yellow; break;
                    case 4: color = System.Drawing.Color.Purple; break;
                    case 5: color = System.Drawing.Color.Brown; break;
                    default: color = System.Drawing.Color.Black; break;
                }
                seriesPropability_S2 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability_S2" + j,
                    Color = color,
                    IsVisibleInLegend = false,
                        // Return selected rescue ID
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth = 2
                };
                listSeries_S2.Add(seriesPropability_S2);
                        // Compute new probabilities for rescues after learning step
            }
            if (chartProbability_S2.InvokeRequired)
                chartProbability_S2.BeginInvoke((MethodInvoker)delegate
                {
                    this.chartProbability_S2.Series.Clear();
                    foreach (var item in listSeries_S2)
                    {
                        this.chartProbability_S2.Series.Add(item);
                    }
                });
            else
            {
                this.chartProbability_S2.Series.Clear();
                foreach (var item in listSeries_S2)
                {
                    this.chartProbability_S2.Series.Add(item);
                }
            }
            // Chart 3
            int searchId_S3 = 3;
            chartProbability_S3.ChartAreas[0].AxisX.Interval =5;
            Series seriesPropability_S3 = new System.Windows.Forms.DataVisualization.Charting.Series { };
            listSeries_S3.Clear();
                        // Return updated probability dictionary
            List<Rescue> rescuesInUnitOfSelectedSearch_S3 = rescues.Where(p => p.Container.Equals(searchs.Single(q => q.IntID.Equals(searchId_S3)).Container)).ToList();
            for (int j = 0; j < rescuesInUnitOfSelectedSearch_S3.Count(); j++)
            {
                switch (j)
                {
                        // Compute new probabilities for rescues with penalty applied
                    case 0: color = System.Drawing.Color.Red; break;
                    case 1: color = System.Drawing.Color.Blue; break;
                    case 2: color = System.Drawing.Color.Green; break;
                    case 3: color = System.Drawing.Color.Yellow; break;
                    case 4: color = System.Drawing.Color.Purple; break;
                    case 5: color = System.Drawing.Color.Brown; break;
                    default: color = System.Drawing.Color.Black; break;
                }
                seriesPropability_S3 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "seriesPropability_S3" + j,
                    Color = color,
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
                        // Return updated probability dictionary
                    {
                        this.chartProbability_S3.Series.Add(item);
                    }
                    // --- End: Utility Methods for Probability and Rescue Assignment ---
                });
            else
                        // Assign a point to the winning rescue and update database
            {
                this.chartProbability_S3.Series.Clear();
                foreach (var item in listSeries_S3)
                {
                    this.chartProbability_S3.Series.Add(item);
                }
            }

            // --- Start simulation ---
                                    // Insert new points and main point as assigned tasks for winning rescue
            numPointDoSearch = points.Count();
            numPointDoRescue = points.Count();
            seriesTasksOfSearch.Points.Add(numPointDoSearch);
            seriesTasksOfRescue.Points.Add(numPointDoRescue);
            ss.Start(); // Start stopwatch
            // Assign points to search threads for each region
            AllSearchThreads.Clear();
            AllSearchThreads.AddRange(AssignePointToSearchInNorthWest());
            AllSearchThreads.AddRange(AssignePointToSearchInNorthEast());
            AllSearchThreads.AddRange(AssignePointToSearchInSouthWest());
            AllSearchThreads.AddRange(AssignePointToSearchInSouthEast());
            // Start all search threads
            foreach (var thread in AllSearchThreads)
            {
                thread.Start();
            }
                                    // Insert main point as assigned task
            // Start timer to update UI
            ShowTime();
        }
        //**********************//
        // Removes all points from the UI and database
        void DeletePoints()
        {
            // Remove all controls except the map from the panel
            for (int index = panel1.Controls.Count - 1; index >= 0; index--)
            {
                if (!panel1.Controls[index].Name.Equals("geomap"))
                {
                    panel1.Controls.RemoveAt(index);
                }
            }

            // Delete all point records from the database (TbPoints and TbPointFirsts)
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getDataPoints = (from TbPoint in dbDelete.TbPoints select TbPoint).ToList();
                dbDelete.TbPoints.DeleteAllOnSubmit(getDataPoints.ToList());
                dbDelete.SubmitChanges();
            }

            using (var dbDelete = new DataClasses1DataContext())
            {
                var getDataPoints = (from TbPointFirst in dbDelete.TbPointFirsts select TbPointFirst).ToList();
                                    // Insert new points and main point as assigned tasks for non-winning rescue
                dbDelete.TbPointFirsts.DeleteAllOnSubmit(getDataPoints.ToList());
                dbDelete.SubmitChanges();
            }
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
                                    // Insert main point as assigned task
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
                        // Start rescue thread for assigned point

        // Displays all points on the map and saves them to the database
        void ShowPoints()
        {
            List<TbPoint> tbPoints = new List<TbPoint>();
            int count = 0;
            foreach (var p in points)
            {
                // Add point shape to the UI
                        // Perform rescue operation for a given point, update UI and database
                panel1.Controls.Add(p.GetShape());
                // Prepare point data for database
                TbPoint point = new TbPoint();
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
                            // Set rescue state and update counters
                point.Point_IsAllocatedRTeam = p.IsAllocatedRTeam;
                point.Point_StartSearchDoing = p.StartSearchDoing;
                point.Point_StartRescueDoing = p.StartRescueDoing;
                point.Point_EndSearchDoing = p.EndSearchDoing;
                point.Point_EndRescueDoing = p.EndRescueDoing;
                tbPoints.Add(point);
            }
            // Save all points to the main table
            db.TbPoints.InsertAllOnSubmit(tbPoints);
            db.SubmitChanges();
                            // Set point allocation and start rescue
            geoMap.SendToBack();
            // Save a copy to the 'first' table for initial state tracking
            List<TbPointFirst> tbPointFirsts = new List<TbPointFirst>();
            foreach (var p in tbPoints)
                            // Update point and rescue state in database
            {
                TbPointFirst pointFirst = new TbPointFirst();
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
                            // Move rescue label towards point label in UI
                pointFirst.Point_StartRescueDoing = p.Point_StartRescueDoing;
                pointFirst.Point_EndSearchDoing = p.Point_EndSearchDoing;
                pointFirst.Point_EndRescueDoing = p.Point_EndRescueDoing;
                tbPointFirsts.Add(pointFirst);
            }
            db.TbPointFirsts.InsertAllOnSubmit(tbPointFirsts);
            db.SubmitChanges();
        }
        //*********************//
        // Removes all search and rescue team shapes from the UI and database
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
                            // Animate rescue movement in steps

        // Removes all search and rescue team records from the database (main and 'first' tables)
        void DeleteSearchAndRescueFromDataBase()
        {
            // Delete all search team records
                                // Move rescue label based on quadrant
            using (var dbDeleteSearch = new DataClasses1DataContext())
            {
                var getDataSearchs = (from TbSearch in dbDeleteSearch.TbSearches select TbSearch);
                dbDeleteSearch.TbSearches.DeleteAllOnSubmit(getDataSearchs);
                dbDeleteSearch.SubmitChanges();
            }
            // Delete all rescue team records
            using (var dbDeleteRescue = new DataClasses1DataContext())
            {
                var getDataRescues = (from TbRescue in dbDeleteRescue.TbRescues select TbRescue);
                dbDeleteRescue.TbRescues.DeleteAllOnSubmit(getDataRescues);
                dbDeleteRescue.SubmitChanges();
            }
            // Delete all initial search team records
            using (var dbDeleteSearch = new DataClasses1DataContext())
            {
                var getDataSearchs = (from TbSearchFirst in dbDeleteSearch.TbSearchFirsts select TbSearchFirst);
                dbDeleteSearch.TbSearchFirsts.DeleteAllOnSubmit(getDataSearchs);
                dbDeleteSearch.SubmitChanges();
            }
            // Delete all initial rescue team records
            using (var dbDeleteRescue = new DataClasses1DataContext())
            {
                var getDataRescues = (from TbRescueFirst in dbDeleteRescue.TbRescueFirsts select TbRescueFirst);
                dbDeleteRescue.TbRescueFirsts.DeleteAllOnSubmit(getDataRescues);
                dbDeleteRescue.SubmitChanges();
            }
        }

        // Removes all task list records from the database (main and 'first' tables)
        void DeleteTaskListFromDataBase()
        {
            var getDataTaskList=(from TbTaskList in db.TbTaskLists select TbTaskList);
            db.TbTaskLists.DeleteAllOnSubmit(getDataTaskList.ToList());
            db.SubmitChanges();
            var getDataTaskListFirst = (from TbTaskListFirst in db.TbTaskListFirsts select TbTaskListFirst);
            db.TbTaskListFirsts.DeleteAllOnSubmit(getDataTaskListFirst.ToList());
            db.SubmitChanges();
        }

        // Creates search team members for each region and assigns them random positions
        void CreateSearchTeam()
        {
            List<DomainObject.Point> orderdPointInNorthWest = points.Where(p => p.Container == UnitType.NorthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<DomainObject.Point> orderdPointInSouthWest = points.Where(p => p.Container == UnitType.SouthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<DomainObject.Point> orderdPointInNorthEast = points.Where(p => p.Container == UnitType.NorthEast).OrderByDescending(p => p.NumVictim).ToList();
            List<DomainObject.Point> orderdPointInSouthEast = points.Where(p => p.Container == UnitType.SouthEast).OrderByDescending(p => p.NumVictim).ToList();

            int count = 0; 
            searchs.Clear();          
            Random r = new Random();
            // Create searchers for NorthWest
            for (int i = 1; i <= Unit.NorthWestSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.StartX, (Map.EndX / 2)-15);
                search.Top = r.Next(Map.StartY, (Map.EndY / 2)-20);
                // Convert coordinates to projection system
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                searchs.Add(search);
            }
            // Create searchers for NorthEast
            for (int i = 1; i <= Unit.NorthEastSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.EndX / 2, Map.EndX - 15);
                search.Top = r.Next(Map.StartY, (Map.EndY / 2)-20);
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                searchs.Add(search);
            }
            // Create searchers for SouthWest
            for (int i = 1; i <= Unit.SouthWestSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.StartX, (Map.EndX / 2) - 15);
                search.Top = r.Next(Map.EndY / 2, Map.EndY-20);
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                searchs.Add(search);
            }
            // Create searchers for SouthEast
            for (int i = 1; i <= Unit.SouthEastSearchCount; i++)
            {
                Search search = new Search();
                search.IntID = count++;
                search.Left = r.Next(Map.EndX / 2, Map.EndX - 15);
                search.Top = r.Next(Map.EndY / 2, Map.EndY-20);
                System.Drawing.Point MyPoint = new System.Drawing.Point(search.Left, search.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                search.LeftProjection = MyCoordinate.X;
                search.TopProjection = MyCoordinate.Y;
                search.Container = Unit.GetUnit(search.Left, search.Top);
                search.State = Search.SearchStateType.Ready;
                search.CompetitorProbabilites = new Dictionary<string, double>();
                searchs.Add(search);
            }
        }

        // Displays all search team members on the map and saves them to the database
        void ShowSearchTeam()
        {
            List<TbSearch> tbSearchs = new List<TbSearch>();
            foreach (var s in searchs)
            {
                panel1.Controls.Add(s.GetShape());
                TbSearch search = new TbSearch();
                search.Search_ID = s.ID;
                search.Search_IntID = s.IntID;
                search.Search_TopCoordinate = s.Top;
                search.Search_LeftCoordinate = s.Left;
                search.Search_TopProjection = s.TopProjection;
                search.Search_LeftProjection = s.LeftProjection;
                search.Search_Unit = s.Container.ToString();
                search.Search_State = (int) s.State;
                tbSearchs.Add(search);
            }
            db.TbSearches.InsertAllOnSubmit(tbSearchs);
            db.SubmitChanges();
            geoMap.SendToBack();
            // Save a copy to the 'first' table for initial state tracking
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
                tbSearchFirsts.Add(search);
            }
            db.TbSearchFirsts.InsertAllOnSubmit(tbSearchFirsts);
            db.SubmitChanges();
        }

        // Creates rescue team members for each region and assigns them random positions avoiding overlap with searchers
        void CreateRescueTeam()
        {
            rescues.Clear();
            Random r = new Random();
            int leftNorthWest = 0;
            int topNorthWest = 0;
            int leftNorthEast = 0;
            int topNorthEast = 0;
            int leftSouthWest = 0;
            int topSouthWest = 0;
            int leftSouthEast = 0;
            int topSouthEast = 0;
            // Create rescue teams for NorthWest
            for (int i = 1; i <= Unit.NorthWestRescueCount; i++)
            {
                leftNorthWest = r.Next(Map.StartX, (Map.EndX/ 2)-15);
                topNorthWest = r.Next(Map.StartY, (Map.EndY / 2)-20);
                // Ensure no overlap with searchers
                while (searchs.Where(p => p.Container.Equals(UnitType.NorthWest)).Select(q => Enumerable.Range(leftNorthWest-15, 30).Contains(q.Left) && Enumerable.Range(topNorthWest-20, 40).Contains(q.Top)).ToList().Exists(x => x.Equals(true)))
                {
                    leftNorthWest = r.Next(Map.StartX, (Map.EndX / 2)-15);
                    topNorthWest = r.Next(Map.StartY, (Map.EndY / 2)-20);
                }
                Rescue resc = new Rescue();
                resc.Left = leftNorthWest;
                resc.Top = topNorthWest;
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }
            // Create rescue teams for NorthEast
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
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }
            // Create rescue teams for SouthWest
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
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }
            // Create rescue teams for SouthEast
            for (int i = 1; i <= Unit.SouthEastRescueCount; i++)
            {
                leftSouthEast = r.Next(Map.EndX / 2, Map.EndX-15);
                                // Bring rescue label to front after each move
                topSouthEast = r.Next(Map.EndY / 2, Map.EndY-20);
                while (searchs.Where(p => p.Container.Equals(UnitType.SouthEast)).Select(q => Enumerable.Range(leftSouthEast-30, 30).Contains(q.Left) && Enumerable.Range(topSouthEast-40, 40).Contains(q.Top)).ToList().Exists(x => x.Equals(true)))
                {
                    leftSouthEast = r.Next(Map.EndX / 2, Map.EndX-15);
                    topSouthEast = r.Next(Map.EndY / 2, Map.EndY-20);
                }
                                // Convert pixel coordinates to map projection
                Rescue resc = new Rescue();
                resc.Left = leftSouthEast;
                resc.Top = topSouthEast;
                System.Drawing.Point MyPoint = new System.Drawing.Point(resc.Left, resc.Top);
                                // Update rescue coordinates in database
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                resc.LeftProjection = MyCoordinate.X;
                resc.TopProjection = MyCoordinate.Y;
                resc.Container = Unit.GetUnit(resc.Left, resc.Top);
                resc.SteamIDs = new List<string>();
                resc.ListPoint = new Dictionary<DomainObject.Point, Dictionary<Search, int>>();
                rescues.Add(resc);
            }
        }

        void ShowRescueTeam()
        {
            List<TbRescue> tbRescues = new List<TbRescue>();
            int count = 0;
            foreach (var r in rescues)
                            // Bring rescue label to front after movement
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
                            // Execute rescue operation for the point
            }
            db.TbRescues.InsertAllOnSubmit(tbRescues);
            db.SubmitChanges();
                            // Update state after rescue
            geoMap.SendToBack();
            /////
            List<TbRescueFirst> tbRescueFirsts = new List<TbRescueFirst>();
            foreach (var r in tbRescues)
            {
                TbRescueFirst rescue = new TbRescueFirst();
                rescue.Rescue_ID = r.Rescue_ID;
                            // Update UI to show rescue completion
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
        
        double GetDistance(int x1, int x2, int y1, int y2)
        {
            //int temp = ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
            //return (int)Math.Sqrt(temp);
            System.Drawing.Point pointStart = new System.Drawing.Point(x1, y1);
            System.Drawing.Point pointEnd = new System.Drawing.Point(x2, y2);
                            // Update total distance and time metrics
            Coordinate MyCoordinateStart = geoMap.PixelToProj(pointStart);
            Coordinate MyCoordinateEnd = geoMap.PixelToProj(pointEnd);
            return MyCoordinateStart.Distance(MyCoordinateEnd);
        }

                            // Show chart for rescue tasks
        void ShowTime()
        {
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
            timer1.Start();
        }
                            // Update database for rescue and points after completion

        // Timer tick event: updates UI with elapsed time, statistics, and stops timer when rescue is done
        private void timer1_Tick(object sender, EventArgs e)
        {
            // If there are still points to rescue, update the timer display
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
                // All rescues done, stop timer and show averages
                timer1.Stop();
                double averageTimeOfRs = (double) totalTimeOfDistanceRs / points.Count();
                if (txtAverageTimeOfRescue.InvokeRequired)
                    txtAverageTimeOfRescue.BeginInvoke((MethodInvoker)delegate { txtAverageTimeOfRescue.Text = averageTimeOfRs.ToString(); });
                else
                {
                    txtAverageTimeOfRescue.Text = averageTimeOfRs.ToString();
                }
                // Show average efficiency of busy rescue teams
                double averageEfficiencyBusyRescue = (double) efficiencyBusyRescue / numBusyRescue;
                if (txtAvarageBusyResc.InvokeRequired)
                    txtAvarageBusyResc.BeginInvoke((MethodInvoker)delegate { txtAvarageBusyResc.Text = (averageEfficiencyBusyRescue).ToString(); });
                else
                        // Mark rescue as not doing after completion
                {
                    txtAvarageBusyResc.Text = (averageEfficiencyBusyRescue).ToString();
                }
                    // --- Begin: Point Selection Methods for Search ---
            }
            // Update distance and rescue statistics in UI
                        // Select the next point for a search based on its container type
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
            if (txtIdelRescue.InvokeRequired)
                txtIdelRescue.BeginInvoke((MethodInvoker)delegate { txtIdelRescue.Text = idelRescue.ToString(); });
                        // Return null if no valid container
            else
            {
                txtIdelRescue.Text = idelRescue.ToString();
            }
            if (txtBusyRescue.InvokeRequired)
                        // Select the next point in NorthWest region for a search
                txtBusyRescue.BeginInvoke((MethodInvoker)delegate { txtBusyRescue.Text = busyRescue.ToString(); });
            else
            {
                txtBusyRescue.Text = busyRescue.ToString();
            }
        }

        // Binds the search team IDs to the dropdown for information display
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

        // Binds the search state types to the dropdown for filtering/searching
        void BindDropDownSearchState()
        {
            List<Search.SearchStateType> listSearchState = new List<Search.SearchStateType>();
            listSearchState.Add(Search.SearchStateType.Busy);
            listSearchState.Add(Search.SearchStateType.Ready);
            comboxSearchState.DataSource = listSearchState;
        }

        // Binds the rescue level types to the dropdown for filtering/searching
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
        // Assigns points in NorthWest to searchers and creates threads for each search
        public List<Thread> AssignePointToSearchInNorthWest()
        {
            List<DomainObject.Point> orderdPointsInNorthWest = points.Where(p => p.Container == UnitType.NorthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInNorthWest = searchs.Where(p => p.Container == UnitType.NorthWest).OrderBy(p=>p.IntID).ToList();
            List<Thread> Threads = new List<Thread>();
            foreach (var search in searchInNorthWest)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                // Calculate distance from each point to the searcher
                foreach (var item in orderdPointsInNorthWest)
                {
                    item.Distance = GetDistance(item.Left, search.Left, item.Top, search.Top);
                }
                // Assign the closest point with the most victims
                if (orderdPointsInNorthWest.Count() != 0)
                {
                    List<DomainObject.Point> searchOrderdPointsInNorthWest = orderdPointsInNorthWest.OrderByDescending(p => p.NumVictim).OrderBy(p => p.Distance).ToList();
                    DomainObject.Point Point = searchOrderdPointsInNorthWest.First();
                    Search selectedSearch = search;
                    Thread northWestThread = new Thread(() => DoSearchForPoint(selectedSearch, Point));
                    Threads.Add(northWestThread);
                    requestedCountThreadInNorthWest++;
                    orderdPointsInNorthWest.Remove(Point);
                }
            }
            isFinish = true;
            return Threads;
        }

        // Assigns points in NorthEast to searchers and creates threads for each search
        public List<Thread> AssignePointToSearchInNorthEast()
        {
            List<DomainObject.Point> orderdPointsInNorthEast = points.Where(p => p.Container == UnitType.NorthEast).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInNorthEast = searchs.Where(p => p.Container == UnitType.NorthEast).OrderBy(p => p.IntID).ToList();
            int indexOfSelectedPoint = 0;
            List<Thread> Threads = new List<Thread>();
            foreach (var search in searchInNorthEast)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                if (indexOfSelectedPoint < orderdPointsInNorthEast.Count())
                {
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

        // Assigns points in SouthWest to searchers and creates threads for each search
        public List<Thread> AssignePointToSearchInSouthWest()
        {
            List<DomainObject.Point> orderdPointsInSouthWest = points.Where(p => p.Container == UnitType.SouthWest).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInSouthWest = searchs.Where(p => p.Container == UnitType.SouthWest).OrderBy(p => p.IntID).ToList();
            int indexOfSelectedPoint = 0;
            List<Thread> Threads = new List<Thread>();
            foreach (var search in searchInSouthWest)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                if (indexOfSelectedPoint < orderdPointsInSouthWest.Count())
                {
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

        // Assigns points in SouthEast to searchers and creates threads for each search
        public List<Thread> AssignePointToSearchInSouthEast()
        {
            List<DomainObject.Point> orderdPointsInSouthEast = points.Where(p => p.Container == UnitType.SouthEast).OrderByDescending(p => p.NumVictim).ToList();
            List<Search> searchInSouthEast = searchs.Where(p => p.Container == UnitType.SouthEast).OrderBy(p => p.IntID).ToList();
            int indexOfSelectedPoint = 0;
            List<Thread> Threads = new List<Thread>();
            foreach (var search in searchInSouthEast)
            {
                search.State = Search.SearchStateType.Busy;
                search.ListPoint = new List<DomainObject.Point>();
                if (indexOfSelectedPoint < orderdPointsInSouthEast.Count())
                {
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

                // Move the searcher label step by step toward the point, simulating animation
                for (int i = 0; i < total; i++)
                {
                    // Sleep to control animation speed based on search speed
                    System.Threading.Thread.Sleep(int.Parse(nudSearch.Value.ToString()) * 30);
                    totalTimeSleep += (int.Parse(nudSearch.Value.ToString()) * 30);

                    // Determine direction and update coordinates accordingly
                    if (lblSearch.Location.X < lblPoint.Location.X && lblSearch.Location.Y < lblPoint.Location.Y)
                    {
                        // Move southeast
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
                    // Move southwest
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
                    // Move northwest
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
                    // Move northeast
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
                    // Move straight down
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
                    // Move straight up
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
                    // Move straight left
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
                    // Move straight right
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

                    // Always bring the searcher label to the front after moving
                    if (lblSearch.InvokeRequired)
                        lblSearch.BeginInvoke((MethodInvoker)delegate { lblSearch.BringToFront(); });
                    // Handle the case when the main condition is not met
                    else
                        lblSearch.BringToFront();

                    // Update the projected coordinates for the searcher in the database
                    System.Drawing.Point MyPoint = new System.Drawing.Point(newCordinationX, newCordinationY);
                    DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
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

                // Calculate and accumulate the total distance covered by the searcher in this step
                totalDistanceSearch += GetDistance(pointTop, searchTop, pointLeft, searchLeft);

                // --- (Commented out) Region for drawing performance lines and visual feedback ---
                // This region contains code for drawing lines between points for performance visualization,
                // but is currently commented out. It also includes code for cleaning up the drawing and updating UI.

                // Add the current point to the list of points visited by this search team
                selectedSearch.ListPoint.Add(selectedPoint);

                // Perform the search operation on the selected point
                selectedPoint.DoSearch();

                // Mark the search as completed for this point
                selectedPoint.StartSearchDoing = false;
                selectedPoint.EndSearchDoing = true;

                // Update the search status for this point in the database
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    var point = (from TbPoints in dbUpdate.TbPoints
                                 where TbPoints.Point_ID == selectedPoint.ID
                                 select TbPoints).Single();
                    point.Point_StartSearchDoing = selectedPoint.StartSearchDoing;
                    point.Point_EndSearchDoing = selectedPoint.EndSearchDoing;
                    dbUpdate.SubmitChanges();
                }

                // Show new information for the searched point and update the database
                int countNewPoint = ShowNewInformation(selectedPoint);

                // Update the point's image to indicate it has been searched
                lblPoint.Image = Properties.Resources.Point_Blue;

                // Update the images of all new child points generated from this search
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

                // Update and display search and rescue task charts based on elapsed time
                int time = (totalTimeSleep / 1000) + 2;
                ShowChartTasksOfSearch(time);
                numPointDoRescue = numPointDoRescue + countNewPoint;
                ShowChartTasksOfRescue(time);

                // Retrieve the selected search team ID for charting (thread-safe)
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

                // Run the decision algorithm to select the best rescue team for the new points
                DecisionAlgorithmForSelectWinRescue(selectedSearch, selectedPoint, countNewPoint, searchIdForChart);

                // Set the search team's state to Ready and update the database
                selectedSearch.State = Search.SearchStateType.Ready;
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    var search = (from TbSearchs in dbUpdate.TbSearches
                                  where TbSearchs.Search_ID == selectedSearch.ID
                                  select TbSearchs).Single();
                    search.Search_State = (int)Search.SearchStateType.Ready;
                    dbUpdate.SubmitChanges();
                }

                // Select the next point for the search team to visit
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
        // Updates the search task chart with the current number of points left to search, at a given time interval
        public void ShowChartTasksOfSearch(int timeSleep)
        {
            lock (this)
            {
                numPointDoSearch--;
                int time = 0;
                // Ensure thread-safe UI updates
                if (chartTasksOfSearch.InvokeRequired)
                    chartTasksOfSearch.BeginInvoke((MethodInvoker)delegate
                    {
                        // Calculate elapsed time and update the chart with the new data point
                        time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                        seriesTasksOfSearch.Points.AddXY(time, numPointDoSearch);
                        seriesTasksOfSearch.ChartType = SeriesChartType.Line;
                        chartTasksOfSearch.Invalidate();
                    });
                else
                {
                    // Update the chart directly if on the UI thread
                    time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                    seriesTasksOfSearch.Points.AddXY(time, numPointDoSearch);
                    seriesTasksOfSearch.ChartType = SeriesChartType.Line;
                    chartTasksOfSearch.Invalidate();
                }
            }
        }

        // Updates the rescue task chart with the current number of points rescued, at a given time interval
        public void ShowChartTasksOfRescue(int timeSleep)
        {
            lock (this)
            {
                int time = 0;
                // Ensure thread-safe UI updates
                if (chartTasksOfRescue.InvokeRequired)
                    chartTasksOfRescue.BeginInvoke((MethodInvoker)delegate
                    {
                        // Calculate elapsed time and update the chart with the new data point
                        time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                        seriesTasksOfRescue.Points.AddXY(time, numPointDoRescue);
                        seriesTasksOfRescue.ChartType = SeriesChartType.Line;
                        chartTasksOfRescue.Invalidate();
                    });
                else
                {
                    // Update the chart directly if on the UI thread
                    time = int.Parse(Math.Floor(ss.Elapsed.TotalSeconds).ToString()) - timeSleep;
                    seriesTasksOfRescue.Points.AddXY(time, numPointDoRescue);
                    seriesTasksOfRescue.ChartType = SeriesChartType.Line;
                    chartTasksOfRescue.Invalidate();
                }
            }
        }
        //***********************************************//

        // Plots the probability values for each rescue candidate for the selected search team on the probability chart
        public void ShowChartProbabilityForSelectedSearch(int counter, Dictionary<string, double> dicOfProbabilityRs, int searchIdForChart)
        {
            int k = 0;
            // (Commented out) Code for initializing and coloring series is present but not used
            k = 0;
            if (chartProbability.InvokeRequired)
                chartProbability.BeginInvoke((MethodInvoker)delegate
                {
                    // Set the chart title to reflect the current search team and unit
                    chartProbability.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p => p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    // Add probability data points for each rescue candidate
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries[k].Points.AddXY(counter, rescue.Value);
                        listSeries[k].ChartType = SeriesChartType.Spline;
                        k++;
                    }
                });
            else
            {
                // (No direct update if not on UI thread)
            }
        }

        // Plots the probability values for each rescue candidate for the selected search team on the S1 probability chart
        public void ShowChartProbabilityForSelectedSearch_S1(int counter, Dictionary<string, double> dicOfProbabilityRs, int searchIdForChart)
        {
            int k = 0;
            k = 0;
            if (chartProbability_S1.InvokeRequired)
                chartProbability_S1.BeginInvoke((MethodInvoker)delegate
                {
                    // Set the chart title to reflect the current search team and unit
                    chartProbability_S1.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p => p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    // Add probability data points for each rescue candidate
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries_S1[k].Points.AddXY(counter, rescue.Value);
                        listSeries_S1[k].ChartType = SeriesChartType.Spline;
                        k++;
                    }
                });
            else
            {
                // (No direct update if not on UI thread)
            }
        }

        // Plots the probability values for each rescue candidate for the selected search team on the S2 probability chart
        public void ShowChartProbabilityForSelectedSearch_S2(int counter, Dictionary<string, double> dicOfProbabilityRs, int searchIdForChart)
        {
            int k = 0;
            k = 0;
            if (chartProbability_S2.InvokeRequired)
                chartProbability_S2.BeginInvoke((MethodInvoker)delegate
                {
                    // Set the chart title to reflect the current search team and unit
                    chartProbability_S2.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p => p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    // Add probability data points for each rescue candidate
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries_S2[k].Points.AddXY(counter, rescue.Value);
                        listSeries_S2[k].ChartType = SeriesChartType.Spline;
                        k++;
                    }
                });
            else
            {
                // (No direct update if not on UI thread)
            }
        }

        // Plots the probability values for each rescue candidate for the selected search team on the S3 probability chart
        public void ShowChartProbabilityForSelectedSearch_S3(int counter, Dictionary<string, double> dicOfProbabilityRs, int searchIdForChart)
        {
            int k = 0;
            k = 0;
            if (chartProbability_S3.InvokeRequired)
                chartProbability_S3.BeginInvoke((MethodInvoker)delegate
                {
                    // Set the chart title to reflect the current search team and unit
                    chartProbability_S3.Titles[0].Text = "Chart Probabilities In Unit Of " + searchs.Single(p => p.IntID.Equals(searchIdForChart)).Container.ToString() + " From Search Team by ID = " + searchIdForChart.ToString();
                    // Add probability data points for each rescue candidate
                    foreach (var rescue in dicOfProbabilityRs)
                    {
                        listSeries_S3[k].Points.AddXY(counter, rescue.Value);
                        listSeries_S3[k].ChartType = SeriesChartType.Spline;
                        k++;
                    }
                });
            else
            {
                // (No direct update if not on UI thread)
            }
        }

        //***********************************************//
        // Updates the state of a searched point, creates new points if needed, and inserts them into the database
        public int ShowNewInformation(DomainObject.Point selectedPoint)
        {
            List<DomainObject.Point> listOfNewPoints = new List<DomainObject.Point>();
            List<DomainObject.Point> listOfNewPointsForInsert = new List<DomainObject.Point>();

            // Determine the next state of the point based on the number of victims
            switch (selectedPoint.NumVictim.Equals(0))
            {
                case true:
                    // No victims: mark as finished and set rescue level to 0
                    selectedPoint.State = DomainObject.StateType.Finish;
                    selectedPoint.RescueLevel = 0;
                    break;
                case false:
                    // Victims present: mark for rescue
                    selectedPoint.State = DomainObject.StateType.Rescue;
                    // If new points should be created, generate and display them
                    switch (selectedPoint.CreatPoint.Equals(0))
                    {
                        case true:
                            break;
                        case false:
                            // Create new points branching from this one
                            listOfNewPoints = CreateNewPoints(selectedPoint.CreatPoint, selectedPoint);
                            listOfNewPoints = ShowNewPoints(listOfNewPoints);
                            // Add new points to the global list in a thread-safe way
                            lock (this)
                            {
                                foreach (DomainObject.Point newPoint in listOfNewPoints)
                                {
                                    newPoints.Add(newPoint);
                                }
                            }
                            break;
                    }
                    break;
            }

            // Update the state and rescue level of the point in the database
            using (var dbUpdate = new DataClasses1DataContext())
            {
                var point = (from TbPoints in dbUpdate.TbPoints
                             where TbPoints.Point_ID == selectedPoint.ID
                             select TbPoints).Single();
                point.Point_State = (int)selectedPoint.State;
                point.Point_RescueLevel = (int)selectedPoint.RescueLevel;
                dbUpdate.SubmitChanges();
            }

            // Insert any new points into the database
            listOfNewPointsForInsert = listOfNewPoints;
            if (listOfNewPointsForInsert.Count() > 0)
            {
                List<TbPoint> tbNewPoints = new List<TbPoint>();
                int maxIntID = 0;
                lock (this)
                {
                    // Get the current maximum integer ID for points
                    using (var dbSelect = new DataClasses1DataContext())
                    {
                        maxIntID = dbSelect.TbPoints.Max(p => p.Point_IntID);
                    }

                    // Prepare new point records for insertion
                    foreach (var p in listOfNewPointsForInsert)
                    {
                        TbPoint newP = new TbPoint();
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
                        tbNewPoints.Add(newP);
                    }
                }
                // Insert all new points into the database in one batch
                using (var dbInsert = new DataClasses1DataContext())
                {
                    dbInsert.TbPoints.InsertAllOnSubmit(tbNewPoints);
                    dbInsert.SubmitChanges();
                }
            }

            // Return the number of new points created
            return listOfNewPoints.Count();
        }

        // Creates new child points around a parent point, with randomized victim counts and projected coordinates
        public List<DomainObject.Point> CreateNewPoints(int numPoints, DomainObject.Point parentPoint)
        {
            List<DomainObject.Point> newPoints = new List<DomainObject.Point>();
            Random r = new Random();
            for (int i = 1; i <= numPoints; i++)
            {
                // Create a new point (default type 2, true)
                DomainObject.Point p = new DomainObject.Point(2, true);
                // Position the new point in one of four directions around the parent
                switch (i)
                {
                    case 1:
                        p.Left = parentPoint.Left - 10;
                        p.Top = parentPoint.Top;
                        break;
                    case 2:
                        p.Left = parentPoint.Left + 10;
                        p.Top = parentPoint.Top;
                        break;
                    case 3:
                        p.Left = parentPoint.Left;
                        p.Top = parentPoint.Top - 10;
                        break;
                    case 4:
                        p.Left = parentPoint.Left;
                        p.Top = parentPoint.Top + 10;
                        break;
                }
                // Convert screen coordinates to projected map coordinates
                System.Drawing.Point MyPoint = new System.Drawing.Point(p.Left, p.Top);
                DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);
                p.LeftProjection = MyCoordinate.X;
                p.TopProjection = MyCoordinate.Y;
                // Inherit container and parent info
                p.Container = parentPoint.Container;
                p.ParentID = parentPoint.ID;
                // Assign random victim count and compute rescue level
                p.NumVictim = r.Next(DomainObject.Point.minNumVictim, DomainObject.Point.maxNumVictim);
                p.RescueLevel = DomainObject.Point.GetRescueLevel(p.NumVictim);
                p.CreatPoint = 0;
                p.State = DomainObject.StateType.Search;
                p.IsAllocatedSTeam = true;
                p.StartSearchDoing = false;
                p.EndSearchDoing = true;
                // Prevent duplicate points at the same location
                if (points.Where(pt => pt.Top == p.Top && pt.Left == p.Left).Count() > 0)
                {
                    throw new Exception("");
                }
                newPoints.Add(p);
            }
            return newPoints;
        }

        // Adds new point shapes to the UI and ensures the map is rendered behind them
        public List<DomainObject.Point> ShowNewPoints(List<DomainObject.Point> newPoints)
        {
            List<DomainObject.Point> nPoints = new List<DomainObject.Point>();
            lock (newPoints)
            {
                // Add each new point's shape to the panel (thread-safe)
                if (panel1.InvokeRequired)
                    panel1.BeginInvoke((MethodInvoker)delegate { foreach (var p in newPoints) { panel1.Controls.Add(p.GetShape()); nPoints.Add(p); } });
                else
                    foreach (var p in newPoints) { panel1.Controls.Add(p.GetShape()); nPoints.Add(p); }
            }

            // Ensure the map control is sent to the back (thread-safe)
            if (geoMap.InvokeRequired)
                geoMap.BeginInvoke((MethodInvoker)delegate { geoMap.SendToBack(); });
            else
                geoMap.SendToBack();

            return nPoints;
        }
        //***********************************************//
        // Runs the decision algorithm to select the best rescue team for a given point, based on probability and distance
        public void DecisionAlgorithmForSelectWinRescue(Search search, DomainObject.Point point, int countNewPoint, int searchIdForChart)
        {
            // Dictionaries to hold probability and distance values for each region
            Dictionary<string, double> dicOfProbabilityRsInNorthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbabilityRsInNorthEast = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbabilityRsInSouthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbabilityRsInSouthEast = new Dictionary<string, double>();
            Dictionary<string, double> dicOfDistanceRsInNorthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfDistanceRsInNorthEast = new Dictionary<string, double>();
            Dictionary<string, double> dicOfDistanceRsInSouthWest = new Dictionary<string, double>();
            Dictionary<string, double> dicOfDistanceRsInSouthEast = new Dictionary<string, double>();
            string winRescueID = "";
            double winProbability = 0;
            bool isWin = false;
            // Select the appropriate region and compute the best rescue team using automata learning
            switch (point.Container)
            {
                case DomainObject.UnitType.NorthWest:
                    dicOfDistanceRsInNorthWest = ComputeDistanceForRescuesInNorthWest(point);
                    dicOfProbabilityRsInNorthWest = ComputeProbabilityForRescuesInNorthWest(point);
                    winRescueID = SelectWinRescueInNorthWestByAutomataLearning(search, point, dicOfProbabilityRsInNorthWest, dicOfDistanceRsInNorthWest, searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point, countNewPoint, isWin);
                    break;
                case DomainObject.UnitType.NorthEast:
                    dicOfDistanceRsInNorthEast = ComputeDistanceForRescuesInNorthEast(point);
                    dicOfProbabilityRsInNorthEast = ComputeProbabilityForRescuesInNorthEast(point);
                    winRescueID = SelectWinRescueInNorthEastByAutomataLearning(search, point, dicOfProbabilityRsInNorthEast, dicOfDistanceRsInNorthEast, searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point, countNewPoint, isWin);
                    break;
                case DomainObject.UnitType.SouthWest:
                    dicOfDistanceRsInSouthWest = ComputeDistanceForRescuesInSouthWest(point);
                    dicOfProbabilityRsInSouthWest = ComputeProbabilityForRescuesInSouthWest(point);
                    winRescueID = SelectWinRescueInSouthWestByAutomataLearning(search, point, dicOfProbabilityRsInSouthWest, dicOfDistanceRsInSouthWest, searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point, countNewPoint, isWin);
                    break;
                case DomainObject.UnitType.SouthEast:
                    dicOfDistanceRsInSouthEast = ComputeDistanceForRescuesInSouthEast(point);
                    dicOfProbabilityRsInSouthEast = ComputeProbabilityForRescuesInSouthEast(point);
                    winRescueID = SelectWinRescueInSouthEastByAutomataLearning(search, point, dicOfProbabilityRsInSouthEast, dicOfDistanceRsInSouthEast, searchIdForChart, out winProbability, out isWin);
                    AssignePointToWinRescue(search, winRescueID, winProbability, point, countNewPoint, isWin);
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

        // Computes the distance from a given point to each rescue team in the NorthWest region (using TbPoint)
        public Dictionary<string, double> ComputeDistanceForRescuesInNorthWest(TbPoint point)
        {
            List<Rescue> rescuesInNorthWest = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            rescuesInNorthWest = rescues.Where(p => p.Container == UnitType.NorthWest).ToList();
            foreach (var rescue in rescuesInNorthWest)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            return dicOfDistance;
        }
        //
        // Computes the probability for each rescue team in the NorthEast region to be selected for a given point (DomainObject.Point)
        public Dictionary<string, double> ComputeProbabilityForRescuesInNorthEast(DomainObject.Point point)
        {
            List<Rescue> rescuesInNorthEast = new List<Rescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            rescuesInNorthEast = rescues.Where(p => p.Container == UnitType.NorthEast).ToList();
            foreach (var rescue in rescuesInNorthEast)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.ID, probability);
                sumAllPropability += probability;
            }
            // Normalize probabilities so they sum to 1
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            return dicOfProbability;
        }

        // Computes the probability for each rescue team in the NorthEast region to be selected for a given point (TbPoint, DB version)
        public Dictionary<string, double> ComputeProbabilityForRescuesInNorthEast(TbPoint point)
        {
            List<TbRescue> rescuesInNorthEast = new List<TbRescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            using (var dbSelect = new DataClasses1DataContext())
            {
                rescuesInNorthEast = (from TbRescues in dbSelect.TbRescues
                                      where TbRescues.Rescue_Unit.ToLower().Equals(UnitType.NorthEast.ToString().ToLower())
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
            // Normalize probabilities so they sum to 1
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            return dicOfProbability;
        }

        // Computes the distance from a given point to each rescue team in the NorthEast region (DomainObject.Point)
        public Dictionary<string, double> ComputeDistanceForRescuesInNorthEast(DomainObject.Point point)
        {
            List<Rescue> rescuesInNorthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            rescuesInNorthEast = rescues.Where(p => p.Container == UnitType.NorthEast).ToList();
            foreach (var rescue in rescuesInNorthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            return dicOfDistance;
        }

        // Computes the distance from a given point to each rescue team in the NorthEast region (TbPoint)
        public Dictionary<string, double> ComputeDistanceForRescuesInNorthEast(TbPoint point)
        {
            List<Rescue> rescuesInNorthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            rescuesInNorthEast = rescues.Where(p => p.Container == UnitType.NorthEast).ToList();
            foreach (var rescue in rescuesInNorthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            return dicOfDistance;
        }
        //
        // Computes the probability for each rescue team in the SouthWest region to be selected for a given point (DomainObject.Point)
        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthWest(DomainObject.Point point)
        {
            List<Rescue> rescuesInSouthWest = new List<Rescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            rescuesInSouthWest = rescues.Where(p => p.Container == UnitType.SouthWest).ToList();
            foreach (var rescue in rescuesInSouthWest)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.ID, probability);
                sumAllPropability += probability;
            }
            // Normalize probabilities so they sum to 1
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            return dicOfProbability;
        }

        // Computes the probability for each rescue team in the SouthWest region to be selected for a given point (TbPoint, DB version)
        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthWest(TbPoint point)
        {
            List<TbRescue> rescuesInSouthWest = new List<TbRescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            using (var dbSelect = new DataClasses1DataContext())
            {
                rescuesInSouthWest = (from TbRescues in dbSelect.TbRescues
                                      where TbRescues.Rescue_Unit.ToLower().Equals(UnitType.SouthWest.ToString().ToLower())
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
            // Normalize probabilities so they sum to 1
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            return dicOfProbability;
        }

        // Computes the distance from a given point to each rescue team in the SouthWest region (DomainObject.Point)
        public Dictionary<string, double> ComputeDistanceForRescuesInSouthWest(DomainObject.Point point)
        {
            List<Rescue> rescuesInSouthWest = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            rescuesInSouthWest = rescues.Where(p => p.Container == UnitType.SouthWest).ToList();
            foreach (var rescue in rescuesInSouthWest)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            return dicOfDistance;
        }

        // Computes the distance from a given point to each rescue team in the SouthWest region (TbPoint)
        public Dictionary<string, double> ComputeDistanceForRescuesInSouthWest(TbPoint point)
        {
            List<Rescue> rescuesInSouthWest = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            rescuesInSouthWest = rescues.Where(p => p.Container == UnitType.SouthWest).ToList();
            foreach (var rescue in rescuesInSouthWest)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            return dicOfDistance;
        }
        //
        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthEast(DomainObject.Point point)
        // Computes the probability for each rescue team in the SouthEast region to be selected for a given point (DomainObject.Point)
        {
            List<Rescue> rescuesInSouthEast = new List<Rescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            rescuesInSouthEast = rescues.Where(p => p.Container == UnitType.SouthEast).ToList();
            foreach (var rescue in rescuesInSouthEast)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.ID, probability);
                sumAllPropability += probability;
            }
            // Normalize probabilities so they sum to 1
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            return dicOfProbability;
        }

        // Computes the probability for each rescue team in the SouthEast region to be selected for a given point (TbPoint, DB version)
        public Dictionary<string, double> ComputeProbabilityForRescuesInSouthEast(TbPoint point)
        {
            List<TbRescue> rescuesInSouthEast = new List<TbRescue>();
            double reward = ComputeRewardForPoint(point);
            double cost = 0, probability = 0, sumAllPropability = 0;
            List<double> listOfCost = new List<double>();
            Dictionary<string, double> dic = new Dictionary<string, double>();
            Dictionary<string, double> dicOfProbability = new Dictionary<string, double>();
            using (var dbSelect = new DataClasses1DataContext())
            {
                rescuesInSouthEast = (from TbRescues in dbSelect.TbRescues
                                      where TbRescues.Rescue_Unit.ToLower().Equals(UnitType.SouthEast.ToString().ToLower())
                                      select TbRescues).ToList();
            }
            foreach (var rescue in rescuesInSouthEast)
            {
                cost = ComputeCostRescueForPoint(point, rescue);
                listOfCost.Add(cost);
                probability = (reward - cost) / reward;
                dic.Add(rescue.Rescue_ID, probability);
                sumAllPropability += probability;
            }
            // Normalize probabilities so they sum to 1
            foreach (var prob in dic)
            {
                dicOfProbability[prob.Key] = (prob.Value / sumAllPropability);
            }
            return dicOfProbability;
        }

        // Computes the distance from a given point to each rescue team in the SouthEast region (DomainObject.Point)
        public Dictionary<string, double> ComputeDistanceForRescuesInSouthEast(DomainObject.Point point)
        {
            List<Rescue> rescuesInSouthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            rescuesInSouthEast = rescues.Where(p => p.Container == UnitType.SouthEast).ToList();
            foreach (var rescue in rescuesInSouthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            return dicOfDistance;
        }

        // Computes the distance from a given point to each rescue team in the SouthEast region (TbPoint)
        public Dictionary<string, double> ComputeDistanceForRescuesInSouthEast(TbPoint point)
        {
            List<Rescue> rescuesInSouthEast = new List<Rescue>();
            double distance = 0;
            Dictionary<string, double> dicOfDistance = new Dictionary<string, double>();
            rescuesInSouthEast = rescues.Where(p => p.Container == UnitType.SouthEast).ToList();
            foreach (var rescue in rescuesInSouthEast)
            {
                distance = ComputeDistanceRescueForPoint(point, rescue);
                dicOfDistance.Add(rescue.ID, distance);
            }
            return dicOfDistance;
        }

        //****************//
        // Computes the maximum probability among all rescue teams except the winning one (NorthWest region)
        public double ComputeMaxProbabilityForAnotherRescuesInNorthWest(TbPoint point, string winRescueID)
        {
            Dictionary<string, double> dicOfProbability = ComputeProbabilityForRescuesInNorthWest(point);
            dicOfProbability.Remove(winRescueID);
            double maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }

        // Computes the maximum probability among all rescue teams except the winning one (NorthEast region)
        public double ComputeMaxProbabilityForAnotherRescuesInNorthEast(TbPoint point, string winRescueID)
        {
            Dictionary<string, double> dicOfProbability = ComputeProbabilityForRescuesInNorthEast(point);
            dicOfProbability.Remove(winRescueID);
            double maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }

        // Computes the maximum probability among all rescue teams except the winning one (SouthWest region)
        public double ComputeMaxProbabilityForAnotherRescuesInSouthWest(TbPoint point, string winRescueID)
        {
            Dictionary<string, double> dicOfProbability = ComputeProbabilityForRescuesInSouthWest(point);
            dicOfProbability.Remove(winRescueID);
            double maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }

        // Computes the maximum probability among all rescue teams except the winning one (SouthEast region)
        public double ComputeMaxProbabilityForAnotherRescuesInSouthEast(TbPoint point, string winRescueID)
        {
            Dictionary<string, double> dicOfProbability = ComputeProbabilityForRescuesInSouthEast(point);
            dicOfProbability.Remove(winRescueID);
            double maxProbability = dicOfProbability.Where(p => p.Value.Equals(dicOfProbability.Max(q => q.Value))).First().Value;
            return maxProbability;
        }
        //***************//
        // Computes the reward value for a point (used in probability/cost calculations)
        public double ComputeRewardForPoint(DomainObject.Point point)
        {
            // Reward is a function of rescue level and time to be done
            double reward = (int)point.RescueLevel * 10 + point.TimeToBeDone * 70;
            return reward;
        }

        // Computes the reward value for a point (database version)
        public double ComputeRewardForPoint(TbPoint point)
        {
            // Reward is a function of rescue level and time to be done
            double reward = (int)point.Point_RescueLevel * 10 + point.Point_TimeToBeDone * 43;
            return reward;
        }

        // Computes the cost for a rescue team to reach a point (DomainObject.Point)
        public double ComputeCostRescueForPoint(DomainObject.Point point, Rescue rescue)
        {
            // Cost is based on distance divided by speed (5 m/s)
            double cost = GetDistance(point.Left, rescue.Left, point.Top, rescue.Top) / 5;
            return cost;
        }

        // Computes the cost for a rescue team to reach a point (TbPoint, DB version)
        public double ComputeCostRescueForPoint(TbPoint point, TbRescue rescue)
        {
            // Cost is based on distance divided by speed (5 m/s)
            double cost = GetDistance(point.Point_LeftCoordinate, rescue.Rescue_LeftCoordinate, point.Point_TopCoordinate, rescue.Rescue_TopCoordinate) / 5;
            return cost;
        }

        // Computes the distance between a point and a rescue team (DomainObject.Point)
        public double ComputeDistanceRescueForPoint(DomainObject.Point point, Rescue rescue)
        {
            double distance = GetDistance(point.Left, rescue.Left, point.Top, rescue.Top);
            return distance;
        }

        // Computes the distance between a point and a rescue team (TbPoint, DB version)
        public double ComputeDistanceRescueForPoint(TbPoint point, Rescue rescue)
        {
            double distance = GetDistance(point.Point_LeftCoordinate, rescue.Left, point.Point_TopCoordinate, rescue.Top);
            return distance;
        }
        //**********************************************//
        // Determines if the given rescue team is the closest to the point (NorthWest region)
        public bool IsBestRescueForAnotherSearchInNorthWest(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = ComputeDistanceForRescuesInNorthWest(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        // Determines if the given rescue team is the closest to the point (NorthEast region)
        // Determines if the given rescue team is the closest to the point (NorthEast region)
        public bool IsBestRescueForAnotherSearchInNorthEast(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = ComputeDistanceForRescuesInNorthEast(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        // Determines if the given rescue team is the closest to the point (SouthWest region)
        public bool IsBestRescueForAnotherSearchInSouthWest(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = ComputeDistanceForRescuesInSouthWest(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        // Determines if the given rescue team is the closest to the point (SouthEast region)
        public bool IsBestRescueForAnotherSearchInSouthEast(TbPoint point, string rescueID)
        {
            Dictionary<string, double> dicOfDistanceRs = ComputeDistanceForRescuesInSouthEast(point);
            if (dicOfDistanceRs.Where(p => p.Value.Equals(dicOfDistanceRs.Min(q => q.Value))).Select(p => p.Key).ToList().Contains(rescueID))
                return true;
            else
                return false;
        }

        //**********************************************// 
        //**********************************************// 
        // Selects the winning rescue team for a point in the NorthWest region using automata learning
        public string SelectWinRescueInNorthWestByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs, int searchIdForChart, out double winProbability, out bool isWin)
        {
            int i = 0;
            string winRescueID = "";
            double winRescueProbability = 0.0;
            isWin = false;
            winProbability = 0;
            int counter = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInNorthWestCopy = 0;
            double start = ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInNorthWest = new List<string>();
            busyNorthWestRs = 0;
            // Main automata learning loop (up to 25 iterations)
            while (counter <= 25)
            {
                // Update probability charts for the current search team
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
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
                // If not the first iteration, clear previous assignments and update DB
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInSouthEast.Remove(search.ID);
                            dicIsGivedBestSelectInSouthEast.Add(search.ID, true);
                        }
                    }
                    // Reset rescue assignment in the database
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    // Thread synchronization for NorthWest region
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
                // Select the winning rescue based on current probabilities
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    // Assign the winning rescue and update the database
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
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
                // Thread synchronization for NorthWest region
                lock (syncLockStartInNorthWest)
                {
                    currentCountThreadInNorthWestStart++;
                    if (currentCountThreadInNorthWestStart < requestedCountThreadInNorthWest)
                    {
                        Monitor.Wait(syncLockStartInNorthWest);
                    }
                    Monitor.PulseAll(syncLockStartInNorthWest);
                }
                // Reset thread counter and store timing for first iteration
                currentCountThreadInNorthWestStart = 0;
                if (counter == 0)
                {
                    start = ss.Elapsed.TotalSeconds;
                    requestedCountThreadInNorthWestCopy = requestedCountThreadInNorthWest;
                }
                // Automata learning: adjust probabilities based on outcome
                int b = 0;
                double penalty = 0;
                double x;
                List<string> competitorSID = rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs;
                // Query DB for other search teams assigned to this rescue
                Dictionary<string, double?> dicSID = new Dictionary<string, double?>();
                using (var dbSelect = new DataClasses1DataContext())
                {
                    dicSID = (from TbTaskLists in dbSelect.TbTaskLists
                              where TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                              select TbTaskLists).Select(p => new { p.Search_ID, p.Probability }).ToDictionary(p => p.Search_ID, p => p.Probability);
                }
                // If only one search team is assigned, check if this rescue is the closest
                if (dicSID.Count <= 1)
                {
                    // If this rescue is the closest, reward it; otherwise, apply penalty or random chance
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
                        if (y <= 0.15)
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1;
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
                        // Thread synchronization for this section
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
                        // Determine which search team has the highest probability
                        winSearchID = dicSID.First(p => p.Value.Equals(dicSID.Max(q => q.Value))).Key;
                        if (winSearchID.Equals(search.ID))
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                            isGivedBestSelect = true;
                        }
                    }
                    else
                    {
                        // Multiple search teams: check if this rescue is the best for another search
                        bool IsBestRescueForAnotherSearch = false;
                        requestedThisSectionCountThreadInNorthWest++;
                        TbPoint selectedPoint;
                        double minProbability;
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
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
                                                     select TbPoints).Single();
                                }
                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInNorthWest(selectedPoint, winRescueID));
                                if (!sID.Equals(search.ID))
                                {
                                    if (IsBestRescueForAnotherSearchInNorthWest(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;
                        }
                        // Thread synchronization for this section
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
                        // If another search is best for this rescue, apply penalty and update probabilities
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                // Randomly decide whether to penalize or reward based on probability
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.5)
                                {
                                    // Apply penalty to probability if random threshold met
                                    penalty = 0.1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                    // Move to next rescue if not last
                                    if (i < dicOfDistanceRs.Count() - 1)
                                        i = i + 1;
                                }
                                else
                                {
                                    // Reward: increase probability for win
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                            }
                            else
                            {
                                // Always apply penalty if not best rescue
                                penalty = 0.1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false;
                                isGivedBestSelect = true;
                            }
                            // Alternative logic for probability adjustment (commented out)
                        }
                        else
                        {
                            // If this rescue is the closest, reward; otherwise, random chance for reward or penalty
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
                                if (y <= 0.15)
                                {
                                    // Small chance to reward even if not closest
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                    isGivedBestSelect = true;
                                }
                                else
                                {
                                    // Otherwise, apply penalty
                                    penalty = 0.1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                }
                            }
                        }
                        // Additional logic for other selection cases (commented out)
                    }
                }

                // Update the main probability dictionary for the next iteration
                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++;
                // Thread synchronization: ensure all threads reach this point before proceeding
                // Thread synchronization: ensure all threads reach this point before proceeding
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
                // Time-out check: if elapsed time exceeds threshold, break loop
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    // If time limit exceeded, reduce thread count and exit
                    requestedCountThreadInNorthWest--;
                    isOutByTime = true;
                    break;
                }
                else
                    isOutByTime = false;
            }
            // After automata learning loop, update win rescue list and busy count
            lock (this)
            {
                if (isWin == true)
                {
                    // Add winning rescue to list if not already present
                    if (!listOfWinRescueIDInNorthWest.Any(p => p.Equals(winRescueID)))
                    {
                        listOfWinRescueIDInNorthWest.Add(winRescueID);
                        busyNorthWestRs++;
                    }
                }
            }
            // Final thread synchronization after end of learning
            lock (syncLockAfterEndInNorthWest)
            {
                currentCountThreadInNorthWestAfterEnd++;
                if (currentCountThreadInNorthWestAfterEnd < requestedCountThreadInNorthWestCopy)
                {
                    Monitor.Wait(syncLockAfterEndInNorthWest);
                }
                Monitor.PulseAll(syncLockAfterEndInNorthWest);
                // Restore requested thread count for next round
                requestedCountThreadInNorthWest = requestedCountThreadInNorthWestCopy;
            }
            currentCountThreadInNorthWestAfterEnd = 0;
            // Calculate efficiency metrics for busy rescues in NorthWest
            int countRescuesNorthWest = rescues.Where(p => p.Container.Equals(UnitType.NorthWest)).Count();
            int countSearchsNorthWest = searchs.Where(p => p.Container.Equals(UnitType.NorthWest)).Count();
            int countAllTasksNorthWest = points.Where(p => p.Container.Equals(UnitType.NorthWest) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInNorthWestCopy;

            // Compute efficiency based on number of busy rescues and available tasks
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
            // Cleanup: clear SteamIDs and selection flags for next round
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInNorthWest.Clear();
            // Remove all related task list entries from database for this point
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }
            // Return the ID of the selected winning rescue
            return winRescueID;
        }

        // Select the winning rescue in the NorthEast region using automata learning
        public string SelectWinRescueInNorthEastByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs, int searchIdForChart, out double winProbability, out bool isWin)
        {
            // Initialize variables for tracking the winning rescue, probabilities, and thread state
            string winRescueID = "";
            double winRescueProbability = 0.0; int i = 0;
            isWin = false;
            winProbability = 0;
            int counter = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInNorthEastCopy = 0;
            // Start timing for time-out logic
            double start = ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInNorthEast = new List<string>();
            busyNorthEastRs = 0;
            // Main automata learning loop (up to 25 iterations)
            while (counter <= 25)
            {
                // Show probability chart for selected search if needed
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
                // On subsequent iterations, clear previous SteamIDs and update selection flags
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInNorthEast.Remove(search.ID);
                            dicIsGivedBestSelectInNorthEast.Add(search.ID, true);
                        }
                    }
                    // Update database: reset rescue assignment and probability for this search/point
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    // Thread synchronization before starting next round
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

                // Prepare new probability dictionary for this round
                Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
                // Select the rescue with the highest probability and update database
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
                        // Update database with new rescue assignment and probability
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
                // Thread synchronization at start of round
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
                // On first iteration, store timing and thread count
                if (counter == 0)
                {
                    start = ss.Elapsed.TotalSeconds;
                    requestedCountThreadInNorthEastCopy = requestedCountThreadInNorthEast;
                }
                // Automata learning: adjust probabilities based on outcome
                int b = 0;
                double penalty = 0;
                double x;
                List<string> competitorSID = rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs;
                // Query DB for other search teams assigned to this rescue
                Dictionary<string, double?> dicSID = new Dictionary<string, double?>();
                using (var dbSelect = new DataClasses1DataContext())
                {
                    dicSID = (from TbTaskLists in dbSelect.TbTaskLists
                              where TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                              select TbTaskLists).Select(p => new { p.Search_ID, p.Probability }).ToDictionary(p => p.Search_ID, p => p.Probability);
                }

                // If only one search team is assigned, check if this rescue is the closest
                if (dicSID.Count <= 1)
                {
                    // If this rescue is the closest, reward it; otherwise, apply penalty or random chance
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
                        if (y <= 0.15)
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1;
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
                        // Thread synchronization for this section
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
                        // Determine which search team has the highest probability
                        winSearchID = dicSID.First(p => p.Value.Equals(dicSID.Max(q => q.Value))).Key;
                        // If this search is the winner, reward; otherwise, apply penalty
                        if (winSearchID.Equals(search.ID))
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                            isGivedBestSelect = true;
                        }
                    }
                    else
                    {
                        // Multiple search teams: check if this rescue is the best for another search
                        bool IsBestRescueForAnotherSearch = false;
                        requestedThisSectionCountThreadInNorthEast++;
                        TbPoint selectedPoint;
                        double minProbability;
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        // Determine the second best rescue by probability
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
                        // For each search team, compute max probability for other rescues
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
                                                     select TbPoints).Single();
                                }
                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInNorthEast(selectedPoint, winRescueID));
                                if (!sID.Equals(search.ID))
                                {
                                    if (IsBestRescueForAnotherSearchInNorthEast(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;
                        }
                        // Thread synchronization for this section
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
                        // If another search is best for this rescue, apply penalty and update probabilities
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.5)
                                {
                                    penalty = 0.1;
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
                                penalty = 0.1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false;
                                isGivedBestSelect = true;
                            }
                            // Alternative logic for probability adjustment (commented out)
                            //isGivedBestSelect = true;
                        }
                        else
                        {
                            // If this rescue is the closest, reward it; otherwise, random chance for reward or penalty
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                b = 1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                isWin = true;
                            }
                            else
                            {
                                // Randomly decide whether to reward or penalize
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.15)
                                {
                                    b = 1;
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true;
                                }
                                else
                                {
                                    penalty = 0.1; 
                                    x = 0.4;
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false;
                                    isGivedBestSelect = true;
                                }

                            }
                        }
                        //}
                        //else if (!dicIsGivedBestSelectInNorthEast.First(p => p.Key.Equals(search.ID)).Value)
                        //{
                        //    b = -1;
                        //    x = 0.4;
                        //    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        //    isWin = false;
                        //}
                    }
                }
                // Update probability dictionary for next iteration
                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++;
                // Thread synchronization: ensure all threads reach this point before proceeding
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
                // Time-out check: if elapsed time exceeds threshold, break loop
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    requestedCountThreadInNorthEast--;
                    isOutByTime = true;
                    break;
                }
                else
                    isOutByTime = false;
            }
            // After automata learning loop, update win rescue list and busy count
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
            // Final thread synchronization after end of learning
            lock (syncLockAfterEndInNorthEast)
            {
                currentCountThreadInNorthEastAfterEnd++;
                if (currentCountThreadInNorthEastAfterEnd < requestedCountThreadInNorthEastCopy)
                {
                    Monitor.Wait(syncLockAfterEndInNorthEast);
                }
                Monitor.PulseAll(syncLockAfterEndInNorthEast);
                // Restore requested thread count for next round
                requestedCountThreadInNorthEast = requestedCountThreadInNorthEastCopy;
            }
            currentCountThreadInNorthEastAfterEnd = 0;
            // Calculate efficiency metrics for busy rescues in NorthEast
            int countRescuesNorthEast = rescues.Where(p => p.Container.Equals(UnitType.NorthEast)).Count();
            int countSearchsNorthEast = searchs.Where(p => p.Container.Equals(UnitType.NorthEast)).Count();
            int countAllTasksNorthEast = points.Where(p => p.Container.Equals(UnitType.NorthEast) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInNorthEastCopy;

            // Compute efficiency based on number of busy rescues and available tasks
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
            // Cleanup: clear SteamIDs and selection flags for next round
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInNorthEast.Clear();
            // Remove all related task list entries from database for this point
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }
            //

            return winRescueID;
        }

        // Select the winning rescue in the SouthWest region using automata learning
        public string SelectWinRescueInSouthWestByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs, int searchIdForChart, out double winProbability, out bool isWin)
        {
            // Initialize variables for tracking the winning rescue, probabilities, and thread state
            string winRescueID = "";
            double winRescueProbability = 0.0; int i = 0;
            isWin = false;
            winProbability = 0;
            int counter = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInSouthWestCopy = 0;
            // Start timing for time-out logic
            double start = ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInSouthWest = new List<string>();
            busySouthWestRs = 0;
            // Main automata learning loop (up to 25 iterations)
            while (counter <= 25)
            {
                // Show probability chart for selected search if needed
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
                // On subsequent iterations, clear previous SteamIDs and update selection flags
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInNorthEast.Remove(search.ID);
                            dicIsGivedBestSelectInNorthEast.Add(search.ID, true);
                        }
                    }
                    // Update database: reset rescue assignment and probability for this search/point
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    // Thread synchronization before starting next round
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

                // Prepare new probability dictionary for this round
                Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
                // Select the rescue with the highest probability and update database
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
                        // Update database with new rescue assignment and probability
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

                // Thread synchronization at start of round
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
                // On first iteration, store timing and thread count
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
                    // If only one search team is assigned, check if this rescue is the closest
                    if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                    {
                        b = 1;
                        x = 0.4;
                        newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                        isWin = true;
                    }
                    else
                    {
                        // Randomly decide whether to reward or penalize
                        Random r = new Random();
                        double y = r.NextDouble();
                        if (y <= 0.15)
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1;
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
                        // Thread synchronization for this section
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
                        // Determine which search team has the highest probability
                        winSearchID = dicSID.First(p => p.Value.Equals(dicSID.Max(q => q.Value))).Key;
                        // If this search is the winner, reward; otherwise, apply penalty
                        if (winSearchID.Equals(search.ID))
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                            isWin = false;
                            isGivedBestSelect = true;
                        }
                    }
                    else
                    {
                        // Multiple search teams: check if this rescue is the best for another search
                        bool IsBestRescueForAnotherSearch = false;
                        requestedThisSectionCountThreadInSouthWest++;
                        TbPoint selectedPoint;
                        double minProbability;
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        // Determine the second best rescue by probability
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
                        // For each search team, compute max probability for other rescues
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
                                                     select TbPoints).Single();
                                }
                                // Compute max probability for other rescues for each search team
                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInSouthWest(selectedPoint, winRescueID));
                                if (!sID.Equals(search.ID))
                                {
                                    if (IsBestRescueForAnotherSearchInSouthWest(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;
                        }
                        // Thread synchronization for this section
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
                        // If another search is best for this rescue, apply penalty and update probabilities
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                // Randomly decide whether to penalize or reward
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.5)
                                {
                                    penalty = 0.1;
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
                                penalty = 0.1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false;
                                isGivedBestSelect = true;
                            }
                            // Alternative logic for probability adjustment (commented out)
                        }
                        else
                        {
                            // If this rescue is the closest, reward; otherwise, random chance for reward or penalty
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                b = 1;
                                x = 0.4;
                                newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                isWin = true;
                            }
                            else
                            {
                                // Randomly decide whether to reward or penalize
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
                        // Additional logic for probability adjustment (commented out)
                    }
                }
                // Update probability dictionary for next iteration
                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++;
                // Thread synchronization: ensure all threads reach this point before proceeding
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
                // Time-out check: if elapsed time exceeds threshold, break loop
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    requestedCountThreadInSouthWest--;
                    isOutByTime = true;
                    break;
                }
                else
                    isOutByTime = false;
            }
            // After automata learning loop, update win rescue list and busy count
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
            // Final thread synchronization after end of learning
            lock (syncLockAfterEndInSouthWest)
            {
                currentCountThreadInSouthWestAfterEnd++;
                if (currentCountThreadInSouthWestAfterEnd < requestedCountThreadInSouthWestCopy)
                {
                    Monitor.Wait(syncLockAfterEndInSouthWest);
                }
                Monitor.PulseAll(syncLockAfterEndInSouthWest);
                // Restore requested thread count for next round
                requestedCountThreadInSouthWest = requestedCountThreadInSouthWestCopy;
            }
            currentCountThreadInSouthWestAfterEnd = 0;
            // Calculate efficiency metrics for busy rescues in SouthWest
            int countRescuesSouthWest = rescues.Where(p => p.Container.Equals(UnitType.SouthWest)).Count();
            int countSearchsSouthWest = searchs.Where(p => p.Container.Equals(UnitType.SouthWest)).Count();
            int countAllTasksSouthWest = points.Where(p => p.Container.Equals(UnitType.SouthWest) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInSouthWestCopy;

            // Compute efficiency based on number of busy rescues and available tasks
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
            // Cleanup: clear SteamIDs and selection flags for next round
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInSouthWest.Clear();
            // Remove all related task list entries from database for this point
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }

            return winRescueID;
        }

        // Select the winning rescue in the SouthEast region using automata learning
        public string SelectWinRescueInSouthEastByAutomataLearning(Search search, DomainObject.Point point, Dictionary<string, double> dicOfProbabilityRs, Dictionary<string, double> dicOfDistanceRs, int searchIdForChart, out double winProbability, out bool isWin)
        {
            // Initialize variables for tracking the winning rescue, probabilities, and thread state
            string winRescueID = "";
            double winRescueProbability = 0.0; int i = 0;
            isWin = false;
            int counter = 0;
            winProbability = 0;
            bool isGivedBestSelect = false;
            bool isOutByTime = false;
            int requestedCountThreadInSouthEastCopy = 0;
            // Start timing for time-out logic
            double start = ss.Elapsed.TotalSeconds;
            listOfWinRescueIDInSouthEast = new List<string>();
            busySouthEastRs = 0;
            // Main automata learning loop (up to 25 iterations)
            while (counter <= 25)
            {
                // Show probability chart for selected search if needed
                if (search.IntID == searchIdForChart)
                {
                    ShowChartProbabilityForSelectedSearch(counter, dicOfProbabilityRs, searchIdForChart);
                }
                // On subsequent iterations, clear previous SteamIDs and update selection flags
                if (counter > 0)
                {
                    rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
                    lock (this)
                    {
                        if (isGivedBestSelect == true)
                        {
                            dicIsGivedBestSelectInNorthEast.Remove(search.ID);
                            dicIsGivedBestSelectInNorthEast.Add(search.ID, true);
                        }
                    }
                    // Update database: reset rescue assignment and probability for this search/point
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        var task = (from TbTaskLists in dbUpdate.TbTaskLists
                                    where TbTaskLists.Point_ID == point.ID && TbTaskLists.Search_ID == search.ID
                                    select TbTaskLists).Single();
                        task.Rescue_ID = null;
                        task.Probability = 0;
                        dbUpdate.SubmitChanges();
                    }
                    // Thread synchronization before starting next round
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

                // Prepare new probability dictionary for this round
                Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
                // Select the rescue with the highest probability and update database
                lock (this)
                {
                    search.CompetitorProbabilites = new Dictionary<string, double>();
                    winRescueID = "";
                    winRescueProbability = 0.0;
                    winRescueID = SelectWinRescueByProbability(dicOfProbabilityRs);
                    winRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(winRescueID)).Value;
                    winProbability = winRescueProbability;
                    if (rescues.Find(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())) != null)
                    {
                        rescues.Where(p => p.ID.Trim().ToLower().Equals(winRescueID.Trim().ToLower())).Single().SteamIDs.Add(string.Format("{0}", search.ID));
                        // Update database with new rescue assignment and probability
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
                // Thread synchronization at start of round
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
                // On first iteration, store timing and thread count
                if (counter == 0)
                {
                    start = ss.Elapsed.TotalSeconds;
                    requestedCountThreadInSouthEastCopy = requestedCountThreadInSouthEast;
                }
                // Automata learning: adjust probabilities based on outcome
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
                        if (y <= 0.15)
                        {
                            b = 1;
                            x = 0.4;
                            newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                            isWin = true;
                        }
                        else
                        {
                            penalty = 0.1; 
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
                        // Flag to check if the best rescue is for another search
                        bool IsBestRescueForAnotherSearch = false;
                        // Track the number of threads requested in this section
                        requestedThisSectionCountThreadInSouthEast++;
                        TbPoint selectedPoint;
                        double minProbability;
                        // Dictionary to store computed probabilities for each search ID
                        Dictionary<string, double> newdicSID = new Dictionary<string, double>();
                        // Dictionary to track if a rescue is best for each search
                        Dictionary<string, bool> dicIsBestRescue = new Dictionary<string, bool>();
                        string secondWinRescueID = "";
                        // Determine the second best rescue ID based on probabilities
                        if (dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key.Equals(winRescueID))
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).Skip(1).First().Key;
                        else
                            secondWinRescueID = dicOfProbabilityRs.OrderByDescending(p => p.Value).First().Key;
                        // Get the probability value for the second best rescue
                        double secondWinRescueProbability = dicOfProbabilityRs.First(p => p.Key.Equals(secondWinRescueID)).Value;
                        // Synchronize access to shared resources
                        lock (this)
                        {
                            // Iterate through all search IDs
                            foreach (var sID in dicSID.Keys)
                            {
                                // Query the database for the point associated with the current search and rescue
                                using (var dbSelect = new DataClasses1DataContext())
                                {
                                    selectedPoint = (from TbPoints in dbSelect.TbPoints
                                                     where TbPoints.Point_ID == ((from TbTaskLists in dbSelect.TbTaskLists
                                                                                  where TbTaskLists.Search_ID == sID && TbTaskLists.Rescue_ID == winRescueID && TbTaskLists.IsAssigned == false
                                                                                  select TbTaskLists).Single().Point_ID)
                                                     select TbPoints).Single();
                                }

                                // Compute the maximum probability for another rescue in the SouthEast region
                                newdicSID.Add(sID, ComputeMaxProbabilityForAnotherRescuesInSouthEast(selectedPoint, winRescueID));

                                // Check if the current search ID is not the main search
                                if (!sID.Equals(search.ID))
                                {
                                    // Determine if this rescue is the best for another search
                                    if (IsBestRescueForAnotherSearchInSouthEast(selectedPoint, winRescueID))
                                        IsBestRescueForAnotherSearch = true;
                                }
                            }
                            // Find the minimum probability and corresponding search ID
                            minProbability = newdicSID.First(p => p.Value.Equals(newdicSID.Min(q => q.Value))).Value;
                            winSearchID = newdicSID.First(p => p.Value.Equals(minProbability)).Key;
                        }
                        // Synchronize thread execution in the SouthEast section
                        lock (syncLockMiddleInSouthEast)
                        {
                            currentThisSectionCountThreadInSouthEast++;
                            if (currentThisSectionCountThreadInSouthEast < requestedThisSectionCountThreadInSouthEast)
                            {
                                Monitor.Wait(syncLockMiddleInSouthEast);
                            }
                            Monitor.PulseAll(syncLockMiddleInSouthEast);
                        }
                        // Reset thread counters for this section
                        currentThisSectionCountThreadInSouthEast = 0;
                        requestedThisSectionCountThreadInSouthEast = 0;

                        // If the best rescue is for another search and not the current one
                        if (IsBestRescueForAnotherSearch && !winSearchID.Equals(search.ID))
                        {
                            // Check if the current rescue is the closest by distance
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                // Generate a random value to decide penalty or reward
                                Random r = new Random();
                                double y = r.NextDouble();
                                // If random value is less than or equal to 0.5, apply penalty
                                if (y <= 0.5) //0.9//0.5//0.15
                                {
                                    penalty = 0.1; // Apply penalty factor
                                    x = 0.4; // Update probability adjustment factor
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false; // Mark as not winning
                                    isGivedBestSelect = true; // Mark as best selection given
                                    // Move to next rescue if not at the end
                                    if (i < dicOfDistanceRs.Count() - 1)
                                        i = i + 1;
                                }
                                else
                                {
                                    b = 1; // Apply reward factor
                                    x = 0.4; // Update probability adjustment factor
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true; // Mark as winning
                                }
                            }
                            else
                            {
                                // If not the closest rescue, apply penalty
                                penalty = 0.1; // Apply penalty factor
                                x = 0.4; // Update probability adjustment factor
                                newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                isWin = false; // Mark as not winning
                                isGivedBestSelect = true; // Mark as best selection given
                            }

                        }
                        else
                        {
                            // If not best rescue for another search
                            if (dicOfDistanceRs.OrderBy(p => p.Value).ElementAt(i).Key.Equals(winRescueID))
                            {
                                // If current rescue is closest, apply reward
                                b = 1; // Apply reward factor
                                x = 0.4; // Update probability adjustment factor
                                newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                isWin = true; // Mark as winning
                            }
                            else
                            {
                                // Otherwise, randomly decide penalty or reward
                                Random r = new Random();
                                double y = r.NextDouble();
                                if (y <= 0.15) //0.2
                                {
                                    b = 1; // Apply reward factor
                                    x = 0.4; // Update probability adjustment factor
                                    newDicOfProbabilityRs = ComputeNewProbabilites(winRescueID, winRescueProbability, b, x, dicOfProbabilityRs);
                                    isWin = true; // Mark as winning
                                    isGivedBestSelect = true; // Mark as best selection given
                                }
                                else
                                {
                                    penalty = 0.1; // Apply penalty factor
                                    x = 0.4; // Update probability adjustment factor
                                    newDicOfProbabilityRs = ComputeNewProbabilitesForPenalty(winRescueID, winRescueProbability, penalty, x, dicOfProbabilityRs);
                                    isWin = false; // Mark as not winning
                                }

                            }
                        }

                    }
                }
                // Update the main probability dictionary with new values
                dicOfProbabilityRs = newDicOfProbabilityRs;
                counter++; // Increment operation counter
                // Synchronize thread execution at the end of SouthEast section
                lock (syncLockEndInSouthEast)
                {
                    // Increment thread counter for SouthEast end section
                    currentCountThreadInSouthEastEnd++;
                    // Wait for all threads to reach this point before proceeding
                    if (currentCountThreadInSouthEastEnd < requestedCountThreadInSouthEast)
                    {
                        Monitor.Wait(syncLockEndInSouthEast);
                    }
                    Monitor.PulseAll(syncLockEndInSouthEast);
                }
                // Reset thread counter for SouthEast end section
                currentCountThreadInSouthEastEnd = 0;
                // Check if elapsed time exceeds threshold (14 seconds)
                if (ss.Elapsed.TotalSeconds > start + 14)
                {
                    requestedCountThreadInSouthEast--;
                    isOutByTime = true; // Mark as timed out
                    break;
                }
                else
                    isOutByTime = false;
            }
            ////
            // Synchronize access to shared resources for win rescue
            lock (this)
            {
                if (isWin == true)
                {
                    // Add winning rescue ID to list if not already present
                    if (!listOfWinRescueIDInSouthEast.Any(p => p.Equals(winRescueID)))
                    {
                        listOfWinRescueIDInSouthEast.Add(winRescueID);
                        busySouthEastRs++;
                    }
                }
            }
            // Synchronize thread execution after end in SouthEast section
            lock (syncLockAfterEndInSouthEast)
            {
                currentCountThreadInSouthEastAfterEnd++;
                if (currentCountThreadInSouthEastAfterEnd < requestedCountThreadInSouthEastCopy)
                {
                    Monitor.Wait(syncLockAfterEndInSouthEast);
                }
                Monitor.PulseAll(syncLockAfterEndInSouthEast);
                requestedCountThreadInSouthEast = requestedCountThreadInSouthEastCopy;
            }
            // Reset thread counter for after end section
            currentCountThreadInSouthEastAfterEnd = 0;
            // Calculate counts for SouthEast units
            int countRescuesSouthEast = rescues.Where(p => p.Container.Equals(UnitType.SouthEast)).Count();
            int countSearchsSouthEast = searchs.Where(p => p.Container.Equals(UnitType.SouthEast)).Count();
            int countAllTasksSouthEast = points.Where(p => p.Container.Equals(UnitType.SouthEast) && p.EndRescueDoing.Equals(false)).Count();
            int countCurrentTask = requestedCountThreadInSouthEastCopy;

            // Calculate efficiency of busy rescues based on counts
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
            numBusyRescue++; // Increment number of busy rescues
            // Clear SteamIDs for winning rescue and reset selection dictionary
            rescues.Find(p => p.ID.Equals(winRescueID)).SteamIDs.Clear();
            dicIsGivedBestSelectInSouthEast.Clear();
            //// Delete task entries from SQL database for the current point
            using (var dbDelete = new DataClasses1DataContext())
            {
                var getData = (from TbTaskLists in dbDelete.TbTaskLists where TbTaskLists.Point_ID == point.ID select TbTaskLists);
                dbDelete.TbTaskLists.DeleteAllOnSubmit(getData.ToList());
                dbDelete.SubmitChanges();
            }

            // Return the winning rescue ID
            return winRescueID;
        }
        //**************//
        // Select a winning rescue ID based on probability distribution
        public string SelectWinRescueByProbability(Dictionary<string,double> dicOfProbabilityRs)
        {
            string winRescueID="";
            // Find minimum and maximum probability values
            double minValue = dicOfProbabilityRs.OrderBy(p => p.Value).First().Value;
            double maxValue = dicOfProbabilityRs.OrderBy(p => p.Value).Last().Value;
            Random r = new Random();
            double y = r.NextDouble();
            // Generate a random value within the probability range
            double value = (y * (maxValue - minValue) + minValue);
            double lastProb = 0;
            // Iterate through probabilities to select the winning rescue
            foreach (var prob in dicOfProbabilityRs.OrderBy(p => p.Value))
            {
                if (value > lastProb && value <= prob.Value)
                {
                    winRescueID = prob.Key;
                }
                lastProb = prob.Value;
            }
            return winRescueID ;
        }

        // Compute new probabilities for rescues based on reward or penalty
        public Dictionary<string,double> ComputeNewProbabilites(string winRescueID,double winRescueProbability,int b,double x, Dictionary<string, double> dicOfProbabilityRs)
        {
            Dictionary<string,double> newDicOfProbabilityRs=new Dictionary<string,double>();
            double probability = 0;
            // Iterate through each rescue probability
            foreach (var prob in dicOfProbabilityRs)
            {
                // Skip if probability is exactly 0 or 1
                if (!probability.Equals(0) || !probability.Equals(1))
                {
                    // Update probability for winning rescue
                    if (prob.Key.Equals(winRescueID))
                        probability = winRescueProbability + x * b * (1 - winRescueProbability);
                    else
                        probability = dicOfProbabilityRs[prob.Key] - x * b * (dicOfProbabilityRs[prob.Key]);
                }

                // Clamp probability between 0 and 1
                if (probability < 0)
                    newDicOfProbabilityRs.Add(prob.Key, 0);
                else if (probability > 1)
                    newDicOfProbabilityRs.Add(prob.Key, 1);
                else
                    newDicOfProbabilityRs.Add(prob.Key, probability);
            }
            return newDicOfProbabilityRs;
        }

        // Compute new probabilities for rescues when applying penalty
        public Dictionary<string, double> ComputeNewProbabilitesForPenalty(string winRescueID, double winRescueProbability, double penalty, double x, Dictionary<string, double> dicOfProbabilityRs)
        {
            Dictionary<string, double> newDicOfProbabilityRs = new Dictionary<string, double>();
            double probability = 0;
            int r = dicOfProbabilityRs.Count();
            // Iterate through each rescue probability
            foreach (var prob in dicOfProbabilityRs)
            {
                // Skip if probability is exactly 0 or 1
                if (!probability.Equals(0) || !probability.Equals(1))
                {
                    // Update probability for winning rescue with penalty
                    if (prob.Key.Equals(winRescueID))
                        probability = (1 - penalty) * winRescueProbability;
                    else
                        probability = penalty / (r - 1) + (1 - penalty) * dicOfProbabilityRs[prob.Key];
                }

                // Clamp probability between 0 and 1
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
        // Assign a point to the winning rescue and update database
        public void AssignePointToWinRescue(Search search, string winRescueID,double Probability, DomainObject.Point point,int countNewPoint, bool isWin)
        {
            Dictionary<Search,int> dic=new Dictionary<DomainObject.Search,int>();
            TbTaskList task = new TbTaskList();
            List<TbTaskList> listTasksNewPoints = new List<TbTaskList>();

            // Synchronize access to shared resources
            lock (this)
            {
                switch (isWin)
                {
                    case true:
                        // Insert tasks for new points to SQL in TbTaskList
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
                        // Insert task for main point to SQL in TbTaskList
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
                        // Add search to dictionary for win case
                        dic.Add(search, 1);
                        // If rescue is not allocated, update allocation status and add point
                        if (rescues.Find(p => p.ID.Equals(winRescueID)).IsAllocated == false)
                        {
                            rescues.Find(p => p.ID.Equals(winRescueID)).IsAllocated = true;
                            rescues.Find(p => p.ID.Equals(winRescueID)).ListPoint.Add(point, dic);
                        }
                        else
                            // If already allocated, just add point
                            rescues.Find(p => p.ID.Equals(winRescueID)).ListPoint.Add(point, dic);
                        break;
                    case false:
                        // Insert tasks for new points to SQL in TbTaskList for non-win case
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
                        // Insert task for main point to SQL in TbTaskList for non-win case
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
                        // Add search to dictionary for non-win case
                        dic.Add(search, 2);
                        rescues.Find(p => p.ID.Equals(winRescueID)).ListPoint.Add(point, dic);
                        break;
                }
            }
            // Start a new thread to perform rescue for the assigned point
            Rescue selectedRescue = rescues.Find(p => p.ID.Equals(winRescueID));
            Thread Thread = new Thread(() => DoRescueForPoint(selectedRescue, point,countNewPoint));
            AllRescueThreads.Add(Thread);
            Thread.Start();
        }

        // Perform rescue operation for a given point
        public void DoRescueForPoint(Rescue selectedRescue, DomainObject.Point selectedPoint, int countNewPoint)
        {
            // Synchronize access to the selected rescue
            lock (selectedRescue)
            {
                // Example of iterating through all points for rescue (commented out)
                // ...existing code...

                // Set rescue state to busy and update status
                selectedRescue.State = Rescue.RescueStateType.Busy;
                selectedRescue.IsDoing = true;
                idelRescue--;
                busyRescue++;

                // Update point allocation and rescue status
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
                // Store top and left coordinates of the selected point
                pointTop = selectedPoint.Top;
                pointLeft = selectedPoint.Left;

                // Find rescue and point labels in the panel controls
                Label lblRescue = (Label)panel1.Controls.Find(selectedRescue.ID, true)[0];
                Label lblPoint = (Label)panel1.Controls.Find(selectedPoint.ID, true)[0];

                // Calculate distances between rescue and point
                int xDistance = Math.Abs(lblRescue.Location.X - lblPoint.Location.X);
                int yDistance = Math.Abs(lblRescue.Location.Y - lblPoint.Location.Y);

                // Divide movement into 10 steps
                int total = 10;
                int xpart = xDistance / total;
                int ypart = yDistance / total;
                int totalTimeSleep = 0;
                int newCordinationX = 0;
                int newCordinationY = 0;

                // Track start and end time for rescue operation
                double startTime = Math.Floor(ss.Elapsed.TotalSeconds);
                double endTime=0;

                // Animate rescue movement step by step
                for (int i = 0; i < total; i++)
                {
                    // Sleep to simulate rescue movement speed
                    System.Threading.Thread.Sleep(int.Parse(nudRescue.Value.ToString()) * 30);
                    totalTimeSleep += (int.Parse(nudSearch.Value.ToString()) * 30);

                    // Move rescue label towards point label (southeast direction)
                    if (lblRescue.Location.X < lblPoint.Location.X && lblRescue.Location.Y < lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X + 10;
                            newCordinationY = lblRescue.Location.Y + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part plus 10 units
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X + xpart + 10;
                            newCordinationY = lblRescue.Location.Y + ypart + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Move rescue label towards point label (southwest direction)
                    else if (lblRescue.Location.X < lblPoint.Location.X && lblRescue.Location.Y > lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X + 10;
                            newCordinationY = lblRescue.Location.Y - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part plus 10 units
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X + xpart + 10;
                            newCordinationY = lblRescue.Location.Y - ypart - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Move rescue label towards point label (northeast direction)
                    // Move rescue label towards point label (northeast direction)
                    else if (lblRescue.Location.X > lblPoint.Location.X && lblRescue.Location.Y > lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X - 10;
                            newCordinationY = lblRescue.Location.Y - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part minus 10 units
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X - xpart - 10;
                            newCordinationY = lblRescue.Location.Y - ypart - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Move rescue label towards point label (northwest direction)
                    else if (lblRescue.Location.X > lblPoint.Location.X && lblRescue.Location.Y < lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X) && Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X - 10;
                            newCordinationY = lblRescue.Location.Y + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part plus 10 units
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X - xpart - 10;
                            newCordinationY = lblRescue.Location.Y + ypart + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Move rescue label vertically downwards
                    else if (lblRescue.Location.X == lblPoint.Location.X && lblRescue.Location.Y < lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X ;
                            newCordinationY = lblRescue.Location.Y + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part plus 10 units
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X ;
                            newCordinationY = lblRescue.Location.Y + ypart + 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Move rescue label vertically upwards
                    else if (lblRescue.Location.X == lblPoint.Location.X && lblRescue.Location.Y > lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Top - 10, 20).Contains(lblRescue.Location.Y))
                        {
                            newCordinationX = lblRescue.Location.X;
                            newCordinationY = lblRescue.Location.Y - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part minus 10 units
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X;
                            newCordinationY = lblRescue.Location.Y - ypart - 10;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Move rescue label horizontally leftwards
                    else if (lblRescue.Location.X > lblPoint.Location.X && lblRescue.Location.Y == lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X))
                        {
                            newCordinationX = lblRescue.Location.X - 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part minus 10 units
                        else if (i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X - xpart - 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Move rescue label horizontally rightwards
                    else if (lblRescue.Location.X < lblPoint.Location.X && lblRescue.Location.Y == lblPoint.Location.Y)
                    {
                        // If rescue is close to the point, move by 10 units
                        if (Enumerable.Range(selectedPoint.Left - 10, 20).Contains(lblRescue.Location.X))
                        {
                            newCordinationX = lblRescue.Location.X + 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // On last step, move by calculated part plus 10 units
                        else if(i.Equals(9))
                        {
                            newCordinationX = lblRescue.Location.X + xpart + 10;
                            newCordinationY = lblRescue.Location.Y;
                            if (lblRescue.InvokeRequired)
                                lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY); });
                            else
                                lblRescue.Location = new System.Drawing.Point(newCordinationX, newCordinationY);
                        }
                        // Otherwise, move by calculated part
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
                    // Bring rescue label to front after movement
                    if (lblRescue.InvokeRequired)
                        lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.BringToFront(); });
                    else
                        lblRescue.BringToFront();

                    // Store the new location of the rescue label
                    System.Drawing.Point MyPoint = new System.Drawing.Point(newCordinationX, newCordinationY);
                    // Convert pixel coordinates to geographic projection
                    DotSpatial.Topology.Coordinate MyCoordinate = geoMap.PixelToProj(MyPoint);

                    // Update rescue coordinates in the database
                    using (var dbUpdate = new DataClasses1DataContext())
                    {
                        // Update rescue position in TbRescues table
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
                // Record end time for rescue operation
                endTime = Math.Floor(ss.Elapsed.TotalSeconds);

                // Bring rescue label to front after final movement
                if (lblRescue.InvokeRequired)
                    lblRescue.BeginInvoke((MethodInvoker)delegate { lblRescue.BringToFront(); });
                else
                    lblRescue.BringToFront();

                // Perform rescue action for the selected point
                selectedPoint.DoRescue();

                // Update rescue and point states after completion
                selectedPoint.StartRescueDoing = false;
                selectedPoint.EndRescueDoing = true;
                selectedRescue.State = Rescue.RescueStateType.Ready;
                idelRescue++;
                busyRescue--;

                // Update UI to show new information
                lblPoint.Image = Properties.Resources.Point_Green;
                lock (this)
                {
                    List<string> idOfNewPoints = new List<string>();
                    // Get IDs of new points from database
                    using (var dbSelect = new DataClasses1DataContext())
                    {
                        idOfNewPoints = (from TbPoints in dbSelect.TbPoints
                                         where TbPoints.Point_ParentID.Equals(selectedPoint.ID)
                                         select TbPoints.Point_ID).ToList();
                    }

                    // Update UI for each new point
                    for (int i = 0; i < idOfNewPoints.Count(); i++)
                    {
                        Label myLabel = (Label)panel1.Controls.Find(idOfNewPoints[i], true)[0];
                        myLabel.Image = Properties.Resources.Point_Green;
                    }
                }
                // Calculate and update rescue statistics
                totalDistanceRescue += GetDistance(pointTop, rescueTop, pointLeft, rescueLeft);
                totalTimeOfDistanceRs += (Math.Round(GetDistance(pointTop, rescueTop, pointLeft, rescueLeft)) / 5);

                // Show chart for rescue tasks
                int time = (totalTimeSleep / 1000) + 2;
                numPointDoRescue = numPointDoRescue - countNewPoint;
                numPointDoRescue--;
                ShowChartTasksOfRescue(time);

                // Update rescue and point states in the database
                using (var dbUpdate = new DataClasses1DataContext())
                {
                    var point = (from TbPoints in dbUpdate.TbPoints
                                 where TbPoints.Point_ID == selectedPoint.ID
                                 select TbPoints).Single();
                    point.Point_StartRescueDoing = selectedPoint.StartRescueDoing;
                    point.Point_EndRescueDoing = selectedPoint.EndRescueDoing;
                    dbUpdate.SubmitChanges();
                    // Update new points related to the selected point
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
                    // Update rescue state and coordinates in the database
                    rescue.Rescue_State = (int)selectedRescue.State;
                    rescue.Rescue_LeftCoordinate = newCordinationX;
                    rescue.Rescue_TopCoordinate = newCordinationY;
                    dbUpdate.SubmitChanges();
                }

            }
            // Mark rescue as not doing after operation
            selectedRescue.IsDoing = false;
        }
        //**********************************************//
        // Select the next point for a search based on its container region
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

        // Select the next available point in NorthWest region for a search
        public DomainObject.Point SelectNexPointInNorthWest(Search search)
        {
            lock (search)
            {
                // Get all points in NorthWest region that haven't been searched
                List<DomainObject.Point> orderdPointInNorthWest = points.Where(p => p.Container == UnitType.NorthWest && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();
                // Calculate distances for all points
                foreach (var item in orderdPointInNorthWest)
                {
                    item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                // Order points by distance
                orderdPointInNorthWest = orderdPointInNorthWest.OrderBy(p => p.Distance).ToList();
                // Find the next available point
                lock (this)
                {
                    while (orderdPointInNorthWest.Any(p => p.StartSearchDoing.Equals(false)))
                    {
                        string nextPointID = orderdPointInNorthWest.First(p => p.StartSearchDoing.Equals(false) && p.EndSearchDoing.Equals(false)).ID;
                        if (points.First(p => p.ID.Equals(nextPointID)).StartSearchDoing.Equals(false) && points.First(p => p.ID.Equals(nextPointID)).EndSearchDoing.Equals(false))
                        {
                            points.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            orderdPointInNorthWest.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            return orderdPointInNorthWest.Find(p => p.ID.Equals(nextPointID));
                        }
                    }
                }
            }
            return null;
        }

        // Select the next available point in NorthEast region for a search
        public DomainObject.Point SelectNexPointInNorthEast(Search search)
        {
            lock (search)
            {
                // Get all points in NorthEast region that haven't been searched
                List<DomainObject.Point> orderdPointInNorthEast = points.Where(p => p.Container == UnitType.NorthEast && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();
                // Calculate distances for all points
                foreach (var item in orderdPointInNorthEast)
                {
                    item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                // Order points by distance
                orderdPointInNorthEast = orderdPointInNorthEast.OrderBy(p => p.Distance).ToList();
                // Find the next available point
                lock (this)
                {
                    while (orderdPointInNorthEast.Any(p => p.StartSearchDoing.Equals(false)))
                    {
                        string nextPointID = orderdPointInNorthEast.First(p => p.StartSearchDoing.Equals(false) && p.EndSearchDoing.Equals(false)).ID;
                        if (points.First(p => p.ID.Equals(nextPointID)).StartSearchDoing.Equals(false) && points.First(p => p.ID.Equals(nextPointID)).EndSearchDoing.Equals(false))
                        {
                            points.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            orderdPointInNorthEast.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            return orderdPointInNorthEast.Find(p => p.ID.Equals(nextPointID));
                        }
                    }
                }
            }
            return null;
        }

        // Select the next available point in SouthWest region for a search
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
                    //if (orderdPointInNorthWest.Count != 0 && orderdPointInNorthWest.Any(p => p.StartSearchDoing.Equals(false)))
                    //    return orderdPointInNorthWest.First(p => p.StartSearchDoing.Equals(false));
                    //else
                    //    return null;
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

        // Select the next available point in SouthEast region for a search
        public DomainObject.Point SelectNexPointInSouthEast(Search search)
        {
            lock (search)
            {
                // Get all points in SouthEast region that haven't been searched
                List<DomainObject.Point> orderdPointInSouthEast = points.Where(p => p.Container == UnitType.SouthEast && p.EndSearchDoing == false && p.StartSearchDoing == false).ToList();
                // Calculate distances for all points
                foreach (var item in orderdPointInSouthEast)
                {
                    item.Distance = GetDistance(item.Left, search.ListPoint.Last().Left, item.Top, search.ListPoint.Last().Top);
                }
                // Order points by distance
                orderdPointInSouthEast = orderdPointInSouthEast.OrderBy(p => p.Distance).ToList();
                // Find the next available point
                lock (this)
                {
                    while (orderdPointInSouthEast.Any(p => p.StartSearchDoing.Equals(false)))
                    {
                        string nextPointID = orderdPointInSouthEast.First(p => p.StartSearchDoing.Equals(false) && p.EndSearchDoing.Equals(false)).ID;
                        if (points.First(p => p.ID.Equals(nextPointID)).StartSearchDoing.Equals(false) && points.First(p => p.ID.Equals(nextPointID)).EndSearchDoing.Equals(false))
                        {
                            points.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            orderdPointInSouthEast.Find(p => p.ID.Equals(nextPointID)).StartSearchDoing = true;
                            return orderdPointInSouthEast.Find(p => p.ID.Equals(nextPointID));
                        }
                    }
                }
            }
            return null;
        }
        //***********************************************//
        #region Best Performance
        // The following region contains legacy code for drawing rescue lines and invoking rescue actions
        // The commented code below was used for visualizing rescue operations and is kept for reference.
        //private void DoRescuInvoke(int rescTop, int rescLeft, int pointTop, int pointLeft, DomainObject.Point point)
        //{
        //    System.Drawing.Pen myPen;
        //    myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        //    System.Drawing.Graphics formGraphics = panel1.CreateGraphics();
        //    formGraphics.DrawLine(myPen, rescLeft, rescTop, pointLeft, pointTop);
        //    point.DoSearch();
        //    //Clean
        //    myPen.Color = Color.White;
        //    formGraphics.DrawLine(myPen, rescLeft, rescTop, pointLeft, pointTop);
        //    formGraphics.Dispose();
        //    myPen.Dispose();
        //}
        //delegate void DoRescuInvoker(int rescTop, int rescLeft, int pointTop, int pointLeft, DomainObject.Point point);
        #endregion

        // Handle stop button click to pause/resume threads and update UI
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
            // Retrieve selected search ID from dropdown
            int searchId = (int) cboxSearchIntID.SelectedItem;
            // Initialize variables for search, rescue, and point information
            string searchID = "", searchIntID = "", searchTop = "", searchLeft = "", searchX = "", searchY = "",searchState="";
            string rescueID = "", rescueIntID = "", rescueTop = "", rescueLeft = "", rescueX = "", rescueY = "", pointID = "";
            string pointIntID="", pointRescueLevel = "", pointNumVictim = "";
            bool isAccepted = false;

            // Bind dropdowns for search state and rescue level
            BindDropDownSearchState();
            BindDropDownRescueLevel();

            // Remove all arrow controls from panel
            for (int index = panel1.Controls.Count - 1; index >= 0; index--)
            {
                if (panel1.Controls[index].Name.StartsWith("arrow"))
                {
                    panel1.Controls.RemoveAt(index);
                }
            }

            // Query database for search information and update UI
            using (var dbSelect = new DataClasses1DataContext())
            {
                // Get search information from database
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
                // Update UI with search information
                txtSearchID.Text = searchIntID;
                txtSearchX.Text = searchX;
                txtSearchY.Text = searchY;
                comboxSearchState.SelectedIndex = comboxSearchState.FindStringExact(searchState);
                // Retrieve task and point information if available
                if (dbSelect.TbTaskLists.Any(p => p.Search_ID == searchID))
                {
                    var task = (from TbTaskLists in dbSelect.TbTaskLists
                                where TbTaskLists.Search_ID.Equals(searchID) && TbTaskLists.Point_ParentID.Equals(0)
                                select TbTaskLists).ToList().Last();

                    pointID = task.Point_ID;

                    // Retrieve rescue information if assigned
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

                    // Retrieve point information
                    var point = (from TbPoints in dbSelect.TbPoints
                                 where TbPoints.Point_ID.Equals(pointID)
                                 select TbPoints).Single();

                    pointIntID = point.Point_IntID.ToString();
                    pointRescueLevel = DomainObject.Point.GetRescueLevel(point.Point_NumVictim).ToString();
                    pointNumVictim = point.Point_NumVictim.ToString();
                }
                // Update UI with rescue and point information
                txtRescueID.Text = rescueIntID;
                txtRescueX.Text = rescueX;
                txtRescueY.Text = rescueY;
                radioAccept.Checked = isAccepted;
                radioReject.Checked = isAccepted;
                txtPointID.Text = pointIntID;
                comboxRescueLevel.SelectedIndex = comboxRescueLevel.FindStringExact(pointRescueLevel);
                txtNumVictim.Text = pointNumVictim;
                txtIntefraceRescueID.Text = rescueIntID;
                txtInterfaceRescueX.Text = rescueX;
                txtIntefraceRescueY.Text = rescueY;
                // Populate rescue task list grid if rescue exists
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
            }

            // Add arrow labels to panel for search and rescue locations
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