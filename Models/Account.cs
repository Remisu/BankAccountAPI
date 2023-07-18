namespace BankAccountAPI.Models
{
    public class Conta
    {
        public string NumeroConta { get; set; }
        public string TipoConta { get; set; }
        public decimal Saldo { get; set; }
        public string Titular { get; set; }
        public DateTime DataAbertura { get; set; }
        public List<string> HistoricoTransacoes { get; set; }
        public string StatusConta { get; set; }
    }
}
