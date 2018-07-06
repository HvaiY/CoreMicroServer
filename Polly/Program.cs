using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace PollyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Install-Package Polly -Version 6.0.1
            //使用 Polly
#if false //简单使用  出现异常立即执行FallBack
            try
            {
                //策略
                Policy policy = Policy.Handle<ArgumentException>()
                    .Fallback(() =>
                    {
                        Console.WriteLine("出错了！");
                        Console.WriteLine("出错后执行了我这里");
                    });

                //执行(动作)
                policy.Execute(() =>
                {
                    Console.WriteLine("开始执行");
                    //如出现异常就执行上面
                    throw new ArgumentException();
                    Console.WriteLine("执行结束");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"异常：{e}");
            } 
#endif
#if false //出现异常  返回带返回值的 polly           降级 
            Policy<string> policy = Policy<string>
                .Handle<Exception>()
                .Fallback(() =>
                 {
                     Console.WriteLine("出错了返回一个值");
                     return "降级值";
                 });

            string value = policy.Execute(() =>
            {
                Console.WriteLine("开始执行");
                throw new Exception("异常");
                Console.WriteLine("执行结束");
                return "正常值";
            });

            Console.WriteLine($"执行结果;{value}");
#endif
#if false //异常 重试                 熔断重试

            Policy policy = Policy
                .Handle<Exception>()
                // .Retry(3);
                .WaitAndRetry(5, i => TimeSpan.FromSeconds(1));//重试五次  每次等待一秒 ，如果五秒后还异常那么 抛出异常
            policy.Execute(() =>
            {
                Console.WriteLine("开始任务");
                if (DateTime.Now.Second % 10 != 0)
                {
                    throw new Exception("出错");
                }
                Console.WriteLine("完成任务");
            });
            //RetryForever()是一直重试直到成功
            //Retry()是重试最多一次；
            //Retry(n) 是重试最多n次；
            //WaitAndRetry()可以实现“如果出错等待100ms再试还不行再等150ms秒。。。。”，重载方法很
            //    多，不再一一介绍。还有WaitAndRetryForever。
#endif

#if false // 短路保护(Circuit Breaker)    熔断重试
            /*
             * 出现N次连续错误，则把“熔断器”（保险丝）熔断，等待一段时间，等待这段时间内如果再Execute
则直接抛出BrokenCircuitException异常，根本不会再去尝试调用业务代码。等待时间过去之后，再
执行Execute的时候如果又错了（一次就够了），那么继续熔断一段时间，否则就恢复正常。
这样就避免一个服务已经不可用了，还是使劲的请求给系统造成更大压力。
Policy policy = Policy
.Handle<Exception>()
.CircuitBreaker(6,TimeSpan.FromSeconds(5));//连续出错6次之后熔断5秒(不会再
去尝试执行业务代码）
             */

            Policy policy = Policy
                .Handle<Exception>()
                .CircuitBreaker(2, TimeSpan.FromSeconds(5));//连续出错2次之后熔断5秒(不会再去尝试执行业务代码）。
            while (true)
            {
                Console.WriteLine("开始Execute");
                try
                {
                    policy.Execute(() => {
                        Console.WriteLine("开始任务");
                        throw new Exception("出错");
                        Console.WriteLine("完成任务");
                    });
                }
                catch (Exception ex)
     
                {
                    Console.WriteLine("execute出错" + ex);
                }
                Thread.Sleep(500);
            }
#endif

#if false //策略包裹(warp)的方式实现（）   熔断降级
            /*
             *可以把多个ISyncPolicy合并到一起执行：
policy3= policy1.Wrap(policy2);
执行policy3就会把policy1、policy2封装到一起执行
policy9=Policy.Wrap(policy1, policy2, policy3, policy4, policy5);把更多一起封装。
             */
             //p1
            Policy policyFallBack = Policy.Handle<Exception>().Fallback(() =>
            {
                Console.WriteLine("降级");
            });
            //p2
            Policy policyRetry = Policy.Handle<Exception>().Retry(3);
            var p = policyFallBack.Wrap(policyRetry);
            p.Execute(() =>
            {
                Console.WriteLine("开始执行");
                throw  new Exception("异常了");
                Console.WriteLine("执行结束");
            });
            //实现的效果是 尝试执行1次重试3次  然后 异常后降级(处理回调函数)
#endif
#if false //超时策略
            //出现超时3秒 是一个策略 需要warp后才能使用 
            //这里设计的是3秒没响应就降级
            Policy pt = Policy.Timeout(3, TimeoutStrategy.Pessimistic);
            Policy pb = Policy.Handle<Exception>().Fallback(() =>
            {
                Console.WriteLine("超时了，降级吧");
            });
            Policy pw = pb.Wrap(pt);
            pw.Execute(() =>
            {
                Console.WriteLine("开始执行");
                Thread.Sleep(5000);
                Console.WriteLine("执行完成");
            });
#endif


            Console.ReadKey();
        }
    }
}
