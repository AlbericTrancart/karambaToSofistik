//GrassHopper To Sofistik component for GrassHopper
//Convert a karamba model to a .dat file readable by Sofistik
//Contact: Albéric Trancart: alberic.trancart@eleves.enpc.fr

using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Karamba.Models;
using Karamba.Elements;

using GhToSofistik.Classes;

namespace GhToSofistik {
    public class GhToSofistikComponent : GH_Component {
        // Component configuration
        public GhToSofistikComponent() : base("GhToSofistik", "GtS", "Convert Karamba model to a .dat file readable by Sofistik", "Karamba", "Extra") {}

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

        // This is the method that actually does the work.
        protected override void SolveInstance(IGH_DataAccess DA) {
            ///* Some variables *\\\
            string output = "";
            string status = "Starting component...";
            List<Material> materials = new List<Material>();
            List<CrossSection> crossSections = new List<CrossSection>();
            List<Node> nodes = new List<Node>();
            List<Beam> beams = new List<Beam>();
            List<Load> loads = new List<Load>();

            try {
                ///* Load the data from Karamba *\\\

                // Retrieve and clone the input model
                GH_Model in_gh_model = null;
                if (!DA.GetData<GH_Model>(0, ref in_gh_model)) return;
                Model model = in_gh_model.Value;
                model = (Karamba.Models.Model) model.Clone();
                
                string path = null;
                if (!DA.GetData<string>(1, ref path)) { path = ""; }
                if(path == "")
                    status += "\nNo file path specified. Will not save data to a .dat file.";



                ///* Retrieve and store the data *\\\
                foreach(Karamba.Materials.FemMaterial material in model.materials) {
                    materials.Add(new Material(material));
                }

                foreach (Karamba.CrossSections.CroSec crosec in model.crosecs) {
                    crossSections.Add(new CrossSection(crosec));
                }

                foreach (Karamba.Nodes.Node node in model.nodes) {
                    nodes.Add(new Node(node));
                }

                foreach (Karamba.Utilities.BeamSet beam in model.beamsets) {
                    beams.Add(new Beam(beam));
                }
                

                ///* Write the data into a .dat file format *\\\
                Parser parser = new Parser(materials, crossSections, nodes, beams, loads);
                output = parser.file;
            }
            catch (Exception e) {
                status += "\nERROR!\n" + e.Message + "\n" + e.StackTrace + "\n" + e.Source;
            }

            ///* Return data *\\\
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
