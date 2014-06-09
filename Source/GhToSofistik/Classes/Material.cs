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
            E = material.E /10000;
            G = material.G / 10000;
            gamma = material.gamma;
            alphaT = material.alphaT;
            fy = material.fy / 10000;
            name = material.name;
        }

        public string sofistring() {
            // We need not to forget ton convert into units used by Sofistik
            return "STEE NO " + id + " ES " + Math.Truncate(E * 1000) / 100
                                + " GAM " + Math.Truncate(gamma * 100) / 100
                                + " ALFA " + Math.Truncate(alphaT * 100) / 100
                                + " GMOD " + Math.Truncate(G * 1000) / 100
                                + " FY " + Math.Truncate(fy * 1000) / 100;
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
