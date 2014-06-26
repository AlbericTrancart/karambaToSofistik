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

        public Material(Karamba.Materials.FemMaterial material = null) {
            ids = new List<string>();
            id = 1;
            E = G = gamma = alphaT = fy = 0;
            name = "";

            if (material != null)
                hydrate(material);
        }

        public void hydrate(Karamba.Materials.FemMaterial material) {
            id = (int) material.ind + 1; //Sofistik begins at 1 not 0
            ids = material.elemIds;
            E = material.E / 1000;
            G = material.G / 1000;
            gamma = material.gamma;
            alphaT = material.alphaT;
            fy = material.fy / 1000;
            name = material.name;
        }

        public string sofistring() {
            // We need not to forget ton convert into units used by Sofistik
            return "STEE NO " + id + " ES "   + E.ToString("F")
                                   + " GAM "  + gamma.ToString("F")
                                   + " ALFA " + alphaT.ToString("F")
                                   + " GMOD " + G.ToString("F")
                                   + " FY "   + fy.ToString("F");
        }

        //Check if "test" is a duplicate of this material - necessary because karamba adds preset materials
        public bool duplicate(Material test) {
            if(E == test.E
                    && G == test.G
                    && gamma == test.gamma
                    && alphaT == test.alphaT
                    && fy == test.fy
                ) {
                    ids.AddRange(test.ids);
                    return true;
            }
            return false;
        }
    }
}
