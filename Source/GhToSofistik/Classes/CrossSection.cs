using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class CrossSection {
        public int id;
        public List<string> ids; // Elements to apply to
        public string type;
        public double diameter, thickness, height, upperWidth, lowerWidth, upperThick, lowerThick, sWallThick, webThick, filletRadius;
        public string name;
        
        public CrossSection(Karamba.CrossSections.CroSec crosec) {
            id = 0;
            ids = new List<string>();
            diameter = thickness = height = upperWidth = lowerWidth = upperThick = lowerThick = sWallThick = webThick = filletRadius = 0;
            type = "";
            name = "";

            hydrate(crosec);
        }

        public void hydrate(Karamba.CrossSections.CroSec crosec) {
            id = (int) crosec.ind;
            ids = crosec.elemIds;
            name = crosec.name;
            
            if (name == "Trapezoid") {
                height = (double) crosec.dims[0];
                upperWidth = (double) crosec.dims[2];
                lowerWidth = (double) crosec.dims[4];
            }
            else if (name == "O-Profile") {
                diameter = (double) crosec.dims[0];
                thickness = (double) crosec.dims[1];
            }
            else if (name == "[]-Profile") {
                height = (double) crosec.dims[0];
                sWallThick = (double) crosec.dims[1];
                upperWidth = (double) crosec.dims[2];
                upperThick = (double) crosec.dims[3];
                lowerWidth = (double) crosec.dims[4];
                lowerThick = (double) crosec.dims[5];
                filletRadius = (double) crosec.dims[6];
            }
        }

        public string sofistring() {
            return "SREC";
        }
    }
}
