using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GhToSofistik.Classes {
    class Beam {
        public int id;
        public string user_id;
        public List<int> ids;
        public Node start;
        public Node end;
        public CrossSection sec;
        public Color color;

        public Beam(Karamba.Elements.ModelElement beam = null) {
            id = 1;
            ids = new List<int>();
            user_id = "";
            start = end = new Node();
            sec = new CrossSection();
            color = new Color();

            if (beam != null)
                hydrate(beam);
        }

        public void hydrate(Karamba.Elements.ModelElement beam) {
            id = beam.ind + 1;  //Sofistik begins at 1 not 0
            ids = beam._node_inds;
            user_id = beam.id;

            if(!GhToSofistikComponent.beam_groups.Contains(beam.id))
                GhToSofistikComponent.beam_groups.Add(beam.id);
            
            color = beam.color;
        }

        public string sofistring() {
            return "BEAM NO " + id + " NA " + start.id + " NE " + end.id + " NCS " + sec.id;
        }
    }
}
