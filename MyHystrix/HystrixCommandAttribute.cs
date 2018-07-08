using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyHystrix
{
    [AttributeUsage(AttributeTargets.Method)]//约束只标记方法(该特性只标记方法)
    class HystrixCommandAttribute : AbstractInterceptorAttribute
    {
        private string _fallBackMethod;
        public HystrixCommandAttribute(string fallBackMethod)
        {
            this._fallBackMethod = fallBackMethod;
        }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception)
            {
                Console.WriteLine("出错 调用降级方法");
                //调用降级方法
                //1、获取降级方法
                //2、调用降级方法
                //3、把降级方法返回值返回。

                //获取降级方法
                MethodInfo fallbackMethod = context.Implementation.GetType().GetMethod(_fallBackMethod);

                Object returnValue = fallbackMethod.Invoke(context.Implementation, context.Parameters);
                context.ReturnValue = returnValue;
            }

        }
    }
}
