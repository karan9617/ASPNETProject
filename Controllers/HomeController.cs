﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASPProject4.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
using DisplayDataExample.Code;
using System.Text;
using ExcelDataReader;
using System.Collections;

namespace DisplayDataExample.Code
{
    public class Student
    {
        public string Name
        {
            get;
            set;
        }
        public int Age
        {
            get;
            set;
        }
        public string City
        {
            get;
            set;
        }
    }
}

namespace ASPProject4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new List<UserModel>());
        }
        [HttpPost]
        public IActionResult Index(IFormFile file, IFormFile file1)
        {
            
            List<UserModel> users = new List<UserModel>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            HashSet<string> set = new HashSet<string>();

            Hashtable assetsTable = new Hashtable();
            int j1 = 0;
            using (var stream = file1.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    while (reader.Read()) //Each row of the file
                    {
                        if (j1 == 0)
                            j1++;
                        else
                        {
                            string AssetId = reader.GetValue(0).ToString();
                            string AssetName = reader.GetValue(1).ToString();

                            string InterestedFirm = reader.GetValue(2).ToString();
                            if (assetsTable.ContainsKey(AssetName))
                            {
                                ((List<string>)assetsTable[AssetName]).Add(InterestedFirm);
                            }
                            else
                            {
                                assetsTable[AssetName] = new List<string>();
                                ((List<string>)assetsTable[AssetName]).Add(InterestedFirm);
                            }
                            if (!set.Contains(AssetName))
                            {
                                set.Add(AssetName);
                            }
                        }
                        
                    }
                }
            }
            int i = 0,j2 = 0;
            List<string> s1 = new List<string>();
         
            foreach(string ss in set)
            {
               s1.Add(ss);
            }
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        if (j2 == 0)
                        {
                            users.Add(new UserModel
                            {
                                FirmName = "Firm Name",
                                companytable = s1,
            
                            });
                            j2++;
                        }
                        else
                        {
                            string id = reader.GetValue(0).ToString();
                            string name = reader.GetValue(1).ToString();
                            
                            List<string> newListForCompany = new List<string>();
                            foreach (DictionaryEntry de in assetsTable)
                            {
                                if (((List<string>)assetsTable[de.Key]).Contains(id))
                                {
                                    newListForCompany.Add(id);
                                }
                                else
                                {
                                    newListForCompany.Add("S");
                                }
                            }
                            users.Add(new UserModel
                            {
                                FirmName = name,
                                companytable = newListForCompany,
                     
                            });
                        }
                                                
                    }
                }
            }
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
