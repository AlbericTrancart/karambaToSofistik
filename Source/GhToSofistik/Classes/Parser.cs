using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhToSofistik.Classes {
    class Parser {
        static public int id_count = 1; // Karamba does not provide IDs for loads so we must create one
        public string file { get; protected set; }

        public Parser(List<Material> materials, List<CrossSection> crossSections, List<Node> nodes, List<Beam> beams, List<Load> loads) {
            file = "";
            
            // AQUA definitions
            file += "+PROG AQUA urs:1\nHEAD Material and cross section definitions\nUNIT 5\n\n";

            foreach (Material material in materials) {
                if(material.sofistring() != "")
                    file += material.sofistring() + "\n";
            }
            file += "\n";
            foreach (CrossSection crossSection in crossSections) {
                if (crossSection.sofistring() != "")
                    file += crossSection.sofistring() + "\n";
            }

            // SOFIMSHA definitions
            file += "\nEND\n\n\n+PROG SOFIMSHA urs:2\nHEAD Elements\nUNIT 5\nSYST GDIR NEGZ GDIV 1000\n\n";

            foreach (Node node in nodes) {
                file += node.sofistring() + "\n";
            }

            // Special addition of beam: we must define groups
            int cluster_start, node_start, node_end, crosec; 
            cluster_start = node_start = node_end = crosec = 1;
            int iterator = 0;
            Beam last_beam = new Beam();

            foreach (string group in GhToSofistikComponent.beam_groups) {
                file += "\nGRP " + GhToSofistikComponent.beam_groups.IndexOf(group) + ";\n";
                iterator = 0;

                foreach (Beam beam in beams) {
                    // Output one group after the other
                    if (beam.user_id == group) {
                        // Beams are automatically ordered by their ID, therefore it is simple to clear the syntax by defining them in clusters

                        last_beam = beam;
                        if (iterator == 0) {
                            // Start a new cluster
                            cluster_start = beam.id;
                            node_start = beam.start.id;
                            node_end = beam.end.id;
                            crosec = beam.sec.id;
                            iterator = 1;
                            continue;
                        }
                        
                        // Check if we are moving into another cluster
                        if(beam.id != cluster_start + iterator
                            || beam.start.id != node_start + iterator
                            || beam.end.id != node_end + iterator
                            || beam.sec.id != crosec) {

                            // End the cluster and print it
                            if(iterator == 1){
                                // Normal beam
                                file += beam.sofistring() + "\n";
                            }
                            else {
                                // Clusterized definition
                                file += "BEAM NO (" + cluster_start + " " + (cluster_start + iterator - 1) + " 1)"
                                      + " NA (" + node_start + " 1)"
                                      + " NE (" + node_end + " 1)"
                                      + " NCS " + crosec + "\n";
                            }
                            
                            // Start a new cluster
                            cluster_start = beam.id;
                            node_start = beam.start.id;
                            node_end = beam.end.id;
                            crosec = beam.sec.id;
                            iterator = 1;
                            continue;
                        }
                        else {
                            iterator++;
                        }
                    }
                }

                // Print the last cluster
                if (iterator == 1) {
                    // Normal beam
                    file += last_beam.sofistring() + "\n";
                }
                else {
                    // Clusterized definition
                    file += "BEAM NO (" + cluster_start + " " + (cluster_start + iterator - 1) + " 1)"
                          + " NA (" + node_start + " 1)"
                          + " NE (" + node_end + " 1)"
                          + " NCS " + crosec + "\n";
                }
            }

            // SOFILOAD definitions
            file += "\n\n+PROG SOFILOAD urs:4\nHEAD Loads\nUNIT 5\n\n";
            foreach (Load load in loads) {
                file += load.sofistring() + "\n";
            }

            // Analysis
            file += "\nEND\n\n\n+PROG ASE urs:13\nHEAD Solving\n\nSYST PROB LINE\nLC ALL\n\nEND\n";
        }
    }
}
