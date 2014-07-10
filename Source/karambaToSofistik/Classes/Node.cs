using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace karambaToSofistik.Classes {
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
            x = Math.Round(node.pos.X, 3);
            y = Math.Round(node.pos.Y, 3);
            z = Math.Round(node.pos.Z, 3);
            id = node.ind + 1; // Sofistik begins at 1 not 0
        }

        public string sofistring() {
            string sofi = "";
            sofi += "NODE NO " + id + " X " + x
                                    + " Y " + y
                                    + " Z " + z;

            if (constraints.Count != 0) {
                sofi += " FIX ";

                foreach (string condition in constraints) {
                    if (!Regex.IsMatch(sofi, condition, RegexOptions.IgnoreCase))
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