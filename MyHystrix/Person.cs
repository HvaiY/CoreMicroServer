using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyHystrix
{
 
        public class Person//需要 public 类
        {
            [HystrixCommand(nameof(HelloFallBackAsync))]  //降级的方法 HelloFallBackAsync
        public virtual async Task<string> HelloAsync(string name)//需要是虚方法
            {
            //如果出现异常 那么降级
            throw new Exception();
                Console.WriteLine("hello" + name);
                String s = null;
                // s.ToString();
                return "ok";
            }
            public async Task<string> HelloFallBackAsync(string name)
            {
                Console.WriteLine("执行失败" + name);
                return "fail";
            }

            [HystrixCommand(nameof(AddFall))]
            public virtual int Add(int i, int j)
            {
                String s = null;
                // s.ToArray();
                return i + j;
            }
            public int AddFall(int i, int j)
            {
                return 0;
            }
        }
    }
