using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Node {
        public int id;
        public double x, y, z;
        public List<string> constraints;

        public Node(Karamba.Nodes.Node node) {
            id = IdManager.createId("nod");
            x = y = z = 0;
            constraints = new List<string>();

            hydrate(node);
        }

        public void hydrate(Karamba.Nodes.Node node) {
            x = node.pos.X;
            y = node.pos.Y;
            z = node.pos.Z;
        }

        public string sofistring() {
            return "NODE " + id + " X " + x + " Y " + y + " Z " + z + "\n";
        }
    }
}