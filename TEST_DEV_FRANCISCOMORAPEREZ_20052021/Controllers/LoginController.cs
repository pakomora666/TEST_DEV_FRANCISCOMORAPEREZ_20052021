using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TEST_DEV_FRANCISCOMORAPEREZ_20052021.Data;
using TEST_DEV_FRANCISCOMORAPEREZ_20052021.Models;
namespace TEST_DEV_FRANCISCOMORAPEREZ_20052021.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main()
        {
            ClientCollection clientCollection = new ClientCollection();

           // List<Client> clients = new List<Client>();
            var url = $"https://api.toka.com.mx/candidato/api/customers";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + MyGlobals.token);
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        //if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            clientCollection = JsonConvert.DeserializeObject<ClientCollection>(responseBody);
                            //clients = clientCollection.Data;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            //DataTable dt = ConvertToDataTable(clients);
            return View(clientCollection);

        }


        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Username,Password")] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var url = $"https://api.toka.com.mx/candidato/api/login/authenticate";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = $"{{\"Username\":\"{loginModel.Username}\" , \"Password\":\"{loginModel.Password}\" }}";
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream strReader = response.GetResponseStream())
                        {
                            //if (strReader == null) return;
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                // Do something with responseBody
                                LoginData loginData = JsonConvert.DeserializeObject<LoginData>(responseBody);
                                MyGlobals.token = loginData.Data;
                                System.Diagnostics.Debug.WriteLine(loginData.Data);
                                Console.WriteLine(responseBody);
                                return RedirectToAction(nameof(Main));

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                }
            }
            return View();
        }


    }

}
public static class MyGlobals
{
    public static string token = "";
}