using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GhToSofistik.Classes {
    class Beam {
        public int id;
        public List<int> ids; // Elements to apply to
        public Node start;
        public Node end;
        public CrossSection sec;
        public Color color;

        public Beam(Karamba.Elements.ModelElement beam) {
            id = 0;
            ids = new List<int>();
            hydrate(beam);
        }

        public void hydrate(Karamba.Elements.ModelElement beam) {
            id = beam.ind;
            ids = beam._node_inds;
            color = beam.color;
        }

        public string sofistring() {
            return "BEAM NO " + id + " NA " + start.id + " NE " + end.id + " NCS ";
        }
    }
}
