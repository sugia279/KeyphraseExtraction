using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace KeyphraseExtraction.Model
{
    public class DocumentExtractionXML
    {
        private static DocumentExtractionXML _instance;

        public static DocumentExtractionXML Instance()
        {
            if (_instance == null)
            {
                _instance = new DocumentExtractionXML();
            }
            return _instance;
        }

        private XmlDocument _document;
        private XmlSchema schema = new XmlSchema();
        private XmlElement _rootDocument, _metadata, _structure, _keyphrases;
        private List<XmlElement> _dublinCoreItems;
        private List<string> _dcNames = new List<string>(){ "Title", "FileName", "Creator", "Publisher",
                                                            "Contributor", "PublishedDate", "Language", "Format",
                                                            "Relation", "Coverage", "Rights", "Identifier",
                                                            "Source", "Description"};
        private XmlElement _section,_keyphrase;
        private XmlAttribute _beginRow, _endRow;
        public DocumentExtractionXML()
        {
            _document = new XmlDocument();
            //schema.Namespaces.Add("xmlns", "http://example.org/myapp/");
            //schema.Namespaces.Add(schema.Items[0] + "xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //schema.Namespaces.Add("xsi:schemaLocation", "http://example.org/myapp/ http://example.org/myapp/schema.xsd");
            //schema.Namespaces.Add("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            //schema.Namespaces.Add("xmlns:dcterms", "http://purl.org/dc/terms/");
            //_document.Schemas.Add(schema);
            _rootDocument = _document.CreateElement("document");
            _document.AppendChild(_rootDocument);

            _metadata = _document.CreateElement("metadata");
            _metadata.SetAttribute("xmlns", "http://example.org/myapp/");
            _metadata.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            _metadata.SetAttribute("xsi:schemaLocation", "http://example.org/myapp/ http://example.org/myapp/schema.xsd");
            _metadata.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            _metadata.SetAttribute("xmlns:dcterms", "http://purl.org/dc/terms/");
            _document.DocumentElement.AppendChild(_metadata);

            _dublinCoreItems = new List<XmlElement>();
            foreach (string elementName in _dcNames)
            {
                _dublinCoreItems.Add(_document.CreateElement("dc:" + elementName));
                _metadata.AppendChild(_dublinCoreItems.Last());
            }

            _structure = _document.CreateElement("structure");
            _document.DocumentElement.AppendChild(_structure);

            _keyphrases = _document.CreateElement("Keyphrases");
            _document.DocumentElement.AppendChild(_keyphrases);
        }

        public void SetDocument(Document doc)
        {
            foreach (var prop in doc.GetType().GetProperties())
            {
                if (_dcNames.Contains(prop.Name))
                {
                    _dublinCoreItems.FirstOrDefault(x => x.Name.Contains(prop.Name)).InnerText = prop.GetValue(doc, null).ToString();
                }
                else if (prop.Name == "DocumentStructure")
                {
                    _structure.RemoveAll();
                    foreach (DocumentItem docI in doc.DocumentStructure.DocumentItems)
                    {
                        XmlElement section = _document.CreateElement("section");
                        _structure.AppendChild(section);
                        section.SetAttribute("Name", docI.ItemName);
                        section.SetAttribute("BeginRow", docI.BeginRow.ToString());
                        section.SetAttribute("EndRow", docI.EndRow.ToString());
                        section.SetAttribute("Weight", docI.Weight.ToString());
                    }
                }
                else if (prop.Name == "AssignedCandidateTerms")
                {
                    _keyphrases.RemoveAll();
                    foreach (CandidateTerm canTerm in doc.AssignedCandidateTerms)
                    {
                        foreach (string term in canTerm.Terms.Split('\n'))
                        {
                            XmlElement keyphrase = _document.CreateElement("keyphrase");
                            keyphrase.InnerText = term;
                            _keyphrases.AppendChild(keyphrase);
                        }
                    }
                    
                }
            }
        }
        public void SaveToXML(string xmlFilePath)
        {
            _document.Save(xmlFilePath);
        }

        public void LoadXML(string filePath, Document doc)
        {
            
            string xmlPath = Path.Combine(Path.GetDirectoryName(filePath),Path.GetFileNameWithoutExtension(filePath)+".xml");
            if (File.Exists(xmlPath))
            {
                XmlDocument docXml = new XmlDocument();
                docXml.Load(xmlPath);
                
                foreach (XmlElement e1 in docXml.FirstChild)
                {
                    if (e1.Name.ToLower() == "metadata")
                    {
                        FillDocumentMetadataByXML(doc, e1);
                    }
                    else if (e1.Name.ToLower() == "structure")
                    {
                        FillDocumentStructureByXML(doc, e1);
                    }
                    else if (e1.Name.ToLower() == "keyphrases")
                    {
                        FillKeyphrasesByXML(doc, e1);
                    }
                }
                
            }
        }

        private void FillKeyphrasesByXML(Document doc, XmlElement e1)
        {   
            foreach (XmlElement e2 in e1)
            {
                if (!doc.AssignedKeyphrase.Contains(e2.InnerText))
                    doc.AssignedKeyphrase.Add(new PorterStemmer().stemTerm(e2.InnerText).ToLower());
            }
        }

        private void FillDocumentStructureByXML(Document doc, XmlElement e1)
        {
            foreach (XmlElement e2 in e1)
            {
                foreach (DocumentItem docI in doc.DocumentStructure.DocumentItems)
                {
                    docI.BeginRow = docI.ItemName == e2.GetAttribute("Name")?int.Parse(e2.GetAttribute("BeginRow")):docI.BeginRow;
                    docI.EndRow = docI.ItemName == e2.GetAttribute("Name") ? int.Parse(e2.GetAttribute("EndRow")) : docI.EndRow;
                    docI.Weight = docI.ItemName == e2.GetAttribute("Name") ? double.Parse(e2.GetAttribute("Weight")) : docI.Weight;
                }
            }
        }

        private void FillDocumentMetadataByXML(Document doc, XmlElement e1)
        {
            foreach (XmlElement e2 in e1)
            {
                doc.Title = e2.Name == "Title" ? e2.InnerText : doc.Title;
                doc.Creator = e2.Name == "Creator" ? e2.InnerText : doc.Creator;
                doc.Publisher = e2.Name == "Publisher" ? e2.InnerText : doc.Publisher;
                doc.Contributor = e2.Name == "Contributor" ? e2.InnerText : doc.Contributor;
                doc.PublishedDate = e2.Name == "PublishedDate" ? e2.InnerText : doc.PublishedDate;
                doc.Identifier = e2.Name == "Identifier" ? e2.InnerText : doc.Identifier;
                doc.Source = e2.Name == "Source" ? e2.InnerText : doc.Source;
                doc.Language = e2.Name == "Language" ? e2.InnerText : doc.Language;
                doc.Relation = e2.Name == "Relation" ? e2.InnerText : doc.Relation;
                doc.Coverage = e2.Name == "Coverage" ? e2.InnerText : doc.Coverage;
                doc.Description = e2.Name == "Description" ? e2.InnerText : doc.Description;                
            }
        }
    }
}
