using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace PharmacyApp.Helper
{
    public static class XmlHelper
    {
        /// <summary>
        /// Translates xml string parameters into an object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="xmlParameters">Xml string parameters</param>
        /// <returns></returns>
        public static T ToObject<T>(this string xmlParameters) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(xmlParameters)) return null;
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(new StringReader(xmlParameters));
        }

        /// <summary>
        /// Translates object to xml string parameters
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="setting">Setting object</param>
        /// <returns>Xml string parameters</returns>
        public static string ToXmlString<T>(this T setting) where T : class, new()
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            string parameters = string.Empty;
            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, setting);
                writer.Flush();
                parameters = writer.ToString();
            }
            return parameters;
        }

    }
}