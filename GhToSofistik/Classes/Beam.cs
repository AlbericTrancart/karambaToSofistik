using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Beam : Karamba.Utilities.BeamSet {
        public int id;
        public string GHid;
        public List<string> ids;
        public Node start;
        public Node end;
        public CrossSection sec;

        public Beam(Karamba.Utilities.BeamSet beam) {
            id = IdManager.createId("beam");
            GHid = "";
            ids = new List<string>();

            hydrate(beam);
        }

        public void hydrate(Karamba.Utilities.BeamSet beam) {


        }

        public string sofistring() {
            return "CONC \n";
        }
    }
}
