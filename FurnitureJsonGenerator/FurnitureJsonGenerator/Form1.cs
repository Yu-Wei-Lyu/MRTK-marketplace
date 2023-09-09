using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FurnitureJsonGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            URLPrefixTextBox.Text = "http://26.122.221.31:8765/";
        }

        // Reset button click event
        private void ResetButton_Click(object sender, EventArgs e)
        {
            IDTextBox.Text = "";
            NameTextBox.Text = "";
            PriceTextBox.Text = "";
            SizeTextBox.Text = "";
            TagsTextBox.Text = "";
            DescriptionTextBox.Text = "";
            MaterialTextBox.Text = "";
            ManufacturerTextBox.Text = "";
            ImageURLTextBox.Text = "";
            ModelURLTextBox.Text = "";
            ResultTextBox.Text = "";
        }

        // Generate button click event
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string id = IDTextBox.Text;
            string name = NameTextBox.Text;
            string price = PriceTextBox.Text;
            string size = SizeTextBox.Text;
            string tags = TagsTextBox.Text;
            string description = DescriptionTextBox.Text;
            string material = MaterialTextBox.Text;
            string manufacturer = ManufacturerTextBox.Text;
            string imageURL = URLPrefixTextBox.Text + ImageURLTextBox.Text;
            string modelURL = URLPrefixTextBox.Text + ModelURLTextBox.Text;
            string result = $"{{\r\n\t\"ID\": {id},\r\n\t\"Name\": \"{name}\",\r\n\t\"Price\": {price},\r\n\t\"Size\": \"{size}\",\r\n\t\"Tags\": \"{tags}\",\r\n\t\"Description\": \"{description}\",\r\n\t\"Material\": \"{material}\",\r\n\t\"Manufacturer\": \"{manufacturer}\",\r\n\t\"ImageURL\": \"{imageURL}\",\r\n\t\"ModelURL\": \"{modelURL}\"\r\n}}";
            ResultTextBox.Text = result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
