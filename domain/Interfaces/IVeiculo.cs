
using minimal_api.domain.entities;

namespace minimal_api.domain.Interfaces
{
    public interface IVeiculo
    {
        List<Veiculo> FiltrarVeiculos(int? pagina = 1, string? nome = null, string? marca = null, string? ano = null);
        Veiculo? ExibirVeiculoPorId(int id);
        void CriarVeiculo(Veiculo veiculo);
        void AtualizarVeiculo(Veiculo veiculo);
        void DeletarVeiculo(Veiculo veiculo);

    }
}