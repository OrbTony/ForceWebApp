using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymApplication.SFDC;

namespace GymApplication.Controllers
{
    public class ClientsController : Controller
    {
        string userName = "mcconvilletony@tutorial.com";

        string password = "First58PeterhzvEfe7Ujw78k2x6789AuVHg";

        // GET: Clients
        public ActionResult Index()
        {

            IEnumerable<Client__c> selectedClients = Enumerable.Empty<Client__c>();
            //authenticate();

             SforceService SfdcBinding = new SforceService();
            LoginResult CurrentLoginResult = null;

            try
            {
                CurrentLoginResult = SfdcBinding.login(userName, password);
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                //This is likley to be caused by bad username or password
                SfdcBinding = null;

                throw e;
            }
            catch (Exception e)
            {
                //This is something else, probably comminication
                SfdcBinding = null;

                throw e;
            }
            //Change the binding to the new endpoint
            SfdcBinding.Url = CurrentLoginResult.serverUrl;

            //Create a new session header object and set the session id to that returned by the login
            SfdcBinding.SessionHeaderValue = new SessionHeader();
            SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;

            QueryResult queryResult = null;
            String SOQL = "";

            SOQL = "SELECT First_Name__c, Last_Name__c, Email__c, Phone_Number__c FROM Client__c";

            queryResult = SfdcBinding.query(SOQL);



            //var model = (from p in db.Persons // .Includes("Addresses") here?
            //             select new PersonAddViewModel()
            //             {
            //                 Id = p.Id,
            //                 Name = p.Name,
            //                 Street = p.Address.Street,
            //                 // or if collection
            //                 Street2 = p.Addresses.Select(a => a.Street).FirstOrDefault()
            //             });

            selectedClients = queryResult.records.AsEnumerable().Cast<Client__c>();

            return View(selectedClients);
        }

        //public IEnumerable<Client__c> authenticate()
        //{
            //SforceService SfdcBinding = new SforceService();
            //LoginResult CurrentLoginResult = null;

            //try
            //{
            //    CurrentLoginResult = SfdcBinding.login(userName, password);
            //}
            //catch (System.Web.Services.Protocols.SoapException e)
            //{
            //    //This is likley to be caused by bad username or password
            //    SfdcBinding = null;

            //    throw e;
            //}
            //catch (Exception e)
            //{
            //    //This is something else, probably comminication
            //    SfdcBinding = null;

            //    throw e;
            //}
            ////Change the binding to the new endpoint
            //SfdcBinding.Url = CurrentLoginResult.serverUrl;

            ////Create a new session header object and set the session id to that returned by the login
            //SfdcBinding.SessionHeaderValue = new SessionHeader();
            //SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;

            //QueryResult queryResult = null;
            //String SOQL = "";

            //SOQL = "SELECT First_Name__c, Last_Name__c, Email__c, Phone_Number__c FROM Client__c";

            //queryResult = SfdcBinding.query(SOQL);

            //if (queryResult.size > 0)
            //{
            //    //put some code in here to handle the records being returned
            //    int i = 0;
            //    Client__c lead = (Lead)queryResult.records[i];
            //    string firstName = lead.FirstName;
            //    string lastName = lead.LastName;
            //    string businessPhone = lead.Phone;
            //}
            //else
            //{
            //    //put some code in here to handle no records being returned
            //    string message = "No records returned.";
            //}

//            return queryResult.records;

//        }
    }
}