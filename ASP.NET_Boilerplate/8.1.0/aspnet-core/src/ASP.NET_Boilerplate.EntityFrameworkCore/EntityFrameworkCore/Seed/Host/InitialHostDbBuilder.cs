namespace ASP.NET_Boilerplate.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly NET_BoilerplateDbContext _context;

        public InitialHostDbBuilder(NET_BoilerplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
