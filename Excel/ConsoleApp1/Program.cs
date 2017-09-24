using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.OleDb;
using System.Data;

namespace ConsoleApp1
{
    public class Product
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string ParentId { get; set; }
        public string ImageData { get; set; }
        public List<string> Keywords { get; set; }
    }

    class Program
    {        
        static void Main(string[] args)
        {
           DataSet s = ReadExcelFile();
        }

        private static string GetConnectionString()
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = @"C:\Users\prasad.pawale\Downloads\Excel\ConsoleApp1\temp3.xls";

            // XLS - Excel 2003 and Older
            //props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            //props["Extended Properties"] = "Excel 8.0";
            //props["Data Source"] = "C:\\MyExcel.xls";

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }
        private static DataSet ReadExcelFile()
        {
            DataSet ds = new DataSet();

            string connectionString = GetConnectionString();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                // Get all Sheets in Excel File
                System.Data.DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // Loop through all Sheets to get data
                foreach (DataRow dr in dtSheet.Rows)
                {
                    string sheetName = dr["TABLE_NAME"].ToString();

                    if (!sheetName.EndsWith("$"))
                        continue;

                    // Get all rows from the Sheet
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.TableName = sheetName;

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    ds.Tables.Add(dt);
                    List<Product> products = new List<Product>();
                    foreach (DataRow row in dt.Rows) {
                        Product p = new Product();
                        p.Name = row["Name"].ToString();
                        p.Category = row["Category"].ToString();
                        p.Description = row["Description"].ToString();
                        string tempCategory = row["Folder"].ToString();
                        p.ImageData = string.Format("http://thesnapshopapp.blob.core.windows.net/products/{0}/{1}", tempCategory , row["ImageData"].ToString())  ;
                        List<string> csvKeywords = row["Keywords"].ToString().ToLower().Split(',').ToList();
                        p.Keywords = csvKeywords;
                        p.Price = row["Price"].ToString();
                        products.Add(p);
                    }
                    PushToMongo(products);
                }

                cmd = null;
                conn.Close();
            }

            return ds;
        }

        private static void PushToMongo(List<Product> products)
        {            
            IMongoDatabase mongoDb = GetMongoDb();
            //foreach(Product product in products)
            mongoDb.GetCollection<Product>("test").InsertMany(products);
        }

        private static IMongoDatabase GetMongoDb()
        {
            MongoClient mongoClient;
            IMongoDatabase mongoDb = null;
            string connectionStringItems = "mongodb://localhost/?safe=true";
            //string connectionStringItems = "mongodb://thesnapshopapp:sz94BSTRMYIoNDhN4rsYCmgUZh55AsR1P9qAcfvow3tZ6OPLEqIAKid9hj2nAG6UJyX6INUqmBZC7zg9nG2wzA==@thesnapshopapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            string database = "SnapShop";
            mongoClient = new MongoClient(new MongoUrlBuilder(connectionStringItems) { MaxConnectionIdleTime = TimeSpan.FromMinutes(1) }.ToMongoUrl());
            if (mongoDb == null || mongoDb.DatabaseNamespace.DatabaseName != database)
                mongoDb = mongoClient.GetDatabase(database);
            return mongoDb;
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        // Reference to Excel Application.
    //        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

    //        Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Path.GetFullPath("temp3.xls"));

    //        // Get the first worksheet.
    //        Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Sheets.get_Item(1);

    //        //PushToMongo(xlWorksheet);
    //        var cell = (Microsoft.Office.Interop.Excel.Range)xlWorksheet.Cells[10, 2];
    //        // Get the range of cells which has data.
    //        Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

    //        // Get an object array of all of the cells in the worksheet with their values.
    //        object[,] valueArray = (object[,])xlRange.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);
    //        //PushToMongo(valueArray);
    //        // Close the Workbook.
    //        xlWorkbook.Close(false);

    //        // Relase COM Object by decrementing the reference count.
    //        Marshal.ReleaseComObject(xlWorkbook);

    //        // Close Excel application.
    //        xlApp.Quit();

    //        // Release COM object.
    //        Marshal.FinalReleaseComObject(xlApp);

    //        Console.ReadLine();

    //    }

    //    private static void PushToMongo(Microsoft.Office.Interop.Excel.Worksheet xlWorksheet)
    //    {
    //        List<Product> products = new List<Product>();
    //        for (int i = 1; i < 8; i++) {
    //            Product product = new Product();
    //            product.Name = xlWorksheet.Cells[i, 1];
    //            product.Description = xlWorksheet.Cells[i, 1];
    //            product.ImageData = "https://thesnapshopapp.blob.core.windows.net/products/Television/"+xlWorksheet.Cells[i, 1];
    //            product.Keywords = xlWorksheet.Cells[i, 1];
    //            product.Price = xlWorksheet.Cells[i, 1];
    //            product.Category = xlWorksheet.Cells[i, 1];
    //        }

    //        IMongoDatabase mongoDb = GetMongoDb();

    //    }

    //    private static IMongoDatabase GetMongoDb() {
    //        MongoClient mongoClient;
    //        IMongoDatabase mongoDb = null;
    //        string connectionStringItems = "mongodb://localhost/?safe=true";
    //        string database = "SnapShop";
    //        mongoClient = new MongoClient(new MongoUrlBuilder(connectionStringItems) { MaxConnectionIdleTime = TimeSpan.FromMinutes(1) }.ToMongoUrl());
    //        if (mongoDb == null || mongoDb.DatabaseNamespace.DatabaseName != database)
    //            mongoDb = mongoClient.GetDatabase(database);
    //        return mongoDb;
    //    }
    //}
}


