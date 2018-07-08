using AspectCore.DynamicProxy;
using System;

namespace AopTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //为了使用熔断降级 ，代码不至于与业务代码在一起 ，使用aop
            //国产Aop  Install-Package AspectCore.Core -Version 0.5.0
            //版本更新比较快，注意使用
            //使用Aop封装降级  见项目 MyHystrix

            //方法调用  
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                Person p = proxyGenerator.CreateClassProxy<Person>();
                p.Say("大海");
            }

            Console.ReadKey();
        }
    }
}
