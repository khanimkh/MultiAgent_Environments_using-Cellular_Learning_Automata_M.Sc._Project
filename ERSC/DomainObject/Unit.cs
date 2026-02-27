using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSC.DomainObject
{
    // Enum representing the unit (region) types in the map
    public enum UnitType
    {
        NorthWest,
        SouthWest,
        NorthEast,
        SouthEast,
        None
    };

    // Represents unit (region) logic and counters for search/rescue operations
    public class Unit
    {
        // Determines the unit type based on x and y coordinates
        public static UnitType GetUnit(int x, int y)
        {
            if (x >= Map.StartX && x <= Map.EndX / 2)
            {
                if (y >= Map.StartY && y <= (Map.EndY / 2))
                {
                    return UnitType.NorthWest;
                }
                else
                {
                    return UnitType.SouthWest;
                }
            }
            else if (x >= Map.StartX / 2 && x <= Map.EndX)
            {
                if (y >= Map.StartY && y <= (Map.EndY / 2))
                {
                    return UnitType.NorthEast;
                }
                else
                {
                    return UnitType.SouthEast;
                }
            }
            return UnitType.None;
        }

        // Search team counters for each unit
        public static int NorthWestSearchCount
        {
            get;
            set;
        }

        public static int SouthWestSearchCount
        {
            get;
            set;
        }

        public static int NorthEastSearchCount
        {
            get;
            set;
        }

        public static int SouthEastSearchCount
        {
            get;
            set;
        }

        // Rescue team counters for each unit
        public static int NorthWestRescueCount
        {
            get;
            set;
        }

        public static int SouthWestRescueCount
        {
            get;
            set;
        }

        public static int NorthEastRescueCount
        {
            get;
            set;
        }

        public static int SouthEastRescueCount
        {
            get;
            set;
        }
    }
}