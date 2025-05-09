using Microsoft.EntityFrameworkCore;

namespace kubernets.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }  // Mapeando a tabela Devices

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Substitua o caminho conforme necessário para a sua base de dados
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Documentos\\Universidade\\LTI\\LTI-TL2\\LTI_TL2\\kubernets\\kubernets\\Data\\Kubernetes.mdf;Integrated Security=True;Connect Timeout=30");
        }
    }
}
