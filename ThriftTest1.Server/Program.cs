using System;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace ThriftTest1.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            
            TServerTransport transport = new TServerSocket(8800);
            var processor = new RuPeng.ThriftTest1.Contract.UserService.Processor(new UserServiceImpl());
            TServer server = new TThreadPoolServer(processor, transport);
            server.Serve();
            /*
            TServerTransport transport = new TServerSocket(8800);
            var processorUserService = new RuPeng.ThriftTest1.Contract.UserService.Processor(new UserServiceImpl());
            var processorCalcService = new RuPeng.ThriftTest1.Contract.CalcService.Processor(new CalcServiceImpl());

            var processorMulti = new TMultiplexedProcessor();
            processorMulti.RegisterProcessor("userService", processorUserService);
            processorMulti.RegisterProcessor("calcService", processorCalcService);

            TServer server = new TThreadPoolServer(processorMulti, transport);

            server.Serve();
            */

            Console.ReadKey();
        }
    }
}
