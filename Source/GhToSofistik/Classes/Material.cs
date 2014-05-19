using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Material {
        public int id;
        public int GHid;
        public List<string> ids; // Elements to apply to
        public double E, G, gamma, alphaT, fy;
        string name;

        public Material(Karamba.Materials.FemMaterial material) {
            id = IdManager.createId("material");
            ids = new List<string>();
            GHid = 0;
            E = G = gamma = alphaT = fy = 0;
            name = "";

            hydrate(material);
        }

        public void hydrate(Karamba.Materials.FemMaterial material) {
            GHid = (int) material.ind;
            ids = material.elemIds;
            E = material.E;
            G = material.G;
            gamma = material.gamma;
            alphaT = material.alphaT;
            fy = material.fy;
            name = material.name;
        }

        public string sofistring() {
            return "CONC ";
        }
    }
}
