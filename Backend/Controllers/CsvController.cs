using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.Interfaces;
using Backend.Models;
using Backend.RequestModels;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Backend.Controllers
{
    public class CsvController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;
        private readonly ITransactionService _trans;

        public CsvController(IWebHostEnvironment env, ITransactionService trans)
        {
            _env = env;
            _trans = trans;
        }


        [HttpPost("import")]
        public async Task<ActionResult> ImportCsvFile(IFormFile uploadFile)
        {
            IFormFile file = Request.Form.Files[0];

            string folderName = "files";
            string webRootPath = _env.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            };

            if (file.Length > 0)
            {
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                TextReader reader = new StreamReader(fullPath);
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HeaderValidated = null;
                    var records = csv.GetRecords<TransactionMD>();
                    
                    foreach(TransactionMD item in records)
                    {
                        // check if transaction exists
                        if (_trans.CheckTransactionExists(item.TransactionId))
                            await _trans.UpdateTransactionByIdAsync(item);
                        else
                            await _trans.CreateTransactionAsync(item);
                    }
                }

                System.IO.File.Delete(fullPath);
            }

            return Ok();
        }


        [HttpGet("download-csv")]
        public async Task<ActionResult> ImportToCsv([FromBody] TransactionFilterDownloadModel filters)
        {
            var listOfData = await _trans.GetTransactionsWithFiltersDonwloadAsync(filters);
            string headerColumnNames = GetHeaderColumnNames();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(headerColumnNames);

            foreach(TransactionMDDto item in listOfData)
            {
                builder.AppendLine($"{item.TransactionId},{item.Status},{item.Type},{item.ClientName},{item.Amount}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"Transaction_{DateTime.Now.ToString("MM/dd/yyyy_HH:mm")}.csv");
        }


        [HttpGet("download-excel")]
        public async Task<ActionResult> ImportToExcel([FromQuery] TransactionFilterDownloadModel filters)
        {
            var listOfData = await _trans.GetTransactionsWithFiltersDonwloadAsync(filters);

            // save to excel file

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reprt");

            ws.Cells["A1"].Value = "Report";
            ws.Cells["B1"].Value = "Transaction Report";

            ws.Cells["A2"].Value = "Date";
            ws.Cells["B2"].Value = dateNow;

            ws.Cells["A2"].Value = "Amount of Data";
            ws.Cells["B2"].Value = listOfData.Count;


            ws.Cells["A6"].Value = "TransactionId";
            ws.Cells["B6"].Value = "Status";
            ws.Cells["C6"].Value = "Type";
            ws.Cells["D6"].Value = "ClientName";
            ws.Cells["E6"].Value = "Amount";

            int rowStart = 7;
            foreach(TransactionMDDto item in listOfData)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.TransactionId;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Status;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Type;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ClientName;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Amount;

                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            return File(
                    pck.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Transaction_{dateNow}.xlsx"
                );
        }


        #region private methods
        private string GetHeaderColumnNames()
        {
            return string.Join(",", typeof(TransactionMD).GetProperties().Select(f => f.Name).ToList());
        }
        #endregion
    }
}