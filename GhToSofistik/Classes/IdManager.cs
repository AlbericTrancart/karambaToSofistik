using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class IdManager {
        static private int materials = 0;
        static private int crossSections = 0;
        static private int nodes = 0;
        static private int beams = 0;
        static private int loads = 0;
        static private int other = 0;
        
        static public int createId(string type) {
            switch(type) {
                case "material": materials++; return materials; break;
                case "crosec": crossSections++; return crossSections; break;
                case "node": nodes++; return nodes; break;
                case "beam": beams++; return beams; break;
                case "load": loads++; return loads; break;
                case "other": other++; return other; break;
                default: return 0;
            }
        }
    }
}
