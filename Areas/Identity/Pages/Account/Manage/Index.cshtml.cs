using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhyApp.Models;

namespace WhyApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
            //IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
        }

        //public string Username { get; set; }
        public string Picture { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public DateTime BirthDate { get; set; }
            public string Age { get; set; }
            public string Picture { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            //UserName = userName;

            Input = new InputModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                BirthDate = user.BirthDate,
                Age = user.Age,
                Email = email,
                PhoneNumber = phoneNumber
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.UserName != user.UserName)
            {
                user.UserName = Input.UserName;
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.City != user.City)
            {
                user.City = Input.City;
            }

            if (Input.State != user.State)
            {
                user.State = Input.State;
            }

            if (Input.ZipCode != user.ZipCode)
            {
                user.ZipCode = Input.ZipCode;
            }

            if (Input.BirthDate != user.BirthDate)
            {
                user.BirthDate = Input.BirthDate;
            }

            if (Input.Age != user.Age)
            {
                user.Age = Input.Age;
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            /*****************************************************************
            
            Below is an Example of how I'm changing a user's picture in another app.

            I have a blob storage account in Azure that I'm uploading the image to
            and then saving the path to that image in the database.

            In order for this to work you'd have to inject IConfiguration and store
            your credentials in Azure. There's other ways of doing this, but I wanted
            to share how I'm doing it for those looking to learn. Currently the app 
            will allow you to click and change the user's picture from the interface
            but nothing will be saved unless you have something like this.

            *****************************************************************/

            // Start of image upload code block

            // var account = _configuration["AzureStorageConfig:AccountName"];
            // var key = _configuration["AzureStorageConfig:AccountKey"];
            // var storageCredentials = new StorageCredentials(account, key);
            // var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            // var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            // var container = cloudBlobClient.GetContainerReference("images");
            // await container.CreateIfNotExistsAsync();

            // if (Input.Picture != user.Picture)
            // {
            //     var files = HttpContext.Request.Form.Files;

            //     if (files.Count != 0) 
            //     {
            //         var extension = Path.GetExtension(files[0].FileName);
            //         var newBlob = container.GetBlockBlobReference("User-" + user.Id + extension);

            //         using (var filestream = new MemoryStream())
            //         {
            //             files[0].CopyTo(filestream);
            //             filestream.Position = 0;
            //             await newBlob.UploadFromStreamAsync(filestream);
            //         }
            //         user.Picture = "whatever-url-to-your-blob/User-" + user.Id + extension;
            //     }
            // }
            //
            // End of image upload code block

            //await _signInManager.RefreshSignInAsync(user);
            await _userManager.UpdateAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            // await _emailSender.SendEmailAsync(
            //     email,
            //     "Confirm your email",
            //     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
