using System;
using System.Net.Http;
using RestTemplateCore;
namespace resttemplatetest1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HttpClient http = new HttpClient())
            {
                RestTemplate rest = new RestTemplate(http);
                rest.ConsulServerUrl = "http://127.0.0.1:8500";
                /* SendSms sms = new SendSms();
                 sms.msg = "您好，欢迎";
                 sms.phoneNum = "189189";
                 var resp = rest.PostAsync("http://MsgService/api/SMS/Send_MI", sms).Result;
                 Console.WriteLine(resp.StatusCode);*/
                /*
               var res = rest.GetForEntityAsync<Product[]>("http://ProductService/api/Product").Result;
               Console.WriteLine(res.StatusCode);
               foreach(var p in res.Body)
               {
                   Console.WriteLine(p.Id+p.Name);
               }*/
                var res = rest.GetForEntityAsync<Product>("http://ProductService/api/Product/1").Result;
                Console.WriteLine(res.StatusCode);
                Console.WriteLine(res.Body.Name);
            }

                Console.ReadKey();
        }
    }
    class SendSms
    {
        public String phoneNum { get; set; }
        public String msg { get; set; }
    }

    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
