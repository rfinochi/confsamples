using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FotoGol_Email
{

    public class PhotoData
    {
        public string From
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string ImageType
        {
            get;
            set;
        }

        public byte[] Data
        {
            get;
            set;
        }

        public string FileName { get; set; }
    }
}
