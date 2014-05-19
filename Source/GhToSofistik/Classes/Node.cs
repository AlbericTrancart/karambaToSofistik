using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Node {
        public int id;
        public int GHid;
        public double x, y, z;
        public List<string> constraints;

        public Node(Karamba.Nodes.Node node) {
            id = IdManager.createId("node");
            GHid = 0;
            x = y = z = 0;
            constraints = new List<string>();

            hydrate(node);
        }

        public void hydrate(Karamba.Nodes.Node node) {
            x = node.pos.X;
            y = node.pos.Y;
            z = node.pos.Z;
            GHid = node.ind;
        }

        public string sofistring() {
            string sofi = "";
            sofi += "NODE " + id + " X " + x + " Y " + y + " Z " + z;

            if (constraints.Count != 0) {
                sofi += " FIX ";

                foreach (string condition in constraints) {
                    sofi += condition;
                }
            }

            return sofi;
        }

        public void addConstraint(Karamba.Supports.Support support) {
            string[] cons = new string[] {"PX", "PY", "PZ", "MX", "MY", "MZ"};
            int i = 0;

            foreach(bool boolean in support._condition) {
                if(boolean)
                    constraints.Add(cons[i]);
                // TODO: prescribed displacement
                i++;
            }
        }
    }
}