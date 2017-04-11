using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CAFE.Core.Configuration;
using CAFE.Core.Security;
using CAFE.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Constants = CAFE.Core.Misc.Constants;
using Messages = CAFE.Core.Misc.Messages;
using CAFE.Web.Shared.Extensions;
using CAFE.Web.Models.Manage;

namespace CAFE.Web.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private SignInManager<User, string> _signInManager;
		private UserManager<User> _userManager;
		private IConfigurationProvider _configurationProvider;
		private readonly ISecurityService _securityService;

		//public AccountController()
		//{
		//}

		public AccountController(UserManager<User> userManager, SignInManager<User, string> signInManager, IConfigurationProvider configurationProvider, ISecurityService securityService)
		{
			UserManager = userManager;
			SignInManager = signInManager;
			_configurationProvider = configurationProvider;
			_securityService = securityService;
		}

		public SignInManager<User, string> SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<SignInManager<User, string>>();
			}
			private set 
			{ 
				_signInManager = value; 
			}
		}

		public UserManager<User> UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager<User>>();
			}
			private set
			{
				_userManager = value;
			}
		}

		public ActionResult Details(string id)
		{
			if (id == null)
			{
				ViewBag.errorMessage = "Invalid user Id.";
				return View("Error");
			}

			var dbUser = _securityService.GetUserById(id);
			var userViewModel = AutoMapper.Mapper.Map<User, UserProfileViewModel>(dbUser);
			//ViewBag.UserViewModel = userViewModel;

			return View(userViewModel);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> EmailNotConfirmed()
		{
			await Task.Delay(0);
			return View();
		}

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;

			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, change to shouldLockout: true
			var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
                    {
                        // Require the user to have a confirmed email before they can log on.
                        var user = await UserManager.FindByNameAsync(model.UserName);
                        if (user != null)
                        {
                            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                            {
                                return RedirectToAction(nameof(EmailNotConfirmed), this.ControllerNameForRouting<AccountController>());
                            }

                            var applicationConfiguration = _configurationProvider.Get<ApplicationConfiguration>();
                            if (applicationConfiguration.IsUsersAcceptanceNeed && !user.IsAccepted)
                            {
                                return View("AccountNotAccepted");
                            }
                        }

                        return RedirectToLocal(returnUrl);
                    }
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return View(model);
			}
		}

		//
		// GET: /Account/VerifyCode
		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
		{
			// Require that the user has already logged in via username/password or external login
			if (!await SignInManager.HasBeenVerifiedAsync())
			{
				return View("Error");
			}
			return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//
		// POST: /Account/VerifyCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// The following code protects for brute force attacks against the two factor codes. 
			// If a user enters incorrect codes for a specified amount of time then the user account 
			// will be locked out for a specified amount of time. 
			// You can configure the account lockout settings in IdentityConfig
			var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(model.ReturnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid code.");
					return View(model);
			}
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{

			if (ModelState.IsValid)
			{
				var emailIsAlreadyTaken = await UserManager.FindByEmailAsync(model.Email) != null;

				if (emailIsAlreadyTaken)
				{
					ModelState.AddModelError(nameof(RegisterViewModel.Email), Messages.AccountRegistration_ValidationErrors_EmailIsAlreadyTaken);
				}

				if(!emailIsAlreadyTaken)
				{
					var user = new User { UserName = model.Email, Email = model.Email, Name = model.Name, Surname = model.Surname, IsActive = true };
                    var role = _securityService.GetGroupByName(Constants.UserRoleName);
                    user.Groups.Add(role);

                    var result = await UserManager.CreateAsync(user, model.Password);

					if (result.Succeeded)
					{
     
                        var identityResult = await UserManager.AddToRoleAsync(user.Id, Constants.UserRoleName);
						await SendEmailConfirmationTokenAsync(user);

						return RedirectToAction(nameof(RegistrationSuccess), this.ControllerNameForRouting<AccountController>());
					}

					AddErrors(result);
				}
			}

			return View(model);
		}

		private async Task<string> SendEmailConfirmationTokenAsync(User user)
		{
			string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
			var callbackUrl = Url.Action("ConfirmEmail", "Account",
			   new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
			await UserManager.SendEmailAsync(
				user.Id, 
				Messages.AccountRegistration_Messages_Emails_ConfirmYourAccountEmailSubject,
				String.Format(Messages.AccountRegistration_Messages_Emails_ConfirmYourAccountEmailMail, callbackUrl, user.Name, user.Surname));

			return callbackUrl;
		}

		[AllowAnonymous]
		public async Task<ActionResult> Info()
		{
			await Task.Delay(0);
			return View();
		}

		[AllowAnonymous]
		public async Task<ActionResult> RegistrationSuccess()
		{
			await Task.Delay(0);
			return View();
		}

		//
		// GET: /Account/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("Error");
			}

			var result = await UserManager.ConfirmEmailAsync(userId, code);

			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		//
		// GET: /Account/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		//
		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}
				var user = await UserManager.FindByNameAsync(model.EmailOrUserName);
				if (user == null)
				{
					user = await UserManager.FindByEmailAsync(model.EmailOrUserName);
				}

				var userExists = user != null;
				var userEmailIsConfirmed = userExists && user.EmailConfirmed;

				if (userExists && userEmailIsConfirmed)
				{
					string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
					var callbackUrl = this.ActionUrl<AccountController>(nameof(ResetPassword), new {userId = user.Id, code = code}, protocol: Request.Url.Scheme);
						
					await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
					
					return RedirectToAction(nameof(ForgotPasswordConfirmation), this.ControllerNameForRouting<AccountController>());
				}

				if (!userExists)
				{
					ModelState.AddModelError(nameof(ForgotPasswordViewModel.EmailOrUserName), "This email or login is not registered, please check it and try again");
				}
				else if (!userEmailIsConfirmed)
				{
					ModelState.AddModelError(nameof(ForgotPasswordViewModel.EmailOrUserName), "This email is not confirmed yet, please confirm your email first");
				}
				else
				{
					throw new Exception("User may not reset the password for unknown reason");
				}
			}

			return View(model);
		}

		//
		// GET: /Account/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View();
		}


		[AllowAnonymous]
		public ActionResult ResendRegistrationEmail()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResendRegistrationEmail(ResendRegistrationEmailViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByEmailAsync(model.Email);

				var userExists = user != null;
				var userEmailAlreadyConfirmed = userExists && await UserManager.IsEmailConfirmedAsync(user.Id);

				if (userExists && !userEmailAlreadyConfirmed)
				{
					await SendEmailConfirmationTokenAsync(user);
					return RedirectToAction(nameof(RegistrationSuccess), this.ControllerNameForRouting<AccountController>());
				}

				if (userEmailAlreadyConfirmed)
				{
					ModelState.AddModelError(nameof(ForgotPasswordViewModel.EmailOrUserName), "This email was already confirmed");
				}
				else
				{
					ModelState.AddModelError(nameof(ForgotPasswordViewModel.EmailOrUserName), "This email is not registered, please check your email address and try again");
				}
			}

			return View(model);

		}


		//
		// GET: /Account/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword(string userId, string code)
		{
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.UserId = userId;
            model.Code = code;

            return code == null ? View("Error") : View(model);
		}

		//
		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await UserManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			AddErrors(result);
			return View();
		}

		//
		// GET: /Account/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		//
		// GET: /Account/SendCode
		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
		{
			var userId = await SignInManager.GetVerifiedUserIdAsync();
			if (userId == null)
			{
				return View("Error");
			}
			var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
			return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//
		// POST: /Account/SendCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			// Generate the token and send it
			if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
			{
				return View("Error");
			}
			return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
		}


		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Dashboard");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}