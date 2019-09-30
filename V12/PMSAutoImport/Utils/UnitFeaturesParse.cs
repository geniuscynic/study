using Aspose.Cells;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PMSAutoImport.Utils
{
    public class UnitFeaturesParse:ParseBase
    {
        private HtmlAgilityPack.HtmlDocument htmlDoc;
        private List<NameValue> fields = new List<NameValue>();
        private string unitId = "";
        private string unitName = "";
        private string importFolder = "";
        private string fileName = "";
        public UnitFeaturesParse(string unitId, string unitName, string response, string importFolder)
        {
            htmlDoc = new HtmlAgilityPack.HtmlDocument();

            //htmlDoc.Load("template/response.txt");
            htmlDoc.LoadHtml(response);

            this.unitId = unitId;
            this.unitName = unitName;

            this.importFolder = importFolder;
            this.fileName = importFolder + string.Format("{0}_{1}.txt", unitName, Guid.NewGuid().ToString());
        }
        public override void parse()
        {             
            //bedroom, King, Queen, Double and Twin
            var bedroom = getFieldValue("Bedrooms");
            bedroom = trimAndGetDefaultValue(bedroom, "Bedroom");

            var king = bedroom;
            var queen = "0";
            var doubleRoom = "0";
            var twin = "0";

            addToList("king", king);
            addToList("queen", queen);
            addToList("doubleRoom", doubleRoom);
            addToList("twin", twin);

            //Occupancy,bathTowel ,washCloth
            //var occupancy = getFieldValue("Occupancy");
            //occupancy = getDefalutValue(occupancy);

            //var bathTowel = "0";
            //var washCloth = "0";
            //if (occupancy != "")
            //{
            //    try
            //    {
            //        bathTowel = (int.Parse(occupancy) * 2).ToString();
            //        washCloth = bathTowel;
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //addToList("bathTowel", bathTowel);
            //addToList("washCloth", washCloth);

            //bathRooms
            var bathRooms = getFieldValue("Bathrooms");
            bathRooms = trimAndGetDefaultValue(bathRooms, "Bathroom");
            addToList("bathRoom", bathRooms);

            //Half Baths 
            var halfBaths = getFieldValue("Half Baths");
            halfBaths = trimAndGetDefaultValue(halfBaths, "Half Bath");
            addToList("halfBath", halfBaths);

            
            //Hand towel 
            //var handTowel = "";
            //try
            //{
            //    handTowel = (double.Parse(bathRooms) + double.Parse(halfBaths)).ToString();
            //}
            //catch (Exception ex)
            //{

            //}
            //addToList("handTowel", handTowel);

            //Bath Mat 
            //var bathMat = bathRooms;
            //addToList("bathMat", bathMat);

            //Kitchen 
            //var kitchen = "1";
            //addToList("kitchen", kitchen);

            //is line
            var type = "";
            var linens = getFieldValue("Linens");
            var linens_Towels = getFieldValue("Linens/Towels");
            if (linens == "Yes")
            {
                type = "Linens";
            }
            else if (linens_Towels == "Yes")
            {
                type = "Linens_Towels";
            }
            addToList("type", type);

            var jsonObject = new
            {
                unitId = unitId,
                unitName = unitName,
                @fields = fields
               
            };

            string json = JsonConvert.SerializeObject(jsonObject);

            
            File.WriteAllText(fileName, json);
        }


        public string GetFileName()
        {
            return fileName;
        }

        class NameValue
        {

            public string name = "";
            public string value = "";

            public NameValue(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }

        private void addToList(string name, string value)
        {
            fields.Add(new NameValue(name, value));
        }

        #region tool helper function
        private string getFieldValue(string nodeName)
        {
            string xpath = buildPath(nodeName, "ctl00_MainBody_UnitAttr1_DataGridUnit");
            return getSelectValue(xpath);
        }
        private string getFieldValue(string nodeName, string tableId)
        {
            string xpath = buildPath(nodeName, tableId);
            return getSelectValue(xpath);
        }

        private string buildPath(string nodeName, string tableId)
        {
            var path = "//table[@id='{0}']//span[text()='{1}']/ancestor::td[1]//select/option[@selected='selected']";
            path = string.Format(path, tableId, nodeName);

            return path;

        }


        private string getSelectValue(string xpath)
        {
            return htmlDoc.DocumentNode.SelectSingleNode(xpath).NextSibling.InnerText.Trim();
        }

        private string trimValue(string value, string trimValue)
        {
            return value.Replace(trimValue, "").Trim();
        }

        private string getDefalutValue(string value)
        {
            if (value == "")
            {
                value = "0";
            }

            return value;
        }
        private string trimAndGetDefaultValue(string value, string trimValue)
        {
            value = value.Replace(trimValue + "s", "").Trim();
            value = value.Replace(trimValue, "").Trim();


            return getDefalutValue(value);
        }
        #endregion
    }
}
