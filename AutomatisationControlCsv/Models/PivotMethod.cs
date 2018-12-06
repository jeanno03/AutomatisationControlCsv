using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

using AutomatisationControlCsv.Tools;

namespace AutomatisationControlCsv.Models
{
    public class PivotMethod : IPivotMethod
    {
        static string fileRepository = ConfigurationManager.AppSettings.GetAppConfigValues("fileRepository");

        //simulation de l'API de RSA archer
        public PivotReportClasses GetPivotReport()
        {
            //TextWriterTraceListener tr1 = new TextWriterTraceListener(System.Console.Out);
            //Debug.Listeners.Add(tr1);

            //TextWriterTraceListener tr2 = new TextWriterTraceListener(System.IO.File.CreateText(fileRepository + "logout.txt"));
            //Debug.Listeners.Add(tr2);

            //Debug.WriteLine("Debug Method Starting");

            XmlDocument document = new XmlDocument();
            document.Load(fileRepository+ "PivotReport.xml");

            //To get a string from a xml
            string stringXml = GetXMLAsString(document);

            //Deserialize the string
            PivotReportClasses pivotReportClasses = Deserialize<PivotReportClasses>(stringXml);

            //Debug.WriteLine("Debug Method Ending");

            //Debug.Flush();

            return pivotReportClasses;
        }


        //To get a string from a xml
        private string GetXMLAsString(XmlDocument Input)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            Input.WriteTo(tx);

            string str = sw.ToString();
            return str;
        }

        //Deserialize a string to an object
        private T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}