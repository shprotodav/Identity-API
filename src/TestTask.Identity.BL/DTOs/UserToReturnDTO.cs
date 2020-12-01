using System;

namespace TestTask.Identity.BL.DTOs
{
    public class UserToReturnDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }

    public class UserDataDTO
    {
        public string Token { get; set; }
        public DateTime ExpirationUTC { get; set; }
        public UserToReturnDTO UserToReturnDTO { get; set; }
    }
}
