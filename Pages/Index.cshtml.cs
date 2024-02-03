using AS_Assignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace AS_Assignment.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        
        private readonly IDataProtectionProvider dataProtectionProvider;

        private readonly IOptions<SessionOptions> sessionOptions;

        public IndexModel(
           ILogger<IndexModel> logger,
           UserManager<ApplicationUser> userManager,
        
           IDataProtectionProvider dataProtectionProvider, 
           IOptions<SessionOptions> sessionOptions)
        {
            _logger = logger;
            this.userManager = userManager;
            this.dataProtectionProvider = dataProtectionProvider;
            this.sessionOptions = sessionOptions;
        }

        public ApplicationUser UserInfo { get; set; }

        public void OnGet()
        {
            var user = userManager.GetUserAsync(User).Result;

            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("FullName", user.FullName);

            UserInfo = user;

  
            var encryptedFirstName = user.FullName;
            var unprotectedFirstName = UnprotectData(encryptedFirstName);

            var encryptedCreditCard = user.CreditCardNo;
            var unprotectedCreditCard = UnprotectData(encryptedCreditCard);
     
            ViewData["DecryptedFullName"] = unprotectedFirstName;
            ViewData["DecryptedCreditCard"] = unprotectedCreditCard;
        }

        private string UnprotectData(string protectedData)
        {
            var protector = dataProtectionProvider.CreateProtector("YourPurpose");
            return protector.Unprotect(protectedData);
        }
    }

}