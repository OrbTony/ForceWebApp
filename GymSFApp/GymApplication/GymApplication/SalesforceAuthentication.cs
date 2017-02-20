using GymApplication.SFDC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApplication
{
    public class SalesforceAuthentication
    {
        string userName = "mcconvilletony@tutorial.com";

        string password = "First58PeterhzvEfe7Ujw78k2x6789AuVHg";

        public SforceService SfdcBinding { get; set; }

        public LoginResult CurrentLoginResult { get; set; }

        public SalesforceAuthentication()
        {

            SfdcBinding = new SforceService();

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
                //This is something else, probably communication
                SfdcBinding = null;

                throw e;
            }
            //Change the binding to the new endpoint
            SfdcBinding.Url = CurrentLoginResult.serverUrl;

            //Create a new session header object and set the session id to that returned by the login
            SfdcBinding.SessionHeaderValue = new SessionHeader();
            SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;

        }


    }
}