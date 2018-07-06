using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly.Timeout;

namespace PollyAsync
{
    class Program
    {
        static void Main(string[] args)
        {
#if true //Polly 异步


            Policy<byte[]> pasync = Policy<byte[]>.Handle<Exception>()
                .FallbackAsync(async r =>
                {
                    Console.WriteLine("执行出错");
                    return new byte[0];
                }, async r =>
                {
                    Console.WriteLine(r.Exception);
                });

            pasync = pasync.WrapAsync(Policy.TimeoutAsync(20, TimeoutStrategy.Pessimistic,
                async (context, timespan, task) =>
                {
                    Console.WriteLine("timeout");
                }));

            exce(pasync);


#endif

            Console.ReadKey();
        }

        static async Task exce(Policy<byte[]> p)
        {
            var bytes = await p.ExecuteAsync(async () =>
             {
                 Console.WriteLine("开始任务");
                 HttpClient httpClient = new HttpClient();
                 var result = await httpClient.GetByteArrayAsync("http://static.rupeng.com/upload/chatimage/20183/07EB793A4C247A654B31B4D14EC64BCA.png");

               //  await Task.Delay(5000);//注意不能用Thread.Sleep(5000);
                 Console.WriteLine("完成任务");
                 return result;
             });
            Console.WriteLine("bytes长度" + bytes.Length);
        }
    }
}
