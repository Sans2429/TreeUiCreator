using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SiemensXmlCreation.helpers
{
    internal static class Utils
    {
        public static string GetParentImageNames(string name, MacroTreeNode node)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(name);

            stringBuilder = GetRecursiveName(stringBuilder, node as MacroTreeNode);
            string parentNodeNames = stringBuilder.ToString();
            if (parentNodeNames.StartsWith("_"))
                return string.Empty;
            return parentNodeNames;
        }
        private static StringBuilder GetRecursiveName(StringBuilder stringBuilder, MacroTreeNode node)
        {
            if (node is null || node.Parent is null)
                return stringBuilder;

            stringBuilder.Insert(0, '_');
            stringBuilder.Insert(0, node.CurrentValue.SelectedIndex);
            stringBuilder.Insert(0, node.CurrentValue.Name);
            return GetRecursiveName(stringBuilder, node.Parent as MacroTreeNode);
        }
    }
}
