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
            ids  = new List<string>();
            id   = 1;
            E    = G 
                 = gamma 
                 = alphaT 
                 = fy 
                 = 0;
            name = "";

            if (material != null)
                hydrate(material);
        }

        public void hydrate(Karamba.Materials.FemMaterial material) {
            id = (int) material.ind; // We do not add 1 as for other elements because the first material being corrupted we need to delete it, i.e starting id counting 1 lower. id + 1 - 1 = id.
            ids    = material.elemIds;
            E      = Math.Round(material.E / 1000, 3);
            G      = Math.Round(material.G / 1000, 3);
            gamma  = Math.Round(material.gamma, 3);
            alphaT = Math.Round(material.alphaT, 3);
            fy     = Math.Round(material.fy / 1000, 3);
            name   = material.name;
        }

        public string sofistring() {
            // We need not to forget ton convert into units used by Sofistik
            return "STEE NO " + id + " ES "   + E
                                   + " GAM "  + gamma
                                   + " ALFA " + alphaT
                                   + " GMOD " + G
                                   + " FY "   + fy;
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
