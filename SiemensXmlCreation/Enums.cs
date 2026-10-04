using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadMacro.Simulator.CommonControls.Helpers
{
    /// <summary>
    /// Content types for siemens contour UI
    /// </summary>
    public enum ContentType { TextBox, ComboBox };
    /// <summary>
    /// Value types for the variable for siemens contour UI
    /// </summary>
    public enum ItemSourceType { Value, Image };
    /// <summary>
    /// Custom control type for siemens contour UI
    /// </summary>
    public enum ControlType { Empty, Mono, Dual, Tri, Quad };
}
