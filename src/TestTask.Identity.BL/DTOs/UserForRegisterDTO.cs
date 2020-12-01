using System;

namespace TestTask.Identity.BL.DTOs
{
    public class UserForRegisterDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
    }
}
