using System;
using System.Collections.Generic;
using System.Text;

namespace DD
{

    public enum RuleUsage
    {
        AnyRule,
        AllRule
    }

    public class DefectInfo
    {
        public string Name { get; set; }
        public RuleUsage Usage { get; set; }

        public List<DefectRule> Rules { get; set; }

        public  DefectInfo()
        {
            Rules = new List<DefectRule>();
        }

    }
}
