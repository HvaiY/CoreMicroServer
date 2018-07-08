using AspectCore.DynamicProxy;
using System;

namespace MyHystrix
{
    class Program
    {
        /// <summary>
        /// Install-Package AspectCore.Core -Version 0.5.0
        /// 创建对象继承AbstractInterceptorAttribute
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                Person p = proxyGenerator.CreateClassProxy<Person>();
               var res= p.HelloAsync("大海").Result;
                Console.WriteLine(res);
            }

            Console.ReadKey();
        }
    }
}
