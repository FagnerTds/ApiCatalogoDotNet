namespace ApiCatalogo.Services
{
    public class MeuServico : IMeuServico
    {
        public string Saudacao(string nome)
        {
            return $"Olá {nome} hoje é {DateTime.UtcNow}";
        }
    }
}
