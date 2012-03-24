﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raita.Web.Models;

namespace Raita.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

        	const string aFeature =
@"@wip
Feature: EPI GBP conversion

Background: 
	Given RSG exchange rates of:
		| DateValid  | CurrencyCode | Rate |
		| 2009-09-01 | EUR          | 0.95 |
	Given fx rates of:
		| From Date  | To Date    | Source | Dest | Rate |
		| 2009-09-30 | 2009-09-30 | EUR    | GBP  | 0.95 |

Scenario: EPI conversion
	Given a contract with original currency of EUR and Effective Date of 2009-09-30
	When the EPI for the section is changed to
		| Account       | Amount |
		| EPI100Percent | 1m     |
		| EPIOurShare   | 500k   |
	Then An EPI Transaction should be created
	And the EPI transaction should be converted to GBP
	And the converted amounts should be
		| Account       | Amount |
		| Epi100Percent | 950k   |
		| EpiOurShare   | 475k   |";
        	return View(new Editor {Content = aFeature});
        }

    }
}
