namespace SJBCS.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class AmsModel
    {
        public AmsModel(string connectionString)
             : base(connectionString)
        {

        }
    }
}