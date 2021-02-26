using APIWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;

namespace APIWeb.Controllers
{
    public class WebAPIController : ApiController
    {
        string server = "https://app.asana.com";
        string ptoken = "1/1125679134753613:8d05fc089b3b6b2ca7934dc7ed39196b";
        public WebAPIController()
        {

        }
        //Sample
        //[HttpGet]
        //public string Sample(int id)
        //{
        //  return "sample";
        //}
        [HttpGet]
        public Response GetSample()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            Response resp = new Response();
            String res = null;
            String req = null;
            String endpoint = server + "/api/1.0/projects";
            String info = new JavaScriptSerializer().Serialize(req);
            var uri = new Uri(endpoint);
            res = SendRequest(info, uri);

            dynamic ddata = new JavaScriptSerializer().Deserialize(res, typeof(Response));
            resp = ddata;

            //for (int i = 0; i < resp.country.Count; i++)
            //{
            //    list.Add(new CountryList
             //   {
               //     isoCode = resp.country[i].isoCode.ToString(),
                 //   name = resp.country[i].name.ToString()

                //});
            //}
            return new Response { data = resp.data};
        }
        [HttpGet]
        public string GetAllUsers()
        {
            var req = WebRequest.Create("https://app.asana.com/api/1.0/users");
            SetBasicAuthentication(req);
            return new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
        }
        [HttpGet]
        public string GetUsersByID(string gid)
        {
            var req = WebRequest.Create("https://app.asana.com/api/1.0/users/" + gid);
            SetBasicAuthentication(req);
            return new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
        }
        [HttpPost]
        public string CreateTask(RequestParam param)
        {
            string json = null;
            byte[] bytes = null;
            string url = "https://app.asana.com/api/1.0/tasks";
            HttpWebRequest req = default(HttpWebRequest);
            Stream reqStream = default(Stream);
            string authInfo = null;

            try
            {
                authInfo = ptoken + Convert.ToString(":");

                param.data.workspace = "";
                param.data.name = "";
                param.data.notes = "";

                json = JsonConvert.SerializeObject(param);
                json = json.Insert((json.Length - 1), ",\"projects\":[" + "" + "]");
                json = Convert.ToString("{ \"data\":") + json + "}";

                bytes = Encoding.UTF8.GetBytes(json);

                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "application/json";
                req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(authInfo)));
                req.ContentLength = bytes.Length;

                reqStream = req.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Close();


                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                string res = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return res;

            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                string resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return resp;
            }
        }

        [HttpGet]
        public string GetTasks(string gid)
        {
            var req = WebRequest.Create("https://app.asana.com/api/1.0/projects/" + gid + "/tasks?opt_fields=%27name,due_on,completed,memberships.section.name");
            SetBasicAuthentication(req);

            var result = JsonConvert.DeserializeObject(new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd());
            return result.ToString();
        }
        [HttpGet]
        public String GetProjectByID(string gid)
        {
            var paramuri = "https://app.asana.com/api/1.0/projects/" + gid;
            var req = WebRequest.Create(paramuri);
            SetBasicAuthentication(req);
            return new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
        }
        [HttpGet]
        public string GetTasksNyID(string gid)
        {
            var paramuri = "https://app.asana.com/api/1.0/tasks/" + gid;
            var req = WebRequest.Create(paramuri);
            SetBasicAuthentication(req);
            return new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
        }

        void SetBasicAuthentication(WebRequest req)
        {
            var authInfo = ptoken + ":";
            var encodedAuthInfo = Convert.ToBase64String(
                Encoding.Default.GetBytes(authInfo));
            req.Headers.Add("Authorization", "Basic " + encodedAuthInfo);
        }
        private String SendRequest(String info, Uri uri)
        {
            try
            {
                String res = null;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;
                HttpWebRequest web = (HttpWebRequest)WebRequest.Create(uri);
                web.ContentType = "application/json";
                web.Method = "POST";
                web.Headers.Add("Authorization: Bearer" + ptoken);
                web.ContentLength = info.Length;
                StreamWriter requestWriter = new StreamWriter(web.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(info);
                requestWriter.Close();
                WebResponse webresponse = web.GetResponse();
                Stream response = webresponse.GetResponseStream();
                res = new StreamReader(response).ReadToEnd();

                return res;
            }
            catch(Exception ex) {
                return ex.ToString();
            }
           
        }
    }
}
