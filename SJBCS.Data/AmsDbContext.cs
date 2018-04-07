namespace SJBCS.Data
{
    public class AmsDbContext : AmsModel
    {
        public AmsModel GetConnection()
        {
            return ConnectionHelper.CreateConnection();
        }

    }
}
