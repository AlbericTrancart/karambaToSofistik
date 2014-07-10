using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace karambaToSofistik.Classes {
    class CrossSection {
        public int id;
        public List<string> ids; // Elements to apply to
        public string shape;     // Available: O, [], V, I
        public double diameter, thickness, height, 
                      upperWidth, lowerWidth, upperThick, lowerThick, 
                      sWallThick, webThick, filletRadius;
        public string name;
        public Material material;
        
        public CrossSection(Karamba.CrossSections.CroSec crosec = null) {
            id       = 1;
            ids      = new List<string>();
            shape    = "";
            name     = "";
            material = new Material();
            diameter = thickness 
                     = height 
                     = upperWidth 
                     = lowerWidth 
                     = upperThick 
                     = lowerThick 
                     = sWallThick 
                     = webThick 
                     = filletRadius 
                     = 0;

            if(crosec != null)
                hydrate(crosec);
        }

        public void hydrate(Karamba.CrossSections.CroSec crosec) {
            id = (int) crosec.ind + 1; // Sofistik begins at 1 not 0
            ids = crosec.elemIds;
            name = crosec.name;
            shape = crosec.shape();

            if (shape == "V") {
                height       = Math.Round((double) crosec.dims[0] * 1000, 3);
                upperWidth   = Math.Round((double) crosec.dims[2] * 1000, 3);
                lowerWidth   = Math.Round((double) crosec.dims[4] * 1000, 3);
            }
            else if (shape == "O") {
                diameter     = Math.Round((double) crosec.dims[0] * 1000, 3);
                thickness    = Math.Round((double) crosec.dims[1] * 1000, 3);
            }
            else if (shape == "[]") {
                height       = Math.Round((double) crosec.dims[0] * 1000, 3);
                sWallThick   = Math.Round((double) crosec.dims[1] * 1000, 3);
                upperWidth   = Math.Round((double) crosec.dims[2] * 1000, 3);
                upperThick   = Math.Round((double) crosec.dims[3] * 1000, 3);
                lowerWidth   = Math.Round((double) crosec.dims[4] * 1000, 3);
                lowerThick   = Math.Round((double) crosec.dims[5] * 1000, 3);
                filletRadius = Math.Round((double) crosec.dims[6] * 1000, 3);
            }
            else if (shape == "I") {
                height       = Math.Round((double) crosec.dims[0] * 1000, 3);
                webThick     = Math.Round((double) crosec.dims[1] * 1000, 3);
                upperWidth   = Math.Round((double) crosec.dims[2] * 1000, 3);
                upperThick   = Math.Round((double) crosec.dims[3] * 1000, 3);
                lowerWidth   = Math.Round((double) crosec.dims[4] * 1000, 3);
                lowerThick   = Math.Round((double) crosec.dims[5] * 1000, 3);
                filletRadius = Math.Round((double) crosec.dims[6] * 1000, 3);
            }
        }

        public string sofistring() {
            // Sofistik wants millimeters
            if (shape == "V") {
                return "SECT " + id + " MNO " + material.id
                               + "\nPLAT NO 1 YB " + (-upperWidth / 2)
                                          + " ZB " + height
                                          + " YE " + (upperWidth / 2)
                                          + " ZE " + height 
                                          + " T 10"
                               + "\nNO 2 YB " + (upperWidth / 2)
                                     + " ZB " + height
                                     + " YE " + (upperWidth / 2)
                                     + " ZE " + 0 
                                     + " T 10"
                               + "\nNO 3 YB " + (upperWidth / 2)
                                     + " ZB " + 0
                                     + " YE " + (-upperWidth / 2)
                                     + " ZE " + 0
                                     + " T 10"
                               + "\nNO 4 YB " + (-upperWidth / 2)
                                     + " ZB " + 0
                                     + " YE " + (-upperWidth / 2)
                                     + " ZE " + height
                                     + " T 10";
            }
            else if (shape == "O") {
                return "TUBE " + id + " MNO " + material.id
                                    + " D "   + diameter        
                                    + " T "   + thickness;
            }
            else if (shape == "[]") {
                return "SREC " + id + " MNO " + material.id
                                    + " H "   + height
                                    + " HO "  + Math.Max(lowerThick, upperThick)
                                    + " B "   + lowerWidth
                                    + " BO "  + upperWidth;
            }
            else if (shape == "I") {
                return "SECT " + id + " MNO " + material.id
                               + "\nPLAT NO 1 YB " + (-upperWidth / 2)
                                          + " ZB " + (height - upperThick / 2)
                                          + " YE " + 0
                                          + " ZE " + (height - upperThick / 2)
                                          + " T " + upperThick
                               + "\nNO 2 YB " + 0
                                     + " ZB " + (height - upperThick / 2)
                                     + " YE " + (upperWidth / 2)
                                     + " ZE " + (height - upperThick / 2)
                                     + " T " + upperThick
                               + "\nNO 3 YB " + 0
                                     + " ZB " + (height - upperThick / 2)
                                     + " YE " + 0
                                     + " ZE " + (lowerThick / 2)
                                     + " T " + webThick
                               + "\nNO 4 YB " + (-lowerWidth / 2)
                                     + " ZB " + (lowerThick / 2)
                                     + " YE " + 0
                                     + " ZE " + (lowerThick / 2)
                                     + " T " + lowerThick
                               + "\nNO 5 YB " + 0
                                     + " ZB " + (lowerThick / 2)
                                     + " YE " + (lowerWidth / 2)
                                     + " ZE " + (lowerThick / 2)
                                     + " T " + lowerThick;
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
