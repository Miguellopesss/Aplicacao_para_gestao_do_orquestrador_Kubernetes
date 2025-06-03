using Microsoft.EntityFrameworkCore;

namespace kubernets.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Table> Table { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //ALTERAR O CAMINHO DA BASE DE DADOS AQUI
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\joaom\\Documents\\GitHub\\LTI_TL2\\LTI_TL2_JoaoTendeiro_MiguelLopes\\kubernets\\Data\\Kubernetes.mdf;Integrated Security=True");
        }
    }
}
