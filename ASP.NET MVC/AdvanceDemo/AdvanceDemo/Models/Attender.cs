using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.MobileControls;
using System.ComponentModel;

namespace CodeCamp2009Demos.Models
{
    public class Attender 
    {   
        public string Name { get; set; }
        public int Age { get; set; }
        public string Profesion { get; set; }
        public int Id { get; set; }
        public bool ReturnNextYear { get; set; }
        public string Comments { get; set; }
        public int[] TalksIds { get; set; }
    }
}