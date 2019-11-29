using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PMSAutoImport.Utils
{
    public class ArrivalsByUnitParse : ParseBase
    {

        private DataTable GetDataTable(string path)
        {
            Workbook workbook = new Workbook();
            workbook.Open(path);
            Cells cells = workbook.Worksheets[0].Cells;
            DataTable dt = new DataTable();
            bool d = true;//防止表头重复加载
            for (int i = 0; i < cells.MaxDataRow + 1; i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                {
                    if (d)
                    {
                        dt.Columns.Add(cells[0, j].StringValue.Trim());
                    }

                    row[j] = cells[i + 1, j].StringValue.Trim();
                }
                dt.Rows.Add(row);
                d = false;
            }
            return dt;
        }

        public override void parse()
        {

           var fileName = "template/ArrivalsByUnit_.xls";
           DataTable dt =   GetDataTable(fileName);
           
            foreach (DataRow row in dt.Rows) {
                var propertyId = row["Unit"].ToString ();
                var Arrive = row["Arrive"].ToString();
                var Depart = row["Depart"].ToString();
                var Reservation = row["FolioID"].ToString();
                var Guest = row["Guest"].ToString();

            }

        }
    }
}
