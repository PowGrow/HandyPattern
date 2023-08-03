using HandyPattern.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace HandyPattern
{
    public static class Data
    {
        public const string TEMP_SAVE_PATH = "Data/ContentView.json";

        public static void SerializeAndSaveCollection(List<IElement> data)
        {
            try
            {
                var indented = Newtonsoft.Json.Formatting.Indented;
                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All
                };
                string serialized = JsonConvert.SerializeObject(data, indented, settings);
                File.WriteAllText(TEMP_SAVE_PATH, serialized);
            }
            catch
            {
                MessageBox.Show("Unhandled excpetion", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static List<IElement> DeserializeAndLoadCollection()
        {
            if (File.Exists(TEMP_SAVE_PATH))
            {
                var data = new List<IElement>();
                try
                {
                    var settings = new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.All
                    };
                    string jsonString = File.ReadAllText(TEMP_SAVE_PATH);
                    data = JsonConvert.DeserializeObject<List<IElement>>(jsonString,settings);
                }
                catch (JsonException)
                {
                    MessageBox.Show($"There is no data to load", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return data;
            }
            return null;
        }


        public static List<IElement> CreateSerializeCollection(UIElementCollection contentViewChildren)
        {
            var elementData = new List<IElement>();
            int index = 0;
            foreach (IElement element in contentViewChildren)
            {
                if(element.IsFolder)
                    elementData.Add((PatternFolder)element);
                else
                    elementData.Add((Pattern)element);
                index++;
            }
            return elementData;
        }

        public static FlowDocument ConvertXmalString(string flowDocumentXAML)
        {
            StringReader stringReader = new StringReader(flowDocumentXAML);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            return (FlowDocument)XamlReader.Load(xmlTextReader);
        }

        public static string ConvertFlowDocument(FlowDocument flowDocument)
        {
            return XamlWriter.Save(flowDocument);
        }
    }
}
