using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Menzies1.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;

namespace Menzies1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult CustomerQuiry(EnquiryForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage mailMsg = new MailMessage();

                    // To
                    mailMsg.To.Add(new MailAddress("junnicesu@aliyun.com"));
                    mailMsg.To.Add(new MailAddress("2018008452@studetn.sit.ac.nz"));
                    mailMsg.To.Add(new MailAddress("2011002622@student.sit.ac.nz"));
                    mailMsg.To.Add(new MailAddress("2014006531@student.sit.ac.nz"));
                    mailMsg.To.Add(new MailAddress("2018001812@student.sit.ac.nz"));

                    // From
                    mailMsg.From = new MailAddress(form.Email, "From " + form.FirstName + " " + form.LastName);

                    // Subject and multipart/alternative Body
                    mailMsg.Subject = "Enquiry from " + form.FirstName;
                    string text = form.Enquiry;
                    //string html = @"<p>html body</p>";
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                    //mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                    // Init SmtpClient and send
                    SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_918bbf674ec43e1970d4218fe3c4d728@azure.com", "Password(123)");
                    smtpClient.Credentials = credentials;

                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    ViewBag.data = ex.Message;
                    throw ex;
                }
            }
            else
            {
                //invalid data for the form
                return RedirectToAction("Contact");
                //return RedirectToAction()
                //!!! the data validation doesn't work. Is it becasue there is no standalone View for action CustomerQuiry() ?  
                //If so, could it be a partial View for the form and data validation in the controller help?  
            }

            return RedirectToAction("Contact"); 
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

    public class EnquiryForm
    {
        [Required]
        [MinLength(2, ErrorMessage = "Please tell us your first name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Please tell us your last name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(20, ErrorMessage = "Minimum length of Enquiry is 20 letters.")]
        public string Enquiry { get; set; }
    }
}
