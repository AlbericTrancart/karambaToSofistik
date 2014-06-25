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
            force = load.Load;
        }

        public string sofistring() {
            id = id_count;
            id_count++;

            if (type == "G")
                return "LC NO " + id + " TYPE P DLX " + force.X
                                            + " DLY " + force.Y
                                            + " DLZ " + force.Z;
            else if (type == "P")
                return "LC NO " + id + " TYPE L\nNODE NO " + node.id
                                             + " TYPE PP"
                                             + " P1 " + force.X
                                             + " P2 " + force.Y
                                             + " P3 " + force.Z;
            else if (type == "E")
                return "LC NO " + id + " TYPE L\nBEAM " + beam.id
                                             + " TYPE " + ((orientation == 1) ? "PXXPYYPZZ" : "PXPYPZ")
                                             + " PA " + force.X + "," + force.Y + "," + force.Z;
            return "";
        }
    }
}
