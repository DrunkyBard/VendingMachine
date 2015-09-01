namespace VendingMachineApp.DataAccess.Entities
{
    public class UserWalletEntity
    {
        public int Id { get; set; }

        public decimal FaceValue { get; set; }

        public int Count { get; set; }

        public UserEntity User { get; set; }

        public int UserId { get; set; }
    }
}
