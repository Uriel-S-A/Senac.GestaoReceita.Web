﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : LoginExtention
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}