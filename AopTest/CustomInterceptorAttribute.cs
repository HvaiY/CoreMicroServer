using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AopTest
{
    class CustomInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine("执行之前");
            await next(context);//执行被拦截的方法
            Console.WriteLine("执行之后");
        }
    }
}
