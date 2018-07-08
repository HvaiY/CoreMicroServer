using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Consul;

namespace 服务消费者1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var consul=new ConsulClient(c=>c.Address=new Uri("http://127.0.0.1:8500")))
            {
#if false
                //异步获取consul中所有的服务 api
                var servers = consul.Agent.Services().Result.Response;//键值对集合
                foreach (var s in servers.Values)
                {
                    Console.WriteLine($"id={s.ID} service={s.Service} addr={s.Address} port={s.Port}");
                } 
#endif
                //匹配MsgService的服务 (前提是consul中已注册大量服务) （忽略名称大小写）
                var services = consul.Agent.Services().Result.Response.Values.Where(s =>
                    s.Service.Equals("MsgService", StringComparison.OrdinalIgnoreCase));
                //随机获取一个/台服务(自己决定选择某一台服务器，就是一种客户端负载均衡)

                if (!services.Any())
                {
                    Console.WriteLine("没有任何服务器");
                }
                else
                {
                    Random rand = new Random();
                    int index = rand.Next(services.Count());//[0-services.Count()) 包含左边 不包含右边
                    var service = services.ElementAt(index);
                    Console.WriteLine($"id={service.ID} service={service.Service} addr={service.Address} port={service.Port}");
                    //发送请求
                    Console.WriteLine("发送请求");
                    using (HttpClient http = new HttpClient())
                    {
                        using (var httpContent = new StringContent("{phoneNum:'119',msg:'帮助我 火警'}", Encoding.UTF8, "application/json"))
                        {
                            var result = http.PostAsync($"http://{service.Address}:{service.Port}/api/SMS/Send_HW", httpContent).Result;
                            Console.WriteLine(result.StatusCode);
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
