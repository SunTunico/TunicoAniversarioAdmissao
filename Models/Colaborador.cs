namespace TunicoAniversarioAdmissao.Models
{
    public class Colaborador
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public DateTime admission_date { get; set; }
        public int years_on_company { get; set; }
    }
}
