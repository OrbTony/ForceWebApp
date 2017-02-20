using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymApplication.SFDC;
using System.Threading.Tasks;

namespace GymApplication.Controllers
{
    public class ClientsController : Controller
    {

        SalesforceAuthentication sforceAuth = new SalesforceAuthentication();

        // GET: Clients
        public ActionResult Index()
        {

            IEnumerable<Client__c> selectedClients = Enumerable.Empty<Client__c>();
            //authenticate();

            QueryResult queryResult = null;
            String SOQL = "";

            SOQL = "SELECT Name, First_Name__c, Last_Name__c, Email__c, Phone_Number__c FROM Client__c";

            queryResult = sforceAuth.SfdcBinding.query(SOQL);


            selectedClients = queryResult.records.AsEnumerable().Cast<Client__c>();

            return View(selectedClients);
        }

        public ActionResult Create()
        {
            return View();
        }

        //POST: Client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client__c client)
        {

            try
            {
                SaveResult[] saveResults = sforceAuth.SfdcBinding.create(new sObject[] { client });

                if (saveResults[0].success)
                {
                    string Id = "";
                    Id = saveResults[0].id;
                }
                else
                {
                    string result = "";
                    result = saveResults[0].errors[0].message;
                }

            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Create Salesforce Client";
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(client);
            }
        }
    }
}