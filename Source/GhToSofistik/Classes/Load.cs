using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace GhToSofistik.Classes {
    class Load {
        static public int id_count = 1; //Karamba does not provide IDs so we must create one
        public int id;
        public Beam beam;               // Element to apply to for element load
        public string beam_id;
        public int orientation;         // Local (0), global (1) or projeted (2) orientation for element loads
        public Node node;               // For Point Loads
        public string type;             // Valid types are "G, P, E" for Gravity, Point and Element Load
        public Vector3d force;
        

        public void init(string par_type) {
            type = par_type;
            beam = new Beam();
            node = new Node();
            id = 1;
            force = new Vector3d();
        }

        public Load(KeyValuePair<int, Karamba.Loads.GravityLoad> load) { init("G"); hydrate(load.Value); }
        public Load(Karamba.Loads.PointLoad load) { init("P"); hydrate(load); }
        public Load(Karamba.Loads.UniformlyDistLoad load) { init("E"); hydrate(load); }

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

        public string sofistring() {
            id = id_count;
            id_count++;

            if (type == "G")
                return "LC NO " + id + " TYPE P\nBEAM FROM 1 TO 999999 TYPE PXX,PYY,PZZ" 
                                     + " PA " + force.X.ToString("F0")
                                     + ","  + force.Y.ToString("F0")
                                     + ","  + force.Z.ToString("F0");
            else if (type == "P")
                return "LC NO " + id + " TYPE L\nNODE NO " + node.id
                                     + " TYPE PP"
                                     + " P1 " + force.X.ToString("F0")
                                     + " P2 " + force.Y.ToString("F0")
                                     + " P3 " + force.Z.ToString("F0");
            else if (type == "E") {
                string from = "";
                if (beam_id == "")
                    from = "1 TO 999999";
                else
                    from = "GRP " + GhToSofistikComponent.beam_groups.IndexOf(beam_id);

                string load_type = "";
                if (orientation == 0)
                    load_type = "PX,PY,PZ";
                else if (orientation == 2)
                    load_type = "PXP,PYP,PZP";
                else
                    load_type = "PXX, PYY, PZZ";

                return "LC NO " + id + " TYPE L\nBEAM FROM " + from
                                     + " TYPE " + load_type
                                     + " PA " + force.X.ToString("F0")
                                     + "," + force.Y.ToString("F0")
                                     + "," + force.Z.ToString("F0");
            }

            return "";
        }

        // Karamba creates duplicates for element loads if beam ids are not precised
        public bool duplicate(Load test) {
            if (beam_id == "" 
                    && test.beam_id == ""
                    && type == "E"
                    && test.type == "E"
                    && force == test.force 
                    && orientation == test.orientation
                ) {
                return true;
            }
            return false;
        }
    }
}
