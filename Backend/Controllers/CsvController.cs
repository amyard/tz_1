using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                            // update
                            await _trans.UpdateTransactionByIdAsync(item);
                        else
                            await _trans.CreateTransactionAsync(item);
                    }
                }

                System.IO.File.Delete(fullPath);
            }

            return Ok();
        }
    }
}