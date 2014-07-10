using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace karambaToSofistik.Classes {
    class Load {
        public int id;
        public Beam beam;               // Element to apply to for element load
        public string beam_id;
        public int orientation;         // Local (0), global (1) or projeted (2) orientation for element loads
        public Node node;               // For Point Loads
        public string type;             // Valid types are "G, P, E" for Gravity, Point and Element Load
        public Vector3d force;
        public double coef;             // For pretension and temperature loads
        

        public void init(string par_type) {
            type = par_type;
            beam = new Beam();
            node = new Node();
            id = 1;
            coef = 0;
            force = new Vector3d();
        }

        public Load() { init(""); }
        public Load(KeyValuePair<int, Karamba.Loads.GravityLoad> load) { init("G"); hydrate(load.Value); }
        public Load(Karamba.Loads.PointLoad load) { init("P"); hydrate(load); }
        public Load(Karamba.Loads.UniformlyDistLoad load) { init("E"); hydrate(load); }
        public Load(Karamba.Loads.PreTensionLoad load) { init("S"); hydrate(load); }
        public Load(Karamba.Loads.TemperatureLoad load) { init("T"); hydrate(load); }

        public void hydrate(Karamba.Loads.GravityLoad load) {
            force = load.force;
        }

        public void hydrate(Karamba.Loads.PointLoad load) {
            force = load.force;
        }

        public void hydrate(Karamba.Loads.UniformlyDistLoad load) {
            orientation = (int) load.q_orient;
            beam_id = load.beamId;
            force = load.Load;
        }

        public void hydrate(Karamba.Loads.PreTensionLoad load) {
            beam_id = load.beamId;
            coef = Math.Round(load.EPS0, 3);
        }

        public void hydrate(Karamba.Loads.TemperatureLoad load) {
            beam_id = load.beamId;
            coef = Math.Round(load.incT, 3);
        }

        public string sofistring() {
            id = Parser.id_count;
            Parser.id_count++;

            switch (type) { 
                case "G":
                    return "LC NO " + id + " TYPE P\nBEAM FROM 1 TO 999999 TYPE PXX,PYY,PZZ" 
                                         + " PA " + Math.Round(force.X, 3)
                                         + ","    + Math.Round(force.Y, 3)
                                         + ","    + Math.Round(force.Z, 3);
                case "P":
                    return "LC NO " + id + " TYPE L\nNODE NO " + node.id
                                         + " TYPE PP"
                                         + " P1 " + Math.Round(force.X, 3)
                                         + " P2 " + Math.Round(force.Y, 3)
                                         + " P3 " + Math.Round(force.Z, 3);
                case "E":
                case "S":
                case "T":
                    string from = "";
                    if (beam_id == "")
                        from = "1 TO 999999";
                    else
                        from = "GRP " + karambaToSofistikComponent.beam_groups.IndexOf(beam_id);

                    if (type == "E") {
                        string load_type = "";
                        if (orientation == 0)
                            load_type = "PX,PY,PZ";
                        else if (orientation == 2)
                            load_type = "PXP,PYP,PZP";
                        else
                            load_type = "PXX,PYY,PZZ";

                        return "LC NO " + id + " TYPE L\nBEAM FROM " + from
                                             + " TYPE " + load_type
                                             + " PA " + Math.Round(force.X, 3)
                                             + ","    + Math.Round(force.Y, 3)
                                             + ","    + Math.Round(force.Z, 3);
                    }
                    else if(type == "S") {
                        return "LC NO " + id + " TYPE L\nBEAM FROM " + from
                                             + " TYPE PNX PA " + coef;
                    }
                    else if (type == "T") {
                        return "LC NO " + id + " TYPE L\nBEAM FROM " + from
                                             + " TYPE TEMP PA " + coef;
                    }
                    break;
            }
            return "";
        }
    }
}
