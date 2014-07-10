// Karamba To Sofistik component for GrassHopper
// Convert a karamba model to a .dat file readable by Sofistik
// Git: https://github.com/AlbericTrancart/karambaToSofistik
// Contact: alberic.trancart@eleves.enpc.fr

using System;
using System.Collections.Generic;
using System.IO;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Karamba.Models;
using Karamba.Elements;

using karambaToSofistik.Classes;

namespace karambaToSofistik {
    public class karambaToSofistikComponent : GH_Component {
        // Component configuration
        public karambaToSofistikComponent() : base("karambaToSofistik", "ktS", "Converts a Karamba model to a .dat file readable by Sofistik", "Karamba", "Extra") { }

        // Registers all the input parameters for this component.
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddParameter(new Param_Model(), "Model", "Model", "Model to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "Path", "Save the .dat file to this path", GH_ParamAccess.item, @"");
        }

        // Registers all the output parameters for this component.
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.Register_StringParam("Output", "Output", "Converted model");
            pManager.Register_StringParam("Status", "Status", "Errors or success messages");
        }

        // We need to register all groups defined in Grasshopper
        static public List<string> beam_groups = new List<string>();

        // This is the method that actually does the work.
        protected override void SolveInstance(IGH_DataAccess DA) {
            // Some variables
            string output = "";                        // The file output
            string status = "Starting component...\n"; // The debug output

            // Several arrays where the data is stored
            List<Material> materials = new List<Material>();
            List<CrossSection> crossSections = new List<CrossSection>();
            List<Node> nodes = new List<Node>();
            List<Beam> beams = new List<Beam>();
            List<Load> loads = new List<Load>();

            // We need to reset some variables because the objects are not freed until Grasshopper is unloaded
            Parser.id_count = 1;

            try {
                // Load the data from Karamba

                // Retrieve and clone the input model
                GH_Model in_gh_model = null;
                if (!DA.GetData<GH_Model>(0, ref in_gh_model)) return;
                Model model = in_gh_model.Value;
                model = (Karamba.Models.Model) model.Clone(); // If the model is not cloned a modification to this variable will imply modification of the input model, thus modifying behavior in other components.

                if (model == null) {
                    status += "ERROR: The input model is null.";
                    output = "Nothing to convert";
                }
                else {
                    string path = null;
                    if (!DA.GetData<string>(1, ref path)) { path = ""; }
                    if (path == "") {
                        status += "No file path specified. Will not save data to a .dat file.\n";
                    }



                    // Retrieve and store the data

                    // Materials
                    foreach (Karamba.Materials.FemMaterial material in model.materials) {
                        // The first material seems to be wong but I don't know why it exists
                        if(model.materials.IndexOf(material) != 0)
                            materials.Add(new Material(material));
                    }

                    /*Disabled for forward compatibility
                    // Check for material duplicates
                    // This is necessary because karamba uses a preset material that is added every time that a model is assembled
                    // As a consequence a model can get a great amount of redundant materials that will flood the output 

                    // Furthermore karamba seems to create a buggy material at index 0 during the cloning operation
                    materials.RemoveAt(0);
                    // Using a for loop because a collection used in foreach is immutable
                    for (int i = 0; i < materials.Count; i++) {
                        materials.RemoveAll(delegate(Material test_material) {
                            return test_material.id != materials[i].id && materials[i].duplicate(test_material);
                        });
                    }
                    */
                    status += materials.Count + " materials loaded...\n";



                    // Cross sections
                    foreach (Karamba.CrossSections.CroSec crosec in model.crosecs) {
                        crossSections.Add(new CrossSection(crosec));
                    }
                    /*Disabled for forward compatibility
                    // The same happens with Cross Sections
                    crossSections.RemoveAt(0);
                    for (int i = 0; i < crossSections.Count; i++) {
                        crossSections.RemoveAll(delegate(CrossSection test_crosec) {
                            return test_crosec.id != crossSections[i].id && crossSections[i].duplicate(test_crosec);
                        });
                    }
                    status += crossSections.Count + " cross sections loaded...\n";
                    */


                    // Nodes
                    foreach (Karamba.Nodes.Node node in model.nodes) {
                        nodes.Add(new Node(node));
                    }
                    status += nodes.Count + " nodes loaded...\n";

                    foreach (Karamba.Supports.Support support in model.supports) {
                        nodes[support.node_ind].addConstraint(support);
                    }
                    status += "Support constraints added to " + model.supports.Count + " nodes.\n";



                    // Beams
                    foreach (Karamba.Elements.ModelElement beam in model.elems) {
                        Beam curBeam = new Beam(beam);

                        // Adding the start and end nodes
                        curBeam.start = nodes[curBeam.ids[0]];
                        curBeam.end = nodes[curBeam.ids[1]];
                        beams.Add(curBeam);
                    }
                    status += beams.Count + " beams loaded...\n";



                    // Loads
                    foreach (KeyValuePair<int, Karamba.Loads.GravityLoad> load in model.gravities) {
                        loads.Add(new Load(load));
                    }
                    status += model.gravities.Count + " gravity loads added.\n";
                    
                    foreach (Karamba.Loads.PointLoad load in model.ploads) {
                        Load current = new Load(load);
                        current.node = nodes[load.node_ind];
                        loads.Add(current);
                    }
                    status += model.ploads.Count + " point loads added.\n";
                    
                    foreach (Karamba.Loads.ElementLoad load in model.eloads) {
                        // Create a load variable base on the load type
                        Load current = new Load();

                        Karamba.Loads.UniformlyDistLoad line = load as Karamba.Loads.UniformlyDistLoad;
                        Karamba.Loads.PreTensionLoad pret = load as Karamba.Loads.PreTensionLoad;
                        Karamba.Loads.TemperatureLoad temp = load as Karamba.Loads.TemperatureLoad;
                                
                        if (line != null) {
                            current = new Load(line);
                        }
                        // Very important to check Temperature BEFORE Pretension becaus Temperature derivates from Pretension
                        else if (temp != null) {
                            current = new Load(temp);
                        }
                        else if (pret != null) {
                            current = new Load(pret);
                        }
                        

                        // If there is not target element, apply the load to the whole structure
                        if (load.beamId == "") {
                            current.beam_id = "";
                            loads.Add(current);
                        }
                        else {
                            // We search the element
                            current.beam = beams.Find(delegate(Beam beam) {
                                return beam.user_id == load.beamId;
                            });
                            loads.Add(current);
                        }
                    }

                    status += model.eloads.Count + " line loads added.\n";

                    // ID matching
                    // Karamba and Sofistik use different ID systems
                    // Karamba's materials and cross sections are pointing to an element ID
                    // Sofistik's elements need a cross section ID which needs a material ID
                    
                    foreach (Material material in materials){
                        // If the IDs list is empty, it means that we want to apply the material to the whole structure (whichi is the default behavior: the default material is set by the constructors of all elements)
                        bool test = false;
                        foreach (string id in material.ids) {
                            if(id != "")
                                test = true;
                        }
                        if (test) {
                            foreach (CrossSection crosec in crossSections) {
                                if(material.ids.Contains((crosec.id - 1).ToString()))
                                    crosec.material = material;
                            }
                        }
                    }
                    status += "Matching with material IDs...\n";
                    
                    foreach (CrossSection crosec in crossSections) {
                        // If the IDs list is empty, it means that we want to apply the cross section to the whole structure (which is the default behavior: the default cross section is set by the constructors of all elements)
                        bool test = false;
                        foreach (string id in crosec.ids) {
                            if (id != "")
                                test = true;
                        }
                        if (test) {
                            foreach (Beam beam in beams) {
                                if (crosec.ids.Contains((beam.id - 1).ToString()))
                                    beam.sec = crosec;
                            }
                        }
                    }
                    status += "Matching with cross section IDs...\n";

                    // Write the data into a .dat file format
                    Parser parser = new Parser(materials, crossSections, nodes, beams, loads);
                    output = parser.file;

                    if (path != "") {
                        status += "Saving file to " + path + "\n";
                        System.IO.File.WriteAllText(@path, output);
                        status += "File saved!\n";
                    }
                }
            }
            catch (Exception e) {
                status += "\nERROR!\n" + e.ToString() + "\n" + e.Data;
            }

            // Return data
            DA.SetData(0, output);
            DA.SetData(1, status);
        }

        // Icon
        protected override System.Drawing.Bitmap Icon {
            get { return Resource.Icon; }
        }

        // Each component must have a unique Guid to identify it. 
        public override Guid ComponentGuid {
            get { return new Guid("{1954a147-f7a2-4d9c-b150-b788821ccae7}"); }
        }
    }
}
