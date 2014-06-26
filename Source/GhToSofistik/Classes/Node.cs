using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Node {
        public int id;
        public double x, y, z;
        public List<string> constraints;

        public Node(Karamba.Nodes.Node node = null) {
            id = 1;
            x = y = z = 0;
            constraints = new List<string>();

            if (node != null)
                hydrate(node);
        }

        public void hydrate(Karamba.Nodes.Node node) {
            x = node.pos.X;
            y = node.pos.Y;
            z = node.pos.Z;
            id = node.ind + 1; //Sofistik begins at 1 not 0
        }

        public string sofistring() {
            string sofi = "";
            sofi += "NODE NO " + id + " X " + x.ToString("F3")
                                    + " Y " + y.ToString("F3")
                                    + " Z " + z.ToString("F3"); //We only want three decimals

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