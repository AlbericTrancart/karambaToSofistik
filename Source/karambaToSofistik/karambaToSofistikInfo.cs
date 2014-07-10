using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace karambaToSofistik {
    // Component info
    public class karambaToSofistikInfo : GH_AssemblyInfo {
        public override string Name { get { return "karambaToSofistik"; } }
        public override Bitmap Icon { get { return Resource.Icon; } }
        public override string Description { get { return "Convert a model from Karamba to a .dat file readable by Sofistik"; } }
        public override Guid Id { get { return new Guid("c39f7c34-8a3f-4035-8673-c35a21ff4266"); } }
        public override string AuthorName { get { return "Albéric Trancart (ENPC)"; } }
        public override string AuthorContact { get { return "alberic.trancart@eleves.enpc.fr"; } }
    }
}
