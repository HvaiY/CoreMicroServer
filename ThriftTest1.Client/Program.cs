using RuPeng.ThriftTest1.Contract;
using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace ThriftTest1.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (TTransport transport = new TSocket("localhost", 8800))
            using (TProtocol protocol = new TBinaryProtocol(transport))
            using (var clientUser = new UserService.Client(protocol))
            {
                transport.Open();
                var users = clientUser.GetAll();
                foreach(var u in users)
                {
                    Console.WriteLine($"{u.Id},{u.Name}");
                }
            }
            /*
            using (TTransport transport = new TSocket("localhost", 8800))
            using (TProtocol protocol = new TBinaryProtocol(transport))
            using (var protocolUserService = new TMultiplexedProtocol(protocol, "userService"))
            using (var clientUser = new UserService.Client(protocolUserService))
            using (var protocolCalcService = new TMultiplexedProtocol(protocol, "calcService"))
            using (var clientCalc = new CalcService.Client(protocolCalcService))
            {
                transport.Open();
                User u = clientUser.Get(1);
                Console.WriteLine($"{u.Id},{u.Name}");
                Console.WriteLine(clientCalc.Add(1, 2));
            }*/


            Console.ReadKey();
        }
    }
}
