using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymApplication.SFDC;
using System.Threading.Tasks;
using System.Net;

namespace GymApplication.Controllers
{
    public class ClientsController : Controller
    {

        SalesforceAuthentication sforceAuth = new SalesforceAuthentication();

        // GET: Clients
        //This should be async
        public ActionResult Index()
        {

            IEnumerable<Client__c> selectedClients = Enumerable.Empty<Client__c>();
            //authenticate();

            QueryResult queryResult = null;
            String SOQL = "";

            SOQL = "SELECT Id, Name, First_Name__c, Last_Name__c, Email__c, Phone_Number__c FROM Client__c";

            queryResult = sforceAuth.SfdcBinding.query(SOQL);


            selectedClients = queryResult.records.AsEnumerable().Cast<Client__c>();

            return View(selectedClients);
        }

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Client
        //This should be async
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

        #endregion


        #region Delete

        //this should be async
        public ActionResult Delete(string id)        {            if (id == null)            {                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);            }

            IEnumerable<Client__c> selectedClients = Enumerable.Empty<Client__c>();
            try            {
                QueryResult queryResult = null;
                String SOQL = "";

                SOQL = string.Format("SELECT Id, Name, First_Name__c, Last_Name__c, Email__c, Phone_Number__c FROM Client__c WHERE Id ='{0}'", id);

                queryResult = sforceAuth.SfdcBinding.query(SOQL);
                selectedClients = queryResult.records.AsEnumerable().Cast<Client__c>();            }
            catch (Exception e)            {                this.ViewBag.OperationName = "query Salesforce Contacts";                this.ViewBag.ErrorMessage = e.Message;            }
            if (selectedClients.Count() == 0)            {                return View();            }
            else            {                return View(selectedClients.FirstOrDefault());            }
        }

        //this should be async
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            String[] ids = new String[] { id };
            bool success = false;
            try
            {
                DeleteResult[] deleteResults = sforceAuth.SfdcBinding.delete(ids);
                DeleteResult deleteResult = deleteResults[0];

                if (deleteResult.success)
                {
                    success = true;
                }

            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Delete Gym Clients";
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }



        #endregion

        public ActionResult Edit(string id)
        {
            IEnumerable<Client__c> selectedClients = Enumerable.Empty<Client__c>();
            try            {
                QueryResult queryResult = null;
                String SOQL = "";

                SOQL = string.Format("SELECT Id, Name, First_Name__c, Last_Name__c, Email__c, Phone_Number__c FROM Client__c WHERE Id ='{0}'", id);

                queryResult = sforceAuth.SfdcBinding.query(SOQL);
                selectedClients = queryResult.records.AsEnumerable().Cast<Client__c>();            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Edit Gym Clients";
                this.ViewBag.ErrorMessage = e.Message;
            }
            return View(selectedClients.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Client__c updatedClient)
        {

            bool success = false;

            try
            {
                SaveResult[] saveResults = sforceAuth.SfdcBinding.update(new sObject[] { updatedClient });

                if (saveResults[0].success)
                {
                    success = true;
                }
            }

            catch (Exception e)
            {
                this.ViewBag.OperationName = "Edit Salesforce Contact";
                this.ViewBag.ErrorMessage = e.Message;
            }
           
            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(updatedClient);
            }
        }



    }
}