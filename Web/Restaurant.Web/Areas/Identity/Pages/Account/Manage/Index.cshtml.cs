namespace Restaurant.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Restaurant.Data.Models;
    using Restaurant.Services.Data;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IWebHostEnvironment environment;
        private readonly IUserImageService userImageService;
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment,
            IUserImageService userImageService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.environment = environment;
            this.userImageService = userImageService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string ImageUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Display(Name = "Profile picture")]
            public IFormFile Image { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            var firstName = this.Input.FirstName;

            if (firstName != user.FirstName)
            {
               user.FirstName = firstName;
            }

            var lastName = this.Input.LastName;

            if (lastName != user.LastName)
            {
                user.LastName = lastName;
            }

            if (this.Username != this.Input.UserName)
            {
                user.UserName = this.Input.UserName;
            }

            var userImage = this.userManager.Users.Include(x => x.UserImage).FirstOrDefault(x => x.Id == user.Id).UserImage;

            if (userImage is not null)
            {
                await this.userImageService.DeleteAsync(userImage);
            }

            Directory.CreateDirectory($"{this.environment.WebRootPath}/images/users/");

            var extension = Path.GetExtension(this.Input.Image.FileName).TrimStart('.');

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid image extension {extension}");
            }

            var dbImage = new UserImage
            {
                Extension = extension,
            };

            user.UserImage = dbImage;

            var physicalPath = $"{this.environment.WebRootPath}/images/users/{dbImage.Id}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await this.Input.Image.CopyToAsync(fileStream);

            await this.userManager.UpdateAsync(user);
            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.Username = userName;
            var userImage = this.userManager.Users.Include(x => x.UserImage).FirstOrDefault(x => x.Id == user.Id).UserImage;

            if (userImage is not null)
            {
                this.ImageUrl = $"/images/users/{userImage.Id}.{userImage.Extension}";
            }

            this.Input = new InputModel
            {
                UserName = this.Username,
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }
}
