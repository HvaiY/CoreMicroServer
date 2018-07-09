using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace JWTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Install-Package JWT");
            //加密;
            //  Install - Package JWT
            var payload = new Dictionary<string, object>
            {
                { "UserId", 123 },
                { "UserName", "admin" }
            };
            var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";//不要泄露（随便写 但是不要泄露）
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            //加密的token
            var token = encoder.Encode(payload, secret);
            Console.WriteLine(token);
            //token 就是明文+加密签名  可以 https://jwt.io/ 查看


            //解密：：：：
            var token2 =
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJVc2VySWQiOjEyMywiVXNlck5hbWUiOiJhZG1pbiJ9.Qjw1epD5P6p4Yy2yju3-fkq28PddznqRj3ESfALQy_U";
            var secret2 = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
            try
            {
                IJsonSerializer serializer2 = new JsonNetSerializer();
                IDateTimeProvider provider2 = new UtcDateTimeProvider();
                IJwtValidator validator2 = new JwtValidator(serializer2, provider2);
                IBase64UrlEncoder urlEncoder2 = new JwtBase64UrlEncoder();
                IJwtDecoder decoder2 = new JwtDecoder(serializer2, validator2, urlEncoder2);
                var json = decoder2.Decode(token2, secret2, verify: true);
                Console.WriteLine(json);
            }
            catch (FormatException)
            {
                Console.WriteLine("Token format invalid");
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }
            Console.ReadKey();
        }
    }
}
