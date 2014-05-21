using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Creates unique IDs for the Sofistik file
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
                case "material": materials++; return materials;
                case "crosec": crossSections++; return crossSections;
                case "node": nodes++; return nodes;
                case "beam": beams++; return beams;
                case "load": loads++; return loads;
                case "other": other++; return other;
                default: return 0;
            }
        }
    }
}
