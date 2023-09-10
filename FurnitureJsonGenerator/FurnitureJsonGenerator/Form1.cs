using Newtonsoft.Json;
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

        // Reset json button click event
        private void ResetJsonButton_Click(object sender, EventArgs e)
        {
            ResultTextBox.Text = "";
        }

        // Reset fields button click event
        private void ResetFieldsButton_Click(object sender, EventArgs e)
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
        }

        // Generate Json button click event
        private void GenerateJsonButton_Click(object sender, EventArgs e)
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
            DebugLabel.ForeColor = Color.Black;
            DebugLabel.Text = "Info: No error happened.";
        }

        // Generate fields button click event
        private void GenerateFieldsButton_Click(object sender, EventArgs e)
        {
            try
            {
                FurnitureData furnitureData = JsonConvert.DeserializeObject<FurnitureData>(ResultTextBox.Text);
                IDTextBox.Text = furnitureData.ID.ToString();
                NameTextBox.Text = furnitureData.Name;
                PriceTextBox.Text = furnitureData.Price.ToString();
                SizeTextBox.Text = furnitureData.Size;
                TagsTextBox.Text = furnitureData.Tags;
                DescriptionTextBox.Text = furnitureData.Description;
                MaterialTextBox.Text = furnitureData.Material;
                ManufacturerTextBox.Text = furnitureData.Manufacturer;
                ImageURLTextBox.Text = furnitureData.ImageURL.Replace(URLPrefixTextBox.Text, "");
                ModelURLTextBox.Text = furnitureData.ModelURL.Replace(URLPrefixTextBox.Text, "");
                DebugLabel.ForeColor = Color.Black;
                DebugLabel.Text = "Info: No error happened.";
            } catch
            {
                DebugLabel.ForeColor = Color.Red;
                DebugLabel.Text = $"ERROR: Json data invalid.";
            }
        }
    }
}
