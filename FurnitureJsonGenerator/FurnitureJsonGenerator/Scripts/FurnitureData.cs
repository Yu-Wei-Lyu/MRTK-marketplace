using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureJsonGenerator
{
    [System.Serializable]
    public class FurnitureData
    {
        public int ID
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public double Price
        {
            get; set;
        }
        public string Size
        {
            get; set;
        }
        public string Tags
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string Material
        {
            get; set;
        }
        public string Manufacturer
        {
            get; set;
        }
        public string ImageURL
        {
            get; set;
        }
        public string ModelURL
        {
            get; set;
        }

    }
}
