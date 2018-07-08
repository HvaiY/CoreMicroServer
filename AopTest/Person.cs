using System;
using System.Collections.Generic;
using System.Text;

namespace AopTest
{
    /// <summary>
    /// 需要能被反射 方法能够用这里面的特性拦截 类需要为public   方法为virtual
    /// </summary>
    public class Person
    {
        [CustomInterceptor] //特性标记该方法  切面过滤
        public virtual void Say(string name)
        {
          
            Console.WriteLine($"名字为：{name}");
        }
    }
}
