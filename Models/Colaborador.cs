namespace TunicoAniversarioAdmissao.Models
{
    public class Colaborador
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime DataAdmissao { get; set; }
        public int AnosNaEmpresa { get; set; }
    }
}
