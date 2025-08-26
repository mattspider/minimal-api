using minimal_api.domain.entities;
using minimal_api.domain.Interfaces;
using minimal_api.infraestructure.Data;
using minimal_api.infraestructure.Interfaces;

namespace minimal_api.infraestructure.services
{
    public class VeiculoService : IVeiculo
    {
        
        private readonly DBContext _context;

        public VeiculoService(DBContext context)
        {
            _context = context;
        }

        public List<Veiculo> FiltrarVeiculos(int? pagina = 1, string? nome = null, string? marca = null, string? ano = null)
        {
            var query = _context.Veiculos.AsQueryable();

            query = (nome, marca, ano) switch
            {
                var t when !string.IsNullOrWhiteSpace(t.nome) 
                    => query.OrderBy(v => v.Nome),

                var t when !string.IsNullOrWhiteSpace(t.marca) 
                    => query.OrderBy(v => v.Marca),

                var t when !string.IsNullOrWhiteSpace(t.ano) 
                    => query.OrderBy(v => v.Ano),

                _ => query.OrderBy(v => v.Nome) // fallback
            };

            if (pagina == null || pagina <= 0)
            {
                pagina = 1;
            }
            int pageSize = 10;
            return query.Skip(((int)pagina - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public Veiculo ExibirVeiculoPorId(int id)
        {
            var veiculo = _context.Veiculos.Where(v => v.Id == id).FirstOrDefault();
            if (veiculo == null)
            {
                throw new KeyNotFoundException($"Veiculo with id {id} not found.");
            }
            return veiculo;
        }

        public void CriarVeiculo(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();
        }

        public void AtualizarVeiculo(Veiculo veiculo)
        {
            if (veiculo == null)
            {
                throw new ArgumentNullException(nameof(veiculo), "Veiculo nao pode ser nulo.");
            }

            _context.Veiculos.Update(veiculo);
            _context.SaveChanges();
        }

        public void DeletarVeiculo(Veiculo veiculo)
        {
            _context.Veiculos.Remove(veiculo);
            _context.SaveChanges();
        }

    }
    
}