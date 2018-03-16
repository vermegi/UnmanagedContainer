using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalcApi.Controllers
{
    [Route("api/[controller]")]
    public class CalcController : Controller
    {
        [DllImport(@"HelloCppCalc.dll", 
            EntryPoint = "math_add", 
            CallingConvention = CallingConvention.StdCall)]
        public static extern int Add(int a, int b);

        // GET: api/Calc
        [HttpGet]
        public int Get(int a, int b)
        {
            return Add(a, b);
        }
    }
}
