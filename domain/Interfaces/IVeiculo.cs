
using minimal_api.domain.entities;

namespace minimal_api.domain.Interfaces
{
    public interface IVeiculo
    {
        List<Veiculo> ExibirVeiculos();
        List<Veiculo> FiltrarVeiculos(int pageNumber, string? nome, string? marca, string? ano);
        Veiculo? ExibirVeiculoPorId(int id);
        void CriarVeiculo(Veiculo veiculo);
        void AtualizarVeiculo(Veiculo veiculo);
        void DeletarVeiculo(Veiculo veiculo);

    }
}