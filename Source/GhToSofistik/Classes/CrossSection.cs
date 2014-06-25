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
                height = (double) crosec.dims[0];
                upperWidth = (double) crosec.dims[2];
                lowerWidth = (double) crosec.dims[4];
            }
            else if (shape == "O") {
                diameter = (double) crosec.dims[0];
                thickness = (double) crosec.dims[1];
            }
            else if (shape == "[]") {
                height = (double) crosec.dims[0];
                sWallThick = (double) crosec.dims[1];
                upperWidth = (double) crosec.dims[2];
                upperThick = (double) crosec.dims[3];
                lowerWidth = (double) crosec.dims[4];
                lowerThick = (double) crosec.dims[5];
                filletRadius = (double) crosec.dims[6];
            }
            else if (shape == "I") {
                height = (double) crosec.dims[0];
                webThick = (double) crosec.dims[1];
                upperWidth = (double) crosec.dims[2];
                upperThick = (double) crosec.dims[3];
                lowerWidth = (double) crosec.dims[4];
                lowerThick = (double) crosec.dims[5];
                filletRadius = (double) crosec.dims[6];
            }
        }

        public string sofistring() {
            // Sofistik wants millimeters
            if (shape == "V") {
                return "SREC " + id + " MNO " + material.id;
            }
            else if (shape == "O") {
                return "TUBE " + id + " MNO " + material.id
                                    + " D " + Math.Truncate(diameter * 1000)        
                                    + " T " + Math.Truncate(thickness * 1000);
            }
            else if (shape == "[]") {
                return "SREC " + id + " MNO " + material.id 
                                    + " H " + Math.Truncate(height * 1000)
                                    + " B " + Math.Truncate(thickness * 1000)
                                    + " HO " + Math.Truncate(upperThick * 1000)
                                    + " BO " + Math.Truncate(upperWidth * 1000);
            }
            else if (shape == "I") {
                return "SREC " + id + " MNO " + material.id;
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
