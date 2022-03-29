using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace techsolaWorkClockTimer;

//public static class DevOpsApi
//{
//    public static string ResponseBody = null!;
//    public static async void GetProjects()
//    {
//        var personalaccesstoken = "7xq2ullzgxwpxkorverym4nubytxxc6lojbakyouotbr64j72vuq";

//        using (HttpClient client = new HttpClient())
//        {
//            client.DefaultRequestHeaders.Accept.Add(
//                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
//                Convert.ToBase64String(
//                    System.Text.ASCIIEncoding.ASCII.GetBytes(
//                        string.Format("{0}:{1}", "", personalaccesstoken))));

//            using (HttpResponseMessage response = await client.GetAsync(
//                       "http://azuredevops/Techsola/HeritagePMS/_apis/wit/queries/?api-version=6.0"))
//            {
//                response.EnsureSuccessStatusCode();
//                ResponseBody = await response.Content.ReadAsStringAsync();
//            }
//        }
//    }
//}