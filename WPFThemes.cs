using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCore
{
    class Theme
    {
        private string color1value;
        private string color2value;
        private string color3value;


        public string Color1
        {
            get { return color1value; }
            set { color1value = value; }
        }

        public string Color2
        {
            get { return color2value; }
            set { color2value = value; }
        }

        public string Color3
        {
            get { return color3value; }
            set { color3value = value; }
        }
    }
}

