using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAFE.Core.Misc
{
	public static class Messages
	{
		public const string AccountRegistration_Messages_Emails_ConfirmYourAccountEmailSubject = "Please confirm your registration";

        public const string AccountRegistration_Messages_Emails_AcceptedAccountEmailSubject = "Account approved!";
        public const string AccountRegistration_Messages_Emails_AcceptedAccountEmail =
        @"<p>Dear {0} {1},</p> 
          <p>Wecome to EASE!</p> 
          <p>Your account has been approved by Administrator. <a href='{2}'><b>Click here to Log in using your E-mail and password</b></a>.</p>";

        public const string AccountRegistration_Messages_Emails_ConfirmYourAccountEmailMail =
			"<p>Dear {1} {2},</p> <p>Someone, probably you, has registered an account with this e-mail address for usage with an Essential Annotation Schema for Ecology (EASE)</p><p>To confirm that this account really does belong to You and activate it, open this link in your browser:</p>{0}<p>If you did not register the account, please just ignore this e-mail.</p>";

        public const string AcessRequestAccepted_Subject = "Access accepted!";

        public const string AcessRequestAccepted =
            "<p>Dear {0} {1},</p> <p>Your access request for item(-s) <b>{2}</b> was accepted by the data owner. Please <a href='{3}'><b>Log In</b></a> to see more details.";

        public const string AcessRequestRejected_Subject = "Access rejected!";
        public const string AcessRequestRejected =
          "<p>Dear {0} {1},</p> <p>Your access request for item(-s) <b>{2}</b> was rejected by the data owner. Please <a href='{3}'><b>Log In</b></a> to see more details.";

        public const string AccountRegistration_ValidationErrors_EmailIsAlreadyTaken = "This email is already taken. Try with another one.";
		public const string AccountRegistration_ValidationErrors_UserNameIsAlreadyTaken = "This login name is already taken. Try with another one.";
	}
}
