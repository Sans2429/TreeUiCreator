using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml;

namespace MacroSimulatorTest.Helpers
{
    internal class FileHandler
    {
        public enum ResourceType
        {
            /// <summary>
            /// represents file type
            /// </summary>
            File,
            /// <summary>
            /// represents uri type
            /// </summary>
            PackUri
        }
        private const string _xmlVersion = "1.0";
        private const string _xmlEncoding = "utf-8";
        private const string _xmlStandalone = "yes";
        /// <summary>
        /// Get value from the xml file 
        /// </summary>
        /// <typeparam name="IData"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IData GetValue<IData>(string filePath, ResourceType resourceType = ResourceType.File)
        {
            XDocument doc = GetDocument(filePath, resourceType);
            if (!doc.Root.HasElements) return default;
            IEnumerable<XElement> elements = doc.Root.Elements();
            Type type = typeof(IData);
            ConstructorInfo constructorInfo = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                Type.EmptyTypes,
                null);

            IData data = (IData)constructorInfo.Invoke(null);
            AssignValues(data, type, elements.FirstOrDefault());
            return data;
        }
        private static XDocument GetDocument(string path, ResourceType resourceType = ResourceType.File)
        {
            if (resourceType is ResourceType.File)
            {
                return XDocument.Load(path);
            }
            else if (resourceType is ResourceType.PackUri)
            {
                Uri uri = new Uri(path, UriKind.Absolute);
                var resourceStream = System.Windows.Application.GetResourceStream(uri);
                using (Stream stream = resourceStream.Stream)
                {
                    return XDocument.Load(stream);
                }
            }
            else
                throw new Exception("Invalid resource type");

        }
        /// <summary>
        /// Get collection from the xml
        /// </summary>
        /// <typeparam name="Data"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IEnumerable<Data> GetData<Data>(string filePath, ResourceType resourceType = ResourceType.File)
        {
            XDocument doc = GetDocument(filePath, resourceType);
            if (!doc.Root.HasElements) yield break;
            IEnumerable<XElement> elements = doc.Root.Elements();
            Type type = typeof(Data);
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { });
            foreach (XElement element in elements)
            {
                Data data = (Data)constructorInfo.Invoke(null);
                AssignValues(data, type, element);
                yield return data;
            }
        }
        /// <summary>
        /// Add element in the xml file
        /// </summary>
        /// <typeparam name="Data"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void AddElement<Data>(string filePath, Data data)
        {
            if (!File.Exists(filePath))
                CreateNewFile(filePath);
            XDocument doc = XDocument.Load(filePath);
            XElement element = GetElement(data);
            doc.Root.Add(element);
            doc.Save(filePath);
        }
        /// <summary>
        /// Remove element from the xml file
        /// </summary>
        /// <typeparam name="IData"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        /// <param name="attribute"></param>
        public static void RemoveElement<IData>(string filePath, IData data, string attributeName)
        {
            if (!File.Exists(filePath)) return;
            PropertyInfo propertyInfo = data.GetType().GetProperty(attributeName);
            object value = propertyInfo.GetValue(data);
            XDocument doc = XDocument.Load(filePath);
            doc.Root
               .Elements()
               .Where(e => e.Attribute(attributeName).Value == value.ToString())
               .Remove();

            doc.Save(filePath);
        }
        private static void AssignValues<Data>(Data data, Type type, XElement element)
        {
            if (!element.HasAttributes) return;
            IEnumerable<XAttribute> attributes = element.Attributes();
            foreach (XAttribute attribute in attributes)
            {
                PropertyInfo propertyInfo = type.GetProperty(attribute.Name.LocalName);
                if (propertyInfo is null) continue;
                object value = GetParsedValue(attribute.Value, propertyInfo.PropertyType);
                propertyInfo.SetValue(data, value);
            }
        }
        private static object GetParsedValue(string value, Type type)
        {
            if (type.IsEnum)
                return Enum.Parse(type, value);
            else
                return Convert.ChangeType(value, type);
        }
        private static void CreateNewFile(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            XElement rootElement = new XElement(fileName);
            XDocument doc = new XDocument(new XDeclaration(_xmlVersion, _xmlEncoding, _xmlStandalone));
            doc.Add(rootElement);
            doc.Save(filePath);
        }
        private static XElement GetElement<Data>(Data data)
        {
            string[] elementNames = data.ToString().Split('.');
            XElement element = new XElement(elementNames[elementNames.Length - 1]);
            Type type = data.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string propertyName = propertyInfo.Name;
                object propertyValue = propertyInfo.GetValue(data);
                XAttribute attribute = new XAttribute(propertyName, propertyValue);
                element.Add(attribute);
            }
            return element;
        }
    }
}
