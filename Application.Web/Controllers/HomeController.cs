using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Application.Web.Custom;
using Application.Web.Models;

namespace Application.Web.Controllers
{
    public class HomeController : Custom.JsonController
    {
        private const string BaseUrl = "https://www.dzo.com.ua";
        private const string PathPrefix = "DozTendersExp_";

        public ActionResult Index()
        {
            return View();
        }

        // API methods

        [HttpPost]
        public async Task<ActionResult> Login(LoginData loginData)
        {
            List<FragmentInformation> elementsList;
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("----WebKitFormBoundary2dxruVQQzCywiSYw"))
                {
                    content.Add(new StringContent(loginData.UserName), "email");
                    content.Add(new StringContent(loginData.Password), "psw");
                    content.Add(new StringContent("auth"), "formname");

                    using (var message = await client.PostAsync("https://www.dzo.com.ua/cabinet?", content))
                    {
                        var input = await message.Content.ReadAsStringAsync();

                        elementsList = Parser.CompanyFragmentToObject(input);
                        Response.StatusCode = (int) message.StatusCode;
                    }
                }
            }

            return Json(elementsList);
        }

        [HttpPost]
        public async Task<ActionResult> RetrieveTenders(string[] links)
        {
            using (var client = new HttpClient())
            {
                var taskList = links.Select(link => client.GetStringAsync(BaseUrl + link));
                try
                {
                    var result = await Task.WhenAll(taskList)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    var tenders = Parser.TenderFragmentsToObjects(result);

                    // Manage excel files
                    var genPath = Path.GetTempPath();

                    // Lookup for old files
                    var invalidFileTime = DateTime.Now.AddMinutes(-30);
                    var invalidFiles = new DirectoryInfo(genPath)
                        .GetFiles()
                        .Where(f => f.Name.StartsWith(PathPrefix) &&
                                    f.CreationTime < invalidFileTime)
                        .Select(f => f.FullName)
                        .ToList();

                    // Delete invalid files
                    try
                    {
                        invalidFiles.ForEach(System.IO.File.Delete);
                    }
                    catch
                    {
                        // ignored
                    }

                    // Create temp file for with export data
                    var reportFileName = PathPrefix + Guid.NewGuid().ToString("N") + ".xlsx";
                    var reportPath = genPath + reportFileName;

                    // Export data into file
                    var exp = new SpreadSheetFacade(reportPath);
                    exp.ExportData(tenders);

                    return Content(reportFileName);

                }
                catch (Exception exception)
                {
                    Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    return Content(exception.Message);
                }
            }
        }

        [HttpGet]
        public ActionResult RetrieveTenders(string reportFileName)
        {
            var path = Path.Combine(Path.GetTempPath(), reportFileName);
            if (!System.IO.File.Exists(path))
            {
                throw new HttpException(404, "File not found");
            }
            return new FilePathResult(path, "application/vnd.ms-excel")
            {
                FileDownloadName = PathPrefix + DateTime.Now.ToShortDateString() + ".xlsx"
            };
        }
    }
}