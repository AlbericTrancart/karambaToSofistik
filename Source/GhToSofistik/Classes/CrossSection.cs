using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class CrossSection {
        public int id;
        public List<string> ids; // Elements to apply to
        public string type;
        public double diameter, thickness, upperWidth, lowerWidth, upperThick, lowerThick, sWallThick, webThick, filletRadius;
        public string name;

        public CrossSection(Karamba.CrossSections.CroSec crosec) {
            id = 0;
            ids = new List<string>();
            diameter = thickness = upperWidth = lowerWidth = upperThick = lowerThick = sWallThick = webThick = filletRadius = 0;
            type = "";
            name = "";

            hydrate(crosec);
        }

        public void hydrate(Karamba.CrossSections.CroSec crosec) {
            id = (int) crosec.ind;
            ids = crosec.elemIds;
            name = crosec.name;
        }

        public string sofistring() {
            return "SREC";
        }
    }
}
