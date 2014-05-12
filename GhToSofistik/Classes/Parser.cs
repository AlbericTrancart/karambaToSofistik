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
            file += "+PROG AQUA urs:1\nHEAD\n\n";

            foreach (Material material in materials) {
                file += material.sofistring() + "\n";
            }

            foreach (CrossSection crossSection in crossSections) {
                file += crossSection.sofistring() + "\n";
            }

            // SOFIMSHA definitions
            file += "END\n\n+PROG SOFIMSHA urs:2\nHEAD MESH\nPAGE UNII\nSYST 3D GDIR NEGZ GDIV 1000\n\n";

            foreach (Node node in nodes) {
                file += node.sofistring() + "\n";
            }

            foreach (Beam beam in beams) {
                file += beam.sofistring() + "\n";
            }

            // SOFILOAD definitions
            file += "\n\n+PROG SOFILOAD urs:4\nHEAD carico\n\n";
            foreach (Load load in loads) {
                file += load.sofistring() + "\n";
            }

            // Analysis
            file += "END\n\n+PROG ASE urs:13\nSYST PROB line\nLC 11  TITL\n";
            file += "END\n";
        }
    }
}
