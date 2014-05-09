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
        }

        // Registers all the output parameters for this component.
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.Register_StringParam("Output", "Output", "Converted model");
            pManager.Register_StringParam("Status", "Status", "Errors or success messages");
        }

        // This is the method that actually does the work.
        protected override void SolveInstance(IGH_DataAccess DA) {
            //Load the data from Karamba

            //Write the data into a .dat file format
            Parser parser = new Parser();
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
