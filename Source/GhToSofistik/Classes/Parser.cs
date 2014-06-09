using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Parser {
        public string file { get; protected set; }

        public Parser(List<Material> materials, List<CrossSection> crossSections, List<Node> nodes, List<Beam> beams, List<Load> loads) {
            file = "";
            
            // AQUA definitions
            file += "+PROG AQUA urs:1\nHEAD Material and cross section definitions\n\n";

            foreach (Material material in materials) {
                if(material.sofistring() != "")
                    file += material.sofistring() + "\n\n";
            }
            file += "\n";
            foreach (CrossSection crossSection in crossSections) {
                if (crossSection.sofistring() != "")
                    file += crossSection.sofistring() + "\n";
            }

            // SOFIMSHA definitions
            file += "\nEND\n\n+PROG SOFIMSHA urs:2\nHEAD Elements\n\nPAGE UNII\nSYST 3D GDIR NEGZ GDIV 1000\n\n";

            foreach (Node node in nodes) {
                file += node.sofistring() + "\n";
            }
            file += "\n";
            foreach (Beam beam in beams) {
                file += beam.sofistring() + "\n";
            }

            // SOFILOAD definitions
            file += "\n\n+PROG SOFILOAD urs:4\nHEAD Loads\n\n";
            foreach (Load load in loads) {
                file += load.sofistring() + "\n";
            }

            // Analysis
            file += "END\n\n+PROG ASE urs:13\nHEAD Solving\n\nSYST PROB line\nLC ALL\n\nEND\n";
        }
    }
}
