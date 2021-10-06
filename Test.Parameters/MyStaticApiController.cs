using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebserver;

namespace Test.Parameters
{
    [AttributeRoutePrefix("MyStaticApi")]
    public static class MyStaticApiController
    {
        [AttributeRoute(HttpMethod.GET, "GetTest1000")]  // HttpGet("GetTest1000") -> Rename GetTestNoNumber to GetTest1000
        public static async Task<MyClass> GetTestNoNumber(int x, int y)
        {
            return new MyClass() { X = x * y };
        }

        [AttributeRoute(HttpMethod.GET)]
        public static async Task<MyClass> GetTest1(int x, int y)
        {
            return new MyClass() { X = x * y };
        }

        [AttributeRoute(HttpMethod.GET)]
        public static async Task<MyClass> GetTest2(HttpContext ctx, int x, int y)
        {
            return new MyClass()
            {
                X = x * y,
                Message = ctx.Request.Url.Full
            };
        }

        [AttributeRoute(HttpMethod.POST)]
        public static async Task<MyClass> PostTest1(MyClass mc)
        {
            return new MyClass() { X = mc.X * 2 };
        }

        [AttributeRoute(HttpMethod.POST)]
        public static async Task<MyClass> PostTest2(HttpContext ctx, MyClass mc)
        {
            return new MyClass()
            {
                X = mc.X * 2,
                Message = ctx.Request.Url.Full
            };
        }

        [AttributeRoute(HttpMethod.POST)]
        public static async Task<MyClass> PostTest3(HttpContext ctx, MyClass mc, int multiplier)
        {
            return new MyClass()
            {
                X = mc.X * multiplier,
                Message = ctx.Request.Url.Full
            };
        }
    }
}
