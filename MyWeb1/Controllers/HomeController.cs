using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWeb1.Models;

namespace MyWeb1.Controllers
{
    public class HomeController : Controller
    {
        private SMSService smsService;

        public HomeController(SMSService smsService)
        {
            this.smsService = smsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost(nameof(SendSMS))]
        [HttpPost]
        public async Task<IActionResult> SendSMS()
        {
            await smsService.Send_MI("110", "help");
            return View();
        }
    }
}
