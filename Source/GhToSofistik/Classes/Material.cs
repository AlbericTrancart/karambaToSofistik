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
            if (name == "Concrete") {
                return "CONC " + id + " HA '50' FC 42.50000 FCT 4.071628 FCTK 2.850139 EC 32902.46 GAM 25 ALFA  1.00000E-05 SCM 1.500000 TYPR BILI FCR 58 ECR 38660 FBD 4.275208 FFAT 22.66667 GMOD 13709.36 KMOD 18279 GC 20 GF 0.050000 MUEC 0.200000 TITL '=HA 50 (EHE)'\nHMAT " + id + " TYPE FOUR TEMP 0 KXX 1.951408 KYY 0 KZZ 0 S 2070000 NSP 0";
            }
            else if (name == "Steel") {
                return "STEE " + id + " B '500' FY 400 FT 450 FP 400 ES 210000 GAM 0.05\nHMAT " + id + " TYPE FOUR TEMP 0";
            }
            return "STEE " + id + " ES " + E + " GAM " + gamma + " ALFA " + alphaT + " GMOD " + G + " FY " + fy + "\nHMAT " + id + " TYPE FOUR TEMP 0";
        }
    }
}
