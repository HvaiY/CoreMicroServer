using System;
using System.Net.Http;
using RestTemplateCore;

namespace RestTemplateTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            //@杨中科 封装 负载均衡模板
            //地址：https://github.com/yangzhongke/RuPeng.RestTemplateCore
            //开发完成发布到Nuget:https://www.nuget.org/packages/RestTemplateCore
            // Install-Package RestTemplateCore -Version 1.0.1
            //使用 模板（简单的负载均衡，简化http请求）

            using (HttpClient http = new HttpClient())
            {
                RestTemplate rest = new RestTemplate(http);
                rest.ConsulServerUrl = "http://127.0.0.1:8500";


                #region Post请求
                //SendSms sms = new SendSms() { phoneNum = "119119", msg = "着火了主要火了" };
                //var s = rest.PostAsync("http://MsgService/api/SMS/Send_MI", sms).Result;
                //Console.WriteLine(s.StatusCode); 
                #endregion

                #region get请求

                var result = rest.GetForEntityAsync<Product[]>("http://ProductService/api/Product").Result;
                Console.WriteLine(result.StatusCode);
                foreach (var p in result.Body)
                {
                    Console.WriteLine($"id:{p.Id} Name:{p.Name}");
                }
                #endregion

            }
            Console.ReadKey();

        }
    }
    class SendSms
    {
        public string phoneNum { get; set; }
        public string msg { get; set; }
    }
    class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
