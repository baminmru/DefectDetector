using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace DD
{
    public class DefectScanner
    {
        List<DefectInfo> Defects { get; set; }

        private string _XmlPath;

        public DefectScanner(string XMLPath)
        {
            _XmlPath = XMLPath;

            Defects = new List<DefectInfo>();
             
            // init rule
            DefectInfo di = new DefectInfo();
            di.Name = "Use prohibit functions";
            di.Usage = RuleUsage.AllRule;

            DefectRule r;
            r = new DefectRule();
            r.XPath = "//src:call/src:name[contains(. ,'sprintf')]";
            r.Sequence = 1;
            r.Info = "use sprintf  fucntion prohibit";
            di.Rules.Add(r);


            r = new DefectRule();
            r.XPath = "//src:call/src:name[contains(. ,'printf')]";
            r.Sequence = 2;
            r.Info = "use printf fucntion prohibit";
            di.Rules.Add(r);


            r = new DefectRule();
            r.XPath = "//src:call/src:name[contains(. ,'scanf')]";
            r.Sequence = 3;
            r.Info = "use scanf fucntion prohibit";
            di.Rules.Add(r);

            Defects.Add(di);

        }


        public bool Detect()
        {
            

            foreach(DefectInfo di  in Defects)
            {
                bool found = false;
                foreach(DefectRule r in di.Rules)
                {

                    
                    string outFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +@"\temp" + r.Sequence.ToString() + ".xml";
                    Process p = new Process();
                    p.StartInfo.FileName = "srcML";
                    p.StartInfo.Arguments = _XmlPath + @" --xpath=""" + r.XPath + @""" -o " + outFileName ;
                    p.Start();
                    p.WaitForExit();
                    p.Dispose();

                    if (File.Exists(outFileName))
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(outFileName);
                        XmlNode xN = xDoc.LastChild;
                        if (xN.ChildNodes.Count > 0)
                        {
                            XmlNodeList xNL = xN.ChildNodes;
                            foreach(XmlNode n in xNL)
                            {
                                XmlElement xe = (XmlElement)n;
                                Console.WriteLine(r.Info +" at " + n.Attributes["filename"].Value + " line:col=" + n.FirstChild.Attributes["pos:start"].Value);
                                
                            }
                            found = true;
                        }


                    }

                }

                if (found) return true;
            }

            

            return false;
        }
    }
}
