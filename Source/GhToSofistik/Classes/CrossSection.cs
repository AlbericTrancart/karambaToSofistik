using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class CrossSection {
        public int id;
        public List<string> ids; // Elements to apply to
        public string shape;     // Available: O, [], V, I
        public double diameter, thickness, height, upperWidth, lowerWidth, upperThick, lowerThick, sWallThick, webThick, filletRadius;
        public string name;
        public Material material;
        
        public CrossSection(Karamba.CrossSections.CroSec crosec = null) {
            id = 1;
            ids = new List<string>();
            diameter = thickness = height = upperWidth = lowerWidth = upperThick = lowerThick = sWallThick = webThick = filletRadius = 0;
            shape = "";
            name = "";
            material = new Material();

            if(crosec != null)
                hydrate(crosec);
        }

        public void hydrate(Karamba.CrossSections.CroSec crosec) {
            id = (int) crosec.ind + 1; //Sofistik begins at 1 not 0
            ids = crosec.elemIds;
            name = crosec.name;
            shape = crosec.shape();

            if (shape == "V") {
                height = (double) crosec.dims[0] * 1000;
                upperWidth = (double) crosec.dims[2] * 1000;
                lowerWidth = (double) crosec.dims[4] * 1000;
            }
            else if (shape == "O") {
                diameter = (double) crosec.dims[0] * 1000;
                thickness = (double) crosec.dims[1] * 1000;
            }
            else if (shape == "[]") {
                height = (double) crosec.dims[0] * 1000;
                sWallThick = (double) crosec.dims[1] * 1000;
                upperWidth = (double) crosec.dims[2] * 1000;
                upperThick = (double) crosec.dims[3] * 1000;
                lowerWidth = (double) crosec.dims[4] * 1000;
                lowerThick = (double) crosec.dims[5] * 1000;
                filletRadius = (double) crosec.dims[6] * 1000;
            }
            else if (shape == "I") {
                height = (double) crosec.dims[0] * 1000;
                webThick = (double) crosec.dims[1] * 1000;
                upperWidth = (double) crosec.dims[2] * 1000;
                upperThick = (double) crosec.dims[3] * 1000;
                lowerWidth = (double) crosec.dims[4] * 1000;
                lowerThick = (double) crosec.dims[5] * 1000;
                filletRadius = (double) crosec.dims[6] * 1000;
            }
        }

        public string sofistring() {
            // Sofistik wants millimeters
            if (shape == "V") {
                return "SECT " + id + " MNO " + material.id
                               + "\nPLAT 1 " + (-upperWidth / 2).ToString("F0") + " "
                                             + height.ToString("F0") + " "
                                             + (upperWidth / 2).ToString("F0") + " "
                                             + height.ToString("F0") + " " 
                                             + "T 10"
                               + "\n2 " + (upperWidth / 2).ToString("F0") + " "
                                        + height.ToString("F0") + " "
                                        + (upperWidth / 2).ToString("F0") + " "
                                        + 0 + " " 
                                        + "T 10"
                               + "\n3 " + (upperWidth / 2).ToString("F0") + " "
                                        + 0 + " "
                                        + (-upperWidth / 2).ToString("F0") + " "
                                        + 0 + " "
                                        + "T 10"
                               + "\n4 " + (-upperWidth / 2).ToString("F0") + " "
                                        + 0 + " "
                                        + (-upperWidth / 2).ToString("F0") + " "
                                        + height.ToString("F0") + " "
                                        + "T 10";
            }
            else if (shape == "O") {
                return "TUBE " + id + " MNO " + material.id
                                    + " D "   + diameter.ToString("F0")        
                                    + " T "   + thickness.ToString("F0");
            }
            else if (shape == "[]") {
                return "SREC " + id + " MNO " + material.id
                                    + " H "   + height.ToString("F0")
                                    + " B "   + lowerWidth.ToString("F0");
            }
            else if (shape == "I") {
                return "SECT " + id + " MNO " + material.id
                               + "\nPLAT 1 " + (-upperWidth / 2).ToString("F0") + " " 
                                             + height.ToString("F0") + " " 
                                             + (upperWidth / 2).ToString("F0") + " "
                                             + height.ToString("F0") + " "
                                             + "T 10"
                               + "\n2 "  + (upperWidth / 2).ToString("F0") + " "
                                         + height.ToString("F0") + " "
                                         + (upperWidth / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + "T 10"
                               + "\n3 "  + (upperWidth / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + (webThick / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + "T 10"
                               + "\n4 "  + (webThick / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + (webThick / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + "T 10"
                               + "\n5 "  + (webThick / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + (lowerWidth / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + "T 10"
                               + "\n6 "  + (lowerWidth / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + (lowerWidth / 2).ToString("F0") + " "
                                         + 0 + " "
                                         + "T 10"
                               + "\n7 "  + (lowerWidth / 2).ToString("F0") + " "
                                         + 0 + " "
                                         + (-lowerWidth / 2).ToString("F0") + " "
                                         + 0 + " "
                                         + "T 10"
                               + "\n8 "  + (-lowerWidth / 2).ToString("F0") + " "
                                         + 0 + " "
                                         + (-lowerWidth / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + "T 10"
                               + "\n9 "  + (-lowerWidth / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + (-webThick / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + "T 10"
                               + "\n10 " + (-webThick / 2).ToString("F0") + " "
                                         + lowerThick.ToString("F0") + " "
                                         + (-webThick / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + "T 10"
                               + "\n11 " + (-webThick / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + (-upperWidth / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + "T 10"
                               + "\n12 " + (-upperWidth / 2).ToString("F0") + " "
                                         + (height - upperThick).ToString("F0") + " "
                                         + (-upperWidth / 2).ToString("F0") + " "
                                         + height.ToString("F0") + " "
                                         + "T 10";
            }
            return "";
        }

        // Check if "test" is a duplicate of this cross section - necessary because karamba adds preset cross sections
        public bool duplicate(CrossSection test) {
            if (shape == "V") {
                if (height == test.height && lowerWidth == test.lowerWidth && upperWidth == test.upperWidth) {
                    ids.AddRange(test.ids);
                    return true;
                }
            }
            else if (shape == "O") {
                if (diameter == test.diameter && thickness == test.thickness) {
                    ids.AddRange(test.ids);
                    return true;
                }
            }
            else if (shape == "[]") {
                if (
                    height == test.height 
                    && sWallThick == test.sWallThick 
                    && filletRadius == test.filletRadius 
                    && upperWidth == test.upperWidth 
                    && lowerWidth == test.lowerWidth 
                    && upperThick == test.upperThick 
                    && lowerThick== test.lowerThick
                ) {
                    ids.AddRange(test.ids);
                    return true;
                }
            }
            else if (shape == "I") {
                if (
                    height == test.height
                    && webThick == test.webThick
                    && filletRadius == test.filletRadius
                    && upperWidth == test.upperWidth
                    && lowerWidth == test.lowerWidth
                    && upperThick == test.upperThick
                    && lowerThick == test.lowerThick
                ) {
                    ids.AddRange(test.ids);
                    return true;
                }
            }

            return false;
        }
    }
}
