using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using CAFE.Core.Configuration;
using CAFE.Core.Security;
using CAFE.Web.Models.Manage;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CAFE.Core.Integration;

namespace CAFE.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private SignInManager<User, string> _signInManager;
        private UserManager<User> _userManager;
        private readonly ISecurityServiceAsync _securityService;
        private readonly Core.Configuration.IConfigurationProvider _configurationProvider;
        private readonly IUserDataIntegrationService _userDataIntegrationService;

        public ManageController(UserManager<User> userManager, SignInManager<User, string> signInManager, 
            ISecurityServiceAsync securityService, Core.Configuration.IConfigurationProvider configurationProvider,
            IUserDataIntegrationService userDataIntegrationService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _securityService = securityService;
            _configurationProvider = configurationProvider;
            _userDataIntegrationService = userDataIntegrationService;
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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        ////
        //// GET: /Manage/ChangePassword
        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = PasswordChangeResult.FailedNewPasswordInput;

                if (string.IsNullOrWhiteSpace((model.OldPassword)))
                    errorMessage = PasswordChangeResult.FailedOldPasswordInput;
                else if(model.NewPassword.Length < 6)
                    errorMessage = PasswordChangeResult.FailedNewPasswordLength;

                return RedirectToAction("Edit", new { Message = errorMessage });
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Edit", new { Message = PasswordChangeResult.SuccessChangePassword });
            }
            AddErrors(result);
            return RedirectToAction("Edit", new { Message = PasswordChangeResult.FailedOldPasswordInput });
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }


        //
        // GET: /Manage/Details
        public async Task<ActionResult> Details(ImportErrors? error)
        {
            if (error != null)
            {
                switch (error)
                {
                    case ImportErrors.FileFormatError:
                        ViewData["ErrorMessage"] = "Error! You can only import from .zip - archives generated by EASE - tool";
                        break;
                }
            }
            var user = await _securityService.GetUserByIdAsync(User.Identity.GetUserId<string>());
            return View(Mapper.Map(user, new Models.Manage.UserDetailsViewModel()));
        }

        //
        // GET: /Manage/Edit
        public async Task<ActionResult> Edit(PasswordChangeResult? message)
        {
            ViewBag.Message = message;
            if (message == PasswordChangeResult.FailedNewPasswordInput)
            {
                ViewData.ModelState.AddModelError("", "New password and confirm password do not match");
            }
            else if (message == PasswordChangeResult.FailedOldPasswordInput)
            {
                ViewData.ModelState.AddModelError("", "Invalid old password");
            }
            else if (message == PasswordChangeResult.FailedNewPasswordLength)
            {
                ViewData.ModelState.AddModelError("", "The New password must be at least 6 characters long.");
            }

            var user = await _securityService.GetUserByIdAsync(User.Identity.GetUserId<string>());
            return View(Mapper.Map(user, new UserEditViewModel()));
        }

        //
        // POST: /Manage/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            var foundUser = await _securityService.GetUserByIdAsync(User.Identity.GetUserId<string>());
            try
            {
                var mappedUser = Mapper.Map(model, foundUser);
                await _securityService.SaveUserAsync(mappedUser);
                return RedirectToAction("Details");

            }
            catch (Exception exception)
            {
                //TODO: log here
            }

            return View(model);
        }

        //
        // POST: /Manage/Edit
        [HttpPost]
        public async Task<ActionResult> Remove(bool removeOwnData)
        {
            await
                _securityService.RemoveUserAsync(
                    await _securityService.GetUserByIdAsync(User.Identity.GetUserId<string>()), removeOwnData);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> UploadAvatar()
        {
            var file = Request.Files["avatar"];
            if (file != null)
            {
                var appConfig = _configurationProvider.Get<ApplicationConfiguration>();
                var basePath = appConfig.ApploadsRoot;
                var userAvatarsPath = Path.Combine(basePath, "UserAvatars");
                string oldfilename = Path.GetFileName(file.FileName);
                if (oldfilename != null)
                {
                    var oldExtension = Path.GetExtension(oldfilename);
                    var newFileName = string.Concat(Guid.NewGuid().ToString(), oldExtension);
                    var fileVirtualPath = userAvatarsPath + "/" + newFileName;
                    file.SaveAs(Server.MapPath(fileVirtualPath));

                    var user = await _securityService.GetUserByIdAsync(User.Identity.GetUserId());
                    user.PhotoUrl = fileVirtualPath;
                    await _securityService.SaveUserAsync(user);
                }
            }


            return RedirectToAction("Edit");
        }


        /// <summary>
        /// Export user's data
        /// </summary>
        /// <returns>Bool</returns>
        [HttpPost]
        public async Task<ActionResult> ExportData()
        {
            var currentUser = await _securityService.GetUserByIdAsync(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            var basePath = appConfig.ApploadsRoot;
            var userFilesPath = Path.Combine(Environment.CurrentDirectory, basePath, "UserFiles");
            
            var rootPath = HttpContext.Server.MapPath(userFilesPath);

            var host = this.HttpContext.Request.Url.Scheme + "://" + this.HttpContext.Request.Url.Authority;
            var arch = _userDataIntegrationService.ExportData(currentUser, rootPath, host);

            if (!string.IsNullOrEmpty(arch))
            {
                return new FileContentResult(Convert.FromBase64String(arch), "application/zip")
                {
                    FileDownloadName =
                        "Exported_user_data_" + DateTime.Now.Date.ToString().Replace(".", "").Replace("/", "") + ".zip"
                };
            }

            return View();
        }


        /// <summary>
        /// Import user's data
        /// </summary>
        /// <returns>Bool</returns>
        [HttpPost]
        public async Task<ActionResult> ImportData(HttpPostedFileBase file)
        {
            string fileData = string.Empty;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    if(Path.GetExtension(file.FileName).Replace(".", "").ToLower() != "zip") throw new InvalidOperationException("Need to upload zip archive");
                    byte[] data;
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    fileData = Convert.ToBase64String(data);
                }
                else if (Request.Files.Count > 0)
                {
                    var file1 = Request.Files[0];

                    if (Path.GetExtension(file1.FileName).Replace(".", "").ToLower() != "zip") throw new InvalidOperationException("Need to upload zip archive");

                    if (file1 != null && file1.ContentLength > 0)
                    {
                        //var fileName = Path.GetFileName(file1.FileName);
                        //var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        //file.SaveAs(path);
                        byte[] data;
                        using (Stream inputStream = file1.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                        }

                        fileData = Convert.ToBase64String(data);
                    }
                }

                if (string.IsNullOrEmpty(fileData)) return RedirectToAction("Index", "Dashboard");

                var currentUser = await _securityService.GetUserByIdAsync(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var appConfig = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
                var basePath = appConfig.ApploadsRoot;
                var userFilesPath = Path.Combine(Environment.CurrentDirectory, basePath, "UserFiles");
                var rootPath = HttpContext.Server.MapPath(userFilesPath);

                _userDataIntegrationService.ImportData(currentUser, fileData, rootPath);
            }
            catch(Exception ex)
            {
                var values = new RouteValueDictionary();
                values.Add("error", ImportErrors.FileFormatError);
                return RedirectToAction("Details", "Manage", values);
            }

            return RedirectToAction("Index", "Dashboard");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }

    public enum PasswordChangeResult
    {
        FailedNewPasswordInput,
        FailedNewPasswordLength,
        FailedOldPasswordInput,
        SuccessChangePassword
    }

    public enum ImportErrors
    {
        FileFormatError
    }
}