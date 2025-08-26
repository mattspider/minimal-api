namespace minimal_api.domain.DTO
{
    public record VeiculoDto
    {

        public int Id { get; set; } = default;

        public string Nome { get; set; } = default;

        public string Marca { get; set; } = default;

        public int Ano { get; set; } = default;
    }
}