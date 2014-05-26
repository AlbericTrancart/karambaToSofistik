using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Material {
        public int id;
        public List<string> ids; // Elements to apply to
        public double E, G, gamma, alphaT, fy;
        string name;

        public Material(Karamba.Materials.FemMaterial material) {
            ids = new List<string>();
            id = 0;
            E = G = gamma = alphaT = fy = 0;
            name = "";
            hydrate(material);
        }

        public void hydrate(Karamba.Materials.FemMaterial material) {
            id = (int) material.ind;
            ids = material.elemIds;
            E = material.E;
            G = material.G;
            gamma = material.gamma;
            alphaT = material.alphaT;
            fy = material.fy;
            name = material.name;
        }

        public string sofistring() {
            return "STEE " + id + " ES " + E + " GAM " + gamma + " ALFA " + alphaT + " GMOD " + G + " FY " + fy + "\nHMAT " + id + " TYPE FOUR TEMP 0";
        }

        //Check if "test" is a duplicate of this material - necessary because karamba adds preset materials
        public bool duplicate(Material test) {
            return (ids == test.ids
                    && E == test.E
                    && G == test.G
                    && gamma == test.gamma
                    && alphaT == test.alphaT
                    && fy == test.fy
                );
        }
    }
}
