using Microsoft.AspNetCore.Identity;
using System;

namespace WhyApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string Age { get; set; }
        public string Picture { get; set; }
    }
}
